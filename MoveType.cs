using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    public enum MoveType
    {
        CastleRight,
        CastleLeft,
        EnPassant,
        QueenPromote,
        KnightPromote,
        RookPromote,
        BishopPromote,
        None   //all other moves
    };
}
