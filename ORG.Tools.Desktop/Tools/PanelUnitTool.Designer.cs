namespace ORG.Tools.Desktop.Tools
{
    partial class PanelUnitTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelUnitTool));
            this.pnlProgress = new System.Windows.Forms.Panel();
            this.lblProgress = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPagging = new System.Windows.Forms.Label();
            this.pnlProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlProgress
            // 
            resources.ApplyResources(this.pnlProgress, "pnlProgress");
            this.pnlProgress.Controls.Add(this.lblProgress);
            this.pnlProgress.Name = "pnlProgress";
            // 
            // lblProgress
            // 
            this.lblProgress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(175)))), ((int)(((byte)(240)))));
            resources.ApplyResources(this.lblProgress, "lblProgress");
            this.lblProgress.Name = "lblProgress";
            // 
            // pnlMain
            // 
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Name = "pnlMain";
            // 
            // lblPagging
            // 
            this.lblPagging.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPagging, "lblPagging");
            this.lblPagging.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.lblPagging.Name = "lblPagging";
            this.lblPagging.Click += new System.EventHandler(this.lblPagging_Click);
            // 
            // PanelUnitTool
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.lblPagging);
            this.Controls.Add(this.pnlProgress);
            this.Name = "PanelUnitTool";
            this.pnlProgress.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.Label lblProgress;
        public System.Windows.Forms.FlowLayoutPanel pnlMain;
        private System.Windows.Forms.Label lblPagging;
    }
}
