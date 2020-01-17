#include "MoveSequence.h"
#include "move.h"
#include "Pegs.h"

#include "../MathematicFunctions/MathematicFunctions.h"

#include <future>
#include <cstdint>
#include <stdexcept>
#include <vector>
#include <algorithm>


template <typename N, typename T> std::future<MoveCollection<T>*> CalculateMoveSequenceAsync(T pegCount, N diskCount)
{
	return std::async(static_cast<MoveCollection<T> * (*)(T, N)>(CalculateMoveSequence), pegCount, diskCount);
}
template <typename N, typename T> MoveCollection<T>* CalculateMoveSequence(T pegCount, N diskCount)
{
	if (pegCount < 3)
		throw std::invalid_argument("Less than 3 pegs are senseless");
	if (diskCount == 0)
		throw std::invalid_argument("You need one move at least");

	uint64_t moveCount = CalculateMoveCount(pegCount, diskCount);
	if (moveCount > size_t(-1))
	{
		throw std::invalid_argument("Too Many Moves");
	}
	MoveCollection<T>* moveCollection = new MoveCollection<T>(size_t(moveCount));
	Pegs<T>* pegs = new Pegs<T>(pegCount);
	auto task = CalculateMoveSequenceAsync(pegCount, diskCount, *pegs, T(0), T(pegCount - 1), moveCollection, size_t(moveCount));
	task.get();

	return moveCollection;
}

template <typename N, typename T> std::future<void> CalculateMoveSequenceAsync(T pegCount, N diskCount, Pegs<T>& pegs, T startPeg, T endPeg, MoveCollection<T>* moveCollection, size_t moveCount)
{
	return diskCount == 1
		? std::async([&](MoveCollection<T>* moveCollection, Pegs<T>& pegs)
			{
				moveCollection->Add(startPeg, endPeg);

				delete moveCollection;
				delete& pegs;
			}, moveCollection, std::ref(pegs))
		: pegCount == 3
				? Calculate3PegAsync(diskCount, pegs, startPeg, endPeg, moveCollection)
				: Calculate4PlusPegAsync(pegCount, diskCount, pegs, startPeg, endPeg, moveCollection, moveCount);
}

template <typename N, typename T> TowerHeights<N, T>* GetTowerHeights(T pegCount, uint64_t increment)
{
	T capacity = pegCount - 1;
	TowerHeights<N, T>* ret = new TowerHeights<N, T>(capacity);

	(*ret)(0, pegCount - 2) = N(1);
	(*ret)(1, pegCount - 2) = N(1);

	try
	{
		for (N i = 1; i < pegCount - 1; i++)
		{
			uint64_t n = increment - 2 + i;
			uint64_t k = i;

			uint64_t previousMax = (*ret)(0, N(pegCount) - 1 - i);
			throwIfMultiplyOverflow(previousMax, n);
			(*ret)(0, pegCount - 2 - i) = throwIfCastOverflow<uint64_t, N>(previousMax * n / k);

			uint64_t previousMin = (*ret)(1, N(pegCount) - 1 - i);
			(*ret)(1, pegCount - 2 - i) = throwIfCastOverflow<uint64_t, N>(previousMin * (n - 1) / k);
		}
	}
	catch (const std::exception &)
	{
		delete ret;
		throw;
	}

	return ret;
}

template <typename N, typename T> void Calculate3Peg(N diskCount, Pegs<T>& pegs, T startPeg, T endPeg, MoveCollection<T>* moveCollection)
{
	T middlePeg =
		(pegs[0] == startPeg) || (pegs[0] == endPeg)
		? (pegs[1] == startPeg) || (pegs[1] == endPeg)
		? pegs[2]
		: pegs[1]
		: pegs[0];

	size_t count = 0;
	moveCollection->Add(startPeg, diskCount % 2 == 1 ? endPeg : middlePeg);
	for (N i = 1; i < diskCount; i++)
	{
		T tempPeg = (diskCount - i) % 2 == 1 ? endPeg : middlePeg;
		count = count * 2 + 1;
		moveCollection->Add(startPeg, tempPeg);
		moveCollection->InverseMoves(tempPeg, moveCollection->GetSubSegment(count + 1, count), 1 + count);
	}

	delete& pegs;
	if (!moveCollection->IsParent())
		delete moveCollection;
}
template <typename N, typename T> std::future<void> Calculate3PegAsync(N diskCount, Pegs<T>& pegs, T startPeg, T endPeg, MoveCollection<T>* moveCollection)
{
	return std::async(static_cast<void (*)(N diskCount, Pegs<T> & pegs, T startPeg, T endPeg, MoveCollection<T> * moveCollection)>(Calculate3Peg), diskCount, std::ref(pegs), startPeg, endPeg, moveCollection);
}

