using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    class Game
    {
        public Board mainBoard;
        public Stack<BoardHistory> boardHistory;
        public List<PieceType> lostWhitePieces, lostBlackPieces, lostWhitePawns, lostBlackPawns;
        private Queue<Move> last9Moves;
        //private int difficulty;
        public long totalTimeSoFar = 0;
        public int totalMovesMade = 0;

        public Game()
        {
            Zobrist.removeBoardsWithAgeLessThan(ushort.MaxValue); //Remove all transposition table
            
            last9Moves = new Queue<Move>(9);
            mainBoard = new Board(true);
            mainBoard.initialisePieces();
            mainBoard.setup();

            boardHistory = new Stack<BoardHistory>();
            lostWhitePieces = new List<PieceType>();
            lostBlackPieces = new List<PieceType>();
            lostWhitePawns = new List<PieceType>();
            lostBlackPawns = new List<PieceType>();

            PlayGameSound();
        }

        public bool UndoMove()
        {           
            if (boardHistory.Count > 0) 
            {
                BoardHistory lastBoardHistory = boardHistory.Pop();
                mainBoard = lastBoardHistory.board;
                lostWhitePieces = lastBoardHistory.lostWhitePieces;
                lostBlackPieces = lastBoardHistory.lostBlackPieces;
                lostWhitePawns  = lastBoardHistory.lostWhitePawns;
                lostBlackPawns  = lastBoardHistory.lostBlackPawns;

                return (boardHistory.Count == 0); //return true if empty
            }

            return true; //return true if Move history is empty
        }

        public bool IsMoveLegal(byte fromIndex, byte toIndex)
        {
            List<Move> moves = mainBoard.setup();
            foreach (Move move in moves)
            {
                if ((move.fromIndex == fromIndex) && (move.toIndex == toIndex))
                    return true;
            }
            return false;
        }

        //used to determine the green squares
        public List<byte> GetLegalSquares(byte fromIndex)
        {
            List<byte> legalSquares = new List<byte>();
            List<Move> moves = mainBoard.setup();
            foreach (Move move in moves)
            {
                if(move.fromIndex == fromIndex)
                    legalSquares.Add(move.toIndex);
            }
            return legalSquares;
        }

        public void CollectTakenPieces(Move move)
        {
            if (move.takenType != PieceType.None)
            { 
                if (mainBoard.isPlayersTurn()) //white just moved
                {
                    if (move.takenType == PieceType.Pawn)
                        lostBlackPawns.Add(move.takenType);
                    else
                        lostBlackPieces.Add(move.takenType);
                }
                else
                {
                    if (move.takenType == PieceType.Pawn)
                        lostWhitePawns.Add(move.takenType);
                    else
                        lostWhitePieces.Add(move.takenType);
                }
            }

            //byte x1 = Constants.IndexToX(move.fromIndex);
            //byte y1 = Constants.IndexToY(move.fromIndex);
            //byte x2 = Constants.IndexToX(move.toIndex);
            //byte y2 = Constants.IndexToY(move.toIndex);

            //en passant
            //if ((takenPieceType == PieceType.None) && (x1 != x2) && (movedPieceType == PieceType.Pawn))
            //{
            //    if (mainBoard.isPlayersTurn() && (y2 == 2) && (y1 == 3))
            //        lostBlackPawns.Add(mainBoard.GetPieceTypeAt(Constants.XYToIndex(x2, 3)));
            //    else if (!mainBoard.isPlayersTurn() && (y2 == 5) && (y1 == 4))
            //        lostWhitePawns.Add(mainBoard.GetPieceTypeAt(Constants.XYToIndex(x2, 4)));
            //}
        }

        public void MakePlayerMove(byte fromIndex, byte toIndex)
        {
            //record previous mainboard, so moves can be undone. 
            boardHistory.Push(new BoardHistory(mainBoard, lostWhitePieces, lostBlackPieces, lostWhitePawns, lostBlackPawns));
            //actually make the player move
            Move move = new Move(fromIndex, toIndex, mainBoard.GetPieceTypeAt(fromIndex), mainBoard.GetPieceTypeAt(toIndex));
            CollectTakenPieces(move);
            mainBoard.makeMove(move);
            DetermineRepeatedMoveStalemate(move);
            mainBoard.setup();
            if (mainBoard.isGameOver())
                PlayGameSound();
            else
                PlayMoveSound();
        }

        public Move MakeBestComputerMove(byte depth, Action IncProgress)
        {                                             
            boardHistory.Push(new BoardHistory(mainBoard, lostWhitePieces, lostBlackPieces, lostWhitePawns, lostBlackPawns));
            mainBoard.age++;

            Algorithms.difficulty = depth;
            Board.boardsCreated = 0;
            Zobrist.BoardsFound = 0;
            mainBoard.ply = 0;

            Move move = null;
            if (depth == 0)
                move = getRandomLegalMove();
            else
            {
                move = Algorithms.AlphaBetaEntry(mainBoard, depth, IncProgress);
                //Algorithms.AlphaBetaWithMemoryEntry(mainBoard, NodeType.Min, depth, -Constants.MATE_SCORE, Constants.MATE_SCORE, ref move);
                //2move = Algorithms.IterativeDeepening(mainBoard, NodeType.Min, depth, IncProgress);
            }

            CollectTakenPieces(move);
            mainBoard.makeMove(move);
            DetermineRepeatedMoveStalemate(move);
            mainBoard.setup(); //determine legal moves for player
            
            if (mainBoard.isGameOver())
                PlayGameSound();
            else
                PlayMoveSound();
            totalMovesMade++;
            Zobrist.removeBoardsWithAgeLessThan((ushort)(mainBoard.age - Zobrist.REMOVE_AGE));
            return move;
        }

        private Move getRandomLegalMove()
        {
            Move bestMove = null;
            Random randomGenerator = new Random();
            int randomIndex;
            List<Move> moves = mainBoard.setup();
            if (moves.Count != 0)
            {
                randomIndex = randomGenerator.Next(0, moves.Count);
                bestMove = moves.ElementAt(randomIndex);
            }

            return bestMove;
        }

        //This was used in the algorithm to determine this kind of stalemate.
        //However, checking this for each board, slowed the algorithm by approx 20%. So its not worth it,
        //This function is used to detect stalemate for the mainBoard only. 
        public void DetermineRepeatedMoveStalemate(Move move)
        {
            last9Moves.Enqueue(move);
            if (last9Moves.Count > 9)
                last9Moves.Dequeue();

            if ((last9Moves.Count == 9) && 
                (last9Moves.ElementAt(0) == last9Moves.ElementAt(4)) &&
                (last9Moves.ElementAt(0) == last9Moves.ElementAt(8)))
                mainBoard.setStalemate();
        }

        private void PlayGameSound()
        {
            System.IO.Stream str = Properties.Resources.chessStartFinish;
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.Play();
        }

        private void PlayMoveSound()
        {
            System.IO.Stream str = Properties.Resources.chessMove;
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.Play();
        }
    }
}
