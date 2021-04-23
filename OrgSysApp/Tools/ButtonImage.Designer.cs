
namespace OrgSysApp.Tools
{
    partial class ButtonImage
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
            this.lblActive = new System.Windows.Forms.Label();
            this.imgImage = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imgImage)).BeginInit();
            this.SuspendLayout();
            // 
            // lblActive
            // 
            this.lblActive.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblActive.Location = new System.Drawing.Point(0, 97);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(100, 3);
            this.lblActive.TabIndex = 0;
            this.lblActive.Click += new System.EventHandler(this.this_Click);
            this.lblActive.MouseEnter += new System.EventHandler(this.this_MouseEnter);
            this.lblActive.MouseLeave += new System.EventHandler(this.this_MouseLeave);
            // 
            // imgImage
            // 
            this.imgImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.imgImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgImage.Location = new System.Drawing.Point(0, 0);
            this.imgImage.Name = "imgImage";
            this.imgImage.Size = new System.Drawing.Size(100, 72);
            this.imgImage.TabIndex = 1;
            this.imgImage.TabStop = false;
            this.imgImage.Click += new System.EventHandler(this.this_Click);
            this.imgImage.MouseEnter += new System.EventHandler(this.this_MouseEnter);
            this.imgImage.MouseLeave += new System.EventHandler(this.this_MouseLeave);
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTitle.Location = new System.Drawing.Point(0, 72);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(100, 25);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Click += new System.EventHandler(this.this_Click);
            this.lblTitle.MouseEnter += new System.EventHandler(this.this_MouseEnter);
            this.lblTitle.MouseLeave += new System.EventHandler(this.this_MouseLeave);
            // 
            // ButtonImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.imgImage);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblActive);
            this.Name = "ButtonImage";
            this.Size = new System.Drawing.Size(100, 100);
            ((System.ComponentModel.ISupportInitialize)(this.imgImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.PictureBox imgImage;
        private System.Windows.Forms.Label lblTitle;
    }
}
