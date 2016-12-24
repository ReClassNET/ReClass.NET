//#include <experimental/filesystem>
#include <boost/filesystem.hpp>
#include <sstream>

#include "NativeCore.hpp"

//using fs = std::experimental::filesystem;
//using sys = std;
/*using fs = boost::filesystem;
using sys = boost::system;*/

enum class Platform
{
	Unknown,
	X86,
	X64
};

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
							//auto elfHeader = processPath / "";

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
