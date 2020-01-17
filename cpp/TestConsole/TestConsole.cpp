// TestConsole.cpp : Diese Datei enthält die Funktion "main". Hier beginnt und endet die Ausführung des Programms.
//
#include <iostream>
#include "MoveSequence.h"

using namespace std;

int main()
{
	uint32_t diskCount;
	cin >> diskCount;
	for (volatile uint64_t count = 0;; count++)
	{
		MoveCollection<uint16_t>* i;
		try
		{
			i = CalculateMoveSequence<uint32_t, uint16_t>(4, diskCount);
		}
		catch (const std::exception&)
		{
			throw;
		}
		delete i;
		cout << count << endl;
	}
}