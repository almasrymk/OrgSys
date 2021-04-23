
namespace OrgSysApp.Tools
{
    partial class MainMenu
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnData = new OrgSysApp.Tools.ButtonImage();
            this.btnInvoice = new OrgSysApp.Tools.ButtonImage();
            this.btnReturn = new OrgSysApp.Tools.ButtonImage();
            this.btnReport = new OrgSysApp.Tools.ButtonImage();
            this.SuspendLayout();
            // 
            // btnData
            // 
            this.btnData.active = false;
            this.btnData.activeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnData.BackColor = System.Drawing.Color.Transparent;
            this.btnData.ButtonName = "Data";
            this.btnData.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnData.font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnData.fontColor = System.Drawing.Color.Gray;
            this.btnData.fontColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnData.image = global::OrgSysApp.Properties.Resources.Data1;
            this.btnData.imageActive = global::OrgSysApp.Properties.Resources.Data;
            this.btnData.layOut = System.Windows.Forms.ImageLayout.Zoom;
            this.btnData.Location = new System.Drawing.Point(0, 0);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(100, 78);
            this.btnData.TabIndex = 0;
            this.btnData.Title = "Data";
            this.btnData.click += new System.EventHandler(this.this_Click);
            // 
            // btnInvoice
            // 
            this.btnInvoice.active = false;
            this.btnInvoice.activeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnInvoice.BackColor = System.Drawing.Color.Transparent;
            this.btnInvoice.ButtonName = "Invoice";
            this.btnInvoice.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnInvoice.font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnInvoice.fontColor = System.Drawing.Color.Gray;
            this.btnInvoice.fontColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnInvoice.image = global::OrgSysApp.Properties.Resources._01InvoiceActiv111e;
            this.btnInvoice.imageActive = global::OrgSysApp.Properties.Resources._01InvoiceActives;
            this.btnInvoice.layOut = System.Windows.Forms.ImageLayout.Zoom;
            this.btnInvoice.Location = new System.Drawing.Point(100, 0);
            this.btnInvoice.Name = "btnInvoice";
            this.btnInvoice.Size = new System.Drawing.Size(100, 78);
            this.btnInvoice.TabIndex = 0;
            this.btnInvoice.Title = "Invoice";
            this.btnInvoice.click += new System.EventHandler(this.this_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.active = false;
            this.btnReturn.activeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnReturn.BackColor = System.Drawing.Color.Transparent;
            this.btnReturn.ButtonName = "Return";
            this.btnReturn.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnReturn.font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnReturn.fontColor = System.Drawing.Color.Gray;
            this.btnReturn.fontColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnReturn.image = global::OrgSysApp.Properties.Resources._01ReturnInvoiceActiveB22;
            this.btnReturn.imageActive = global::OrgSysApp.Properties.Resources._01InvoiceActiveB22;
            this.btnReturn.layOut = System.Windows.Forms.ImageLayout.Zoom;
            this.btnReturn.Location = new System.Drawing.Point(200, 0);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(100, 78);
            this.btnReturn.TabIndex = 0;
            this.btnReturn.Title = "Return";
            this.btnReturn.click += new System.EventHandler(this.this_Click);
            // 
            // btnReport
            // 
            this.btnReport.active = false;
            this.btnReport.activeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnReport.BackColor = System.Drawing.Color.Transparent;
            this.btnReport.ButtonName = "Report";
            this.btnReport.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnReport.font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnReport.fontColor = System.Drawing.Color.Gray;
            this.btnReport.fontColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnReport.image = global::OrgSysApp.Properties.Resources.Report1;
            this.btnReport.imageActive = global::OrgSysApp.Properties.Resources.Report;
            this.btnReport.layOut = System.Windows.Forms.ImageLayout.Zoom;
            this.btnReport.Location = new System.Drawing.Point(300, 0);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(100, 78);
            this.btnReport.TabIndex = 0;
            this.btnReport.Title = "Report";
            this.btnReport.click += new System.EventHandler(this.this_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnInvoice);
            this.Controls.Add(this.btnData);
            this.Name = "MainMenu";
            this.Size = new System.Drawing.Size(579, 78);
            this.ResumeLayout(false);

        }

        #endregion

        private ButtonImage btnData;
        private ButtonImage btnInvoice;
        private ButtonImage btnReturn;
        private ButtonImage btnReport;
    }
}
