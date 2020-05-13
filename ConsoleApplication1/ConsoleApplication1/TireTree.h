#pragma once
#include <string>
#include <set>

using namespace std;

struct TireTree
{
public:
	int count = 0;
	TireTree * child[26];

	void insert(string s)
	{
		TireTree * tmp = this;
		for (int i = 0;i < s.size();i++)
		{
			int index = s.at(i) - 'a';
			if (tmp->child[index] == NULL)
			{
				tmp->child[index] = new TireTree();
				tmp->child[index]->count = 1;

			}
			else
			{
				tmp->child[index]->count++;
			}
			tmp = tmp->child[index];

		}
	}
	TireTree * searchNode(string s)
	{
		TireTree * tmp = this;
		for (int i = 0;i < s.size();i++)
		{
			int index = s.at(i) - 'a';
			if (tmp->child[index] == NULL) return NULL;
			tmp = tmp->child[index];

		}
		return tmp;
	}
	void getLeftPossibleString(string current, int count, TireTree *node, set<string> & left)
	{
		bool isend = false;
		for (int i = 0;i < 26;i++)
		{
			if (node->child[i] != NULL)
			{
				if (node->child[i]->count < count)
				{
					if (left.find(current) == left.end()) left.insert(current);
				}
				string curr(current);
				curr.push_back(i + 'a');
				getLeftPossibleString(curr, node->child[i]->count, node->child[i], left);
				isend = true;
			}
		}
		if (!isend)
		{
			if (left.find(current) == left.end()) left.insert(current);
		}
	}
	bool search(string s)
	{
		TireTree * tmp = this;
		for (int i = 0;i < s.size();i++)
		{
			int index = s.at(i) - 'a';
			if (tmp->child[index] == NULL) return false;
			tmp = tmp->child[index];

		}
		return true;
	}
	TireTree()
	{
		for (int i = 0;i < 26;i++)
		{
			child[i] = NULL;
		}
	}
	~TireTree()
	{
	}
};