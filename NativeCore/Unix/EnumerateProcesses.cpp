#include <iostream>
#include <string>
#include <sstream>
#include <fstream>

#include "NativeCore.hpp"

// std::filesystem library doesn't work @Ubuntu 16.10, read_symlink() always fails.
//#define USE_STD_FILESYSTEM
#ifdef USE_STD_FILESYSTEM
#include <experimental/filesystem>
#else
#include <dirent.h>
#include <sys/stat.h>
#include <unistd.h>

class path
{
public:
	path() = default;

	path(const char* _path)
		: path(std::string(_path))
	{
	}

	path(const std::string& _path)
		: buf(_path)
	{
	}

	path(const path&) = default;

	path& append(const std::string& part)
	{
		if (buf.back() != separator)
		{
			buf += separator;
		}
		buf += part;

		return *this;
	}

	path& operator/=(const path& p)
	{
		append(p.buf);

		return *this;
	}

	const std::string& string() const
	{
		return buf;
	}

	const char* c_str() const
	{
		return buf.c_str();
	}

private:
	static const char separator = '/';

	std::string buf;
};

inline path operator/(const path& lhs, const path& rhs)
{
	return path(lhs) /= rhs;
}

enum class FileType
{
	Unknown,

	File,
	Directory,
	Symlink
};

FileType file_type(const path& p)
{
	struct stat path_stat = {};
	if (::lstat(p.c_str(), &path_stat) == 0)
	{
		if (S_ISREG(path_stat.st_mode))
		{
			return FileType::File;
		}
		else if (S_ISDIR(path_stat.st_mode))
		{
			return FileType::Directory;
		}
		else if (S_ISLNK(path_stat.st_mode))
		{
			return FileType::Symlink;
		}
	}

	return FileType::Unknown;
}

inline bool is_directory(const path& p)
{
	return file_type(p) == FileType::Directory;
}

inline bool is_symlink(const path& p)
{
	return file_type(p) == FileType::Symlink;
}

bool read_symlink(const path& p, path& out_p)
{
	std::string temp(64, '\0');

	for (;; temp.resize(temp.size() * 2))
	{
		ssize_t result;
		if ((result = ::readlink(p.c_str(), /*temp.data()*/ &temp[0], temp.size())) == -1)
		{
			return false;
		}
		else
		{
			if (result != (ssize_t)temp.size())
			{
				out_p = path(std::string(temp.begin(), temp.begin() + result));

				return true;
			}
		}
	}
}

#endif

bool is_number(const std::string& s)
{
	auto it = s.begin();
	for (; it != s.end() && std::isdigit(*it); ++it);
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
	if (callbackProcess == nullptr)
	{
		return;
	}

#ifdef USE_STD_FILESYSTEM
	using namespace std::experimental::filesystem;
	using namespace std;

	path procPath("/proc");
	if (is_directory(procPath))
	{
		for (auto& d : directory_iterator(procPath))
		{
			if (is_directory(d))
			{
				auto pidPath = d.path();

				auto name = processPath.filename().string();
				if (is_number(name))
				{
					auto exeSymLink = processPath / "exe";
					if (is_symlink(symlink_status(exeSymLink)))
					{
						error_code ec;
						auto linkPath = read_symlink(exeSymLink, ec).string();
						if (!ec)
						{
#else
	path procPath("/proc");
	if (is_directory(procPath))
	{
		auto directory = opendir(procPath.c_str());
		if (directory == nullptr)
		{
			return;
		}

		struct dirent *entry;
		while ((entry = readdir(directory)) != nullptr)
		{
			auto pidPath = procPath / entry->d_name;
			if (is_directory(pidPath))
			{
				std::string name(entry->d_name);
				if (is_number(name))
				{
					auto exePath = pidPath / "exe";
					if (is_symlink(exePath))
					{
						path linkPath;
						if (read_symlink(exePath, linkPath))
						{
#endif
							auto auxvPath = pidPath / "auxv";
							
							auto platform = GetProcessPlatform(auxvPath.string());
#ifdef NATIVE_CORE_64
							if (platform == Platform::X64)
#else
							if (platform == Platform::X86)
#endif
							{
								EnumerateProcessData data = {};
								data.Id = parse_type<size_t>(name);
								MultiByteToUnicode(linkPath.c_str(), data.ModulePath, PATH_MAXIMUM_LENGTH);

								callbackProcess(&data);
							}
						}
					}
				}
			}
		}
	}
}
