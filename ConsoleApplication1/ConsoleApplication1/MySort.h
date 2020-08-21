#include <map>
#pragma once
class MySort
{
public:
	MySort();
	~MySort();
	//合并排序
	//O(N-NLogN) S(N) stable
	void mergeSort(int * arr, int l, int r);
	//快排
	//O(NLogN-N*N) S(LogN) not stable
	void quickSort(int *arr, int low, int high);
	//基排，按每个数的位逐步排序，个位数->十位->百位
	//O(n) S(N)
	void radixsort(int *arr, int n);
	//桶排,思路,将数分配到一个桶里，然后桶内排序，然后按照大小顺序合并桶内数据
	void bucketSort(float *arr, int n);

	//void mapSortByValue(map<int, int> & m);
private:
	void merge(int * arr, int l, int m, int r);
	int partition(int *arr, int low, int high);
	void swap(int* a, int* b);

	int getMax(int *arr, int n);
	void countSort(int *arr, int n, int exp);

};

