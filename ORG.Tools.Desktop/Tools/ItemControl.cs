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
    public partial class ItemControl : UserControl
    {
        
        public event _SelectItem SelectItem;
       
        #region Proparty
        long _ItemID = 0;
        public long ItemID
        {
            get { return _ItemID; }
            set { _ItemID = value; }
        }

        string _ItemCode = string.Empty;
        public string ItemCode
        {
            get { return _ItemCode; }
            set { _ItemCode = value; }
        }

        string _ItemName = "Item Name";
        public string ItemName
        {
            get { return _ItemName; }
            set { lblItemName.Text = _ItemName = value; }
        }

        bool _Selected = false;
        public bool Selected
        {
            get { return _Selected; }
            set { _Selected = value; }
        }

        Nullable<decimal> _Quantity = 0;
        public Nullable<decimal> Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; }
        }

        Nullable<decimal> _Price = 0;
        public Nullable<decimal> Price
        {
            get { return _Price; }
            set { _Price = value;
            lblItemPrice.Text = value.ToString();
            }
        }

        object _item = new object();
        public object Item
        {
            get { return _item; }
            set { _item = value; }
        }
        #endregion

        #region Event
        public ItemControl()
        {
            InitializeComponent();
        }

        private void Select_Click(object sender, EventArgs e)
        {
            if (SelectItem != null)
                SelectItem(this);
        }

        private void picImage_MouseDown(object sender, MouseEventArgs e)
        {
            this.BackColor = Color.FromArgb(98, 174, 225);
        }

        private void picImage_MouseUp(object sender, MouseEventArgs e)
        {
            this.BackColor = Color.Gainsboro;
        }

        private void picImage_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Gainsboro;
        }
        #endregion
    }
}
