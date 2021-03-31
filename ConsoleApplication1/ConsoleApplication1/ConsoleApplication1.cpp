// ConsoleApplication1.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "pch.h"
#include <vector>
#include <set>
#include <iostream>
#include <algorithm> 
#include <map>
#include <unordered_map>
#include <deque>
#include <queue>
#include <string>>
#include "MySort.h"
#include "ZombieInject.h"
#include "FindSubStrings.h"
#include "DataCenterConnect.h"
#include"Tree.h"
#include "TireTree.h"
#include "DPAlgo.h"
#include "StockSpanner.h"

using namespace std;
class Solution {
public:
	int singleNumber(vector<int>& nums) {
		while(nums.size()>1)
		{
			nums[0] = nums[0] ^ nums[nums.size()-1];
			nums.pop_back();
		}
		return nums[0];
	}

	bool isHappy(int n)
	{

		while (n != 1 && n != 4)
		{
			int _sum = 0;
			while(n)
			{
				_sum += (n % 10) * (n % 10);
				n /= 10;
			}
			n = _sum;
			
		}
		return n == 1;
	}
	int maxSubArray(vector<int>& nums) {
		
		int result = nums[0];
		int sum = nums[0];

		for (int i = 1; i < nums.size(); i++) {
			sum = max(nums[i], sum + nums[i]);
			result = max(result, sum);
		}

		return result;
	}

