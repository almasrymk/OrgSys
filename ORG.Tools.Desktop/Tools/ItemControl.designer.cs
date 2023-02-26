namespace ORG.Tools.Desktop
{
    partial class ItemControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
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
            this.g = new System.Windows.Forms.GroupBox();
            this.pnlItem = new System.Windows.Forms.Panel();
            this.p1 = new System.Windows.Forms.Panel();
            this.lblItemName = new System.Windows.Forms.Label();
            this.lblItemPrice = new System.Windows.Forms.Label();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.pnlItem.SuspendLayout();
            this.p1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // g
            // 
            this.g.BackColor = System.Drawing.Color.DimGray;
            this.g.Location = new System.Drawing.Point(6, 138);
            this.g.Name = "g";
            this.g.Size = new System.Drawing.Size(98, 1);
            this.g.TabIndex = 8;
            this.g.TabStop = false;
            // 
            // pnlItem
            // 
            this.pnlItem.BackColor = System.Drawing.Color.White;
            this.pnlItem.Controls.Add(this.p1);
            this.pnlItem.Controls.Add(this.picImage);
            this.pnlItem.Location = new System.Drawing.Point(3, 3);
            this.pnlItem.Name = "pnlItem";
            this.pnlItem.Size = new System.Drawing.Size(104, 194);
            this.pnlItem.TabIndex = 10;
            this.pnlItem.Click += new System.EventHandler(this.Select_Click);
            // 
            // p1
            // 
            this.p1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.p1.Controls.Add(this.lblItemName);
            this.p1.Controls.Add(this.lblItemPrice);
            this.p1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.p1.Location = new System.Drawing.Point(0, 137);
            this.p1.Name = "p1";
            this.p1.Size = new System.Drawing.Size(104, 57);
            this.p1.TabIndex = 10;
            // 
            // lblItemName
            // 
            this.lblItemName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblItemName.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.lblItemName.Location = new System.Drawing.Point(0, 0);
            this.lblItemName.Name = "lblItemName";
            this.lblItemName.Size = new System.Drawing.Size(104, 37);
            this.lblItemName.TabIndex = 0;
            this.lblItemName.Text = "Item Name";
            this.lblItemName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblItemName.Click += new System.EventHandler(this.Select_Click);
            this.lblItemName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picImage_MouseDown);
            this.lblItemName.MouseLeave += new System.EventHandler(this.picImage_MouseLeave);
            this.lblItemName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picImage_MouseUp);
            // 
            // lblItemPrice
            // 
            this.lblItemPrice.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblItemPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.lblItemPrice.Location = new System.Drawing.Point(0, 37);
            this.lblItemPrice.Name = "lblItemPrice";
            this.lblItemPrice.Size = new System.Drawing.Size(104, 20);
            this.lblItemPrice.TabIndex = 1;
            this.lblItemPrice.Text = "0.00";
            this.lblItemPrice.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblItemPrice.Click += new System.EventHandler(this.Select_Click);
            this.lblItemPrice.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picImage_MouseDown);
            this.lblItemPrice.MouseLeave += new System.EventHandler(this.picImage_MouseLeave);
            this.lblItemPrice.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picImage_MouseUp);
            // 
            // picImage
            // 
            this.picImage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.picImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picImage.Location = new System.Drawing.Point(6, 5);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(92, 126);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImage.TabIndex = 0;
            this.picImage.TabStop = false;
            this.picImage.Click += new System.EventHandler(this.Select_Click);
            this.picImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picImage_MouseDown);
            this.picImage.MouseLeave += new System.EventHandler(this.picImage_MouseLeave);
            this.picImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picImage_MouseUp);
            // 
            // ItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.Controls.Add(this.g);
            this.Controls.Add(this.pnlItem);
            this.Name = "ItemControl";
            this.Size = new System.Drawing.Size(110, 200);
            this.pnlItem.ResumeLayout(false);
            this.p1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.GroupBox g;
        private System.Windows.Forms.Panel pnlItem;
        private System.Windows.Forms.Panel p1;
        private System.Windows.Forms.Label lblItemName;
        private System.Windows.Forms.Label lblItemPrice;
    }
}
