using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
//using System.Data.SQLite;

namespace DMCChess
{
    public partial class MainForm : Form
    {
        private int squareWidth = 65;
        private const int LOST_WIDTH = 25;

        private Game game;

        private bool firstPick = true;
        private PictureBox firstPickBox;
        private PictureBox[] chessboardPicBox;
        private PictureBox[] lostBlackPawns;
        private PictureBox[] lostBlackPieces;
        private PictureBox[] lostWhitePawns;
        private PictureBox[] lostWhitePieces;
        private Stopwatch sw;

        public MainForm()
        {
            InitializeComponent();
            Zobrist.zobristFillArray();
            King.InitKingAttacks();
            Knight.InitKnightAttacks();
            //Magic.initmagicmoves();
            //RayAttacks.initRayAttacks();
            //Quintessence.initQuintessence();

            game = new Game();
            sw = new Stopwatch();

            squareWidth = progressBar1.Width/8;
            InitialiseGUI();
        }

        private byte getDifficulty()
        {
            if (rbRandom.Checked)
                return 0;
            if (rbEasy.Checked)
                return 1;
            else if (rbIntermediate.Checked)
                return 2;
            else if (rbHard.Checked)
                return 3;
            else if (rbVeryHard.Checked)
                return 4;
            else if (rbExtreme.Checked)
                return 5;
            else if (rbAlien.Checked)
                return 6;
            else if (rbNightmare.Checked)
                return 7;
            return 0;
        }

        //might use this somehow
        private double getDepthLeafCount(int depth)
        {
            //https://chessprogramming.wikispaces.com/Perft+Results
            switch(depth)
            {
                case 0:
                    return 1;
                case 1:
                    return 20;
                case 2:
                    return 400;
                case 3:
                    return 8902;
                case 4:
                    return 197281;
                case 5:
                    return 4865609;
                case 6:
                    return 119060324;
                case 7:
                    return 3195901860;
                case 8:
                    return 84998978956;
            }
            return 0;
        }

        private void InitialiseGUI()
        {
            //put pictureboxes into array
            chessboardPicBox = new System.Windows.Forms.PictureBox[64];

            for (byte i = 0; i < 64; i++)
            {
                int x = 10 + squareWidth * Constants.IndexToX(i);
                int y = 10 + squareWidth * Constants.IndexToY(i);
                chessboardPicBox[i] = CreatePB(x, y, squareWidth);
                chessboardPicBox[i].Click += pictureBox_Click;
                chessboardPicBox[i].Name = "pb" + i;
            }

            lostBlackPawns = new System.Windows.Forms.PictureBox[8];
            lostBlackPieces = new System.Windows.Forms.PictureBox[8];
            lostWhitePawns = new System.Windows.Forms.PictureBox[8];
            lostWhitePieces = new System.Windows.Forms.PictureBox[8];

            for (byte i = 0; i < 8; i++)
            {
                int x = 730;
                int y = 15 + LOST_WIDTH*i;
                lostBlackPieces[i] = CreatePB(x, y, LOST_WIDTH);
                y = 690 - LOST_WIDTH * i;
                lostWhitePieces[i] = CreatePB(x, y, LOST_WIDTH);

                x += 35;
                y = 15 + LOST_WIDTH*i;
                lostBlackPawns[i] = CreatePB(x, y, LOST_WIDTH);
                y = 690 - LOST_WIDTH * i;
                lostWhitePawns[i] = CreatePB(x, y, LOST_WIDTH);
            }

            ResetGUI();
        }

        private PictureBox CreatePB(int x, int y, int width)
        {
            PictureBox pb = new PictureBox();
            pb.Parent = this;
            pb.Location = new Point(x, y);
            pb.Height = width;
            pb.Width = width;
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            return pb;
        }

        private void ResetGUI()
        {
            lblBoards.Text = "";
            lblTime.Text = "";
            lblAvgTime.Text = "";
            lblTimeBoard.Text = "";
            lblZobrist.Text = "";
            lblZobristTotal.Text = "";

            //clear empty pieces images
            for (byte i = 0; i < 8; i++)
            {
                lostBlackPawns[i].Image = null;
                lostBlackPieces[i].Image = null;
                lostWhitePawns[i].Image = null;
                lostWhitePieces[i].Image = null;
            }

            PaintOrigColors();
            PaintImages();
            UpdateFeedBack();
            Refresh();
        }

