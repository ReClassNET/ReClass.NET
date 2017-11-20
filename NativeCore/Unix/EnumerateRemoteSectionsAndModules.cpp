#include <fstream>
#include <sstream>
#include <unordered_map>

#include "NativeCore.hpp"

inline bool operator&(SectionProtection& lhs, SectionProtection rhs)
{
	using T = std::underlying_type_t<SectionProtection>;

	return (static_cast<T>(lhs) & static_cast<T>(rhs)) == static_cast<T>(rhs);
}

template<typename T>
inline std::istream& skip(std::istream& s)
{
	auto f = s.flags();
	s >> std::noskipws;

	T t;
	s >> t;

	s.flags(f);

	return s;
}

std::istream& operator >> (std::istream& s, SectionProtection& protection)
{
	protection = SectionProtection::NoAccess;

	if (s.get() == 'r') protection |= SectionProtection::Read;
	if (s.get() == 'w') protection |= SectionProtection::Write;
	if (s.get() == 'x') protection |= SectionProtection::Execute;

	return s;
}

extern "C" void RC_CallConv EnumerateRemoteSectionsAndModules(RC_Pointer handle, EnumerateRemoteSectionsCallback callbackSection, EnumerateRemoteModulesCallback callbackModule)
{
	if (callbackSection == nullptr && callbackModule == nullptr)
	{
		return;
	}

	struct ModuleInfo
	{
		intptr_t Start = 0;
		intptr_t End = 0;
		RC_UnicodeChar Path[PATH_MAXIMUM_LENGTH] = {};
	};

	std::ifstream input(static_cast<std::stringstream&>(std::stringstream() << "/proc/" << reinterpret_cast<intptr_t>(handle) << "/maps").str());

	std::unordered_map<int, ModuleInfo> modules;

	std::string line;
	while (std::getline(input, line))
	{
		std::stringstream ss(line);

		intptr_t start;
		intptr_t end;
		SectionProtection protection;
		intptr_t offset;
		int dev1, dev2;
		int inode;
		std::string path;
		ss >> std::hex >> start >> skip<char> >> end >> skip<char> >> protection >> skip<char> >> offset >> dev1 >> skip<char> >> dev2 >> std::dec >> inode >> std::skipws >> path;

		EnumerateRemoteSectionData section = {};
		section.BaseAddress = reinterpret_cast<RC_Pointer>(start);
		section.Size = end - start;
		section.Protection = protection;

		section.Category = SectionCategory::Unknown;
		section.Type = SectionType::Unknown;
		if (inode != 0)
		{
			section.Type = SectionType::Image;

			if (protection & SectionProtection::Read && protection & SectionProtection::Execute)
			{
				section.Category = SectionCategory::CODE;
			}
			else if (protection & SectionProtection::Read && protection & SectionProtection::Write)
			{
				section.Category = SectionCategory::DATA;
			}

			MultiByteToUnicode(path.c_str(), section.ModulePath, PATH_MAXIMUM_LENGTH);

			auto& module = modules[inode];
			module.Start = module.Start != 0 ? std::min(module.Start, start) : start;
			module.End = module.End != 0 ? std::max(module.End, end) : end;
			if (module.Path[0] == 0)
			{
				std::memcpy(module.Path, section.ModulePath, PATH_MAXIMUM_LENGTH);
			}
		}
		else
		{
			section.Type = SectionType::Mapped;

			if (protection & SectionProtection::Read || protection & SectionProtection::Write)
			{
				section.Category = SectionCategory::HEAP;
			}
		}

		if (callbackSection != nullptr)
		{
			callbackSection(&section);
		}
	}

	if (callbackModule != nullptr)
	{
		for (auto&& kv : modules)
		{
			EnumerateRemoteModuleData module = {};
			module.BaseAddress = reinterpret_cast<RC_Pointer>(kv.second.Start);
			module.Size = kv.second.End - kv.second.Start;
			std::memcpy(module.Path, kv.second.Path, PATH_MAXIMUM_LENGTH);

			callbackModule(&module);
		}
	}
}
