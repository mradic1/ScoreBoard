using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScoreBoardV2
{
    /// <summary>
    /// Interaction logic for ScreenArea.xaml
    /// </summary>
    public partial class ScreenArea : UserControl
    {
        Stopwatch GameTime;
        System.Windows.Forms.Timer ViewUpdateTimer;

        TimeSpan GlobalTime;
        TimeSpan OffsetTime;


        Thickness img1Margin = new Thickness(0, 0, 0, 0);
        Thickness img2Margin = new Thickness(0, 0, 0, 0);

        public bool StopAtTimeIsActive
        {
            get
            {
                return stopAtTime;
            }
        }

        private bool stopAtTime;

        private string homeName;
        private string awayName;
        private int homeScore;
        private int awayScore;

        private Brush BackColor;
        private Brush TextColor;

        private FontFamily NameFont;
        private FontFamily ScoreFont;
        private FontFamily TimeFont;


        int halfMin = 0;
        int halfSec = 0;

        int fullMin = 0;
        int fullSec = 0;

        bool halfHappened = false;

        public string HomeName
        {
            get { return homeName; }
            set 
            { 
                homeName = value;
            }
        }

        public string AwayName
        {
            get { return awayName; }
            set 
            { 
                awayName = value;
            }
        }
        
        public int HomeScore
        {
            get { return homeScore; }
            set 
            { 
                homeScore = value;
                score1Label.Content = homeScore;
            }
        }

        public int AwayScore
        {
            get { return awayScore; }
            set 
            { 
                awayScore = value;
                score2Label.Content = awayScore;
            }
        }

        public TimeSpan GameTimeElapsed
        {
            get
            {
                return GlobalTime;
            }
        }

        public ScreenArea(InitArgs initArgs)
        {
            InitializeComponent();
            InitObjects(initArgs);
        }

        private void InitObjects(InitArgs args)
        {
            GameTime = new Stopwatch();

            ViewUpdateTimer = new System.Windows.Forms.Timer();
            ViewUpdateTimer.Tick += ViewUpdateTimer_Tick;
            ViewUpdateTimer.Interval = 250;
            ViewUpdateTimer.Start();

            GlobalTime = new TimeSpan();
            
            InitViews(args);
        }
        
        public void InitViews(InitArgs args)
        {
            Color bgColor = new Color();
            bgColor.R = (byte)args.BackgroundColor.Red;
            bgColor.G = (byte)args.BackgroundColor.Green;
            bgColor.B = (byte)args.BackgroundColor.Blue;
            bgColor.A = 255;
            Color textColor = new Color();
            textColor.R = (byte)args.TextColor.Red;
            textColor.G = (byte)args.TextColor.Green;
            textColor.B = (byte)args.TextColor.Blue;
            textColor.A = 255;
            BackColor = new SolidColorBrush(bgColor);
            TextColor = new SolidColorBrush(textColor);

            this.Background = BackColor;

            goal1.Foreground = TextColor;
            goal2.Foreground = TextColor;
            score1Label.Foreground = TextColor;
            score2Label.Foreground = TextColor;
            timeLabel.Foreground = TextColor;

            TimeFont = new FontFamily(args.TextFont.FontName);
            ScoreFont = new FontFamily(args.ScoreFont.FontName);
            NameFont = new FontFamily(args.NameFont.FontName);

            goal1.FontFamily = NameFont;
            goal2.FontFamily = NameFont;
            goal1.FontSize = args.NameFont.FontSize;
            goal2.FontSize = args.NameFont.FontSize;

            score1Label.FontFamily = ScoreFont;
            score2Label.FontFamily = ScoreFont;
            score1Label.FontSize = args.ScoreFont.FontSize;
            score2Label.FontSize = args.ScoreFont.FontSize;

            timeLabel.FontFamily = TimeFont;
            timeLabel.FontSize = args.TextFont.FontSize;

        }

        private void ViewUpdateTimer_Tick(object sender, EventArgs e)
        {
            GlobalTime = GameTime.Elapsed.Add(OffsetTime);

            if (GlobalTime.Seconds >= 30 && GlobalTime.Seconds <= 60)
            {
                timeLabel.Content = String.Format("{0}:{1}", (GlobalTime.TotalMinutes - 1).ToString("00"), GlobalTime.Seconds.ToString("00"));

                if ((int)GlobalTime.TotalMinutes - 1 == halfMin && (int)GlobalTime.Seconds == halfSec && !halfHappened && GameTime.IsRunning)
                {
                    StopTimer();
                    halfHappened = true;
                }

                if((int)GlobalTime.TotalMinutes - 1 == fullMin && (int)GlobalTime.Seconds == fullSec)
                {
                    StopTimer();
                }
            }
            else
            {
                timeLabel.Content = String.Format("{0}:{1}", GlobalTime.TotalMinutes.ToString("00"), GlobalTime.Seconds.ToString("00"));

                if ((int)GlobalTime.TotalMinutes == halfMin && (int)GlobalTime.Seconds == halfSec && !halfHappened && GameTime.IsRunning)
                {
                    StopTimer();
                    halfHappened = true;
                }

                if((int)GlobalTime.TotalMinutes == fullMin && (int)GlobalTime.Seconds == fullSec)
                {
                    StopTimer();
                }
            }
        }

        public void StartTimer()
        {
            GameTime.Start();
        }

        public void StopTimer()
        {
            GameTime.Stop();
        }

        public void ResetTimer()
        {
            GameTime.Reset();
            OffsetTime = new TimeSpan(0,0,0);
            halfHappened = false;
        }

        public void SetHalfTime(int min, int sec)
        {
            halfMin = min;
            halfSec = sec;
        }

        public void SetFullTime(int min, int sec)
        {
            fullMin = min;
            fullSec = sec;
        }

        public void ResetStopAt()
        {
            stopAtTime = false;
        }

        public void UpdateImage(ImageInitArgs args)
        {
            img1.Source = new BitmapImage(new Uri(args.HomeImagePath));
            img2.Source = new BitmapImage(new Uri(args.AwayImagePath));

            img1.Height = args.HomeImageSize.Height;
            img1.Width = args.HomeImageSize.Width;

            img2.Height = args.AwayImageSize.Height;
            img2.Width = args.AwayImageSize.Width;
        }

        public void SetTime(int min, int sec)
        {
            OffsetTime = new TimeSpan(0, min, sec);
        }

        public void UpdateMarginTop(int margin)
        {
            img1Margin.Top = margin;
            img2Margin.Top = margin;
            img1.Margin = img1Margin;
            img2.Margin = img2Margin;
            goal1.Margin = img1Margin;
            goal2.Margin = img2Margin;
        }

        public void UpdateMarginSide(int margin)
        {
            img1Margin.Left = margin;
            img2Margin.Right = margin;
            img1.Margin = img1Margin;
            img2.Margin = img2Margin;
            goal1.Margin = img1Margin;
            goal2.Margin = img2Margin;
        }

        public void UpdateFontMargin(int margin)
        {
            Thickness margin1 = new Thickness();
            margin1.Right = margin;
            Thickness margin2 = new Thickness();
            margin2.Left = margin;

            score1Label.Margin = margin1;
            score2Label.Margin = margin2;
        }

        public void AddHomeScore(string name, int minute)
        {
            goal1.Text += String.Format("{0}' - {1}\n", minute, name);
        }

        public void AddAwayScore(string name, int minute)
        {
            goal2.Text += String.Format("{0}' - {1}\n", minute, name);
        }

        public void ResetHomePlayers()
        {
            goal1.Text = "";
        }

        public void ResetAwayPlayers()
        {
            goal2.Text = "";
        }
    }
}
