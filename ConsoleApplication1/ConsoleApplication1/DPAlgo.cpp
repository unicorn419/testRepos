#include "pch.h"
#include "DPAlgo.h"
#include <algorithm>
#include <map>

using namespace std;
DPAlgo::DPAlgo()
{
}


DPAlgo::~DPAlgo()
{
}

int DPAlgo::maxSubMartix(vector<vector<int>> grid)
{
	int n, i, j, k, maxsubrec=0, maxsubarr;
	int *  arr = new int[grid.size()];
	maxsubrec = 0;
	for (i = 0;i < grid.size();i++)
	{
		memset(arr, 0, sizeof(arr));
		for (j = i;j < grid.size();j++)
		{
			for (k = 0;k < grid.size();k++)
				arr[k] += grid[j][k];
			maxsubarr = maxSubArray(arr, grid.size());
			maxsubrec = max(maxsubarr,maxsubrec);
		}
	}
	delete[] arr;
	return maxsubrec;

}

int DPAlgo::countSquares(vector<vector<int>>& matrix)
{
	int res = 0;
	int m = matrix.size();
	int n = matrix[0].size();

	vector<vector<int>> vis(m, vector<int>(n, 0));

	for (int i = 0;i < m;i++)
	{
		vis[i][0] = matrix[i][0];
		if (vis[i][0]) res++;
	}
	for (int i = 1;i < n;i++)
	{
		vis[0][i] = matrix[0][i];
		if (vis[0][i]) res++;
	}
	for (int i = 1;i < m;i++)
	{
		for (int j = 1;j < n;j++)
		{
			if (matrix[i][j])
			{
				//通过最小值获得前子矩阵的个数，潜在逻辑: 上、左、上左位置如果相等且当前值=1，则矩阵宽度为临近矩阵宽度+1，不等则应为临近的最小矩阵宽度+1，
				vis[i][j] = min(vis[i - 1][j], vis[i][j - 1]);
				vis[i][j] = min(vis[i][j], vis[i - 1][j - 1]);
				vis[i][j]++;
			}

			res += vis[i][j];
			//printf(" %d",vis[i][j]);
		}
		//printf("\n");
	}


	return res;


}

//b[i]=max(sum(0-i)) return max(b[i-1] + a[i],a[i])
int DPAlgo::maxSubArray(int * arr, int n)
{
	int i, b = 0, sum = 0;
	for (i = 0;i < n;i++)
	{
		if (b > 0)                // 若a[i]+b[i-1]会减小
			b += arr[i];        // 则以a[i]为首另起一个子段
		else
			b = arr[i];
		b = max(sum, b);
	}
	return sum;
}

int DPAlgo::maxOrMinPath(vector<vector<int>> grid)
{
	vector<vector<int>> dp(grid.size()+1, vector<int>(grid[0].size()+1, 0));
	for (int i = 0;i < grid.size();i++)
	{
		for (int j = 0;j < grid[i].size();j++)
		{
			dp[i + 1][j + 1] = max(dp[i][j + 1], dp[i + 1][j]) + grid[i][j];
			//dp[i + 1][j + 1] = min(dp[i][j + 1], dp[i + 1][j]) + grid[i][j];
		}
	}
	return dp[grid.size()][grid[0].size()];
}

int DPAlgo::maxValuePackage(int capacity, int goodsCount, vector<int>& weight, vector<int>& goodsValues)
{
	//dp[i][j] 可以使用j 空间的情况下 塞入0-i 的最大价值
	vector<vector<int>>dp(goodsCount+1,vector<int>(capacity+1,0));
	int maxVal = 0, minVal = 0;

	for (int i = 1; i <= goodsCount; i++) {
		for (int j = 1; j <= capacity; j++) {
			//dp[i - 1][j - weight[i]] 为前一个可放置物品的最大值 
			//dp[i - 1][j] 为前一个可放置物品当前最大值

			//假如可放入i 物品
			if (j >= weight[i])
				dp[i][j] = max(dp[i-1][j], dp[i][j - weight[i]] + goodsValues[i]);
			//如计算最小成本 ，此处改为min
			else
				//无法放置i物品,
				dp[i][j] = dp[i - 1][j];
		}
	}

	return dp[goodsCount][capacity];
}

int DPAlgo::changeCoins(int money, vector<int>& coins,int index)
{
	int res = 0;
	if (money == 0) return 1;
	if (money < 0) return 0;
	for (int i = 0;i < coins.size();i++)
	{
		for (int j = 1;j <= money / coins[i];j++)
		{
			res += changeCoins(money - coins[i] * j, coins,index-1);
		}
	}
	return res;
}

int DPAlgo::minCoins(int money, vector<int>& coins)
{
	vector<vector<int>> dp(coins.size()+1, vector<int>(money+1, 0));
	for (int j = 1;j <= money;j++) dp[0][j] = INT_MAX;
	for (int i = 1;i <= coins.size();i++)
	{
		for (int j = 1;j <= money;j++)
		{
			if (j >= coins[i-1])
			{
				dp[i][j] = min(dp[i - 1][j], dp[i][j-coins[i-1]]+1) ;
			}
			else
			{
				dp[i][j] = dp[i - 1][j];
			}
		}
	}
	return dp[coins.size()][money];



}


vector<string> DPAlgo::popularNFeatures(int numFeatures, int topFeatures,
	vector<string> possibleFeatures,
	int numFeatureRequests,
	vector<string> featureRequests)
{
	vector<string> res;

	std::map<string, int> mapWords;
	for (int i = 0;i < numFeatures;i++) mapWords[possibleFeatures[i]] = 0;

	for (int i = 0;i < numFeatureRequests;i++)
	{
		std::map<string, int> mapOccus;
		string request = featureRequests[i];
		int  end = 0;
		while (end < request.size())
		{
			string word(move(""));
			while (end < request.size() &&((request.at(end) >= 'a' && request.at(end) <= 'z') ||
				(request.at(end) >= 'A' && request.at(end) <= 'Z')))
			{
				if ((request.at(end) >= 'A' && request.at(end) <= 'Z'))
					word.push_back(request.at(end) - 'A' + 'a');
				else
					word.push_back(request.at(end));
				end++;
			}
			mapOccus[word]++;
			if (mapOccus[word] == 1)
				mapWords[word]++;

			end++;
		}
	}
	vector<pair<string, int>> vec;
	copy(mapWords.begin(), mapWords.end(), back_inserter<vector<pair<string, int>>>(vec));
	sort(vec.begin(), vec.end(), [](pair<string, int> &l, pair<string, int> &r) {
			return l.second >= r.second;
	});
	for (int i = 0;i < vec.size();i++)
		res.push_back(vec[i].first);

	return res;

}
