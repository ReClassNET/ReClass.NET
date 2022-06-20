#include "Utils.h"

void StrtoWStr(const std::string& in, std::wstring& outWStr)
{
	std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>> converter;

	outWStr = converter.from_bytes(in);
}

std::wstring StrtoWStr(const std::string& in)
{
	std::wstring result = L"";

	StrtoWStr(in, result);

	return result;
}
