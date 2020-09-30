using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreBoardV2
{
    public class InitArgs
    {
        public FontInfo TextFont { get; set; }
        public FontInfo NameFont { get; set; }
        public FontInfo ScoreFont { get; set; }

        public CustomColorWrapper BackgroundColor { get; set; }
        public CustomColorWrapper TextColor { get; set; }
    }

    public class WindowInitArgs
    {
        public Point WindowLocation { get; set; }
        public Size WindowSize { get; set; }
    }

    public struct FontInfo
    {
        public string FontName { get; set; }
        public int FontSize { get; set; }

        public FontInfo(string fontName, int fontSize)
        {
            FontName = fontName;
            FontSize = fontSize;
        }
    }

    public struct CustomColorWrapper
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public CustomColorWrapper(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }

    public struct ImageInitArgs
    {
        public string HomeImagePath { get; set; }
        public string AwayImagePath { get; set; }
        public Size HomeImageSize { get; set; }
        public Size AwayImageSize { get; set; }
    }
}

