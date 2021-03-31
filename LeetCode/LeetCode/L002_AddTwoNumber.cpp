#include "pch.h"
#include "L002_AddTwoNumber.h"


L002_AddTwoNumber::L002_AddTwoNumber()
{
}


L002_AddTwoNumber::~L002_AddTwoNumber()
{
}

ListNode * L002_AddTwoNumber::addTwoNumbers(ListNode * l1, ListNode * l2)
{
	bool flag = 0;
	ListNode * res = l1;
	ListNode * last = l1;
	while (l1 != nullptr)
	{
		if (l2 != nullptr)
		{
			l1->val += l2->val + flag;
		}
		else
		{
			l1->val += flag;
		}
		if (l1->val >= 10)
		{
			flag = l1->val / 10;
			l1->val %= 10;
		}
		else
			flag = 0;
		last = l1;
		l1 = l1->next;
		if (l1 == nullptr && flag) {
			last->next = new ListNode(1);
			l1 = last->next;
			flag = 0;
		}
		if (l2 != nullptr) l2 = l2->next;
	}
	if (l2 != nullptr) last->next = l2;
	while (l2 != nullptr && flag)
	{
		l2->val += flag;
		flag = l2->val / 10;
		l2->val %= 10;

		if (l2 != nullptr) l2 = l2->next;
	}
	return res;
}
	
	


void L002_AddTwoNumber::Test()
{
	ListNode l1(2,new ListNode(4,new ListNode(3)));
	ListNode l2(5, new ListNode(6, new ListNode(4)));

	ListNode *res= addTwoNumbers(&l1, &l2);

	return;
	
}
