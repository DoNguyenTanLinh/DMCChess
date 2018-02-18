using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ForcedChess
{
    public partial class BoardRepDialog : Form
    {
        public static bool useBitBoards = true;
        public BoardRepDialog(bool useBB)
        {
            useBitBoards = useBB;                      
            InitializeComponent();                       
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            useBitBoards = rbBitboards.Checked;
            DialogResult = DialogResult.OK;
        }

        private void BoardRepDialog_Shown(object sender, EventArgs e)
        {
            rbBitboards.Checked = useBitBoards;
            rbList.Checked = !useBitBoards;
        }
    }
}
