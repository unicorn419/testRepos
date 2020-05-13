#pragma once
using namespace std;

template<class T>
class Tree
{
public:
	Tree();
	~Tree();
	T val;
	Tree * Left;
	Tree * Right;
	Tree(T v)
	{
		val = v;
		Left = NULL;
		Right = NULL;
	}
};

