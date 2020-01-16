#pragma once

#include "Constants.h"

#include "move.h"
#include "Pegs.h"
#include <future>

template <typename N, typename T> std::future<MoveCollection<T>*> CalculateMoveSequenceAsync(T pegCount, N diskCount);
template <typename N, typename T> MoveCollection<T>* CalculateMoveSequence(T pegCount, N diskCount);
template <typename N, typename T> std::future<void> CalculateMoveSequenceAsync(T pegCount, N diskCount, Pegs<T>& pegs, T startPeg, T endPeg, MoveCollection<T>* moveCollection, size_t moveCount);
template <typename N, typename T> TowerHeights<N, T>* GetTowerHeights(T pegCount, uint64_t increment);
template <typename N, typename T> void Calculate3Peg(N diskCount, Pegs<T>& pegs, T startPeg, T endPeg, MoveCollection<T>* moveCollection);
template <typename N, typename T> std::future<void> Calculate3PegAsync(N diskCount, Pegs<T>& pegs, T startPeg, T endPeg, MoveCollection<T>* moveCollection);
template <typename N, typename T> void Calculate4PlusPeg(T pegCount, N diskCount, Pegs<T>& pegs, T startPeg, T endPeg, MoveCollection<T>* moveCollection, size_t moveCount);
template <typename N, typename T> std::future<void> Calculate4PlusPegAsync(T pegCount, N diskCount, Pegs<T>& pegs, T startPeg, T endPeg, MoveCollection<T>* moveCollection, size_t moveCount);

template MOVESEQUENCE_API struct Move<uint16_t>;
template MOVESEQUENCE_API class MoveCollection<uint16_t>;
template MOVESEQUENCE_API std::ostream& operator<<(std::ostream& stream, const MoveCollection<uint16_t>& moveCollection);
template MOVESEQUENCE_API MoveCollection<uint16_t>* CalculateMoveSequence(uint16_t, uint32_t);
template MOVESEQUENCE_API std::future<MoveCollection<uint16_t>*> CalculateMoveSequenceAsync(uint16_t, uint32_t);

template MOVESEQUENCE_API struct Move<uint32_t>;
template MOVESEQUENCE_API class MoveCollection<uint32_t>;
template MOVESEQUENCE_API std::ostream& operator<<(std::ostream& stream, const MoveCollection<uint32_t>& moveCollection);
template MOVESEQUENCE_API MoveCollection<uint32_t>* CalculateMoveSequence(uint32_t, uint32_t);
template MOVESEQUENCE_API std::future<MoveCollection<uint32_t>*> CalculateMoveSequenceAsync(uint32_t, uint32_t);