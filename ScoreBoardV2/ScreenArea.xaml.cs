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

        int minToStop = 0;
        int secToStop = 0;

        public string HomeName
        {
            get { return homeName; }
            set 
            { 
                homeName = value;
                name1Label.Content = homeName;
            }
        }

        public string AwayName
        {
            get { return awayName; }
            set 
            { 
                awayName = value;
                name2Label.Content = awayName;
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

            name1Label.Foreground = TextColor;
            name2Label.Foreground = TextColor;
            score1Label.Foreground = TextColor;
            score2Label.Foreground = TextColor;
            timeLabel.Foreground = TextColor;

            TimeFont = new FontFamily(args.TextFont.FontName);
            ScoreFont = new FontFamily(args.ScoreFont.FontName);
            NameFont = new FontFamily(args.NameFont.FontName);

            name1Label.FontFamily = NameFont;
            name2Label.FontFamily = NameFont;
            name1Label.FontSize = args.NameFont.FontSize;
            name2Label.FontSize = args.NameFont.FontSize;

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

                if ((int)GlobalTime.TotalMinutes - 1 == minToStop && (int)GlobalTime.Seconds == secToStop && stopAtTime)
                {
                    StopTimer();
                    stopAtTime = false;
                }
            }
            else
            {
                timeLabel.Content = String.Format("{0}:{1}", GlobalTime.TotalMinutes.ToString("00"), GlobalTime.Seconds.ToString("00"));

                if ((int)GlobalTime.TotalMinutes == minToStop &&(int)GlobalTime.Seconds == secToStop && stopAtTime)
                {
                    StopTimer();
                    stopAtTime = false;
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
        }

        public void StopTimerAt(int minutes, int seconds)
        {
            stopAtTime = true;
            minToStop = minutes;
            secToStop = seconds;
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

        public void UpdateMargin(int margin)
        {
            img1.Margin = new Thickness(0, margin, 0, 0);
            img2.Margin = new Thickness(0, margin, 0, 0);
        }
    }
}
