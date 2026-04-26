# Fleet Monitoring & Vehicle Management System
**Apex Auto Solutions | SPM622 Formative Assessment 1**

![C#](https://img.shields.io/badge/C%23-12-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![Platform](https://img.shields.io/badge/Platform-Windows-blue.svg)
![License](https://img.shields.io/badge/License-Academic-green.svg)

## 📌 Overview
A professional-grade desktop application designed for **Apex Auto Solutions** to streamline complex fleet operations. Built using the latest **.NET 8** framework and **Windows Forms**, this system serves as a centralized command center for asset management, driver tracking, and operational safety.

## 🚀 Key Features
### 📊 Live Dashboard
Instant visual summary of fleet health, including vehicle count, active drivers, cumulative fuel consumption, and safety alert tallies.

### 🚛 Asset & Human Resource Management
Full CRUD (Create, Read, Update, Delete) capabilities for both vehicle and driver entities, featuring dynamic assignment logic and real-time UI synchronization.

### 🛰️ Intelligent Data Capture
Unified input module for logging critical trip data (fuel used, mileage, speed) with automated validation and real-time feedback.

### ⚠️ Safety & Compliance
Integrated safety monitoring that triggers real-time visual warnings and automated manager notifications when the **120 km/h** speed limit is breached.

### 📑 Professional Reporting Engine
*   **On-Screen Filtering:** Dynamic data slicing based on specific Vehicle IDs.
*   **CSV Export:** Seamless integration with Excel and external BI tools.
*   **Advanced Printing:** Custom `PrintDocument` logic featuring branded headers, summary statistics, and multi-page support.

### 🔐 Secure Access
Hardened authentication gate requiring validated administrative credentials to access sensitive fleet data.

## � Technical Stack
- **Language:** C# 12
- **Framework:** .NET 8.0 (Windows Forms)
- **Data Management:** High-performance In-Memory `DataTables` with relational-style lookup logic.
- **Null Safety:** Implemented **C# Nullable Reference Types** across all modules to eliminate `NullReferenceException` risks.
- **UI/UX:** Custom Teal/Dark/Accent color palette designed for high-contrast visibility and reduced eye strain during long-shift monitoring.

## 🏗️ Architecture & Best Practices
- **Constant Management:** Centralized business rules (e.g., `SPEED_LIMIT_KMH`) for easy maintainability.
- **Validation Layer:** Robust error handling and user input sanitization to ensure data integrity.
- **Event-Driven Design:** Decoupled UI updates triggered by state changes in the underlying data layer.

<!-- 
## 📸 Screenshots
Add your screenshots here:
!Login Screen
!Dashboard
!Reports
-->

## 💻 Getting Started
### Prerequisites
- .NET 8.0 SDK

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/NickiMash17/FleetManagementSystem.git
   ```
2. Navigate to the source folder:
   ```powershell
   cd FleetManagementSystem/FleetManagementSystem
   ```
3. Build and run:
   ```powershell
   dotnet run
   ```

## 🔑 Access Credentials
- **Username:** `admin`
- **Password:** `fleet2026`

## 🗺️ Roadmap
- [ ] Persistent Database Integration (SQLite/SQL Server).
- [ ] Real-time GPS Telemetry simulation.
- [ ] Driver performance scoring and leaderboard.
- [ ] Automated maintenance scheduling alerts.

## 👤 Developer Information
- **Name:** Nicolette Mashaba
- **Student Number:** 20232990
- **Module:** SPM622 - Formative Assessment 1