        private void PaintOrigColors()
        {
            for (byte i = 0; i < 64; i++)
            {
                chessboardPicBox[i].BackColor = GetOrigColor(i);
            }
            firstPick = true;
        }

        private Image GetImageOfPiece(bool isWhite, PieceType aPieceType)
        {
            if (aPieceType == PieceType.None) return null;

            switch (aPieceType)
            {
                case PieceType.Pawn:
                    if (isWhite)
                        return global::DMCChess.Properties.Resources.WP;
                    else
                        return global::DMCChess.Properties.Resources.BP;
                case PieceType.Rook:
                    if (isWhite)
                        return global::DMCChess.Properties.Resources.WC;
                    else
                        return global::DMCChess.Properties.Resources.BC;
                case PieceType.Knight:
                    if (isWhite)
                        return global::DMCChess.Properties.Resources.WKN;
                    else
                        return global::DMCChess.Properties.Resources.BKN;
                case PieceType.Bishop:
                    if (isWhite)
                        return global::DMCChess.Properties.Resources.WB;
                    else
                        return global::DMCChess.Properties.Resources.BB;
                case PieceType.Queen:
                    if (isWhite)
                        return global::DMCChess.Properties.Resources.WQ;
                    else
                        return global::DMCChess.Properties.Resources.BQ;
                case PieceType.King:
                    if (isWhite)
                        return global::DMCChess.Properties.Resources.WK;
                    else
                        return global::DMCChess.Properties.Resources.BK;
                default:
                    return null;
            }
        }

        private void PaintImages()
        {
            for (byte i = 0; i < 64; i++)
                    chessboardPicBox[i].Image = GetImageOfPiece(game.mainBoard.IsPlayersPieceHere(i), game.mainBoard.GetPieceTypeAt(i));
            PaintLostImages();
        }

        private void PaintLostImages()
        {
            //black pawns
            int i = 0;
            foreach(PieceType pt in game.lostBlackPawns)
            {
                lostBlackPawns[i].Image = GetImageOfPiece(false, PieceType.Pawn);
                i++;
            }

            i = 0;
            foreach (PieceType pt in game.lostBlackPieces)
            {
                lostBlackPieces[i].Image = GetImageOfPiece(false, pt);
                i++;
            }

            i = 0;
            foreach (PieceType pt in game.lostWhitePawns)
            {
                lostWhitePawns[i].Image = GetImageOfPiece(true, PieceType.Pawn);
                i++;
            }

            i = 0;
            foreach (PieceType pt in game.lostWhitePieces)
            {
                lostWhitePieces[i].Image = GetImageOfPiece(true, pt);
                i++;
            }
        }

        private void IncProgress()
        {
            progressBar1.Increment(1);
        }

        private byte GetIndexFromPictureBox(PictureBox PB)
        {
            return (byte)Convert.ToInt32(PB.Name.Remove(0, 2));
        }

