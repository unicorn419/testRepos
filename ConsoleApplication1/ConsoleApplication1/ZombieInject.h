#pragma once

#include <vector>

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

