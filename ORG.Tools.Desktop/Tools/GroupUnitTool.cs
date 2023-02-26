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
    public delegate object _selectGroup(GroupUnitTool MyControl, object OB, int Index);
    public partial class GroupUnitTool : UserControl
    {
        public event _selectGroup SelectGroup;

        public GroupUnitTool()
        {
            InitializeComponent();
        }

        #region Properties
        bool _Selected = false;
        public bool Selected
        {
            get
            {
                return this._Selected;
            }
            set
            {
                this._Selected = value;
                if (value)
                {
                    picKey.BackColor = lblGroup.ForeColor = this.BackColor;
                    pnlGroup.BackColor = Color.FromArgb(100, 150, 250);
                    //picKey.BackgroundImage = ORG.Tools.Desktop.Properties.Resources._03RightActive;
                    picGroup.BackgroundImage = _GroupImage;
                }
                else
                {
                    picKey.BackColor = lblGroup.ForeColor = Color.FromArgb(100, 150, 250);
                    pnlGroup.BackColor = this.BackColor;
                    // picKey.BackgroundImage = ORG.Tools.Desktop.Properties.Resources._04M;
                    picGroup.BackgroundImage = _NullImage;
                }
                picKey.Refresh();
            }
        }

        long _ID = 0;
        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        [Localizable(true)]
        public string GroupName
        {
            get
            {
                return this.lblGroup.Text;
            }
            set
            {
                this.lblGroup.Text = value;
            }
        }

        Image _NullImage = null;
        public Image NullImage
        {
            get { return _NullImage; }
            set
            {
                if (_GroupImage == null)
                    picGroup.BackgroundImage = value;
                _NullImage = value;
            }
        }

        Image _GroupImage = null;
        public Image GroupImage
        {
            get { return _GroupImage; }
            set
            {
                if (value == null)
                    picGroup.BackgroundImage = _NullImage;
                else
                    picGroup.BackgroundImage = value;
                _GroupImage = value;
            }
        }

        public bool Split
        {
            get { return pnlSplit.Visible; }
            set { pnlSplit.Visible = value; }
        }

        int _Index = 0;
        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }

        object _MyObject = new object();
        public object MyObject
        {
            get
            {
                return this._MyObject;
            }
            set
            {
                this._MyObject = value;
            }
        }

        Color _ColorControl = Color.FromArgb(100, 150, 250);
        public Color ColorControl
        {
            get { return _ColorControl; }
            set { _ColorControl = value; this.BackColor = _ColorControl; }
        }
        #endregion

        private void Group_Click(object sender, EventArgs e)
        {
            if (SelectGroup != null)
            {              
                Helper.Sound(Application.StartupPath, SoudType.GroupMouseSelect);
                SelectGroup(this, _MyObject, _Index);
            }
        }

        bool runSound = false;
        private void picGroup_MouseEnter(object sender, EventArgs e)
        {
            if (!runSound)
            {
                Helper.Sound(Application.StartupPath, SoudType.GroupMouseEnter);              
                runSound = true;
            }

            picKey.BackColor = lblGroup.ForeColor = this.BackColor;
            pnlGroup.BackColor = Color.FromArgb(100, 150, 250);            
            picGroup.BackgroundImage = _GroupImage;
        }

        private void picGroup_MouseLeave(object sender, EventArgs e)
        {
            if (!_Selected)
            {
                picKey.BackColor = lblGroup.ForeColor = Color.FromArgb(100, 150, 250);
                pnlGroup.BackColor = this.BackColor;
                picGroup.BackgroundImage = _NullImage;
            }
            if (!this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
                runSound = false;
        }
    }
}
