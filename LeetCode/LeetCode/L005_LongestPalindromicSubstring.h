/*
Given a string s, find the longest palindromic substring in s. You may assume that the maximum length of s is 1000.

Example 1:

Input: "babad"
Output: "bab"
Note: "aba" is also a valid answer.
Example 2:

Input: "cbbd"
Output: "bb"
*/
#include <string>
using namespace std;
#pragma once
class L005_LongestPalindromicSubstring
{
public:
	L005_LongestPalindromicSubstring();
	~L005_LongestPalindromicSubstring();
	string longestPalindrome(string s);
};

