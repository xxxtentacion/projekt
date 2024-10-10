namespace Auth.Interface
{
    partial class User
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
            this.components = new System.ComponentModel.Container();
            this.siticonePanel1 = new Siticone.UI.WinForms.SiticonePanel();
            this.label1 = new System.Windows.Forms.Label();
            this.siticoneButton2 = new Siticone.UI.WinForms.SiticoneButton();
            this.siticoneButton1 = new Siticone.UI.WinForms.SiticoneButton();
            this.buttonPaddle = new Siticone.UI.WinForms.SiticoneRoundedButton();
            this.panelBall = new Siticone.UI.WinForms.SiticonePanel();
            this.timerGame = new System.Windows.Forms.Timer(this.components);
            this.siticoneButton3 = new Siticone.UI.WinForms.SiticoneButton();
            this.siticonePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // siticonePanel1
            // 
            this.siticonePanel1.BackColor = System.Drawing.Color.Crimson;
            this.siticonePanel1.Controls.Add(this.label1);
            this.siticonePanel1.Controls.Add(this.siticoneButton2);
            this.siticonePanel1.Controls.Add(this.siticoneButton1);
            this.siticonePanel1.Location = new System.Drawing.Point(-1, -1);
            this.siticonePanel1.Name = "siticonePanel1";
            this.siticonePanel1.ShadowDecoration.Parent = this.siticonePanel1;
            this.siticonePanel1.Size = new System.Drawing.Size(552, 24);
            this.siticonePanel1.TabIndex = 4;
            this.siticonePanel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.siticonePanel1_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Interface - User";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            // 
            // siticoneButton2
            // 
            this.siticoneButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.siticoneButton2.CheckedState.Parent = this.siticoneButton2;
            this.siticoneButton2.CustomImages.Parent = this.siticoneButton2;
            this.siticoneButton2.FillColor = System.Drawing.Color.Crimson;
            this.siticoneButton2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.siticoneButton2.ForeColor = System.Drawing.Color.White;
            this.siticoneButton2.HoveredState.Parent = this.siticoneButton2;
            this.siticoneButton2.Location = new System.Drawing.Point(494, 0);
            this.siticoneButton2.Name = "siticoneButton2";
            this.siticoneButton2.ShadowDecoration.Parent = this.siticoneButton2;
            this.siticoneButton2.Size = new System.Drawing.Size(22, 23);
            this.siticoneButton2.TabIndex = 6;
            this.siticoneButton2.Text = "_";
            this.siticoneButton2.Click += new System.EventHandler(this.siticoneButton2_Click);
            // 
            // siticoneButton1
            // 
            this.siticoneButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.siticoneButton1.CheckedState.Parent = this.siticoneButton1;
            this.siticoneButton1.CustomImages.Parent = this.siticoneButton1;
            this.siticoneButton1.FillColor = System.Drawing.Color.Crimson;
            this.siticoneButton1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.siticoneButton1.ForeColor = System.Drawing.Color.White;
            this.siticoneButton1.HoveredState.Parent = this.siticoneButton1;
            this.siticoneButton1.Location = new System.Drawing.Point(516, 0);
            this.siticoneButton1.Name = "siticoneButton1";
            this.siticoneButton1.ShadowDecoration.Parent = this.siticoneButton1;
            this.siticoneButton1.Size = new System.Drawing.Size(22, 23);
            this.siticoneButton1.TabIndex = 5;
            this.siticoneButton1.Text = "×";
            this.siticoneButton1.Click += new System.EventHandler(this.siticoneButton1_Click);
            // 
            // buttonPaddle
            // 
            this.buttonPaddle.CheckedState.Parent = this.buttonPaddle;
            this.buttonPaddle.CustomImages.Parent = this.buttonPaddle;
            this.buttonPaddle.FillColor = System.Drawing.Color.Crimson;
            this.buttonPaddle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonPaddle.ForeColor = System.Drawing.Color.White;
            this.buttonPaddle.HoveredState.Parent = this.buttonPaddle;
            this.buttonPaddle.Location = new System.Drawing.Point(130, 420);
            this.buttonPaddle.Name = "buttonPaddle";
            this.buttonPaddle.ShadowDecoration.Parent = this.buttonPaddle;
            this.buttonPaddle.Size = new System.Drawing.Size(268, 10);
            this.buttonPaddle.TabIndex = 6;
            // 
            // panelBall
            // 
            this.panelBall.BackColor = System.Drawing.Color.White;
            this.panelBall.Location = new System.Drawing.Point(254, 395);
            this.panelBall.Name = "panelBall";
            this.panelBall.ShadowDecoration.Parent = this.panelBall;
            this.panelBall.Size = new System.Drawing.Size(20, 20);
            this.panelBall.TabIndex = 7;
            // 
            // timerGame
            // 
            this.timerGame.Interval = 20;
            this.timerGame.Tick += new System.EventHandler(this.timerGame_Tick);
            // 
            // siticoneButton3
            // 
            this.siticoneButton3.CheckedState.Parent = this.siticoneButton3;
            this.siticoneButton3.CustomImages.Parent = this.siticoneButton3;
            this.siticoneButton3.FillColor = System.Drawing.Color.Crimson;
            this.siticoneButton3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.siticoneButton3.ForeColor = System.Drawing.Color.White;
            this.siticoneButton3.HoveredState.Parent = this.siticoneButton3;
            this.siticoneButton3.Location = new System.Drawing.Point(192, 270);
            this.siticoneButton3.Margin = new System.Windows.Forms.Padding(2);
            this.siticoneButton3.Name = "siticoneButton3";
            this.siticoneButton3.ShadowDecoration.Parent = this.siticoneButton3;
            this.siticoneButton3.Size = new System.Drawing.Size(135, 24);
            this.siticoneButton3.TabIndex = 15;
            this.siticoneButton3.Text = "start game";
            this.siticoneButton3.Click += new System.EventHandler(this.siticoneButton3_Click);
            // 
            // User
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.ClientSize = new System.Drawing.Size(538, 442);
            this.Controls.Add(this.siticoneButton3);
            this.Controls.Add(this.panelBall);
            this.Controls.Add(this.buttonPaddle);
            this.Controls.Add(this.siticonePanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "User";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User";
            this.Load += new System.EventHandler(this.User_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.User_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.User_MouseMove);
            this.siticonePanel1.ResumeLayout(false);
            this.siticonePanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Siticone.UI.WinForms.SiticonePanel siticonePanel1;
        private System.Windows.Forms.Label label1;
        private Siticone.UI.WinForms.SiticoneButton siticoneButton2;
        private Siticone.UI.WinForms.SiticoneButton siticoneButton1;
        private Siticone.UI.WinForms.SiticoneRoundedButton buttonPaddle;
        private Siticone.UI.WinForms.SiticonePanel panelBall;
        private System.Windows.Forms.Timer timerGame;
        private Siticone.UI.WinForms.SiticoneButton siticoneButton3;
    }
}