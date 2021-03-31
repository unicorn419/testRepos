#include "pch.h"
#include <vector>
#include "L001_TwoSum.h"
#include <map>


L001_TwoSum::L001_TwoSum()
{
}


L001_TwoSum::~L001_TwoSum()
{
}

vector<int> L001_TwoSum::twoSum(vector<int>& nums, int target)
{
	vector<int> res;
	map<int, int> _map;
	for (int i = 0;i < nums.size();++i)
	{
		int diff = target - nums[i];
		if (_map.find(diff) != _map.end())
		{
			res.push_back(_map[diff]);
			res.push_back(i);
		}
		else
		{
			_map[nums[i]] = i;
		}
	}
	return res;
}

void L001_TwoSum::Test()
{
	vector<int> input{ 2, 7, 11, 15 };
	vector<int> res = twoSum(input, 9);
	return;
}
