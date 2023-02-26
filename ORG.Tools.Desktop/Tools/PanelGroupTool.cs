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
using System.IO;

namespace ORG.Tools.Desktop.Tools
{
    public partial class PanelGroupTool : UserControl
    {
        public event EventHandler SelectITem; 
        public event ClickOB SelectImage;
        bool Flag = false;
       public object ObjectSelected;
        /// <summary>
        /// 
        /// </summary>
        public PanelGroupTool()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="tt"></typeparam>
        /// <param name="items"></param>        
        public void FillItems<tt>(List<tt> items)
        {
            this.panel1.Controls.Clear();
            int CountGroup = 0;
            if (items == null || items.Count == 0) return;
            Application.DoEvents();
            foreach (object _item in items)
            {
                if (CountGroup != this.panel1.Controls.Count) break;
                Application.DoEvents();
                GroupUnitTool _frm_GroupUnit = new GroupUnitTool();
                _frm_GroupUnit.Width = _WidthItem;
                _frm_GroupUnit.Left = 10;                
                _frm_GroupUnit.Height = _HeightItem;
                _frm_GroupUnit.Margin = new Padding(5);
                _frm_GroupUnit.Index = CountGroup + 1;
                _frm_GroupUnit.MyObject = _item;
                _frm_GroupUnit.ColorControl = _ColorControl;
                _frm_GroupUnit.lblGroup.Font = _FontTitle;
                _frm_GroupUnit.SelectGroup += new _selectGroup(selectGroup);
                Type t = _item.GetType();
                PropertyInfo ID = t.GetProperty("Id");
                if (ID != null)
                    _frm_GroupUnit.ID = long.Parse("0" + ID.GetValue(_item, null).ToString());

                PropertyInfo NameF = t.GetProperty("Name");
                if (ID != null)
                    _frm_GroupUnit.GroupName = NameF.GetValue(_item, null).ToString();
                if (CountGroup == 0)
                    _frm_GroupUnit.Split = false;


                PropertyInfo ImageFile = t.GetProperty("ImageFile");
                if (ImageFile != null)
                {
                    var img = (byte[])ImageFile.GetValue(_item, null);
                    if(img != null && img.Length > 0)
                    {
                        MemoryStream ms = new MemoryStream(img);                        
                        _frm_GroupUnit.NullImage = Image.FromStream(ms);
                        _frm_GroupUnit.GroupImage = Image.FromStream(ms);
                        _frm_GroupUnit.picGroup.BackgroundImage = Image.FromStream(ms);
                    }
                    else
                    {
                        _frm_GroupUnit.NullImage = _NullImage;
                        _frm_GroupUnit.GroupImage = Select_Image;
                        _frm_GroupUnit.picGroup.BackgroundImage = _NullImage;
                    }
                }
                else
                {
                    _frm_GroupUnit.NullImage = _NullImage;
                    _frm_GroupUnit.GroupImage = Select_Image;
                    _frm_GroupUnit.picGroup.BackgroundImage = _NullImage;
                }
                            
                this.panel1.Controls.Add(_frm_GroupUnit);
                _frm_GroupUnit.BringToFront();
                CountGroup++;
            }           
        }

        Image _NullImage = null;
        public Image NullImage
        {
            get { return _NullImage; }
            set { _NullImage = value; }
        }

        Image _SelectImage = null;
        public Image Select_Image
        {
            get { return _SelectImage; }
            set { _SelectImage = value; }
        }

        Color _ColorControl = Color.FromArgb(100, 150, 250);
        public Color ColorControl
        {
            get { return _ColorControl; }
            set { _ColorControl = value; }
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

        int _WidthItem = 112;
        public int WidthItem
        {
            get { return _WidthItem; }
            set { _WidthItem = value; }
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
            for (int i = 0; i < this.panel1.Controls.Count; i++)
            {
                ((GroupUnitTool)this.panel1.Controls[i]).Selected = false;
            }
            MyControl.Selected = !MyControl.Selected;
            ObjectSelected = OB;
            SelectITem(ObjectSelected, null);
            return ObjectSelected;
        }

        Font _FontTitle = new Font("Arial", 10, FontStyle.Bold);
        public Font FontTitle
        {
            get { return _FontTitle; }
            set
            {
                _FontTitle = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ResizeForm(object sender, EventArgs e)
        {
            int x = 10, y = 10;
            for (int i = 0; i < this.panel1.Controls.Count; i++)
            {
                Application.DoEvents();
                this.panel1.Controls[i].Location = GlobalsMembers.LocationItem(10, 10, 10, 10, this.panel1.Width, this.Controls[i].Height, this.panel1.Controls[i].Width, ref x, ref y);
            }

        }
    }
}
