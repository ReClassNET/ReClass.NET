//#include <experimental/filesystem>
#include <boost/filesystem.hpp>
#include <sstream>
#include <fstream>

#include "NativeCore.hpp"

bool is_number(const std::string& s)
{
	std::string::const_iterator it = s.begin();
	while (it != s.end() && std::isdigit(*it)) ++it;
	return !s.empty() && it == s.end();
}

template<typename T>
T parse_type(const std::string& s)
{
	std::stringstream ss(s);

	T val;
	ss >> val;
	return val;
}

enum class Platform
{
	Unknown,
	X86,
	X64
};

Platform GetProcessPlatform(const std::string& auxvPath)
{
	auto platform = Platform::Unknown;

	std::ifstream file(auxvPath);
	if (file)
	{
		char buffer[16];
		while (true)
		{
			file.read(buffer, 16);

			if (!file)
			{
				return Platform::X64;
			}

			if (buffer[4] != 0 || buffer[5] != 0 || buffer[6] != 0 || buffer[7] != 0)
			{
				return Platform::X86;
			}
		}
	}

	return platform;
}

extern "C" void EnumerateProcesses(EnumerateProcessCallback callbackProcess)
{
	//using namespace std::experimental::filesystem;
	//using namespace std;

	using namespace boost::filesystem;
	using namespace boost::system;

	if (callbackProcess == nullptr)
	{
		return;
	}

	path proc("/proc");

	if (is_directory(proc))
	{
		for (auto& p : directory_iterator(proc))
		{
			if (is_directory(p))
			{
				auto processPath = p.path();

				auto name = processPath.filename().string();
				if (is_number(name))
				{
					size_t pid = parse_type<size_t>(name);

					auto exeSymLink = processPath / "exe";
					if (is_symlink(symlink_status(exeSymLink)))
					{
						error_code ec;
						auto e = read_symlink(exeSymLink, ec);

						if (!ec)
						{
							auto auxvPath = processPath / "auxv";
							
							auto platform = GetProcessPlatform(auxvPath.string());
#ifdef NATIVE_CORE_64
							if (platform == Platform::X64)
#else
							if (platform == Platform::X86)
#endif
							{
								EnumerateProcessData data = {};
								data.Id = pid;
								MultiByteToUnicode(e.string().c_str(), data.ModulePath, PATH_MAXIMUM_LENGTH);

								callbackProcess(&data);
							}
						}
					}
				}
			}
		}
	}
}
