using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    class Knight
    {
        public static UInt64[] KnightAttacks;

        internal static UInt64 GetAllTargets(UInt64 knights, bool pieceColor, Board board)
        {
            UInt64 targets = 0UL;

            while (knights != 0)
            {
                // accede all'array di mosse precalcolate cercando il primo bit attivo all'interno della Board
                targets |= KnightAttacks[(BitOps.BitScanForwardReset(ref knights))];
            }

            return targets & ~board.GetColorPieces(pieceColor);
        }

        internal static void InitKnightAttacks()
        {
            KnightAttacks = new UInt64[64];
            // inizializza l'array di mosse precalcolate
            for (byte sq = 0; sq < 64; sq++)
            {
                KnightAttacks[sq] = GetKnightAttacks(1UL << sq);
            }
        }

        private static UInt64 GetKnightAttacks(UInt64 knights)
        {
            UInt64 west, east, attacks;
            east = BitOps.OneStepEast(knights);
            west = BitOps.OneStepWest(knights);
            attacks = (east | west) << 16;
            attacks |= (east | west) >> 16;
            east = BitOps.OneStepEast(east);
            west = BitOps.OneStepWest(west);
            attacks |= (east | west) << 8;
            attacks |= (east | west) >> 8;

            return attacks;
        }

    }
}
