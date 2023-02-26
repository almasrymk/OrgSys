using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORG.Tools.Desktop.Tools
{
    public partial class GroupByItemsTool : UserControl
    {
        public event EventHandler SelectItem;
        public GroupByItemsTool()
        {
            InitializeComponent();
        }

        Image _ImageKeyTitleOpen = null;
        public Image ImageKeyTitleOpen
        {
            get { return _ImageKeyTitleOpen; }
            set
            {
                _ImageKeyTitleOpen = value;             
            }
        }

        Image _ImageKeyTitleClose = null;
        public Image ImageKeyTitleClose
        {
            get { return _ImageKeyTitleClose; }
            set
            {
                _ImageKeyTitleClose = value;               
            }
        }

        Image _ImageKeyOpen = null;
        public Image ImageKeyOpen
        {
            get { return _ImageKeyOpen; }
            set
            {
                _ImageKeyOpen = value;               
            }
        }

        Image _ImageKeyClose = null;
        public Image ImageKeyClose
        {
            get { return _ImageKeyClose; }
            set
            {
                _ImageKeyClose = value;                
            }
        }

        bool _OpenClose = false;
        public bool OpenClose
        {
            get { return _OpenClose; }
            set
            {
                _OpenClose = value;
                if (_OpenClose)
                {
                    picTitle.Image = _ImageKeyTitleOpen;
                    picKey.Image = _ImageKeyOpen;                   
                }
                else
                {
                    picTitle.Image = _ImageKeyTitleClose;
                    picKey.Image = _ImageKeyClose;                   
                }
            }
        }

        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                lblTitle.Text = _Title;
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

        public string GroupName
        {
            get
            {
                return this.lblTitle.Text;
            }
            set
            {
                this.lblTitle.Text = value;
            }
        }

        Image _NullImage = null;
        public Image NullImage
        {
            get { return _NullImage; }
            set
            {
                if (_GroupImage == null)
                    picTitle.BackgroundImage = value;
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
                    picTitle.BackgroundImage = _NullImage;
                else
                    picTitle.BackgroundImage = value;
                _GroupImage = value;
            }
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

        private void OpenClose_Click(object sender, EventArgs e)
        {
            OpenClose = !OpenClose;
            if (SelectItem != null && OpenClose)
                SelectItem(this, e);
            //if(_OpenClose)
            //    this.Height = panelUnitTool1.MaxHieght + 100;
            //else
            //    this.Height = 50;            
        }
    }
}
