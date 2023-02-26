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

namespace Farm.Tools
{
    public partial class txtBox : UserControl
    {
        #region Varibles
        bool Locked = false, LockTexted = false;
        public event EventHandler ClickFinder;
        public event EventHandler CodeChange;
        public event EventHandler TextChange;
        #endregion

        #region Properties
        [Category("US")]
        public string TextValue
        {
            get
            {
                return txt1.Text.Trim().Replace("%", "");
            }
            set
            {
                if (!LockTexted)
                    txt1.Text =  value;
            }
        }

        bool _Multiline = false;
        [Category("US")]
        public bool Multiline
        {
            get { return _Multiline; }
            set { _Multiline = value; }
        }

        typeText _TypeText = typeText.String;
        [Category("US")]
        public typeText TypeText
        {
            get
            { return _TypeText; }
            set
            {
                _TypeText = value;
                switch (_TypeText)
                {
                    case typeText.String:
                    case typeText.Date:
                    case typeText.DateTime:
                        txt1.Text = "";
                        break;
                    case typeText.Float:
                        txt1.Text = "0.00";
                        break;
                    case typeText.Integer:
                        txt1.Text = "0";
                        break;
                    case typeText.Decimal:
                        txt1.Text = "0.00";
                        break;
                    case typeText.Percent:
                        txt1.Text = "%0.00";
                        break;
                    default:
                        break;
                }
            }
        }

        Color _BackColor = Color.Transparent;
        [Category("US")]
        public  Color BackColorContol
        {
            get
            {
                return _BackColor;
            }
            set
            {
                _BackColor = value;
               txtCode.BackColor = txt1.BackColor = _BackColor;
                this.BackColor = Color.Transparent;
            }
        }

        Color _BackColorButton = Color.Transparent;
        [Category("US")]
        public  Color BackColorButton
        {
            get
            {
                return _BackColorButton;
            }
            set
            {
                _BackColorButton = value;
                btn1.BackColor = label1.BackColor= _BackColorButton;
                this.BackColor = Color.Transparent;
            }
        }

        Color _EnterColor = Color.Transparent;
        [Category("US")]
        public Color EnterColor
        {
            get
            {
                return _EnterColor;
            }
            set
            {
                _EnterColor = value;
            }
        }

        Color _FontColor = Color.Transparent;
        [Category("US")]
        public Color FontColor
        {
            get
            {
                return _FontColor;
            }
            set
            {
                _FontColor = value;
                txtCode.ForeColor = txt1.ForeColor = _FontColor;
                this.BackColor = Color.Transparent;
            }
        }

        Color _FontEnterColor = Color.Transparent;
        [Category("US")]
        public Color FontEnterColor
        {
            get
            {
                return _FontEnterColor;
            }
            set
            {
                _FontEnterColor = value;
            }
        }

        int _WidhtCode = 37;
        [Category("US")]
        public int WidhtCode
        {
            get { return _WidhtCode; }
            set { _WidhtCode = value;
                txtCode.Width = _WidhtCode;
            }
        }

        bool _Required = false;
        [Category("US")]
        public bool Required
        {
            get { return _Required; }
            set
            {
                _Required = value;
                label1.Visible = _Required;
            }
        }

        HorizontalAlignment _TextAlign = HorizontalAlignment.Center;
        [Category("US")]
        public HorizontalAlignment TextAlign
        {
            get { return _TextAlign; }
            set
            {
                _TextAlign = value;
                txtCode.TextAlign = txt1.TextAlign = _TextAlign;
            }
        }

        long _ID = 0;
        [Browsable(false)]
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        [Browsable(false)]
        public string Serial
        {
            get { return txtCode.Text.Trim(); }
            set
            {
                if (!Locked)
                    txtCode.Text =  value;
            }
        }

