using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    class Quintessence
    {
        //Ranks 
        private static UInt64[] RanksBoard = new UInt64[]
            {
                0xFFUL, 0xFF00UL, 0xFF0000UL, 0xFF000000UL, 0xFF00000000UL, 
                0xFF0000000000UL, 0xFF000000000000UL, 0xFF00000000000000UL
            };

        //Files 
        private static UInt64[] FilesBoard = new UInt64[]
            {
                0x101010101010101UL, 0x202020202020202UL, 0x404040404040404UL, 0x808080808080808UL,
                0x1010101010101010UL, 0x2020202020202020UL, 0x4040404040404040UL, 0x8080808080808080UL
            };

        //Diagonals
         private static UInt64[] DiagonalBoard = new UInt64[]
            {
	        0x1UL, 0x102UL, 0x10204UL, 0x1020408UL, 
            0x102040810UL, 0x10204081020UL, 0x1020408102040UL, 0x102040810204080UL, 
            0x204081020408000UL, 0x408102040800000UL, 0x810204080000000UL, 0x1020408000000000UL, 
            0x2040800000000000UL, 0x4080000000000000UL, 0x8000000000000000UL
            };

         //AntiDiagonals
         private static UInt64[] AntiDiagonalBoard = new UInt64[]
            {
	        0x80UL, 0x8040UL, 0x804020UL, 0x80402010UL, 
            0x8040201008UL, 0x804020100804UL, 0x80402010080402UL, 0x8040201008040201UL, 
            0x4020100804020100UL, 0x2010080402010000UL, 0x1008040201000000UL, 0x804020100000000UL, 
            0x402010000000000UL, 0x201000000000000UL, 0x100000000000000UL
        };

        private static UInt64[,] maskEx = new UInt64[64, 4];
        private static UInt64[] bitMask = new UInt64[64]; 

        public static void initQuintessence()
        {
            //UInt64 squareMask;
            for (int i = 0; i<64; i++)
            {                
                bitMask[i] = 1UL << i;
            }
        }

        public static UInt64 rookAttacks(UInt64 occ, int s)
        {
            UInt64 binaryS = bitMask[s];
	        UInt64 possibilitiesHorizontal = (occ - 2 * binaryS) ^ BitOps.ReverseBytes(BitOps.ReverseBytes(occ) - 2 * BitOps.ReverseBytes(binaryS));
            UInt64 possibilitiesVertical = ((occ & FilesBoard[s % 8]) - (2 * binaryS)) ^ BitOps.ReverseBytes(BitOps.ReverseBytes(occ & FilesBoard[s % 8]) - (2 * BitOps.ReverseBytes(binaryS)));
            return (possibilitiesHorizontal & RanksBoard[s / 8]) | (possibilitiesVertical & FilesBoard[s % 8]);
        }

        public static UInt64 bishopAttacks(UInt64 occ, int s)
        {
            UInt64 binaryS = bitMask[s];
            UInt64 possibilitiesDiagonal = ((occ & DiagonalBoard[(s / 8) + (s % 8)]) - (2 * binaryS)) ^ BitOps.ReverseBytes(BitOps.ReverseBytes(occ & DiagonalBoard[(s / 8) + (s % 8)]) - (2 * BitOps.ReverseBytes(binaryS)));
            UInt64 possibilitiesAntiDiagonal = ((occ & AntiDiagonalBoard[(s / 8) + 7 - (s % 8)]) - (2 * binaryS)) ^ BitOps.ReverseBytes(BitOps.ReverseBytes(occ & AntiDiagonalBoard[(s / 8) + 7 - (s % 8)]) - (2 * BitOps.ReverseBytes(binaryS)));
            return (possibilitiesDiagonal & DiagonalBoard[(s / 8) + (s % 8)]) | (possibilitiesAntiDiagonal & AntiDiagonalBoard[(s / 8) + 7 - (s % 8)]);
        }

        public static UInt64 queenAttacks(UInt64 occ, int sq)
        {
            return bishopAttacks(occ, sq) | rookAttacks(occ, sq);
        }

    }
}
