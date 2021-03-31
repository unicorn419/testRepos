// ConsoleApplication2.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "pch.h"
#include <iostream>

#include <stdio.h>
#include <stdlib.h>

int main(void) {
	int *n4, *n6, *n9, *cost;

	n4 = (int*)malloc(100 * sizeof(int));
	n6 = (int*)malloc(100 * sizeof(int));
	n9 = (int*)malloc(100 * sizeof(int));
	cost = (int*)malloc(100 * sizeof(int));

	int draw(int *, int* , int * , int * );
	int n = 0;
	n = draw(n4, n6, n9, cost);
	for (int i = 0;i < n;++i)
	{
		printf("%d %d %d %d\n", n4[i], n6[i], n9[i], cost[i]);
	}

	//????????????malloc ?free ??? ???(c??),  New ? Delete ??? ???(c++)?
	free(n4);
	//???????????????NULL? ???????????????????????????????????????????
	n4 = NULL;
	free(n6);
	n6 = NULL;
	free(n9);
	n9 = NULL;
	free(cost);
	cost = NULL;


}

/*
int draw(int * n4, int * n6, int * n9, int * cost)
{
	int count = 0;
	int e4 = 100 / 4;
	for (int i = 1;i <= e4;++i)
	{
		//?????i?4?????????6????
		int e6 = (100 - 4 * i) / 6;
		for (int j = 1;j <= e6;++j)
		{
			//?????i?4???j ?6 ?????????9????
			int e9 = (100 - 4 * i - 6 * j) / 9;
			for (int k = 1;k <= e9;++k)
			{
				if ((i * 4 + j * 6 + k * 9) == 100)
				{
					n4[count] = i;
					n6[count] = j;
					n9[count] = k;
					cost[count] = i * 20 + j * 25 + k * 30;
					//??????+1
					count++;

				}
			}
		}
	}
	return count;
}

*/

int draw(int *n4, int *n6, int *n9, int *cost) 
{ 
	int c4 = 20, c6 = 25, c9 = 30;
	int e4 = 100 / 4, e6 = 100 / 6, e9 = 100 / 9;
	int n = 0;
	for (int i=1; i <= e4; i++)
		for (int j=1; j <= e6; j++) 
			for (int k=1; k <= e9; k++) 
			{ 
				if (4 * i + 6 * j + 9 * k == 100) 
				{ 
					*(n4 + n) = i;
					*(n6 + n) = j; 
					*(n9 + n) = k;
					*(cost + n) = i * c4 + j * c6 + k * c9;
					n++; 
			} }
	return n; 
}