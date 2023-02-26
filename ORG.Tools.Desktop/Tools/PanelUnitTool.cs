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
using System.Drawing.Imaging;

namespace ORG.Tools.Desktop.Tools
{
    public partial class PanelUnitTool : UserControl
    {
        public event EventHandler SelectITem;        
        public event _Pagging pagging;
        public int CurrPage = 1;
        public PanelUnitTool()
        {
            InitializeComponent();
        }

        public object ObjectSelected;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="tt"></typeparam>
        /// <param name="items"></param>        
        public void FillItems<tt>(List<tt> items)
        {
            if (CurrPage == 1)
            {
                this.pnlMain.Controls.Clear();
            }

            lblPagging.Visible = items!= null && items.Count == 50;

            int CountGroup = 0, x = 10, y = 10, valueStap = 0;
            lblProgress.Width = 0;
            if (items == null || items.Count == 0) return;
            pnlProgress.Visible = lblProgress.Visible = true;
            if (items.Count > 0)
                valueStap = pnlProgress.Width / items.Count;
            Application.DoEvents();
            foreach (object _item in items)
            {
                lblProgress.Width += valueStap;
                Application.DoEvents();
                UnitTool _frm_GroupUnit = new UnitTool();
                _frm_GroupUnit.Height = _HeightItem;
                _frm_GroupUnit.Width = _WidthItem;
                _frm_GroupUnit.MyObject = _item;
                _frm_GroupUnit.ClickObject += new ClickOB(selectGroup);
                _frm_GroupUnit.ColorPic = _ColorPic;
                _frm_GroupUnit.ColorBody = _ColorBody;
                _frm_GroupUnit.lblName.Font = _FontTitle;
                Type t = _item.GetType();
                PropertyInfo ID = t.GetProperty("Id");
                if (ID != null)
                    _frm_GroupUnit.ID = long.Parse("0" + ID.GetValue(_item, null).ToString());

                PropertyInfo Name = t.GetProperty("Name");
                if (Name != null)
                    _frm_GroupUnit.UnitName = Name.GetValue(_item, null).ToString();
                PropertyInfo NameF = t.GetProperty("NameF");
                if (NameF != null)
                    _frm_GroupUnit.UnitName = NameF.GetValue(_item, null).ToString();
                PropertyInfo UserName = t.GetProperty("UserName");
                if (UserName != null)
                    _frm_GroupUnit.UnitName =("" + UserName.GetValue(_item, null)).ToString();
                long _ItemID = _frm_GroupUnit.ID;
                PropertyInfo ItemID = t.GetProperty("ItemID");
                if (ItemID != null)
                    _ItemID = long.Parse("0" + ItemID.GetValue(_item, null).ToString());

                Bitmap comp = new Bitmap(NullImage);
                MemoryStream ms = new MemoryStream();
                comp.Save(ms, ImageFormat.Png);
                _frm_GroupUnit.UnitImage = ms.ToArray();

                PropertyInfo ImageFile = t.GetProperty("ImageFile");
                if (ImageFile != null)
                {
                    var img = (byte[])ImageFile.GetValue(_item, null);
                    if (img != null)
                        _frm_GroupUnit.UnitImage = img;                    
                }      
               
                this.pnlMain.Controls.Add(_frm_GroupUnit);
                CountGroup++;
            }
            pnlProgress.Visible = lblProgress.Visible = false;
        }

        Image _NullImage = null;
        public Image NullImage
        {
            get { return _NullImage; }
            set { _NullImage = value; }
        }

        int _HeightItem = 100;
        public int HeightItem
        {
            get { return _HeightItem; }
            set { _HeightItem = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        int _WidthItem = 150;
        public int WidthItem
        {
            get { return _WidthItem; }
            set { _WidthItem = value; }
        }
    
        Color _ColorPic = Color.FromArgb(100, 150, 250);
        public Color ColorPic
        {
            get { return _ColorPic; }
            set { _ColorPic = value; }
        }

        Color _ColorBody = Color.WhiteSmoke;
        public Color ColorBody
        {
            get { return _ColorBody; }
            set { _ColorBody = value; }
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

        Font _FontDescription = new Font("Arial", 10, FontStyle.Bold);
        public Font FontDescription
        {
            get { return _FontDescription; }
            set
            {
                _FontDescription = value;
            }
        }

        string _UnitDescription = "";
        public string UnitDescription
        {
            get { return _UnitDescription; }
            set { _UnitDescription = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyControl"></param>
        /// <param name="OB"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public object selectGroup(object OB)
        {
            ObjectSelected = OB;
            Type t = ObjectSelected.GetType();
            PropertyInfo ID = t.GetProperty("Id");
            if (SelectITem != null)
            {
                if (ID != null)
                    SelectITem(int.Parse("0" + ID.GetValue(ObjectSelected).ToString()), null);
            }
            return ObjectSelected;
        }

        private void lblPagging_Click(object sender, EventArgs e)
        {
            if (pagging != null)
                pagging(++CurrPage);
        }
    }
}
