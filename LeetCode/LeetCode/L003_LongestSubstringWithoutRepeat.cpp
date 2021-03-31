#include "pch.h"
#include "L003_LongestSubstringWithoutRepeat.h"

using namespace std;
L003_LongestSubstringWithoutRepeat::L003_LongestSubstringWithoutRepeat()
{
}


L003_LongestSubstringWithoutRepeat::~L003_LongestSubstringWithoutRepeat()
{
}

int L003_LongestSubstringWithoutRepeat::lengthOfLongestSubstring(string s)
{
	int res = 0;
	map<int, int> _map;
	int j = 0;
	for (int i = 0;i < s.length();++i)
	{
		if (_map.find(s[i]) != _map.end() && s[i] != -1)
		{

			for (;j <= _map[s[i]];++j)
				s[j] = -1;
		}
		_map[s[i]] = i;
		res = max(res, i - j + 1);
	}

	return res;

}
