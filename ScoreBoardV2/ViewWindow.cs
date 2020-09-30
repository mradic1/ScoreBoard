using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScoreBoardV2
{
    public partial class ViewWindow : Form
    {
        public ScreenArea ScreenArea;
        
        public ViewWindow(InitArgs areaArgs,WindowInitArgs initArgs)
        {
            InitializeComponent();
            ScreenArea = new ScreenArea(areaArgs);
            Location = initArgs.WindowLocation;
            Size = initArgs.WindowSize;
            elementHost1.Child = ScreenArea;
            BackColor = Color.Black;
            elementHost1.Show();
        }

        public void UpdateSize(Size size)
        {
            this.Size = size;
        }

        public void UpdateLocation(Point point)
        {
            this.Location = point;
        }

        public void ShowArea()
        {
            elementHost1.Visible = true;
        }

        public void HideArea()
        {
            elementHost1.Visible = false;
        }
    }
}
