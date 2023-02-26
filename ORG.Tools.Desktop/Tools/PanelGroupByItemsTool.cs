using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace ORG.Tools.Desktop.Tools
{
    public partial class PanelGroupByItemsTool : UserControl
    {
        public PanelGroupByItemsTool()
        {
            InitializeComponent();
        }

        Color _ColorBackGround = Color.WhiteSmoke;
        public Color ColorBackGround
        {
            get { return _ColorBackGround; }
            set
            {
                _ColorBackGround = value;
                this.BackColor = _ColorBackGround;
            }
        }

        public event EventHandler SelectITem;
        bool Flag = false;
        object ObjectSelected;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="tt"></typeparam>
        /// <param name="items"></param>        
        public void FillItems<tt>(List<tt> items)
        {
            this.pnlMain.Controls.Clear();
            pnlMain.Visible = false;
            int CountGroup = 0;
            if (items == null || items.Count == 0) return;
            Application.DoEvents();
            foreach (object _item in items)
            {
                if (CountGroup != this.pnlMain.Controls.Count) break;
                //Application.DoEvents();
                GroupByItemsTool _frm_GroupUnit = new GroupByItemsTool();
                _frm_GroupUnit.Width = this.Width - 40;
                //_frm_GroupUnit.Left = 10;
                _frm_GroupUnit.Dock = DockStyle.Top;
                _frm_GroupUnit.Height = _HeightItem;
                _frm_GroupUnit.ImageKeyOpen = _ImageKeyOpen;
                _frm_GroupUnit.ImageKeyClose = _ImageKeyClose;
                _frm_GroupUnit.ImageKeyTitleOpen = _ImageKeyTitleOpen;
                _frm_GroupUnit.ImageKeyTitleClose = _ImageKeyTitleClose;
                _frm_GroupUnit.OpenClose = false;
                _frm_GroupUnit.SelectItem += SelectItem;
               
                pnlMain.Height = _HeightItem * CountGroup;
                //_frm_GroupUnit.Top = (_HeightItem + 10) * CountGroup;
                //_frm_GroupUnit.Top = _HeightItem * CountGroup + 10;

                _frm_GroupUnit.Index = CountGroup + 1;
                _frm_GroupUnit.MyObject = _item;
                //_frm_GroupUnit.ColorControl = _ColorControl;
                //_frm_GroupUnit.SelectGroup += new _selectGroup(selectGroup);

                Type t = _item.GetType();
                PropertyInfo ID = t.GetProperty("ID");
                if (ID != null)
                    _frm_GroupUnit.ID = long.Parse("0" + ID.GetValue(_item , null).ToString());

                PropertyInfo NameF = t.GetProperty("NameF");
                if (ID != null)
                    _frm_GroupUnit.GroupName = NameF.GetValue(_item, null).ToString();              

                if (_NullImage != null)
                    _frm_GroupUnit.NullImage = _NullImage;


                this.pnlMain.Controls.Add(_frm_GroupUnit);
                _frm_GroupUnit.BringToFront();
                CountGroup++;
            }
            pnlMain.Height += HeightItem;
            pnlMain.Visible = true;
        }

        Image _NullImage = null;
        public Image NullImage
        {
            get { return _NullImage; }
            set { _NullImage = value; }
        }

        Color _ColorControl = Color.FromArgb(100, 150, 250);
        public Color ColorControl
        {
            get { return _ColorControl; }
            set { _ColorControl = value; }
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

        /// <summary>
        /// 
        /// </summary>
        int _HeightItem = 50;
        public int HeightItem
        {
            get { return _HeightItem; }
            set { _HeightItem = value; }
        }

        int _WidhtItem = 100;
        public int WidhtItem
        {
            get { return _WidhtItem; }
            set { _WidhtItem = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyControl"></param>
        /// <param name="OB"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public object selectGroup(GroupUnitTool MyControl, object OB, int Index)
        {
            for (int i = 0; i < this.Controls.Count; i++)
            {
                ((GroupUnitTool)this.Controls[i]).Selected = false;
            }
            MyControl.Selected = !MyControl.Selected;
            ObjectSelected = OB;
            SelectITem(ObjectSelected, null);
            return ObjectSelected;
        }

        public void SelectItem(object sender, EventArgs e)
        {
            if(SelectITem != null)
                SelectITem(sender , e);
        }
    }
}
