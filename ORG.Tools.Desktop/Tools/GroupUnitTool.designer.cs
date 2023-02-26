namespace ORG.Tools.Desktop.Tools
{
    partial class GroupUnitTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupUnitTool));
            this.lblGroup = new System.Windows.Forms.Label();
            this.pnlGroup = new System.Windows.Forms.Panel();
            this.picGroup = new System.Windows.Forms.PictureBox();
            this.picKey = new System.Windows.Forms.PictureBox();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.pnlSplit = new System.Windows.Forms.Panel();
            this.pnlGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picKey)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblGroup
            // 
            resources.ApplyResources(this.lblGroup, "lblGroup");
            this.lblGroup.BackColor = System.Drawing.Color.Transparent;
            this.lblGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.Click += new System.EventHandler(this.Group_Click);
            this.lblGroup.MouseEnter += new System.EventHandler(this.picGroup_MouseEnter);
            this.lblGroup.MouseLeave += new System.EventHandler(this.picGroup_MouseLeave);
            // 
            // pnlGroup
            // 
            this.pnlGroup.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlGroup.Controls.Add(this.picGroup);
            this.pnlGroup.Controls.Add(this.lblGroup);
            this.pnlGroup.Controls.Add(this.picKey);
            resources.ApplyResources(this.pnlGroup, "pnlGroup");
            this.pnlGroup.Name = "pnlGroup";
            this.pnlGroup.Click += new System.EventHandler(this.Group_Click);
            this.pnlGroup.MouseEnter += new System.EventHandler(this.picGroup_MouseEnter);
            this.pnlGroup.MouseLeave += new System.EventHandler(this.picGroup_MouseLeave);
            // 
            // picGroup
            // 
            this.picGroup.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.picGroup, "picGroup");
            this.picGroup.Name = "picGroup";
            this.picGroup.TabStop = false;
            this.picGroup.Click += new System.EventHandler(this.Group_Click);
            this.picGroup.MouseEnter += new System.EventHandler(this.picGroup_MouseEnter);
            this.picGroup.MouseLeave += new System.EventHandler(this.picGroup_MouseLeave);
            // 
            // picKey
            // 
            this.picKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            resources.ApplyResources(this.picKey, "picKey");
            this.picKey.Name = "picKey";
            this.picKey.TabStop = false;
            this.picKey.Click += new System.EventHandler(this.Group_Click);
            this.picKey.MouseEnter += new System.EventHandler(this.picGroup_MouseEnter);
            this.picKey.MouseLeave += new System.EventHandler(this.picGroup_MouseLeave);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.pnlSplit);
            resources.ApplyResources(this.pnlFooter, "pnlFooter");
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.MouseEnter += new System.EventHandler(this.picGroup_MouseEnter);
            this.pnlFooter.MouseLeave += new System.EventHandler(this.picGroup_MouseLeave);
            // 
            // pnlSplit
            // 
            this.pnlSplit.BackColor = System.Drawing.Color.LightGray;
            resources.ApplyResources(this.pnlSplit, "pnlSplit");
            this.pnlSplit.Name = "pnlSplit";
            // 
            // GroupUnitTool
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.pnlGroup);
            this.Controls.Add(this.pnlFooter);
            this.Name = "GroupUnitTool";
            this.pnlGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picKey)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label lblGroup;
        public System.Windows.Forms.PictureBox picKey;
        private System.Windows.Forms.Panel pnlGroup;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Panel pnlSplit;
        public System.Windows.Forms.PictureBox picGroup;


    }
}
