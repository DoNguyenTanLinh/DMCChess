using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    class King
    {
        public static UInt64[] KingAttacks;

        internal static UInt64 GetAllTargets(UInt64 king, bool pieceColor, Board board)
        {
            UInt64 kingMoves = KingAttacks[(BitOps.bitScanForward(king))];
            
            //Cannot castle if already castled or is in check
            UInt64 empty;
            if (pieceColor && !board.playerHasCastled && !board.isCheck)
            {
                empty = board.GetEmptySquares();
                //Right castle
                if (BitOps.IsBitSet(empty, 61) && BitOps.IsBitSet(empty, 62) && BitOps.IsBitSet(board.WR, 63))
                {
                    kingMoves |= (1UL << 62);
                }
                //Left castle
                else if (BitOps.IsBitSet(empty, 59) && BitOps.IsBitSet(empty, 58) && BitOps.IsBitSet(empty, 57) && BitOps.IsBitSet(board.WR, 56))
                {
                    kingMoves |= (1UL << 58);
                }
            }
            else if (!pieceColor && !board.computerHasCastled && !board.isCheck)
            {
                empty = board.GetEmptySquares();
                //Right castle
                if (BitOps.IsBitSet(empty, 5) && BitOps.IsBitSet(empty, 6) && BitOps.IsBitSet(board.BR, 7))
                {
                    kingMoves |= (1UL << 6);
                }
                //Left castle
                else if (BitOps.IsBitSet(empty, 3) && BitOps.IsBitSet(empty, 2) && BitOps.IsBitSet(empty, 1) && BitOps.IsBitSet(board.BR, 0))
                {
                    kingMoves |= (1UL << 2);
                }
            }

            return kingMoves & ~board.GetColorPieces(pieceColor);
        }

        internal static void InitKingAttacks()
        {
            KingAttacks = new UInt64[64];
            for (byte sq = 0; sq < 64; sq++)
            {
                KingAttacks[sq] = GetKingAttacks(1UL << sq);
            }
        }

        public static UInt64 GetKingAttacks(UInt64 king)
        {
            UInt64 attacks = BitOps.OneStepEast(king) | BitOps.OneStepWest(king);
            king |= attacks;
            attacks |= BitOps.OneStepNorth(king) | BitOps.OneStepSouth(king);
            return attacks;
        }      

    }
}
