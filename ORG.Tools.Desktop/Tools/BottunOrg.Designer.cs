namespace ORG.Tools.Desktop.Tools
{
    partial class BottunOrg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BottunOrg));
            this.Splitter = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pic = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            this.SuspendLayout();
            // 
            // Splitter
            // 
            this.Splitter.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.Splitter, "Splitter");
            this.Splitter.Name = "Splitter";
            this.Splitter.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            this.label1.Click += new System.EventHandler(this.This_Click);
            this.label1.MouseEnter += new System.EventHandler(this.MouseEnter);
            this.label1.MouseLeave += new System.EventHandler(this.MouseLeave);
            // 
            // pic
            // 
            resources.ApplyResources(this.pic, "pic");
            this.pic.Name = "pic";
            this.pic.TabStop = false;
            this.pic.Tag = "";
            this.pic.Click += new System.EventHandler(this.This_Click);
            this.pic.MouseEnter += new System.EventHandler(this.MouseEnter);
            this.pic.MouseLeave += new System.EventHandler(this.MouseLeave);
            // 
            // BottunOrg
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.pic);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Splitter);
            this.Name = "BottunOrg";
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox pic;
        public System.Windows.Forms.GroupBox Splitter;
        private System.Windows.Forms.Label label1;
    }
}
