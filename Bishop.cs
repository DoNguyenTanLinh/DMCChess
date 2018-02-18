using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    class Bishop
    {
        internal static UInt64 GetAllTargets(UInt64 bishops, bool pieceColor, Board board)
        {
            //UInt64 targets = 0UL;
            //UInt64 occ = board.GetOccupiedSquares();
            //while (bishops != 0UL)
            //    targets = Magic.Bmagic(BitOps.BitScanForwardReset(ref bishops), occ);

            //UInt64 targets = 0UL;
            //UInt64 occ = board.GetOccupiedSquares();
            //while (bishops != 0UL)
            //    targets = RayAttacks.bishopAttacks(occ, BitOps.BitScanForwardReset(ref bishops));

            //UInt64 targets = 0UL;
            //UInt64 occ = board.GetOccupiedSquares();
            //while (bishops != 0UL)
            //    targets = Quintessence.bishopAttacks(occ, BitOps.BitScanForwardReset(ref bishops));

            UInt64 targets = KoggeStone.bishopAttacks(bishops, board.GetEmptySquares());

            return targets & ~board.GetColorPieces(pieceColor);
        }

    }
}
