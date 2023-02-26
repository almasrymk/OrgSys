namespace ORG.UIL.Desktop
{
    partial class frm_Invoice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Invoice));
            this.pnlControls.SuspendLayout();
            this.pnlControlNew.SuspendLayout();
            this.SuspendLayout();
            // 
            // bttnNew
            // 
            // 
            // bttnCancel
            // 
            this.bttnCancel.BackgroundImage = global::ORG.UIL.Desktop.Properties.Resources._02Search__3_;
            resources.ApplyResources(this.bttnCancel, "bttnCancel");
            // 
            // toolSearchInfo1
            // 
            this.toolSearchInfo1.EMPImage = global::ORG.UIL.Desktop.Properties.Resources.user_iconb;
            this.toolSearchInfo1.SearchEvent += new ORG.UIL._SearchEvent(this.toolSearchInfo1_SearchEvent);
            this.toolSearchInfo1.pagging += new ORG.UIL._Pagging(this.toolSearchInfo1_pagging);
            this.toolSearchInfo1.SelectEdit += new ORG.UIL._Select(this.toolSearchInfo1_SelectEdit);
            this.toolSearchInfo1.PrintInvoice += new ORG.UIL._Select(this.toolSearchInfo1_PrintInvoice);
            this.toolSearchInfo1.DeleteInvoice += new ORG.UIL._Select(this.toolSearchInfo1_DeleteInvoice);
            // 
            // toolCashier1
            // 
            this.toolCashier1.UnitDescription = "PruchesPrice1";
            this.toolCashier1.SelectCategroy += new ORG.UIL._Select(this.toolCashier1_SelectCategroy);
            this.toolCashier1.SelectItemInvoice += new ORG.UIL._SelectOb(this.toolCashier1_SelectItemInvoice);
            this.toolCashier1.SearchItem += new ORG.UIL._SearchEvent(this.toolCashier1_SearchItem);
            this.toolCashier1.pagging += new ORG.UIL._Pagging(this.toolCashier1_pagging);
            // 
            // timer1
            // 
            this.timer1.Interval = 100;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frm_Invoice
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "frm_Invoice";
            this.Load += new System.EventHandler(this.frm_Invoice_Load);
            this.pnlControls.ResumeLayout(false);
            this.pnlControlNew.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}