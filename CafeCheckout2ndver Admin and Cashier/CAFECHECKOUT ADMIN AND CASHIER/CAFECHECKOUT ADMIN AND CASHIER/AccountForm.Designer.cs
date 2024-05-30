namespace CAFECHECKOUT_ADMIN_AND_CASHIER
{
    partial class AccountForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param ="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountForm));
            this.Back_but = new Guna.UI2.WinForms.Guna2Button();
            this.NextButt = new Guna.UI2.WinForms.Guna2Button();
            this.label7 = new System.Windows.Forms.Label();
            this.AccountDetails = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.AccountPicture = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ManageAccButt = new Guna.UI2.WinForms.Guna2Button();
            this.PreviousButt = new Guna.UI2.WinForms.Guna2Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AccountPicture)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
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
            // NextButt
            // 
            this.NextButt.Animated = true;
            this.NextButt.AnimatedGIF = true;
            this.NextButt.AutoRoundedCorners = true;
            this.NextButt.BackColor = System.Drawing.Color.Transparent;
            this.NextButt.BorderRadius = 21;
            this.NextButt.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.NextButt.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.NextButt.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.NextButt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.NextButt.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.NextButt.Font = new System.Drawing.Font("Rockwell", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextButt.ForeColor = System.Drawing.Color.White;
            this.NextButt.Location = new System.Drawing.Point(155, 15);
            this.NextButt.Name = "NextButt";
            this.NextButt.Size = new System.Drawing.Size(95, 45);
            this.NextButt.TabIndex = 7;
            this.NextButt.Text = "Next";
            this.NextButt.Click += new System.EventHandler(this.NextButt_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Rockwell", 36F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(413, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(278, 53);
            this.label7.TabIndex = 8;
            this.label7.Text = "ACCOUNTS";
            // 
            // AccountDetails
            // 
            this.AccountDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountDetails.AutoScroll = true;
            this.AccountDetails.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.AccountDetails.Location = new System.Drawing.Point(377, 82);
            this.AccountDetails.Name = "AccountDetails";
            this.AccountDetails.Size = new System.Drawing.Size(550, 365);
            this.AccountDetails.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.AccountPicture);
            this.panel1.Location = new System.Drawing.Point(171, 82);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 221);
            this.panel1.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Rockwell", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Employee Image";
            // 
            // AccountPicture
            // 
            this.AccountPicture.BackColor = System.Drawing.Color.White;
            this.AccountPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AccountPicture.Location = new System.Drawing.Point(18, 19);
            this.AccountPicture.Name = "AccountPicture";
            this.AccountPicture.Size = new System.Drawing.Size(165, 162);
            this.AccountPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.AccountPicture.TabIndex = 0;
            this.AccountPicture.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ManageAccButt);
            this.panel2.Controls.Add(this.PreviousButt);
            this.panel2.Controls.Add(this.NextButt);
            this.panel2.Location = new System.Drawing.Point(377, 453);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(550, 79);
            this.panel2.TabIndex = 11;
            // 
            // ManageAccButt
            // 
            this.ManageAccButt.Animated = true;
            this.ManageAccButt.AnimatedGIF = true;
            this.ManageAccButt.AutoRoundedCorners = true;
            this.ManageAccButt.BackColor = System.Drawing.Color.Transparent;
            this.ManageAccButt.BorderRadius = 21;
            this.ManageAccButt.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.ManageAccButt.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.ManageAccButt.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.ManageAccButt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.ManageAccButt.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.ManageAccButt.Font = new System.Drawing.Font("Rockwell", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ManageAccButt.ForeColor = System.Drawing.Color.White;
            this.ManageAccButt.Location = new System.Drawing.Point(326, 15);
            this.ManageAccButt.Name = "ManageAccButt";
            this.ManageAccButt.Size = new System.Drawing.Size(205, 45);
            this.ManageAccButt.TabIndex = 9;
            this.ManageAccButt.Text = "Manage Accounts";
            this.ManageAccButt.Click += new System.EventHandler(this.ManageAccButt_Click);
            // 
            // PreviousButt
            // 
            this.PreviousButt.Animated = true;
            this.PreviousButt.AnimatedGIF = true;
            this.PreviousButt.AutoRoundedCorners = true;
            this.PreviousButt.BackColor = System.Drawing.Color.Transparent;
            this.PreviousButt.BorderRadius = 21;
            this.PreviousButt.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.PreviousButt.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.PreviousButt.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.PreviousButt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.PreviousButt.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.PreviousButt.Font = new System.Drawing.Font("Rockwell", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreviousButt.ForeColor = System.Drawing.Color.White;
            this.PreviousButt.Location = new System.Drawing.Point(14, 15);
            this.PreviousButt.Name = "PreviousButt";
            this.PreviousButt.Size = new System.Drawing.Size(120, 45);
            this.PreviousButt.TabIndex = 8;
            this.PreviousButt.Text = "Previous";
            this.PreviousButt.Click += new System.EventHandler(this.PreviousButt_Click);
            // 
            // AccountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1117, 641);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AccountDetails);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Back_but);
            this.DoubleBuffered = true;
            this.Name = "AccountForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Account";
            this.Load += new System.EventHandler(this.AccountForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AccountPicture)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button Back_but;
        private Guna.UI2.WinForms.Guna2Button NextButt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.FlowLayoutPanel AccountDetails;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox AccountPicture;
        private System.Windows.Forms.Panel panel2;
        private Guna.UI2.WinForms.Guna2Button PreviousButt;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button ManageAccButt;
    }
}