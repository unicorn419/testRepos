#include "pch.h"
#include "DataCenterConnect.h"
#include <map>
#include <set>
#include <queue>

using namespace std;
DataCenterConnect::DataCenterConnect()
{
}


DataCenterConnect::~DataCenterConnect()
{
}

int DataCenterConnect::findComponent(int count, map<int,set<int>> &m)
{
	int * visited = new int[count];
	int res = 0;
	for (int i = 0;i < count;i++) visited[i] = 0;

	for (auto iter = m.begin();iter != m.end();iter++)
	{
		queue<int> q;
		if (!visited[iter->first - 1])
		{
			q.push(iter->first);

			while (!q.empty())
			{
				int node = q.front();
				visited[node - 1] = true;
				q.pop();

				for (auto it = m[node].begin();it != m[node].end();it++)
				{
					if (!visited[*it - 1])
						q.push(*it);
				}
			}
			res++;
		}
	}
	delete[] visited;
	return res;
}

int DataCenterConnect::findComponent(int count, int * set)
{
	return 0;
}

vector<vector<int>> DataCenterConnect::findIsolatedComponent(int count, map<int, set<int>> &m)
{
	int * visited = new int[count];
	vector<vector<int>> res ;
	int index = 0;
	for (int i = 0;i < count;i++) visited[i] = 0;

	for (int i=1;i<=count;i++)
	{
		if (!visited[i-1])
		{
			queue<int> q;
			q.push(i);

			while (!q.empty())
			{
				int node = q.front();
				q.pop();
				if (!visited[node - 1])
				{
					visited[node - 1] = true;
					if (index >= res.size()) res.push_back(vector<int>{node});
					else
						res[index].push_back(node);


					for (auto it = m[node].begin();it != m[node].end();it++)
					{
						if (!visited[*it - 1])
							q.push(*it);
					}
				}
			}
			index++;
		}
	}
	delete[] visited;
	return res;
}

int DataCenterConnect::find(int x,int* set)
{
	/* 循环算法，缺点无法进行路径压缩
	int tmp=x;
	while(tmp!=set[tmp])
	{
	    tmp=set[tmp];
		//set[tmp]=
	}
	//set[x]=tmp;
	return tmp;
	*/
	return x == set[x] ? x : (set[x] = find(set[x],set));   //递归查找集合的代表元素，含路径压缩。但可能会导致栈溢出
}

vector<vector<int>> DataCenterConnect::FindKeyPath(int count, vector<vector<int>> &conns)
{
	vector<vector<int>> res;
	map<int, set<int>> m;
	for (int i = 0;i < conns.size();i++)
	{
		m[conns[i][0]].insert(conns[i][1]);
		m[conns[i][1]].insert(conns[i][0]);
	}
	for (int i = 0;i < conns.size();i++)
	{
		m[conns[i][0]].erase(conns[i][1]);
		m[conns[i][1]].erase(conns[i][0]);

		int c = findComponent(count, m);
		if (c > 1) res.push_back(conns[i]);
		m[conns[i][0]].insert(conns[i][1]);
		m[conns[i][1]].insert(conns[i][0]);
	}
	
	return res;
}

vector<vector<int>> DataCenterConnect::FindLowCostKeyPath(int count, vector<vector<int>>& conns, vector<vector<int>>& costs)
{
	vector<vector<int>> res;
	map<int, set<int>> m;
	for (int i = 0;i < conns.size();i++)
	{
		m[conns[i][0]].insert(conns[i][1]);
		m[conns[i][1]].insert(conns[i][0]);
	}
	vector<vector<int>> components = findIsolatedComponent(count, m);

	return res;
}

bool DataCenterConnect::MergeSearch(int count, vector<vector<int>>& conns)
{
	int n, m, i, x, y;
	int * set = new int[count];
	
	for (i = 1;i < count;++i)        //初始化个集合，数组值等于坐标的点为根节点。
		set[i] = i;

	for (i = 0;i < conns.size();++i) {

		int a=conns[i][0], b=conns[i][1];

		int fx = find(a,set), fy = find(b,set);

		set[fx] = fy;                      //合并有边相连的各个连通分量

	}

	int cnt = 0;

	for (i = 1;i <= count;++i)          //统计集合个数，即为连通分量个数，为一时，图联通。
		if (set[i] == i)
			++cnt;

	delete[] set;
	if (cnt == 1) return true;
	else return false;
	//return cnt;
}
