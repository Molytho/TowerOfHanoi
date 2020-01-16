#pragma once
#include <cstdint>
#include <stdexcept>

template <typename T>
class Pegs
{
public:
	Pegs(T count, T pegs[])
	{
		_array = pegs;
		_count = count;

		needDispose = false;
	};
	Pegs(T count)
	{
		_array = new T[count];
		_count = count;

		for (T i = 0; i < count; i++)
			_array[i] = i;

		needDispose = true;
	};
	~Pegs()
	{
		if (needDispose)
			delete[] _array;
	};

	T& operator[](T index)
	{
		return _array[index];
	};
	T Count() { return _count; }

private:
	T _count;
	T* _array;
	bool needDispose;
};

template <typename N, typename T>
class TowerHeights
{
public:
	TowerHeights(T capacity)
	{
		if (sizeof(size_t) < sizeof(T))
			throw std::invalid_argument("T is not allowed to be bigger than size_t");
		_topBorder = new N[capacity];
		_bottomBorder = new N[capacity];
	};
	~TowerHeights()
	{
		delete[] _topBorder;
		delete[] _bottomBorder;
	};
	N& operator() (bool max_min, T index)
	{
		return max_min ? _topBorder[index] : _bottomBorder[index];
	};

private:
	N* _bottomBorder;
	N* _topBorder;
};