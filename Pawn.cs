using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    class Pawn
    {
        internal static UInt64 GetAllTargets(UInt64 pawns, bool pieceColor, Board board)
        {
            return GetQuietTargets(pieceColor, pawns, board.GetEmptySquares()) | GetAnyAttack(pieceColor, pawns, board);
        }

        private static UInt64 GetQuietTargets(bool isPlayer, UInt64 pawns, UInt64 empty)
        {
            return GetSinglePushTargets(isPlayer, pawns, empty) | GetDoublePushTargets(isPlayer, pawns, empty);
        }

        private static UInt64 GetSinglePushTargets(bool isPlayer, UInt64 pawns, UInt64 empty)
        {
            if (isPlayer)
                return BitOps.OneStepNorth(pawns) & empty;
            else
                return BitOps.OneStepSouth(pawns) & empty;
        }

        private static UInt64 GetDoublePushTargets(bool isPlayer, UInt64 pawns, UInt64 empty)
        {
            UInt64 singlePush = GetSinglePushTargets(isPlayer, pawns, empty);
            if (isPlayer)
                return BitOps.OneStepNorth(singlePush) & empty & Constants.Ranks.Four;
            else
                return BitOps.OneStepSouth(singlePush) & empty & Constants.Ranks.Five;
        }

        private static UInt64 GetPawnsAbleToSinglePush(bool isPlayer, UInt64 pawns, UInt64 empty)
        {
            if (isPlayer)
                return BitOps.OneStepSouth(empty) & pawns;
            else
                return BitOps.OneStepNorth(empty) & pawns;
        }

        private static UInt64 GetPawnsAbleToDoublePush(bool isPlayer, UInt64 pawns, UInt64 empty)
        {
            if (isPlayer)
            {
                UInt64 emptyRank3 = BitOps.OneStepSouth(empty & Constants.Ranks.Four) & empty;
                return GetPawnsAbleToSinglePush(isPlayer, pawns, emptyRank3);
            }
            else
            {
                UInt64 emptyRank6 = BitOps.OneStepNorth(empty & Constants.Ranks.Six) & empty;
                return GetPawnsAbleToSinglePush(isPlayer, pawns, emptyRank6);
            }
        }

        private static UInt64 GetEastAttacks(bool isPlayer, UInt64 pawns)
        {
            if (isPlayer)
                return BitOps.OneStepNorthEast(pawns);
            else
                return BitOps.OneStepSouthEast(pawns);
        }

        private static UInt64 GetWestAttacks(bool isPlayer, UInt64 pawns)
        {
            if (isPlayer)
                return BitOps.OneStepNorthWest(pawns);
            else
                return BitOps.OneStepSouthWest(pawns);
        }

        private static UInt64 GetAnyAttack(bool isPlayer, UInt64 pawns, Board board)
        {
            return (GetEastAttacks(isPlayer, pawns) | GetWestAttacks(isPlayer, pawns)) & board.GetColorPieces(!isPlayer);
        }
    }
}
