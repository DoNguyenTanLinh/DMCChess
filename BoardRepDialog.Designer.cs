namespace ForcedChess
{
    partial class BoardRepDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.rbBitboards = new System.Windows.Forms.RadioButton();
            this.rbList = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnNewGame
            // 
            this.btnNewGame.Location = new System.Drawing.Point(96, 66);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(75, 37);
            this.btnNewGame.TabIndex = 0;
            this.btnNewGame.Text = "New Game";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // rbBitboards
            // 
            this.rbBitboards.AutoSize = true;
            this.rbBitboards.Checked = true;
            this.rbBitboards.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBitboards.Location = new System.Drawing.Point(74, 12);
            this.rbBitboards.Name = "rbBitboards";
            this.rbBitboards.Size = new System.Drawing.Size(86, 21);
            this.rbBitboards.TabIndex = 1;
            this.rbBitboards.TabStop = true;
            this.rbBitboards.Text = "Bitboards";
            this.rbBitboards.UseVisualStyleBackColor = true;
            // 
            // rbList
            // 
            this.rbList.AutoSize = true;
            this.rbList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbList.Location = new System.Drawing.Point(74, 39);
            this.rbList.Name = "rbList";
            this.rbList.Size = new System.Drawing.Size(110, 21);
            this.rbList.TabIndex = 2;
            this.rbList.TabStop = true;
            this.rbList.Text = "List of Pieces";
            this.rbList.UseVisualStyleBackColor = true;
            // 
            // BoardRepDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 114);
            this.Controls.Add(this.rbList);
            this.Controls.Add(this.rbBitboards);
            this.Controls.Add(this.btnNewGame);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "BoardRepDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Internal Board Representation";
            this.Shown += new System.EventHandler(this.BoardRepDialog_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNewGame;
        private System.Windows.Forms.RadioButton rbBitboards;
        private System.Windows.Forms.RadioButton rbList;
    }
}