        object _OB = null;
        [Browsable(false)]
        public object OB
        {
            get { return _OB; }
            set
            {
                _OB = value;
                if (_OB != null)
                {
                    Type _Type = _OB.GetType();
                    PropertyInfo _Value;

                    if (_ValueMember != null && _ValueMember != "")
                    {
                        _Value = _Type.GetProperty(_ValueMember);
                        if (_Value != null && _Value.GetValue(_OB, null) != null)
                        {
                            _ID = int.Parse("0" + _Value.GetValue(_OB, null));
                        }
                    }
                    else
                        _ID = 0;

                    if (_SerialMember != null && _SerialMember != "")
                    {
                        _Value = _Type.GetProperty(_SerialMember);
                        if (_Value != null && _Value.GetValue(_OB, null) != null)
                        {
                            Serial = _Value.GetValue(_OB, null).ToString();
                        }
                    }
                    else
                        Serial = "";

                    if (_DisplayMember != null && _DisplayMember != "")
                    {
                        _Value = _Type.GetProperty(_DisplayMember);
                        if (_Value != null && _Value.GetValue(_OB, null) != null)
                        {
                            TextValue = _Value.GetValue(_OB, null).ToString();
                        }
                    }
                    else
                        TextValue = "";
                }
                else
                {
                    _ID = 0;
                    Serial = "";
                    TextValue = "";
                }
            }
        }

        string _ValueMember = null;
        [Category("US")]
        public string ValueMember
        {
            get { return _ValueMember; }
            set { _ValueMember = value; }
        }

        string _SerialMember = null;
        [Category("US")]
        public string SerialMember
        {
            get { return _SerialMember; }
            set { _SerialMember = value; }
        }

        string _DisplayMember = null;
        [Category("US")]
        [Localizable(true)]
        public string DisplayMember
        {
            get { return _DisplayMember; }
            set { _DisplayMember = value; }
        }

        bool _Finder = true;
        [Category("US")]
        public bool Finder
        {
            get { return _Finder; }
            set
            {
                _Finder = value;
                btn1.Visible = _Finder;
            }
        }

        bool _CodeAndTetx = true;
        [Category("US")]
        public bool CodeAndTetx
        {
            get { return _CodeAndTetx; }
            set
            {
                _CodeAndTetx = value;
                txtCode.Visible = _CodeAndTetx;
            }
        }
        #endregion

        #region Event
        /// <summary>
        /// Constuctor
        /// </summary>
        public txtBox()
        {
            InitializeComponent();
        }

