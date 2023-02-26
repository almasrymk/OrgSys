using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace ORG.Tools.Desktop
{
    public partial class PicImageTool : UserControl
    {
        public PicImageTool()
        {
            InitializeComponent();
        }

        byte[] _Image = null;
        public byte[] ShowImage
        {
            get { return _Image; }
            set
            {
                _Image = value;
                if (_Image != null && _Image.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(_Image);
                    pic.BackgroundImage = Image.FromStream(ms);
                }
                else
                    pic.BackgroundImage = null;
            }
        }     

        private void SelectImage_Click(object sender, EventArgs e)
        {
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                Image OBImage = Image.FromFile(openfile.FileName);
                Size s = new System.Drawing.Size(100, 100);
                Bitmap comp = new Bitmap(OBImage, s);
                MemoryStream ms = new MemoryStream();
                comp.Save(ms, ImageFormat.Png);
                ShowImage = ms.ToArray();
            }
        }
    }
}
