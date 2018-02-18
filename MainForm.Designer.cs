namespace DMCChess
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// 
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnNewGame = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rbEasy = new System.Windows.Forms.RadioButton();
            this.rbIntermediate = new System.Windows.Forms.RadioButton();
            this.rbHard = new System.Windows.Forms.RadioButton();
            this.rbVeryHard = new System.Windows.Forms.RadioButton();
            this.gbDifficulty = new System.Windows.Forms.GroupBox();
            this.rbNightmare = new System.Windows.Forms.RadioButton();
            this.rbAlien = new System.Windows.Forms.RadioButton();
            this.rbRandom = new System.Windows.Forms.RadioButton();
            this.rbExtreme = new System.Windows.Forms.RadioButton();
            this.btnUndo = new System.Windows.Forms.Button();
            this.lblFeedBack = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblLastMoveTime = new System.Windows.Forms.Label();
            this.lblBoardsVisited = new System.Windows.Forms.Label();
            this.lblBoards = new System.Windows.Forms.Label();
            this.lblTimePerBoard = new System.Windows.Forms.Label();
            this.lblTimeBoard = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblZobrist = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblZobristTotal = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblMemory = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblAvgTime = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblBWorth = new System.Windows.Forms.Label();
            this.lblWWorth = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.gbDifficulty.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNewGame
            // 
            this.btnNewGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewGame.BackColor = System.Drawing.SystemColors.Menu;
            this.btnNewGame.Location = new System.Drawing.Point(729, 332);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(60, 39);
            this.btnNewGame.TabIndex = 65;
            this.btnNewGame.Text = "New Game";
            this.btnNewGame.UseVisualStyleBackColor = false;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(10, 721);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(710, 23);
            this.progressBar1.TabIndex = 66;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(810, 712);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 12);
            this.label1.TabIndex = 67;
            this.label1.Text = "Daniel Campbell";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(833, 724);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 12);
            this.label2.TabIndex = 68;
            this.label2.Text = "2017";
            // 
            // rbEasy
            // 
            this.rbEasy.AutoSize = true;
            this.rbEasy.Checked = true;
            this.rbEasy.Location = new System.Drawing.Point(6, 40);
            this.rbEasy.Name = "rbEasy";
            this.rbEasy.Size = new System.Drawing.Size(46, 17);
            this.rbEasy.TabIndex = 69;
            this.rbEasy.TabStop = true;
            this.rbEasy.Text = "Frog";
            this.rbEasy.UseVisualStyleBackColor = true;
            // 
            // rbIntermediate
            // 
            this.rbIntermediate.AutoSize = true;
            this.rbIntermediate.Location = new System.Drawing.Point(6, 60);
            this.rbIntermediate.Name = "rbIntermediate";
            this.rbIntermediate.Size = new System.Drawing.Size(62, 17);
            this.rbIntermediate.TabIndex = 70;
            this.rbIntermediate.Text = "Donkey";
            this.rbIntermediate.UseVisualStyleBackColor = true;
            // 
            // rbHard
            // 
            this.rbHard.AutoSize = true;
            this.rbHard.Location = new System.Drawing.Point(6, 80);
            this.rbHard.Name = "rbHard";
            this.rbHard.Size = new System.Drawing.Size(55, 17);
            this.rbHard.TabIndex = 71;
            this.rbHard.Text = "Spider";
            this.rbHard.UseVisualStyleBackColor = true;
            // 
            // rbVeryHard
            // 
            this.rbVeryHard.AutoSize = true;
            this.rbVeryHard.Location = new System.Drawing.Point(6, 100);
            this.rbVeryHard.Name = "rbVeryHard";
            this.rbVeryHard.Size = new System.Drawing.Size(54, 17);
            this.rbVeryHard.TabIndex = 72;
            this.rbVeryHard.Text = "Chimp";
            this.rbVeryHard.UseVisualStyleBackColor = true;
            // 
            // gbDifficulty
            // 
            this.gbDifficulty.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.gbDifficulty.Controls.Add(this.rbNightmare);
            this.gbDifficulty.Controls.Add(this.rbAlien);
            this.gbDifficulty.Controls.Add(this.rbRandom);
            this.gbDifficulty.Controls.Add(this.rbExtreme);
            this.gbDifficulty.Controls.Add(this.rbEasy);
            this.gbDifficulty.Controls.Add(this.rbVeryHard);
            this.gbDifficulty.Controls.Add(this.rbIntermediate);
            this.gbDifficulty.Controls.Add(this.rbHard);
            this.gbDifficulty.Location = new System.Drawing.Point(795, 294);
            this.gbDifficulty.Name = "gbDifficulty";
            this.gbDifficulty.Size = new System.Drawing.Size(88, 185);
            this.gbDifficulty.TabIndex = 73;
            this.gbDifficulty.TabStop = false;
            this.gbDifficulty.Text = "Difficulty";
            // 
            // rbNightmare
            // 
            this.rbNightmare.AutoSize = true;
            this.rbNightmare.Location = new System.Drawing.Point(6, 160);
            this.rbNightmare.Name = "rbNightmare";
            this.rbNightmare.Size = new System.Drawing.Size(73, 17);
            this.rbNightmare.TabIndex = 76;
            this.rbNightmare.Text = "Nightmare";
            this.rbNightmare.UseVisualStyleBackColor = true;
            // 
            // rbAlien
            // 
            this.rbAlien.AutoSize = true;
            this.rbAlien.Location = new System.Drawing.Point(6, 140);
            this.rbAlien.Name = "rbAlien";
            this.rbAlien.Size = new System.Drawing.Size(48, 17);
            this.rbAlien.TabIndex = 75;
            this.rbAlien.Text = "Alien";
            this.rbAlien.UseVisualStyleBackColor = true;
            // 
            // rbRandom
            // 
            this.rbRandom.AutoSize = true;
            this.rbRandom.Location = new System.Drawing.Point(6, 20);
            this.rbRandom.Name = "rbRandom";
            this.rbRandom.Size = new System.Drawing.Size(44, 17);
            this.rbRandom.TabIndex = 74;
            this.rbRandom.Text = "Fish";
            this.rbRandom.UseVisualStyleBackColor = true;
            // 
            // rbExtreme
            // 
            this.rbExtreme.AutoSize = true;
            this.rbExtreme.Location = new System.Drawing.Point(6, 120);
            this.rbExtreme.Name = "rbExtreme";
            this.rbExtreme.Size = new System.Drawing.Size(59, 17);
            this.rbExtreme.TabIndex = 73;
            this.rbExtreme.Text = "Human";
            this.rbExtreme.UseVisualStyleBackColor = true;
            // 
            // btnUndo
            // 
            this.btnUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUndo.BackColor = System.Drawing.SystemColors.Menu;
            this.btnUndo.Enabled = false;
            this.btnUndo.Location = new System.Drawing.Point(729, 381);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(60, 25);
            this.btnUndo.TabIndex = 74;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = false;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // lblFeedBack
            // 
            this.lblFeedBack.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFeedBack.AutoSize = true;
            this.lblFeedBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFeedBack.ForeColor = System.Drawing.Color.Tomato;
            this.lblFeedBack.Location = new System.Drawing.Point(806, 594);
            this.lblFeedBack.Name = "lblFeedBack";
            this.lblFeedBack.Size = new System.Drawing.Size(56, 17);
            this.lblFeedBack.TabIndex = 75;
            this.lblFeedBack.Text = "Check!";
            this.lblFeedBack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTime
            // 
            this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(815, 25);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(13, 13);
            this.lblTime.TabIndex = 109;
            this.lblTime.Text = "0";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLastMoveTime
            // 
            this.lblLastMoveTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLastMoveTime.AutoSize = true;
            this.lblLastMoveTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastMoveTime.Location = new System.Drawing.Point(807, 13);
            this.lblLastMoveTime.Name = "lblLastMoveTime";
            this.lblLastMoveTime.Size = new System.Drawing.Size(64, 9);
            this.lblLastMoveTime.TabIndex = 110;
            this.lblLastMoveTime.Text = "Move Time (sec)";
            // 
            // lblBoardsVisited
            // 
            this.lblBoardsVisited.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBoardsVisited.AutoSize = true;
            this.lblBoardsVisited.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBoardsVisited.Location = new System.Drawing.Point(809, 83);
            this.lblBoardsVisited.Name = "lblBoardsVisited";
            this.lblBoardsVisited.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblBoardsVisited.Size = new System.Drawing.Size(58, 9);
            this.lblBoardsVisited.TabIndex = 111;
            this.lblBoardsVisited.Text = "Boards Created";
            // 
            // lblBoards
            // 
            this.lblBoards.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBoards.AutoSize = true;
            this.lblBoards.Location = new System.Drawing.Point(815, 95);
            this.lblBoards.Name = "lblBoards";
            this.lblBoards.Size = new System.Drawing.Size(13, 13);
            this.lblBoards.TabIndex = 112;
            this.lblBoards.Text = "0";
            this.lblBoards.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTimePerBoard
            // 
            this.lblTimePerBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimePerBoard.AutoSize = true;
            this.lblTimePerBoard.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimePerBoard.Location = new System.Drawing.Point(809, 118);
            this.lblTimePerBoard.Name = "lblTimePerBoard";
            this.lblTimePerBoard.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTimePerBoard.Size = new System.Drawing.Size(55, 9);
            this.lblTimePerBoard.TabIndex = 114;
            this.lblTimePerBoard.Text = "Boards per ms";
            // 
            // lblTimeBoard
            // 
            this.lblTimeBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeBoard.AutoSize = true;
            this.lblTimeBoard.Location = new System.Drawing.Point(815, 130);
            this.lblTimeBoard.Name = "lblTimeBoard";
            this.lblTimeBoard.Size = new System.Drawing.Size(13, 13);
            this.lblTimeBoard.TabIndex = 115;
            this.lblTimeBoard.Text = "0";
            this.lblTimeBoard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(809, 188);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(72, 9);
            this.label3.TabIndex = 116;
            this.label3.Text = "Zobrist Hash Found";
            // 
            // lblZobrist
            // 
            this.lblZobrist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblZobrist.AutoSize = true;
            this.lblZobrist.Location = new System.Drawing.Point(815, 200);
            this.lblZobrist.Name = "lblZobrist";
            this.lblZobrist.Size = new System.Drawing.Size(13, 13);
            this.lblZobrist.TabIndex = 117;
            this.lblZobrist.Text = "0";
            this.lblZobrist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(809, 223);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label4.Size = new System.Drawing.Size(48, 9);
            this.label4.TabIndex = 119;
            this.label4.Text = "Zobrist Total";
            // 
            // lblZobristTotal
            // 
            this.lblZobristTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblZobristTotal.AutoSize = true;
            this.lblZobristTotal.Location = new System.Drawing.Point(815, 235);
            this.lblZobristTotal.Name = "lblZobristTotal";
            this.lblZobristTotal.Size = new System.Drawing.Size(13, 13);
            this.lblZobristTotal.TabIndex = 120;
            this.lblZobristTotal.Text = "0";
            this.lblZobristTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(809, 153);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label5.Size = new System.Drawing.Size(67, 9);
            this.label5.TabIndex = 122;
            this.label5.Text = "Memory Used Mb";
            // 
            // lblMemory
            // 
            this.lblMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMemory.AutoSize = true;
            this.lblMemory.Location = new System.Drawing.Point(815, 165);
            this.lblMemory.Name = "lblMemory";
            this.lblMemory.Size = new System.Drawing.Size(13, 13);
            this.lblMemory.TabIndex = 123;
            this.lblMemory.Text = "0";
            this.lblMemory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(807, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 9);
            this.label6.TabIndex = 128;
            this.label6.Text = "Avg Move Time (sec)";
            // 
            // lblAvgTime
            // 
            this.lblAvgTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAvgTime.AutoSize = true;
            this.lblAvgTime.Location = new System.Drawing.Point(815, 60);
            this.lblAvgTime.Name = "lblAvgTime";
            this.lblAvgTime.Size = new System.Drawing.Size(13, 13);
            this.lblAvgTime.TabIndex = 127;
            this.lblAvgTime.Text = "0";
            this.lblAvgTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(810, 494);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label7.Size = new System.Drawing.Size(47, 9);
            this.label7.TabIndex = 129;
            this.label7.Text = "Black Worth";
            // 
            // lblBWorth
            // 
            this.lblBWorth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBWorth.AutoSize = true;
            this.lblBWorth.Location = new System.Drawing.Point(825, 503);
            this.lblBWorth.Name = "lblBWorth";
            this.lblBWorth.Size = new System.Drawing.Size(13, 13);
            this.lblBWorth.TabIndex = 130;
            this.lblBWorth.Text = "0";
            this.lblBWorth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWWorth
            // 
            this.lblWWorth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWWorth.AutoSize = true;
            this.lblWWorth.Location = new System.Drawing.Point(826, 534);
            this.lblWWorth.Name = "lblWWorth";
            this.lblWWorth.Size = new System.Drawing.Size(13, 13);
            this.lblWWorth.TabIndex = 132;
            this.lblWWorth.Text = "0";
            this.lblWWorth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(811, 525);
            this.label10.Name = "label10";
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label10.Size = new System.Drawing.Size(48, 9);
            this.label10.TabIndex = 131;
            this.label10.Text = "White Worth";
            // 
            // lblScore
            // 
            this.lblScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScore.AutoSize = true;
            this.lblScore.Location = new System.Drawing.Point(827, 561);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(13, 13);
            this.lblScore.TabIndex = 134;
            this.lblScore.Text = "0";
            this.lblScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(812, 552);
            this.label12.Name = "label12";
            this.label12.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label12.Size = new System.Drawing.Size(44, 9);
            this.label12.TabIndex = 133;
            this.label12.Text = "Total Score";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 745);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblWWorth);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblBWorth);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblAvgTime);
            this.Controls.Add(this.lblMemory);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblZobristTotal);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblZobrist);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTimeBoard);
            this.Controls.Add(this.lblTimePerBoard);
            this.Controls.Add(this.lblBoards);
            this.Controls.Add(this.lblBoardsVisited);
            this.Controls.Add(this.lblLastMoveTime);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblFeedBack);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.gbDifficulty);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnNewGame);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DMC Chess";
            this.gbDifficulty.ResumeLayout(false);
            this.gbDifficulty.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNewGame;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbEasy;
        private System.Windows.Forms.RadioButton rbIntermediate;
        private System.Windows.Forms.RadioButton rbHard;
        private System.Windows.Forms.RadioButton rbVeryHard;
        private System.Windows.Forms.GroupBox gbDifficulty;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Label lblFeedBack;
        private System.Windows.Forms.RadioButton rbExtreme;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblLastMoveTime;
        private System.Windows.Forms.Label lblBoardsVisited;
        private System.Windows.Forms.Label lblBoards;
        private System.Windows.Forms.RadioButton rbRandom;
        private System.Windows.Forms.Label lblTimePerBoard;
        private System.Windows.Forms.Label lblTimeBoard;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblZobrist;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblZobristTotal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblMemory;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblAvgTime;
        private System.Windows.Forms.RadioButton rbAlien;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblBWorth;
        private System.Windows.Forms.Label lblWWorth;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton rbNightmare;
    }
}

