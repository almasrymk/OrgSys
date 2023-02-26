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

namespace ORG.Tools.Desktop
{
    public partial class MenuItemTool : UserControl
    {
        public event EventHandler SelectFromMenu;

        public MenuItemTool()
        {
            InitializeComponent();
            _Font = new Font("Arial", 8, FontStyle.Italic);
        }

        bool _Selected = false;
        [Category("ORG")]
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                _Selected = value;
                if (_Selected)
                {
                    picTitle.BackgroundImage = _ImageTitleEnter;
                    lblTitle.ForeColor = _ForColorTitleEnter;
                    pnlFooter.BackColor = _ColorFooterEnter;
                    pnlFooter.Visible = !_ViewFooter;
                }
                else
                {
                    picTitle.BackgroundImage = _ImageTitle;
                    lblTitle.ForeColor = _ForColorTitle;
                    pnlFooter.BackColor = _ColorFooter;
                    pnlFooter.Visible = _ViewFooter;
                }
            }
        }

        string _TextTitle = "Title";
        [Category("ORG")]
        [Localizable(true)]
        public string TextTitle
        {
            get { return _TextTitle; }
            set { _TextTitle = value; lblTitle.Text = value; }
        }

        Font _Font = null;
        [Category("ORG")]
        public Font FontTitle
        {
            get
            {
                return _Font;
            }
            set
            {
                _Font = value;
                lblTitle.Font = _Font;
            }
        }

        int _SizeFooter = 10;
        [Category("ORG")]
        public int SizeFooter
        {
            get { return _SizeFooter; }
            set { _SizeFooter = value; pnlFooter.Height = _SizeFooter; }
        }

        Image _ImageTitle = null;
        [Category("ORG")]
        public Image ImageTitle
        {
            get { return _ImageTitle; }
            set
            {
                _ImageTitle = value;
                picTitle.BackgroundImage = value;
            }
        }

        ImageLayout _ImageTitleLayOut = ImageLayout.Zoom;
        [Category("ORG")]
        public ImageLayout ImageTitleLayout
        {
            get { return _ImageTitleLayOut; }
            set
            {
                _ImageTitleLayOut = value;
                picTitle.BackgroundImageLayout = value;
            }
        }

        Image _ImageTitleEnter = null;
        [Category("ORG")]
        public Image ImageTitleEnter
        {
            get { return _ImageTitleEnter; }
            set { _ImageTitleEnter = value; }
        }

        Color _ForColorTitle = Color.FromArgb(100, 150, 250);
        [Category("ORG")]
        public Color ForColorTitle
        {
            get { return _ForColorTitle; }
            set
            {
                _ForColorTitle = value;
                lblTitle.ForeColor = _ForColorTitle;
            }
        }

        Color _ForColorTitleEnter = Color.FromArgb(100, 150, 250);
        [Category("ORG")]
        public Color ForColorTitleEnter
        {
            get { return _ForColorTitleEnter; }
            set
            {
                _ForColorTitleEnter = value;
            }
        }

        Color _ColorFooter = Color.FromArgb(100, 150, 250);
        [Category("ORG")]
        public Color ColorFooter
        {
            get { return _ColorFooter; }
            set
            {
                _ColorFooter = value;
                pnlFooter.BackColor = _ColorFooter;
            }
        }

        Color _ColorFooterEnter = Color.FromArgb(100, 150, 250);
        [Category("ORG")]
        public Color ColorFooterEnter
        {
            get { return _ColorFooterEnter; }
            set
            {
                _ColorFooterEnter = value;
            }
        }

        bool _ViewFooter = false;
        [Category("ORG")]
        public bool ViewFooter
        {
            get { return _ViewFooter; }
            set { _ViewFooter = value; }
        }


        bool runSound = false;
        private void Controls_Click(object sender, EventArgs e)
        {
            if (SelectFromMenu == null || Selected) return;
            Helper.Sound(Application.StartupPath, SoudType.MenuMouseSelect);
            Selected = !Selected;
            SelectFromMenu(this, e);
        }

        private void lblTitle_MouseEnter(object sender, EventArgs e)
        {
            if (!_Selected && !runSound)
            {
                Helper.Sound(Application.StartupPath, SoudType.MenuMouseEnter);               
                runSound = true;
            }

            picTitle.BackgroundImage = _ImageTitleEnter;
            lblTitle.ForeColor = _ForColorTitleEnter;
            pnlFooter.BackColor = _ColorFooterEnter;
            pnlFooter.Visible = !_ViewFooter;
        }

        private void lblTitle_MouseLeave(object sender, EventArgs e)
        {
            if (!_Selected)
            {
                picTitle.BackgroundImage = _ImageTitle;
                lblTitle.ForeColor = _ForColorTitle;
                pnlFooter.BackColor = _ColorFooter;
                pnlFooter.Visible = _ViewFooter;
            }
            if (!this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
                runSound = false;
        }
    }
}
