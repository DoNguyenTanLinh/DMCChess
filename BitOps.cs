using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DMCChess
{
    //http://chessprogramming.wikispaces.com/BitScan

    static class BitOps
    {
        private static byte[] DeBrujinTable = new byte[64]{
            0, 47,  1, 56, 48, 27,  2, 60,
           57, 49, 41, 37, 28, 16,  3, 61,
           54, 58, 35, 52, 50, 42, 21, 44,
           38, 32, 29, 23, 17, 11,  4, 62,
           46, 55, 26, 59, 40, 36, 15, 53,
           34, 51, 20, 43, 31, 22, 10, 45,
           25, 39, 14, 33, 19, 30,  9, 24,
           13, 18,  8, 12,  7,  6,  5, 63
        };

        private const UInt64 debruijn64 = 0x03f79d71b4cb0a89UL;

        const UInt64 notAFile = 0xfefefefefefefefeUL;
        const UInt64 notHFile = 0x7f7f7f7f7f7f7f7fUL;

        private const UInt64 M1 = 0x5555555555555555UL;  // 1 zero,  1 one ...
        private const UInt64 M2 = 0x3333333333333333UL;  // 2 zeros,  2 ones ...
        private const UInt64 M4 = 0x0f0f0f0f0f0f0f0fUL;  // 4 zeros,  4 ones ...
        private const UInt64 M8 = 0x00ff00ff00ff00ffUL;  // 8 zeros,  8 ones ...
        private const UInt64 M16 = 0x0000ffff0000ffffUL;  // 16 zeros, 16 ones ...
        private const UInt64 M32 = 0x00000000ffffffffUL;  // 32 zeros, 32 ones

        public static UInt64 OneStepSouth(UInt64 b)     { return b << 8; }
        public static UInt64 OneStepNorth(UInt64 b)     { return b >> 8; }
        public static UInt64 OneStepEast(UInt64 b)      { return (b << 1) & notAFile; }
        public static UInt64 OneStepWest(UInt64 b)      { return (b >> 1) & notHFile; }

        public static UInt64 OneStepNorthEast(UInt64 b) { return (b >> 7) & notAFile; }
        public static UInt64 OneStepSouthEast(UInt64 b) { return (b << 9) & notAFile; }
        public static UInt64 OneStepSouthWest(UInt64 b) { return (b << 7) & notHFile; }
        public static UInt64 OneStepNorthWest(UInt64 b) { return (b >> 9) & notHFile; }

        //is this index at this position a 1?
        internal static bool IsBitSet(UInt64 bb, byte posBit)
        {
            return (bb & (1UL << posBit)) != 0;
        }

        //A bitscan forward that turns the least significant 1 bit into a 0
        internal static byte BitScanForwardReset(ref UInt64 bb)
        {
            byte bitIndex = DeBrujinTable[((bb ^ (bb - 1)) * debruijn64) >> 58];
            bb &= bb - 1;
            return bitIndex;
        }

        //A bitscan forward is used to find the index of the least significant 1 bit
        public static byte bitScanForward(UInt64 bb)
        {
            //assert(bb != 0);
            return DeBrujinTable[((bb ^ (bb - 1)) * debruijn64) >> 58];
        }

        /**
         * bitScanReverse
         * @authors Kim Walisch, Mark Dickinson
         * @param bb Board to scan
         * @precondition bb != 0
         * @return index (0..63) of most significant one bit
         */
        public static byte bitScanReverse(UInt64 bb)
        {
            //assert(bb != 0);
            bb |= bb >> 1;
            bb |= bb >> 2;
            bb |= bb >> 4;
            bb |= bb >> 8;
            bb |= bb >> 16;
            bb |= bb >> 32;
            return DeBrujinTable[(bb * debruijn64) >> 58];
        }

        //adds up all the 1's in the Board
        public static byte BitCountWegner(UInt64 bb)
        {
            byte count = 0;
            while (bb != 0){
                count++;
                bb &= bb - 1; // reset LS1B
            }
            return count;
        }

        //unused alternative to Wegner (this is slower, I've tested it under the same conditions)
        internal static int PopCount(UInt64 bb)
        {
            bb = (bb & M1)  + ((bb >> 1) & M1);   //put count of each  2 bits into those  2 bits
            bb = (bb & M2)  + ((bb >> 2) & M2);   //put count of each  4 bits into those  4 bits
            bb = (bb & M4)  + ((bb >> 4) & M4);   //put count of each  8 bits into those  8 bits
            bb = (bb & M8)  + ((bb >> 8) & M8);   //put count of each 16 bits into those 16 bits
            bb = (bb & M16) + ((bb >> 16) & M16);   //put count of each 32 bits into those 32 bits
            bb = (bb & M32) + ((bb >> 32) & M32);   //put count of each 64 bits into those 64 bits
            return (int)bb;
        }

        // reverse byte order (64-bit)
        public static UInt64 ReverseBytes(UInt64 value)
        {
            return (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
                   (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
                   (value & 0x000000FF00000000UL) >> 8  | (value & 0x0000FF0000000000UL) >> 24 |
                   (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
        }

        public static UInt64 randomUInt64(Random rand)
        {
            var buffer = new byte[sizeof(UInt64)];
            rand.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }

        public static int GetPieceAttackScore(UInt64 attackSet, UInt64 attackedPieces, int attackedPieceValue)
        {
            return attackedPieceValue*BitCountWegner(attackSet & attackedPieces);
        }

    }
}
