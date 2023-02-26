namespace ORG.Tools.Desktop
{
    partial class RedioTool
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
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.txtLeft = new System.Windows.Forms.Label();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.txtRight = new System.Windows.Forms.Label();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlLeft.Controls.Add(this.txtLeft);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(25, 20);
            this.pnlLeft.TabIndex = 0;
            // 
            // txtLeft
            // 
            this.txtLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLeft.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLeft.ForeColor = System.Drawing.Color.Black;
            this.txtLeft.Location = new System.Drawing.Point(0, 0);
            this.txtLeft.Name = "txtLeft";
            this.txtLeft.Size = new System.Drawing.Size(25, 20);
            this.txtLeft.TabIndex = 3;
            this.txtLeft.Text = "Ok";
            this.txtLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtLeft.Click += new System.EventHandler(this.SelectedClick);
            // 
            // pnlRight
            // 
            this.pnlRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlRight.Controls.Add(this.txtRight);
            this.pnlRight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlRight.Location = new System.Drawing.Point(25, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(25, 20);
            this.pnlRight.TabIndex = 1;
            // 
            // txtRight
            // 
            this.txtRight.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRight.ForeColor = System.Drawing.Color.Red;
            this.txtRight.Location = new System.Drawing.Point(0, 0);
            this.txtRight.Name = "txtRight";
            this.txtRight.Size = new System.Drawing.Size(25, 20);
            this.txtRight.TabIndex = 2;
            this.txtRight.Text = "No";
            this.txtRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtRight.Click += new System.EventHandler(this.UnselectedClick);
            // 
            // RedioTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.MaximumSize = new System.Drawing.Size(50, 20);
            this.MinimumSize = new System.Drawing.Size(50, 20);
            this.Name = "RedioTool";
            this.Size = new System.Drawing.Size(50, 20);
            this.pnlLeft.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label txtLeft;
        private System.Windows.Forms.Label txtRight;
    }
}
