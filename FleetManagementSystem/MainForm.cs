// =============================================================================
//  Fleet Monitoring & Vehicle Management System
//  Apex Auto Solutions  |  SPM622 Formative Assessment 1
//  Developer : Nicolette Mashaba  |  Student No: 200232990
//  Date      : 28 April 2026
//  UI        : Premium dark theme, custom-drawn tabs, animated cards,
//              gradient headers, icon buttons, smooth hover effects
// =============================================================================

using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace FleetManagementSystem
{
    public partial class MainForm : Form
    {
        // ── Data ───────────────────────────────────────────────────────────────
        private readonly DataTable vehicleTable = new DataTable();
        private readonly DataTable driverTable = new DataTable();
        private readonly DataTable usageTable = new DataTable();

        // ── Dashboard stat panels ──────────────────────────────────────────────
        private Panel? pnlCardVehicles, pnlCardDrivers, pnlCardFuel,
                       pnlCardMileage, pnlCardAlerts;
        private Label? lblCardVehicles, lblCardDrivers, lblCardFuel,
                       lblCardMileage, lblCardAlerts;

        // ── Vehicle tab ────────────────────────────────────────────────────────
        private TextBox? txtVehicleID, txtModel;
        private ComboBox? cmbVehicleType, cmbAssignedDriver;
        private DataGridView? dgvVehicles;

        // ── Driver tab ─────────────────────────────────────────────────────────
        private TextBox? txtDriverID, txtDriverName, txtLicence, txtContact;
        private DataGridView? dgvDrivers;

        // ── Data Capture tab ───────────────────────────────────────────────────
        private ComboBox? cmbVehicleCapture;
        private TextBox? txtFuel, txtMileage, txtSpeed, txtNotes;
        private DataGridView? dgvUsage;
        private Label? lblSpeedWarning;

        // ── Reports tab ────────────────────────────────────────────────────────
        private DataGridView? dgvReport;
        private Label? lblRptVehicles, lblRptFuel, lblRptMileage, lblRptAlerts;
        private Button? btnGenerateReport, btnExportReport, btnPrintReport;
        private ComboBox? cmbFilterVehicle;

        // ── Status bar ─────────────────────────────────────────────────────────
        private Label? lblStatusLeft, lblStatusRight;

        // ── Print ──────────────────────────────────────────────────────────────
        private PrintDocument? printDoc;
        private int printPageIndex = 0;
        private const double SPEED_LIMIT = 120.0;

        // ══════════════════════════════════════════════════════════════════════
        //  COLOUR PALETTE  — Deep navy / teal premium theme
        // ══════════════════════════════════════════════════════════════════════
        private readonly Color C_BG        = Color.FromArgb(8,   14,  26);
        private readonly Color C_PANEL     = Color.FromArgb(13,  21,  38);
        private readonly Color C_PANEL2    = Color.FromArgb(18,  29,  52);
        private readonly Color C_PANEL3    = Color.FromArgb(22,  36,  62);
        private readonly Color C_BORDER    = Color.FromArgb(28,  50,  72);
        private readonly Color C_TEAL      = Color.FromArgb(0,  188, 172);
        private readonly Color C_TEAL2     = Color.FromArgb(0,  120, 110);
        private readonly Color C_TEAL_DIM  = Color.FromArgb(0,   50,  50);
        private readonly Color C_WHITE     = Color.White;
        private readonly Color C_TEXT      = Color.FromArgb(205, 225, 232);
        private readonly Color C_MUTED     = Color.FromArgb(90,  130, 150);
        private readonly Color C_INPUT     = Color.FromArgb(10,  18,  34);
        private readonly Color C_GREEN     = Color.FromArgb(32,  178, 108);
        private readonly Color C_GREEN_DIM = Color.FromArgb(12,  60,  40);
        private readonly Color C_RED       = Color.FromArgb(220, 65,  65);
        private readonly Color C_RED_DIM   = Color.FromArgb(60,  15,  15);
        private readonly Color C_ORANGE    = Color.FromArgb(230, 138, 30);
        private readonly Color C_ORANGE_DIM= Color.FromArgb(60,  38,   8);
        private readonly Color C_PURPLE    = Color.FromArgb(110, 80,  200);
        private readonly Color C_ROW_ALT   = Color.FromArgb(16,  26,  46);
        private readonly Color C_GRID_HDR  = Color.FromArgb(8,   16,  30);
        private readonly Color C_SEL       = Color.FromArgb(0,   60,  70);

        // ══════════════════════════════════════════════════════════════════════
        public MainForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer, true);
            InitialiseDataTables();
            SetupUI();
            LoadSampleData();
            UpdateDashboard();
            UpdateStatus("System ready  |  Logged in as: Administrator");
        }

        // ── Data schemas ───────────────────────────────────────────────────────
        private void InitialiseDataTables()
        {
            vehicleTable.Columns.Add("Vehicle ID",       typeof(string));
            vehicleTable.Columns.Add("Model",            typeof(string));
            vehicleTable.Columns.Add("Type",             typeof(string));
            vehicleTable.Columns.Add("Assigned Driver",  typeof(string));
            vehicleTable.Columns.Add("Status",           typeof(string));
            vehicleTable.Columns.Add("Registered",       typeof(string));

            driverTable.Columns.Add("Driver ID",  typeof(string));
            driverTable.Columns.Add("Full Name",  typeof(string));
            driverTable.Columns.Add("Licence No", typeof(string));
            driverTable.Columns.Add("Contact",    typeof(string));
            driverTable.Columns.Add("Status",     typeof(string));

            usageTable.Columns.Add("Vehicle ID",    typeof(string));
            usageTable.Columns.Add("Date & Time",   typeof(string));
            usageTable.Columns.Add("Fuel Used (L)", typeof(double));
            usageTable.Columns.Add("Mileage (km)",  typeof(double));
            usageTable.Columns.Add("Speed (km/h)",  typeof(double));
            usageTable.Columns.Add("Notes",         typeof(string));
        }

        private void LoadSampleData()
        {
            driverTable.Rows.Add("DRV001", "Sipho Dlamini",   "GP123456", "071 234 5678", "Active");
            driverTable.Rows.Add("DRV002", "Thandeka Nkosi",  "WC987654", "082 345 6789", "Active");
            driverTable.Rows.Add("DRV003", "Priya Reddy",     "GP555444", "063 456 7890", "Active");

            vehicleTable.Rows.Add("AAS001", "Toyota HiAce",      "Van",    "Sipho Dlamini",  "Active", DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
            vehicleTable.Rows.Add("AAS002", "Ford Ranger",        "Bakkie", "Thandeka Nkosi", "Active", DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
            vehicleTable.Rows.Add("AAS003", "Mercedes Sprinter",  "Truck",  "Priya Reddy",    "Active", DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd"));

            usageTable.Rows.Add("AAS001", DateTime.Now.AddHours(-5).ToString("yyyy-MM-dd HH:mm"),    45.5,  320.0,  90.0, "Morning delivery");
            usageTable.Rows.Add("AAS002", DateTime.Now.AddHours(-3).ToString("yyyy-MM-dd HH:mm"),    60.2,  480.5, 105.0, "Highway run");
            usageTable.Rows.Add("AAS001", DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm"),    30.1,  210.0,  75.0, "Afternoon route");
            usageTable.Rows.Add("AAS003", DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm"), 85.0,  610.0, 125.0, "Long haul - SPEED ALERT");
        }

        // ══════════════════════════════════════════════════════════════════════
        //  MASTER UI BUILD
        // ══════════════════════════════════════════════════════════════════════
        private void SetupUI()
        {
            this.Text          = "Fleet Monitoring & Vehicle Management System  |  Apex Auto Solutions";
            this.Size          = new Size(1320, 840);
            this.MinimumSize   = new Size(1100, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor     = C_BG;
            this.Font          = new Font("Segoe UI", 9f);
            this.FormClosing  += MainForm_FormClosing;
            this.Icon          = CreateAppIcon();

            BuildHeader();
            BuildStatusBar();
            BuildTabControl();
        }

        // ── HEADER ────────────────────────────────────────────────────────────
        private void BuildHeader()
        {
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 68, BackColor = C_PANEL };

            pnlHeader.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using var grad = new LinearGradientBrush(
                    pnlHeader.ClientRectangle,
                    Color.FromArgb(20, 32, 56), C_PANEL,
                    LinearGradientMode.Horizontal);
                g.FillRectangle(grad, pnlHeader.ClientRectangle);

                using var accentBrush = new LinearGradientBrush(
                    new Rectangle(0, 0, 5, pnlHeader.Height),
                    C_TEAL, C_TEAL2, LinearGradientMode.Vertical);
                g.FillRectangle(accentBrush, 0, 0, 5, pnlHeader.Height);

                using var borderPen = new Pen(C_BORDER, 1f);
                g.DrawLine(borderPen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);

                using var glowPen = new Pen(Color.FromArgb(60, C_TEAL), 1f);
                g.DrawLine(glowPen, 0, pnlHeader.Height - 2, pnlHeader.Width, pnlHeader.Height - 2);
            };

            var pnlIcon = new Panel { Location = new Point(16, 12), Size = new Size(44, 44), BackColor = C_TEAL_DIM };
            pnlIcon.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var b = new SolidBrush(C_TEAL_DIM);
                g.FillEllipse(b, 0, 0, 43, 43);
                using var p = new Pen(C_TEAL, 1.5f);
                g.DrawEllipse(p, 1, 1, 41, 41);
                using var tf = new Font("Segoe UI", 16f);
                using var tb = new SolidBrush(C_TEAL);
                g.DrawString("⚙", tf, tb,
                    new RectangleF(0, 2, 44, 40),
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            };

            var lblName = new Label
            {
                Text      = "Fleet Monitoring & Vehicle Management System",
                Location  = new Point(70, 10),
                Size      = new Size(560, 24),
                ForeColor = C_WHITE,
                Font      = new Font("Segoe UI", 12.5f, FontStyle.Bold),
                BackColor = Color.Transparent
            };
            var lblSub = new Label
            {
                Text      = "Apex Auto Solutions  |  Nicolette Mashaba  |  200232990",
                Location  = new Point(70, 36),
                Size      = new Size(560, 18),
                ForeColor = C_MUTED,
                Font      = new Font("Segoe UI", 8.5f),
                BackColor = Color.Transparent
            };
            var lblDate = new Label
            {
                Text      = DateTime.Now.ToString("dd MMM yyyy"),
                Location  = new Point(950, 22),
                Size      = new Size(110, 24),
                ForeColor = C_MUTED,
                Font      = new Font("Segoe UI", 8.5f),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            var btnLogout = MakeIconBtn("Logout", C_TEAL2, new Point(1078, 16), 100);
            var btnExit   = MakeIconBtn("Exit",   C_RED,   new Point(1188, 16),  90);
            btnLogout.Click += BtnLogout_Click;
            btnExit.Click   += BtnExit_Click;

            pnlHeader.Controls.AddRange(new Control[]
                { pnlIcon, lblName, lblSub, lblDate, btnLogout, btnExit });
            this.Controls.Add(pnlHeader);
        }

        // ── STATUS BAR ────────────────────────────────────────────────────────
        private void BuildStatusBar()
        {
            var pnlStatus = new Panel { Dock = DockStyle.Bottom, Height = 26, BackColor = C_PANEL };
            pnlStatus.Paint += (s, e) => {
                using var p = new Pen(C_BORDER, 1f);
                e.Graphics.DrawLine(p, 0, 0, pnlStatus.Width, 0);
                using var b = new SolidBrush(C_TEAL);
                e.Graphics.FillEllipse(b, 8, 8, 8, 8);
            };

            lblStatusLeft = new Label
            {
                Text      = "Ready",
                Location  = new Point(24, 4),
                Size      = new Size(700, 18),
                ForeColor = C_MUTED,
                Font      = new Font("Segoe UI", 7.5f),
                BackColor = Color.Transparent
            };
            lblStatusRight = new Label
            {
                Text      = DateTime.Now.ToString("dd MMM yyyy  HH:mm:ss"),
                Dock      = DockStyle.Right,
                Width     = 200,
                ForeColor = C_MUTED,
                Font      = new Font("Segoe UI", 7.5f),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent,
                Padding   = new Padding(0, 0, 12, 0)
            };

            var timer = new System.Windows.Forms.Timer { Interval = 1000 };
            timer.Tick += (s, e) =>
                lblStatusRight.Text = DateTime.Now.ToString("dd MMM yyyy  HH:mm:ss");
            timer.Start();

            pnlStatus.Controls.AddRange(new Control[] { lblStatusLeft, lblStatusRight });
            this.Controls.Add(pnlStatus);
        }

        // ── TAB CONTROL ───────────────────────────────────────────────────────
        private void BuildTabControl()
        {
            var tabMain = new TabControl
            {
                Dock      = DockStyle.Fill,
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                Padding   = new Point(22, 9),
                BackColor = C_BG
            };
            tabMain.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabMain.DrawItem += TabMain_DrawItem;
            tabMain.SelectedIndexChanged += (s, e) => {
                UpdateDashboard();
                BtnGenerateReport_Click(this, EventArgs.Empty);
            };

            tabMain.TabPages.Add(BuildDashboardTab());
            tabMain.TabPages.Add(BuildVehicleTab());
            tabMain.TabPages.Add(BuildDriverTab());
            tabMain.TabPages.Add(BuildDataCaptureTab());
            tabMain.TabPages.Add(BuildReportsTab());

            this.Controls.Add(tabMain);
            tabMain.BringToFront();
        }

        // ── CUSTOM TAB DRAWING ────────────────────────────────────────────────
        private void TabMain_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is not TabControl tab) return;
            var rect = tab.GetTabRect(e.Index);
            bool sel = e.Index == tab.SelectedIndex;

            string[] icons = { "  Dashboard", "  Vehicles", "  Drivers", "  Data Capture", "  Reports" };
            string label   = e.Index < icons.Length ? icons[e.Index] : tab.TabPages[e.Index].Text;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (sel)
            {
                using var bg = new LinearGradientBrush(rect, C_PANEL3, C_PANEL, LinearGradientMode.Vertical);
                g.FillRectangle(bg, rect);
                using var accent = new LinearGradientBrush(
                    new Rectangle(rect.X, rect.Y, rect.Width, 3),
                    C_TEAL, C_TEAL2, LinearGradientMode.Horizontal);
                g.FillRectangle(accent, rect.X, rect.Y, rect.Width, 3);
            }
            else
            {
                using var bg = new SolidBrush(C_PANEL);
                g.FillRectangle(bg, rect);
                using var line = new Pen(Color.FromArgb(30, C_TEAL), 1f);
                g.DrawLine(line, rect.X, rect.Bottom - 1, rect.Right, rect.Bottom - 1);
            }

            using var txt = new SolidBrush(sel ? C_WHITE : C_MUTED);
            using var fnt = new Font("Segoe UI", 8.5f, sel ? FontStyle.Bold : FontStyle.Regular);
            var sf = new StringFormat
            {
                Alignment     = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString(label, fnt, txt, rect, sf);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  TAB BUILDERS
        // ══════════════════════════════════════════════════════════════════════

        // ── DASHBOARD ─────────────────────────────────────────────────────────
        private TabPage BuildDashboardTab()
        {
            var tab = MakeTab("Dashboard");

            var pnlWelcome = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.Transparent };
            pnlWelcome.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var grad = new LinearGradientBrush(pnlWelcome.ClientRectangle,
                    Color.FromArgb(16, 28, 50), C_BG, LinearGradientMode.Vertical);
                g.FillRectangle(grad, pnlWelcome.ClientRectangle);
                using var b = new SolidBrush(C_WHITE);
                using var f = new Font("Segoe UI", 13f, FontStyle.Bold);
                g.DrawString("Fleet Performance Dashboard", f, b, new Point(18, 8));
                using var b2 = new SolidBrush(C_MUTED);
                using var f2 = new Font("Segoe UI", 8.5f);
                g.DrawString($"Overview as of {DateTime.Now:dd MMM yyyy  HH:mm}", f2, b2, new Point(20, 34));
            };
            tab.Controls.Add(pnlWelcome);

            var pnlCards = new Panel
            {
                Dock        = DockStyle.Top,
                Height      = 118,
                BackColor   = Color.Transparent,
                Padding     = new Padding(14, 10, 14, 6)
            };

            (pnlCardVehicles, lblCardVehicles) = MakeStatCard("TOTAL VEHICLES",     "0",    C_TEAL,   C_TEAL_DIM,                    0);
            (pnlCardDrivers,  lblCardDrivers)  = MakeStatCard("REGISTERED DRIVERS", "0",    C_PURPLE, Color.FromArgb(28, 18, 58),     1);
            (pnlCardFuel,     lblCardFuel)     = MakeStatCard("FUEL USED (L)",      "0.00", C_GREEN,  C_GREEN_DIM,                   2);
            (pnlCardMileage,  lblCardMileage)  = MakeStatCard("FLEET MILEAGE",      "0.00", C_ORANGE, C_ORANGE_DIM,                  3);
            (pnlCardAlerts,   lblCardAlerts)   = MakeStatCard("SPEED ALERTS",       "0",    C_RED,    C_RED_DIM,                     4);

            pnlCards.Controls.AddRange(new Control[]
                { pnlCardVehicles, pnlCardDrivers, pnlCardFuel, pnlCardMileage, pnlCardAlerts });
            tab.Controls.Add(pnlCards);

            tab.Controls.Add(MakeSectionBand("RECENT VEHICLE USAGE"));

            var dgv = StyledGrid();
            dgv.Dock             = DockStyle.Fill;
            dgv.DataSource       = usageTable;
            dgv.CellFormatting  += Dgv_CellFormatting;
            tab.Controls.Add(dgv);

            tab.Controls.SetChildIndex(dgv,                                    0);
            tab.Controls.SetChildIndex(tab.Controls[tab.Controls.Count - 2],   1);
            tab.Controls.SetChildIndex(pnlCards,                               2);
            tab.Controls.SetChildIndex(pnlWelcome,                             3);
            return tab;
        }

        // ── VEHICLES ──────────────────────────────────────────────────────────
        private TabPage BuildVehicleTab()
        {
            var tab   = MakeTab("Vehicles");
            var split = MakeSplit(370);

            var form = MakeFormPanel("REGISTER NEW VEHICLE");
            int y = 54;
            txtVehicleID      = AddField(form, "Vehicle ID",   ref y, 16, 320, "e.g. AAS004");
            txtModel          = AddField(form, "Model",        ref y, 16, 320, "e.g. Toyota HiAce");
            cmbVehicleType    = AddCombo(form, "Vehicle Type", ref y, 16, 320,
                new[] { "Van", "Bakkie", "Truck", "Car", "Bus", "Motorcycle" });
            cmbAssignedDriver = AddCombo(form, "Assign Driver", ref y, 16, 320,
                new[] { "Unassigned" });
            RefreshDriverCombo();

            y += 8;
            var btnAdd    = AddFormBtn(form, "Add Vehicle",      C_TEAL,   y); y += 52;
            var btnClear  = AddFormBtn(form, "Clear Fields",     C_PANEL3, y); y += 52;
            var btnRemove = AddFormBtn(form, "Remove Selected",  C_RED,    y);
            btnAdd.Click    += BtnAddVehicle_Click;
            btnClear.Click  += (s, e) => ClearVehicle();
            btnRemove.Click += BtnRemoveVehicle_Click;
            split.Panel1.Controls.Add(form);

            var right = MakeGridPanel("REGISTERED VEHICLES");
            dgvVehicles            = StyledGrid();
            dgvVehicles.Dock       = DockStyle.Fill;
            dgvVehicles.DataSource = vehicleTable;
            right.Controls.Add(dgvVehicles);
            split.Panel2.Controls.Add(right);

            tab.Controls.Add(split);
            return tab;
        }

        // ── DRIVERS ───────────────────────────────────────────────────────────
        private TabPage BuildDriverTab()
        {
            var tab   = MakeTab("Drivers");
            var split = MakeSplit(370);

            var form = MakeFormPanel("REGISTER NEW DRIVER");
            int y = 54;
            txtDriverID   = AddField(form, "Driver ID",  ref y, 16, 320, "e.g. DRV004");
            txtDriverName = AddField(form, "Full Name",  ref y, 16, 320, "e.g. Thabo Molefe");
            txtLicence    = AddField(form, "Licence No", ref y, 16, 320, "e.g. GP789012");
            txtContact    = AddField(form, "Contact No", ref y, 16, 320, "e.g. 082 345 6789");

            y += 8;
            var btnAdd    = AddFormBtn(form, "Add Driver",      C_TEAL,   y); y += 52;
            var btnClear  = AddFormBtn(form, "Clear Fields",    C_PANEL3, y); y += 52;
            var btnRemove = AddFormBtn(form, "Remove Selected", C_RED,    y);
            btnAdd.Click    += BtnAddDriver_Click;
            btnClear.Click  += (s, e) => ClearDriver();
            btnRemove.Click += BtnRemoveDriver_Click;
            split.Panel1.Controls.Add(form);

            var right = MakeGridPanel("REGISTERED DRIVERS");
            dgvDrivers            = StyledGrid();
            dgvDrivers.Dock       = DockStyle.Fill;
            dgvDrivers.DataSource = driverTable;
            right.Controls.Add(dgvDrivers);
            split.Panel2.Controls.Add(right);

            tab.Controls.Add(split);
            return tab;
        }

        // ── DATA CAPTURE ──────────────────────────────────────────────────────
        private TabPage BuildDataCaptureTab()
        {
            var tab   = MakeTab("Data Capture");
            var split = MakeSplit(370);

            var form = MakeFormPanel("CAPTURE VEHICLE USAGE DATA");
            int y = 54;
            cmbVehicleCapture = AddCombo(form, "Select Vehicle", ref y, 16, 320,
                new[] { "-- Select Vehicle --" });
            RefreshVehicleCombo();
            txtFuel    = AddField(form, "Fuel Used (Litres)", ref y, 16, 320, "e.g. 45.5");
            txtMileage = AddField(form, "Mileage (km)",       ref y, 16, 320, "e.g. 320");
            txtSpeed   = AddField(form, "Speed (km/h)",       ref y, 16, 320, "e.g. 90");

            lblSpeedWarning = new Label
            {
                Location  = new Point(16, y),
                Size      = new Size(320, 22),
                ForeColor = C_RED,
                Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                BackColor = Color.Transparent,
                Text      = ""
            };
            form.Controls.Add(lblSpeedWarning);
            y += 26;

            txtNotes = AddField(form, "Notes (optional)", ref y, 16, 320, "e.g. Morning delivery route");
            y += 6;

            var btnCapture = AddFormBtn(form, "Capture Data",  C_GREEN,  y); y += 52;
            var btnClear   = AddFormBtn(form, "Clear Fields",  C_PANEL3, y);
            btnCapture.Click += BtnCaptureData_Click;
            btnClear.Click   += (s, e) => ClearCapture();

            txtSpeed.TextChanged += (s, e) => {
                if (txtSpeed != null && double.TryParse(txtSpeed.Text, out double spd) && spd > SPEED_LIMIT)
                {
                    if (lblSpeedWarning != null)
                        lblSpeedWarning.Text = $"  WARNING: {spd} km/h exceeds {SPEED_LIMIT} km/h limit!";
                }
                else if (lblSpeedWarning != null)
                    lblSpeedWarning.Text = "";
            };

            split.Panel1.Controls.Add(form);

            var right = MakeGridPanel("CAPTURED USAGE RECORDS");
            dgvUsage           = StyledGrid();
            dgvUsage.Dock      = DockStyle.Fill;
            dgvUsage.DataSource= usageTable;
            dgvUsage.CellFormatting += Dgv_CellFormatting;
            right.Controls.Add(dgvUsage);
            split.Panel2.Controls.Add(right);

            tab.Controls.Add(split);
            return tab;
        }

        // ── REPORTS ───────────────────────────────────────────────────────────
        private TabPage BuildReportsTab()
        {
            var tab = MakeTab("Reports");

            var toolbar = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = C_PANEL2 };
            toolbar.Paint += (s, e) => {
                using var p = new Pen(C_BORDER, 1f);
                e.Graphics.DrawLine(p, 0, toolbar.Height - 1, toolbar.Width, toolbar.Height - 1);
            };

            cmbFilterVehicle = new ComboBox
            {
                Location      = new Point(16, 14),
                Size          = new Size(185, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor     = C_INPUT,
                ForeColor     = C_TEXT,
                FlatStyle     = FlatStyle.Flat,
                Font          = new Font("Segoe UI", 9f)
            };
            cmbFilterVehicle.Items.Add("All Vehicles");
            cmbFilterVehicle.SelectedIndex = 0;
            RefreshFilterCombo();

            btnGenerateReport = MakeToolBtn("Generate",   C_TEAL2,  new Point(212, 12), 120);
            btnExportReport   = MakeToolBtn("Export CSV", C_GREEN,  new Point(342, 12), 120);
            btnPrintReport    = MakeToolBtn("Print",      C_PANEL3, new Point(472, 12), 100);
            btnGenerateReport.Click += BtnGenerateReport_Click;
            btnExportReport.Click   += BtnExportReport_Click;
            btnPrintReport.Click    += BtnPrintReport_Click;

            toolbar.Controls.AddRange(new Control[]
                { cmbFilterVehicle, btnGenerateReport, btnExportReport, btnPrintReport });
            tab.Controls.Add(toolbar);

            var pnlSum = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = C_PANEL };
            pnlSum.Paint += (s, e) => {
                using var p = new Pen(C_BORDER, 1f);
                e.Graphics.DrawLine(p, 0, pnlSum.Height - 1, pnlSum.Width, pnlSum.Height - 1);
            };
            lblRptVehicles = MakeSumLabel("Vehicles: 0",        new Point(18,  14));
            lblRptFuel     = MakeSumLabel("Total Fuel: 0.00 L", new Point(220, 14));
            lblRptMileage  = MakeSumLabel("Total Mileage: 0 km",new Point(460, 14));
            lblRptAlerts   = MakeSumLabel("Speed Alerts: 0",    new Point(740, 14));
            pnlSum.Controls.AddRange(new Control[]
                { lblRptVehicles, lblRptFuel, lblRptMileage, lblRptAlerts });
            tab.Controls.Add(pnlSum);

            dgvReport              = StyledGrid();
            dgvReport.Dock         = DockStyle.Fill;
            dgvReport.CellFormatting += Dgv_CellFormatting;
            tab.Controls.Add(dgvReport);

            tab.Controls.SetChildIndex(dgvReport, 0);
            tab.Controls.SetChildIndex(pnlSum,    1);
            tab.Controls.SetChildIndex(toolbar,   2);

            BtnGenerateReport_Click(this, EventArgs.Empty);
            return tab;
        }

        // ── Speed row colour coding in grids ──────────────────────────────────
        private void Dgv_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (sender is not DataGridView grid) return;
            if (e.RowIndex < 0 || e.RowIndex >= grid.Rows.Count || grid.Columns.Count <= e.ColumnIndex) return;
            var row = grid.Rows[e.RowIndex];

            if (grid.Columns.Contains("Speed (km/h)") &&
                e.ColumnIndex == grid.Columns["Speed (km/h)"].Index)
            {
                var style = e.CellStyle;
                if (style == null) return;
                if (double.TryParse(row.Cells["Speed (km/h)"].Value?.ToString(), out double spd))
                {
                    if (spd > SPEED_LIMIT)
                    {
                        style.ForeColor = C_RED;
                        style.Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                        style.BackColor = Color.FromArgb(40, 10, 10);
                    }
                    else if (spd > 100)
                        style.ForeColor = C_ORANGE;
                    else
                        style.ForeColor = C_GREEN;
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  EVENT HANDLERS
        // ══════════════════════════════════════════════════════════════════════

        private void BtnAddVehicle_Click(object? sender, EventArgs e)
        {
            if (txtVehicleID == null || txtModel == null || dgvVehicles == null) return;

            string id     = txtVehicleID!.Text.Trim().ToUpper();
            string model  = txtModel!.Text.Trim();
            string type   = cmbVehicleType?.SelectedItem?.ToString() ?? "";
            string driver = cmbAssignedDriver?.SelectedItem?.ToString() ?? "Unassigned";

            if (IsHint(txtVehicleID) || string.IsNullOrEmpty(id))
            { ShowError("Vehicle ID is required."); return; }
            if (IsHint(txtModel) || string.IsNullOrEmpty(model))
            { ShowError("Model is required."); return; }

            foreach (DataRow r in vehicleTable.Rows)
                if (r["Vehicle ID"]?.ToString() == id)
                { ShowError($"Vehicle ID '{id}' already exists."); return; }

            vehicleTable.Rows.Add(id, model, type, driver, "Active",
                DateTime.Now.ToString("yyyy-MM-dd"));
            dgvVehicles.DataSource = null;
            dgvVehicles.DataSource = vehicleTable;
            RefreshDriverCombo(); RefreshVehicleCombo(); RefreshFilterCombo();
            UpdateDashboard(); ClearVehicle();
            UpdateStatus($"Vehicle '{id}' registered successfully.");
        }

        private void BtnRemoveVehicle_Click(object? sender, EventArgs e)
        {
            if (dgvVehicles == null) return;
            if (dgvVehicles.SelectedRows.Count == 0)
            { ShowError("Select a vehicle row to remove."); return; }
            string? id = dgvVehicles.SelectedRows[0].Cells["Vehicle ID"].Value?.ToString();
            if (id == null || !Confirm($"Remove vehicle '{id}'? This cannot be undone.")) return;
            for (int i = vehicleTable.Rows.Count - 1; i >= 0; i--)
                if (vehicleTable.Rows[i]["Vehicle ID"]?.ToString() == id)
                    vehicleTable.Rows.RemoveAt(i);
            RefreshVehicleCombo(); RefreshFilterCombo();
            UpdateDashboard();
            UpdateStatus($"Vehicle '{id}' removed.");
        }

        private void BtnAddDriver_Click(object? sender, EventArgs e)
        {
            if (txtDriverID == null || txtDriverName == null ||
                txtLicence  == null || txtContact   == null || dgvDrivers == null) return;

            string id      = txtDriverID!.Text.Trim().ToUpper();
            string name    = txtDriverName!.Text.Trim();
            string licence = txtLicence!.Text.Trim().ToUpper();
            string contact = txtContact!.Text.Trim();

            if (IsHint(txtDriverID)   || string.IsNullOrEmpty(id))
            { ShowError("Driver ID is required."); return; }
            if (IsHint(txtDriverName) || string.IsNullOrEmpty(name))
            { ShowError("Full Name is required."); return; }
            if (IsHint(txtLicence)    || string.IsNullOrEmpty(licence))
            { ShowError("Licence No is required."); return; }

            foreach (DataRow r in driverTable.Rows)
                if (r["Driver ID"]?.ToString() == id)
                { ShowError($"Driver ID '{id}' already exists."); return; }

            driverTable.Rows.Add(id, name, licence, contact, "Active");
            dgvDrivers.DataSource = null;
            dgvDrivers.DataSource = driverTable;
            RefreshDriverCombo(); UpdateDashboard(); ClearDriver();
            UpdateStatus($"Driver '{name}' registered successfully.");
        }

        private void BtnRemoveDriver_Click(object? sender, EventArgs e)
        {
            if (dgvDrivers == null) return;
            if (dgvDrivers.SelectedRows.Count == 0)
            { ShowError("Select a driver row to remove."); return; }
            string? id = dgvDrivers.SelectedRows[0].Cells["Driver ID"].Value?.ToString();
            if (id == null || !Confirm($"Remove driver '{id}'?")) return;
            for (int i = driverTable.Rows.Count - 1; i >= 0; i--)
                if (driverTable.Rows[i]["Driver ID"]?.ToString() == id)
                    driverTable.Rows.RemoveAt(i);
            UpdateDashboard();
            UpdateStatus($"Driver '{id}' removed.");
        }

        private void BtnCaptureData_Click(object? sender, EventArgs e)
        {
            if (cmbVehicleCapture == null || txtFuel    == null ||
                txtMileage        == null || txtSpeed   == null ||
                txtNotes          == null || dgvUsage   == null) return;

            string veh = cmbVehicleCapture.SelectedItem?.ToString() ?? "";
            if (string.IsNullOrEmpty(veh) || veh.StartsWith("--"))
            { ShowError("Please select a vehicle."); return; }

            if (!double.TryParse(txtFuel.Text,    out double fuel)    || IsHint(txtFuel))
            { ShowError("Enter a valid number for Fuel Used (L)."); return; }
            if (!double.TryParse(txtMileage.Text, out double mileage) || IsHint(txtMileage))
            { ShowError("Enter a valid number for Mileage (km)."); return; }
            if (!double.TryParse(txtSpeed.Text,   out double speed)   || IsHint(txtSpeed))
            { ShowError("Enter a valid number for Speed (km/h)."); return; }
            if (fuel < 0 || mileage < 0 || speed < 0)
            { ShowError("Values cannot be negative."); return; }

            string notes = IsHint(txtNotes) ? "" : (txtNotes?.Text.Trim() ?? "");
            usageTable.Rows.Add(veh, DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                fuel, mileage, speed, notes);
            dgvUsage.DataSource = null;
            dgvUsage.DataSource = usageTable;
            UpdateDashboard(); ClearCapture();
            UpdateStatus($"Usage data captured for {veh}.");

            if (speed > SPEED_LIMIT)
                MessageBox.Show(
                    $"SPEED ALERT\n\nVehicle {veh} was recorded at {speed} km/h.\n" +
                    $"This exceeds the {SPEED_LIMIT} km/h company speed limit.\n\n" +
                    "Please notify the fleet manager immediately.",
                    "Speed Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void BtnGenerateReport_Click(object? sender, EventArgs e)
        {
            if (cmbFilterVehicle == null || dgvReport     == null ||
                lblRptVehicles   == null || lblRptFuel    == null ||
                lblRptMileage    == null || lblRptAlerts  == null) return;

            string filter = cmbFilterVehicle.SelectedItem?.ToString() ?? "All Vehicles";
            DataTable disp = usageTable.Clone();
            foreach (DataRow r in usageTable.Rows)
                if (filter == "All Vehicles" || r["Vehicle ID"]?.ToString() == filter)
                    disp.ImportRow(r);
            dgvReport.DataSource = disp;

            double tf = 0, tm = 0;
            int al = 0;
            foreach (DataRow r in disp.Rows)
            {
                tf += Convert.ToDouble(r["Fuel Used (L)"]);
                tm += Convert.ToDouble(r["Mileage (km)"]);
                if (Convert.ToDouble(r["Speed (km/h)"]) > SPEED_LIMIT) al++;
            }

            lblRptVehicles.Text  = $"Vehicles: {vehicleTable.Rows.Count}";
            lblRptFuel.Text      = $"Total Fuel: {tf:F2} L";
            lblRptMileage.Text   = $"Total Mileage: {tm:F2} km";
            lblRptAlerts.Text    = $"Speed Alerts: {al}";
            lblRptAlerts.ForeColor = al > 0 ? C_RED : C_GREEN;
        }

        private void BtnExportReport_Click(object? sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog
            {
                Filter   = "CSV files (*.csv)|*.csv",
                FileName = $"FleetReport_{DateTime.Now:yyyyMMdd_HHmm}.csv"
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;
            try
            {
                var sb = new System.Text.StringBuilder();
                for (int i = 0; i < usageTable.Columns.Count; i++)
                {
                    sb.Append(usageTable.Columns[i].ColumnName);
                    if (i < usageTable.Columns.Count - 1) sb.Append(",");
                }
                sb.AppendLine();
                foreach (DataRow row in usageTable.Rows)
                {
                    for (int i = 0; i < usageTable.Columns.Count; i++)
                    {
                        sb.Append(row[i]);
                        if (i < usageTable.Columns.Count - 1) sb.Append(",");
                    }
                    sb.AppendLine();
                }
                System.IO.File.WriteAllText(dlg.FileName, sb.ToString());
                UpdateStatus($"Report exported to: {dlg.FileName}");
                ShowSuccess("Report exported successfully.");
            }
            catch (Exception ex) { ShowError($"Export failed: {ex.Message}"); }
        }

        private void BtnPrintReport_Click(object? sender, EventArgs e)
        {
            if (usageTable.Rows.Count == 0)
            { ShowError("No data to print. Capture some usage data first."); return; }
            printDoc = new PrintDocument();
            printDoc.DocumentName = "Apex Auto Solutions - Fleet Report";
            printDoc.PrintPage   += PrintDoc_PrintPage;
            printPageIndex        = 0;
            using var preview = new PrintPreviewDialog
            {
                Document = printDoc,
                Width    = 900,
                Height   = 660,
                Text     = "Print Preview - Fleet Usage Report"
            };
            preview.ShowDialog();
        }

        // ── PRINT HANDLER — fixed CS8602 warnings ─────────────────────────────
        private void PrintDoc_PrintPage(object? sender, PrintPageEventArgs e)
        {
            // Guard — capture non-null references once, use throughout
            if (e.Graphics == null || printDoc == null) return;
            Graphics g  = e.Graphics;                          // non-null after guard
            float mg    = 55f;
            float y     = mg;
            float pw    = e.PageBounds.Width - mg * 2;

            // All brushes / fonts declared here so they live for the whole method
            using var fTitle  = new Font("Segoe UI", 14f, FontStyle.Bold);
            using var fSub    = new Font("Segoe UI", 9f);
            using var fHeader = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            using var fBody   = new Font("Segoe UI", 8f);
            using var bTeal   = new SolidBrush(Color.FromArgb(0,   107, 107));
            using var bGrey   = new SolidBrush(Color.FromArgb(242, 246, 250));
            using var bMuted  = new SolidBrush(Color.FromArgb(100, 100, 130));
            using var bRed    = new SolidBrush(Color.FromArgb(180, 0,   0));
            using var bAlert  = new SolidBrush(Color.FromArgb(255, 240, 240));

            // ── First-page header ─────────────────────────────────────────────
            if (printPageIndex == 0)
            {
                g.FillRectangle(bTeal, mg, y, pw, 46);
                g.DrawString("Apex Auto Solutions", fTitle, Brushes.White, mg + 10, y + 6);
                g.DrawString("Fleet Monitoring & Vehicle Management System", fSub, Brushes.White, mg + 10, y + 28);
                string dt = $"Printed: {DateTime.Now:dd MMM yyyy  HH:mm}";
                SizeF ds  = g.MeasureString(dt, fSub);
                g.DrawString(dt, fSub, Brushes.White, mg + pw - ds.Width - 10, y + 18);
                y += 54;

                g.DrawString("FLEET USAGE REPORT", fHeader, bTeal, mg, y + 4);
                y += 24;

                double tf = 0, tm = 0;
                int al = 0;
                foreach (DataRow r in usageTable.Rows)
                {
                    tf += Convert.ToDouble(r["Fuel Used (L)"]);
                    tm += Convert.ToDouble(r["Mileage (km)"]);
                    if (Convert.ToDouble(r["Speed (km/h)"]) > SPEED_LIMIT) al++;
                }

                g.FillRectangle(bGrey, mg, y, pw, 20);
                g.DrawString(
                    $"Vehicles: {vehicleTable.Rows.Count}   |   Fuel: {tf:F2} L" +
                    $"   |   Mileage: {tm:F2} km   |   Speed Alerts: {al}",
                    fBody, bMuted, mg + 6, y + 4);
                y += 26;

                // Column headers
                float[] cw = { 75, 115, 65, 80, 70, pw - 405 };
                string[] hd = { "Vehicle ID", "Date & Time", "Fuel (L)", "Mileage (km)", "Speed", "Notes" };
                g.FillRectangle(bTeal, mg, y, pw, 20);
                float cx = mg;
                for (int i = 0; i < hd.Length; i++)
                {
                    g.DrawString(hd[i], fHeader, Brushes.White, cx + 3, y + 3);
                    cx += cw[i];
                }
                y += 22;
            }

            // ── Data rows ─────────────────────────────────────────────────────
            float[] colW = { 75, 115, 65, 80, 70, pw - 405 };
            bool shade   = printPageIndex % 2 == 0;

            while (printPageIndex < usageTable.Rows.Count)
            {
                if (y + 18 > e.PageBounds.Height - mg)
                {
                    e.HasMorePages = true;
                    return;
                }

                DataRow row = usageTable.Rows[printPageIndex];
                double spd  = Convert.ToDouble(row["Speed (km/h)"]);

                // Row background
                if (shade)           g.FillRectangle(bGrey,  mg, y, pw, 17); // fix warning 664
                if (spd > SPEED_LIMIT) g.FillRectangle(bAlert, mg, y, pw, 17);

                // Cell values
                string[] vals =
                {
                    row["Vehicle ID"]?.ToString()  ?? "",
                    row["Date & Time"]?.ToString() ?? "",
                    $"{Convert.ToDouble(row["Fuel Used (L)"]):F1}",
                    $"{Convert.ToDouble(row["Mileage (km)"]):F1}",
                    spd > SPEED_LIMIT ? $"{spd:F0} !" : $"{spd:F0}",
                    row["Notes"]?.ToString() ?? ""
                };

                float rx = mg;
                for (int i = 0; i < vals.Length; i++)
                {
                    // Fix warnings 670 & 674: assign brush to typed local — avoids
                    // ambiguous cast in ternary that confused nullable analysis
                    Brush cellBrush = (i == 4 && spd > SPEED_LIMIT)
                        ? (Brush)bRed
                        : Brushes.Black;
                    g.DrawString(vals[i], fBody, cellBrush, rx + 3, y + 2);
                    rx += colW[i];
                }

                y += 18;
                shade = !shade;
                printPageIndex++;
            }

            // ── Footer ────────────────────────────────────────────────────────
            y += 8;
            g.DrawLine(Pens.LightGray, mg, y, mg + pw, y);
            y += 4;
            g.DrawString(
                $"Apex Auto Solutions Fleet Report  |  {DateTime.Now:dd MMM yyyy}  |  Confidential",
                fBody, bMuted, mg, y);
            e.HasMorePages = false;
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            if (!Confirm("Are you sure you want to log out?\nAny unsaved data will be lost.")) return;
            var login = new LoginForm();
            login.Show();
            this.Close();
        }

        private void BtnExit_Click(object? sender, EventArgs e)
        {
            if (!Confirm("Are you sure you want to exit?\nThe application will close completely.")) return;
            Application.Exit();
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            if (!Confirm("Close the application?\nAny unsaved data will be lost."))
                e.Cancel = true;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  DASHBOARD UPDATE
        // ══════════════════════════════════════════════════════════════════════
        private void UpdateDashboard()
        {
            if (pnlCardVehicles == null || pnlCardDrivers == null ||
                pnlCardFuel     == null || pnlCardMileage == null || pnlCardAlerts == null ||
                lblCardVehicles == null || lblCardDrivers == null ||
                lblCardFuel     == null || lblCardMileage == null || lblCardAlerts == null)
                return;

            double tf = 0, tm = 0;
            int al = 0;
            foreach (DataRow r in usageTable.Rows)
            {
                tf += Convert.ToDouble(r["Fuel Used (L)"]);
                tm += Convert.ToDouble(r["Mileage (km)"]);
                if (Convert.ToDouble(r["Speed (km/h)"]) > SPEED_LIMIT) al++;
            }

            lblCardVehicles!.Text = vehicleTable.Rows.Count.ToString();
            lblCardDrivers!.Text  = driverTable.Rows.Count.ToString();
            lblCardFuel!.Text     = $"{tf:F2}";
            lblCardMileage!.Text  = $"{tm:F2} km";
            lblCardAlerts!.Text   = al.ToString();

            pnlCardVehicles!.Invalidate();
            pnlCardDrivers!.Invalidate();
            pnlCardFuel!.Invalidate();
            pnlCardMileage!.Invalidate();
            pnlCardAlerts!.Invalidate();
        }

        private void UpdateStatus(string msg)
        {
            if (lblStatusLeft != null) lblStatusLeft.Text = "   " + msg;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  COMBO REFRESH
        // ══════════════════════════════════════════════════════════════════════
        private void RefreshDriverCombo()
        {
            if (cmbAssignedDriver == null) return;
            cmbAssignedDriver.Items.Clear();
            cmbAssignedDriver.Items.Add("Unassigned");
            foreach (DataRow r in driverTable.Rows)
                cmbAssignedDriver.Items.Add(r["Full Name"]?.ToString() ?? "");
            cmbAssignedDriver.SelectedIndex = 0;
        }

        private void RefreshVehicleCombo()
        {
            if (cmbVehicleCapture == null) return;
            cmbVehicleCapture.Items.Clear();
            cmbVehicleCapture.Items.Add("-- Select Vehicle --");
            foreach (DataRow r in vehicleTable.Rows)
                cmbVehicleCapture.Items.Add(r["Vehicle ID"]?.ToString() ?? "");
            cmbVehicleCapture.SelectedIndex = 0;
        }

        private void RefreshFilterCombo()
        {
            if (cmbFilterVehicle == null) return;
            cmbFilterVehicle.Items.Clear();
            cmbFilterVehicle.Items.Add("All Vehicles");
            foreach (DataRow r in vehicleTable.Rows)
                cmbFilterVehicle.Items.Add(r["Vehicle ID"]?.ToString() ?? "");
            cmbFilterVehicle.SelectedIndex = 0;
        }

        private void ClearVehicle()
        {
            if (txtVehicleID != null)     ClearField(txtVehicleID,  "e.g. AAS004");
            if (txtModel     != null)     ClearField(txtModel,      "e.g. Toyota HiAce");
            if (cmbVehicleType   != null && cmbVehicleType.Items.Count   > 0) cmbVehicleType.SelectedIndex   = 0;
            if (cmbAssignedDriver != null && cmbAssignedDriver.Items.Count > 0) cmbAssignedDriver.SelectedIndex = 0;
        }

        private void ClearDriver()
        {
            if (txtDriverID   != null) ClearField(txtDriverID,   "e.g. DRV004");
            if (txtDriverName != null) ClearField(txtDriverName, "e.g. Thabo Molefe");
            if (txtLicence    != null) ClearField(txtLicence,    "e.g. GP789012");
            if (txtContact    != null) ClearField(txtContact,    "e.g. 082 345 6789");
        }

        private void ClearCapture()
        {
            if (txtFuel    != null) ClearField(txtFuel,    "e.g. 45.5");
            if (txtMileage != null) ClearField(txtMileage, "e.g. 320");
            if (txtSpeed   != null) ClearField(txtSpeed,   "e.g. 90");
            if (txtNotes   != null) ClearField(txtNotes,   "e.g. Morning delivery route");
            if (lblSpeedWarning != null) lblSpeedWarning.Text = "";
        }

        private void ClearField(TextBox? txt, string hint)
        {
            if (txt == null) return;
            txt.Text      = hint;
            txt.ForeColor = Color.FromArgb(55, 95, 110);
        }

        private bool IsHint(TextBox? txt) => txt != null && txt.ForeColor == Color.FromArgb(55, 95, 110);

        private void ShowSuccess(string msg) =>
            MessageBox.Show(msg, "Success",           MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void ShowError(string msg) =>
            MessageBox.Show(msg, "Validation Error",  MessageBoxButtons.OK, MessageBoxIcon.Warning);
        private bool Confirm(string msg) =>
            MessageBox.Show(msg, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

        // ══════════════════════════════════════════════════════════════════════
        //  UI FACTORY METHODS
        // ══════════════════════════════════════════════════════════════════════

        private TabPage MakeTab(string title) => new TabPage
        {
            Text      = title,
            BackColor = C_BG,
            Padding   = new Padding(0)
        };

        private SplitContainer MakeSplit(int dist) => new SplitContainer
        {
            Dock             = DockStyle.Fill,
            Orientation      = Orientation.Vertical,
            SplitterDistance = dist,
            BackColor        = C_BORDER,
            SplitterWidth    = 1
        };

        private Panel MakeFormPanel(string title)
        {
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = C_PANEL, Padding = new Padding(0) };
            var hdr = new Panel { Dock = DockStyle.Top,  Height = 42,         BackColor = C_PANEL };
            hdr.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var grad = new LinearGradientBrush(hdr.ClientRectangle, C_PANEL3, C_PANEL, LinearGradientMode.Vertical);
                g.FillRectangle(grad, hdr.ClientRectangle);
                using var acc = new SolidBrush(C_TEAL);
                g.FillRectangle(acc, 0, 0, 3, hdr.Height);
                using var p = new Pen(C_BORDER, 1f);
                g.DrawLine(p, 0, hdr.Height - 1, hdr.Width, hdr.Height - 1);
                using var f = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                using var b = new SolidBrush(C_TEAL);
                g.DrawString(title, f, b, new PointF(14, 13));
            };
            pnl.Controls.Add(hdr);
            return pnl;
        }

        private Panel MakeGridPanel(string title)
        {
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = C_BG, Padding = new Padding(0) };
            pnl.Controls.Add(MakeSectionBand(title));
            return pnl;
        }

        private Panel MakeSectionBand(string title)
        {
            var band = new Panel { Dock = DockStyle.Top, Height = 32, BackColor = C_PANEL2 };
            band.Paint += (s, e) =>
            {
                var g = e.Graphics;
                using var p = new Pen(C_BORDER, 1f);
                g.DrawLine(p, 0, band.Height - 1, band.Width, band.Height - 1);
                using var f = new Font("Segoe UI", 7.5f, FontStyle.Bold);
                using var b = new SolidBrush(C_MUTED);
                g.DrawString(title, f, b, new PointF(14, 9));
            };
            return band;
        }

        private (Panel panel, Label valueLabel) MakeStatCard(
            string title, string value, Color accentColor, Color dimColor, int index)
        {
            int cardW   = 216;
            int spacing = 14;
            int x       = spacing + index * (cardW + spacing);

            var pnl = new Panel
            {
                Location  = new Point(x, 10),
                Size      = new Size(cardW, 92),
                BackColor = C_PANEL
            };

            pnl.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using var bg = new SolidBrush(C_PANEL2);
                g.FillRectangle(bg, pnl.ClientRectangle);

                using var strip = new LinearGradientBrush(
                    new Rectangle(0, 0, 6, pnl.Height),
                    accentColor, dimColor, LinearGradientMode.Horizontal);
                g.FillRectangle(strip, 0, 0, 6, pnl.Height);

                using var topLine = new LinearGradientBrush(
                    new Rectangle(6, 0, pnl.Width - 6, 2),
                    accentColor, Color.FromArgb(0, accentColor), LinearGradientMode.Horizontal);
                g.FillRectangle(topLine, 6, 0, pnl.Width - 6, 2);

                using var border = new Pen(Color.FromArgb(40, accentColor), 1f);
                g.DrawRectangle(border, 0, 0, pnl.Width - 1, pnl.Height - 1);

                using var tf = new Font("Segoe UI", 7.5f, FontStyle.Bold);
                using var tb = new SolidBrush(Color.FromArgb(160, accentColor));
                g.DrawString(title, tf, tb, new PointF(14, 12));

                using var dimBrush = new SolidBrush(Color.FromArgb(30, accentColor));
                g.FillRectangle(dimBrush, 6, pnl.Height - 16, pnl.Width - 6, 16);
            };

            var lbl = new Label
            {
                Text      = value,
                Location  = new Point(10, 34),
                Size      = new Size(cardW - 16, 42),
                ForeColor = C_WHITE,
                Font      = new Font("Segoe UI", 22f, FontStyle.Bold),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnl.Controls.Add(lbl);
            return (pnl, lbl);
        }

        private Button MakeIconBtn(string text, Color bg, Point loc, int w)
        {
            var btn = new Button
            {
                Text      = text,
                Location  = loc,
                Size      = new Size(w, 36),
                BackColor = bg,
                ForeColor = C_WHITE,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Min(bg.R + 25, 255), Math.Min(bg.G + 25, 255), Math.Min(bg.B + 25, 255));
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(
                Math.Max(bg.R - 20, 0),  Math.Max(bg.G - 20, 0),  Math.Max(bg.B - 20, 0));
            Pill(btn, 5);
            return btn;
        }

        private Button MakeToolBtn(string text, Color bg, Point loc, int w)
        {
            var btn = new Button
            {
                Text      = text,
                Location  = loc,
                Size      = new Size(w, 30),
                BackColor = bg,
                ForeColor = C_WHITE,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Min(bg.R + 20, 255), Math.Min(bg.G + 20, 255), Math.Min(bg.B + 20, 255));
            Pill(btn, 4);
            return btn;
        }

        private Button AddFormBtn(Panel parent, string text, Color bg, int y)
        {
            var btn = new Button
            {
                Text      = text,
                Location  = new Point(16, y),
                Size      = new Size(322, 40),
                BackColor = bg,
                ForeColor = C_WHITE,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Min(bg.R + 20, 255), Math.Min(bg.G + 20, 255), Math.Min(bg.B + 20, 255));
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(
                Math.Max(bg.R - 20, 0),  Math.Max(bg.G - 20, 0),  Math.Max(bg.B - 20, 0));
            Pill(btn, 5);
            parent.Controls.Add(btn);
            return btn;
        }

        private Label MakeSumLabel(string text, Point loc) => new Label
        {
            Text      = text,
            Location  = loc,
            AutoSize  = true,
            ForeColor = C_TEXT,
            Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold),
            BackColor = Color.Transparent
        };

        private TextBox AddField(Panel parent, string label, ref int y, int x, int w, string hint = "")
        {
            var lbl = new Label
            {
                Text      = label.ToUpper(),
                Location  = new Point(x, y),
                Size      = new Size(w, 15),
                ForeColor = C_MUTED,
                Font      = new Font("Segoe UI", 7f, FontStyle.Bold),
                BackColor = Color.Transparent
            };
            y += 17;
            var txt = new TextBox
            {
                Location    = new Point(x, y),
                Size        = new Size(w, 24),
                BackColor   = C_INPUT,
                ForeColor   = Color.FromArgb(55, 95, 110),
                BorderStyle = BorderStyle.None,
                Font        = new Font("Segoe UI", 10f),
                MaxLength   = 100,
                Text        = string.IsNullOrEmpty(hint) ? "" : hint
            };
            var line = new Panel
            {
                Location  = new Point(x, y + 24),
                Size      = new Size(w, 1),
                BackColor = C_BORDER
            };
            txt.GotFocus  += (s, e) => {
                line.BackColor = C_TEAL;
                if (txt.Text == hint) { txt.Text = ""; txt.ForeColor = C_TEXT; }
            };
            txt.LostFocus += (s, e) => {
                line.BackColor = C_BORDER;
                if (string.IsNullOrEmpty(txt.Text) && !string.IsNullOrEmpty(hint))
                { txt.Text = hint; txt.ForeColor = Color.FromArgb(55, 95, 110); }
            };
            y += 42;
            parent.Controls.AddRange(new Control[] { lbl, txt, line });
            return txt;
        }

        private ComboBox AddCombo(Panel parent, string label, ref int y, int x, int w, string[] items)
        {
            var lbl = new Label
            {
                Text      = label.ToUpper(),
                Location  = new Point(x, y),
                Size      = new Size(w, 15),
                ForeColor = C_MUTED,
                Font      = new Font("Segoe UI", 7f, FontStyle.Bold),
                BackColor = Color.Transparent
            };
            y += 17;
            var cmb = new ComboBox
            {
                Location      = new Point(x, y),
                Size          = new Size(w, 24),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor     = C_INPUT,
                ForeColor     = C_TEXT,
                FlatStyle     = FlatStyle.Flat,
                Font          = new Font("Segoe UI", 10f)
            };
            cmb.Items.AddRange(items);
            if (cmb.Items.Count > 0) cmb.SelectedIndex = 0;
            var line = new Panel
            {
                Location  = new Point(x, y + 24),
                Size      = new Size(w, 1),
                BackColor = C_BORDER
            };
            cmb.GotFocus  += (s, e) => line.BackColor = C_TEAL;
            cmb.LostFocus += (s, e) => line.BackColor = C_BORDER;
            y += 42;
            parent.Controls.AddRange(new Control[] { lbl, cmb, line });
            return cmb;
        }

        private DataGridView StyledGrid()
        {
            var g = new DataGridView
            {
                BackgroundColor       = C_BG,
                BorderStyle           = BorderStyle.None,
                RowHeadersVisible     = false,
                AutoSizeColumnsMode   = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows    = false,
                AllowUserToDeleteRows = false,
                ReadOnly              = true,
                SelectionMode         = DataGridViewSelectionMode.FullRowSelect,
                Font                  = new Font("Segoe UI", 8.5f),
                GridColor             = C_BORDER,
                CellBorderStyle       = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = C_ROW_ALT,
                    ForeColor = C_TEXT
                }
            };
            g.DefaultCellStyle.BackColor          = C_PANEL;
            g.DefaultCellStyle.ForeColor          = C_TEXT;
            g.DefaultCellStyle.SelectionBackColor = C_SEL;
            g.DefaultCellStyle.SelectionForeColor = C_WHITE;
            g.DefaultCellStyle.Padding            = new Padding(6, 0, 4, 0);
            g.ColumnHeadersDefaultCellStyle.BackColor        = C_GRID_HDR;
            g.ColumnHeadersDefaultCellStyle.ForeColor        = C_TEAL;
            g.ColumnHeadersDefaultCellStyle.Font             = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Padding          = new Padding(8, 0, 0, 0);
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = C_GRID_HDR;
            g.ColumnHeadersHeight       = 36;
            g.RowTemplate.Height        = 32;
            g.EnableHeadersVisualStyles = false;
            return g;
        }

        private void Pill(Control c, int r)
        {
            var path = new GraphicsPath();
            int d    = r * 2;
            path.AddArc(0,             0,              d, d, 180, 90);
            path.AddArc(c.Width - d,   0,              d, d, 270, 90);
            path.AddArc(c.Width - d,   c.Height - d,   d, d,   0, 90);
            path.AddArc(0,             c.Height - d,   d, d,  90, 90);
            path.CloseAllFigures();
            c.Region = new Region(path);
        }

        private Icon CreateAppIcon()
        {
            try
            {
                using var bmp = new Bitmap(32, 32);
                using var g   = Graphics.FromImage(bmp);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillEllipse(new SolidBrush(Color.FromArgb(0, 107, 107)), 0, 0, 31, 31);
                g.DrawString("⚙", new Font("Segoe UI", 16f), Brushes.White,
                    new RectangleF(0, 1, 32, 30),
                    new StringFormat
                    {
                        Alignment     = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    });
                var hIcon = bmp.GetHicon();
                return Icon.FromHandle(hIcon);
            }
            catch { return SystemIcons.Application; }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
    }
}