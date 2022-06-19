#pragma once

#include <unordered_map>
#include <string>
#include <vector>

template<typename T, typename J>
bool UMapExist(const std::unordered_map<T, J>& map, const T& key)
{
    return map.find(key) != map.end();
}

template<typename T, typename J>
void StoreUMap(const std::unordered_map<T, J>& uMap, std::vector<std::pair<T, J>>& outVec)
{
    for (const auto& kv : uMap)
        outVec.push_back(kv);
}