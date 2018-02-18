using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    public enum NodeType
    {
        Max,
        Min
    }

    public class Algorithms
    {
        public const byte MAX_PLY = 15;
        public static byte difficulty = 0;

        //This function is similar to AlphaBetaMin but it calls IncProgress and returns the move
        public static Move AlphaBetaEntry(Board board, byte depth, Action IncProgress)
        {
            //Black is the minimizing player. But for the first loop calls IncProgress
            List<Move> moves;

            #if SORTING
                //sorting the first branch by this method rather than taken difference,
                //results in a 100% speed up on the average move time!
                moves = board.sortSetupScore();
            #else
                moves = board.setup();
            #endif

            Move bestMove = null;

            #if ZSCORES
            if (Zobrist.FindBoardWithHash(board.hashKey, out bestMove))
            {
                //TODO: Check depth, otherwise use it for sorting
                //if (bestMove != null) return bestMove;


            }
            #endif

            short alpha = -Constants.MATE_SCORE, beta = Constants.MATE_SCORE;
            short score;

            foreach (Move move in moves)
            {
                IncProgress();

                #if DEBUG
                    Board boardBefore = (Board)board.Clone();
                #endif


                board.makeMove(move);
                 //score = AlphaBetaWithMemory(board, NodeType.Max, (byte)(depth - 1), alpha, beta);
                //score = IterativeDeepening(board, NodeType.Max, (byte)(depth - 1));
                score = AlphaBetaMax(board, (byte)(depth - 1), alpha, beta);
                board.reverseMove(move);
                  
                #if DEBUG
                    CompareBoards("MiniMaxMoveInner", boardBefore, board, move);
                #endif

                if (score == -Constants.MATE_SCORE)
                {
                    return move;   // fail hard beta-cutoff
                }
                if (score < beta)
                {
                    beta = score; // beta acts like min in MiniMax
                    bestMove = move;
                }
            }

            //#if ZSCORES
            //    Zobrist.addBoard(board.hashKey, NodeType.Max, depth, alpha, board.age, bestmove);
            //#endif

            return bestMove;
        }

        public static short AlphaBetaMax(Board board, byte depth, short alpha, short beta)
        {
            List<Move> moves;
            #if SORTING
                //Sorting via this method 4 levels up, rather than via taken difference
                //resulted in a 50% speed up (in addition to sorting on the first level)! 
                //This sort method found to be more effective than takenDifference sorting
                if (depth > 3)
                    moves = board.sortSetupScore();
                else
                    moves = board.setup();
            #else
                moves = board.setup();
            #endif

            if (depth == 0 || board.isGameOver())
                return QuiesceMax(board, alpha, beta);

            short score;
            foreach (Move move in moves)
            {
                #if DEBUG
                    Board boardBefore = (Board)board.Clone();
                #endif

                board.makeMove(move);
                score = AlphaBetaMin(board, (byte)(depth - 1), alpha, beta);
                board.reverseMove(move);

                #if DEBUG
                    CompareBoards("AlphaBetaMax", boardBefore, board, move);
                #endif

                if (score >= beta)
                {
                    return beta;   // fail hard beta-cutoff
                }
                if (score > alpha)
                {
                    alpha = score; // alpha acts like max in MiniMax
                }
            }

            return alpha;
        }

        public static short AlphaBetaMin(Board board, byte depth, short alpha, short beta)
        {
            List<Move> moves;
            #if SORTING
                //Sorting via this method 4 levels up, rather than via taken difference
                //resulted in a 50% speed up (in addition to sorting on the first level)! 
                //This sort method found to be more effective than takenDifference sorting
                if (depth > 3)
                    moves = board.sortSetupScore();
                else
                    moves = board.setup();
            #else
                moves = board.setup();
            #endif

            if (depth == 0 || board.isGameOver())
                return QuiesceMin(board, alpha, beta);

            short score;
            foreach (Move move in moves)
            {
                #if DEBUG
                    Board boardBefore = (Board)board.Clone();
                #endif

                board.makeMove(move);
                score = AlphaBetaMax(board, (byte)(depth - 1), alpha, beta);
                board.reverseMove(move);

                #if DEBUG
                    CompareBoards("AlphaBetaMin", boardBefore, board, move);
                #endif

                if (score <= alpha)
                {
                    return alpha; // fail hard alpha-cutoff
                }
                if (score < beta)
                {
                    beta = score; // beta acts like min in MiniMax
                }
            }

            return beta;
        }

        //http://people.csail.mit.edu/plaat/mtdf.html#abmem
        public static short AlphaBetaWithMemory(Board board, NodeType n, byte depth, short alpha, short beta)
        {
            #if ZSCORES
                bool update = false;
                ZobristEntry ze; //ZobristEntry is a struct and cannot be null.
                if (Zobrist.FindBoardWithHash(board.hashKey, out ze))
                {
                    if (ze.depth >= depth)
                    {
                        if (ze.lowerbound >= beta) return ze.lowerbound;
                        if (ze.upperbound <= alpha) return ze.upperbound;                    
                        alpha = Math.Max(alpha, ze.lowerbound);
                        beta = Math.Min(beta, ze.upperbound);
                    }
                    else //Depth replacement scheme. Update the value in the dictionary but use the best move for ordering
                    {
                        //Dont need to update the hashKey
                        ze.depth = depth;
                        ze.lowerbound = alpha;
                        ze.upperbound = beta;
                    }
                    update = true;
                }
            #endif

            List<Move> moves = board.setup();

            #if ZSCORES
                if (ze.bestMove != null)
                {
                    //put this first in moves
                    moves.Remove(ze.bestMove);
                    moves.Insert(0, ze.bestMove);
                }
            #endif

            short g, a, b;
            Move bestMove = null;

            if ((depth == 0) || (board.isGameOver()))
            {
                //g = board.getTotalScore();
                if (n == NodeType.Max)
                    g = QuiesceMax(board, alpha, beta); /* leaf node */
                else
                    g = QuiesceMin(board, alpha, beta); /* leaf node */
            }
            else if (n == NodeType.Max)
            {
                g = -Constants.MATE_SCORE; a = alpha;
                foreach (Move move in moves)
                {
                    board.makeMove(move);
                    //g = AlphaBetaWithMemory(board, NodeType.Min, (byte)(depth - 1), a, beta);
                    g = Math.Max(g, AlphaBetaWithMemory(board, NodeType.Min, (byte)(depth - 1), a, beta));
                    board.reverseMove(move);
                    if (g > a)
                    {
                        a = g;
                        bestMove = move;
                    }
                    if (g >= beta)
                    {
                        bestMove = move;
                        break;
                    }
                }
            }
            else /* n is a MINNODE */
            {
                g = Constants.MATE_SCORE; b = beta;
                foreach (Move move in moves)
                {
                    board.makeMove(move);
                    //g = AlphaBetaWithMemory(board, NodeType.Max, (byte)(depth - 1), alpha, b);
                    g = Math.Min(g, AlphaBetaWithMemory(board, NodeType.Max, (byte)(depth - 1), alpha, b));
                    board.reverseMove(move);
                    if(g < b)
                    {
                        b = g;
                        bestMove = move;
                    }
                    if (g <= alpha)
                    {
                        bestMove = move;
                        break;
                    }
                }
            }

            #if ZSCORES
                if (g <= alpha)
                {
                    if (update)
                    {
                        ze.upperbound = g;
                        ze.age = board.age;
                        ze.bestMove = bestMove;
                    }
                    else
                        Zobrist.addBoard(board.hashKey, depth, beta, g, board.age, bestMove);
                }
                else if ((g > alpha) && (g < beta))
                {
                    if (update)
                    {
                        ze.lowerbound = g;
                        ze.upperbound = g;
                        ze.age = board.age;
                        ze.bestMove = bestMove;
                    }
                    else
                        Zobrist.addBoard(board.hashKey, depth, g, g, board.age, bestMove);
                }
                else if (g >= beta)
                {
                    if (update)
                    {
                        ze.lowerbound = g;
                        ze.age = board.age;
                        ze.bestMove = bestMove;
                    }
                    Zobrist.addBoard(board.hashKey, depth, g, alpha, board.age, bestMove);
                }
            #endif
            return g;
        }

        public static short AlphaBetaWithMemoryEntry(Board board, NodeType n, byte depth, short alpha, short beta, ref Move bestMove)
        {
            #if ZSCORES
                bool update = false;
                ZobristEntry ze; //ZobristEntry is a struct and cannot be null.
                if (Zobrist.FindBoardWithHash(board.hashKey, out ze))
                {
                    if (ze.depth >= depth)
                    {
                        if ((ze.lowerbound >= beta) && ((ze.bestMove != null)))
                        {
                            bestMove = ze.bestMove; 
                            return ze.lowerbound;
                        };
                        if ((ze.upperbound <= alpha) && ((ze.bestMove != null)))
                        { 
                            bestMove = ze.bestMove; 
                            return ze.upperbound; 
                        };
                        alpha = Math.Max(alpha, ze.lowerbound);
                        beta = Math.Min(beta, ze.upperbound);
                    }
                    else //Depth replacement scheme. Update the value in the dictionary but use the best move for ordering
                    {
                        //Dont need to update the hashKey
                        ze.depth = depth;
                        ze.lowerbound = alpha;
                        ze.upperbound = beta;
                    }
                    update = true;
                }
            #endif

            List<Move> moves = board.setup();

            #if ZSCORES
                if (ze.bestMove != null)
                {
                    //put this first in moves
                    moves.Remove(ze.bestMove);
                    moves.Insert(0, ze.bestMove);
                }
            #endif

            short g, a, b;
            bestMove = moves.ElementAt(0);

            if (n == NodeType.Max)
            {
                g = -Constants.MATE_SCORE; a = alpha;
                foreach (Move move in moves)
                {
                    board.makeMove(move);
                    //g = AlphaBetaWithMemory(board, NodeType.Min, (byte)(depth - 1), a, beta);
                    g = Math.Max(g, AlphaBetaWithMemory(board, NodeType.Min, (byte)(depth - 1), a, beta));
                    board.reverseMove(move);
                    if (g > a)
                    {
                        a = g;
                        bestMove = move;
                    }
                    if (g >= beta)
                    {
                        bestMove = move;
                        break;
                    }
                }
            }
            else /* n is a MINNODE */
            {
                g = Constants.MATE_SCORE; b = beta;
                foreach (Move move in moves)
                {
                    board.makeMove(move);
                    //g = AlphaBetaWithMemory(board, NodeType.Max, (byte)(depth - 1), alpha, b);
                    g = Math.Min(g, AlphaBetaWithMemory(board, NodeType.Max, (byte)(depth - 1), alpha, b));
                    board.reverseMove(move);
                    if (g < b)
                    {
                        b = g;
                        bestMove = move;
                    }
                    if (g <= alpha)
                    {
                        bestMove = move;
                        break;
                    }
                }
            }

            #if ZSCORES
                if (g <= alpha)
                {
                    if (update)
                    {
                        ze.upperbound = g;
                        ze.age = board.age;
                        ze.bestMove = bestMove;
                    }
                    else
                        Zobrist.addBoard(board.hashKey, depth, beta, g, board.age, bestMove);
                }
                else if ((g > alpha) && (g < beta))
                {
                    if (update)
                    {
                        ze.lowerbound = g;
                        ze.upperbound = g;
                        ze.age = board.age;
                        ze.bestMove = bestMove;
                    }
                    else
                        Zobrist.addBoard(board.hashKey, depth, g, g, board.age, bestMove);
                }
                else if (g >= beta)
                {
                    if (update)
                    {
                        ze.lowerbound = g;
                        ze.age = board.age;
                        ze.bestMove = bestMove;
                    }
                    Zobrist.addBoard(board.hashKey, depth, g, alpha, board.age, bestMove);
                }
            #endif
            return g;
        }

        public static short MTDF(Board board, NodeType n, byte depth, short f, out Move bestMove)
        {
            short g = f;
            short upper = Constants.MATE_SCORE;
            short lower = -Constants.MATE_SCORE;
            short alpha = lower;
            short beta = upper;

            bestMove = null;
            Move tempMove = null;

            const int start_width = 4;
            const int grow_width = 4;
            int width = start_width;

            do
            {
                alpha = (g == lower) ? (short)(lower + 1) : lower;
                beta = (g == upper) ? (short)(upper - 1) : upper;

                //alpha = Math.Max((g == lower) ? (short)(lower + 1) : lower, g -= (short)(1+width/2));
                //beta = Math.Min((g == upper) ? (short)(upper - 1) : upper, alpha += (short)(1 + width));

                g = AlphaBetaWithMemoryEntry(board, n, depth, alpha, beta, ref tempMove);

                if (g < beta)
                {
                    if (tempMove != null)
                        bestMove = tempMove;
                    if (g > alpha)
                    {
                        break;
                    }
                    upper = g;
                }
                else
                {
                    lower = g;
                }

                width += grow_width;
                //Console.WriteLine("depth: " + depth + " g:" + g + " beta:" + beta + " lowerbound:" + lowerbound + " upperbound:" + upperbound);
            }
            while (lower < upper);
            return g;
        }

        public static Move IterativeDeepening(Board board, NodeType n, byte depth, Action IncProgress)
        {
            short firstguess = 0;
            Move bestMove = null;
            for(byte d = 1; d <= depth; d++)
            {
                IncProgress();
                firstguess = MTDF(board, n, d, firstguess, out bestMove);
                //if times_up()
                //    break;
            }
            return bestMove;
            //return firstguess;
        }

        //https://chessprogramming.wikispaces.com/Quiescence+Search
        //This is like the alphabeta algorithm but only looks at capture moves
        //It can be used on the leafs of the game tree produced by minimax algorithm
        //For instance, make sure we are not making a move that leads to a board where the 
        //black queen will be taken on the next move.

        private static short QuiesceMax(Board board, short alpha, short beta) 
        {
            List<Move> moves = board.setup();
            short score = board.getTotalScore();

            if (score >= beta)
                return beta;

            if (board.ply > MAX_PLY) return score;

            if (score > alpha)
                alpha = score;

            //Is it worth sorting by attack difference here?

            foreach(Move move in moves)
            {
                if (move.takenType != PieceType.None)
                {
                    #if DEBUG
                        Board boardBefore = (Board)board.Clone();
                    #endif

                    board.makeMove(move);
                    score = QuiesceMin(board, alpha, beta);
                    board.reverseMove(move);

                    #if DEBUG
                        CompareBoards("Quiesce", boardBefore, board, move);
                    #endif

                    if (score >= beta)
                    {
                        return beta;   // fail hard beta-cutoff
                    }
                    if (score > alpha)
                    {
                        alpha = score; // alpha acts like max in MiniMax
                    }
                }
            }

            return alpha;
        }

        private static short QuiesceMin(Board board, short alpha, short beta)
        {
            List<Move> moves = board.setup();
            short score = board.getTotalScore();

            if (score <= alpha)
                return alpha;

            if (board.ply > MAX_PLY) return score;

            if (score < beta)
                beta = score;

            //Is it worth sorting by attack difference here?

            foreach (Move move in moves)
            {
                if (move.takenType != PieceType.None)
                {
                    #if DEBUG
                        Board boardBefore = (Board)board.Clone();
                    #endif
                    board.makeMove(move);
                    score = QuiesceMax(board, alpha, beta);
                    board.reverseMove(move);

                    #if DEBUG
                        CompareBoards("Quiesce", boardBefore, board, move);
                    #endif

                    if (score <= alpha)
                    {
                        return alpha; // fail hard alpha-cutoff
                    }
                    if (score < beta)
                    {
                        beta = score; // beta acts like min in MiniMax
                    }
                }
            }

            return beta;
        }

        private static void CompareBoards(String title, Board b1, Board b2, Move move = null)
        {
            if (!b1.Equals(b2))
            {
                Console.WriteLine(title);
                if (move != null)
                    Console.WriteLine("ERROR: move = (" + move.fromIndex + ") -> (" + move.toIndex + ") type=" + move.pieceType.ToString() + " takenType=" + move.takenType.ToString());
                Console.WriteLine("ERROR: boardBefore");
                b1.print();
                Console.WriteLine("ERROR: boardAfter");
                b2.print();
                Board.PrintBB(b1.hashKey, "b1.hashkey");
                Board.PrintBB(b2.hashKey, "b2.hashkey");
            }
        }

        //public static int zWSearch(int beta, Board board, int depth, out Move moveWithMax)
        //{ //fail-hard zero window search, returns either beta-1 or beta
        //    int score = Constants.MIN_SCORE;
        //    moveWithMax = null;
            
        //    //alpha == beta - 1
        //    //this is either a cut- or all-node
        //    if ((depth == 0) || board.isGameOver())
        //    {
        //        score = board.getTotalScore();
        //        return score;
        //    }

        //    //board.sortLegalMoves();
        //    List<Move> moves = board.getLegalMoves();
        //    Board nextBoard;

        //    foreach (Move move in moves)
        //    {
        //        nextBoard = board.copyMake(move);
        //        nextBoard.setup();
        //        Move dummy;
        //        score = -zWSearch(1 - beta, nextBoard, depth - 1, out dummy);

        //        if (score >= beta)
        //        {
        //            moveWithMax = move;
        //            return score; //fail-hard beta-cutoff
        //        }
        //    }
        //    return beta - 1; //fail-hard, return alpha
        //}

        //public static int pvSearch(int alpha, int beta, Board board, int depth, out Move moveWithMax, Action IncProgress)
        //{//using fail soft with negamax
        //    int bestScore;
        //    moveWithMax = null;

        //    if ((depth == 0) || board.isGameOver())
        //    {
        //        bestScore = board.getTotalScore();
        //        return bestScore;
        //    }

        //    //board.sortLegalMoves()
        //    List<Move> moves = board.getLegalMoves();

        //    Board nextBoard = board.copyMake(moves[0]);
        //    nextBoard.setup();

        //    Move dummy;
        //    bestScore = -pvSearch(-beta, -alpha, nextBoard, depth - 1, out dummy, IncProgress);             

        //    if (depth == difficulty)
        //        IncProgress();

        //    if (Math.Abs(bestScore) == Constants.MATE_SCORE)
        //    {
        //        moveWithMax = moves[0];
        //        return bestScore;
        //    }

        //    if (bestScore > alpha)
        //    {
        //        if (bestScore >= beta)
        //        {
        //            //This is a refutation move
        //            //It is not a PV move
        //            //However, it will usually cause a cutoff so it can
        //            //be considered a best move if no other move is found
        //            moveWithMax = moves[0];
        //            return bestScore;
        //        }
        //        alpha = bestScore;
        //    }

        //    int bestMoveIndex = 0;
        //    for (int i = 1; i < moves.Count; i ++)
        //    {
        //        nextBoard = board.copyMake(moves[i]);
        //        nextBoard.setup();

        //        int score = -zWSearch(-alpha, nextBoard, depth - 1, out dummy);

        //        if (depth == difficulty)
        //            IncProgress();

        //        if ((score > alpha) && (score < beta))
        //        {
        //            //research with window [alpha;beta]
        //            bestScore = -pvSearch(-beta, -alpha, nextBoard, depth - 1, out moveWithMax, IncProgress);
        //            if (score > alpha)
        //            {
        //                bestMoveIndex = i;
        //                alpha = score;
        //            }
        //        }

        //        if ((score != Constants.MIN_SCORE) && (score > bestScore))
        //        {
        //            if (score >= beta)
        //            {
        //                moveWithMax = moves[i];
        //                return score;
        //            }

        //            bestScore = score;

        //            if (Math.Abs(bestScore) == Constants.MATE_SCORE)
        //            {
        //                moveWithMax = moves[i];
        //                return bestScore;
        //            }
        //        }

        //    }
        //    moveWithMax = moves[bestMoveIndex];
        //    return bestScore;
        //}

    }
}
