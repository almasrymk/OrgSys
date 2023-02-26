namespace ORG.Tools.Desktop
{
    partial class MenuItemTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuItemTool));
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.picTitle = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.picTitle)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Click += new System.EventHandler(this.Controls_Click);
            this.lblTitle.MouseEnter += new System.EventHandler(this.lblTitle_MouseEnter);
            this.lblTitle.MouseLeave += new System.EventHandler(this.lblTitle_MouseLeave);
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            resources.ApplyResources(this.pnlFooter, "pnlFooter");
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Click += new System.EventHandler(this.Controls_Click);
            this.pnlFooter.MouseEnter += new System.EventHandler(this.lblTitle_MouseEnter);
            this.pnlFooter.MouseLeave += new System.EventHandler(this.lblTitle_MouseLeave);
            // 
            // picTitle
            // 
            resources.ApplyResources(this.picTitle, "picTitle");
            this.picTitle.Name = "picTitle";
            this.picTitle.TabStop = false;
            this.picTitle.Click += new System.EventHandler(this.Controls_Click);
            this.picTitle.MouseEnter += new System.EventHandler(this.lblTitle_MouseEnter);
            this.picTitle.MouseLeave += new System.EventHandler(this.lblTitle_MouseLeave);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.picTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblTitle, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pnlFooter, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.MouseEnter += new System.EventHandler(this.lblTitle_MouseEnter);
            this.tableLayoutPanel1.MouseLeave += new System.EventHandler(this.lblTitle_MouseLeave);
            // 
            // MenuItemTool
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MenuItemTool";
            this.Click += new System.EventHandler(this.Controls_Click);
            this.MouseEnter += new System.EventHandler(this.lblTitle_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.lblTitle_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.picTitle)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
