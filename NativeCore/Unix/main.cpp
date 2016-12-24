#include <iostream>
#include <boost/filesystem.hpp>
#include <sys/uio.h>
#include <sstream>
#include <sys/types.h>
#include <signal.h>
#include <sys/mman.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>

bool is_number(const std::string& s)
{
    std::string::const_iterator it = s.begin();
    while (it != s.end() && std::isdigit(*it)) ++it;
    return !s.empty() && it == s.end();
}

using RC_Pointer = void*;

extern "C" RC_Pointer OpenRemoteProcess(RC_Pointer id, int desiredAccess)
{
	return id;
}

extern "C" bool IsProcessValid(RC_Pointer handle)
{
	return kill((pid_t)(intptr_t)handle, 0) == 0;
}

extern "C" bool ReadRemoteMemory(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int offset, int size)
{
	iovec local[1];
	iovec remote[1];

	local[0].iov_base = ((uint8_t*)buffer + offset);
	local[0].iov_len = size;
	remote[0].iov_base = address;
	remote[0].iov_len = size;

	if (process_vm_readv((pid_t)(intptr_t)handle, local, 1, remote, 1, 0) != size)
	{
		return false;
	}

	return true;
}

extern "C" bool WriteRemoteMemory(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int offset, int size)
{
	iovec local[1];
	iovec remote[1];

	local[0].iov_base = ((uint8_t*)buffer + offset);
	local[0].iov_len = size;
	remote[0].iov_base = address;
	remote[0].iov_len = size;

	if (process_vm_writev((pid_t)(intptr_t)handle, local, 1, remote, 1, 0) != size)
	{
		return false;
	}

	return true;
}

struct map_iterator
{
	off_t offset;
	int fd;
	size_t buf_size;
	char *buf;
	char *buf_end;
	char *path;
};

static inline char* ltoa(char *buf, long val)
{
	char *cp = buf, tmp;
	ssize_t i, len;

	do
	{
		*cp++ = '0' + (val % 10);
		val /= 10;
	} while (val);

	/* reverse the order of the digits: */
	len = cp - buf;
	--cp;
	for (i = 0; i < len / 2; ++i)
	{
		tmp = buf[i];
		buf[i] = cp[-i];
		cp[-i] = tmp;
	}
	return buf + len;
}

static inline int maps_init(struct map_iterator *mi, pid_t pid)
{
	char path[sizeof("/proc/0123456789/maps")], *cp;

	memcpy(path, "/proc/", 6);
	cp = ltoa(path + 6, pid);
	memcpy(cp, "/maps", 6);

	std::stringstream ss;
	ss << "/proc/" << pid << "/maps";

	mi->fd = open(ss.str().c_str(), O_RDONLY);
	if (mi->fd >= 0)
	{
		mi->buf_size = getpagesize();
		cp = (char*)mmap(nullptr, mi->buf_size, PROT_READ | PROT_WRITE, MAP_PRIVATE | MAP_ANONYMOUS, -1, 0);
		if (cp == MAP_FAILED)
		{
			close(mi->fd);
			mi->fd = -1;
			return -1;
		}
		else
		{
			mi->offset = 0;
			mi->buf = mi->buf_end = cp + mi->buf_size;
			return 0;
		}
	}
	return -1;
}

static inline char* skip_whitespace(char *cp)
{
	if (!cp)
		return NULL;

	while (*cp == ' ' || *cp == '\t')
		++cp;
	return cp;
}

static inline char* scan_hex(char *cp, unsigned long *valp)
{
	unsigned long num_digits = 0, digit, val = 0;

	cp = skip_whitespace(cp);
	if (!cp)
		return NULL;

	while (1)
	{
		digit = *cp;
		if ((digit - '0') <= 9)
			digit -= '0';
		else if ((digit - 'a') < 6)
			digit -= 'a' - 10;
		else if ((digit - 'A') < 6)
			digit -= 'A' - 10;
		else
			break;
		val = (val << 4) | digit;
		++num_digits;
		++cp;
	}
	if (!num_digits)
		return NULL;
	*valp = val;
	return cp;
}

static inline char* scan_dec(char *cp, unsigned long *valp)
{
	unsigned long num_digits = 0, digit, val = 0;

	if (!(cp = skip_whitespace(cp)))
		return NULL;

	while (1)
	{
		digit = *cp;
		if ((digit - '0') <= 9)
		{
			digit -= '0';
			++cp;
		}
		else
			break;
		val = (10 * val) + digit;
		++num_digits;
	}
	if (!num_digits)
		return NULL;
	*valp = val;
	return cp;
}

static inline char* scan_char(char *cp, char *valp)
{
	if (!cp)
		return NULL;

	*valp = *cp;

	/* don't step over NUL terminator */
	if (*cp)
		++cp;
	return cp;
}

static inline char* scan_string(char *cp, char *valp, size_t buf_size)
{
	size_t i = 0;

	if (!(cp = skip_whitespace(cp)))
		return NULL;

	while (*cp != ' ' && *cp != '\t' && *cp != '\0')
	{
		if ((valp != NULL) && (i < buf_size - 1))
			valp[i++] = *cp;
		++cp;
	}
	if (i == 0 || i >= buf_size)
		return NULL;
	valp[i] = '\0';
	return cp;
}

