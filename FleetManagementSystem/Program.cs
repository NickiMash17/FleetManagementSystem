// ═══════════════════════════════════════════════════════════════════════════════
//  Fleet Monitoring & Vehicle Management System
//  Entry Point
//  Apex Auto Solutions | SPM622 FA1
//  Developer: Nicolette Mashaba | Student No: 200232990
// ═══════════════════════════════════════════════════════════════════════════════

using System;
using System.Windows.Forms;

namespace FleetManagementSystem
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show login/splash before main form
            Application.Run(new LoginForm());
        }
    }
}