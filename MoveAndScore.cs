using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    //Used for sorting legal moves of a board based on the score of the next board 
    public struct MoveAndScore
    {
        public float score;
        public Move move;

        public MoveAndScore(Move move, float score)
        {
            this.move = move;
            this.score = score;
        }
    }
}
