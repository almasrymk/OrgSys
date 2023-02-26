using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using ORG.UIL.Desktop.Froms.General;
using ORGEntity;

namespace ORG.UIL.Desktop
{
    public enum CountButton { One, Two }
    public enum TypeMessage { Saved, Deleted }
    public static class MessageORG
    {
        public static DialogResult ShowConfirm(string Title, string Message)
        {
            frm_Message _frm_Message = new frm_Message(Title, Message, true);
            DialogResult _DialogResult = _frm_Message.ShowDialog();
            Application.OpenForms["frm_Main"].Activate();
            return _DialogResult;
        }

        public static DialogResult Show(string Title, string Message)
        {
            frm_Message _frm_Message = new frm_Message(Title, Message);
            DialogResult _DialogResult = _frm_Message.ShowDialog();
            Application.OpenForms["frm_Main"].Activate();
            return _DialogResult;
        }

        public static DialogResult Show(string Title, string Message, MessageBoxIcon icon)
        {
            frm_Message _frm_Message = new frm_Message(Title, Message, icon);
            DialogResult _DialogResult = _frm_Message.ShowDialog();
            Application.OpenForms["frm_Main"].Activate();
            return _DialogResult;
        }

        public static DialogResult Show(string Title, string Message, CountButton TypeButton)
        {
            frm_Message _frm_Message = new frm_Message(Title, Message, TypeButton);
            DialogResult _DialogResult = _frm_Message.ShowDialog();
            Application.OpenForms["frm_Main"].Activate();
            return _DialogResult;
        }

        public static DialogResult Show(string Title, string Message, string ScreenActive, CountButton TypeButton, MessageBoxIcon icon)
        {
            frm_Message _frm_Message = new frm_Message(Title, Message, TypeButton, icon);
            DialogResult _DialogResult = _frm_Message.ShowDialog();
            Application.OpenForms[ScreenActive].Activate();
            return _DialogResult;
        }


        public static DialogResult Show(string Title, string Message, CountButton TypeButton, MessageBoxIcon icon)
        {
            frm_Message _frm_Message = new frm_Message(Title, Message, TypeButton, icon);
            DialogResult _DialogResult = _frm_Message.ShowDialog();
            Application.OpenForms["frm_Main"].Activate();
            return _DialogResult;
        }

        public static List<Item> ShowItemSearch(List<int> ids)
        {
            frm_SearchItems _frm_Message = new frm_SearchItems(ids);
            DialogResult _DialogResult = _frm_Message.ShowDialog();
            Application.OpenForms["frm_Main"].Activate();
            if (_DialogResult == DialogResult.OK)
                return _frm_Message.Items;
            return new List<Item>();
        }
    }
}