        private void txtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar.ToString() != "\r")
                {
                    e.Handled = GeneralMember.OnyInteger(((Control)sender).Text, e.KeyChar);
                }
                else if (e.KeyChar.ToString() == "\r" || e.KeyChar.ToString() == "\n")
                {
                    if (!_Multiline)
                    {
                        e.Handled = true;
                        SendKeys.Send("{Tab}");
                        if(_CodeAndTetx )
                            SendKeys.Send("{Tab}");
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void txtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar.ToString() != "\r")
                {
                    switch (_TypeText)
                    {
                        case typeText.Float:
                            e.Handled = GeneralMember.OnyFloat(((Control)sender).Text, e.KeyChar);
                            break;
                        case typeText.Decimal:
                            e.Handled = GeneralMember.OnyDecimal(((Control)sender).Text, e.KeyChar);
                            break;
                        case typeText.Integer:
                            e.Handled = GeneralMember.OnyInteger(((Control)sender).Text, e.KeyChar);
                            break;
                        case typeText.Percent:
                            e.Handled = GeneralMember.OnyFloat(((Control)sender).Text, e.KeyChar);
                            break;                       
                        default:
                            break;
                    }
                }
                else if (e.KeyChar.ToString() == "\r" || e.KeyChar.ToString() == "\n")
                {
                    if (!_Multiline)
                    {
                        e.Handled = true;
                        SendKeys.Send("{Tab}");
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void txtBox_Leave(object sender, EventArgs e)
        {
            try
            {
                txtCode.BackColor = txt1.BackColor = _BackColor;
                txtCode.ForeColor = txt1.ForeColor = _FontColor;
                this.BackColor = Color.Transparent;
                switch (_TypeText)
                {
                    case typeText.Float:
                        if (((Control)sender).Text == "")
                            ((Control)sender).Text = "0.00";
                        ((Control)sender).Text = string.Format("{0:0.00}", float.Parse(((Control)sender).Text));
                        break;
                    case typeText.Integer:
                        if (((Control)sender).Text == "")
                            ((Control)sender).Text = "0";
                        ((Control)sender).Text = string.Format("{0:0}", int.Parse(((Control)sender).Text));
                        break;
                    case typeText.Decimal:
                        if (((Control)sender).Text == "")
                            ((Control)sender).Text = "0.00";
                        ((Control)sender).Text = string.Format("{0:0.00}", decimal.Parse(((Control)sender).Text));
                        break;
                    case typeText.Percent:
                        if (((Control)sender).Text == "")
                            ((Control)sender).Text = "%0.00";
                        ((Control)sender).Text = string.Format("%{0:0.00}", decimal.Parse(((Control)sender).Text.Replace("%", "")));
                        break;
                    case typeText.Date:                   
                        DateTime dt = new DateTime();
                        if (DateTime.TryParse(((Control)sender).Text, out dt))                           
                            ((Control)sender).Text = string.Format("{0:dd/MM/yyyy}", DateTime.Parse(((Control)sender).Text));
                            break;                   
                    default:
                        break;
                }
                if (TextChange != null)
                    TextChange(sender, e);
            }
            catch
            {
                throw;
            }
        }

        private void txtCode_Leave(object sender, EventArgs e)
        {
            try
            {
                txtCode.BackColor = txt1.BackColor = _BackColor;
                txtCode.ForeColor = txt1.ForeColor = _FontColor;
                this.BackColor = Color.Transparent;
                if (((Control)sender).Text == "")
                    ((Control)sender).Text = "";
                ((Control)sender).Text = string.Format("{0:0}", ((Control)sender).Text);
                if (TextChange != null)
                    TextChange(sender, e);
            }
            catch
            {
                throw;
            }
        }

        private void txtBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //if (!((Control)sender).Focused)
                //    txtBox_Leave(sender, e);
                //else
                //{
                    //LockTexted = true;
                    //if (txt1.Text == "")
                    //    OB = null;
                    //if (TextChange != null)
                    //    TextChange(sender, e);
                    //LockTexted = false;
                //}

            }
            catch
            {
                throw;
            }
        }

        private void txt1_KeyDown(object sender, KeyEventArgs e)
        {
            //switch (e.KeyData)
            //{
            //    case Keys.Enter | Keys.Control:
            //        if (!_Multiline)
            //        {
            //            e.Handled = true;
            //            SendKeys.Send("{Tab}");                        
            //        }
            //        break;
            //    case Keys.Enter:
            //        e.Handled = true;
            //        SendKeys.Send("{Tab}");                    
            //        break;
            //    default:
            //        break;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt1_Enter(object sender, EventArgs e)
        {
            txtCode.BackColor = txt1.BackColor = _EnterColor;
            txtCode.ForeColor = txt1.ForeColor = _FontEnterColor;
            this.BackColor = Color.Transparent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!((Control)sender).Focused)
                    txtCode_Leave(sender, e);
                else
                {
                    Locked = true;
                    if (txtCode.Text == "")
                        OB = null;
                    if (CodeChange != null)
                        CodeChange(sender, e);
                    Locked = false;
                }

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn1_Clicked(object sender, EventArgs e)
        {
            if (ClickFinder != null)
                ClickFinder(sender, e);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_RightToLeftChanged(object sender, EventArgs e)
        {
            if (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes)
            {
                txtCode.Dock = DockStyle.Right;
                btn1.Dock = DockStyle.Left;
                label1.Dock = DockStyle.Left;
            }
            else
            {
                txtCode.Dock = DockStyle.Left;
                btn1.Dock = DockStyle.Right;
                label1.Dock = DockStyle.Right;
            }
        }
        #endregion
    }
}