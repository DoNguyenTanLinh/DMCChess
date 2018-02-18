using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    public class Move : IEquatable<Move>, ICloneable
    {
        public byte fromIndex, toIndex;
        public PieceType pieceType, takenType;
        public MoveType moveType; //Handy for pawn promotion, castling and enpassant

        public Move(byte fromIndex, byte toIndex, PieceType pieceType, PieceType takenType)
        {
            this.fromIndex = fromIndex;
            this.toIndex = toIndex;
            this.pieceType = pieceType;
            this.takenType = takenType;
        }

        public bool Equals(Move other)
        {
            if (other == null) return false;
            return (this.fromIndex.Equals(other.fromIndex) && this.toIndex.Equals(other.toIndex) && this.pieceType.Equals(other.pieceType));
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
