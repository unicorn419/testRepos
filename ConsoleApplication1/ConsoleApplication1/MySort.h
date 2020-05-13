#pragma once
class MySort
{
public:
	MySort();
	~MySort();
	//O(N-NLogN) S(N) stable
	void mergeSort(int * arr, int l, int r);

	//O(NLogN-N*N) S(LogN) not stable
	void quickSort(int *arr, int low, int high);

	//O(n) S(N)
	void radixsort(int *arr, int n);

	void bucketSort(float *arr, int n);
private:
	void merge(int * arr, int l, int m, int r);
	int partition(int *arr, int low, int high);
	void swap(int* a, int* b);

	int getMax(int *arr, int n);
	void countSort(int *arr, int n, int exp);

};

