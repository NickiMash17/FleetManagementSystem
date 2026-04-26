﻿// =============================================================================
//  Fleet Monitoring & Vehicle Management System  -  Login Form
//  Apex Auto Solutions  |  SPM622 Formative Assessment 1
//  Developer : Nicolette Mashaba  |  Student No: 20232990
//  Date      : 28 April 2026
// =============================================================================

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FleetManagementSystem
{
    public class LoginForm : Form
    {
        private TextBox txtUsername = null!, txtPassword = null!;
        private Button btnLogin = null!;
        private Label lblError = null!;
        private Panel linUsername = null!, linPassword = null!;

        // Match MainForm colour palette exactly
        private readonly Color C_BG = Color.FromArgb(8, 14, 26);
        private readonly Color C_PANEL = Color.FromArgb(13, 21, 38);
        private readonly Color C_PANEL2 = Color.FromArgb(18, 29, 52);
        private readonly Color C_PANEL3 = Color.FromArgb(22, 36, 62);
        private readonly Color C_TEAL = Color.FromArgb(0, 188, 172);
        private readonly Color C_TEAL2 = Color.FromArgb(0, 120, 110);
        private readonly Color C_BORDER = Color.FromArgb(28, 50, 72);
        private readonly Color C_INPUT = Color.FromArgb(10, 18, 34);
        private readonly Color C_TEXT = Color.FromArgb(205, 225, 232);
        private readonly Color C_MUTED = Color.FromArgb(90, 130, 150);
        private readonly Color C_RED = Color.FromArgb(220, 65, 65);
        private readonly Color C_WHITE = Color.White;

        private readonly string ADMIN_USER = "admin";
        private readonly string ADMIN_PASS = "fleet2026";

        public LoginForm()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            // ── Form setup ─────────────────────────────────────────────────────
            this.Text = "Fleet Management System  -  Login";
            this.Size = new Size(520, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = C_BG;
            this.FormBorderStyle = FormBorderStyle.None;  // Borderless — more modern
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Allow dragging the borderless form
            bool dragging = false;
            Point dragStart = Point.Empty;
            this.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { dragging = true; dragStart = e.Location; } };
            this.MouseMove += (s, e) => { if (dragging) this.Location = new Point(this.Left + e.X - dragStart.X, this.Top + e.Y - dragStart.Y); };
            this.MouseUp += (s, e) => dragging = false;

            // ── Outer border panel (painted) ───────────────────────────────────
            this.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Teal outer glow border
                using var borderPen = new Pen(Color.FromArgb(80, C_TEAL), 1.5f);
                g.DrawRectangle(borderPen, 1, 1, this.Width - 3, this.Height - 3);

                // Very subtle inner border
                using var innerPen = new Pen(C_BORDER, 1f);
                g.DrawRectangle(innerPen, 3, 3, this.Width - 7, this.Height - 7);
            };

            // ── HEADER SECTION ─────────────────────────────────────────────────
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 220,
                BackColor = C_PANEL
            };
            // Painted gradient header
            pnlHeader.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Dark to slightly lighter gradient
                using var grad = new LinearGradientBrush(
                    pnlHeader.ClientRectangle,
                    Color.FromArgb(16, 28, 52),
                    C_PANEL,
                    LinearGradientMode.Vertical);
                g.FillRectangle(grad, pnlHeader.ClientRectangle);

                // Subtle teal glow top-center radial
                using var glowBrush = new PathGradientBrush(new PointF[] {
                    new PointF(260, 0), new PointF(0, 220), new PointF(520, 220)
                });
                glowBrush.CenterColor = Color.FromArgb(40, C_TEAL);
                glowBrush.SurroundColors = new[] { Color.FromArgb(0, C_TEAL) };
                g.FillRectangle(glowBrush, pnlHeader.ClientRectangle);

                // Bottom border line with teal glow
                using var p1 = new Pen(C_BORDER, 1f);
                g.DrawLine(p1, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
                using var p2 = new Pen(Color.FromArgb(60, C_TEAL), 1f);
                g.DrawLine(p2, 0, pnlHeader.Height - 2, pnlHeader.Width, pnlHeader.Height - 2);

                // Gear icon — draw as circle + text
                int cx = pnlHeader.Width / 2;

                // Outer ring
                using var ringPen = new Pen(C_TEAL, 2f);
                g.DrawEllipse(ringPen, cx - 38, 28, 76, 76);

                // Inner circle fill
                using var circleBrush = new SolidBrush(Color.FromArgb(20, C_TEAL));
                g.FillEllipse(circleBrush, cx - 36, 30, 72, 72);

                // Gear symbol
                using var gearFont = new Font("Segoe UI", 32f);
                using var gearBrush = new SolidBrush(C_TEAL);
                var gearRect = new RectangleF(cx - 40, 28, 80, 80);
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString("\u2699", gearFont, gearBrush, gearRect, sf);

                // App name
                using var titleFont = new Font("Segoe UI", 17f, FontStyle.Bold);
                using var titleBrush = new SolidBrush(C_WHITE);
                var titleRect = new RectangleF(20, 116, pnlHeader.Width - 40, 36);
                g.DrawString("Fleet Management System", titleFont, titleBrush, titleRect, sf);

                // Subtitle
                using var subFont = new Font("Segoe UI", 9.5f);
                using var subBrush = new SolidBrush(Color.FromArgb(160, C_TEAL));
                var subRect = new RectangleF(20, 152, pnlHeader.Width - 40, 24);
                g.DrawString("Apex Auto Solutions", subFont, subBrush, subRect, sf);

                // Small tag line
                using var tagFont = new Font("Segoe UI", 8f);
                using var tagBrush = new SolidBrush(C_MUTED);
                var tagRect = new RectangleF(20, 178, pnlHeader.Width - 40, 22);
                g.DrawString("Fleet Monitoring & Vehicle Management  |  SPM622 FA1", tagFont, tagBrush, tagRect, sf);
            };
            pnlHeader.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { dragging = true; dragStart = e.Location; } };
            pnlHeader.MouseMove += (s, e) => { if (dragging) this.Location = new Point(this.Left + e.X - dragStart.X, this.Top + e.Y - dragStart.Y); };
            pnlHeader.MouseUp += (s, e) => dragging = false;
            this.Controls.Add(pnlHeader);

            // ── CLOSE BUTTON (top right) ───────────────────────────────────────
            var btnClose = new Button
            {
                Text = "x",
                Size = new Size(30, 24),
                Location = new Point(this.Width - 36, 6),
                BackColor = Color.Transparent,
                ForeColor = C_MUTED,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = C_RED;
            btnClose.Click += (s, e) => Application.Exit();
            btnClose.MouseEnter += (s, e) => btnClose.ForeColor = C_WHITE;
            btnClose.MouseLeave += (s, e) => btnClose.ForeColor = C_MUTED;
            this.Controls.Add(btnClose);

            // ── FORM BODY ──────────────────────────────────────────────────────
            int fx = 50;   // field x
            int fw = 420;  // field width

            // USERNAME
            var lblUser = MakeFieldLabel("USERNAME", new Point(fx, 238));
            txtUsername = MakeInput(new Point(fx, 258), fw, false, "");
            linUsername = MakeLine(new Point(fx, 285), fw);

            // PASSWORD
            var lblPass = MakeFieldLabel("PASSWORD", new Point(fx, 308));
            txtPassword = MakeInput(new Point(fx, 328), fw, true, "");
            linPassword = MakeLine(new Point(fx, 355), fw);

            // Focus events — animate underline colour
            txtUsername.GotFocus += (s, e) => linUsername.BackColor = C_TEAL;
            txtUsername.LostFocus += (s, e) => linUsername.BackColor = C_BORDER;
            txtPassword.GotFocus += (s, e) => linPassword.BackColor = C_TEAL;
            txtPassword.LostFocus += (s, e) => linPassword.BackColor = C_BORDER;

            // ERROR label
            lblError = new Label
            {
                Location = new Point(fx, 372),
                Size = new Size(fw, 20),
                ForeColor = C_RED,
                Font = new Font("Segoe UI", 8.5f),
                BackColor = Color.Transparent,
                Text = "",
                TextAlign = ContentAlignment.MiddleCenter
            };

            // LOGIN button
            btnLogin = new Button
            {
                Text = "LOGIN",
                Location = new Point(fx, 402),
                Size = new Size(fw, 52),
                BackColor = C_TEAL2,
                ForeColor = C_WHITE,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatAppearance.MouseOverBackColor = C_TEAL;
            btnLogin.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 80, 74);
            btnLogin.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // Gradient fill
                using var grad = new LinearGradientBrush(btnLogin.ClientRectangle,
                    C_TEAL, C_TEAL2, LinearGradientMode.Vertical);
                g.FillRectangle(grad, btnLogin.ClientRectangle);
                // Text
                using var f = new Font("Segoe UI", 12f, FontStyle.Bold);
                using var b = new SolidBrush(C_WHITE);
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString("LOGIN", f, b, btnLogin.ClientRectangle, sf);
            };
            btnLogin.Click += BtnLogin_Click;

            // Hint text
            var lblHint = new Label
            {
                Text = "Default credentials:  admin  /  fleet2026",
                Location = new Point(fx, 466),
                Size = new Size(fw, 18),
                ForeColor = C_MUTED,
                Font = new Font("Segoe UI", 8f),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Footer
            var pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 36,
                BackColor = C_PANEL
            };
            pnlFooter.Paint += (s, e) =>
            {
                using var p = new Pen(C_BORDER, 1f);
                e.Graphics.DrawLine(p, 0, 0, pnlFooter.Width, 0);
                using var f = new Font("Segoe UI", 7.5f);
                using var b = new SolidBrush(C_MUTED);
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(
                    "SPM622  |  Nicolette Mashaba  |  200232990  |  CTU Training Solutions",
                    f, b, new RectangleF(0, 0, pnlFooter.Width, pnlFooter.Height), sf);
            };
            pnlFooter.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { dragging = true; dragStart = e.Location; } };
            pnlFooter.MouseMove += (s, e) => { if (dragging) this.Location = new Point(this.Left + e.X - dragStart.X, this.Top + e.Y - dragStart.Y); };
            pnlFooter.MouseUp += (s, e) => dragging = false;

            // Add all controls
            this.Controls.AddRange(new Control[]
            {
                lblUser, txtUsername, linUsername,
                lblPass, txtPassword, linPassword,
                lblError, btnLogin, lblHint, pnlFooter
            });

            this.AcceptButton = btnLogin;
        }

        // ── LOGIN HANDLER ─────────────────────────────────────────────────────
        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim().ToLower();
            string pass = txtPassword.Text ?? "";

            if (user == ADMIN_USER && pass == ADMIN_PASS)
            {
                this.Hide();
                var main = new MainForm();
                main.FormClosed += (s, args) => this.Close();
                main.Show();
            }
            else
            {
                lblError.Text = "Incorrect username or password. Please try again.";
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        // ── UI HELPERS ────────────────────────────────────────────────────────
        private Label MakeFieldLabel(string text, Point loc) => new Label
        {
            Text = text,
            Location = loc,
            Size = new Size(420, 16),
            ForeColor = Color.FromArgb(90, 130, 150),
            Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
            BackColor = Color.Transparent
        };

        private TextBox MakeInput(Point loc, int w, bool password, string hint) => new TextBox
        {
            Location = loc,
            Size = new Size(w, 26),
            BackColor = Color.FromArgb(10, 18, 34),
            ForeColor = Color.FromArgb(205, 225, 232),
            BorderStyle = BorderStyle.None,
            Font = new Font("Segoe UI", 12f),
            PasswordChar = password ? '\u25CF' : '\0',
            MaxLength = 60
        };

        private Panel MakeLine(Point loc, int w) => new Panel
        {
            Location = loc,
            Size = new Size(w, 1),
            BackColor = Color.FromArgb(28, 50, 72)
        };
    }
}