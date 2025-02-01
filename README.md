# ✈️ Changi Airport Terminal 5 Flight Management System

## 📌 Overview
This project is a **C# console-based Flight Management System** developed for **Changi Airport Terminal 5**. It is designed to **manage flights, airlines, and boarding gates efficiently**. The system allows users to **list, create, modify, and assign flights** while also supporting **bulk flight processing and fee calculation**. 

The program interacts with **CSV files** to store flight and airline data, ensuring **persistence across multiple runs**.

---

## 🛠️ Features & Functionalities
This system is divided into **Basic** and **Advanced** features.

### 🟢 **Basic Features**
1️⃣ **List All Flights** – Displays all flights along with their details.  
2️⃣ **List Boarding Gates** – Shows all available boarding gates and their current status.  
3️⃣ **Assign a Boarding Gate to a Flight** – Manually assign a boarding gate to a flight.  
4️⃣ **Create Flight** – Allows users to create and add a new flight to the system.  
5️⃣ **Display Airline Flights** – View all flights belonging to a specific airline.  
6️⃣ **Modify Flight Details** – Update flight details or delete a flight from the system.  
7️⃣ **Display Flight Schedule** – Displays a sorted list of scheduled flights.  

### 🔵 **Advanced Features**
8️⃣ **Process Unassigned Flights in Bulk**  
   - Automatically assigns flights that **do not have a boarding gate**.  
   - Takes into account **special request codes** when assigning gates.  
   - Provides a **summary report** on how many flights were assigned.  

9️⃣ **Display Total Fees Per Airline**  
   - Computes and displays the **total operational fees** per airline.  
   - Includes **boarding gate fees, departure/arrival charges, and special request fees**.  
   - Applies **discounts for airlines with a high number of flights**.  
   - Displays a **final summary** of the total fees collected by the terminal.  

---

## 🛠️ Installation & Setup
1. **Clone the repository** or download the project files manually.
   ```sh
   git clone https://github.com/your-repo/FlightManagementSystem.git
