#pragma once

#include <vector>

/*
一个二维数组, 0 为 人， 1为僵尸，僵尸每天可以上下左右移动一格，并感染格子里的人.
计算多久天可以感染全部格子.


*/

using namespace std;
class ZombieInject
{
public:
	int calcDays(vector<vector<int>> & input);
	ZombieInject();
	~ZombieInject();
private:
	bool isValid(vector<vector<int>> &grid, int x, int y);
	bool isZombie(vector<vector<int>> &grid, int x, int y);
	bool isPeople(vector<vector<int>> &grid, int x, int y);
	vector<pair<int, int>> coors = { {1,0},{-1,0},{0,1},{0,-1} };
		
};

