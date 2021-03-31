#pragma once
#include <vector>
#include <string>
using namespace std;
/*
找词的不重复子串
*/
class FindSubStrings
{
public:
	FindSubStrings();
	~FindSubStrings();
	vector<string> Find(string s);
	bool FindRepeatSubString(string s);
};

