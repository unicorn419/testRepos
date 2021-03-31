#include "pch.h"
#include "FindSubStrings.h"
#include <map>
#include <set>

FindSubStrings::FindSubStrings()
{
}


FindSubStrings::~FindSubStrings()
{
}

vector<string> FindSubStrings::Find(string S)
{
	vector<string> res;
	
	map<char, vector<int>> m;
	set<char> s;

	for (int i = 0;i < S.length();i++)
	{
		m[S.at(i)].push_back(i);
	}
	string tmp("");
	
	for (auto iter = m.begin();iter != m.end();iter++)
	{
		if (iter->second.size() == 1)
		{
			tmp.push_back(iter->first);
			res.push_back(tmp);
			tmp.clear();
		}
		else
		{
			int prevIndex = iter->second[0];
			bool bContinue = true;
			for (int i = 1;i < iter->second.size();i++)
			{
				if (iter->second[i] - prevIndex > 1) { bContinue = false;break; }
					prevIndex = iter->second[i];
			}
			if (!bContinue) continue;
			else {
				tmp.append(iter->second.size(), iter->first);
				res.push_back(tmp);
				tmp.clear();
			}
		}
	}
	return res;
}

bool FindSubStrings::FindRepeatSubString(string s)
{
	map<char, vector<int>> m;
	//set<char> s;

	for (int i = 0;i < s.length();i++)
	{
		m[s.at(i)].push_back(i);
	}
	string tmp("");

	for (auto iter = m.begin();iter != m.end();iter++)
	{
		if (iter->second.size() == 1)
		{
			//it is not the start charactor
			continue;
		}
		else
		{
			
		}
	}
	return 0;
}
