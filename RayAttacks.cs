using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Note: It was found this was consistently slightly slower than the Kogge-Stone flood algorithm
//So, this class is not used :(

namespace DMCChess
{
    class RayAttacks
    {
        //private enum RayDir
        //{
        //    nort = 0,
        //    sout = 1,
        //    east = 2,
        //    west = 3,
        //    noEa = 4,
        //    soWe = 5,
        //    noWe = 6,
        //    soEa = 7
        //};

        private static UInt64[,] rayAttacks = new UInt64[8,64];

        public static void initRayAttacks()
        {
            UInt64 rayAttack;
            for (byte i = 0; i< 64; i++)
            {
                UInt64 square = 1UL << i;

                //nort                
                rayAttack = BitOps.OneStepNorth(square);
                rayAttack |= BitOps.OneStepNorth(rayAttack);
                rayAttack |= BitOps.OneStepNorth(rayAttack);
                rayAttack |= BitOps.OneStepNorth(rayAttack);
                rayAttack |= BitOps.OneStepNorth(rayAttack);
                rayAttack |= BitOps.OneStepNorth(rayAttack);
                rayAttack |= BitOps.OneStepNorth(rayAttack);
                rayAttacks[0, i] = rayAttack;

                //sout
                rayAttack = BitOps.OneStepSouth(square);
                rayAttack |= BitOps.OneStepSouth(rayAttack);
                rayAttack |= BitOps.OneStepSouth(rayAttack);
                rayAttack |= BitOps.OneStepSouth(rayAttack);
                rayAttack |= BitOps.OneStepSouth(rayAttack);
                rayAttack |= BitOps.OneStepSouth(rayAttack);
                rayAttack |= BitOps.OneStepSouth(rayAttack);
                rayAttacks[1, i] = rayAttack;

                //east
                rayAttack = BitOps.OneStepEast(square);
                rayAttack |= BitOps.OneStepEast(rayAttack);
                rayAttack |= BitOps.OneStepEast(rayAttack);
                rayAttack |= BitOps.OneStepEast(rayAttack);
                rayAttack |= BitOps.OneStepEast(rayAttack);
                rayAttack |= BitOps.OneStepEast(rayAttack);
                rayAttack |= BitOps.OneStepEast(rayAttack);
                rayAttacks[2, i] = rayAttack;

                //west
                rayAttack = BitOps.OneStepWest(square);
                rayAttack |= BitOps.OneStepWest(rayAttack);
                rayAttack |= BitOps.OneStepWest(rayAttack);
                rayAttack |= BitOps.OneStepWest(rayAttack);
                rayAttack |= BitOps.OneStepWest(rayAttack);
                rayAttack |= BitOps.OneStepWest(rayAttack);
                rayAttack |= BitOps.OneStepWest(rayAttack);
                rayAttacks[3, i] = rayAttack;

                //noEa
                rayAttack = BitOps.OneStepNorthEast(square);
                rayAttack |= BitOps.OneStepNorthEast(rayAttack);
                rayAttack |= BitOps.OneStepNorthEast(rayAttack);
                rayAttack |= BitOps.OneStepNorthEast(rayAttack);
                rayAttack |= BitOps.OneStepNorthEast(rayAttack);
                rayAttack |= BitOps.OneStepNorthEast(rayAttack);
                rayAttack |= BitOps.OneStepNorthEast(rayAttack);
                rayAttacks[4, i] = rayAttack;

                //soWe
                rayAttack = BitOps.OneStepSouthWest(square);
                rayAttack |= BitOps.OneStepSouthWest(rayAttack);
                rayAttack |= BitOps.OneStepSouthWest(rayAttack);
                rayAttack |= BitOps.OneStepSouthWest(rayAttack);
                rayAttack |= BitOps.OneStepSouthWest(rayAttack);
                rayAttack |= BitOps.OneStepSouthWest(rayAttack);
                rayAttack |= BitOps.OneStepSouthWest(rayAttack);
                rayAttacks[5, i] = rayAttack;

                //noWe
                rayAttack = BitOps.OneStepNorthWest(square);
                rayAttack |= BitOps.OneStepNorthWest(rayAttack);
                rayAttack |= BitOps.OneStepNorthWest(rayAttack);
                rayAttack |= BitOps.OneStepNorthWest(rayAttack);
                rayAttack |= BitOps.OneStepNorthWest(rayAttack);
                rayAttack |= BitOps.OneStepNorthWest(rayAttack);
                rayAttack |= BitOps.OneStepNorthWest(rayAttack);
                rayAttacks[6, i] = rayAttack;

                //soEa
                rayAttack = BitOps.OneStepSouthEast(square);
                rayAttack |= BitOps.OneStepSouthEast(rayAttack);
                rayAttack |= BitOps.OneStepSouthEast(rayAttack);
                rayAttack |= BitOps.OneStepSouthEast(rayAttack);
                rayAttack |= BitOps.OneStepSouthEast(rayAttack);
                rayAttack |= BitOps.OneStepSouthEast(rayAttack);
                rayAttack |= BitOps.OneStepSouthEast(rayAttack);
                rayAttacks[7, i] = rayAttack;
            }
        }

