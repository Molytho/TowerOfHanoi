#pragma once

#ifdef MATHEMATICFUNCTIONS_EXPORTS
#define MATHEMATICFUNCTIONS_API __declspec(dllexport)
#else
#define MATHEMATICFUNCTIONS_API __declspec(dllimport)
#endif // MATHEMATICFUNCTIONS_EXPORTS


#include <cstdint>
extern MATHEMATICFUNCTIONS_API uint64_t BinomialCoefficent(uint64_t increment, uint64_t peg);
extern MATHEMATICFUNCTIONS_API uint64_t CalculateIncrement(const uint64_t pegCount, const uint64_t diskCount);
extern MATHEMATICFUNCTIONS_API uint64_t CalculateMoveCount(const uint64_t pegCount, const uint64_t diskCount);