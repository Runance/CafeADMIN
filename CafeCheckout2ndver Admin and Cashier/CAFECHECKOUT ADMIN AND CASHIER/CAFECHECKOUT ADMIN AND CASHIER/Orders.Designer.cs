namespace CAFECHECKOUT_ADMIN_AND_CASHIER
{
    partial class Orders
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Orders));
            this.label7 = new System.Windows.Forms.Label();
            this.Back_but = new Guna.UI2.WinForms.Guna2Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Rockwell", 36F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(454, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(203, 53);
            this.label7.TabIndex = 5;
            this.label7.Text = "ORDERS";
            // 
            // Back_but
            // 
            this.Back_but.Animated = true;
            this.Back_but.AnimatedGIF = true;
            this.Back_but.AutoRoundedCorners = true;
            this.Back_but.BackColor = System.Drawing.Color.Transparent;
            this.Back_but.BorderRadius = 21;
            this.Back_but.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.Back_but.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.Back_but.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.Back_but.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.Back_but.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.Back_but.Font = new System.Drawing.Font("Rockwell", 20.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Back_but.ForeColor = System.Drawing.Color.White;
            this.Back_but.Location = new System.Drawing.Point(12, 584);
            this.Back_but.Name = "Back_but";
            this.Back_but.Size = new System.Drawing.Size(180, 45);
            this.Back_but.TabIndex = 6;
            this.Back_but.Text = "Back";
            this.Back_but.Click += new System.EventHandler(this.Back_but_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(75, 95);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(966, 442);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // Orders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1117, 641);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.Back_but);
            this.Controls.Add(this.label7);
            this.DoubleBuffered = true;
            this.Name = "Orders";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Orders";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private Guna.UI2.WinForms.Guna2Button Back_but;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}