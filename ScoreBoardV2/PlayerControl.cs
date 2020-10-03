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

        public EventHandler RedCardClick;
        public EventHandler YellowCardClick;
        public EventHandler GoalClick;

        public PlayerControl()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(RedCardClick != null)
            {
                RedCardClick.Invoke(this, EventArgs.Empty);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(YellowCardClick != null)
            {
                YellowCardClick.Invoke(this, EventArgs.Empty);
            }
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