	void moveZeroes(vector<int>& nums) {

	}
	bool backspaceCompare(string S, string T) {
		int i=S.size()-1,  i1=T.size() - 1;

		while (i>=0 || i1>=0)
		{
			int f = 0, f1 = 0;
			while (i>=0)
			{
				if (S.at(i) >= 'a' && S.at(i) <= 'z') 
				{
					if (f == 0) { break; }
					else { f--; }
				}
				else { f++; }
				i--;
			}
			
			while (i1>=0)
			{
				if (T.at(i1) >= 'a' && T.at(i1) <= 'z')
				{
					if (f1 == 0) { break; }
					else { f1--; }
				}
				else { f1++; }
				i1--;
			}
			if ((f > 0 && f1 > 0)|| (i<0 && i1<0))
				return true;
			else if ((i<0 && i1>=0)  || i>=0 && i1<0)
				return false;

			if (S.at(i) != T.at(i1)) 
				return false;
			i--;
			i1--;
		}
		return true;
	}
	int lastStoneWeight(vector<int>& stones) {
		
		while (stones.size()>1)
		{
			sort(stones.begin(), stones.end());

			int & s1 = stones.back();
			stones.pop_back();
			int & s2 = stones.back();
			stones.pop_back();

			if (s1 == s2) continue;
			else stones.push_back(abs(s1 - s2));
		}

	}
	int longestCommonSubsequence(string text1, string text2) {
		if (text1.length() <= 0) return 0;
		if (text2.length() <= 0) return 0;

		int count = 0;
		vector<vector<int>> visited(text1.length(), vector<int>(text2.length(), 0));
		for (int i = 0;i < text1.length();i++)
		{
			visited[i][0] = text1.at(i) == text2.at(0);
			if (visited[i][0]) count = 1;
		}
		for (int i = 0;i < text2.length();i++)
		{
			visited[0][i] = text2.at(i) == text1.at(0);
			if (visited[0][i]) count = 1;
		}
		for (int i = 1;i < text1.length();i++)
		{
			for (int j = 1;j < text2.length();j++)
			{
				if (text1.at(i) == text2.at(j)) {
					visited[i][j] = max(visited[i - 1][j], visited[i][j - 1]);
					visited[i][j] = max(visited[i][j], visited[i - 2][j - 1]);
					visited[i][j]++;
				}
			}
		}

		return visited[text1.length() - 1][text2.length() - 1];
	}
	int findMaxLength(vector<int>& nums) {

		int maxsize = -1, startindex;
		int n = nums.size();

		int *sumleft = new int[n] ;

		int min, max;

		sumleft[0] = ((nums[0] == 0) ? -1 : 1);
		min = nums[0]; max = nums[0];
		for (int i = 1; i < n; i++)
		{
			sumleft[i] = sumleft[i - 1] + ((nums[i] == 0) ?
				-1 : 1);
			if (sumleft[i] < min)
				min = sumleft[i];
			if (sumleft[i] > max)
				max = sumleft[i];
		}

		int *hash= new int[max - min + 1];

		// Initialize hash table 

		for (int i = 0; i < max - min + 1; i++)
			hash[i] = -1;

		for (int i = 0; i < n; i++)
		{
			// Case 1: when the subarray starts from  
			//         index 0 

			if (sumleft[i] == 0)
			{
				maxsize = i + 1;
				startindex = 0;
			}

			// Case 2: fill hash table value. If already 
			//         filled, then use it 

			if (hash[sumleft[i] - min] == -1)
				hash[sumleft[i] - min] = i;
			else
			{
				if ((i - hash[sumleft[i] - min]) > maxsize)
				{
					maxsize = i - hash[sumleft[i] - min];
					startindex = hash[sumleft[i] - min] + 1;
				}
			}
		}
		if (maxsize == -1)
			printf("No such subarray");
		else
			printf("%d to %d", startindex, startindex + maxsize - 1);
		delete []sumleft;
		delete []hash;
		return maxsize;
	}
	string stringShift(string s, vector<vector<int>>& shift) {
		int left = 0, right = 0;
		for (auto & pairs : shift)
		{
			if (pairs[0] == 1) right += pairs[1];
			else left += pairs[1];
		}
		if (right == left) return s;

		string tmp(s);

		int count = abs(right - left) % s.size();
		for (int i = 0;i < count;++i)
		{
			if (right > left)
			{
				tmp.insert(tmp.begin(), s.at(s.size() - 1 - i));
				tmp.pop_back();
			}
			else
			{
				tmp.push_back(s.at(i));
				tmp.erase(tmp.begin());

			}
		}

		return tmp;
	}
	int subarraySum(vector<int>& nums, int k) {
		unordered_map<int, int> preSum;
		
		int res = 0;

		// Sum of elements so far. 
		int currsum = 0;

		for (int i = 0; i < nums.size(); i++) {

			currsum += nums[i];
			if (currsum == k)
				res++;
			if (preSum.find(currsum - k) !=
				preSum.end())
				res += (preSum[currsum - k]);
			preSum[currsum]++;
		}

		return res;
	}
	
};
class LRUNode
{
public:
	LRUNode * next;
	LRUNode * prev;
	int val;
	int key;
	LRUNode(int _key,int _value)
	{
		key = _key;
		val = _value;
		next = NULL;
		prev = NULL;
	}

};
class LRUCache {
public:
	LRUCache(int capacity) {
		size = capacity;
		head->next = tail;
		tail->prev = head;
	}

	int get(int key) {
		if (deq.find(key) != deq.end())
		{
			LRUNode * newNode = deq[key];
			newNode->prev->next = newNode->next;
			newNode->next->prev = newNode->prev;
			tail->prev->next = newNode;
			newNode->prev = tail->prev;
			newNode->next = tail;
			tail->prev = newNode;
			return deq[key]->val;
		}
		else
		{
			return -1;
		}
	}

	void put(int key, int value) {
		LRUNode * newNode;

		if (deq.find(key) != deq.end())
		{
			newNode = deq[key];
			newNode->val = value;
			newNode->prev->next = newNode->next;
			newNode->next->prev = newNode->prev;
		}
		else
		{
			newNode = new LRUNode(key,value);
			deq[key] = newNode;
		}
		tail->prev->next = newNode;
		newNode->prev = tail->prev;
		newNode->next = tail;
		tail->prev = newNode;

		if (size < deq.size())
		{
			LRUNode * tmp = head->next;
			head->next = tmp->next;
			tmp->next->prev = head;
			
			
			deq.erase(tmp->key);
			delete tmp;

		}
	}
private:
	unordered_map<int, LRUNode*> deq;
	LRUNode *head = new LRUNode(0,0);
	LRUNode *tail = new LRUNode(0,0);

	int size = 0;
};







