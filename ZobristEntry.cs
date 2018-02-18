using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//http://mediocrechess.blogspot.com.au/2007/01/guide-transposition-tables.html

namespace DMCChess
{
    public struct ZobristEntry
    {
        public UInt64 hashKey; //used for collision detection. Just replace if this happens
        public short lowerbound, upperbound;
        public byte depth;
        public Move bestMove; //Used for best-first sorting. Should be stored as a 4 char
        public ushort age; //the age should always be greater than zero, so we use this to check whether to update or add

        //if depth equals 0, this is the score. Otherwise it is the alpha, beta score for a particular depth
        public ZobristEntry(UInt64 hashKey, byte depth, short lowerbound, short upperbound, ushort age, Move bestMove = null)
        {
            this.hashKey = hashKey;
            this.lowerbound = lowerbound;
            this.upperbound = upperbound;
            this.depth = depth;
            this.age = age;
            this.bestMove = bestMove;
        }
    }

    public struct ZobristEntryCheck
    {
        public bool isCheck;
        public ushort age;

        //if depth equals 0, this is the score. Otherwise it is the alpha, beta score for a particular depth
        public ZobristEntryCheck(bool isCheck, ushort age)
        {
            this.isCheck = isCheck;
            this.age = age;
        }
    }
}

//There are three different kinds of evaluations in the alpha-beta search:

//Exact evaluation (HASH_EXACT in Mediocre), when we receive a definite evaluation, that is we searched all possible moves and received a new best move (or received an evaluation from quiescent search that was between alpha and beta).

//Beta evaluation (HASH_BETA), when we exceed beta we know the move is 'too good' and cut off the rest of the search. Since some of the search is cut off we do not know what the actual evaluation of the position was. All we know is it was atleast 'beta' or higher.

//Alpha Evaluation (HASH_ALPHA), when we do not reach up to alpha. We only know that the evaluation was not as high as alpha.

//The exact flag is applied if the evaluation was between alpha and beta.

//The beta flag is applied if the evaluation caused a cut off, i.e. exceeding beta, this is also done if the null-move causes a cut off.

//And the alpha flag is applied if the evaluation never exceeded alpha.
