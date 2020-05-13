#include "pch.h"
#include "ZombieInject.h"
#include<vector>
#include <algorithm>
#include <queue>


ZombieInject::ZombieInject()
{
}


ZombieInject::~ZombieInject()
{
}

int ZombieInject::calcDays(vector<vector<int>> & grid)
{
	if (grid.size() == 0) return -1;

	queue<pair<int, int>> q;
	
	int w = grid[0].size();
	int h = grid.size();

	for (int i = 0; i < h; i++) {
		for (int j = 0; j < w; j++) {
			if (isZombie(grid, i, j)) {
				q.push({ i,j });
			}
		}
	}

	int res = -1;
	while (!q.empty())
	{
		int sz = q.size();
		res++;
		for (int i = 0; i < sz; i++) {
			pair<int,int> temp = q.front();
			q.pop();
			for (auto coor : coors) {
				int x = coor.first + temp.first;
				int y = coor.second + temp.second;
				if (isValid(grid, x, y)) {
					if (isZombie(grid, x, y)) {
						continue;
					}
					if (isPeople(grid, x, y)) {
						grid[x][y] = 1;
						q.push({ x,y });
					}
				}
			}
		}

	}
	for (int i = 0; i < h; i++) {
		for (int j = 0; j < w; j++) {
			if (isPeople(grid, i, j)) {
				return -1;
			}
		}
	}

	return res;
}

bool ZombieInject::isValid(vector<vector<int>> &grid, int x, int y) {
	return x >= 0 && y >= 0 && x <= grid.size() - 1 && y <= grid[0].size() - 1;
}

bool ZombieInject::isZombie(vector<vector<int>> &grid, int x, int y) {
	return grid[x][y] == 1;
}

bool ZombieInject::isPeople(vector<vector<int>> &grid, int x, int y) {
	return grid[x][y] == 0;
}