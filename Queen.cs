using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    class Queen
    {
        internal static UInt64 GetAllTargets(UInt64 queens, bool pieceColor, Board board)
        {
            //UInt64 targets = 0UL;
            //UInt64 occ = board.GetOccupiedSquares();
            //while (queens != 0UL)
            //    targets = Magic.Qmagic(BitOps.BitScanForwardReset(ref queens), occ);

            //UInt64 targets = 0UL;
            //UInt64 occ = board.GetOccupiedSquares();
            //while (queens != 0UL)
            //    targets = RayAttacks.queenAttacks(occ, BitOps.BitScanForwardReset(ref queens));

            //UInt64 targets = 0UL;
            //UInt64 occ = board.GetOccupiedSquares();
            //while (queens != 0UL)
            //    targets = Quintessence.queenAttacks(occ, BitOps.BitScanForwardReset(ref queens));

            UInt64 empty = board.GetEmptySquares();
            UInt64 targets = KoggeStone.rookAttacks(queens, empty) | KoggeStone.bishopAttacks(queens, empty);

            return targets & ~board.GetColorPieces(pieceColor);
        }
    }
}
