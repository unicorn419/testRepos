#pragma once
#include <vector>

/*
算法，集中在DP方面

*/
using namespace std;
class DPAlgo
{
public:
	DPAlgo();
	~DPAlgo();
	//计算最大子块的和
	int maxSubMartix(vector<vector<int>> grid);

	//计算全为1的子正方形个数
	int countSquares(vector<vector<int>>& matrix);
	
	//计算最大连续数组的和
	int maxSubArray(int * arr, int n);
	//二维表中计算A-B的最小或最大路径
	int maxOrMinPath(vector<vector<int>> grid);
	//经典装包问题,容量为capacity 的背包，装 未知个 对应 goodsCost 容量的货物，怎么能让背包里 所有物品的  goodsValues 最大
	int maxValuePackage(int capacity, int goodsCount,vector<int> &goodsCost,vector<int> & goodsValues);
	//经典找零问题，对指定的money 和指定的零钱 找到找零组合个数
	int changeCoins(int money, vector<int> &coins,int index);
	//经典找零问题，对指定的money 和指定的零钱 如何找到最小的找零个数
	int minCoins(int money, vector<int> &coins);
	vector<string> popularNFeatures(int numFeatures, int topFeatures,
		vector<string> possibleFeatures,
		int numFeatureRequests,
		vector<string> featureRequests);
};

