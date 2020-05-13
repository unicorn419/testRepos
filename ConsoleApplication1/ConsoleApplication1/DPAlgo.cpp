#include "pch.h"
#include "DPAlgo.h"
#include <algorithm>

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