        /*
        East                Sout                 SoEa                SoWe
        . . . . . . . .     . . . . . . . .      . . . . . . . .     . . . . . . . .
        . . . . . . . .     . . . . . . . .      . . . . . . . .     . . . . . . . .
        . . . . . . . .     . . . . . . . .      . . . . . . . .     . . . . . . . .
        . . . . . . . .     . . . . . . . .      . . . . . . . .     . . . . . . . .
        . . . R 1 1 1 1     . . . R . . . .      . . . B . . . .     . . . B . . . .
        . . . . . . . .     . . . 1 . . . .      . . . . 1 . . .     . . 1 . . . . .
        . . . . . . . .     . . . 1 . . . .      . . . . . 1 . .     . 1 . . . . . .
        . . . . . . . .     . . . 1 . . . .      . . . . . . 1 .     1 . . . . . . .
        */ 
        private static UInt64 getPositiveRayAttacks(UInt64 occupied, int rayDir, byte square)
        {
           UInt64 attacks = rayAttacks[rayDir,square];
           UInt64 blocker = attacks & occupied;
           if (blocker != 0UL) {
              square = BitOps.bitScanForward(blocker);
              attacks ^= rayAttacks[rayDir,square];
           }
           return attacks;
        }

        /*
        West                Nort                 NoWe                NoEa
        . . . . . . . .     . . . 1 . . . .      . . . . . . . .     . . . . . . . 1
        . . . . . . . .     . . . 1 . . . .      1 . . . . . . .     . . . . . . 1 .
        . . . . . . . .     . . . 1 . . . .      . 1 . . . . . .     . . . . . 1 . .
        . . . . . . . .     . . . 1 . . . .      . . 1 . . . . .     . . . . 1 . . .
        1 1 1 R . . . .     . . . R . . . .      . . . B . . . .     . . . B . . . .
        . . . . . . . .     . . . . . . . .      . . . . . . . .     . . . . . . . .
        . . . . . . . .     . . . . . . . .      . . . . . . . .     . . . . . . . .
        . . . . . . . .     . . . . . . . .      . . . . . . . .     . . . . . . . .
         */
        private static UInt64 getNegativeRayAttacks(UInt64 occupied, int rayDir, byte square)
        {
           UInt64 attacks = rayAttacks[rayDir,square];
           UInt64 blocker = attacks & occupied;
           if (blocker != 0UL) {
              square = BitOps.bitScanReverse(blocker);
              attacks ^= rayAttacks[rayDir,square];
           }
           return attacks;
        }

        private static UInt64 fileAttacks(UInt64 occ, byte sq)
        {
            return getPositiveRayAttacks(occ, 1, sq)/*south*/ | getNegativeRayAttacks(occ, 0, sq); /*north*/
        }

        private static UInt64 rankAttacks(UInt64 occ, byte sq)
        {
            return getPositiveRayAttacks(occ, 2, sq)/*east*/ | getNegativeRayAttacks(occ, 3, sq); /*west*/
        }

        private static UInt64 diagonalAttacks(UInt64 occ, byte sq)
        {
            return getPositiveRayAttacks(occ, 5, sq)/*sowe*/ | getNegativeRayAttacks(occ, 4, sq); /*noea*/
        }

        private static UInt64 antiDiagAttacks(UInt64 occ, byte sq)
        {
            return getPositiveRayAttacks(occ, 7, sq)/*soea*/ | getNegativeRayAttacks(occ, 6, sq); /*nowe*/
        }

        //Public
        public static UInt64 rookAttacks(UInt64 occ, byte sq)
        {
          return fileAttacks(occ, sq) | rankAttacks(occ, sq); // ^ +
        }

        public static UInt64 bishopAttacks(UInt64 occ, byte sq)
        {
          return diagonalAttacks(occ, sq) | antiDiagAttacks(occ, sq); // ^ +
        }

        public static UInt64 queenAttacks(UInt64 occ, byte sq)
        {
          return rookAttacks  (occ, sq) | bishopAttacks(occ, sq); // ^ +
        }

    }
}
