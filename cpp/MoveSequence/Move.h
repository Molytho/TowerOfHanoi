#pragma once

#include "Constants.h"

#include <stdexcept>
#include <stdint.h>
#include <iostream>

template <typename T>
struct Move
{
	T startPeg;
	T endPeg;
};

template <typename T>
class MoveCollection
{
public:
	MoveCollection(size_t capacity)
	{
		_isParent = true;
		_capacity = capacity;
		_count = 0;

		_array = new Move<T>[capacity];
	};
	~MoveCollection()
	{
		if (_isParent)
			delete[] _array;
	};

	void Add(Move<T>& item)
	{
		if (_count = _capacity)
			throw std::out_of_range("Arrayspace already used");

		_array[_count++] = item;
	};
	void Add(T startPeg, T endPeg)
	{
		if (_count == _capacity)
			throw std::out_of_range("Arrayspace already used");

		_array[_count].startPeg = startPeg;
		_array[_count++].endPeg = endPeg;
	};

	void InverseMoves(T endPeg, MoveCollection<T>* collection, size_t noCopy)
	{
		if (_count > collection->_capacity - collection->_count + noCopy)
			throw new std::out_of_range("Not enough space to copy to the collection");
		for (size_t i = _count - noCopy; i > 0; i--)
		{
			T end =
				_array[i - 1].startPeg != 0 && _array[i - 1].startPeg != endPeg
				? _array[i - 1].startPeg
				: _array[i - 1].startPeg == 0
				? endPeg
				: 0;
			T start =
				_array[i - 1].endPeg != 0 && _array[i - 1].endPeg != endPeg
				? _array[i - 1].endPeg
				: _array[i - 1].endPeg == 0
				? endPeg
				: 0;
			collection->Add(start, end);
		}

		delete collection;
	};

	size_t Count() { return _count; }
	bool IsParent() { return _isParent; }

	MoveCollection<T>* GetSubSegment(size_t baseAddress, size_t count)
	{
		auto ret = new MoveCollection(&_array[baseAddress], count);
		this->_count += count;
		return ret;
	};

	friend MOVESEQUENCE_API std::ostream& operator<<(std::ostream& stream, const MoveCollection<T>& moveCollection);

private:
	MoveCollection(Move<T>* arrayBase, size_t capacity)
	{
		_isParent = false;
		_capacity = capacity;
		_count = 0;

		_array = arrayBase;
	};

	Move<T>* _array;
	size_t _count;
	size_t _capacity;
	bool _isParent;
};

template <typename T>
MOVESEQUENCE_API std::ostream& operator<<(std::ostream& stream, const Move<T>& move);
template <typename T>
MOVESEQUENCE_API std::ostream& operator<<(std::ostream& stream, const MoveCollection<T>& moveCollection);