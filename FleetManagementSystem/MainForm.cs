﻿// ═══════════════════════════════════════════════════════════════════════════════
//  Fleet Monitoring & Vehicle Management System
//  Apex Auto Solutions | SPM622 Formative Assessment 1
//  Developer : Nicolette Mashaba  | Student No: 200232990
//  Date      : 28 April 2026
//  Features  : Dashboard, Vehicles, Drivers, Data Capture,
//              Reports, Print Preview, Logout, Exit
// ═══════════════════════════════════════════════════════════════════════════════

using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace FleetManagementSystem
{
    public partial class MainForm : Form
    {
        // ── In-Memory Data Tables ───────────────────────────────────────────────
        private DataTable vehicleTable = new DataTable();
        private DataTable driverTable = new DataTable();
        private DataTable usageTable = new DataTable();

        // ── Dashboard stat card labels ─────────────────────────────────────────
        private Label lblDashVehicles = null!, lblDashDrivers = null!,
                      lblDashFuel = null!, lblDashMileage = null!, lblDashAlerts = null!;

        // ── Vehicle tab controls ───────────────────────────────────────────────
        private TextBox txtVehicleID = null!, txtModel = null!;
        private ComboBox cmbVehicleType = null!, cmbAssignedDriver = null!;
        private DataGridView dgvVehicles = null!;
        private Button btnAddVehicle = null!, btnClearVehicle = null!, btnRemoveVehicle = null!;

        // ── Driver tab controls ────────────────────────────────────────────────
        private TextBox txtDriverID = null!, txtDriverName = null!, txtLicence = null!, txtContact = null!;
        private DataGridView dgvDrivers = null!;
        private Button btnAddDriver = null!, btnClearDriver = null!, btnRemoveDriver = null!;

        // ── Data Capture tab controls ──────────────────────────────────────────
        private ComboBox cmbVehicleCapture = null!;
        private TextBox txtFuel = null!, txtMileage = null!, txtSpeed = null!, txtNotes = null!;
        private DataGridView dgvUsage = null!;
        private Label lblSpeedWarning = null!;
        private Button btnCaptureData = null!, btnClearCapture = null!;

        // ── Reports tab controls ───────────────────────────────────────────────
        private DataGridView dgvReport = null!;
        private Label lblRptVehicles = null!, lblRptFuel = null!, lblRptMileage = null!, lblRptAlerts = null!;
        private Button btnGenerateReport = null!, btnExportReport = null!, btnPrintReport = null!;
        private ComboBox cmbFilterVehicle = null!;

        // ── Print support ──────────────────────────────────────────────────────
        private PrintDocument printDoc = null!;
        private int printPageIndex = 0;

        // ── Colour palette ─────────────────────────────────────────────────────
        private readonly Color C_TEAL = Color.FromArgb(0, 107, 107);
        private readonly Color C_DARK = Color.FromArgb(26, 26, 46);
        private readonly Color C_ACCENT = Color.FromArgb(22, 33, 62);
        private readonly Color C_LIGHT = Color.FromArgb(232, 244, 248);
        private readonly Color C_WHITE = Color.White;
        private readonly Color C_GREY = Color.FromArgb(245, 247, 250);
        private readonly Color C_GREEN = Color.FromArgb(40, 167, 69);
        private readonly Color C_RED = Color.FromArgb(220, 53, 69);
        private readonly Color C_ORANGE = Color.FromArgb(255, 152, 0);
        private const double SPEED_LIMIT_KMH = 120.0;

        // ══════════════════════════════════════════════════════════════════════
        //  CONSTRUCTOR
        // ══════════════════════════════════════════════════════════════════════
        public MainForm()
        {
            InitialiseDataTables();
            SetupUI();
            LoadSampleData();
            UpdateDashboard();
        }

        // ── Define column schemas for all three tables ─────────────────────────
        private void InitialiseDataTables()
        {
            vehicleTable.Columns.Add("Vehicle ID", typeof(string));
            vehicleTable.Columns.Add("Model", typeof(string));
            vehicleTable.Columns.Add("Type", typeof(string));
            vehicleTable.Columns.Add("Assigned Driver", typeof(string));
            vehicleTable.Columns.Add("Status", typeof(string));
            vehicleTable.Columns.Add("Registered", typeof(string));

            driverTable.Columns.Add("Driver ID", typeof(string));
            driverTable.Columns.Add("Full Name", typeof(string));
            driverTable.Columns.Add("Licence No", typeof(string));
            driverTable.Columns.Add("Contact", typeof(string));
            driverTable.Columns.Add("Status", typeof(string));

            usageTable.Columns.Add("Vehicle ID", typeof(string));
            usageTable.Columns.Add("Date & Time", typeof(string));
            usageTable.Columns.Add("Fuel Used (L)", typeof(double));
            usageTable.Columns.Add("Mileage (km)", typeof(double));
            usageTable.Columns.Add("Speed (km/h)", typeof(double));
            usageTable.Columns.Add("Notes", typeof(string));
        }

        // ── Pre-load demo data ─────────────────────────────────────────────────
        private void LoadSampleData()
        {
            driverTable.Rows.Add("DRV001", "Sipho Dlamini", "GP123456", "071 234 5678", "Active");
            driverTable.Rows.Add("DRV002", "Thandeka Nkosi", "WC987654", "082 345 6789", "Active");
            driverTable.Rows.Add("DRV003", "Priya Reddy", "GP555444", "063 456 7890", "Active");

            vehicleTable.Rows.Add("AAS001", "Toyota HiAce", "Van", "Sipho Dlamini", "Active", DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
            vehicleTable.Rows.Add("AAS002", "Ford Ranger", "Bakkie", "Thandeka Nkosi", "Active", DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
            vehicleTable.Rows.Add("AAS003", "Mercedes Sprinter", "Truck", "Priya Reddy", "Active", DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd"));

            usageTable.Rows.Add("AAS001", DateTime.Now.AddHours(-5).ToString("yyyy-MM-dd HH:mm"), 45.5, 320.0, 90.0, "Morning delivery route");
            usageTable.Rows.Add("AAS002", DateTime.Now.AddHours(-3).ToString("yyyy-MM-dd HH:mm"), 60.2, 480.5, 105.0, "Highway run");
            usageTable.Rows.Add("AAS001", DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm"), 30.1, 210.0, 75.0, "Afternoon route");
            usageTable.Rows.Add("AAS003", DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm"), 85.0, 610.0, 125.0, "Long haul - SPEED ALERT");
        }

        // ══════════════════════════════════════════════════════════════════════
        //  UI BUILD
        // ══════════════════════════════════════════════════════════════════════
        private void SetupUI()
        {
            this.Text = "Fleet Monitoring & Vehicle Management System  |  Apex Auto Solutions";
            this.Size = new Size(1200, 780);
            this.MinimumSize = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = C_DARK;
            this.Font = new Font("Segoe UI", 9f);
            this.FormClosing += MainForm_FormClosing;

            // ── Header banner ───────────────────────────────────────────────
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = C_TEAL
            };

            var lblTitle = new Label
            {
                Text = "  Fleet Monitoring & Vehicle Management System",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Dock = DockStyle.Left,
                Width = 620,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblSub = new Label
            {
                Text = "Apex Auto Solutions  |  Nicolette Mashaba  |  200232990",
                ForeColor = Color.FromArgb(200, 255, 255),
                Font = new Font("Segoe UI", 8.5f),
                Location = new Point(625, 0),
                Size = new Size(295, 60),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // ── LOGOUT button (header, right side) ─────────────────────────
            var btnLogout = new Button
            {
                Text = "Logout",
                Size = new Size(90, 34),
                Location = new Point(955, 13),
                BackColor = Color.FromArgb(0, 80, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderColor = Color.FromArgb(0, 55, 55);
            btnLogout.Click += BtnLogout_Click;

            // ── EXIT button (header, right side) ───────────────────────────
            var btnExit = new Button
            {
                Text = "Exit",
                Size = new Size(80, 34),
                Location = new Point(1055, 13),
                BackColor = C_RED,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnExit.FlatAppearance.BorderColor = Color.FromArgb(160, 20, 20);
            btnExit.Click += BtnExit_Click;

            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSub, btnLogout, btnExit });
            this.Controls.Add(pnlHeader);

            // ── Tab Control ─────────────────────────────────────────────────
            var tabMain = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Padding = new Point(18, 6)
            };

            tabMain.TabPages.Add(BuildDashboardTab());
            tabMain.TabPages.Add(BuildVehicleTab());
            tabMain.TabPages.Add(BuildDriverTab());
            tabMain.TabPages.Add(BuildDataCaptureTab());
            tabMain.TabPages.Add(BuildReportsTab());

            this.Controls.Add(tabMain);
            tabMain.BringToFront();
        }

        // ── DASHBOARD TAB ──────────────────────────────────────────────────────
        private TabPage BuildDashboardTab()
        {
            var tab = MakeTab("  Dashboard");
            var lbl = MakeSectionLabel("FLEET PERFORMANCE DASHBOARD");
            lbl.Dock = DockStyle.Top;
            tab.Controls.Add(lbl);

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 130,
                BackColor = Color.Transparent,
                Padding = new Padding(10),
                FlowDirection = FlowDirection.LeftToRight
            };

            lblDashVehicles = MakeStatCard("VEHICLES", "0", C_TEAL);
            lblDashDrivers = MakeStatCard("DRIVERS", "0", C_ACCENT);
            lblDashFuel = MakeStatCard("TOTAL FUEL (L)", "0.00", C_GREEN);
            lblDashMileage = MakeStatCard("MILEAGE (km)", "0.00", C_ORANGE);
            lblDashAlerts = MakeStatCard("SPEED ALERTS", "0", C_RED);

            flow.Controls.AddRange(new[] { lblDashVehicles, lblDashDrivers, lblDashFuel, lblDashMileage, lblDashAlerts });
            tab.Controls.Add(flow);

            var lblR = MakeSectionLabel("RECENT VEHICLE USAGE");
            lblR.Dock = DockStyle.Top;
            tab.Controls.Add(lblR);

            var dgv = StyledGrid();
            dgv.Dock = DockStyle.Fill;
            dgv.DataSource = usageTable;
            tab.Controls.Add(dgv);

            tab.Controls.SetChildIndex(dgv, 0);
            tab.Controls.SetChildIndex(lblR, 1);
            tab.Controls.SetChildIndex(flow, 2);
            tab.Controls.SetChildIndex(lbl, 3);
            return tab;
        }

        // ── VEHICLE TAB ────────────────────────────────────────────────────────
        private TabPage BuildVehicleTab()
        {
            var tab = MakeTab("  Vehicles");
            var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Vertical, SplitterDistance = 340 };

            var pnl = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15), BackColor = C_WHITE };
            pnl.Controls.Add(MakeSectionLabel("REGISTER NEW VEHICLE"));
            int y = 45;
            txtVehicleID = MakeInput(pnl, "Vehicle ID:", ref y);
            txtModel = MakeInput(pnl, "Model:", ref y);
            cmbVehicleType = MakeCombo(pnl, "Vehicle Type:", new[] { "Van", "Bakkie", "Truck", "Car", "Bus", "Motorcycle" }, ref y);
            cmbAssignedDriver = MakeCombo(pnl, "Assign Driver:", new[] { "Unassigned" }, ref y);
            RefreshDriverCombo();

            btnAddVehicle = MakeButton("ADD VEHICLE", C_TEAL, new Point(15, y)); y += 48;
            btnClearVehicle = MakeButton("CLEAR FIELDS", C_ACCENT, new Point(15, y)); y += 48;
            btnRemoveVehicle = MakeButton("REMOVE SELECTED", C_RED, new Point(15, y));

            btnAddVehicle.Click += BtnAddVehicle_Click;
            btnClearVehicle.Click += (s, e) => ClearVehicleFields();
            btnRemoveVehicle.Click += BtnRemoveVehicle_Click;
            pnl.Controls.AddRange(new Control[] { btnAddVehicle, btnClearVehicle, btnRemoveVehicle });
            split.Panel1.Controls.Add(pnl);

            var pnlR = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            pnlR.Controls.Add(MakeSectionLabel("REGISTERED VEHICLES"));
            dgvVehicles = StyledGrid();
            dgvVehicles.Dock = DockStyle.Fill;
            dgvVehicles.DataSource = vehicleTable;
            pnlR.Controls.Add(dgvVehicles);
            split.Panel2.Controls.Add(pnlR);
            tab.Controls.Add(split);
            return tab;
        }

        // ── DRIVER TAB ─────────────────────────────────────────────────────────
        private TabPage BuildDriverTab()
        {
            var tab = MakeTab("  Drivers");
            var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Vertical, SplitterDistance = 340 };

            var pnl = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15), BackColor = C_WHITE };
            pnl.Controls.Add(MakeSectionLabel("REGISTER NEW DRIVER"));
            int y = 45;
            txtDriverID = MakeInput(pnl, "Driver ID:", ref y);
            txtDriverName = MakeInput(pnl, "Full Name:", ref y);
            txtLicence = MakeInput(pnl, "Licence No:", ref y);
            txtContact = MakeInput(pnl, "Contact No:", ref y);

            btnAddDriver = MakeButton("ADD DRIVER", C_TEAL, new Point(15, y)); y += 48;
            btnClearDriver = MakeButton("CLEAR FIELDS", C_ACCENT, new Point(15, y)); y += 48;
            btnRemoveDriver = MakeButton("REMOVE SELECTED", C_RED, new Point(15, y));

            btnAddDriver.Click += BtnAddDriver_Click;
            btnClearDriver.Click += (s, e) => ClearDriverFields();
            btnRemoveDriver.Click += BtnRemoveDriver_Click;
            pnl.Controls.AddRange(new Control[] { btnAddDriver, btnClearDriver, btnRemoveDriver });
            split.Panel1.Controls.Add(pnl);

            var pnlR = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            pnlR.Controls.Add(MakeSectionLabel("REGISTERED DRIVERS"));
            dgvDrivers = StyledGrid();
            dgvDrivers.Dock = DockStyle.Fill;
            dgvDrivers.DataSource = driverTable;
            pnlR.Controls.Add(dgvDrivers);
            split.Panel2.Controls.Add(pnlR);
            tab.Controls.Add(split);
            return tab;
        }

        // ── DATA CAPTURE TAB ───────────────────────────────────────────────────
        private TabPage BuildDataCaptureTab()
        {
            var tab = MakeTab("  Data Capture");
            var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Vertical, SplitterDistance = 340 };

            var pnl = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15), BackColor = C_WHITE };
            pnl.Controls.Add(MakeSectionLabel("CAPTURE VEHICLE USAGE DATA"));
            int y = 45;
            cmbVehicleCapture = MakeCombo(pnl, "Select Vehicle:", new[] { "Select..." }, ref y);
            RefreshVehicleCombo();
            txtFuel = MakeInput(pnl, "Fuel Used (Litres):", ref y, "e.g. 45.5");
            txtMileage = MakeInput(pnl, "Mileage (km):", ref y, "e.g. 320");
            txtSpeed = MakeInput(pnl, "Speed (km/h):", ref y, "e.g. 90");
            txtNotes = MakeInput(pnl, "Notes (optional):", ref y, "e.g. Morning delivery");

            lblSpeedWarning = new Label
            {
                Location = new Point(15, y),
                Size = new Size(290, 26),
                ForeColor = C_RED,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Text = ""
            };
            pnl.Controls.Add(lblSpeedWarning);
            y += 30;

            btnCaptureData = MakeButton("CAPTURE DATA", C_GREEN, new Point(15, y)); y += 48;
            btnClearCapture = MakeButton("CLEAR FIELDS", C_ACCENT, new Point(15, y));
            btnCaptureData.Click += BtnCaptureData_Click;
            btnClearCapture.Click += (s, e) => ClearCaptureFields();

            txtSpeed.TextChanged += (s, e) =>
            {
                if (double.TryParse(txtSpeed.Text, out double spd) && spd > SPEED_LIMIT_KMH)
                    lblSpeedWarning.Text = $"WARNING: Speed {spd} km/h exceeds {SPEED_LIMIT_KMH} km/h limit!";
                else
                    lblSpeedWarning.Text = "";
            };

            pnl.Controls.AddRange(new Control[] { btnCaptureData, btnClearCapture });
            split.Panel1.Controls.Add(pnl);

            var pnlR = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            pnlR.Controls.Add(MakeSectionLabel("CAPTURED USAGE RECORDS"));
            dgvUsage = StyledGrid();
            dgvUsage.Dock = DockStyle.Fill;
            dgvUsage.DataSource = usageTable;
            pnlR.Controls.Add(dgvUsage);
            split.Panel2.Controls.Add(pnlR);
            tab.Controls.Add(split);
            return tab;
        }

        // ── REPORTS TAB ────────────────────────────────────────────────────────
        private TabPage BuildReportsTab()
        {
            var tab = MakeTab("  Reports");
            var toolbar = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = C_DARK, Padding = new Padding(10, 8, 10, 8) };

            cmbFilterVehicle = new ComboBox
            {
                Location = new Point(10, 12),
                Size = new Size(165, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = C_WHITE
            };
            cmbFilterVehicle.Items.Add("All Vehicles");
            cmbFilterVehicle.SelectedIndex = 0;
            RefreshFilterCombo();

            btnGenerateReport = new Button
            {
                Location = new Point(185, 10),
                Size = new Size(140, 32),
                Text = "GENERATE REPORT",
                BackColor = C_TEAL,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            btnGenerateReport.Click += BtnGenerateReport_Click;

            btnExportReport = new Button
            {
                Location = new Point(335, 10),
                Size = new Size(130, 32),
                Text = "EXPORT CSV",
                BackColor = C_GREEN,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            btnExportReport.Click += BtnExportReport_Click;

            // ── PRINT REPORT button ─────────────────────────────────────────
            btnPrintReport = new Button
            {
                Location = new Point(475, 10),
                Size = new Size(140, 32),
                Text = "PRINT REPORT",
                BackColor = C_ACCENT,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            btnPrintReport.Click += BtnPrintReport_Click;

            toolbar.Controls.AddRange(new Control[] { cmbFilterVehicle, btnGenerateReport, btnExportReport, btnPrintReport });
            tab.Controls.Add(toolbar);

            var pnlSummary = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = C_ACCENT, Padding = new Padding(10) };
            lblRptVehicles = MakeSummaryLabel("Vehicles: 0", new Point(10, 15));
            lblRptFuel = MakeSummaryLabel("Total Fuel: 0.00 L", new Point(200, 15));
            lblRptMileage = MakeSummaryLabel("Total Mileage: 0 km", new Point(430, 15));
            lblRptAlerts = MakeSummaryLabel("Speed Alerts: 0", new Point(700, 15));
            pnlSummary.Controls.AddRange(new Control[] { lblRptVehicles, lblRptFuel, lblRptMileage, lblRptAlerts });
            tab.Controls.Add(pnlSummary);

            dgvReport = StyledGrid();
            dgvReport.Dock = DockStyle.Fill;
            tab.Controls.Add(dgvReport);

            tab.Controls.SetChildIndex(dgvReport, 0);
            tab.Controls.SetChildIndex(pnlSummary, 1);
            tab.Controls.SetChildIndex(toolbar, 2);

            BtnGenerateReport_Click(this, EventArgs.Empty);
            return tab;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  EVENT HANDLERS
        // ══════════════════════════════════════════════════════════════════════

        private void BtnAddVehicle_Click(object? sender, EventArgs e)
        {
            string id = txtVehicleID.Text.Trim().ToUpper();
            string model = txtModel.Text.Trim();
            string type = cmbVehicleType.SelectedItem?.ToString() ?? "";
            string driver = cmbAssignedDriver.SelectedItem?.ToString() ?? "Unassigned";

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(model))
            { ShowError("Vehicle ID and Model are required fields."); return; }

            foreach (DataRow r in vehicleTable.Rows)
                if (r["Vehicle ID"].ToString() == id)
                { ShowError($"Vehicle ID '{id}' already exists."); return; }

            vehicleTable.Rows.Add(id, model, type, driver, "Active", DateTime.Now.ToString("yyyy-MM-dd"));
            dgvVehicles.DataSource = null;
            dgvVehicles.DataSource = vehicleTable;
            RefreshDriverCombo(); RefreshVehicleCombo(); RefreshFilterCombo();
            UpdateDashboard(); ClearVehicleFields();
            ShowSuccess($"Vehicle '{id} - {model}' registered successfully!");
        }

        private void BtnRemoveVehicle_Click(object? sender, EventArgs e)
        {
            if (dgvVehicles.SelectedRows.Count == 0) { ShowError("Select a vehicle to remove."); return; }
            string? id = dgvVehicles.SelectedRows[0].Cells["Vehicle ID"].Value?.ToString();
            if (string.IsNullOrEmpty(id)) return;
            if (MessageBox.Show($"Remove vehicle {id}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            {
                for (int i = vehicleTable.Rows.Count - 1; i >= 0; i--)
                    if (vehicleTable.Rows[i]["Vehicle ID"].ToString() == id)
                        vehicleTable.Rows.RemoveAt(i);
                UpdateDashboard();
                RefreshVehicleCombo();
                RefreshFilterCombo();
            }
        }

        private void BtnAddDriver_Click(object? sender, EventArgs e)
        {
            string id = txtDriverID.Text.Trim().ToUpper();
            string name = txtDriverName.Text.Trim();
            string licence = txtLicence.Text.Trim().ToUpper();
            string contact = txtContact.Text.Trim();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(licence))
            { ShowError("Driver ID, Full Name and Licence No are required."); return; }

            foreach (DataRow r in driverTable.Rows)
                if (r["Driver ID"].ToString() == id)
                { ShowError($"Driver ID '{id}' already exists."); return; }

            driverTable.Rows.Add(id, name, licence, contact, "Active");
            dgvDrivers.DataSource = null;
            dgvDrivers.DataSource = driverTable;
            RefreshDriverCombo(); UpdateDashboard(); ClearDriverFields();
            ShowSuccess($"Driver '{name}' registered successfully!");
        }

        private void BtnRemoveDriver_Click(object? sender, EventArgs e)
        {
            if (dgvDrivers.SelectedRows.Count == 0) { ShowError("Select a driver to remove."); return; }
            string? id = dgvDrivers.SelectedRows[0].Cells["Driver ID"].Value?.ToString();
            if (string.IsNullOrEmpty(id)) return;
            if (MessageBox.Show($"Remove driver {id}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            {
                for (int i = driverTable.Rows.Count - 1; i >= 0; i--)
                    if (driverTable.Rows[i]["Driver ID"].ToString() == id)
                        driverTable.Rows.RemoveAt(i);
                UpdateDashboard();
            }
        }

        private void BtnCaptureData_Click(object? sender, EventArgs e)
        {
            string? vehID = cmbVehicleCapture.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(vehID) || vehID == "Select...") { ShowError("Please select a vehicle."); return; }
            if (!double.TryParse(txtFuel.Text, out double fuel)) { ShowError("Enter a valid number for Fuel Used (L)."); return; }
            if (!double.TryParse(txtMileage.Text, out double mileage)) { ShowError("Enter a valid number for Mileage (km)."); return; }
            if (!double.TryParse(txtSpeed.Text, out double speed)) { ShowError("Enter a valid number for Speed (km/h)."); return; }
            if (fuel < 0 || mileage < 0 || speed < 0) { ShowError("Values cannot be negative."); return; }

            usageTable.Rows.Add(vehID, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), fuel, mileage, speed, txtNotes.Text.Trim());
            dgvUsage.DataSource = null;
            dgvUsage.DataSource = usageTable;
            UpdateDashboard(); ClearCaptureFields();

            if (speed > SPEED_LIMIT_KMH)
                MessageBox.Show($"SPEED ALERT!\n\nVehicle {vehID} recorded {speed} km/h.\nThis exceeds the {SPEED_LIMIT_KMH} km/h limit!\nPlease notify the fleet manager immediately.",
                    "Speed Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                ShowSuccess($"Usage data captured for vehicle {vehID}.");
        }

        private void BtnGenerateReport_Click(object? sender, EventArgs e)
        {
            string filter = cmbFilterVehicle?.SelectedItem?.ToString() ?? "All Vehicles";
            DataTable disp = usageTable.Clone();
            foreach (DataRow r in usageTable.Rows)
                if (filter == "All Vehicles" || r["Vehicle ID"].ToString() == filter)
                    disp.ImportRow(r);
            dgvReport.DataSource = disp;

            double tf = 0, tm = 0; int al = 0;
            foreach (DataRow r in disp.Rows)
            {
                tf += Convert.ToDouble(r["Fuel Used (L)"]);
                tm += Convert.ToDouble(r["Mileage (km)"]);
                if (Convert.ToDouble(r["Speed (km/h)"]) > SPEED_LIMIT_KMH) al++;
            }
            if (lblRptVehicles != null)
            {
                lblRptVehicles.Text = $"Vehicles: {vehicleTable.Rows.Count}";
                lblRptFuel.Text = $"Total Fuel: {tf:F2} L";
                lblRptMileage.Text = $"Total Mileage: {tm:F2} km";
                lblRptAlerts.Text = $"Speed Alerts: {al}";
                lblRptAlerts.ForeColor = al > 0 ? C_RED : C_GREEN;
            }
        }

        private void BtnExportReport_Click(object? sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv", FileName = $"FleetReport_{DateTime.Now:yyyyMMdd_HHmm}.csv" };
            if (dlg.ShowDialog() != DialogResult.OK) return;
            try
            {
                var sb = new System.Text.StringBuilder();
                for (int i = 0; i < usageTable.Columns.Count; i++)
                { sb.Append(usageTable.Columns[i].ColumnName); if (i < usageTable.Columns.Count - 1) sb.Append(","); }
                sb.AppendLine();
                foreach (DataRow row in usageTable.Rows)
                {
                    for (int i = 0; i < usageTable.Columns.Count; i++)
                    { sb.Append(row[i].ToString()); if (i < usageTable.Columns.Count - 1) sb.Append(","); }
                    sb.AppendLine();
                }
                System.IO.File.WriteAllText(dlg.FileName, sb.ToString());
                ShowSuccess($"Report exported to:\n{dlg.FileName}");
            }
            catch (Exception ex) { ShowError($"Export failed: {ex.Message}"); }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PRINT REPORT
        // ══════════════════════════════════════════════════════════════════════
        private void BtnPrintReport_Click(object? sender, EventArgs e)
        {
            if (usageTable.Rows.Count == 0)
            { ShowError("No data to print. Capture some usage data first."); return; }

            printDoc = new PrintDocument();
            printDoc.DocumentName = "Apex Auto Solutions - Fleet Report";
            printDoc.PrintPage += PrintDoc_PrintPage;
            printPageIndex = 0;

            // Show print preview dialog so user can review before printing
            using var preview = new PrintPreviewDialog
            {
                Document = printDoc,
                Width = 900,
                Height = 650,
                Text = "Print Preview - Fleet Usage Report"
            };
            preview.ShowDialog();
        }

        // ── Draws the content of each printed page ─────────────────────────────
        private void PrintDoc_PrintPage(object? sender, PrintPageEventArgs e)
        {
            if (e.Graphics == null) return;
            Graphics g = e.Graphics;
            float margin = 55f;
            float y = margin;
            float pageW = e.PageBounds.Width - (margin * 2);

            var fTitle = new Font("Segoe UI", 15f, FontStyle.Bold);
            var fSub = new Font("Segoe UI", 9f);
            var fHeader = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            var fBody = new Font("Segoe UI", 8f);

            var bTeal = new SolidBrush(Color.FromArgb(0, 107, 107));
            var bGrey = new SolidBrush(Color.FromArgb(242, 246, 250));
            var bMuted = new SolidBrush(Color.FromArgb(100, 100, 130));
            var bLight = new SolidBrush(Color.FromArgb(255, 235, 235));

            if (printPageIndex == 0)
            {
                // Company header
                g.FillRectangle(bTeal, margin, y, pageW, 48);
                g.DrawString("Apex Auto Solutions", fTitle, Brushes.White, margin + 10, y + 6);
                g.DrawString("Fleet Monitoring & Vehicle Management System", fSub, Brushes.White, margin + 10, y + 28);
                string dt = $"Printed: {DateTime.Now:dd MMM yyyy  HH:mm}";
                SizeF ds = g.MeasureString(dt, fSub);
                g.DrawString(dt, fSub, Brushes.White, margin + pageW - ds.Width - 10, y + 18);
                y += 56;

                // Section heading
                g.DrawString("FLEET USAGE REPORT", fHeader, bTeal, margin, y + 6);
                y += 26;

                // Summary strip
                double tf = 0, tm = 0; int al = 0;
                foreach (DataRow r in usageTable.Rows)
                {
                    tf += Convert.ToDouble(r["Fuel Used (L)"]);
                    tm += Convert.ToDouble(r["Mileage (km)"]);
                    if (Convert.ToDouble(r["Speed (km/h)"]) > SPEED_LIMIT_KMH) al++;
                }
                g.FillRectangle(bGrey, margin, y, pageW, 20);
                g.DrawString($"Vehicles: {vehicleTable.Rows.Count}   |   Total Fuel: {tf:F2} L   |   Total Mileage: {tm:F2} km   |   Speed Alerts: {al}",
                    fBody, bMuted, margin + 6, y + 4);
                y += 28;

                // Column headers
                float[] cw = { 75, 115, 65, 80, 70, pageW - 405 };
                string[] hd = { "Vehicle ID", "Date & Time", "Fuel (L)", "Mileage (km)", "Speed", "Notes" };
                g.FillRectangle(bTeal, margin, y, pageW, 20);
                float cx = margin;
                foreach (var h in hd) { g.DrawString(h, fHeader, Brushes.White, cx + 3, y + 3); cx += cw[Array.IndexOf(hd, h)]; }
                y += 22;
            }

            // Data rows
            float[] colW = { 75, 115, 65, 80, 70, pageW - 405 };
            bool shade = printPageIndex % 2 == 0;

            while (printPageIndex < usageTable.Rows.Count)
            {
                if (y + 18 > e.PageBounds.Height - margin) { e.HasMorePages = true; return; }

                DataRow row = usageTable.Rows[printPageIndex];
                double speed = Convert.ToDouble(row["Speed (km/h)"]);

                if (shade) g.FillRectangle(bGrey, margin, y, pageW, 17);
                if (speed > SPEED_LIMIT_KMH) g.FillRectangle(bLight, margin, y, pageW, 17);

                string[] vals =
                {
                    row["Vehicle ID"]?.ToString() ?? string.Empty,
                    row["Date & Time"]?.ToString() ?? string.Empty,
                    $"{Convert.ToDouble(row["Fuel Used (L)"]):F1}",
                    $"{Convert.ToDouble(row["Mileage (km)"]):F1}",
                    speed > SPEED_LIMIT_KMH ? $"{speed:F0} !" : $"{speed:F0}",
                    row["Notes"]?.ToString() ?? string.Empty
                };

                float rx = margin;
                for (int i = 0; i < vals.Length; i++)
                {
                    var brush = (i == 4 && speed > SPEED_LIMIT_KMH) ? new SolidBrush(Color.FromArgb(180, 0, 0)) : (Brush)Brushes.Black;
                    g.DrawString(vals[i], fBody, brush, rx + 3, y + 2);
                    rx += colW[i];
                }
                y += 18;
                shade = !shade;
                printPageIndex++;
            }

            // Footer
            y += 8;
            g.DrawLine(Pens.LightGray, margin, y, margin + pageW, y);
            y += 4;
            g.DrawString($"Apex Auto Solutions Fleet Report  |  {DateTime.Now:dd MMM yyyy}  |  Confidential", fBody, bMuted, margin, y);
            e.HasMorePages = false;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  LOGOUT & EXIT
        // ══════════════════════════════════════════════════════════════════════

        // Logout: go back to login screen
        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to log out?\nAny unsaved data will be lost.",
                "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var login = new LoginForm();
                login.Show();
                this.Close();
            }
        }

        // Exit: close the entire application
        private void BtnExit_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to exit?\nThe application will close completely.",
                "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                Application.Exit();
        }

        // Intercept the window X button — same confirm as Exit
        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show(
                    "Are you sure you want to close the application?\nAny unsaved data will be lost.",
                    "Confirm Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    e.Cancel = true;
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  HELPER METHODS
        // ══════════════════════════════════════════════════════════════════════

        private void UpdateDashboard()
        {
            if (lblDashVehicles == null) return;
            double tf = 0, tm = 0; int al = 0;
            foreach (DataRow r in usageTable.Rows)
            {
                tf += Convert.ToDouble(r["Fuel Used (L)"]);
                tm += Convert.ToDouble(r["Mileage (km)"]);
                if (Convert.ToDouble(r["Speed (km/h)"]) > 120) al++;
            }
            SetCard(lblDashVehicles, "VEHICLES", vehicleTable.Rows.Count.ToString());
            SetCard(lblDashDrivers, "DRIVERS", driverTable.Rows.Count.ToString());
            SetCard(lblDashFuel, "TOTAL FUEL (L)", $"{tf:F2}");
            SetCard(lblDashMileage, "MILEAGE (km)", $"{tm:F2}");
            SetCard(lblDashAlerts, "SPEED ALERTS", al.ToString());
        }

        private void SetCard(Label card, string title, string value) => card.Text = $"{title}\n\n{value}";

        private void RefreshDriverCombo()
        {
            cmbAssignedDriver.Items.Clear();
            cmbAssignedDriver.Items.Add("Unassigned");
            foreach (DataRow r in driverTable.Rows) cmbAssignedDriver.Items.Add(r["Full Name"]?.ToString() ?? "");
            cmbAssignedDriver.SelectedIndex = 0;
        }

        private void RefreshVehicleCombo()
        {
            cmbVehicleCapture.Items.Clear();
            cmbVehicleCapture.Items.Add("Select...");
            foreach (DataRow r in vehicleTable.Rows) cmbVehicleCapture.Items.Add(r["Vehicle ID"]?.ToString() ?? "");
            cmbVehicleCapture.SelectedIndex = 0;
        }

        private void RefreshFilterCombo()
        {
            if (cmbFilterVehicle == null) return;
            cmbFilterVehicle.Items.Clear();
            cmbFilterVehicle.Items.Add("All Vehicles");
            foreach (DataRow r in vehicleTable.Rows) cmbFilterVehicle.Items.Add(r["Vehicle ID"]?.ToString() ?? "");
            cmbFilterVehicle.SelectedIndex = 0;
        }

        private void ClearVehicleFields() { txtVehicleID.Clear(); txtModel.Clear(); cmbVehicleType.SelectedIndex = 0; cmbAssignedDriver.SelectedIndex = 0; }
        private void ClearDriverFields() { txtDriverID.Clear(); txtDriverName.Clear(); txtLicence.Clear(); txtContact.Clear(); }
        private void ClearCaptureFields() { txtFuel.Clear(); txtMileage.Clear(); txtSpeed.Clear(); txtNotes.Clear(); lblSpeedWarning.Text = ""; }

        private void ShowSuccess(string msg) => MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void ShowError(string msg) => MessageBox.Show(msg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private Label MakeStatCard(string title, string value, Color color) => new Label
        {
            Size = new Size(185, 100),
            BackColor = color,
            ForeColor = Color.White,
            Text = $"{title}\n\n{value}",
            Font = new Font("Segoe UI", 11f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            Margin = new Padding(8),
            Tag = title
        };

        private TabPage MakeTab(string title) => new TabPage { Text = title, BackColor = C_GREY, Padding = new Padding(10) };

        private Label MakeSectionLabel(string text) => new Label
        {
            Text = "   " + text,
            BackColor = C_DARK,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            Height = 34,
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.MiddleLeft
        };

        private Label MakeSummaryLabel(string text, Point loc) => new Label
        {
            Text = text,
            Location = loc,
            AutoSize = true,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9f, FontStyle.Bold)
        };

        private TextBox MakeInput(Panel parent, string labelText, ref int y, string hint = "")
        {
            var lbl = new Label { Text = labelText, Location = new Point(15, y), Size = new Size(150, 18), Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = C_DARK };
            y += 20;
            var txt = new TextBox { Location = new Point(15, y), Size = new Size(290, 26), Font = new Font("Segoe UI", 9.5f), BackColor = C_GREY };
            if (!string.IsNullOrEmpty(hint))
            {
                txt.ForeColor = Color.Gray; txt.Text = hint;
                txt.GotFocus += (s, e) => { if (txt.Text == hint) { txt.Text = ""; txt.ForeColor = Color.Black; } };
                txt.LostFocus += (s, e) => { if (string.IsNullOrEmpty(txt.Text)) { txt.Text = hint; txt.ForeColor = Color.Gray; } };
            }
            y += 34;
            parent.Controls.AddRange(new Control[] { lbl, txt });
            return txt;
        }

        private ComboBox MakeCombo(Panel parent, string labelText, string[] items, ref int y)
        {
            var lbl = new Label { Text = labelText, Location = new Point(15, y), Size = new Size(150, 18), Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = C_DARK };
            y += 20;
            var cmb = new ComboBox { Location = new Point(15, y), Size = new Size(290, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f), BackColor = C_GREY };
            cmb.Items.AddRange(items);
            if (cmb.Items.Count > 0) cmb.SelectedIndex = 0;
            y += 34;
            parent.Controls.AddRange(new Control[] { lbl, cmb });
            return cmb;
        }

        private Button MakeButton(string text, Color bg, Point loc) => new Button
        {
            Text = text,
            Location = loc,
            Size = new Size(290, 36),
            BackColor = bg,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            Cursor = Cursors.Hand
        };

        private DataGridView StyledGrid()
        {
            var g = new DataGridView
            {
                BackgroundColor = C_WHITE,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9f),
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = C_LIGHT }
            };
            g.ColumnHeadersDefaultCellStyle.BackColor = C_DARK;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            g.ColumnHeadersHeight = 34;
            g.RowTemplate.Height = 28;
            g.EnableHeadersVisualStyles = false;
            return g;
        }

        private void InitializeComponent() { this.SuspendLayout(); this.ResumeLayout(false); }
    }
}