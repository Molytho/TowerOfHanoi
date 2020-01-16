#include "move.h"

template <typename T>
MOVESEQUENCE_API std::ostream& operator<<(std::ostream& stream, const Move<T>& move)
{
	stream << "[" << move.startPeg << ", " << move.endPeg << "]";
	return stream;
}
template <typename T>
MOVESEQUENCE_API std::ostream& operator<<(std::ostream& stream, const MoveCollection<T>& moveCollection)
{
	for (size_t i = 0; i < moveCollection._count - 1; i++)
	{
		stream << moveCollection._array[i] << ", ";
	}
	stream << moveCollection._array[moveCollection._count - 1];

	return stream;
}
