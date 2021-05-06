using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OrgSysApp.Tools
{
    public partial class Element : UserControl
    {
        public Element()
        {
            InitializeComponent();
        }

        [Category("Org")]
        public event EventHandler click;

        Image _image = null;
        Image _imageActive = null;
        ImageLayout _imageLayOut = ImageLayout.Zoom;
        string _title = "";
        string _description = "";
        Font _font = null;
        Font _fontDescription = null;
        Font _fontActive = null;
        Font _fontActiveDescription = null;
        Color _fontColor = Color.Black;
        Color _fontColorActive = Color.Black;
        Color _fontColorDescription = Color.Black;
        Color _fontColorActiveDescription = Color.Black;
        Color _backgroundColor = Color.Transparent;
        Color _backgroundColorActive = Color.Transparent;
        bool _active = false;
        bool _withDescription = false;

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

        [Category("Org.Title")]
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                lblDescription.Text = value;
            }
        }
        
        [Category("Org")]
        public string ButtonName { get; set; }

        [Category("Org")]
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                this.ForeColor = _backgroundColor;
            }
        }

        [Category("Org")]
        public Color BackgroundColorActive
        {
            get { return _backgroundColorActive; }
            set { _backgroundColorActive = value; }
        }

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
        public Font fontActive
        {
            get { return _fontActive; }
            set {_fontActive = value;}
        }
        
        [Category("Org.Title")]
        public Color fontColorActive
        {
            get { return _fontColorActive; }
            set { _fontColorActive = value; }
        }

        [Category("Org.Title")]
        public Font fontDescription
        {
            get { return _fontDescription; }
            set
            {
                _fontDescription = value;
                lblDescription.Font = value;
            }
        }

        [Category("Org.Title")]
        public Font fontActiveDescription
        {
            get { return _fontActiveDescription; }
            set { _fontActiveDescription = value; }
        }

        [Category("Org.Title")]
        public Color fontColorDescription
        {
            get { return _fontColorDescription; }
            set
            {
                _fontColorDescription = value;
                lblDescription.ForeColor = _fontColor;
            }
        }

        [Category("Org.Title")]
        public Color fontColorActiveDescription
        {
            get { return _fontColorActiveDescription; }
            set { _fontColorActiveDescription = value; }
        }

        [Category("Org")]
        public bool active
        {
            get { return _active; }
            set
            {
                _active = value;
                if (_active)
                {
                    imgImage.BackgroundImage = _imageActive;
                    this.BackColor = _backgroundColorActive;
                    lblTitle.Font = _fontActive;
                    lblTitle.ForeColor = _fontColorActive;
                    lblDescription.ForeColor = _fontColorActiveDescription;
                }
                else
                {
                    imgImage.BackgroundImage = _image;
                    this.BackColor = _backgroundColor;
                    lblTitle.Font = _font;
                    lblTitle.ForeColor = _fontColor;
                    lblDescription.ForeColor = _fontColorDescription;
                }
            }
        }

        [Category("Org")]
        public bool WithDescription
        {
            get { return _withDescription; }
            set
            {
                _withDescription = value;
                lblDescription.Visible = value;
            }
        }

        private void this_MouseEnter(object sender, EventArgs e)
        {
            imgImage.BackgroundImage = _imageActive;
            this.BackColor = _backgroundColorActive;
            lblTitle.Font = _fontActive;
            lblTitle.ForeColor = _fontColorActive;
            lblDescription.ForeColor = _fontColorActiveDescription;
        }

        private void this_MouseLeave(object sender, EventArgs e)
        {
            if (!_active)
            {
                imgImage.BackgroundImage = _image;
                this.BackColor = _backgroundColor;
                lblTitle.Font = _font;
                lblTitle.ForeColor = _fontColor;
                lblDescription.ForeColor = _fontColorDescription;
            }
        }

        private void this_Click(object sender, EventArgs e)
        {
            if (click != null)
                click(this, e);
        }
    }
}