class Solution123 {
public:
	int maximalSquare(vector<vector<char>>& matrix) {
		int count = 0;
		vector<vector<int>> visited(matrix.size(), vector<int>(matrix[0].size(), -1));
		for (int i = 0;i < matrix.size();i++)
		{
			visited[i][0] = matrix[i][0]=='1';
		}
		for (int i = 0;i < matrix[0].size();i++)
		{
			visited[0][i] = matrix[0][i]=='1';
		}
		for (int i = 1;i < matrix.size();i++)
		{
			for (int j = 1;j < matrix[i].size();j++)
			{
				if (matrix[i][j] == '1') {
					int minVal = min(visited[i - 1][j], visited[i][j - 1]);
					minVal = min(minVal, visited[i - 1][j - 1]);
					visited[i][j] = minVal + 1;

					count = max(count, minVal + 1);
				}
				else {
					visited[i][j] = 0;
				}
			}
		}
		
		return count*count;
	}
	void DFS(vector<vector<char>>& grid, int i, int j, vector<vector<int>>& visited,int & count)
	{
		if (grid[i][j] == '1' && grid[i - 1][j] == '1' && grid[i][j - 1] == '1' && grid[i - 1][j - 1] == '1')
		{
			visited[i][j] = visited[i - 1][j - 1] + 1;
			count = max(count, visited[i][j]);
		}
		
	}
};

class FirstUnique {
	struct NodeList
	{
		public:
			NodeList * next;
			NodeList * prev;
			int val;
			NodeList(int _val)
			{
				val = _val;
				next = NULL;
				prev = NULL;
			}
	};
	public:
		FirstUnique(vector<int>& nums) {
			head = new NodeList(0);
			NodeList * cur;
			cur = head;
			for (int i = 0;i < nums.size(); ++i)
			{
				NodeList * tmp = new NodeList(nums[i]);
				cur->next = tmp;
				tmp->prev = cur;
				cur = tmp;
				if (counter.find(nums[i]) != counter.end())
					counter[nums[i]]++;
				else
					counter[nums[i]] = 1;
			}
			tail = cur;

		}

		int showFirstUnique() {
			NodeList * cur = head;
			while (cur != NULL)
			{
				if (counter[cur->val] == 1)
					return cur->val;
				else
					cur = cur->next;
			}
			return -1;
		}

		void add(int value) {
			NodeList * tmp = new NodeList(value);
			tail->next = tmp;
			tmp->prev = tail;
			tail = tmp;
			counter[value]++;
		}
	private:

		unordered_map<int, int> counter;
		NodeList * head;
		NodeList * tail;

};

