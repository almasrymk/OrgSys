using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OrgSysApp.Tools
{
    public partial class ButtonImage : UserControl
    {
        public ButtonImage()
        {
            InitializeComponent();
        }

        [Category("Org")]
        public event EventHandler click;

        Image _image = null;
        Image _imageActive = null;
        ImageLayout _imageLayOut = ImageLayout.Zoom;
        string _title = "";
        Font _font = null;
        Color _fontColor = Color.Black;
        Color _fontColorActive = Color.Black;
        Color _activeColor = Color.Transparent;
        bool _active = false;

        [Category("Org.Image")]
        public Image image
        {
            get { return _image; }
            set
            {
                _image = value;
                imgImage.BackgroundImage = _image;
            }
        }

        [Category("Org.Image")]
        public Image imageActive
        {
            get { return _imageActive; }
            set { _imageActive = value; }
        }

        [Category("Org.Image")]
        public ImageLayout layOut
        {
            get { return _imageLayOut; }
            set
            {
                _imageLayOut = value;
                imgImage.BackgroundImageLayout = value;
            }
        }

        [Category("Org.Title")]
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                lblTitle.Text = value;
            }
        }

        [Category("Org")]
        public string ButtonName { get; set; }

        [Category("Org.Title")]
        public Font font
        {
            get { return _font; }
            set
            {
                _font = value;
                lblTitle.Font = value;
            }
        }

        [Category("Org.Title")]
        public Color fontColor
        {
            get { return _fontColor; }
            set
            {
                _fontColor = value;
                lblTitle.ForeColor = _fontColor;
            }
        }

        [Category("Org.Title")]
        public Color fontColorActive
        {
            get { return _fontColorActive; }
            set { _fontColorActive = value; }
        }

        [Category("Org")]
        public Color activeColor
        {
            get { return _activeColor; }
            set{ _activeColor = value; }
        }

        [Category("Org")]
        public bool active
        {
            get { return _active; }
            set 
            { 
                _active = value;
                if(_active)
                {
                    imgImage.BackgroundImage = _imageActive;
                    lblTitle.ForeColor = _fontColorActive;
                    lblActive.BackColor = _activeColor;
                }
                else
                {
                    imgImage.BackgroundImage = _image;
                    lblTitle.ForeColor = _fontColor;
                    lblActive.BackColor = Color.Transparent;
                }
            }
        }

        private void this_MouseEnter(object sender, EventArgs e)
        {
            imgImage.BackgroundImage = _imageActive;
            lblTitle.ForeColor = _fontColorActive;
            lblActive.BackColor = _activeColor;
        }

        private void this_MouseLeave(object sender, EventArgs e)
        {
            if (!_active)
            {
                imgImage.BackgroundImage = _image;
                lblTitle.ForeColor = _fontColor;
                lblActive.BackColor = Color.Transparent;
            }
        }

        private void this_Click(object sender, EventArgs e)
        {
            if (click != null)
                click(this , e);
        }
    }
}
