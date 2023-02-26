using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Farm.Tools
{
    public partial class btn : UserControl
    {
        #region Varibles
        public event EventHandler Clicked;
        #endregion

        #region Properties
        string _Title = "Title";
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                //lbl.Text = _Title;
            }
        }
             
        Color _ForeColorTitle = Color.Black;
        public Color ForeColorTitle
        {
            get
            {
                return _ForeColorTitle;
            }
            set
            {
                _ForeColorTitle = value;
               // lbl.ForeColor = _ForeColorTitle;
            }
        }
        #endregion

        #region Event
        /// <summary>
        /// Constuctor
        /// </summary>
        public btn()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.LightGray;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Transparent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Enter | Keys.Control:
                    if (Clicked != null)
                        Clicked(sender, null);
                    break;
                case Keys.Enter:
                    SendKeys.Send("{Tab}");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, EventArgs e)
        {
            if (Clicked != null)
                Clicked(sender, null);
        }
        #endregion       
    }
}
