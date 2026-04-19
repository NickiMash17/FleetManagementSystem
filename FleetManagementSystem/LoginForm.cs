﻿// ═══════════════════════════════════════════════════════════════════════════════
//  Fleet Monitoring & Vehicle Management System — Login Form
//  Apex Auto Solutions | SPM622 FA1
//  Developer: Nicolette Mashaba | Student No: 200232990
// ═══════════════════════════════════════════════════════════════════════════════

using System;
using System.Drawing;
using System.Windows.Forms;

namespace FleetManagementSystem
{
    public class LoginForm : Form
    {
        private TextBox txtUsername = null!, txtPassword = null!;
        private Button btnLogin = null!;
        private Label lblError = null!;

        private readonly Color C_TEAL = Color.FromArgb(0, 107, 107);
        private readonly Color C_DARK = Color.FromArgb(26, 26, 46);
        private readonly Color C_GREY = Color.FromArgb(245, 247, 250);
        private readonly Color C_WHITE = Color.White;

        // Default credentials (in production, use hashed passwords & database)
        private readonly string ADMIN_USER = "admin";
        private readonly string ADMIN_PASS = "fleet2026";

        public LoginForm()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Fleet Management System — Login";
            this.Size = new Size(440, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = C_DARK;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Logo / Title
            var pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 140,
                BackColor = C_TEAL
            };

            var lblIcon = new Label
            {
                Text = "⚙",
                Font = new Font("Segoe UI", 40f),
                ForeColor = Color.White,
                Location = new Point(0, 15),
                Size = new Size(440, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblTitle = new Label
            {
                Text = "Fleet Management System",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(0, 78),
                Size = new Size(440, 28),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblSub = new Label
            {
                Text = "Apex Auto Solutions",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(200, 255, 255),
                Location = new Point(0, 108),
                Size = new Size(440, 22),
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnlTop.Controls.AddRange(new Control[] { lblIcon, lblTitle, lblSub });
            this.Controls.Add(pnlTop);

            // Login Panel
            var pnl = new Panel
            {
                Location = new Point(60, 160),
                Size = new Size(320, 300),
                BackColor = Color.FromArgb(36, 36, 60),
                Padding = new Padding(20)
            };

            AddLabel(pnl, "Username", 20);
            txtUsername = AddTextBox(pnl, 50, false);

            AddLabel(pnl, "Password", 100);
            txtPassword = AddTextBox(pnl, 130, true);

            lblError = new Label
            {
                Location = new Point(20, 173),
                Size = new Size(280, 22),
                ForeColor = Color.FromArgb(255, 100, 100),
                Font = new Font("Segoe UI", 8.5f),
                Text = ""
            };
            pnl.Controls.Add(lblError);

            btnLogin = new Button
            {
                Location = new Point(20, 200),
                Size = new Size(280, 42),
                Text = "🔐  LOGIN",
                BackColor = C_TEAL,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogin.Click += BtnLogin_Click;

            var lblHint = new Label
            {
                Location = new Point(20, 255),
                Size = new Size(280, 30),
                Text = "Default: admin / fleet2026",
                ForeColor = Color.FromArgb(150, 150, 180),
                Font = new Font("Segoe UI", 8f),
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnl.Controls.AddRange(new Control[] { btnLogin, lblHint });
            this.Controls.Add(pnl);

            // Allow Enter key
            this.AcceptButton = btnLogin;

            // Footer
            var lblFooter = new Label
            {
                Text = "SPM622 | Nicolette Mashaba | 200232990",
                Location = new Point(0, 460),
                Size = new Size(440, 20),
                ForeColor = Color.FromArgb(100, 100, 130),
                Font = new Font("Segoe UI", 7.5f),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblFooter);
        }

        private void AddLabel(Panel parent, string text, int y)
        {
            parent.Controls.Add(new Label
            {
                Text = text,
                Location = new Point(20, y),
                Size = new Size(280, 18),
                ForeColor = Color.FromArgb(180, 180, 210),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold)
            });
        }

        private TextBox AddTextBox(Panel parent, int y, bool password)
        {
            var txt = new TextBox
            {
                Location = new Point(20, y),
                Size = new Size(280, 30),
                Font = new Font("Segoe UI", 10.5f),
                BackColor = Color.FromArgb(50, 50, 75),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            if (password) txt.PasswordChar = '•';
            parent.Controls.Add(txt);
            return txt;
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim().ToLower();
            string pass = txtPassword.Text;

            if (user == ADMIN_USER && pass == ADMIN_PASS)
            {
                this.Hide();
                var main = new MainForm();
                main.FormClosed += (s, args) => this.Close();
                main.Show();
            }
            else
            {
                lblError.Text = "⚠ Invalid username or password. Try again.";
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
    }
}