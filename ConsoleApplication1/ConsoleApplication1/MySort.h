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
	//思路: 找一个数，将小于的数交换到左边，大于的交换到右边，定位该数的确切位置，然后继续进行该数左面的快排和右面的快拍。
	//O(NLogN-N*N) S(LogN) not stable
	void quickSort(int *arr, int low, int high);

	//插排， O(N*N)
	//认为第一个元素是排好序的，从第二个开始遍历。
	//拿出当前元素的值，从排好序的序列中从后往前找。
	//如果序列中的元素比当前元素大，就把它后移。直到找到一个小的。
	//把当前元素放在这个小的后面（后面的比当前大，它已经被后移了）。
	void insertion_sort(vector<int> v);
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