bool compare(int const &i1, int const &i2)
{
	return i1<i2;
}
int main()
{
	MySort mysort;
	int * arr= new int[10] {9,10,4,2,1,5,15,8,7,3};
	mysort.quickSort(arr, 0, 9);
	

	Solution s;
	vector<int> vec{ 4,2,1,2,1 };
	vector<int> maxSubVec{ -2,1,-3,4,-1,2,1,-5,4 };
	vector<int> moveZeroesVec{ 0,1,0,3,12 };
	vector<int> maxLengthSubarray{ 0,1 };
	vector<vector<char>> vectroNums;// { , { "1","1","0","1","0" }, { "1","1","0","0","0" }, { "0","0","0","0","0" } };
	vector<char> v1{ '1', '1', '1', '1', '0' };
	vectroNums.push_back(v1);
	vector<char> v2{ '1', '1', '0', '1', '0' };
	vectroNums.push_back(v2);
	vector<char> v3{ '1', '1', '0', '0', '0' };
	vectroNums.push_back(v3);
	vector<char> v4{ '0', '0', '0', '0', '0' };
	vectroNums.push_back(v4);

	vector<int>::iterator  atr = std::lower_bound(vec.begin()+2, vec.begin()+4, 1);
	if (atr == vec.end())
	{
		int adf = 123;
	}
	else
	{
		int adf;
		adf= atr - vec.begin();
		int aaaaaa = 12;
	}
	
	/*vector<int> vectorsubarray{ 0,0,0,0,0,0,0,0,0,0};
	LRUCache lCache(2);
	lCache.put(1, 1);
	lCache.put(2, 2);
	lCache.get(1);
	lCache.put(3, 3);
	lCache.get(2);
	lCache.put(4, 4);
	lCache.get(1);
	lCache.get(3);
	lCache.get(4);
	*/

	//s.singleNumber(vec);
	//s.isHappy(13);
	//s.maxSubArray(maxSubVec);
	//s.moveZeroes(moveZeroesVec);

	//s.backspaceCompare(string("xywrrmp"), string("xywrrmu#p"));
	//s.findMaxLength(maxLengthSubarray);
	//s.numIslands(vectroNums);
	//s.subarraySum(vectorsubarray,0);


	Solution123 s1;
	s1.maximalSquare(vectroNums);
	s.longestCommonSubsequence("abcde", "ace");
	vector<int> firstUniqueVector{ 233 };
	FirstUnique f(firstUniqueVector);
	f.showFirstUnique();
	f.add(11);
	f.showFirstUnique();
	/*
	Tree<int> root(0);
	queue<Tree<int> *> queue;
	queue.push(&root);
	while (!queue.empty())
	{
		Tree<int> *node = queue.front();
		queue.pop();

		queue.push(root.Left);
		queue.push(root.Right);
	}
	*/
	TireTree tiretree;
	tiretree.insert("abcdefggg");
	tiretree.insert("abcdefgggaaaa");
	tiretree.insert("ccdefgggaaaa");
	bool i = tiretree.search("cbc");
	TireTree * tmp = tiretree.searchNode("a");

	string tmps("");
	set<string> left;

	tmp->getLeftPossibleString(tmps, tmp->count, tmp, left);
	for (set<string, less<string>>::iterator cur = left.begin();cur != left.end();cur++)
	{
		string st = *cur;
		//printf("%s", st.c_str());
		//std::cout << "value:" << st.c_str<<endl;
	}

//	MySort mysort;

//	float arr[] = { 0.897, 0.565, 0.656, 0.1234, 0.665, 0.3434 };
//	int n = sizeof(arr) / sizeof(arr[0]);
	//mysort.bucketSort(&arr, n);

	

	ZombieInject zomb;
	vector<vector<int>> input ={ {0, 0, 1, 0, 0, 0 },
								 {0, 0, 0, 0, 0 ,0 },
								 {0, 0, 0, 0, 0 ,0 },
								 {0, 0, 0, 0, 0 ,0 } };
	int val = zomb.calcDays(input);

	FindSubStrings findSubstrings;
	vector<string> v= findSubstrings.Find("bbeadcxede");
	 v = findSubstrings.Find("baddacxb");

	 DataCenterConnect datacenter;
	 vector<vector<int>> dateCentervector = { {1,2},{1,3},{3,2},{3,4},{4,5},{4,6},{5,6} };
	 vector<vector<int>> dataRes = datacenter.FindKeyPath(4, dateCentervector);

	 dateCentervector = { {1, 4},{4, 5},{2, 3}};
	 vector<vector<int>> costs = { {1,2,5},{1,3,10},{1,6,2},{5,6,5} };
	 dataRes= datacenter.FindLowCostKeyPath(6, dateCentervector, costs);
	 bool isConnected =datacenter.MergeSearch(6, dateCentervector);

	 DPAlgo dpalgo;
	 vector<int> coins = { 1,2,5 };
	 int resMoney = dpalgo.minCoins(18, coins);
	 int index = 2;
	 int totolMoney = dpalgo.changeCoins(5, coins,index);
	 
	 map<string, int> map1;
	 string s11("aaa");
	 string s12("aaa");
	 map1[s11]++;
	 map1[s12]++;

	 vector<string> vFeatures= dpalgo.popularNFeatures(5, 2,{"ab","bc","cd","de","ef"}, 3, {"AB ab cd!","ab de,ef.","ab cd de"});

	 StockSpanner stock;
	 stock.next(100);
	 stock.next(80);
	 stock.next(60);
	 stock.next(70);
	 stock.next(60);
	 stock.next(75);
	 stock.next(85);

	return 0;

	

}


