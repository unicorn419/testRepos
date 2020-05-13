#pragma once
#include <vector>

using namespace std;
class DPAlgo
{
public:
	DPAlgo();
	~DPAlgo();
	int maxSubMartix(vector<vector<int>> grid);
	int maxSubArray(int * arr, int n);
};

