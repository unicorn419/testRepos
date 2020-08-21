#pragma once
#include <vector>
#include <map>
#include <set>

/*
图相关算法

数据中心有N个机器，[1,2] 代表机器1 可以连接2
1.找关键路径 FindKeyPath
2.找最小成本连接两个区域的路径 FindLowCostKeyPath
3. 查看是否连通 MergeSearch，合并查询算法

*/
using namespace std;
class DataCenterConnect
{


public:
	DataCenterConnect();
	~DataCenterConnect();
	//
	vector<vector<int>> FindKeyPath(int count,vector<vector<int>> &conns);
	vector<vector<int>> FindLowCostKeyPath(int count, vector<vector<int>> &conns,vector<vector<int>> &costs);
	bool MergeSearch(int count, vector<vector<int>> &conns);
private:
	//找图中独立不连通的个数
	int findComponent(int count, map<int, set<int>> &m);
	int findComponent(int count, int* set);

	//找图中不连通的集合
	vector<vector<int>> findIsolatedComponent(int count, map<int, set<int>> &m);
	//递归查找集合的代表元素，含路径压缩。但可能会导致栈溢出。我实现了两种算法
	int find(int x,int * set);
};