static inline int maps_next(struct map_iterator *mi, unsigned long *low, unsigned long *high, unsigned long *offset)
{
	char perm[16], dash = 0, colon = 0, *cp;
	unsigned long major, minor, inum;
	ssize_t i, nread;

	if (mi->fd < 0)
		return 0;

	while (true)
	{
		ssize_t bytes_left = mi->buf_end - mi->buf;
		char *eol = NULL;

		for (i = 0; i < bytes_left; ++i)
		{
			if (mi->buf[i] == '\n')
			{
				eol = mi->buf + i;
				break;
			}
			else if (mi->buf[i] == '\0')
				break;
		}
		if (!eol)
		{
			/* copy down the remaining bytes, if any */
			if (bytes_left > 0)
				memmove(mi->buf_end - mi->buf_size, mi->buf, bytes_left);

			mi->buf = mi->buf_end - mi->buf_size;
			nread = read(mi->fd, mi->buf + bytes_left,
				mi->buf_size - bytes_left);
			if (nread <= 0)
				return 0;
			else if ((size_t)(nread + bytes_left) < mi->buf_size)
			{
				/* Move contents to the end of the buffer so we
				maintain the invariant that all bytes between
				mi->buf and mi->buf_end are valid.  */
				memmove(mi->buf_end - nread - bytes_left, mi->buf,
					nread + bytes_left);
				mi->buf = mi->buf_end - nread - bytes_left;
			}

			eol = mi->buf + bytes_left + nread - 1;

			for (i = bytes_left; i < bytes_left + nread; ++i)
				if (mi->buf[i] == '\n')
				{
					eol = mi->buf + i;
					break;
				}
		}
		cp = mi->buf;
		mi->buf = eol + 1;
		*eol = '\0';

		/* scan: "LOW-HIGH PERM OFFSET MAJOR:MINOR INUM PATH" */
		cp = scan_hex(cp, low);
		cp = scan_char(cp, &dash);
		cp = scan_hex(cp, high);
		cp = scan_string(cp, perm, sizeof(perm));
		cp = scan_hex(cp, offset);
		cp = scan_hex(cp, &major);
		cp = scan_char(cp, &colon);
		cp = scan_hex(cp, &minor);
		cp = scan_dec(cp, &inum);
		cp = mi->path = skip_whitespace(cp);
		if (!cp)
			continue;
		cp = scan_string(cp, NULL, 0);
		if (dash != '-' || colon != ':')
			continue;       /* skip line with unknown or bad format */
		return 1;
	}
	return 0;
}

static inline void maps_close(struct map_iterator *mi)
{
	if (mi->fd < 0)
		return;
	close(mi->fd);
	mi->fd = -1;
	if (mi->buf)
	{
		munmap(mi->buf_end - mi->buf_size, mi->buf_size);
		mi->buf = mi->buf_end = NULL;
	}
}

using EnumerateRemoteSectionData = void;
using EnumerateRemoteModuleData = void;

typedef void(EnumerateRemoteSectionsCallback)(EnumerateRemoteSectionData* data);
typedef void(EnumerateRemoteModulesCallback)(EnumerateRemoteModuleData* data);

extern "C" void EnumerateRemoteSectionsAndModules(RC_Pointer handle, EnumerateRemoteSectionsCallback callbackSection, EnumerateRemoteModulesCallback callbackModule)
{
	if (callbackSection == nullptr && callbackModule == nullptr)
	{
		return;
	}

	struct map_iterator mi;
	unsigned long start, end, offset, flags;
	struct map_info *map_list = NULL;
	struct map_info *cur_map;
	if (maps_init(&mi, (pid_t)(intptr_t)handle) < 0)
		return;
	while (maps_next(&mi, &start, &end, &offset, &flags))
	{
		cur_map = (struct map_info *)malloc(sizeof(struct map_info));
		if (cur_map == NULL)
			break;
		cur_map->next = map_list;
		cur_map->start = start;
		cur_map->end = end;
		cur_map->offset = offset;
		cur_map->flags = flags;
		cur_map->path = strdup(mi.path);
		cur_map->ei.size = 0;
		cur_map->ei.image = NULL;
		map_list = cur_map;
	}
	maps_close(&mi);
}

template<typename T>
T parse_type(const std::string& s)
{
    std::stringstream ss(s);

    T val;
    ss >> val;
    return val;
}

int main()
{
    using namespace boost::filesystem;

    path proc("/proc");

    for (auto& p : directory_iterator(proc))
    {
        if (is_directory(p))
        {
            auto name = p.path().filename().string();
            if (is_number(name))
            {
                size_t pid = parse_type<size_t>(name);

                auto pa = p.path() / "exe";

                if (is_symlink(symlink_status(pa)))
                {
                    try
                    {
                        auto e = read_symlink(pa);

                        std::cout << "(" << pid << ", " << e.string() << ")" << std::endl;
                    }
                    catch(...)
                    {
                        std::cout << "Exception: " << std::endl;
                    }
                }
            }
        }
    }

    return 0;
}
