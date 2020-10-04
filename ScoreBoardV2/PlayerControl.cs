using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScoreBoardV2
{
    public partial class PlayerControl : UserControl
    {
        private string _playerName;

        public string PlayerName
        {
            get { return _playerName; }
            set 
            { 
                _playerName = value;
                label2.Text = _playerName;
            }
        }

        private int _playerNumber;

        public int PlayerNumber
        {
            get { return _playerNumber; }
            set 
            { 
                _playerNumber = value;
                label1.Text = _playerNumber.ToString();
            }
        }
        public EventHandler GoalClick;

        public PlayerControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(GoalClick != null)
            {
                GoalClick.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
