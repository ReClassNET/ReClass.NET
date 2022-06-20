#pragma once

#include <codecvt>
#include <locale>

void StrtoWStr(const std::string& in, std::wstring& outWStr);

std::wstring StrtoWStr(const std::string& in);
