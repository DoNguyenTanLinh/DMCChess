using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForcedChess
{
    class PrincipalVariation
    {
        public static float zWSearch(float beta, long WP, long WN, long WB, long WR, long WQ, long WK, 
            long BP, long BN, long BB, long BR, long BQ, long BK, long EP, 
            bool CWK, bool CWQ, bool CBK, bool CBQ, bool WhiteToMove, int depth)
        { //fail-hard zero window search, returns either beta-1 or beta
            float score = float.MinValue;
            //alpha == beta - 1
            //this is either a cut- or all-node
            if (depth == 0)
            {
                score = Rating.evaluate();
                return score;
            }
            string moves;
            if (WhiteToMove)
            {
                moves = Moves.possibleMovesW(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ);
            }
            else
            {
                moves = Moves.possibleMovesB(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ);
            }
            //sortMoves();
            for (int i = 0; i < moves.Length; i += 4)
            {
                long WPt = Moves.makeMove(WP, moves.Substring(i, 4), 'P'), 
                    WNt = Moves.makeMove(WN, moves.Substring(i, 4), 'N'), 
                    WBt = Moves.makeMove(WB, moves.Substring(i, 4), 'B'), 
                    WRt = Moves.makeMove(WR, moves.Substring(i, 4), 'R'), 
                    WQt = Moves.makeMove(WQ, moves.Substring(i, 4), 'Q'), 
                    WKt = Moves.makeMove(WK, moves.Substring(i, 4), 'K'), 
                    BPt = Moves.makeMove(BP, moves.Substring(i, 4), 'p'), 
                    BNt = Moves.makeMove(BN, moves.Substring(i, 4), 'n'), 
                    BBt = Moves.makeMove(BB, moves.Substring(i, 4), 'b'), 
                    BRt = Moves.makeMove(BR, moves.Substring(i, 4), 'r'), 
                    BQt = Moves.makeMove(BQ, moves.Substring(i, 4), 'q'), 
                    BKt = Moves.makeMove(BK, moves.Substring(i, 4), 'k'), 
                    EPt = Moves.makeMoveEP(WP | BP, moves.Substring(i, 4));
                WRt = Moves.makeMoveCastle(WRt, WK | BK, moves.Substring(i, 4), 'R');
                BRt = Moves.makeMoveCastle(BRt, WK | BK, moves.Substring(i, 4), 'r');
                bool CWKt = CWK, CWQt = CWQ, CBKt = CBK, CBQt = CBQ;
                if (char.IsDigit(moves[i + 3]))
                { //'regular' move
                    int start = ((int)char.GetNumericValue(moves[i]) * 8) + ((int)char.GetNumericValue(moves[i + 1]));
                    if (((1L << start) & WK) != 0)
                    {
                        CWKt = false;
                        CWQt = false;
                    }
                    else if (((1L << start) & BK) != 0)
                    {
                        CBKt = false;
                        CBQt = false;
                    }
                    else if (((1L << start) & WR & (1L << 63)) != 0)
                    {
                        CWKt = false;
                    }
                    else if (((1L << start) & WR & (1L << 56)) != 0)
                    {
                        CWQt = false;
                    }
                    else if (((1L << start) & BR & (1L << 7)) != 0)
                    {
                        CBKt = false;
                    }
                    else if (((1L << start) & BR & 1L) != 0)
                    {
                        CBQt = false;
                    }
                }
                if (((WKt & Moves.unSafeForWhite(WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt)) == 0 && WhiteToMove) ||
                    ((BKt & Moves.unSafeForBlack(WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt)) == 0 && !WhiteToMove))
                {
                    score = -zWSearch(1 - beta, WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt, EPt, CWKt, CWQt, CBKt, CBQt, !WhiteToMove, depth - 1);
                }
                if (score >= beta)
                    return score; //fail-hard beta-cutoff
            }
            return beta - 1; //fail-hard, return alpha
        }

        public static int getFirstLegalMove(string moves, long WP, long WN, long WB, long WR, long WQ, long WK, 
            long BP, long BN, long BB, long BR, long BQ, long BK, long EP, 
            bool CWK, bool CWQ, bool CBK, bool CBQ, bool WhiteToMove)
        {
            for (int i = 0; i < moves.Length; i += 4)
            {
                long WPt = Moves.makeMove(WP, moves.Substring(i, 4), 'P'), 
                     WNt = Moves.makeMove(WN, moves.Substring(i, 4), 'N'), 
                     WBt = Moves.makeMove(WB, moves.Substring(i, 4), 'B'), 
                     WRt = Moves.makeMove(WR, moves.Substring(i, 4), 'R'), 
                     WQt = Moves.makeMove(WQ, moves.Substring(i, 4), 'Q'), 
                     WKt = Moves.makeMove(WK, moves.Substring(i, 4), 'K'), 
                     BPt = Moves.makeMove(BP, moves.Substring(i, 4), 'p'), 
                     BNt = Moves.makeMove(BN, moves.Substring(i, 4), 'n'), 
                     BBt = Moves.makeMove(BB, moves.Substring(i, 4), 'b'), 
                     BRt = Moves.makeMove(BR, moves.Substring(i, 4), 'r'), 
                     BQt = Moves.makeMove(BQ, moves.Substring(i, 4), 'q'), 
                     BKt = Moves.makeMove(BK, moves.Substring(i, 4), 'k');
                WRt = Moves.makeMoveCastle(WRt, WK | BK, moves.Substring(i, 4), 'R');
                BRt = Moves.makeMoveCastle(BRt, WK | BK, moves.Substring(i, 4), 'r');
                if (((WKt & Moves.unSafeForWhite(WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt)) == 0 && WhiteToMove) || 
                    ((BKt & Moves.unSafeForBlack(WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt)) == 0 && !WhiteToMove))
                {
                    return i;
                }
            }
            return -1;
        }

        public static float pvSearch(float alpha, float beta, long WP, long WN, long WB, long WR, long WQ, long WK, 
            long BP, long BN, long BB, long BR, long BQ, long BK, long EP, 
            bool CWK, bool CWQ, bool CBK, bool CBQ, bool WhiteToMove, int depth)
        {//using fail soft with negamax
            float bestScore;
            int bestMoveIndex = -1;
            if (depth == 0)
            {
                bestScore = Rating.evaluate();
                return bestScore;
            }
            String moves;
            if (WhiteToMove)
            {
                moves = Moves.possibleMovesW(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ);
            }
            else
            {
                moves = Moves.possibleMovesB(WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ);
            }
            //sortLegalMoves();
            int firstLegalMove = getFirstLegalMove(moves, WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, WhiteToMove);
            if (firstLegalMove == -1)
            {
                return WhiteToMove ? Constants.MATE_SCORE : -Constants.MATE_SCORE;
            }
            long WPt = Moves.makeMove(WP, moves.Substring(firstLegalMove, 4), 'P'), 
                WNt = Moves.makeMove(WN, moves.Substring(firstLegalMove, 4), 'N'), 
                WBt = Moves.makeMove(WB, moves.Substring(firstLegalMove, 4), 'B'), 
                WRt = Moves.makeMove(WR, moves.Substring(firstLegalMove, 4), 'R'), 
                WQt = Moves.makeMove(WQ, moves.Substring(firstLegalMove, 4), 'Q'), 
                WKt = Moves.makeMove(WK, moves.Substring(firstLegalMove, 4), 'K'), 
                BPt = Moves.makeMove(BP, moves.Substring(firstLegalMove, 4), 'p'), 
                BNt = Moves.makeMove(BN, moves.Substring(firstLegalMove, 4), 'n'), 
                BBt = Moves.makeMove(BB, moves.Substring(firstLegalMove, 4), 'b'), 
                BRt = Moves.makeMove(BR, moves.Substring(firstLegalMove, 4), 'r'), 
                BQt = Moves.makeMove(BQ, moves.Substring(firstLegalMove, 4), 'q'), 
                BKt = Moves.makeMove(BK, moves.Substring(firstLegalMove, 4), 'k'), 
                EPt = Moves.makeMoveEP(WP | BP, moves.Substring(firstLegalMove, 4));
            WRt = Moves.makeMoveCastle(WRt, WK | BK, moves.Substring(firstLegalMove, 4), 'R');
            BRt = Moves.makeMoveCastle(BRt, WK | BK, moves.Substring(firstLegalMove, 4), 'r');
            bool CWKt = CWK, CWQt = CWQ, CBKt = CBK, CBQt = CBQ;

            if (Char.IsDigit(moves[firstLegalMove + 3]))
            {//'regular' move
                int start = (int)(Char.GetNumericValue(moves[firstLegalMove]) * 8) + (int)(Char.GetNumericValue(moves[firstLegalMove + 1]));
                if (((1L << start) & WK) != 0) { CWKt = false; CWQt = false; }
                else if (((1L << start) & BK) != 0) { CBKt = false; CBQt = false; }
                else if (((1L << start) & WR & (1L << 63)) != 0) { CWKt = false; }
                else if (((1L << start) & WR & (1L << 56)) != 0) { CWQt = false; }
                else if (((1L << start) & BR & (1L << 7)) != 0) { CBKt = false; }
                else if (((1L << start) & BR & 1L) != 0) { CBQt = false; }
            }
            bestScore = -pvSearch(-beta, -alpha, WPt, WNt, WBt, WRt, WQt, WKt, 
                BPt, BNt, BBt, BRt, BQt, BKt, 
                EPt, CWKt, CWQt, CBKt, CBQt, !WhiteToMove, depth - 1);
            //Orion.moveCounter++;
            if (Math.Abs(bestScore) == Constants.MATE_SCORE)
                return bestScore;

            if (bestScore > alpha)
            {
                if (bestScore >= beta)
                {
                    //This is a refutation move
                    //It is not a PV move
                    //However, it will usually cause a cutoff so it can
                    //be considered a best move if no other move is found
                    return bestScore;
                }
                alpha = bestScore;
            }
            bestMoveIndex = firstLegalMove;
            for (int i = firstLegalMove + 4; i < moves.Length; i += 4)
            {
                float score;
                //Orion.moveCounter++;
                //legal, non-castle move
                WPt = Moves.makeMove(WP, moves.Substring(i, i + 4), 'P');
                WNt = Moves.makeMove(WN, moves.Substring(i, i + 4), 'N');
                WBt = Moves.makeMove(WB, moves.Substring(i, i + 4), 'B');
                WRt = Moves.makeMove(WR, moves.Substring(i, i + 4), 'R');
                WQt = Moves.makeMove(WQ, moves.Substring(i, i + 4), 'Q');
                WKt = Moves.makeMove(WK, moves.Substring(i, i + 4), 'K');
                BPt = Moves.makeMove(BP, moves.Substring(i, i + 4), 'p');
                BNt = Moves.makeMove(BN, moves.Substring(i, i + 4), 'n');
                BBt = Moves.makeMove(BB, moves.Substring(i, i + 4), 'b');
                BRt = Moves.makeMove(BR, moves.Substring(i, i + 4), 'r');
                BQt = Moves.makeMove(BQ, moves.Substring(i, i + 4), 'q');
                BKt = Moves.makeMove(BK, moves.Substring(i, i + 4), 'k');
                EPt = Moves.makeMoveEP(WP | BP, moves.Substring(i, i + 4));
                WRt = Moves.makeMoveCastle(WRt, WK | BK, moves.Substring(i, i + 4), 'R');
                BRt = Moves.makeMoveCastle(BRt, WK | BK, moves.Substring(i, i + 4), 'r');
                CWKt = CWK;
                CWQt = CWQ;
                CBKt = CBK;
                CBQt = CBQ;
                if (Char.IsDigit(moves[i + 3]))
                {//'regular' move
                    int start = (int)(Char.GetNumericValue(moves[i]) * 8) + (int)(Char.GetNumericValue(moves[i + 1]));
                    if (((1L << start) & WK) != 0) { CWKt = false; CWQt = false; }
                    else if (((1L << start) & BK) != 0) { CBKt = false; CBQt = false; }
                    else if (((1L << start) & WR & (1L << 63)) != 0) { CWKt = false; }
                    else if (((1L << start) & WR & (1L << 56)) != 0) { CWQt = false; }
                    else if (((1L << start) & BR & (1L << 7)) != 0) { CBKt = false; }
                    else if (((1L << start) & BR & 1L) != 0) { CBQt = false; }
                }

                score = -zWSearch(-alpha, WPt, WNt, WBt, WRt, WQt, WKt, BPt, BNt, BBt, BRt, BQt, BKt, EPt, CWKt, CWQt, CBKt, CBQt, !WhiteToMove, depth - 1);
                
                if ((score > alpha) && (score < beta))
                {
                    //research with window [alpha;beta]
                    bestScore = -pvSearch(-beta, -alpha, WP, WN, WB, WR, WQ, WK, BP, BN, BB, BR, BQ, BK, EP, CWK, CWQ, CBK, CBQ, !WhiteToMove, depth - 1);
                    if (score > alpha)
                    {
                        bestMoveIndex = i;
                        alpha = score;
                    }
                }

                if ((score != Constants.MIN_FLOAT) && (score > bestScore))
                {
                    if (score >= beta)
                        return score;

                    bestScore = score;

                    if (Math.Abs(bestScore) == Constants.MATE_SCORE)
                        return bestScore;
                }
            }
            return bestScore;
        }

    }
}