template <typename N, typename T> void Calculate4PlusPeg(T pegCount, N diskCount, Pegs<T>& pegs, T startPeg, T endPeg, MoveCollection<T>* moveCollection, size_t moveCount)
{
	TowerHeights<N, T>* towerHeights;
	try
	{
		towerHeights = GetTowerHeights<N, T>(pegCount, CalculateIncrement(pegCount, diskCount));
	}
	catch (const std::exception &)
	{
		delete& pegs;
		delete moveCollection;
		throw;
	}

	N* heightsToBuild = new N[pegCount - 2];
	N tempDisk = diskCount - 1;

	for (T i = 0; i < pegCount - 2; i++)
	{
		heightsToBuild[i] = towerHeights->operator()(1, i);
		tempDisk -= towerHeights->operator()(1, i);
	}
	for (T i = 0; i < pegCount - 2; i++)
	{
		if (towerHeights->operator()(0, i) - towerHeights->operator()(1, i) < tempDisk)
		{
			heightsToBuild[i] = towerHeights->operator()(0, i);
			tempDisk -= towerHeights->operator()(0, i) - towerHeights->operator()(1, i);
		}
		else
		{
			heightsToBuild[i] += tempDisk;
			break;
		}
	}
	delete towerHeights;

	std::vector<T>* alreadyUsed = new std::vector<T>;
	auto GetPegArray = [&]() -> Pegs<T>&
	{
		T count = 0;
		Pegs<T>* ret = new Pegs<T>(pegs.Count() - T(alreadyUsed->size()));
		for (T i = 0; i < pegs.Count(); i++)
		{
			if (!(std::find(alreadyUsed->begin(), alreadyUsed->end(), pegs[i]) != alreadyUsed->end()))
			{
				ret->operator[](count++) = pegs[i];
			}
		}
		return *ret;
	};
	auto GetEndPeg = [&](Pegs<T>& towerPegs) -> T
	{
		if (alreadyUsed->size() != pegs.Count() - 2)
		{
			T peg =
				towerPegs[0] != endPeg && towerPegs[0] != startPeg
				? towerPegs[0]
				: towerPegs[1] != endPeg && towerPegs[1] != startPeg
				? towerPegs[1]
				: towerPegs[2];
			alreadyUsed->push_back(peg);
			return peg;
		}
		else
			return endPeg;
	};


	std::vector<std::future<void>>* buildMoveTasks = new std::vector<std::future<void>>();
	try
	{
		size_t tempBase = 0;
		for (T i = 0; i < pegs.Count() - 2; i++)
		{
			if (heightsToBuild[i] != 0)
			{
				Pegs<T>& pegArray = GetPegArray();
				T end = GetEndPeg(pegArray);
				T internPegCount = pegs.Count() - i;
				size_t internMoveCount = CalculateMoveCount(internPegCount, heightsToBuild[i]);					//Overflow add test 
				MoveCollection<T>* internMoveCollection = moveCollection->GetSubSegment(tempBase, internMoveCount);
				buildMoveTasks->push_back(CalculateMoveSequenceAsync<N, T>(internPegCount, heightsToBuild[i], pegArray, 0, end, internMoveCollection, internMoveCount));
				tempBase += internMoveCount;
			}
		}
	}
	catch (const std::exception & exception)
	{
		delete buildMoveTasks;
		delete alreadyUsed;
		delete[] heightsToBuild;
		throw exception;
	}


	delete[] heightsToBuild;
	delete alreadyUsed;
	delete& pegs;

	try
	{
		for (auto&& item : *buildMoveTasks)
			item.get();
	}
	catch (const std::exception & exception)
	{
		delete buildMoveTasks;
		delete moveCollection;
		throw exception;
	}
	delete buildMoveTasks;

	moveCollection->Add(startPeg, endPeg);

	size_t base = (moveCount + 1) / 2;
	size_t count = base - 1;
	moveCollection->InverseMoves(endPeg, moveCollection->GetSubSegment(base, count), 1 + size_t(count));

	if (!moveCollection->IsParent())
		delete moveCollection;
} //Overflow test
template <typename N, typename T> std::future<void> Calculate4PlusPegAsync(T pegCount, N diskCount, Pegs<T>& pegs, T startPeg, T endPeg, MoveCollection<T>* moveCollection, size_t moveCount)
{
	return std::async(static_cast<void (*)(T, N, Pegs<T>&, T, T, MoveCollection<T>*, size_t)>(Calculate4PlusPeg), pegCount, diskCount, std::ref(pegs), startPeg, endPeg, moveCollection, moveCount);
}