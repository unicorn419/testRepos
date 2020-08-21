#include "pch.h"
#include "StockSpanner.h"
#include <vector>



StockSpanner::StockSpanner()
{
}


StockSpanner::~StockSpanner()
{
}

int StockSpanner::next(int price)
{
	vector<int> node = { price,1,-1 };
	data.push_back(node);
	int prev = data.size() - 2;
	while (prev >= 0)
	{
		if (price >= data[prev][0])
		{
			data[data.size() - 1][1] += data[prev][1];
			prev = data[prev][2];
		}
		else
		{
			data[data.size() - 1][2] = prev;
			break;
		}

	}
	return data[data.size() - 1][1];

}
