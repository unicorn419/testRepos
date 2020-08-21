#include "pch.h"
#include "MySort.h"
#include <algorithm>
#include <vector>

using namespace std;
MySort::MySort()
{
}


MySort::~MySort()
{
}

void MySort::mergeSort(int * arr, int l, int r)
{
	if (l < r)
	{
		
		// Same as (l+r)/2, but avoids overflow for 
		// large l and h 
		int m = l + (r - l) / 2;

		// Sort first and second halves 
		mergeSort(arr, l, m);
		mergeSort(arr, m + 1, r);

		merge(arr, l, m, r);
	}

}

void MySort::merge(int * arr, int l, int m, int r)
{
	int i, j, k;
	int n1 = m - l + 1;
	int n2 = r - m;

	/* create temp arrays */
	int *L = new int[n1]
		, *R = new int[n2];

	/* Copy data to temp arrays L[] and R[] */
	for (i = 0; i < n1; i++)
		L[i] = arr[l + i];
	for (j = 0; j < n2; j++)
		R[j] = arr[m + 1 + j];

	/* Merge the temp arrays back into arr[l..r]*/
	i = 0; // Initial index of first subarray 
	j = 0; // Initial index of second subarray 
	k = l; // Initial index of merged subarray 
	while (i < n1 && j < n2)
	{
		if (L[i] <= R[j])
		{
			arr[k] = L[i];
			i++;
		}
		else
		{
			arr[k] = R[j];
			j++;
		}
		k++;
	}

	/* Copy the remaining elements of L[], if there
	   are any */
	while (i < n1)
	{
		arr[k] = L[i];
		i++;
		k++;
	}

	/* Copy the remaining elements of R[], if there
	   are any */
	while (j < n2)
	{
		arr[k] = R[j];
		j++;
		k++;
	}

	delete[] L;
	delete[] R;
}


int  MySort::partition(int * arr, int low, int high)
{
	int pivot = arr[high]; // pivot  
	int i = (low - 1); // Index of smaller element  

	for (int j = low; j <= high - 1; j++)
	{
		// If current element is smaller than the pivot  
		if (arr[j] < pivot)
		{
			i++; // increment index of smaller element  
			swap(&arr[i], &arr[j]);
		}
	}
	swap(&arr[i + 1], &arr[high]);
	return (i + 1);
}

void MySort::quickSort(int * arr, int low, int high)
{
	if (low < high)
	{
		/* pi is partitioning index, arr[p] is now
		at right place */
		int pi = partition(arr, low, high);

		// Separately sort elements before  
		// partition and after partition  
		quickSort(arr, low, pi - 1);
		quickSort(arr, pi + 1, high);
	}
}

void MySort::radixsort(int * arr, int n)
{
	// Find the maximum number to know number of digits 
	int m = getMax(arr, n);

	// Do counting sort for every digit. Note that instead 
	// of passing digit number, exp is passed. exp is 10^i 
	// where i is current digit number 
	for (int exp = 1; m / exp > 0; exp *= 10)
		countSort(arr, n, exp);
}

void MySort::bucketSort(float * arr, int n)
{
	std::vector<float> * b =new vector<float>[n] ;

	// 2) Put array elements in different buckets 
	for (int i = 0; i < n; i++)
	{
		int bi = n * arr[i]; // Index in bucket 
		b[bi].push_back(arr[i]);
	}

	// 3) Sort individual buckets 
	for (int i = 0; i < n; i++)
		std::sort(b[i].begin(), b[i].end());

	// 4) Concatenate all buckets into arr[] 
	int index = 0;
	for (int i = 0; i < n; i++)
		for (int j = 0; j < b[i].size(); j++)
			arr[index++] = b[i][j];
	delete[] b;
}

/*void MySort::mapSortByValue(map<int, int>& m)
{
	vector<pair<int, int>> vec;
	copy(m.begin(), m.end(), back_inserter<vector<pair<int, int>>>(vec));
	sort(vec.begin(), vec.end(), [](const pair<int, int>&l, const pair<int, int>&r) {
		if (l.second != r.second)
			return l.second < r.second;
		return l.first < r.first;
	});
	
}*/

int MySort::getMax(int *arr, int n)
{
	int mx = arr[0];
	for (int i = 1; i < n; i++)
		if (arr[i] > mx)
			mx = arr[i];
	return mx;
}
void MySort::countSort(int *arr, int n, int exp)
{
	int * output=new int[n]; // output array 
	int i, count[10] = { 0 };

	// Store count of occurrences in count[] 
	for (i = 0; i < n; i++)
		count[(arr[i] / exp) % 10]++;

	// Change count[i] so that count[i] now contains actual 
	//  position of this digit in output[] 
	for (i = 1; i < 10; i++)
		count[i] += count[i - 1];

	// Build the output array 
	for (i = n - 1; i >= 0; i--)
	{
		output[count[(arr[i] / exp) % 10] - 1] = arr[i];

		//if set the pos, then corresponding position will be -1
		count[(arr[i] / exp) % 10]--;
	}

	// Copy the output array to arr[], so that arr[] now 
	// contains sorted numbers according to current digit 
	for (i = 0; i < n; i++)
		arr[i] = output[i];

	delete[] output;
}

void MySort::swap(int* a, int* b)
{
	int t = *a;
	*a = *b;
	*b = t;
}
