using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Molytho.TowerOfHanoi.Game
{
    public struct TowerOfHanoiConfiguration
    {
        private readonly Peg[] pegs;
        private readonly uint _diskCount;
        private readonly ushort _pegCount;
        public TowerOfHanoiConfiguration(uint diskCount, ushort pegCount)
        {
            _diskCount = diskCount;
            _pegCount = pegCount;

            pegs = new Peg[pegCount];
            pegs[0] = Peg.GetNewFull(diskCount);
            for(ushort i = 1; i < pegCount; i++)
                pegs[i] = new Peg(diskCount);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetDiskCountOnPeg(ushort index) => pegs[index].DiskCount;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetDiskSizeOnPeg(ushort index) => pegs[index].TopDiskSize;

        public uint[] this[ushort index] => pegs[index].GetDisksSizes();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool VerifyMove(ushort startIndex, ushort endIndex)
        {
            return pegs[startIndex].TopDiskSize > pegs[endIndex].TopDiskSize;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool VerifyMove(Move move)
        {
            return pegs[move.StartPeg].TopDiskSize > pegs[move.EndPeg].TopDiskSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyMove(ushort startIndex, ushort endIndex)
        {
            if(!VerifyMove(startIndex,endIndex))
                throw new ArgumentException("peg bigger than top of other");

            pegs[endIndex].TopDisk = pegs[startIndex].TopDisk;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyMove(Move move)
        {
            if(!VerifyMove(move))
                throw new ArgumentException("peg bigger than top of other");

            pegs[move.StartPeg].TopDisk = pegs[move.EndPeg].TopDisk;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool VerifyMoveBackward(ushort startIndex, ushort endIndex)
        {
            return pegs[endIndex].TopDiskSize > pegs[startIndex].TopDiskSize;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool VerifyMoveBackward(Move move)
        {
            return pegs[move.EndPeg].TopDiskSize > pegs[move.StartPeg].TopDiskSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyMoveBackward(ushort startIndex, ushort endIndex)
        {
            if(!VerifyMoveBackward(startIndex,endIndex))
                throw new ArgumentException("peg bigger than top of other");

            pegs[startIndex].TopDisk = pegs[endIndex].TopDisk;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyMoveBackward(Move move)
        {
            if(!VerifyMoveBackward(move))
                throw new ArgumentException("peg bigger than top of other");

            pegs[move.EndPeg].TopDisk = pegs[move.StartPeg].TopDisk;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            pegs[0] = Peg.GetNewFull(_diskCount);
            for(ushort i = 1; i < _pegCount; i++)
                pegs[i] = new Peg(_diskCount);
        }
    }
    public struct Peg
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Peg GetNewFull(uint diskCount)
        {
            Disk?[] disks = new Disk?[diskCount];
            for(uint i = 0; i < diskCount; i++)
            {
                disks[i] = new Disk(diskCount - 1 - i);
            }
            return new Peg(disks);
        }

        public Peg(uint diskCount)
        {
            Disks = new Disk?[diskCount];
            DiskCount = 0;
        }
        public Peg(Disk?[] disks)
        {
            Disks = disks;
            DiskCount = (uint)disks.Length;
        }

        public Disk?[] Disks { get; }
        public uint DiskCount { get; private set; }

        public Disk TopDisk
        {
            get
            {
                Disk disk = Disks[DiskCount - 1].Value;
                Disks[DiskCount-- - 1] = null;

                return disk;
            }
            set
            {
                Disks[DiskCount++] = value;
            }
        }
        public uint TopDiskSize => DiskCount != 0 ? Disks[DiskCount - 1].Value.Size : 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint[] GetDisksSizes()
        {
            uint[] ret = new uint[DiskCount];
            for(int i = 0; i < DiskCount; i++)
            {
                ret[i] = Disks[i].Value.Size;
            }
            return ret;
        }
    }
    public readonly struct Disk
    {
        public Disk(uint size) => Size = size;
        public uint Size { get; }
    }
}
