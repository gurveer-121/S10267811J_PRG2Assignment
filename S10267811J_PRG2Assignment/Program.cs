//==========================================================
// Student Number : S10267811J
// Student Name : Gurveer Singh
// Partner Name : BOO yuan sheng
//==========================================================
using System;
using System.Collections.Generic;
using System.IO;


class Program
{
    static Terminal terminal = new Terminal();
    static AdvancedFeatures advancedFeatures = new AdvancedFeatures(terminal);  

        static void Main()
        {
            Console.WriteLine("Loading Airlines...");
            LoadAirlinesAndBoardingGates();

            Console.WriteLine("Loading Flights...");
            LoadFlights();

            Console.WriteLine("=============================================");
            Console.WriteLine("Welcome to Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine("1. List All Flights");
            Console.WriteLine("2. List Boarding Gates");
            Console.WriteLine("3. Assign a Boarding Gate to a Flight");
            Console.WriteLine("4. Create Flight");
            Console.WriteLine("5. Display Airline Flights");
            Console.WriteLine("6. Modify Flight Details");
            Console.WriteLine("7. Display Flight Schedule");
            Console.WriteLine("0. Exit");
            Console.WriteLine("8. Process Unassigned Flights");
            Console.WriteLine("9. Display Total Fees Per Airline");
            Console.Write("Please select your option: ");

            while (true)
            {
                string choice = Console.ReadLine();
                Console.WriteLine(); //ensure correct formatting

                switch (choice)
                {
                    case "1":
                        ListAllFlights(); //gurveer
                        break;
                    case "2":
                        ListAllBoardingGates(); //yuansheng
                        break;
                    case "3":
                        AssignBoardingGateToFlight(); //yuansheng
                        break;
                    case "4":
                        CreateNewFlight(); //gurveer
                        break;
                    case "5":
                        DisplayFullFlightDetails(); //yuansheng
                        break;
                    case "6":
                        ModifyFlightDetails(); //yuansheng
                        break;
                    case "7":
                        DisplayScheduledFlights(); //gurveer
                        break;
                    case "8":
                        advancedFeatures.ProcessUnassignedFlights(); //gurveer
                        break;
                    case "9":
                        advancedFeatures.DisplayTotalFeesPerAirline(); //yuansheng
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.Write("Invalid choice. Please enter a valid option: ");
                        break;
                }
            }
        }

        //load Airlines n Boarding Gates from CSV
        static void LoadAirlinesAndBoardingGates()
        {
            int airlineCount = 0;
            try
            {
                using (StreamReader reader = new StreamReader("airlines.csv"))
                {
                    string line;
                    reader.ReadLine(); // Skip header
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] data = line.Split(',');
                        if (data.Length == 2)
                        {
                            Airline airline = new Airline { Name = data[0], Code = data[1] };
                            terminal.AddAirline(airline);
                            airlineCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading airlines: " + ex.Message);
                return;
            }
            Console.WriteLine($"{airlineCount} Airlines Loaded!");

            int gateCount = 0;
            try
            {
                using (StreamReader reader = new StreamReader("boardinggates.csv"))
                {
                    string line = reader.ReadLine(); //read header
                    string[] headers = line.Split(',');

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] data = line.Split(',');
                        if (data.Length == headers.Length)
                        {
                            BoardingGate gate = new BoardingGate
                            {
                                GateName = data[0],
                                SupportsDDJB = data[1].Trim().ToLower() == "true",
                                SupportsCFFT = data[2].Trim().ToLower() == "true",
                                SupportsLWTT = data[3].Trim().ToLower() == "true"
                            };
                            terminal.AddBoardingGate(gate);
                            gateCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading boarding gates: " + ex.Message);
                return;
            }
            Console.WriteLine($"{gateCount} Boarding Gates Loaded!");
        }

        //load Flights from CSV
        static void LoadFlights()
        {
            int flightCount = 0;
            try
            {
                using (StreamReader reader = new StreamReader("flights.csv"))
                {
                    reader.ReadLine(); // Skip header
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] data = line.Split(',');

                        if (data.Length >= 4)
                        {
                            string flightNumber = data[0].Trim();
                            string origin = data[1].Trim();
                            string destination = data[2].Trim();
                            string expectedTimeStr = data[3].Trim();
                            string specialRequest = data.Length > 4 ? data[4].Trim() : "";

                            DateTime expectedTime;
                            if (!DateTime.TryParse(expectedTimeStr, out expectedTime))
                            {
                                Console.WriteLine($"Skipping flight {flightNumber} due to invalid time format.");
                                continue;
                            }

                            Flight flight;
                            switch (specialRequest)
                            {
                                case "DDJB":
                                    flight = new DDJBIFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime };
                                    break;
                                case "CFFT":
                                    flight = new CFFTFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime };
                                    break;
                                case "LWTT":
                                    flight = new LWTTFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime };
                                    break;
                                default:
                                    flight = new NORMFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime };
                                    break;
                            }

                        if (terminal.Airlines.ContainsKey(flightNumber.Substring(0, 2)))
                        {
                            flight.Airline = terminal.Airlines[flightNumber.Substring(0, 2)]; // ✅ Ensure airline is assigned
                            terminal.AddFlight(flight);
                        }
                        else
                        {
                            Console.WriteLine($"⚠ Warning: Skipping flight {flightNumber} - Airline not found.");
                        }

                    }
                }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading flights: " + ex.Message);
                return;
            }
            Console.WriteLine($"{flightCount} Flights Loaded!");
        }

        //list all Flights
        static void ListAllFlights()
        {
            Console.WriteLine("======================================");
            Console.WriteLine("List of Flights for Changi Airport Terminal 5");
            Console.WriteLine("======================================");

            //column names
            Console.WriteLine("{0,-20} {1,-30} {2,-25} {3,-25} {4,-10}",
                              "Flight Number", "Airline Name", "Origin", "Destination", "Expected ");
            Console.WriteLine("{0,-20} {1,-30} {2,-25} {3,-25} {4,-10}",
                              "Departure/Arrival Time", "", "", "", "");


            // Listing all flights
            foreach (var flight in terminal.Flights.Values)
            {
                string airlineName = flight.Airline != null ? flight.Airline.Name : "Unknown Airline";

                //first row
                string flightInfo = $"{flight.FlightNumber,-20} {airlineName,-30} {flight.Origin,-25} {flight.Destination,-25} {flight.ExpectedTime:dd/MM/yyyy}";
                Console.WriteLine(flightInfo);

                //second row
                string timeInfo = $"{flight.ExpectedTime:hh:mm tt}";  //just for the time
                Console.WriteLine($"{timeInfo,-20}");  //allight to left

                Console.WriteLine(); //spacing between each line                             
            }

            //disply menu
            Console.WriteLine("=============================================");
            Console.WriteLine("Welcome to Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine("1. List All Flights");
            Console.WriteLine("2. List Boarding Gates");
            Console.WriteLine("3. Assign a Boarding Gate to a Flight");
            Console.WriteLine("4. Create Flight");
            Console.WriteLine("5. Display Airline Flights");
            Console.WriteLine("6. Modify Flight Details");
            Console.WriteLine("7. Display Flight Schedule");
            Console.WriteLine("0. Exit");
            Console.WriteLine("8. Process Unassigned Flights");
            Console.WriteLine("9. Display Total Fees Per Airline");
            Console.Write("Please select your option: ");
        }

        //4)List all Boarding Gates 
        static void ListAllBoardingGates()
        {
            Console.WriteLine("======================================");
            Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
            Console.WriteLine("======================================");

            //column widths
            int columnWidth = 20;

            //headow row spacing 
            Console.WriteLine("{0,-" + columnWidth + "} {1,-" + columnWidth + "} {2,-" + columnWidth + "} {3,-" + columnWidth + "}",
                              "Gate Name", "DDJB", "CFFT", "LWTT");


            //repeat for every line 
            foreach (var gate in terminal.BoardingGates.Values)
            {
                Console.WriteLine("{0,-" + columnWidth + "} {1,-" + columnWidth + "} {2,-" + columnWidth + "} {3,-" + columnWidth + "}",
                                  gate.GateName, gate.SupportsDDJB, gate.SupportsCFFT, gate.SupportsLWTT);
            }

            //display the menu again
            Console.WriteLine("=============================================");
            Console.WriteLine("Welcome to Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine("1. List All Flights");
            Console.WriteLine("2. List Boarding Gates");
            Console.WriteLine("3. Assign a Boarding Gate to a Flight");
            Console.WriteLine("4. Create Flight");
            Console.WriteLine("5. Display Airline Flights");
            Console.WriteLine("6. Modify Flight Details");
            Console.WriteLine("7. Display Flight Schedule");
            Console.WriteLine("0. Exit");
            Console.WriteLine("8. Process Unassigned Flights");
            Console.WriteLine("9. Display Total Fees Per Airline");
            Console.Write("Please select your option: ");
        }

        // 5️)Assign Boarding Gate to Flight
       
        static void AssignBoardingGateToFlight()
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("Assign a Boarding Gate to a Flight");
            Console.WriteLine("=============================================");

            //prompt for Flight Number
            Console.Write("Enter Flight Number: ");
            string flightNumber = Console.ReadLine().Trim().ToUpper();

            //check if flights exist
            if (!terminal.Flights.ContainsKey(flightNumber))
            {
                Console.WriteLine("Error: Flight not found. Please enter a valid Flight Number.");
                return;
            }

            Flight flight = terminal.Flights[flightNumber];

            // Prompt for Boarding Gate Name
            Console.Write("Enter Boarding Gate Name: ");
            string gateName = Console.ReadLine().Trim().ToUpper();

            // Check if Boarding Gate Exists
            if (!terminal.BoardingGates.ContainsKey(gateName))
            {
                Console.WriteLine("Error: Boarding Gate not found. Please enter a valid Gate Name.");
                return;
            }

            BoardingGate gate = terminal.BoardingGates[gateName];

            // Check if Gate is Already Assigned to a Flight
            if (gate.AssignedFlight != null)
            {
                Console.WriteLine("Error: This Boarding Gate is already assigned to another flight. Please choose another gate.");
                return;
            }

            // Assign the Boarding Gate to the Flight
            gate.AssignedFlight = flight;
            flight.BoardingGate = gate;

            // Display Flight & Boarding Gate Details
            Console.WriteLine("\nFlight Number: " + flight.FlightNumber);
            Console.WriteLine("Origin: " + flight.Origin);
            Console.WriteLine("Destination: " + flight.Destination);
            Console.WriteLine("Expected Time: " + flight.ExpectedTime.ToString("dd/M/yyyy h:mm:ss tt"));
            Console.WriteLine("Special Request Code: None"); // Currently, no special request handling

            Console.WriteLine("\nBoarding Gate Name: " + gate.GateName);
            Console.WriteLine("Supports DDJB: " + gate.SupportsDDJB);
            Console.WriteLine("Supports CFFT: " + gate.SupportsCFFT);
            Console.WriteLine("Supports LWTT: " + gate.SupportsLWTT);

            // Prompt to Update Flight Status
            Console.Write("\nWould you like to update the status of the flight? (Y/N): ");
            string updateStatus = Console.ReadLine().Trim().ToUpper();

            if (updateStatus == "Y")
            {
                Console.WriteLine("1. Delayed");
                Console.WriteLine("2. Boarding");
                Console.WriteLine("3. On Time");
                Console.Write("Please select the new status of the flight: ");
                string statusChoice = Console.ReadLine().Trim();

                switch (statusChoice)
                {
                    case "1":
                        flight.Status = "Delayed";
                        break;
                    case "2":
                        flight.Status = "Boarding";
                        break;
                    case "3":
                        flight.Status = "On Time";
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Setting status to 'On Time'.");
                        flight.Status = "On Time";
                        break;
                }
            }
            else
            {
                flight.Status = "On Time"; // Default status
            }

            // Confirm Assignment
            Console.WriteLine($"\nFlight {flight.FlightNumber} has been assigned to Boarding Gate {gate.GateName}!");

            // Display the menu again
            Console.WriteLine("=============================================");
            Console.WriteLine("Welcome to Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine("1. List All Flights");
            Console.WriteLine("2. List Boarding Gates");
            Console.WriteLine("3. Assign a Boarding Gate to a Flight");
            Console.WriteLine("4. Create Flight");
            Console.WriteLine("5. Display Airline Flights");
            Console.WriteLine("6. Modify Flight Details");
            Console.WriteLine("7. Display Flight Schedule");
            Console.WriteLine("0. Exit");
            Console.WriteLine("8. Process Unassigned Flights");
            Console.WriteLine("9. Display Total Fees Per Airline");
            Console.Write("Please select your option: ");
        }


        // 6️⃣ Create a New Flight
        static void CreateNewFlight()
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("Create a New Flight");
            Console.WriteLine("=============================================");

            // Prompt for Flight Number
            Console.Write("Enter Flight Number: ");
            string flightNumber = Console.ReadLine().Trim().ToUpper();

            // Prompt for Origin
            Console.Write("Enter Origin: ");
            string origin = Console.ReadLine().Trim();

            // Prompt for Destination
            Console.Write("Enter Destination: ");
            string destination = Console.ReadLine().Trim();

            // Prompt for Expected Departure/Arrival Time
            Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
            string expectedTimeStr = Console.ReadLine().Trim();
            DateTime expectedTime;
            while (!DateTime.TryParseExact(expectedTimeStr, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out expectedTime))
            {
                Console.Write("Invalid date format. Please enter the time again (dd/mm/yyyy hh:mm): ");
                expectedTimeStr = Console.ReadLine().Trim();
            }

            // Prompt for Special Request Code (Optional)
            Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
            string specialRequestCode = Console.ReadLine().Trim().ToUpper();
            if (specialRequestCode != "CFFT" && specialRequestCode != "DDJB" && specialRequestCode != "LWTT" && specialRequestCode != "NONE")
            {
                specialRequestCode = "NONE"; // Default if the user enters something invalid
            }

            // Create the corresponding Flight object
            Flight newFlight;
            switch (specialRequestCode)
            {
                case "CFFT":
                    newFlight = new CFFTFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime };
                    break;
                case "DDJB":
                    newFlight = new DDJBIFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime };
                    break;
                case "LWTT":
                    newFlight = new LWTTFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime };
                    break;
                default:
                    newFlight = new NORMFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime };
                    break;
            }

        // Assign Airline based on flight number prefix
        string airlineCode = flightNumber.Substring(0, 2);
        if (terminal.Airlines.ContainsKey(airlineCode))
        {
            newFlight.Airline = terminal.Airlines[airlineCode];
            terminal.Airlines[airlineCode].AddFlight(newFlight);
        }
        else
        {
            Console.WriteLine($"⚠ Warning: Airline not found for Flight {flightNumber}. Flight will not be assigned to an airline.");
        }

        // Add the new Flight to the dictionary
        terminal.AddFlight(newFlight);


        // Append the new flight to the flights.csv file
        try
        {
                using (StreamWriter writer = new StreamWriter("flights.csv", true))
                {
                    writer.WriteLine($"{flightNumber},{origin},{destination},{expectedTime:dd/MM/yyyy HH:mm},{specialRequestCode}");
                }
                Console.WriteLine($"Flight {flightNumber} has been added!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving the flight to the file: {ex.Message}");
            }

            // Ask if the user wants to add another flight
            Console.Write("Would you like to add another flight? (Y/N): ");
            string addAnother = Console.ReadLine().Trim().ToUpper();

            if (addAnother == "Y")
            {
                // Recursively call this method to add another flight
                CreateNewFlight();
            }
            else
            {
                // Display the menu again
                Console.WriteLine("=============================================");
                Console.WriteLine("Welcome to Changi Airport Terminal 5");
                Console.WriteLine("=============================================");
                Console.WriteLine("1. List All Flights");
                Console.WriteLine("2. List Boarding Gates");
                Console.WriteLine("3. Assign a Boarding Gate to a Flight");
                Console.WriteLine("4. Create Flight");
                Console.WriteLine("5. Display Airline Flights");
                Console.WriteLine("6. Modify Flight Details");
                Console.WriteLine("7. Display Flight Schedule");
                Console.WriteLine("0. Exit");
                Console.WriteLine("8. Process Unassigned Flights");
                Console.WriteLine("9. Display Total Fees Per Airline");
                Console.Write("Please select your option: ");
            }
        }

    // 7️⃣ Display Full Flight Details
    static void DisplayFullFlightDetails()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");

        // List all Airlines
        foreach (var airline in terminal.Airlines.Values)
        {
            Console.WriteLine("{0,-15} {1,-30}", airline.Code, airline.Name);
        }

        // Prompt user to select an Airline
        Console.Write("Enter Airline Code: ");
        string airlineCode = Console.ReadLine().Trim().ToUpper();

        // Check if the airline code exists
        if (!terminal.Airlines.ContainsKey(airlineCode))
        {
            Console.WriteLine("Error: Airline not found. Please enter a valid Airline Code.");
            return;
        }

        Airline selectedAirline = terminal.Airlines[airlineCode];

        Console.WriteLine("=============================================");
        Console.WriteLine("List of Flights for " + selectedAirline.Name);
        Console.WriteLine("=============================================");

        // Display all Flights for the selected airline
        Console.WriteLine("{0,-20} {1,-30} {2,-25} {3,-25} {4,-10}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected ");
        Console.WriteLine("{0,-20} {1,-30} {2,-25} {3,-25} {4,-10}", "Departure/Arrival Time", "", "", "", "");

        // Loop through the flights of the selected airline and display the flight details
        foreach (var flight in terminal.Flights.Values)
        {
            // ✅ Fix: Ensure `flight.Airline` is not null before accessing `.Code`
            if (flight.Airline != null && flight.Airline.Code == airlineCode)
            {
                string flightInfo = $"{flight.FlightNumber,-20} {flight.Airline.Name,-30} {flight.Origin,-25} {flight.Destination,-25} {flight.ExpectedTime:dd/MM/yyyy}";
                Console.WriteLine(flightInfo);

                // Print expected departure/arrival time on the same line as the header
                string timeInfo = $"{flight.ExpectedTime:hh:mm tt}";
                Console.WriteLine($"{timeInfo,-20}");
            }
            else if (flight.Airline == null) // 🚨 Debugging Info (Optional)
            {
                Console.WriteLine($"Warning: Flight {flight.FlightNumber} has no airline assigned.");
            }
        }

        // Display the menu again so the user can select another option
        Console.WriteLine("=============================================");
        Console.WriteLine("Welcome to Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("1. List All Flights");
        Console.WriteLine("2. List Boarding Gates");
        Console.WriteLine("3. Assign a Boarding Gate to a Flight");
        Console.WriteLine("4. Create Flight");
        Console.WriteLine("5. Display Airline Flights");
        Console.WriteLine("6. Modify Flight Details");
        Console.WriteLine("7. Display Flight Schedule");
        Console.WriteLine("0. Exit");
        Console.WriteLine("8. Process Unassigned Flights");
        Console.WriteLine("9. Display Total Fees Per Airline");
        Console.Write("Please select your option: ");
    }


    // 8️⃣ Modify Flight Details
    static void ModifyFlightDetails()
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");

            // List all Airlines
            foreach (var airline in terminal.Airlines.Values)
            {
                Console.WriteLine("{0,-15} {1,-30}", airline.Code, airline.Name);
            }

            // Prompt user to select an Airline
            Console.Write("Enter Airline Code: ");
            string airlineCode = Console.ReadLine().Trim().ToUpper();

            // Check if the airline code exists
            if (!terminal.Airlines.ContainsKey(airlineCode))
            {
                Console.WriteLine("Error: Airline not found. Please enter a valid Airline Code.");
                return;
            }

            Airline selectedAirline = terminal.Airlines[airlineCode];

            Console.WriteLine("=============================================");
            Console.WriteLine("List of Flights for " + selectedAirline.Name);
            Console.WriteLine("=============================================");

            // Display all Flights for the selected airline
            Console.WriteLine("{0,-20} {1,-30} {2,-25} {3,-25} {4,-10}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected ");
            Console.WriteLine("{0,-20} {1,-30} {2,-25} {3,-25} {4,-10}", "Departure/Arrival Time", "", "", "", "");

            // Loop through the flights of the selected airline and display the flight details
            foreach (var flight in terminal.Flights.Values)
            {
            if (flight.Airline != null && flight.Airline.Code == airlineCode)
            {
                string flightInfo = $"{flight.FlightNumber,-20} {flight.Airline.Name,-30} {flight.Origin,-25} {flight.Destination,-25} {flight.ExpectedTime:dd/MM/yyyy}";
                Console.WriteLine(flightInfo);
            }
            else if (flight.Airline == null)
            {
                Console.WriteLine($"⚠ Warning: Flight {flight.FlightNumber} does not have an associated airline.");
            

            // Print expected departure/arrival time on the same line as the header
            string timeInfo = $"{flight.ExpectedTime:hh:mm tt}";
                    Console.WriteLine($"{timeInfo,-20}");
                }
            }

            // Prompt user to choose a Flight to modify or delete
            Console.Write("Choose an existing Flight to modify or delete: ");
            string flightNumber = Console.ReadLine().Trim().ToUpper();

            // Check if the Flight Number exists
            if (!terminal.Flights.ContainsKey(flightNumber))
            {
                Console.WriteLine("Error: Flight not found. Please enter a valid Flight Number.");
                return;
            }

            Flight selectedFlight = terminal.Flights[flightNumber];

            // Prompt for the user to select modify or delete
            Console.WriteLine("1. Modify Flight");
            Console.WriteLine("2. Delete Flight");
            Console.Write("Choose an option: ");
            string option = Console.ReadLine().Trim();

            if (option == "1")
            {
                // Modify Flight
                Console.WriteLine("1. Modify Basic Information");
                Console.WriteLine("2. Modify Status");
                Console.WriteLine("3. Modify Special Request Code");
                Console.WriteLine("4. Modify Boarding Gate");
                Console.Write("Choose an option: ");
                string modifyOption = Console.ReadLine().Trim();

                switch (modifyOption)
                {
                    case "1": // Modify Basic Information
                        Console.Write("Enter new Origin: ");
                        selectedFlight.Origin = Console.ReadLine().Trim();

                        Console.Write("Enter new Destination: ");
                        selectedFlight.Destination = Console.ReadLine().Trim();

                        Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                        string newTimeStr = Console.ReadLine().Trim();
                        DateTime newTime;
                        while (!DateTime.TryParseExact(newTimeStr, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out newTime))
                        {
                            Console.Write("Invalid date format. Please enter the time again (dd/mm/yyyy hh:mm): ");
                            newTimeStr = Console.ReadLine().Trim();
                        }
                        selectedFlight.ExpectedTime = newTime;
                        break;

                    case "2": // Modify Status
                        Console.WriteLine("1. Delayed");
                        Console.WriteLine("2. Boarding");
                        Console.WriteLine("3. On Time");
                        Console.Write("Please select the new status of the flight: ");
                        string statusChoice = Console.ReadLine().Trim();

                        switch (statusChoice)
                        {

                            case "1":
                                selectedFlight.Status = "Delayed";
                                break;
                            case "2":
                                selectedFlight.Status = "Boarding";
                                break;
                            case "3":
                                selectedFlight.Status = "On Time";
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Setting status to 'On Time'.");
                                selectedFlight.Status = "On Time";
                                break;
                        }
                        break;

                    case "3": // Modify Special Request Code
                        Console.Write("Enter new Special Request Code (CFFT/DDJB/LWTT/None): ");
                        selectedFlight.SpecialRequestCode = Console.ReadLine().Trim().ToUpper();
                        break;

                    case "4": // Modify Boarding Gate
                        Console.Write("Enter new Boarding Gate (if any): ");
                        string gateName = Console.ReadLine().Trim().ToUpper();
                        if (terminal.BoardingGates.ContainsKey(gateName))
                        {
                            selectedFlight.BoardingGate = terminal.BoardingGates[gateName];
                        }
                        else
                        {
                            Console.WriteLine("Invalid Boarding Gate.");
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid option selected.");
                        break;


                }

                // Confirm the update
                Console.WriteLine("Flight updated!");
                Console.WriteLine("Flight Number: " + selectedFlight.FlightNumber);
                Console.WriteLine("Airline Name: " + selectedFlight.Airline.Name);
                Console.WriteLine("Origin: " + selectedFlight.Origin);
                Console.WriteLine("Destination: " + selectedFlight.Destination);
                Console.WriteLine("Expected Departure/Arrival Time: " + selectedFlight.ExpectedTime.ToString("dd/MM/yyyy h:mm:ss tt"));
                Console.WriteLine("Status: " + selectedFlight.Status);
                Console.WriteLine("Special Request Code: " + (string.IsNullOrEmpty(selectedFlight.SpecialRequestCode) ? "None" : selectedFlight.SpecialRequestCode));
                Console.WriteLine("Boarding Gate: " + (selectedFlight.BoardingGate != null ? selectedFlight.BoardingGate.GateName : "None"));
            }
            else if (option == "2")
            {
                // Delete Flight
                Console.Write($"Are you sure you want to delete flight {selectedFlight.FlightNumber}? (Y/N): ");
                string confirmDelete = Console.ReadLine().Trim().ToUpper();

                if (confirmDelete == "Y")
                {
                    terminal.Flights.Remove(flightNumber);
                    Console.WriteLine($"Flight {flightNumber} has been deleted.");
                }
            }

            // Display the menu again
            // Display the menu again so the user can select another option
            Console.WriteLine("=============================================");
            Console.WriteLine("Welcome to Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine("1. List All Flights");
            Console.WriteLine("2. List Boarding Gates");
            Console.WriteLine("3. Assign a Boarding Gate to a Flight");
            Console.WriteLine("4. Create Flight");
            Console.WriteLine("5. Display Airline Flights");
            Console.WriteLine("6. Modify Flight Details");
            Console.WriteLine("7. Display Flight Schedule");
            Console.WriteLine("0. Exit");
            Console.WriteLine("8. Process Unassigned Flights");
            Console.WriteLine("9. Display Total Fees Per Airline");
            Console.Write("Please select your option: ");
        }

        // 9️⃣ Display Scheduled Flights
        static void DisplayScheduledFlights()
        {
            // Sort all the flights by expected time
            List<Flight> sortedFlights = new List<Flight>(terminal.Flights.Values);
            sortedFlights.Sort();  // This sorts the flights based on ExpectedTime

            Console.WriteLine("=============================================");
            Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");

            // Header Row for the flight schedule
            Console.WriteLine("{0,-20} {1,-25} {2,-20} {3,-20} {4,-15}",
                              "Flight Number", "Airline Name", "Origin", "Destination", "Expected");
            Console.WriteLine("{0,-25} {1,-15} {2,-15}",
                              "Departure/Arrival Time", "Status", "Boarding Gate");


            // Display each flight in sorted order
            foreach (var flight in sortedFlights)
            {
                string airlineName = flight.Airline != null ? flight.Airline.Name : "Unknown Airline";
                string boardingGate = flight.BoardingGate != null ? flight.BoardingGate.GateName : "Unassigned";
                string status = flight.Status == "On Time" ? "Scheduled" : flight.Status;

                // First row with flight details
                Console.WriteLine("{0,-20} {1,-25} {2,-20} {3,-20} {4,-15}",
                                  flight.FlightNumber, airlineName, flight.Origin, flight.Destination,
                                  flight.ExpectedTime.ToString("dd/MM/yyyy"));

                // Second row with departure/arrival time, status, and boarding gate in correct order
                Console.WriteLine("{0,-25} {1,-15} {2,-15}",
                                  flight.ExpectedTime.ToString("hh:mm:ss tt"), status, boardingGate);
            }


            // Display the menu again so the user can select another option
            Console.WriteLine("=============================================");
            Console.WriteLine("Welcome to Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine("1. List All Flights");
            Console.WriteLine("2. List Boarding Gates");
            Console.WriteLine("3. Assign a Boarding Gate to a Flight");
            Console.WriteLine("4. Create Flight");
            Console.WriteLine("5. Display Airline Flights");
            Console.WriteLine("6. Modify Flight Details");
            Console.WriteLine("7. Display Flight Schedule");
            Console.WriteLine("0. Exit");
            Console.WriteLine("8. Process Unassigned Flights");
            Console.WriteLine("9. Display Total Fees Per Airline");
            Console.Write("Please select your option: ");
            

        }



    }
