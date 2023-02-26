namespace ORG.UIL.Desktop.Froms.Data
{
    partial class ItemGroupInvoice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemGroupInvoice));
            this.label7 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bttnRefrach = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.bttnNew = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlMainData = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.pnlMainData.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.label7.Name = "label7";
            // 
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.ForeColor = System.Drawing.Color.DimGray;
            this.txtName.Name = "txtName";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.bttnRefrach);
            this.panel1.Controls.Add(this.bttnNew);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Name = "panel1";
            // 
            // bttnRefrach
            // 
            this.bttnRefrach.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnRefrach.Color = System.Drawing.Color.WhiteSmoke;
            this.bttnRefrach.ColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            resources.ApplyResources(this.bttnRefrach, "bttnRefrach");
            this.bttnRefrach.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnRefrach.FontColorEnter = System.Drawing.Color.White;
            this.bttnRefrach.Image = global::ORG.UIL.Desktop.Properties.Resources._02Cancelb;
            this.bttnRefrach.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources._02Cancel;
            this.bttnRefrach.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnRefrach.Name = "bttnRefrach";
            this.bttnRefrach.WithSplitter = false;
            this.bttnRefrach.WithTitle = true;
            this.bttnRefrach.Click += new System.EventHandler(this.bttnRefrach_Click);
            // 
            // bttnNew
            // 
            this.bttnNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnNew.Color = System.Drawing.Color.WhiteSmoke;
            this.bttnNew.ColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            resources.ApplyResources(this.bttnNew, "bttnNew");
            this.bttnNew.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnNew.FontColorEnter = System.Drawing.Color.White;
            this.bttnNew.Image = global::ORG.UIL.Desktop.Properties.Resources._02Saveb;
            this.bttnNew.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources._02Save;
            this.bttnNew.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnNew.Name = "bttnNew";
            this.bttnNew.WithSplitter = true;
            this.bttnNew.WithTitle = true;
            this.bttnNew.Click += new System.EventHandler(this.bttnNew_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Name = "label1";
            // 
            // pnlMainData
            // 
            resources.ApplyResources(this.pnlMainData, "pnlMainData");
            this.pnlMainData.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlMainData.Controls.Add(this.label7);
            this.pnlMainData.Controls.Add(this.txtName);
            this.pnlMainData.Name = "pnlMainData";
            // 
            // ItemGroupInvoice
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlMainData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ItemGroupInvoice";
            this.panel1.ResumeLayout(false);
            this.pnlMainData.ResumeLayout(false);
            this.pnlMainData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtName;
        public Tools.Desktop.Tools.BottunOrg bttnRefrach;
        public System.Windows.Forms.Panel panel1;
        public Tools.Desktop.Tools.BottunOrg bttnNew;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Panel pnlMainData;
    }
}