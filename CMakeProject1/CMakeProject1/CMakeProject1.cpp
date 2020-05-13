// CMakeProject1.cpp : Defines the entry point for the application.
//

#include "CMakeProject1.h"
#include <algorithm>
#include <thread>
#include <mutex>
#include <memory>
#include <vector>
#include <string>
#include <codecvt>
#include <queue>
#include <deque>
#include <map>



using namespace std;



template <typename T>
class MyClass
{
public:
	MyClass();
	MyClass(int &);
	virtual ~MyClass();
	MyClass(const MyClass&);
	MyClass<T> & operator=(const MyClass&);
	static void Start();
	virtual void test(int i=0);

private:
	int c;

};

template <typename T>
MyClass<T>::MyClass()
	:c(1)
{
	
	printf("init");
	
}
template <typename T>
MyClass<T>::MyClass(int & val)
	:c(val)
{
	printf("init\n");

}

template <typename T>
MyClass<T>::MyClass(const MyClass& t)
{
	printf("copy");
}

template <typename T>
MyClass<T>::~MyClass()
{
}

template <typename T>
MyClass<T> & MyClass<T>::operator=(const MyClass & t)
{
	printf("=");
	if (this == &t) return *this;
	return *this;
}

template <typename T>
void MyClass<T>::Start()
{
	for (int i = 0;i < 10;i++)
	{
		printf("start");
		
	}
}

template<typename T>
void MyClass<T>::test(int i=0)
{
	printf("%d", i);
}


class MyClass1:public MyClass<int>
{
public:
	MyClass1();
	MyClass1(const MyClass1&);
	void start1() const;
	virtual ~MyClass1();
	virtual void test(int);

private:

};

MyClass1::MyClass1()
{
	printf("InitMyClass1\n");
}

MyClass1::MyClass1(const MyClass1 &)
{
	printf("Copy\n");
}

void MyClass1::start1() const
{
	//printf("myClasss fuc\n");
}

 MyClass1::~MyClass1()
{
}

void MyClass1::test(int i)
{
	printf("%d", i);
}

MyClass1 CreateNew() 
{
	return MyClass1();
}

struct ListNode {
	int val;
	ListNode *next;
	ListNode(int x) : val(x), next(NULL) {}
};

struct TreeNode {
	int val;
	TreeNode *left;
	TreeNode *right;
	TreeNode(int x) : val(x), left(NULL), right(NULL) {}

};

struct TreeNodeLevel
{
	TreeNode * node;
	int level;
	TreeNodeLevel(TreeNode* t, int l) :node(t), level(l) {}
};


class Solution {
public:
	static vector<vector<int>> levelOrder(TreeNode* root) {

		vector<vector<int>> re;
		if (root == NULL) return re;
		queue<TreeNodeLevel> que;
		TreeNodeLevel t(root, 0);
		que.push(t);
		int curLev = -1;
		while ((int)que.size() > 0)
		{
			TreeNodeLevel * tmp = &que.front();
			que.pop();

			if (tmp->level > curLev)
			{
				vector<int> v;
				re.push_back(v);
				curLev++;
			}
			re[tmp->level].push_back(tmp->node->val);



			if (tmp->node->left != NULL)
			{
				TreeNodeLevel tLeft(tmp->node->left, tmp->level++);
				que.push(tLeft);
			}

			if (tmp->node->right != NULL)
			{
				TreeNodeLevel tRight(tmp->node->right, tmp->level++);
				que.push(tRight);
			}
		}
	}






};



int main()
{
	Solution::levelOrder(NULL);


	return 0;
}

