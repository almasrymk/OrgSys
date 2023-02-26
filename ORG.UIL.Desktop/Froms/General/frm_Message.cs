using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORG.UIL.Desktop
{
    public partial class frm_Message : Form
    {
        string title = string.Empty, message = string.Empty;
        frm_panelblack _frm_panelMessage;
        CountButton _TypeButton = CountButton.Two;
        MessageBoxIcon icon;
               
        public frm_Message(string Title, string Message , bool Confirm = false)
        {
            InitializeComponent();
            _frm_panelMessage = new frm_panelblack();
            _frm_panelMessage.Name = "_frm_panelMessage";
            _frm_panelMessage.Show();
            lblTitle.Text = title = Title;
            lblMessage.Text = message = Message;
            if (Confirm)
            {
                picIcon.Visible = false;
                pictureBox1.Visible = true;
                panel3.Visible = false;
                timer1.Enabled = true;
            }
        }

        public frm_Message(string Title, string Message, MessageBoxIcon Icon)
        {
            InitializeComponent();
            _frm_panelMessage = new frm_panelblack();
            _frm_panelMessage.Name = "_frm_panelMessage";
            _frm_panelMessage.Show();
            lblTitle.Text = title = Title;
            lblMessage.Text = message = Message;
            Location();
            switch (Icon)
            {
                case MessageBoxIcon.Question:
                    picIcon.BackgroundImage = Properties.Resources.q;
                    break;

                case MessageBoxIcon.Error:
                    picIcon.BackgroundImage = Properties.Resources.x;
                    break;
            }
        }

        public frm_Message(string Title, string Message, CountButton TypeButton)
        {
            InitializeComponent();
            _frm_panelMessage = new frm_panelblack();
            _frm_panelMessage.Name = "_frm_panelMessage";
            _frm_panelMessage.Show();
            lblTitle.Text = title = Title;
            lblMessage.Text = message = Message;
            _TypeButton = TypeButton;
            Location();           
            //picIcon.BackgroundImage = icon = Icon;
        }

        public frm_Message(string Title, string Message, CountButton TypeButton, MessageBoxIcon Icon)
        {
            InitializeComponent();
            _frm_panelMessage = new frm_panelblack();
            _frm_panelMessage.Name = "_frm_panelMessage";
            _frm_panelMessage.Show();
            lblTitle.Text = title = Title;
            lblMessage.Text = message = Message;
            _TypeButton = TypeButton;
            Location();
            switch (Icon)
            {
                case MessageBoxIcon.Question:
                    picIcon.BackgroundImage = Properties.Resources.q;
                    break;

                case MessageBoxIcon.Error:
                    picIcon.BackgroundImage = Properties.Resources.x;
                    break;
            }
        }

        private void bttnOk_Click(object sender, EventArgs e)
        {
            Application.OpenForms["_frm_panelMessage"].Close();
            //Application.OpenForms[this.Parent.Parent.Name].Activate();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void bttnCancel_Click(object sender, EventArgs e)
        {
            Application.OpenForms["_frm_panelMessage"].Close();
            //Application.OpenForms[this.Parent.Parent.Name].Activate();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Application.OpenForms["_frm_panelMessage"].Close();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void bttnOk2_Click(object sender, EventArgs e)
        {
            Application.OpenForms["_frm_panelMessage"].Close();
            //Application.OpenForms[this.Parent.Parent.Name].Activate();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Location()
        {
            if (_TypeButton == CountButton.Two)
            {
                bttnCancel.Visible = true;
                bttnOk.Visible = true;
                bttnOk2.Visible = false;
            }
            else
            {
                bttnCancel.Visible = false;
                bttnOk.Visible = false;
                bttnOk2.Visible = true;
            }
        }
    }
}
