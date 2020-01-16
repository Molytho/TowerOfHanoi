#pragma once

#ifdef MOVESEQUENCE_EXPORTS
#define MOVESEQUENCE_API __declspec(dllexport)
#else
#define MOVESEQUENCE_API __declspec(dllimport)
#endif

#include <stdexcept>

template <typename T>
void throwIfMultiplyOverflow(T a, T b)
{
	if (a == 0 || b == 0)
		return;

	T result = a * b;
	if (a != result / b)
		throw std::overflow_error("A multiplication overflow occured");
}
template <typename T>
void throwIfAdditionOverflow(T a, T b)
{
	T result = a + b;
	if (result < a && result < b)
		throw std::overflow_error("A addition overflow occured");
}
template <typename A, typename B>
B throwIfCastOverflow(A a)
{
	B cast = B(a);
	if (A(cast) != a)
		throw std::overflow_error("A cast overflow occured");
	return cast;
}