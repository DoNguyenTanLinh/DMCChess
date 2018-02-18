using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    class Zobrist
    {
        public static UInt64[, ,] zArray = new UInt64[2, 6, 64];
        private static UInt64[] zCastle = new UInt64[2];
        private static UInt64[] zDepth = new UInt64[Algorithms.MAX_PLY];
        public static UInt64 zBlackMove;

        public static Dictionary<UInt64, ZobristEntry> boards = new Dictionary<UInt64, ZobristEntry>();
        public static Dictionary<UInt64, ZobristEntryCheck> checkBoards = new Dictionary<UInt64, ZobristEntryCheck>(); 
        public static uint BoardsFound = 0;
        public const ushort REMOVE_AGE = 10;
        //public static int counter = 0;

        public static void zobristFillArray() {
            Random rand = new Random();

            for (int color = 0; color < 2; color++)
            {
                for (int pieceType = 0; pieceType < 6; pieceType++)
                {
                    for (int square = 0; square < 64; square++)
                    {
                        zArray[color, pieceType, square] = BitOps.randomUInt64(rand);
                    }
                }
            }
            for (int i = 0; i < 2; i++)
            {
                zCastle[i] = BitOps.randomUInt64(rand);
            }
            for (int i = 0; i < Algorithms.MAX_PLY; i++)
            {
                zDepth[i] = BitOps.randomUInt64(rand);
            }
            zBlackMove = BitOps.randomUInt64(rand);
        }

        //Only called once at start of game
        public static UInt64 calculateZobristHash(Board board)
        {
            //Note: If its for the check dictionary, we don't include depth
            //This for remembering when a board is in check only

            UInt64 returnZKey = 0UL;
            for (byte square = 0; square < 64; square++)
            {
                if (((board.WP >> square) & 1UL) == 1)
                {
                    returnZKey ^= zArray[0,0,square];
                }
                else if (((board.BP >> square) & 1UL) == 1)
                {
                    returnZKey ^= zArray[1,0,square];
                }
                else if (((board.WN >> square) & 1UL) == 1)
                {
                    returnZKey ^= zArray[0,1,square];
                }
                else if (((board.BN >> square) & 1UL) == 1)
                {
                    returnZKey ^= zArray[1,1,square];
                }
                else if (((board.WB >> square) & 1UL) == 1)
                {
                    returnZKey ^= zArray[0,2,square];
                }

                else if (((board.BB >> square) & 1UL) == 1)
                {
                    returnZKey ^= zArray[1,2,square];
                }
                else if (((board.WR >> square) & 1UL) == 1)
                {
                    returnZKey ^= zArray[0,3,square];
                }
                else if (((board.BR >> square) & 1UL) == 1)
                {
                    returnZKey ^= zArray[1,3,square];
                }
                else if (((board.WQ >> square) & 1UL) == 1)
                {
                    returnZKey ^= zArray[0,4,square];
                }
                else if (((board.BQ >> square) & 1UL) == 1)
                {
                    returnZKey ^= zArray[1,4,square];
                }
                else if (((board.WK >> square) & 1UL) == 1)
                {
                    returnZKey ^= zArray[0,5,square];
                }
                else if (((board.BK >> square) & 1UL) == 1)
                {
                    returnZKey ^= zArray[1,5,square];
                }
            }

            if (board.computerHasCastled)
                returnZKey ^= zCastle[0];
            if (board.playerHasCastled)
                returnZKey ^= zCastle[1];

            if (!board.isPlayersTurn())
                returnZKey ^= zBlackMove;
            return returnZKey;
        }

        //Applying ^ twice, returns an Int64 back to its original. 
        //Use this to create a running total instead of recalculating for every board
        //Also, it means the same function can be used to reverse the board
        public static UInt64 moveChangeHash(UInt64 hashKey, Move move, bool playerMadeMove)
        {
            int playerMove = playerMadeMove ? 0 : 1;
            int pieceType = PieceTypeToInt(move.pieceType);

            hashKey ^= zBlackMove;

            //move piece
            hashKey ^= zArray[playerMove, pieceType, move.fromIndex];
            hashKey ^= zArray[playerMove, pieceType, move.toIndex];
            
            //taken piece
            if (move.takenType != PieceType.None)
                hashKey ^= zArray[1 - playerMove, PieceTypeToInt(move.takenType), move.toIndex];

            //castling move, move the rook
            if(move.pieceType == PieceType.King)
            {
                if (playerMadeMove)
                {
                    if (move.fromIndex == 60) //white castle
                    {
                        if (move.toIndex == 62) //right castle
                            hashKey ^= zArray[playerMove, 3, 61];
                        else if (move.toIndex == 58) //left castle
                            hashKey ^= zArray[playerMove, 3, 59];
                    }
                }
                else
                {
                    if (move.fromIndex == 4) //black castle
                    {
                        if (move.toIndex == 6) //right castle
                            hashKey ^= zArray[playerMove, 3, 5];
                        else if (move.toIndex == 2) //left castle
                            hashKey ^= zArray[playerMove, 3, 3];
                    }
                }
            }

            //Pawn promotion
            if (move.pieceType == PieceType.Pawn)
            {
                if (playerMadeMove && (move.toIndex < 8))
                    hashKey ^= zArray[playerMove, 4, move.toIndex]; //Add queen
                else if (!playerMadeMove && (move.toIndex > 55))
                    hashKey ^= zArray[playerMove, 4, move.toIndex]; //Add queen
            }

            return hashKey;
        }

        private static int PieceTypeToInt(PieceType pt)
        {
            switch(pt)
            {
                case PieceType.Pawn:
                    return 0;
                case PieceType.Knight:
                    return 1;
                case PieceType.Bishop:
                    return 2;
                case PieceType.Rook:
                    return 3;
                case PieceType.Queen:
                    return 4;
                case PieceType.King:
                    return 5;
            }
            return -1; //None
        }

        public static void addBoard(UInt64 hash, byte depth, short lowerbound, short upperbound, ushort age, Move bestMove)
        {
            if (boards.ContainsKey(hash))
            {
                //This happens very rarely, and not sure why
                boards.Remove(hash);
                boards.Add(hash, new ZobristEntry(hash, depth, lowerbound, upperbound, age, bestMove));
            }
            else
                boards.Add(hash, new ZobristEntry(hash, depth, lowerbound, upperbound, age, bestMove));
        }

        public static void updateBoard(ZobristEntry ze, byte depth, short lowerbound, short upperbound, ushort age, Move bestMove)
        {
            ze.depth = depth;
            ze.lowerbound = lowerbound;
            ze.upperbound = upperbound;
            ze.age = age;
            ze.bestMove = bestMove;
        }

        public static bool FindBoardWithHash(UInt64 hash, out ZobristEntry he)
        {
            if (boards.TryGetValue(hash, out he))
            {
                //check for index collision
                if (he.hashKey == hash)
                {
                    BoardsFound++;
                    return true;
                }
                else
                {
                    boards.Remove(hash);
                }
            }
            return false;
        }

        public static bool FindBoardWithHash(UInt64 hash, out Move bestMove)
        {
            ZobristEntry he;
            if (boards.TryGetValue(hash, out he))
            {
                //check for index collision
                if (he.hashKey == hash)
                {
                    BoardsFound++;
                    bestMove = he.bestMove;
                    return true;
                }
                else
                {
                    boards.Remove(hash);
                }
            }
            bestMove = null;
            return false;
        }

        //Transposition table for isCheck
        public static void addBoard(UInt64 hash, bool isCheck, ushort age)
        {
            ZobristEntryCheck ze = new ZobristEntryCheck(isCheck, age); 
            checkBoards.Add(hash, ze);
        }

        public static bool FindBoardWithHash(UInt64 hash, out bool isCheck)
        {
            ZobristEntryCheck ze;
            if (checkBoards.TryGetValue(hash, out ze))
            {
                BoardsFound++;
                isCheck = ze.isCheck;
                return true;
            }
            isCheck = false;
            return false;
        }

        //used after black actually makes a move
        public static void removeBoardsWithAgeLessThan(ushort age)
        {
            List<UInt64> keysToDelete = new List<UInt64>();
            foreach (KeyValuePair<UInt64, ZobristEntryCheck> pair in checkBoards)
            {
                if (pair.Value.age < age)
                    keysToDelete.Add(pair.Key);
            }

            foreach (UInt64 key in keysToDelete)
                checkBoards.Remove(key);

            keysToDelete.Clear();
            foreach (ZobristEntry ze in boards.Values)
            {
                if (ze.age < age)
                    keysToDelete.Add(ze.hashKey);
            }

            foreach (UInt64 key in keysToDelete)
                boards.Remove(key);

        }

    }
}
