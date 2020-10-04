using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScoreBoardV2
{
    public partial class Form1 : Form
    {
        ViewWindow viewWindow;

        InitArgs viewInitArgs;
        WindowInitArgs windowInit;

        Size windowSize;
        Point windowLocation;

        int homeScore = 0;
        int awayScore = 0;

        int minToStop = 0;
        int secToStop = 0;

        ImageInitArgs imageArgs;
        Size homeImg;
        Size awayImg;

        Timer timer = new Timer();

        int setTimeMin = 0;
        int setTimeSec = 0;

        List<PlayerControl> homePlayers;
        List<PlayerControl> awayPlayers;

        int goalMinute = 0;

        int halfMin = 45;
        int halfSec = 0;

        int fullMin = 90;
        int fullSec = 0;

        public Form1()
        {
            InitializeComponent();
            InitSettings();
            InitObjects();
            LoadView();
        }

        private void InitSettings()
        {
            if (Properties.Settings.Default.NameFont == null)
                Properties.Settings.Default.NameFont = new Font("Microsoft Sans Serif", 8);


            if (Properties.Settings.Default.TimeFont == null)
                Properties.Settings.Default.TimeFont = new Font("Microsoft Sans Serif", 8);


            if (Properties.Settings.Default.ScoreFont == null)
                Properties.Settings.Default.ScoreFont = new Font("Microsoft Sans Serif", 8);

            if (Properties.Settings.Default.BackgroundColor == null)
                Properties.Settings.Default.BackgroundColor = Color.Black;

            if (Properties.Settings.Default.TextColor == null)
                Properties.Settings.Default.TextColor = Color.White;

            if (Properties.Settings.Default.Size == null)
                Properties.Settings.Default.Size = new Size();

            if (Properties.Settings.Default.Location == null)
                Properties.Settings.Default.Location = new Point();

            Properties.Settings.Default.Save();
        }

        private void InitObjects()
        {
            viewInitArgs = new InitArgs();
            FontInfo NameFont = new FontInfo(Properties.Settings.Default.NameFont.Name,(int)Properties.Settings.Default.NameFont.Size);
            FontInfo ScoreFont = new FontInfo(Properties.Settings.Default.ScoreFont.Name, (int)Properties.Settings.Default.ScoreFont.Size);
            FontInfo TimeFont = new FontInfo(Properties.Settings.Default.TimeFont.Name, (int)Properties.Settings.Default.TimeFont.Size);
            CustomColorWrapper BacgroundColor = new CustomColorWrapper(Properties.Settings.Default.BackgroundColor.R, Properties.Settings.Default.BackgroundColor.G, Properties.Settings.Default.BackgroundColor.B);
            CustomColorWrapper TextColor = new CustomColorWrapper(Properties.Settings.Default.TextColor.R, Properties.Settings.Default.TextColor.G, Properties.Settings.Default.TextColor.B);
            viewInitArgs.NameFont = NameFont;
            viewInitArgs.TextFont = TimeFont;
            viewInitArgs.ScoreFont = ScoreFont;
            viewInitArgs.BackgroundColor = BacgroundColor;
            viewInitArgs.TextColor = TextColor;

            windowInit = new WindowInitArgs();
            windowInit.WindowLocation = Properties.Settings.Default.Location;
            windowInit.WindowSize = Properties.Settings.Default.Size;

            viewWindow = new ViewWindow(viewInitArgs, windowInit);

            windowSize = Properties.Settings.Default.Size;
            windowLocation = Properties.Settings.Default.Location;

            imageArgs = new ImageInitArgs();
            homeImg = new Size(0, 0);
            awayImg = new Size(0, 0);
            imageArgs.HomeImageSize = homeImg;
            imageArgs.AwayImageSize = awayImg;
            imageArgs.HomeImagePath = "";
            imageArgs.AwayImagePath = "";

            timer.Tick += Timer_Tick;
            timer.Interval = 500;
            timer.Start();

            if (splitContainer1.Width != 0)
                splitContainer1.SplitterDistance = splitContainer1.Width / 2;

            homePlayers = new List<PlayerControl>();
            awayPlayers = new List<PlayerControl>();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (viewWindow.ScreenArea.GameTimeElapsed.Seconds >= 30 && viewWindow.ScreenArea.GameTimeElapsed.Seconds <= 60)
            {
                timeLabel.Text = String.Format("{0}:{1}", (viewWindow.ScreenArea.GameTimeElapsed.TotalMinutes - 1).ToString("00"), viewWindow.ScreenArea.GameTimeElapsed.Seconds.ToString("00"));
                goalMinute =(int) viewWindow.ScreenArea.GameTimeElapsed.TotalMinutes - 1;
            }
            else
            {
                timeLabel.Text = String.Format("{0}:{1}", viewWindow.ScreenArea.GameTimeElapsed.TotalMinutes.ToString("00"), viewWindow.ScreenArea.GameTimeElapsed.Seconds.ToString("00"));
                goalMinute = (int)viewWindow.ScreenArea.GameTimeElapsed.TotalMinutes;
            }

            if(viewWindow.ScreenArea.GameTimeElapsed.Seconds > 0)
            {
                goalMinute += 1;
            }
        }

        private void LoadView()
        {
            locationXTextBox.Text = Properties.Settings.Default.Location.X.ToString();
            locationYTextBox.Text = Properties.Settings.Default.Location.Y.ToString();

            heightTextBox.Text = Properties.Settings.Default.Size.Height.ToString();
            widthTextBox.Text = Properties.Settings.Default.Size.Width.ToString();

            bgColorPanel.BackColor = Properties.Settings.Default.BackgroundColor;
            textColorPanel.BackColor = Properties.Settings.Default.TextColor;

            scoreFontLabel.Text = string.Format("{0} {1}", Properties.Settings.Default.ScoreFont.Name, Properties.Settings.Default.ScoreFont.Size.ToString("0.0"));
            nameFontLabel.Text = string.Format("{0} {1}", Properties.Settings.Default.NameFont.Name, Properties.Settings.Default.NameFont.Size.ToString("0.0"));
            timeFontLabel.Text = string.Format("{0} {1}", Properties.Settings.Default.TimeFont.Name, Properties.Settings.Default.TimeFont.Size.ToString("0.0"));

            homeScoreLabel.Text = homeScore.ToString();
            awayScoreLabel.Text = awayScore.ToString();

            textBox21.Text = halfMin.ToString("00");
            textBox20.Text = halfSec.ToString("00");
            textBox23.Text = fullMin.ToString("00");
            textBox22.Text = fullSec.ToString("00");

        }

        private void openScreenButton_Click(object sender, EventArgs e)
        {
            viewWindow.Show();
        }

        private void showScreenButton_Click(object sender, EventArgs e)
        {
            viewWindow.ShowArea();
        }

        private void hideScreenButton_Click(object sender, EventArgs e)
        {
            viewWindow.HideArea();
        }

        private void nameFontButton_Click(object sender, EventArgs e)
        {
            FontDialog dialog = new FontDialog();
            dialog.Font = Properties.Settings.Default.NameFont;
            dialog.ShowDialog();
            Properties.Settings.Default.NameFont = dialog.Font;
            viewInitArgs.NameFont = new FontInfo(dialog.Font.Name,(int)dialog.Font.Size);
            Properties.Settings.Default.Save();
            nameFontLabel.Text = string.Format("{0} {1}", dialog.Font.Name, dialog.Font.Size.ToString("0.0"));
            viewWindow.ScreenArea.InitViews(viewInitArgs);
            dialog.Dispose();
        }

        private void timeFontButton_Click(object sender, EventArgs e)
        {
            FontDialog dialog = new FontDialog();
            dialog.Font = Properties.Settings.Default.TimeFont;
            dialog.ShowDialog();
            Properties.Settings.Default.TimeFont = dialog.Font;
            viewInitArgs.TextFont = new FontInfo(dialog.Font.Name, (int)dialog.Font.Size);
            Properties.Settings.Default.Save();
            timeFontLabel.Text = string.Format("{0} {1}", dialog.Font.Name, dialog.Font.Size.ToString("0.0"));
            viewWindow.ScreenArea.InitViews(viewInitArgs);
            dialog.Dispose();
        }

        private void scoreFontButton_Click(object sender, EventArgs e)
        {
            FontDialog dialog = new FontDialog();
            dialog.Font = Properties.Settings.Default.ScoreFont;
            dialog.ShowDialog();
            Properties.Settings.Default.ScoreFont = dialog.Font;
            viewInitArgs.ScoreFont = new FontInfo(dialog.Font.Name, (int)dialog.Font.Size);
            Properties.Settings.Default.Save();
            scoreFontLabel.Text = string.Format("{0} {1}", dialog.Font.Name, dialog.Font.Size.ToString("0.0"));
            viewWindow.ScreenArea.InitViews(viewInitArgs);
            dialog.Dispose();
        }

        private void setBgButton_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.ShowDialog();
            Properties.Settings.Default.BackgroundColor = dialog.Color;
            Properties.Settings.Default.Save();
            bgColorPanel.BackColor = dialog.Color;
            viewInitArgs.BackgroundColor = new CustomColorWrapper(dialog.Color.R,dialog.Color.G,dialog.Color.B);
            viewWindow.ScreenArea.InitViews(viewInitArgs);
            dialog.Dispose();
        }

        private void setTextButton_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.ShowDialog();
            Properties.Settings.Default.TextColor = dialog.Color;
            Properties.Settings.Default.Save();
            textColorPanel.BackColor = dialog.Color;
            viewInitArgs.TextColor = new CustomColorWrapper(dialog.Color.R, dialog.Color.G, dialog.Color.B);
            viewWindow.ScreenArea.InitViews(viewInitArgs);
            dialog.Dispose();
        }

        private void button9_Click(object sender, EventArgs e)//+1
        {
            homeScore++;
            homeScoreLabel.Text = homeScore.ToString();
            viewWindow.ScreenArea.HomeScore = homeScore;
        }

        private void button10_Click(object sender, EventArgs e)//-1
        {
            if (homeScore >= 1)
                homeScore--;

            homeScoreLabel.Text = homeScore.ToString();
            viewWindow.ScreenArea.HomeScore = homeScore;
        }

        private void button12_Click(object sender, EventArgs e)//+1
        {
            awayScore++;
            awayScoreLabel.Text = awayScore.ToString();
            viewWindow.ScreenArea.AwayScore = awayScore;

        }

        private void button11_Click(object sender, EventArgs e)//-1
        {
            if (awayScore >= 1)
                awayScore--;

            awayScoreLabel.Text = awayScore.ToString();
            viewWindow.ScreenArea.AwayScore = awayScore;
        }

        private void widthTextBox_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(widthTextBox.Text, out int w);
            windowSize.Width = w;
            windowInit.WindowSize = windowSize;
            viewWindow.UpdateSize(windowSize);
            Properties.Settings.Default.Size = windowSize;
            Properties.Settings.Default.Save();
        }

        private void heightTextBox_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(heightTextBox.Text, out int h);
            windowSize.Height = h;
            windowInit.WindowSize = windowSize;
            viewWindow.UpdateSize(windowSize);
            Properties.Settings.Default.Size = windowSize;
            Properties.Settings.Default.Save();
        }

        private void locationXTextBox_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(locationXTextBox.Text, out int x);
            windowLocation.X = x;
            windowInit.WindowLocation = windowLocation;
            viewWindow.UpdateLocation(windowLocation);
            Properties.Settings.Default.Location = windowLocation;
            Properties.Settings.Default.Save();
        }

        private void locationYTextBox_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(locationYTextBox.Text, out int y);
            windowLocation.Y = y;
            windowInit.WindowLocation = windowLocation;
            viewWindow.UpdateLocation(windowLocation);
            Properties.Settings.Default.Location = windowLocation;
            Properties.Settings.Default.Save();
        }

        private void homeTeamName_TextChanged(object sender, EventArgs e)
        {
            viewWindow.ScreenArea.HomeName = homeTeamName.Text;
        }

        private void awayTeamName_TextChanged(object sender, EventArgs e)
        {
            viewWindow.ScreenArea.AwayName = awayTeamName.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            viewWindow.ScreenArea.StartTimer();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            viewWindow.ScreenArea.StopTimer();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            viewWindow.ScreenArea.ResetTimer();
        }

        private void button4_Click(object sender, EventArgs e) // home image
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            imageArgs.HomeImagePath = dialog.FileName;
            textBox5.Text = Path.GetFileName(dialog.FileName);
        }

        private void button5_Click(object sender, EventArgs e) // away img
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            imageArgs.AwayImagePath = dialog.FileName;
            textBox6.Text = Path.GetFileName(dialog.FileName);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)// home width
        {
            int.TryParse(textBox2.Text, out int w);
            homeImg.Width = w;
            imageArgs.HomeImageSize = homeImg;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)// home height
        {
            int.TryParse(textBox1.Text, out int h);
            homeImg.Height = h;
            imageArgs.HomeImageSize = homeImg;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)// away width
        {
            int.TryParse(textBox4.Text, out int w);
            awayImg.Width = w;
            imageArgs.AwayImageSize = awayImg;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)// away height
        {
            int.TryParse(textBox3.Text, out int h);
            awayImg.Height = h;
            imageArgs.AwayImageSize = awayImg;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(imageArgs.HomeImageSize.Width == 0 || imageArgs.HomeImageSize.Height == 0)
            {
                MessageBox.Show("Home image size not set");
                return;
            }

            if (imageArgs.AwayImageSize.Width == 0 || imageArgs.AwayImageSize.Height == 0)
            {
                MessageBox.Show("Away image size not set");
                return;
            }

            if(string.IsNullOrWhiteSpace(imageArgs.HomeImagePath))
            {
                MessageBox.Show("Home image is not selected");
                return;
            }

            if (string.IsNullOrWhiteSpace(imageArgs.AwayImagePath))
            {
                MessageBox.Show("Away image is not selected");
                return;
            }

            viewWindow.ScreenArea.UpdateImage(imageArgs);
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)// set time min
        {
            setTimeMin = (int)numericUpDown4.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)// set time sec
        {
            setTimeSec = (int)numericUpDown3.Value;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            viewWindow.ScreenArea.SetTime(setTimeMin, setTimeSec);
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox7.Text, out int margin);
            viewWindow.ScreenArea.UpdateMarginTop(margin);
        }

        private void button8_Click(object sender, EventArgs e)// add home player
        {
            if (!string.IsNullOrWhiteSpace(textBox8.Text) && !string.IsNullOrWhiteSpace(textBox9.Text))
            {
                PlayerControl newPlayer = new PlayerControl();
                newPlayer.PlayerName = textBox9.Text;
                int.TryParse(textBox8.Text, out int number);
                newPlayer.PlayerNumber = number;
                newPlayer.Parent = splitContainer1.Panel1;
                newPlayer.Dock = DockStyle.Top;
                newPlayer.GoalClick += goalClick;
                homePlayers.Add(newPlayer);
                textBox8.Text = "";
                textBox9.Text = "";
            }
            else
                MessageBox.Show("Invalid player info");
        }

        private void button13_Click(object sender, EventArgs e)// add away player
        {
            if (!string.IsNullOrWhiteSpace(textBox11.Text) && !string.IsNullOrWhiteSpace(textBox10.Text))
            {
                PlayerControl newPlayer = new PlayerControl();
                newPlayer.PlayerName = textBox10.Text;
                int.TryParse(textBox11.Text, out int number);
                newPlayer.PlayerNumber = number;
                newPlayer.Parent = splitContainer1.Panel2;
                newPlayer.Dock = DockStyle.Top;
                newPlayer.GoalClick += goalClick;
                awayPlayers.Add(newPlayer);
                textBox10.Text = "";
                textBox11.Text = "";
            }
            else
                MessageBox.Show("Invalid player info");
        }

        private void goalClick(object sender, EventArgs e)
        {
            PlayerControl caller = sender as PlayerControl;

            if(homePlayers.Contains(caller))
            {
                viewWindow.ScreenArea.AddHomeScore(caller.PlayerName, goalMinute);
                button9_Click(button9, EventArgs.Empty);
            }
            if(awayPlayers.Contains(caller))
            {
                button12_Click(button12, EventArgs.Empty);
                viewWindow.ScreenArea.AddAwayScore(caller.PlayerName, goalMinute);
            }
        }

        private void button14_Click(object sender, EventArgs e)// Delete home player
        {
            int.TryParse(textBox12.Text, out int number);

            foreach(PlayerControl player in homePlayers.ToList())
            {
                if(player.PlayerNumber == number)
                {
                    homePlayers.Remove(player);
                    player.GoalClick -= goalClick;
                    player.Dispose();
                    textBox12.Text = "";
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int.TryParse(textBox13.Text, out int number);

            foreach (PlayerControl player in awayPlayers.ToList())
            {
                if (player.PlayerNumber == number)
                {
                    awayPlayers.Remove(player);
                    player.GoalClick -= goalClick;
                    player.Dispose();
                    textBox13.Text = "";
                }
            }
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox14.Text, out int margin);
            viewWindow.ScreenArea.UpdateMarginSide(margin);
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox15.Text, out int margin);
            viewWindow.ScreenArea.UpdateFontMargin(margin);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            int.TryParse(textBox16.Text, out int number);
            int.TryParse(textBox17.Text, out int minute);

            string name = "";
            foreach(PlayerControl player in homePlayers)
            {
                if(player.PlayerNumber == number)
                {
                    name = player.PlayerName;
                }
            }

            viewWindow.ScreenArea.AddHomeScore(name, minute);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            int.TryParse(textBox19.Text, out int number);
            int.TryParse(textBox18.Text, out int minute);

            string name = "";
            foreach (PlayerControl player in awayPlayers)
            {
                if (player.PlayerNumber == number)
                {
                    name = player.PlayerName;
                }
            }

            viewWindow.ScreenArea.AddAwayScore(name, minute);

        }

        private void button18_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to reset names", "Warning", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                viewWindow.ScreenArea.ResetHomePlayers();
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to reset names", "Warning", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                viewWindow.ScreenArea.ResetAwayPlayers();
            }
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox21.Text, out int halfm);
            halfMin = halfm;
            viewWindow.ScreenArea.SetHalfTime(halfMin, halfSec);
        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox20.Text, out int halfs);
            halfSec = halfs;
            viewWindow.ScreenArea.SetHalfTime(halfMin, halfSec);
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox23.Text, out int fullm);
            fullMin = fullm;
            viewWindow.ScreenArea.SetFullTime(fullMin, fullSec);
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox22.Text, out int fulls);
            fullSec = fulls;
            viewWindow.ScreenArea.SetFullTime(fullMin, fullSec);
        }
    }
}
