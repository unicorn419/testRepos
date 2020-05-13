#pragma once
#include <vector>
#include <map>
#include <set>
using namespace std;
class DataCenterConnect
{


public:
	DataCenterConnect();
	~DataCenterConnect();
	vector<vector<int>> FindKeyPath(int count,vector<vector<int>> &conns);
	vector<vector<int>> FindLowCostKeyPath(int count, vector<vector<int>> &conns,vector<vector<int>> &costs);
	bool MergeSearch(int count, vector<vector<int>> &conns);
private:
	int findComponent(int count, map<int, set<int>> &m);
	int findComponent(int count, int* set);
	vector<vector<int>> findIsolatedComponent(int count, map<int, set<int>> &m);
	int find(int x,int * set);
};

