using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{


    public class Constants
    {
        public struct Ranks
        {
            public const UInt64 One   = 0xFF00000000000000UL;
            public const UInt64 Two   = 0xFF000000000000UL;
            public const UInt64 Three = 0xFF0000000000UL;
            public const UInt64 Four  = 0xFF00000000UL;
            public const UInt64 Five  = 0xFF000000UL;
            public const UInt64 Six   = 0xFF0000UL;
            public const UInt64 Seven = 0xFF00UL;
            public const UInt64 Eight = 0xFFUL;
        }

        public struct Files //These are wrong but unused. Maybe because they are little endian
        {
            public const UInt64 One   = 8080808080808080UL;
            public const UInt64 Two   = 4040404040404040UL;
            public const UInt64 Three = 2020202020202020UL;
            public const UInt64 Four  = 1010101010101010UL;
            public const UInt64 Five  = 808080808080808UL;
            public const UInt64 Six   = 404040404040404UL;
            public const UInt64 Seven = 202020202020202UL;
            public const UInt64 Eight = 101010101010101UL;
        }

        public const int DEFAULT_MOVE_LIST_SIZE = 20; //There will be an average of 20 moves. This makes about 1% difference

        public const short MATE_SCORE = short.MaxValue; //higher than any possible score
        public const short MIN_SCORE = short.MinValue;

        //https://chessprogramming.wikispaces.com/Point+Value
        //Piece values from Larry Kaufman
        public const short VALUE_PAWN = 100;
        public const short VALUE_KNIGHT = 345;
        public const short VALUE_BISHOP = 350;
        public const short VALUE_ROOK = 525;
        public const short VALUE_QUEEN = 1000;
        public const short VALUE_KING = 5000;

        //Made up
        public const float MOBILITY_WEIGHT = 1;
        public const float ATTACKING_WEIGHT = 0.3f;
        public const short CASTLING_BONUS = 80;
        public const short CHECK_BONUS = 50;

        public const short ATTACK_PAWN = 9;
        public const short ATTACK_KNIGHT = 7;
        public const short ATTACK_BISHOP = 7;
        public const short ATTACK_CASTLE = 5;
        public const short ATTACK_QUEEN = 1;
        public const short ATTACK_KING = 1;

        public static short GetPieceWorth(PieceType pieceType)
        {
            switch (pieceType)
            {
                case PieceType.Pawn:
                    return VALUE_PAWN;
                case PieceType.Rook:
                    return VALUE_ROOK;
                case PieceType.Knight:
                    return VALUE_KNIGHT;
                case PieceType.Bishop:
                    return VALUE_BISHOP;
                case PieceType.Queen:
                    return VALUE_QUEEN;
                case PieceType.King:
                    return VALUE_KING;
            }
            return 0;
        }

        public static short GetPieceAttack(PieceType pieceType)
        {
            switch (pieceType)
            {
                case PieceType.Pawn:
                    return ATTACK_PAWN;
                case PieceType.Rook:
                    return ATTACK_CASTLE;
                case PieceType.Knight:
                    return ATTACK_KNIGHT;
                case PieceType.Bishop:
                    return ATTACK_BISHOP;
                case PieceType.Queen:
                    return ATTACK_QUEEN;
                case PieceType.King:
                    return ATTACK_KING;
            }
            return 0;
        }

        //Little endian system
        public static byte IndexToX(byte index)
        {
            return (byte)(index % 8);
        }

        public static byte IndexToY(byte index)
        {
            return (byte)(index / 8);
        }

        public static byte XYToIndex(byte x, byte y)
        {
            return (byte)((y * 8) + x);
        }
    }
}