        private Color GetOrigColor(byte index)
        {
            byte x = Constants.IndexToX(index);
            byte y = Constants.IndexToY(index);
            if ((y % 2) == 0) //y is even
            {
                if ((x % 2) == 0)
                    return Color.White;
                else
                    return Color.Gray;
            }
            else //y is odd
            {
                if ((x % 2) == 0)
                    return Color.Gray;
                else
                    return Color.White;
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            if (game.mainBoard.isGameOver()) return;

            PictureBox pb = (PictureBox)sender;
            byte fromIndex;

            if (pb.BackColor != Color.Red) //first time picking square, or picking different square
            {                
                if(firstPick)
                {
                    fromIndex = GetIndexFromPictureBox(pb);
                    bool firstIsPlayers = game.mainBoard.IsPlayersPieceHere(fromIndex);

                    if (firstIsPlayers)
                    {
                        firstPickBox = pb;
                        firstPickBox.BackColor = Color.Red;
                        firstPick = false;

                        //color legal squares green
                        List<byte> legalSquares = game.GetLegalSquares(fromIndex);
                        foreach (byte legalSquare in legalSquares)
                        {
                            PictureBox arrayPB = chessboardPicBox[legalSquare];
                            arrayPB.BackColor = Color.Green;
                        }
                    }

                }
                else //secondPick, but not the same square(otherwise it would have been red)
                {
                    firstPick = true;
                    //move the image from one PictureBox to another
                    fromIndex = GetIndexFromPictureBox(firstPickBox);
                    List<byte> legalSquares = game.GetLegalSquares(fromIndex);
                    byte toIndex = GetIndexFromPictureBox(pb);

                    //change the legal squares from green back to original color
                    foreach (byte legalSquare in legalSquares)
                    {
                        PictureBox arrayPB = chessboardPicBox[legalSquare];
                        arrayPB.BackColor = GetOrigColor(legalSquare);
                    }

                    if (game.IsMoveLegal(fromIndex, toIndex))
                    {
                        pb.BackColor = GetOrigColor(toIndex);
                        firstPickBox.BackColor = GetOrigColor(fromIndex);
                        //tell the chess engine the pieces have moved
                        game.MakePlayerMove(fromIndex, toIndex);
                        PaintImages();
                        UpdateFeedBack();
                        Refresh();
                        List<Move> moves = game.mainBoard.setup();
                        if(!game.mainBoard.isGameOver())
                        {
                            progressBar1.Maximum = moves.Count;                            
                            sw.Restart();
                            game.MakeBestComputerMove(getDifficulty(), IncProgress);
                            sw.Stop();

                            progressBar1.Value = 0;
                            PaintImages();
                            UpdateFeedBack();
                            btnUndo.Enabled = true;
                        }
                    }
                    else
                        firstPickBox.BackColor = GetOrigColor(fromIndex);
                }
            }
            else //square is red so turn it back, this must be a cancel type move
            {
                firstPick = true;
                fromIndex = GetIndexFromPictureBox(firstPickBox);
                pb.BackColor = GetOrigColor(fromIndex);

                //change the legal squares from green back to original color
                
                List<byte> legalSquares = game.GetLegalSquares(fromIndex);
                foreach (byte legalSquare in legalSquares)
                {
                    PictureBox PB = chessboardPicBox[legalSquare];
                    PB.BackColor = GetOrigColor(legalSquare);
                }
            }
        }

        private void UpdateFeedBack()
        {
            if (game.mainBoard.isCheckMate())
                lblFeedBack.Text = "Checkmate!";
            else if (game.mainBoard.isStaleMate())
                lblFeedBack.Text = "Stalemate!";
            else if (game.mainBoard.isCheck)
                lblFeedBack.Text = "Check!";
            else
                lblFeedBack.Text = "";

            if (game.mainBoard.isPlayersTurn())
            {
                lblBoards.Text = Board.boardsCreated.ToString();
                game.totalTimeSoFar += sw.ElapsedMilliseconds;
                lblTime.Text = Math.Round(((decimal)sw.ElapsedMilliseconds / (decimal)1000), 4).ToString();
                if (game.totalMovesMade != 0)
                    lblAvgTime.Text = Math.Round(((decimal)game.totalTimeSoFar / (decimal)(1000 * game.totalMovesMade)), 2).ToString();
                if ((decimal)sw.ElapsedMilliseconds != 0)
                {
                    int boardsPerSec = (int)((decimal)Board.boardsCreated / (decimal)sw.ElapsedMilliseconds);
                    lblTimeBoard.Text = boardsPerSec.ToString();
                }
                else
                {
                    lblTimeBoard.Text = "???";
                }
                lblZobrist.Text = Zobrist.BoardsFound.ToString();
                lblZobristTotal.Text = (Zobrist.boards.Count).ToString();
            }

            long memoryUsed = GC.GetTotalMemory(true);
            lblMemory.Text = Math.Round(((decimal)memoryUsed/(decimal)1000000), 2).ToString();

            Board mb = game.mainBoard;
            lblBWorth.Text = mb.blackWorth.ToString();
            lblWWorth.Text = mb.whiteWorth.ToString();
            lblScore.Text = mb.getTotalScore().ToString();
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            game = new Game();
            firstPick = true;
            ResetGUI();
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            game.UndoMove(); //undo black's move
            if (game.UndoMove()) //undo white's move
            {
                btnUndo.Enabled = false;
            }
            PaintOrigColors();
            PaintImages();
            UpdateFeedBack();
        }

    }
}
