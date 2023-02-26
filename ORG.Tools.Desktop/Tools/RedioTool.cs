using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORG.Tools.Desktop
{
    public partial class RedioTool : UserControl
    {
        public event EventHandler Select;
        public RedioTool()
        {
            InitializeComponent();
            _Font = new Font("Arial", 8, FontStyle.Italic);
        }

        bool _Selected = false;
        [Category ("ORG")]
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                _Selected = value;

                if (_Selected)
                {
                    txtLeft.Text = _textLeft;
                    txtRight.Text = string.Empty;
                    pnlLeft.BackgroundImage = _IamgeLeftSelected;
                    pnlRight.BackgroundImage = _IamgeRightSelected;
                }
                else
                {
                    txtLeft.Text = string.Empty;
                    txtRight.Text = _textRight;
                    pnlLeft.BackgroundImage = _IamgeLeft;
                    pnlRight.BackgroundImage = _IamgeRight;
                }
            }
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
                txtLeft.Font = txtRight.Font = _Font;
            }
        }

        string _textLeft = "Ok";
        [Category("ORG")]
        public string TextLeft
        {
            get { return _textLeft; }
            set { _textLeft = value; txtLeft.Text = _textLeft; }
        }

        string _textRight = "No";
        [Category("ORG")]
        public string TextRight
        {
            get { return _textRight; }
            set { _textRight = value; txtRight.Text = _textRight; }
        }

        Image _IamgeLeft = null;
        [Category("ORG")]
        public Image IamgeLeft
        {
            get { return _IamgeLeft; }
            set { _IamgeLeft = value; pnlLeft.BackgroundImage = _IamgeLeft; }
        }

        Image _IamgeLeftSelected = null;
        [Category("ORG")]
        public Image IamgeLeftSelected
        {
            get { return _IamgeLeftSelected; }
            set { _IamgeLeftSelected = value; }
        }

        Image _IamgeRight = null;
        [Category("ORG")]
        public Image IamgeRight
        {
            get { return _IamgeRight; }
            set { _IamgeRight = value; pnlRight.BackgroundImage = _IamgeRight; }
        }

        Image _IamgeRightSelected = null;
        [Category("ORG")]
        public Image IamgeRightSelected 
        {
            get { return _IamgeRightSelected; }
            set { _IamgeRightSelected = value; }
        }

        Color _ForColorLeft = Color.FromArgb(100, 150, 250);
        [Category("ORG")]
        public Color ForColorLeft
        {
            get { return _ForColorLeft; }
            set { _ForColorLeft = value; txtLeft.ForeColor = _ForColorLeft; }
        }

        Color _ForColorRight = Color.Red;
        [Category("ORG")]
        public Color ForColorRight
        {
            get { return _ForColorRight; }
            set { _ForColorRight = value; txtRight.ForeColor = _ForColorRight; }
        }

        private void SelectedClick(object sender, EventArgs e)
        {
            if(!Selected)            
            Selected = true;
            if (Select != null)
                Select(Selected, e);
        }

        private void UnselectedClick(object sender, EventArgs e)
        {
            if(Selected)
            Selected = false;
            if (Select != null)
                Select(Selected, e);
        }
    }
}
