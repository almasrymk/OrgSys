using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace ORG.Tools.Desktop.Tools
{
    public partial class BottunOrg : UserControl
    {
        Image _Image = null;
        [Category("ORG")]
        public Image Image
        {
            get { return _Image; }
            set
            {
                _Image = value;
                pic.BackgroundImage = value;               
            }
        }

        Image _ImageEnter = null;
        [Category("ORG")]
        public Image ImageEnter
        {
            get { return _ImageEnter; }
            set { _ImageEnter = value; }
        }

        ImageLayout _ImageLayOut = ImageLayout.Zoom;
        [Category("ORG")]
        public ImageLayout ImageLayout
        {
            get { return _ImageLayOut; }
            set
            {
                _ImageLayOut = value;
                pic.BackgroundImageLayout = value;
            }
        }

        Color _Color = Color.FromArgb(100, 150, 250);
        [Category("ORG")]
        public Color Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
                pic.BackColor = _Color;
                label1.BackColor = _Color;
            }
        }

        Color _ColorEnter = Color.FromArgb(100, 150, 250);
        [Category("ORG")]
        public Color ColorEnter
        {
            get { return _ColorEnter; }
            set { _ColorEnter = value; }
        }

        Color _FontColor = Color.White;
        [Category("ORG")]
        public Color FontColor
        {
            get { return _FontColor; }
            set
            {
                _FontColor = value;              
                label1.ForeColor = _FontColor;
            }
        }

        Color _FontColorEnter = Color.FromArgb(100, 150, 250);
        [Category("ORG")]
        public Color FontColorEnter
        {
            get { return _FontColorEnter; }
            set
            {
                _FontColorEnter = value;                
            }
        }

        string _TextTitle = "Title";
        [Category("ORG")]
        [Localizable(true)]
        public string TextTitle
        {
            get { return _TextTitle; }
            set { _TextTitle = value; label1.Text = value; }
        }

        bool _WithSplitter = true;
        [Category("ORG")]
        public bool WithSplitter
        {
            get { return _WithSplitter; }
            set { _WithSplitter = value; Splitter.Visible = value; }
        }

        bool _WithTitle = true;
        [Category("ORG")]
        public bool WithTitle
        {
            get { return _WithTitle; }
            set { _WithTitle = value; label1.Visible = value; }
        }

        public BottunOrg()
        {
            InitializeComponent();
        }

        bool runSound = false;
        private void MouseEnter(object sender, EventArgs e)
        {
            if (!runSound)
            {
                Helper.Sound(Application.StartupPath, SoudType.MenuMouseEnter);
                runSound = true;
            }

            pic.BackgroundImage = _ImageEnter;
            pic.BackColor = _ColorEnter;
            label1.BackColor = _ColorEnter;
            label1.ForeColor = _FontColorEnter;
            this.BackColor = _ColorEnter;                
        }

        private void MouseLeave(object sender, EventArgs e)
        {
            pic.BackgroundImage = _Image;          
            pic.BackColor = _Color;
            label1.BackColor = _Color;
            label1.ForeColor = _FontColor;
            this.BackColor = _Color;
            if (!this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
                runSound = false;
        }

        private void This_Click(object sender, EventArgs e)
        {
            Helper.Sound(Application.StartupPath, SoudType.MenuMouseSelect);
            this.OnClick(e);
        }
    }
}
