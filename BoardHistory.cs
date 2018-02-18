using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    public struct BoardHistory
    {
        public Board board;
        public List<PieceType> lostWhitePieces, lostBlackPieces, lostWhitePawns, lostBlackPawns;

        public BoardHistory(Board board, 
            List<PieceType> lostWhitePieces, List<PieceType> lostBlackPieces, 
            List<PieceType> lostWhitePawns, List<PieceType> lostBlackPawns)
        {
            this.board = (Board)board.Clone();
            this.lostWhitePieces = new List<PieceType>(lostWhitePieces);
            this.lostBlackPieces = new List<PieceType>(lostBlackPieces);
            this.lostWhitePawns  = new List<PieceType>(lostWhitePawns);
            this.lostBlackPawns  = new List<PieceType>(lostBlackPawns);
        }

    }
}
