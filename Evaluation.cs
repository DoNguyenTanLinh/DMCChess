using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    public partial class Board
    {
        private const short PawnIsolated = -10;
        private static short[] PawnPassed = new short[8] { 0, 5, 10, 20, 35, 60, 100, 200 };
        private const short RookOpenFile = 10;
        private const short RookSemiOpenFile = 5;
        private const short QueenOpenFile = 5;
        private const short QueenSemiOpenFile = 3;
        private const short BishopPair = 30;
        private const short ENDGAME_MAT = Constants.VALUE_ROOK + (2 * Constants.VALUE_KNIGHT) + (2 * Constants.VALUE_PAWN) + Constants.VALUE_KING;
        private const float POSITIONAL_WEIGHT = 0.3f;

        private static short[] PawnTable = new short[64]{
            0	,	0	,	0	,	0	,	0	,	0	,	0	,	0	,
            10	,	10	,	0	,	-10	,	-10	,	0	,	10	,	10	,
            5	,	0	,	0	,	5	,	5	,	0	,	0	,	5	,
            0	,	0	,	10	,	20	,	20	,	10	,	0	,	0	,
            5	,	5	,	5	,	10	,	10	,	5	,	5	,	5	,
            10	,	10	,	10	,	20	,	20	,	10	,	10	,	10	,
            20	,	20	,	20	,	30	,	30	,	20	,	20	,	20	,
            0	,	0	,	0	,	0	,	0	,	0	,	0	,	0	
        };

        private static short[] KnightTable = new short[64]{
            0	,	-10	,	0	,	0	,	0	,	0	,	-10	,	0	,
            0	,	0	,	0	,	5	,	5	,	0	,	0	,	0	,
            0	,	0	,	10	,	10	,	10	,	10	,	0	,	0	,
            0	,	0	,	10	,	20	,	20	,	10	,	5	,	0	,
            5	,	10	,	15	,	20	,	20	,	15	,	10	,	5	,
            5	,	10	,	10	,	20	,	20	,	10	,	10	,	5	,
            0	,	0	,	5	,	10	,	10	,	5	,	0	,	0	,
            0	,	0	,	0	,	0	,	0	,	0	,	0	,	0		
        };

        private static short[] BishopTable = new short[64]{
            0	,	0	,	-10	,	0	,	0	,	-10	,	0	,	0	,
            0	,	0	,	0	,	10	,	10	,	0	,	0	,	0	,
            0	,	0	,	10	,	15	,	15	,	10	,	0	,	0	,
            0	,	10	,	15	,	20	,	20	,	15	,	10	,	0	,
            0	,	10	,	15	,	20	,	20	,	15	,	10	,	0	,
            0	,	0	,	10	,	15	,	15	,	10	,	0	,	0	,
            0	,	0	,	0	,	10	,	10	,	0	,	0	,	0	,
            0	,	0	,	0	,	0	,	0	,	0	,	0	,	0	
        };

        private static short[] RookTable = new short[64]{
            0	,	0	,	5	,	10	,	10	,	5	,	0	,	0	,
            0	,	0	,	5	,	10	,	10	,	5	,	0	,	0	,
            0	,	0	,	5	,	10	,	10	,	5	,	0	,	0	,
            0	,	0	,	5	,	10	,	10	,	5	,	0	,	0	,
            0	,	0	,	5	,	10	,	10	,	5	,	0	,	0	,
            0	,	0	,	5	,	10	,	10	,	5	,	0	,	0	,
            25	,	25	,	25	,	25	,	25	,	25	,	25	,	25	,
            0	,	0	,	5	,	10	,	10	,	5	,	0	,	0		
        };

        private static short[] KingE = new short[64]{	
	        -50	,	-10	,	0	,	0	,	0	,	0	,	-10	,	-50	,
	        -10,	0	,	10	,	10	,	10	,	10	,	0	,	-10	,
	        0	,	10	,	20	,	20	,	20	,	20	,	10	,	0	,
	        0	,	10	,	20	,	40	,	40	,	20	,	10	,	0	,
	        0	,	10	,	20	,	40	,	40	,	20	,	10	,	0	,
	        0	,	10	,	20	,	20	,	20	,	20	,	10	,	0	,
	        -10,	0	,	10	,	10	,	10	,	10	,	0	,	-10	,
	        -50	,	-10	,	0	,	0	,	0	,	0	,	-10	,	-50	
        };

        private static short[] KingO = new short[64]{	
	        0	,	5	,	5	,	-10	,	-10	,	0	,	10	,	5	,
	        -30	,	-30	,	-30	,	-30	,	-30	,	-30	,	-30	,	-30	,
	        -50	,	-50	,	-50	,	-50	,	-50	,	-50	,	-50	,	-50	,
	        -70	,	-70	,	-70	,	-70	,	-70	,	-70	,	-70	,	-70	,
	        -70	,	-70	,	-70	,	-70	,	-70	,	-70	,	-70	,	-70	,
	        -70	,	-70	,	-70	,	-70	,	-70	,	-70	,	-70	,	-70	,
	        -70	,	-70	,	-70	,	-70	,	-70	,	-70	,	-70	,	-70	,
	        -70	,	-70	,	-70	,	-70	,	-70	,	-70	,	-70	,	-70		
        };

        private static short[] Mirror = new short[64]{
            56	,	57	,	58	,	59	,	60	,	61	,	62	,	63	,
            48	,	49	,	50	,	51	,	52	,	53	,	54	,	55	,
            40	,	41	,	42	,	43	,	44	,	45	,	46	,	47	,
            32	,	33	,	34	,	35	,	36	,	37	,	38	,	39	,
            24	,	25	,	26	,	27	,	28	,	29	,	30	,	31	,
            16	,	17	,	18	,	19	,	20	,	21	,	22	,	23	,
            8	,	9	,	10	,	11	,	12	,	13	,	14	,	15	,
            0	,	1	,	2	,	3	,	4	,	5	,	6	,	7
        };

        public short getTotalScore()
        {
            if (isCheckmate) return PlayersTurn ? (short)(-Constants.MATE_SCORE) : (short)Constants.MATE_SCORE;
            if (isStalemate) return 0;

            UInt64 piece;
            int sq;
            //white is the maximizer
            short score = (short)(whiteWorth - blackWorth);

            //Getting a relative attack score for all possible captures on a board like this is a bad idea for 2 reasons;
            //1. It forces us to calculate the opponents pseudoMoves to get an approx even score, which is inaccurate because it is not their turn.
            //2. It does not consider the order of captures.
            //Both problems solved by Algorithm.Quint, which looks further into the game tree for capture moves only.

            //float attackScore = 0;
            //foreach (Move move in LegalMoves)
            //    attackScore += (int)move.takenType - (int)((decimal)move.pieceType/(decimal)100);

            //foreach (Move move in opponentsPseudoMoves)
            //    attackScore -= (int)move.takenType - (int)((decimal)move.pieceType / (decimal)100);

            short positionalScore = 0;

            //Pawns
            piece = BP;
            while (piece != 0)
            {
                sq = BitOps.BitScanForwardReset(ref piece);
                positionalScore += PawnTable[sq];
            }

            piece = WP;
            while (piece != 0)
            {
                sq = BitOps.BitScanForwardReset(ref piece);
                positionalScore -= PawnTable[Mirror[sq]];
            }

            //Knights
            piece = BN;
            while (piece != 0)
            {
                sq = BitOps.BitScanForwardReset(ref piece);
                positionalScore += KnightTable[sq];
            }

            piece = WN;
            while (piece != 0)
            {
                sq = BitOps.BitScanForwardReset(ref piece);
                positionalScore -= KnightTable[Mirror[sq]];
            }

            //Bishops
            piece = BB;
            while (piece != 0)
            {
                sq = BitOps.BitScanForwardReset(ref piece);
                positionalScore += BishopTable[sq];
            }
            
            piece = WB;
            while (piece != 0)
            {
                sq = BitOps.BitScanForwardReset(ref piece);
                positionalScore -= BishopTable[Mirror[sq]];
            }

            //Rooks
            piece = BR;
            while (piece != 0)
            {
                sq = BitOps.BitScanForwardReset(ref piece);
                positionalScore += RookTable[sq];
            }

            piece = WR;
            while (piece != 0)
            {
                sq = BitOps.BitScanForwardReset(ref piece);
                positionalScore -= RookTable[Mirror[sq]];
            }

            //King
            sq = blackKingIndex;
            if (blackWorth <= ENDGAME_MAT)
                positionalScore += KingE[sq];
            else
                positionalScore += KingO[sq];


            sq = whiteKingIndex;
            if (whiteWorth <= ENDGAME_MAT)
                positionalScore -= KingE[Mirror[sq]];
            else
                positionalScore -= KingO[Mirror[sq]];

            //score += (short)(POSITIONAL_WEIGHT * positionalScore);

            //pce = wP;
            //for (pceNum = 0; pceNum < pos->pceNum[pce]; ++pceNum)
            //{
            //    sq = pos->pList[pce][pceNum];
            //    ASSERT(SqOnBoard(sq));
            //    ASSERT(SQ64(sq) >= 0 && SQ64(sq) <= 63);
            //    score += PawnTable[SQ64(sq)];

            //    if ((IsolatedMask[SQ64(sq)] & pos->pawns[WHITE]) == 0)
            //    {
            //        //printf("wP Iso:%s\n",PrSq(sq));
            //        score += PawnIsolated;
            //    }

            //    if ((WhitePassedMask[SQ64(sq)] & pos->pawns[BLACK]) == 0)
            //    {
            //        //printf("wP Passed:%s\n",PrSq(sq));
            //        score += PawnPassed[RanksBrd[sq]];
            //    }
            //}

            //pce = bP;
            //for (pceNum = 0; pceNum < pos->pceNum[pce]; ++pceNum)
            //{
            //    sq = pos->pList[pce][pceNum];
            //    ASSERT(SqOnBoard(sq));
            //    ASSERT(MIRROR64(SQ64(sq)) >= 0 && MIRROR64(SQ64(sq)) <= 63);
            //    score -= PawnTable[MIRROR64(SQ64(sq))];

            //    if ((IsolatedMask[SQ64(sq)] & pos->pawns[BLACK]) == 0)
            //    {
            //        //printf("bP Iso:%s\n",PrSq(sq));
            //        score -= PawnIsolated;
            //    }

            //    if ((BlackPassedMask[SQ64(sq)] & pos->pawns[WHITE]) == 0)
            //    {
            //        //printf("bP Passed:%s\n",PrSq(sq));
            //        score -= PawnPassed[7 - RanksBrd[sq]];
            //    }
            //}

            //pce = wN;
            //for (pceNum = 0; pceNum < pos->pceNum[pce]; ++pceNum)
            //{
            //    sq = pos->pList[pce][pceNum];
            //    ASSERT(SqOnBoard(sq));
            //    ASSERT(SQ64(sq) >= 0 && SQ64(sq) <= 63);
            //    score += KnightTable[SQ64(sq)];
            //}

            //pce = bN;
            //for (pceNum = 0; pceNum < pos->pceNum[pce]; ++pceNum)
            //{
            //    sq = pos->pList[pce][pceNum];
            //    ASSERT(SqOnBoard(sq));
            //    ASSERT(MIRROR64(SQ64(sq)) >= 0 && MIRROR64(SQ64(sq)) <= 63);
            //    score -= KnightTable[MIRROR64(SQ64(sq))];
            //}

            //pce = wB;
            //for (pceNum = 0; pceNum < pos->pceNum[pce]; ++pceNum)
            //{
            //    sq = pos->pList[pce][pceNum];
            //    ASSERT(SqOnBoard(sq));
            //    ASSERT(SQ64(sq) >= 0 && SQ64(sq) <= 63);
            //    score += BishopTable[SQ64(sq)];
            //}

            //pce = bB;
            //for (pceNum = 0; pceNum < pos->pceNum[pce]; ++pceNum)
            //{
            //    sq = pos->pList[pce][pceNum];
            //    ASSERT(SqOnBoard(sq));
            //    ASSERT(MIRROR64(SQ64(sq)) >= 0 && MIRROR64(SQ64(sq)) <= 63);
            //    score -= BishopTable[MIRROR64(SQ64(sq))];
            //}

            //pce = wR;
            //for (pceNum = 0; pceNum < pos->pceNum[pce]; ++pceNum)
            //{
            //    sq = pos->pList[pce][pceNum];
            //    ASSERT(SqOnBoard(sq));
            //    ASSERT(SQ64(sq) >= 0 && SQ64(sq) <= 63);
            //    score += RookTable[SQ64(sq)];

            //    ASSERT(FileRankValid(FilesBrd[sq]));

            //    if (!(pos->pawns[BOTH] & FileBBMask[FilesBrd[sq]]))
            //    {
            //        score += RookOpenFile;
            //    }
            //    else if (!(pos->pawns[WHITE] & FileBBMask[FilesBrd[sq]]))
            //    {
            //        score += RookSemiOpenFile;
            //    }
            //}

            //pce = bR;
            //for (pceNum = 0; pceNum < pos->pceNum[pce]; ++pceNum)
            //{
            //    sq = pos->pList[pce][pceNum];
            //    ASSERT(SqOnBoard(sq));
            //    ASSERT(MIRROR64(SQ64(sq)) >= 0 && MIRROR64(SQ64(sq)) <= 63);
            //    score -= RookTable[MIRROR64(SQ64(sq))];
            //    ASSERT(FileRankValid(FilesBrd[sq]));
            //    if (!(pos->pawns[BOTH] & FileBBMask[FilesBrd[sq]]))
            //    {
            //        score -= RookOpenFile;
            //    }
            //    else if (!(pos->pawns[BLACK] & FileBBMask[FilesBrd[sq]]))
            //    {
            //        score -= RookSemiOpenFile;
            //    }
            //}

            //pce = wQ;
            //for (pceNum = 0; pceNum < pos->pceNum[pce]; ++pceNum)
            //{
            //    sq = pos->pList[pce][pceNum];
            //    ASSERT(SqOnBoard(sq));
            //    ASSERT(SQ64(sq) >= 0 && SQ64(sq) <= 63);
            //    ASSERT(FileRankValid(FilesBrd[sq]));
            //    if (!(pos->pawns[BOTH] & FileBBMask[FilesBrd[sq]]))
            //    {
            //        score += QueenOpenFile;
            //    }
            //    else if (!(pos->pawns[WHITE] & FileBBMask[FilesBrd[sq]]))
            //    {
            //        score += QueenSemiOpenFile;
            //    }
            //}

            //pce = bQ;
            //for (pceNum = 0; pceNum < pos->pceNum[pce]; ++pceNum)
            //{
            //    sq = pos->pList[pce][pceNum];
            //    ASSERT(SqOnBoard(sq));
            //    ASSERT(SQ64(sq) >= 0 && SQ64(sq) <= 63);
            //    ASSERT(FileRankValid(FilesBrd[sq]));
            //    if (!(pos->pawns[BOTH] & FileBBMask[FilesBrd[sq]]))
            //    {
            //        score -= QueenOpenFile;
            //    }
            //    else if (!(pos->pawns[BLACK] & FileBBMask[FilesBrd[sq]]))
            //    {
            //        score -= QueenSemiOpenFile;
            //    }
            //}
            ////8/p6k/6p1/5p2/P4K2/8/5pB1/8 b - - 2 62 
            //pce = wK;
            //sq = pos->pList[pce][0];
            //ASSERT(SqOnBoard(sq));
            //ASSERT(SQ64(sq) >= 0 && SQ64(sq) <= 63);

            //if ((pos->material[BLACK] <= ENDGAME_MAT))
            //{
            //    score += KingE[SQ64(sq)];
            //}
            //else
            //{
            //    score += KingO[SQ64(sq)];
            //}

            //pce = bK;
            //sq = pos->pList[pce][0];
            //ASSERT(SqOnBoard(sq));
            //ASSERT(MIRROR64(SQ64(sq)) >= 0 && MIRROR64(SQ64(sq)) <= 63);

            //if ((pos->material[WHITE] <= ENDGAME_MAT))
            //{
            //    score -= KingE[MIRROR64(SQ64(sq))];
            //}
            //else
            //{
            //    score -= KingO[MIRROR64(SQ64(sq))];
            //}

            //if (pos->pceNum[wB] >= 2) score += BishopPair;
            //if (pos->pceNum[bB] >= 2) score -= BishopPair;

            if (isCheck)
                score += PlayersTurn ? (short)-Constants.CHECK_BONUS : Constants.CHECK_BONUS;
            return score;
        }

        //unused now because there is a running total of totalWorth
        public int getTotalWorth()
        {
            //Player has negative score. Is there any point including the kings here? They will always cancel
            return BitOps.BitCountWegner(BP) * Constants.VALUE_PAWN +
                   BitOps.BitCountWegner(BN) * Constants.VALUE_KNIGHT +
                   BitOps.BitCountWegner(BB) * Constants.VALUE_BISHOP +
                   BitOps.BitCountWegner(BR) * Constants.VALUE_ROOK +
                   BitOps.BitCountWegner(BQ) * Constants.VALUE_QUEEN +
                   BitOps.BitCountWegner(BK) * Constants.VALUE_KING -

                   BitOps.BitCountWegner(WP) * Constants.VALUE_PAWN -
                   BitOps.BitCountWegner(WN) * Constants.VALUE_KNIGHT -
                   BitOps.BitCountWegner(WB) * Constants.VALUE_BISHOP -
                   BitOps.BitCountWegner(WR) * Constants.VALUE_ROOK -
                   BitOps.BitCountWegner(WQ) * Constants.VALUE_QUEEN -
                   BitOps.BitCountWegner(WK) * Constants.VALUE_KING;
        }
    }
}
