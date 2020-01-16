#include "MathematicFunctions.h"

#include <cstdint>

static uint64_t Pow(uint64_t base, uint64_t exponent)
{
	uint64_t temp = 1;
	for (uint64_t i = 1; i <= exponent; i++)
		temp *= base;
	return temp;
}

static uint64_t CalculateMoveCountIntern(const uint64_t increment, const uint64_t pegCount, const uint64_t diskCount)
{
	uint64_t temp = diskCount * Pow(2, increment - 1);

	for (uint64_t i = 1; i < increment + 1; i++)
	{
		temp += Pow(2, i - 1) * BinomialCoefficent(i + pegCount - 4, pegCount - 3);
	}
	for (uint64_t i = 1; i < increment + 1; i++)
	{
		temp -= Pow(2, increment - 1) * BinomialCoefficent(i + pegCount - 4, pegCount - 3);
	}

	return temp;
}

uint64_t BinomialCoefficent(uint64_t n, uint64_t k)
{
	if (2 * k > n) k = n - k;
	if (k == 0) return 1;
	if (k < 1) return 0;
	uint64_t ergebnis = 1;
	for (uint64_t i = 1; i <= k; i++)
	{
		ergebnis = ergebnis * (n - k + i) / i;
	}
	return ergebnis;
}

uint64_t CalculateIncrement(const uint64_t pegCount, const uint64_t diskCount)
{
	if (diskCount == 0) return 0;
	uint64_t increment = 0;
	uint64_t temp = 1;

	while (diskCount > temp)
	{
		increment++;
		temp = BinomialCoefficent(pegCount - 2 + increment, pegCount - 2);
	}

	return increment + 1;
}

uint64_t CalculateMoveCount(const uint64_t pegCount, const uint64_t diskCount)
{
	uint64_t ret;
	if (pegCount == 3)
	{
		ret = Pow(2, diskCount) - 1;
		return ret;
	}

	uint64_t increment;

	increment = CalculateIncrement(pegCount, diskCount);
	ret = CalculateMoveCountIntern(increment, pegCount, diskCount);

	return ret;
}

