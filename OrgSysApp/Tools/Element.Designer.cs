
namespace OrgSysApp.Tools
{
    partial class Element
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
            this.imgImage = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imgImage)).BeginInit();
            this.SuspendLayout();
            // 
            // imgImage
            // 
            this.imgImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgImage.Location = new System.Drawing.Point(0, 0);
            this.imgImage.Name = "imgImage";
            this.imgImage.Size = new System.Drawing.Size(100, 95);
            this.imgImage.TabIndex = 4;
            this.imgImage.TabStop = false;
            this.imgImage.Click += new System.EventHandler(this.this_Click);
            this.imgImage.MouseEnter += new System.EventHandler(this.this_MouseEnter);
            this.imgImage.MouseLeave += new System.EventHandler(this.this_MouseLeave);
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTitle.Location = new System.Drawing.Point(0, 95);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(100, 30);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Click += new System.EventHandler(this.this_Click);
            this.lblTitle.MouseEnter += new System.EventHandler(this.this_MouseEnter);
            this.lblTitle.MouseLeave += new System.EventHandler(this.this_MouseLeave);
            // 
            // lblDescription
            // 
            this.lblDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblDescription.Location = new System.Drawing.Point(0, 125);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(100, 25);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Click += new System.EventHandler(this.this_Click);
            this.lblDescription.MouseEnter += new System.EventHandler(this.this_MouseEnter);
            this.lblDescription.MouseLeave += new System.EventHandler(this.this_MouseLeave);
            // 
            // Element
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.imgImage);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblDescription);
            this.Name = "Element";
            this.Size = new System.Drawing.Size(100, 150);
            ((System.ComponentModel.ISupportInitialize)(this.imgImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imgImage;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDescription;
    }
}
