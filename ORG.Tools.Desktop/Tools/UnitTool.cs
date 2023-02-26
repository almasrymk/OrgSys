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
using System.IO;

namespace ORG.Tools.Desktop
{
    public partial class UnitTool : UserControl
    {
        public event SelectedOB SelectObject;
        public event ClickOB ClickObject;

        public UnitTool()
        {
            InitializeComponent();
        }

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private object _MyObject;
        public object MyObject
        {
            get { return _MyObject; }
            set { _MyObject = value; }
        }

        byte[] _UnitImage = null;
        public byte[] UnitImage
        {
            get { return _UnitImage; }
            set
            {
                _UnitImage = value;
                if (_UnitImage != null && _UnitImage.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(_UnitImage);
                    picUnit.BackgroundImage = Image.FromStream(ms);
                }
            }
        }

        public string UnitName
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        Color _ColorPic = Color.FromArgb(100, 150, 250);
        public Color ColorPic
        {
            get { return _ColorPic; }
            set { _ColorPic = value; picUnit.BackColor = _ColorPic; }
        }

        Color _ColorBody = Color.WhiteSmoke;
        public Color ColorBody
        {
            get { return _ColorBody; }
            set { _ColorBody = value; panel1.BackColor = _ColorBody; }
        }

        private void Unit_Enter(object sender, EventArgs e)
        {
            if (SelectObject != null)
                SelectObject(_MyObject);
        }

        private void Unit_Click(object sender, EventArgs e)
        {
            if (ClickObject != null)
            {
                Helper.Sound(Application.StartupPath, SoudType.ItemMouseClick);
                ClickObject(_MyObject);
            }
        }
    }
}