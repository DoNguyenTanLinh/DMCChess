using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    class Rook
    {
        internal static UInt64 GetAllTargets(UInt64 rooks, bool pieceColor, Board board)
        {
            //UInt64 targets = 0UL;
            //UInt64 occ = board.GetOccupiedSquares();
            //while (rooks != 0UL)
            //    targets = Magic.Rmagic(BitOps.BitScanForwardReset(ref rooks), occ);

            //UInt64 targets = 0UL;
            //UInt64 occ = board.GetOccupiedSquares();
            //while (rooks != 0UL)
            //    targets = RayAttacks.rookAttacks(occ, BitOps.BitScanForwardReset(ref rooks));

            //UInt64 targets = 0UL;
            //UInt64 occ = board.GetOccupiedSquares();
            //while (rooks != 0UL)
            //    targets = Quintessence.rookAttacks(occ, BitOps.BitScanForwardReset(ref rooks));

            UInt64 targets = KoggeStone.rookAttacks(rooks, board.GetEmptySquares());

            return targets & ~board.GetColorPieces(pieceColor);
        }
    }
}
