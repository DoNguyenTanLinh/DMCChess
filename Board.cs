using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    public partial class Board : IEquatable<Board>, ICloneable
    {
        public static uint boardsCreated = 0;

        public bool PlayersTurn;
        public bool isCheck = false;
        public bool isCheckmate = false;
        public bool isStalemate = false;

        public bool playerHasCastled = false;
        public bool computerHasCastled = false;

        public UInt64 WP, WN, WB, WR, WQ, WK;
        public UInt64 BP, BN, BB, BR, BQ, BK;
        public List<Move> legalMoves;

        //More efficient to update in the makeMove() function, rather than to recalculate
        public UInt64 empty, occupied, whitePieces, blackPieces;
        public short whiteWorth;
        public short blackWorth;
        public ushort ply;
        public ushort age;
        public UInt64 hashKey;
        public byte whiteKingIndex, blackKingIndex; //keeping track to avoid bitscanforward in isKingAttacked(). Leads to 1% speed up

        //pieceColor = true for white
        private delegate UInt64 GetTargetsDelegate(UInt64 pieces, bool pieceColor, Board board);

        public Board(bool playersTurn)
        {
            this.PlayersTurn = playersTurn;            
            boardsCreated++;        
        }

        public void initialisePieces()
        {
            //Little endian system
            // white pieces
            WP = 0xFF000000000000UL;
            WN = 0x4200000000000000UL;
            WB = 0x2400000000000000UL;
            WR = 0x8100000000000000UL;
            WQ = 0x800000000000000UL;
            WK = 0x1000000000000000UL;

            // black pieces
            BP = 0xFF00UL;
            BN = 0x42UL;
            BB = 0x24UL;
            BR = 0x81UL;
            BQ = 0x8UL;
            BK = 0x10UL;

            //Running totals
            whiteKingIndex = 60;
            blackKingIndex = 4;

            whitePieces = (WP | WN | WB | WR | WQ | WK);
            blackPieces = (BP | BN | BB | BR | BQ | BK);
            occupied = (whitePieces | blackPieces);
            empty = ~occupied;

            whiteWorth = 8 * Constants.VALUE_PAWN +
                         2 * (Constants.VALUE_BISHOP + Constants.VALUE_KNIGHT + Constants.VALUE_ROOK) +
                         Constants.VALUE_QUEEN + Constants.VALUE_KING;
            blackWorth = whiteWorth;

            //whiteWorth = Constants.VALUE_QUEEN + Constants.VALUE_KING;
            //blackWorth = (2*Constants.VALUE_ROOK) + Constants.VALUE_KING;

            ply = 0;

            hashKey = Zobrist.calculateZobristHash(this);
            age = 0;
        }

        public void makeMove(Move move)
        {
            boardsCreated++;
            ply++;

            UInt64 toBoard = 1UL << move.toIndex;
            UInt64 takenBoard = ~toBoard;
            UInt64 notFromBoard = ~(1UL << move.fromIndex);

            if (PlayersTurn)
            {                
                whitePieces = (whitePieces & notFromBoard) | toBoard;

                switch (move.pieceType)
                {
                    case PieceType.Knight:
                        WN = (WN & notFromBoard) | toBoard;
                        break;
                    case PieceType.Bishop:
                        WB = (WB & notFromBoard) | toBoard;
                        break;
                    case PieceType.Rook:
                        WR = (WR & notFromBoard) | toBoard;
                        break;
                    case PieceType.Queen:
                        WQ = (WQ & notFromBoard) | toBoard;
                        break;
                    case PieceType.King:
                        whiteKingIndex = move.toIndex;
                        WK = (WK & notFromBoard) | toBoard;

                        //castling move, so move the rook
                        if (move.fromIndex == 60) //white castle
                        {
                            if (move.toIndex == 62) //right castle
                            {
                                UInt64 removeRook = ~(1UL << 63);
                                UInt64 addRook = (1UL << 61);
                                WR &= removeRook;
                                WR |= addRook;
                                whitePieces &= removeRook;
                                whitePieces |= addRook;
                            }
                            else if (move.toIndex == 58) //left castle
                            {
                                UInt64 removeRook = ~(1UL << 56);
                                UInt64 addRook = (1UL << 59);
                                WR &= removeRook;
                                WR |= addRook;
                                whitePieces &= removeRook;
                                whitePieces |= addRook;
                            }
                        }

                        //King can't castle if it has moved
                        playerHasCastled = true;
                        break;
                    case PieceType.Pawn:
                        if (move.toIndex < 8) //pawn promotion
                        {
                            WP = (WP & notFromBoard);
                            WQ |= toBoard;
                            whiteWorth -= Constants.VALUE_PAWN;
                            whiteWorth += Constants.VALUE_QUEEN;
                        }
                        else
                            WP = (WP & notFromBoard) | toBoard;
                        break;
                }

                //black's piece being taken (king can't be taken)
                if (move.takenType != PieceType.None)
                {
                    blackPieces &= takenBoard;
                    blackWorth -= Constants.GetPieceWorth(move.takenType);
                    switch (move.takenType)
                    {
                        case PieceType.Pawn:
                            BP &= takenBoard;
                            break;
                        case PieceType.Knight:
                            BN &= takenBoard;
                            break;
                        case PieceType.Bishop:
                            BB &= takenBoard;
                            break;
                        case PieceType.Rook:
                            BR &= takenBoard;
                            break;
                        case PieceType.Queen:
                            BQ &= takenBoard;
                            break;
                        default:
                            break;
                    }
                }
            }
            else //black's turn
            {                
                blackPieces = (blackPieces & notFromBoard) | toBoard;
                //When a black piece is moved, it can't possibly change the position of other black pieces, 
                //but it might change white pieces
                switch (move.pieceType)
                {
                    case PieceType.Knight:
                        BN = (BN & notFromBoard) | toBoard;
                        break;
                    case PieceType.Bishop:
                        BB = (BB & notFromBoard) | toBoard;
                        break;
                    case PieceType.Rook:
                        BR = (BR & notFromBoard) | toBoard;
                        break;
                    case PieceType.Queen:
                        BQ = (BQ & notFromBoard) | toBoard;
                        break;
                    case PieceType.King:
                        blackKingIndex = move.toIndex;
                        BK = (BK & notFromBoard) | toBoard;
                        //Castling move, so move the rook
                        if (move.fromIndex == 4) //black castle
                        {
                            if (move.toIndex == 6) //right castle
                            {
                                UInt64 removeRook = ~(1UL << 7);
                                UInt64 addRook = (1UL << 5);
                                BR &= removeRook;
                                BR |= addRook;
                                blackPieces &= removeRook;
                                blackPieces |= addRook;
                            }
                            else if (move.toIndex == 2) //left castle
                            {
                                UInt64 removeRook = ~(1UL << 0);
                                UInt64 addRook = (1UL << 3);
                                BR &= removeRook;
                                BR |= addRook;
                                blackPieces &= removeRook;
                                blackPieces |= addRook;
                            }
                        }

                        //King can't castle if it has moved
                        computerHasCastled = true;
                        break;
                    case PieceType.Pawn:

                        //pawn promotion
                        if (move.toIndex > 55)
                        {
                            BP = (BP & notFromBoard);
                            BQ |= toBoard;
                            blackWorth -= Constants.VALUE_PAWN;
                            blackWorth += Constants.VALUE_QUEEN;
                        }
                        else
                            BP = (BP & notFromBoard) | toBoard;
                        break;
                }

                //white's piece being taken (king can't be taken)
                if (move.takenType != PieceType.None)
                {
                    whiteWorth -= Constants.GetPieceWorth(move.takenType);
                    whitePieces &= takenBoard;
                    switch(move.takenType)
                    {
                        case PieceType.Pawn:
                            WP &= takenBoard;
                            break;
                        case PieceType.Knight:
                            WN &= takenBoard;
                            break;
                        case PieceType.Bishop:
                            WB &= takenBoard;
                            break;
                        case PieceType.Rook:
                            WR &= takenBoard;
                            break;
                        case PieceType.Queen:
                            WQ &= takenBoard;
                            break;
                        default:
                            break;
                    }
                }
            }

            occupied = whitePieces | blackPieces;
            empty = ~occupied;

            PlayersTurn = !PlayersTurn;
            
            //increment hash key with ^. It's faster than recalculating it.
            hashKey = Zobrist.moveChangeHash(hashKey, move, PlayersTurn);
        }

        public void reverseMove(Move move)
        {
            ply--;
            PlayersTurn = !PlayersTurn;
            
            UInt64 fromBoard  = (1UL << move.fromIndex);
            UInt64 toBoard    = (1UL << move.toIndex);
            UInt64 notToBoard = ~toBoard;

            if (PlayersTurn) //the side that made the move was white
            {                
                whitePieces = (whitePieces & notToBoard) | fromBoard;                   
                switch (move.pieceType)
                {
                    case PieceType.Knight:
                        WN = (WN & notToBoard) | fromBoard;
                        break;
                    case PieceType.Bishop:
                        WB = (WB & notToBoard) | fromBoard;
                        break;
                    case PieceType.Rook:
                        WR = (WR & notToBoard) | fromBoard;
                        break;
                    case PieceType.Queen:
                        WQ = (WQ & notToBoard) | fromBoard;
                        break;
                    case PieceType.King:
                        whiteKingIndex = move.fromIndex;
                        WK = (WK & notToBoard) | fromBoard;

                        //castling move, so move the rook
                        if (move.fromIndex == 60) //white castle
                        {
                            if (move.toIndex == 62) //right castle
                            {
                                UInt64 addRook = (1UL << 63);
                                UInt64 removeRook = ~(1UL << 61);
                                WR &= removeRook;
                                WR |= addRook;
                                whitePieces &= removeRook;
                                whitePieces |= addRook;
                                playerHasCastled = false;
                            }
                            else if (move.toIndex == 58) //left castle
                            {
                                UInt64 addRook = (1UL << 56);
                                UInt64 removeRook = ~(1UL << 59);
                                WR &= removeRook;
                                WR |= addRook;
                                whitePieces &= removeRook;
                                whitePieces |= addRook;
                                playerHasCastled = false;
                            }
                        }

                        //This is a hack and not necessarily correct. The king may have moved before, etc.
                        //It really requires a history of the board
                        if(move.fromIndex == 60)
                            playerHasCastled = false;
                        break;
                    case PieceType.Pawn:
                        if (move.toIndex < 8) //pawn promotion
                        {
                            WP |= fromBoard;
                            WQ &= notToBoard;
                            whiteWorth += (Constants.VALUE_PAWN - Constants.VALUE_QUEEN);
                        }
                        else
                            WP = (WP & notToBoard) | fromBoard;
                        break;
                }

                //black's piece being taken (king can't be taken). This is the creation of a piece
                if (move.takenType != PieceType.None)
                {
                    blackWorth += Constants.GetPieceWorth(move.takenType);
                    blackPieces |= toBoard;
                    switch (move.takenType)
                    {
                        case PieceType.Pawn:
                            BP |= toBoard;
                            break;
                        case PieceType.Knight:
                            BN |= toBoard;
                            break;
                        case PieceType.Bishop:
                            BB |= toBoard;
                            break;
                        case PieceType.Rook:
                            BR |= toBoard;
                            break;
                        case PieceType.Queen:
                            BQ |= toBoard;
                            break;
                        default:
                            break;
                    }
                }
            }
            else //the side that made the move was black
            {                
                blackPieces = (blackPieces & notToBoard) | fromBoard;
                //When a black piece is moved, it can't possibly change the position of other black pieces, 
                //but it might change white pieces
                switch (move.pieceType)
                {
                    case PieceType.Knight:
                        BN = (BN & notToBoard) | fromBoard;
                        break;
                    case PieceType.Bishop:
                        BB = (BB & notToBoard) | fromBoard;
                        break;
                    case PieceType.Rook:
                        BR = (BR & notToBoard) | fromBoard;
                        break;
                    case PieceType.Queen:
                        BQ = (BQ & notToBoard) | fromBoard;
                        break;
                    case PieceType.King:
                        blackKingIndex = move.fromIndex;
                        BK = (BK & notToBoard) | fromBoard;
                        //Castling move, so move the rook
                        if (move.fromIndex == 4) //black castle
                        {
                            if (move.toIndex == 6) //right castle
                            {
                                UInt64 addRook    = (1UL << 7);
                                UInt64 removeRook = ~(1UL << 5);
                                BR &= removeRook;
                                BR |= addRook;
                                blackPieces &= removeRook;
                                blackPieces |= addRook;
                                computerHasCastled = false;
                            }
                            else if (move.toIndex == 2) //left castle
                            {
                                UInt64 addRook    = (1UL << 0);
                                UInt64 removeRook = ~(1UL << 3);
                                BR &= removeRook;
                                BR |= addRook;
                                blackPieces &= removeRook;
                                blackPieces |= addRook;
                                computerHasCastled = false;
                            }
                        }

                        //This is a hack and not necessarily correct. The king may have moved before, etc.
                        //It really requires a history of the board
                        if (move.fromIndex == 4)
                            computerHasCastled = false;
                        break;
                    case PieceType.Pawn:

                        //pawn promotion
                        if (move.toIndex > 55)
                        {
                            BP |= fromBoard;
                            BQ &= notToBoard;
                            blackWorth += (Constants.VALUE_PAWN - Constants.VALUE_QUEEN);
                        }
                        else
                            BP = (BP & notToBoard) | fromBoard;
                        break;
                }

                //white's piece being taken (king can't be taken)
                if (move.takenType != PieceType.None)
                {
                    whiteWorth += Constants.GetPieceWorth(move.takenType);
                    whitePieces |= toBoard;
                    switch (move.takenType)
                    {
                        case PieceType.Pawn:
                            WP |= toBoard;
                            break;
                        case PieceType.Knight:
                            WN |= toBoard;
                            break;
                        case PieceType.Bishop:
                            WB |= toBoard;
                            break;
                        case PieceType.Rook:
                            WR |= toBoard;
                            break;
                        case PieceType.Queen:
                            WQ |= toBoard;
                            break;
                        default:
                            break;
                    }
                }
            }

            occupied = whitePieces | blackPieces;
            empty = ~occupied;

            //decrement hash key with ^. It's faster than recalculating it.
            hashKey = Zobrist.moveChangeHash(hashKey, move, !PlayersTurn);
        }

        public Board copyMake(Move move)
        {
            Board nextBoard = new Board(PlayersTurn);

            nextBoard.playerHasCastled = playerHasCastled;
            nextBoard.computerHasCastled = computerHasCastled;

            UInt64 toBoard = 1UL << move.toIndex;
            UInt64 takenBoard    = ~toBoard;
            UInt64 notFromBoard = ~(1UL << move.fromIndex);

            if (PlayersTurn)
            {
                nextBoard.whiteWorth = whiteWorth;
                
                nextBoard.whitePieces = (whitePieces & notFromBoard) | toBoard;
                //When a white piece is moved, it can't possibly change the position of other white pieces, 
                //but it might change black pieces. Doing it this way is an approx 6% time saving rather
                //than simply applying makeMove() to all pieces.
                switch (move.pieceType)
                {
                    case PieceType.Knight:
                        nextBoard.WN = (WN & notFromBoard) | toBoard;
                        nextBoard.WB = WB;
                        nextBoard.WR = WR;
                        nextBoard.WQ = WQ;
                        nextBoard.WK = WK;
                        nextBoard.WP = WP;
                        break;
                    case PieceType.Bishop:
                        nextBoard.WB = (WB & notFromBoard) | toBoard;
                        nextBoard.WN = WN;
                        nextBoard.WR = WR;
                        nextBoard.WQ = WQ;
                        nextBoard.WK = WK;
                        nextBoard.WP = WP; 
                        break;
                    case PieceType.Rook:
                        nextBoard.WR = (WR & notFromBoard) | toBoard;
                        nextBoard.WN = WN;
                        nextBoard.WB = WB;
                        nextBoard.WQ = WQ;
                        nextBoard.WK = WK;
                        nextBoard.WP = WP;
                        break;
                    case PieceType.Queen:
                        nextBoard.WQ = (WQ & notFromBoard) | toBoard;
                        nextBoard.WN = WN;
                        nextBoard.WB = WB;
                        nextBoard.WR = WR;
                        nextBoard.WK = WK;
                        nextBoard.WP = WP;
                        break;
                    case PieceType.King:
                        nextBoard.WK = (WK & notFromBoard) | toBoard;
                        nextBoard.WR = WR;
                        //castling move, so move the rook
                        if (move.fromIndex == 60) //white castle
                        {
                            if (move.toIndex == 62) //right castle
                            {
                                UInt64 removeRook = ~(1UL << 63);
                                UInt64 addRook = (1UL << 61);
                                nextBoard.WR &= removeRook;
                                nextBoard.WR |= addRook;
                                nextBoard.whitePieces &= removeRook;
                                nextBoard.whitePieces |= addRook;
                            }
                            else if (move.toIndex == 58) //left castle
                            {
                                UInt64 removeRook = ~(1UL << 56);
                                UInt64 addRook = (1UL << 59);
                                nextBoard.WR &= removeRook;
                                nextBoard.WR |= addRook;
                                nextBoard.whitePieces &= removeRook;
                                nextBoard.whitePieces |= addRook;
                            }
                        }
                        nextBoard.WN = WN;
                        nextBoard.WB = WB;
                        nextBoard.WQ = WQ;
                        nextBoard.WP = WP;
                        //King can't castle if it has moved
                        nextBoard.playerHasCastled = true;
                        break;
                    case PieceType.Pawn:
                        nextBoard.WQ = WQ;
                        //pawn promotion
                        if (move.toIndex < 8)
                        {
                            nextBoard.WP = (WP & notFromBoard);
                            nextBoard.WQ |= toBoard;
                            nextBoard.whiteWorth -= Constants.VALUE_PAWN;
                            nextBoard.whiteWorth += Constants.VALUE_QUEEN;
                        }
                        else
                            nextBoard.WP = (WP & notFromBoard) | toBoard;
                        nextBoard.WN = WN;
                        nextBoard.WB = WB;
                        nextBoard.WR = WR;
                        nextBoard.WK = WK;
                        break;
                }

                //black's piece being taken (king can't be taken)

                if (move.takenType != PieceType.None)
                {
                    nextBoard.BN = BN & takenBoard;
                    nextBoard.BB = BB & takenBoard;
                    nextBoard.BR = BR & takenBoard;
                    nextBoard.BQ = BQ & takenBoard;
                    nextBoard.BP = BP & takenBoard;
                    nextBoard.BK = BK;
                    nextBoard.blackPieces = blackPieces & takenBoard;
                    nextBoard.blackWorth = (short)(blackWorth - Constants.GetPieceWorth(move.takenType));
                }
                else
                {
                    nextBoard.BN = BN;
                    nextBoard.BB = BB;
                    nextBoard.BR = BR;
                    nextBoard.BQ = BQ;
                    nextBoard.BP = BP;
                    nextBoard.BK = BK;
                    nextBoard.blackPieces = blackPieces;
                    nextBoard.blackWorth = blackWorth;
                }
            }
            else //black move
            {
                nextBoard.blackWorth = blackWorth;
                
                nextBoard.blackPieces = (blackPieces & notFromBoard) | toBoard;
                //When a black piece is moved, it can't possibly change the position of other black pieces, 
                //but it might change white pieces
                switch (move.pieceType)
                {
                    case PieceType.Knight:
                        nextBoard.BN = (BN & notFromBoard) | toBoard;
                        nextBoard.BB = BB;
                        nextBoard.BR = BR;
                        nextBoard.BQ = BQ;
                        nextBoard.BK = BK;
                        nextBoard.BP = BP;
                        break;
                    case PieceType.Bishop:
                        nextBoard.BB = (BB & notFromBoard) | toBoard;
                        nextBoard.BN = BN;
                        nextBoard.BR = BR;
                        nextBoard.BQ = BQ;
                        nextBoard.BK = BK;
                        nextBoard.BP = BP;
                        break;
                    case PieceType.Rook:
                        nextBoard.BR = (BR & notFromBoard) | toBoard;
                        nextBoard.BN = BN;
                        nextBoard.BB = BB;
                        nextBoard.BQ = BQ;
                        nextBoard.BK = BK;
                        nextBoard.BP = BP;
                        break;
                    case PieceType.Queen:
                        nextBoard.BQ = (BQ & notFromBoard) | toBoard;
                        nextBoard.BN = BN;
                        nextBoard.BB = BB;
                        nextBoard.BR = BR;
                        nextBoard.BK = BK;
                        nextBoard.BP = BP;
                        break;
                    case PieceType.King:
                        nextBoard.BK = (BK & notFromBoard) | toBoard;
                        nextBoard.BR = BR;
                        //Castling move, so move the rook
                        if (move.fromIndex == 4) //black castle
                        {
                            if (move.toIndex == 6) //right castle
                            {
                                UInt64 removeRook = ~(1UL << 7);
                                UInt64 addRook = (1UL << 5);
                                nextBoard.BR &= removeRook;
                                nextBoard.BR |= addRook;
                                nextBoard.blackPieces &= removeRook;
                                nextBoard.blackPieces |= addRook;
                            }
                            else if (move.toIndex == 2) //left castle
                            {
                                UInt64 removeRook = ~(1UL << 0);
                                UInt64 addRook = (1UL << 3);
                                nextBoard.BR &= removeRook;
                                nextBoard.BR |= addRook;
                                nextBoard.blackPieces &= removeRook;
                                nextBoard.blackPieces |= addRook;
                            }
                        }
                        nextBoard.BN = BN;
                        nextBoard.BB = BB;
                        nextBoard.BQ = BQ;
                        nextBoard.BP = BP;
                        //King can't castle if it has moved
                        nextBoard.computerHasCastled = true;
                        break;
                    case PieceType.Pawn:
                        nextBoard.BQ = BQ;
                        //pawn promotion
                        if (move.toIndex > 55)
                        {
                            nextBoard.BP = (BP & notFromBoard);
                            nextBoard.BQ |= toBoard;
                            nextBoard.blackWorth -= Constants.VALUE_PAWN;
                            nextBoard.blackWorth += Constants.VALUE_QUEEN;
                        }
                        else
                            nextBoard.BP = (BP & notFromBoard) | toBoard;
                        nextBoard.BN = BN;
                        nextBoard.BB = BB;
                        nextBoard.BR = BR;
                        nextBoard.BK = BK;
                        break;
                }

                //white's piece being taken (king can't be taken)
                if (move.takenType != PieceType.None)
                {
                    nextBoard.WN = WN & takenBoard;
                    nextBoard.WB = WB & takenBoard;
                    nextBoard.WR = WR & takenBoard;
                    nextBoard.WQ = WQ & takenBoard;
                    nextBoard.WP = WP & takenBoard;
                    nextBoard.WK = WK;
                    nextBoard.whitePieces = whitePieces & takenBoard;
                    nextBoard.whiteWorth = (short)(whiteWorth - Constants.GetPieceWorth(move.takenType));

                }
                else
                {
                    nextBoard.WN = WN;
                    nextBoard.WB = WB;
                    nextBoard.WR = WR;
                    nextBoard.WQ = WQ;
                    nextBoard.WP = WP;
                    nextBoard.WK = WK;
                    nextBoard.whitePieces = whitePieces;
                    nextBoard.whiteWorth = whiteWorth;
                }
            }

            nextBoard.occupied = nextBoard.whitePieces | nextBoard.blackPieces;
            nextBoard.empty = ~nextBoard.occupied;

            nextBoard.setPlayersTurn(!PlayersTurn);
            return nextBoard;
        }

        public List<Move> setup()
        {
            isStalemate = false;
            isCheckmate = false;
            List<Move> legalMoves = new List<Move>(Constants.DEFAULT_MOVE_LIST_SIZE); ; //memory may still exist if it it being looped through

            if ((WK | BK) == occupied) //Only kings exist?
            {
                isStalemate = true;
                return legalMoves;
            }

            isCheck = isKingAttacked(!PlayersTurn);

            //List<Move> pseudoMoves = new List<Move>(Constants.DEFAULT_MOVE_LIST_SIZE);
            //pseudoMoves.AddRange(ExtractAllPseudoMoves(PlayersTurn));

            List<Move> pseudoMoves = new List<Move>(Constants.DEFAULT_MOVE_LIST_SIZE);
            pseudoMoves.AddRange(ExtractPseudoMoves(PieceType.Bishop, PlayersTurn ? WB : BB, PlayersTurn, Bishop.GetAllTargets));
            pseudoMoves.AddRange(ExtractPseudoMoves(PieceType.Rook, PlayersTurn ? WR : BR, PlayersTurn, Rook.GetAllTargets));
            pseudoMoves.AddRange(ExtractPseudoMoves(PieceType.Queen, PlayersTurn ? WQ : BQ, PlayersTurn, Queen.GetAllTargets));
            pseudoMoves.AddRange(ExtractPseudoMoves(PieceType.Pawn, PlayersTurn ? WP : BP, PlayersTurn, Pawn.GetAllTargets));
            pseudoMoves.AddRange(ExtractPseudoMoves(PieceType.Knight, PlayersTurn ? WN : BN, PlayersTurn, Knight.GetAllTargets));
            pseudoMoves.AddRange(ExtractPseudoMoves(PieceType.King, PlayersTurn ? WK : BK, PlayersTurn, King.GetAllTargets));

            //only add the pseudo move as a legal move if it doesn't put them in check	
            foreach (Move pseudoMove in pseudoMoves)
            {
                //Note: Do not call setup() here, we dont need to know the legalMoves for the next board
                makeMove(pseudoMove);
                if (!isKingAttacked(PlayersTurn))
                    legalMoves.Add(pseudoMove);
                reverseMove(pseudoMove);
            }

            if (legalMoves.Count == 0)
            {
                if (isCheck)
                    isCheckmate = true;
                else
                    isStalemate = true;
            }
            return legalMoves;
        }

        //Checking whether the king was attacked in the opposite direction made a huge performance impact. 
        //From 620 boards/ms to 2330 boards/ms. But this function can't determine mobility/attacking scores at the same time.
        public bool isKingAttacked(bool attackingColor)
        {
            #if ZCHECKS
                if (Zobrist.FindBoardWithHash(hashKey, out isCheck)) return isCheck;
            #endif

            byte kingSquare;

            //We are using a time saving trick here. Instead of looking at ALL the opponents moves
            //and checking if they attack a king. We project the moves from the king square (in the opposite direction) to
            //check if they attack the opponents pieces.
            //Keeping track of kings index position in make/reverseMove to avoid a bitScan
            if (attackingColor) //is white attacking black king?
            {
                UInt64 king = BK;
                kingSquare = blackKingIndex;

                if ((((BitOps.OneStepSouthEast(king) | BitOps.OneStepSouthWest(king)) & WP) != 0UL) ||
                    ((Knight.KnightAttacks[kingSquare] & WN) != 0UL) ||
                    ((King.KingAttacks[kingSquare] & WK) != 0UL) ||
                    ((KoggeStone.bishopAttacks(king, empty) & (WB | WQ)) != 0UL) ||
                    ((KoggeStone.rookAttacks(king, empty) & (WR | WQ)) != 0UL))
                {
                    #if ZCHECKS
                        Zobrist.addBoard(hashKey, true, age);
                    #endif
                    return true;
                };
            }
            else //its whites turn. Is the white king being attacked
            {
                UInt64 king = WK;
                kingSquare = whiteKingIndex;

                if ((((BitOps.OneStepNorthEast(king) | BitOps.OneStepNorthWest(king)) & BP) != 0UL) ||
                    ((Knight.KnightAttacks[kingSquare] & BN) != 0UL) ||
                    ((King.KingAttacks[kingSquare] & BK) != 0UL) ||
                    ((KoggeStone.bishopAttacks(king, empty) & (BB | BQ)) != 0UL) ||
                    ((KoggeStone.rookAttacks(king, empty) & (BR | BQ)) != 0UL))
                {
                    #if ZCHECKS
                        Zobrist.addBoard(hashKey, true, age);
                    #endif
                    return true;
                };
            }

            #if ZCHECKS
                Zobrist.addBoard(hashKey, false, age);
            #endif
            return false;
        }

        private List<Move> ExtractPseudoMoves(PieceType type, UInt64 pieces, bool pieceColor, GetTargetsDelegate getTargets)
        {
            List<Move> moves = new List<Move>(Constants.DEFAULT_MOVE_LIST_SIZE);
            while (pieces != 0)
            {
                byte fromIndex = BitOps.BitScanForwardReset(ref pieces); // search for LS1B and then reset it               
                UInt64 targets = getTargets.Invoke(1UL << fromIndex, pieceColor, this);

                while (targets != 0)
                {
                    byte toIndex = BitOps.BitScanForwardReset(ref targets);
                    moves.Add(new Move(fromIndex, toIndex, type, getPieceType(!pieceColor, 1UL << toIndex)));
                }
            }
            return moves;
        }

        //This is about 8% faster, but about 20 times more boards are created overall. Why??? It creates the same number of moves 
        private List<Move> ExtractAllPseudoMoves(bool pieceColor)
        {
            List<Move> moves = new List<Move>(Constants.DEFAULT_MOVE_LIST_SIZE);
            byte fromIndex, toIndex;
            UInt64 piece;
            UInt64 targets;

            if (pieceColor)
            {
                piece = WP;
                while (piece != 0)
                {
                    fromIndex = BitOps.BitScanForwardReset(ref piece); // search for LS1B and then reset it               
                    targets = Pawn.GetAllTargets(1UL << fromIndex, pieceColor, this);

                    while (targets != 0)
                    {
                        toIndex = BitOps.BitScanForwardReset(ref targets);
                        moves.Add(new Move(fromIndex, toIndex, PieceType.Pawn, getPieceType(!pieceColor, 1UL << toIndex)));
                    }
                }
                piece = WN;
                while (piece != 0)
                {
                    fromIndex = BitOps.BitScanForwardReset(ref piece); // search for LS1B and then reset it               
                    targets = Knight.GetAllTargets(1UL << fromIndex, pieceColor, this);

                    while (targets != 0)
                    {
                        toIndex = BitOps.BitScanForwardReset(ref targets);
                        moves.Add(new Move(fromIndex, toIndex, PieceType.Knight, getPieceType(!pieceColor, 1UL << toIndex)));
                    }
                }
                piece = WB;
                while (piece != 0)
                {
                    fromIndex = BitOps.BitScanForwardReset(ref piece); // search for LS1B and then reset it               
                    targets = Bishop.GetAllTargets(1UL << fromIndex, pieceColor, this);

                    while (targets != 0)
                    {
                        toIndex = BitOps.BitScanForwardReset(ref targets);
                        moves.Add(new Move(fromIndex, toIndex, PieceType.Bishop, getPieceType(!pieceColor, 1UL << toIndex)));
                    }
                }
                piece = WR;
                while (piece != 0)
                {
                    fromIndex = BitOps.BitScanForwardReset(ref piece); // search for LS1B and then reset it               
                    targets = Rook.GetAllTargets(1UL << fromIndex, pieceColor, this);

                    while (targets != 0)
                    {
                        toIndex = BitOps.BitScanForwardReset(ref targets);
                        moves.Add(new Move(fromIndex, toIndex, PieceType.Rook, getPieceType(!pieceColor, 1UL << toIndex)));
                    }
                }
                piece = WQ;
                while (piece != 0)
                {
                    fromIndex = BitOps.BitScanForwardReset(ref piece); // search for LS1B and then reset it               
                    targets = Queen.GetAllTargets(1UL << fromIndex, pieceColor, this);

                    while (targets != 0)
                    {
                        toIndex = BitOps.BitScanForwardReset(ref targets);
                        moves.Add(new Move(fromIndex, toIndex, PieceType.Queen, getPieceType(!pieceColor, 1UL << toIndex)));
                    }
                }
                piece = WK;
                while (piece != 0)
                {
                    fromIndex = BitOps.BitScanForwardReset(ref piece); // search for LS1B and then reset it               
                    targets = King.GetAllTargets(1UL << fromIndex, pieceColor, this);

                    while (targets != 0)
                    {
                        toIndex = BitOps.BitScanForwardReset(ref targets);
                        moves.Add(new Move(fromIndex, toIndex, PieceType.King, getPieceType(!pieceColor, 1UL << toIndex)));
                    }
                }
            }
            else //black move
            {
                piece = BP;
                while (piece != 0)
                {
                    fromIndex = BitOps.BitScanForwardReset(ref piece); // search for LS1B and then reset it               
                    targets = Pawn.GetAllTargets(1UL << fromIndex, pieceColor, this);

                    while (targets != 0)
                    {
                        toIndex = BitOps.BitScanForwardReset(ref targets);
                        moves.Add(new Move(fromIndex, toIndex, PieceType.Pawn, getPieceType(!pieceColor, 1UL << toIndex)));
                    }
                }
                piece = BN;
                while (piece != 0)
                {
                    fromIndex = BitOps.BitScanForwardReset(ref piece); // search for LS1B and then reset it               
                    targets = Knight.GetAllTargets(1UL << fromIndex, pieceColor, this);

                    while (targets != 0)
                    {
                        toIndex = BitOps.BitScanForwardReset(ref targets);
                        moves.Add(new Move(fromIndex, toIndex, PieceType.Knight, getPieceType(!pieceColor, 1UL << toIndex)));
                    }
                }
                piece = BB;
                while (piece != 0)
                {
                    fromIndex = BitOps.BitScanForwardReset(ref piece); // search for LS1B and then reset it               
                    targets = Bishop.GetAllTargets(1UL << fromIndex, pieceColor, this);

                    while (targets != 0)
                    {
                        toIndex = BitOps.BitScanForwardReset(ref targets);
                        moves.Add(new Move(fromIndex, toIndex, PieceType.Bishop, getPieceType(!pieceColor, 1UL << toIndex)));
                    }
                }
                piece = BR;
                while (piece != 0)
                {
                    fromIndex = BitOps.BitScanForwardReset(ref piece); // search for LS1B and then reset it               
                    targets = Rook.GetAllTargets(1UL << fromIndex, pieceColor, this);

                    while (targets != 0)
                    {
                        toIndex = BitOps.BitScanForwardReset(ref targets);
                        moves.Add(new Move(fromIndex, toIndex, PieceType.Rook, getPieceType(!pieceColor, 1UL << toIndex)));
                    }
                }
                piece = BQ;
                while (piece != 0)
                {
                    fromIndex = BitOps.BitScanForwardReset(ref piece); // search for LS1B and then reset it               
                    targets = Queen.GetAllTargets(1UL << fromIndex, pieceColor, this);

                    while (targets != 0)
                    {
                        toIndex = BitOps.BitScanForwardReset(ref targets);
                        moves.Add(new Move(fromIndex, toIndex, PieceType.Queen, getPieceType(!pieceColor, 1UL << toIndex)));
                    }
                }
                piece = BK;
                while (piece != 0)
                {
                    fromIndex = BitOps.BitScanForwardReset(ref piece); // search for LS1B and then reset it               
                    targets = King.GetAllTargets(1UL << fromIndex, pieceColor, this);

                    while (targets != 0)
                    {
                        toIndex = BitOps.BitScanForwardReset(ref targets);
                        moves.Add(new Move(fromIndex, toIndex, PieceType.King, getPieceType(!pieceColor, 1UL << toIndex)));
                    }
                }
            }
            return moves;
        }

        //used to determine capture type of move
        private PieceType getPieceType(bool pieceColor, UInt64 bb)
        {
            if(pieceColor)
            {
                //Most of the time there will be no capture, so test this first
                if ((WP & bb) != 0UL) return PieceType.Pawn;
                if ((WN & bb) != 0UL) return PieceType.Knight;
                if ((WB & bb) != 0UL) return PieceType.Bishop;
                if ((WR & bb) != 0UL) return PieceType.Rook;
                if ((WQ & bb) != 0UL) return PieceType.Queen;
            }
            else
            {
                if ((BP & bb) != 0UL) return PieceType.Pawn;
                if ((BN & bb) != 0UL) return PieceType.Knight;
                if ((BB & bb) != 0UL) return PieceType.Bishop;
                if ((BR & bb) != 0UL) return PieceType.Rook;
                if ((BQ & bb) != 0UL) return PieceType.Queen;
            }
            return PieceType.None;
        }


        //****************HELPER FUNCTIONS**********************

        internal UInt64 GetColorPieces(bool pieceColor)
        {
            return pieceColor ? whitePieces : blackPieces;
        }

        internal UInt64 GetEmptySquares()
        {
            return empty;
        }

        internal UInt64 GetOccupiedSquares()
        {
            return occupied;
        }

        //Only used by the GUI
        public PieceType GetPieceTypeAt(byte index)
        {
            if (BitOps.IsBitSet(WP | BP, index)) return PieceType.Pawn;
            if (BitOps.IsBitSet(WN | BN, index)) return PieceType.Knight;
            if (BitOps.IsBitSet(WB | BB, index)) return PieceType.Bishop;
            if (BitOps.IsBitSet(WR | BR, index)) return PieceType.Rook;
            if (BitOps.IsBitSet(WQ | BQ, index)) return PieceType.Queen;
            if (BitOps.IsBitSet(WK | BK, index)) return PieceType.King;

            return PieceType.None;
        }

        //Only used by the GUI
        public bool IsPlayersPieceHere(byte index)
        {
            //check if square is occupied by a white piece
            return BitOps.IsBitSet(GetColorPieces(true), index); 
        }

        public void print() 
        {
            String[,] chessBoard = new String[8,8];
            for (byte i = 0; i < 64; i++)
            {
                chessBoard[Constants.IndexToX(i), Constants.IndexToY(i)] = "  ";
            }
            for (byte i = 0; i < 64; i++)
            {
                if (BitOps.IsBitSet(WP, i)) { chessBoard[Constants.IndexToX(i), Constants.IndexToY(i)] = "WP"; }
                else if (BitOps.IsBitSet(WN, i)) { chessBoard[Constants.IndexToX(i), Constants.IndexToY(i)] = "WN"; }
                else if (BitOps.IsBitSet(WB, i)) { chessBoard[Constants.IndexToX(i), Constants.IndexToY(i)] = "WB"; }
                else if (BitOps.IsBitSet(WR, i)) { chessBoard[Constants.IndexToX(i), Constants.IndexToY(i)] = "WR"; }
                else if (BitOps.IsBitSet(WQ, i)) { chessBoard[Constants.IndexToX(i), Constants.IndexToY(i)] = "WQ"; }
                else if (BitOps.IsBitSet(WK, i)) { chessBoard[Constants.IndexToX(i), Constants.IndexToY(i)] = "WK"; }
                else if (BitOps.IsBitSet(BP, i)) { chessBoard[Constants.IndexToX(i), Constants.IndexToY(i)] = "BP"; }
                else if (BitOps.IsBitSet(BN, i)) { chessBoard[Constants.IndexToX(i), Constants.IndexToY(i)] = "BN"; }
                else if (BitOps.IsBitSet(BB, i)) { chessBoard[Constants.IndexToX(i), Constants.IndexToY(i)] = "BB"; }
                else if (BitOps.IsBitSet(BR, i)) { chessBoard[Constants.IndexToX(i), Constants.IndexToY(i)] = "BR"; }
                else if (BitOps.IsBitSet(BQ, i)) { chessBoard[Constants.IndexToX(i), Constants.IndexToY(i)] = "BQ"; }
                else if (BitOps.IsBitSet(BK, i)) { chessBoard[Constants.IndexToX(i), Constants.IndexToY(i)] = "BK"; }
            }

            for (int i=0;i<8;i++) 
            {
                Console.Write('|');
                for (int j = 0; j < 8;j++)
                {
                    Console.Write(chessBoard[j,i]);
                    Console.Write('|');
                }
                Console.Write('\n');    
            }
            List<Move> moves = setup();
            foreach (Move move in moves)
            {
                switch(move.pieceType)
                {
                    case PieceType.Pawn:
                        Console.Write("Pawn:");
                        break;
                    case PieceType.Rook:
                        Console.Write("Rook:");
                        break;
                    case PieceType.Knight:
                        Console.Write("Knight:");
                        break;
                    case PieceType.Bishop:
                        Console.Write("Bishop:");
                        break;
                    case PieceType.Queen:
                        Console.Write("Queen:");
                        break;
                    case PieceType.King:
                        Console.Write("King:");
                        break;
                }

                Console.WriteLine("(" + move.fromIndex + ") -> (" + move.toIndex + ")");
            }
        }

        public static void PrintBB(UInt64 bb, string title)
        {
            Console.WriteLine(title);
            String[,] chessBoard = new String[8, 8];
            for (byte i = 0; i < 64; i++)
            {
                //Console.WriteLine("i:" + i + " x:" + IndexToX(i) + " y:" + IndexToY(i));
                chessBoard[Constants.IndexToX(i), Constants.IndexToY(i)] = "0";
            }
            for (byte i = 0; i < 64; i++)
            {
                if (BitOps.IsBitSet(bb, i)) { chessBoard[Constants.IndexToX(i), Constants.IndexToY(i)] = "1"; }
            }

            for (int i = 0; i < 8; i++)
            {
                Console.Write("|");
                for (int j = 0; j < 8; j++)
                {
                    Console.Write(chessBoard[j, i]);
                    Console.Write("|");
                }
                Console.Write("\n");
            }
            Console.Write("\n");
        }

        public List<Move> sortSetupScore()
        {
            List<MoveAndScore> moveAndScores = new List<MoveAndScore>();

            //Simple black move ordering
            //Calculate all scores for the next 2 levels down in the tree. (Will take less than a ms)
            //This is a simple assumption. Take the maximum minimum.
            List<Move> blackMoves = setup();
            foreach (Move move in blackMoves)
            {
                int bestScoreToAdd;
                makeMove(move);
                List<Move> whiteMoves = setup();

                if (isGameOver())
                {
                    bestScoreToAdd = getTotalScore();
                    MoveAndScore moveAndScore = new MoveAndScore(move, bestScoreToAdd);
                    moveAndScores.Add(moveAndScore);
                }
                else
                {
                    if (PlayersTurn)
                    {
                        bestScoreToAdd = -Constants.MATE_SCORE; //assume the worst
                        foreach (Move nextMove in whiteMoves)
                        {
                            makeMove(nextMove);
                            setup();
                            bestScoreToAdd = Math.Max(getTotalScore(), bestScoreToAdd);
                            reverseMove(nextMove);
                        }
                    }
                    else //white to play
                    {
                        bestScoreToAdd = Constants.MATE_SCORE; //assume the worst
                        foreach (Move nextMove in whiteMoves)
                        {
                            makeMove(nextMove);
                            setup();
                            bestScoreToAdd = Math.Min(getTotalScore(), bestScoreToAdd);
                            reverseMove(nextMove);
                        }
                    }
                    MoveAndScore moveAndScore = new MoveAndScore(move, bestScoreToAdd);
                    moveAndScores.Add(moveAndScore);
                }
                reverseMove(move);
            }

            //sort moves based on score
            if (!PlayersTurn) //ascending order
                moveAndScores.Sort(delegate(MoveAndScore x, MoveAndScore y)
                { return x.score.CompareTo(y.score); });
            else //descending order
                moveAndScores.Sort(delegate(MoveAndScore x, MoveAndScore y)
                { return y.score.CompareTo(x.score); });

            blackMoves.Clear();
            foreach (MoveAndScore moveAndScore in moveAndScores)
                blackMoves.Add(moveAndScore.move);
            return blackMoves;
        }

        //unused. I think is ascending order, which is wrong
        //public void sortLegalMovesTakenDifference()
        //{
        //    legalMoves.Sort(delegate(Move x, Move y)
        //    {
        //        return (x.takenType - x.pieceType).CompareTo(x.takenType - x.pieceType);
        //    });
        //}

        public void setStalemate()
        {
            isStalemate = true;
        }

        public bool isPlayersTurn()
        {
            return PlayersTurn;
        }

        public void setPlayersTurn(bool playersTurn)
        {
            this.PlayersTurn = playersTurn;
        }

        public bool isGameOver()
        {
            return (isCheckmate || isStalemate);
        }

        public bool isCheckMate()
        {
            return isCheckmate;
        }

        public bool isStaleMate()
        {
            return isStalemate;
        }

        public bool Equals(Board other)
        {
            if (WP != other.WP)
                return false;
            if (WN != other.WN)
                return false;
            if (WB != other.WB)
                return false;
            if (WR != other.WR)
                return false;
            if (WQ != other.WQ)
                return false;
            if (WK != other.WK)
                return false;

            if (BP != other.BP)
                return false;
            if (BN != other.BN)
                return false;
            if (BB != other.BB)
                return false;
            if (BR != other.BR)
                return false;
            if (BQ != other.BQ)
                return false;
            if (BK != other.BK)
                return false;

            if (PlayersTurn != other.PlayersTurn)
                return false;

            if (whitePieces != other.whitePieces)
                return false;
            if (blackPieces != other.blackPieces)
                return false;
            if (occupied != other.occupied)
                return false;
            if (empty != other.empty)
                return false;

            if (whiteWorth != other.whiteWorth)
                return false;
            if (blackWorth != other.blackWorth)
                return false;
            if (ply != other.ply)
                return false;
            if (hashKey != other.hashKey)
                return false;

            return true;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
