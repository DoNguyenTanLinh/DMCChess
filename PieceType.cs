namespace DMCChess
{
    public enum PieceType
    {
        //These numbers are used when sorting moves because the Move contains pieceType information. 
        //Sorting with these values will be faster than looking up the value for each piecetype
        None = 0,
        Pawn = Constants.VALUE_PAWN,
        Knight = Constants.VALUE_KNIGHT,
        Bishop = Constants.VALUE_BISHOP,
        Rook = Constants.VALUE_ROOK,
        Queen = Constants.VALUE_QUEEN,
        King = Constants.VALUE_KING       
    };
}