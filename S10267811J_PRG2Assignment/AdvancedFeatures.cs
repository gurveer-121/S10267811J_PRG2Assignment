//==========================================================
// Student Number : S10267811J
// Student Name : Gurveer Singh
// Partner Name : BOO Yuan Sheng
//==========================================================

using System;
using System.Collections.Generic;

public class AdvancedFeatures
{
    private Terminal terminal;

    //constructor to start advanced features
    public AdvancedFeatures(Terminal terminal)
    {
        this.terminal = terminal;
    }

    //Method a)process Unassigned Flights in Bulk
    public void ProcessUnassignedFlights()  //gurveer
    {
        Queue<Flight> unassignedFlights = new Queue<Flight>(); //q to store flights 
        int unassignedFlightsCount = 0; //counter for unasagined flight 
        int unassignedGatesCount = 0;   //counter for unassinged gates 

        //Identify unassigned flights and add them to quue
        foreach (var flight in terminal.Flights.Values)
        {
            if (flight.BoardingGate == null)
            {
                unassignedFlights.Enqueue(flight);
                unassignedFlightsCount++;
            }
        }

        //total no. available boardigng gates
        foreach (var gate in terminal.BoardingGates.Values)
        {
            if (gate.AssignedFlight == null)
            {
                unassignedGatesCount++;
            }
        }

        Console.WriteLine($"Total Unassigned Flights: {unassignedFlightsCount}");
        Console.WriteLine($"Total Unassigned Boarding Gates: {unassignedGatesCount}");

        int assignedFlightsCount = 0; //counter for assigned flights 

        //proces queeue
        while (unassignedFlights.Count > 0)
        {
            Flight flight = unassignedFlights.Dequeue();
            BoardingGate assignedGate = null;

            //Find a boarding gate
            foreach (var gate in terminal.BoardingGates.Values)
            {
                if (gate.AssignedFlight == null) //
                {
                    //if flight has a request find a gate
                    if (!string.IsNullOrEmpty(flight.SpecialRequestCode))
                    {
                        if ((flight.SpecialRequestCode == "DDJB" && gate.SupportsDDJB) ||
                            (flight.SpecialRequestCode == "CFFT" && gate.SupportsCFFT) ||
                            (flight.SpecialRequestCode == "LWTT" && gate.SupportsLWTT))
                        {
                            assignedGate = gate;
                            break;
                        }
                    }
                    else
                    {
                        //if no special requests assign a gate
                        assignedGate = gate;
                        break;
                    }
                }
            }

            //Assign gates
            if (assignedGate != null)
            {
                flight.BoardingGate = assignedGate;
                assignedGate.AssignedFlight = flight;
                assignedFlightsCount++;

                //final flight details
                Console.WriteLine("\nFlight Assigned:");
                Console.WriteLine($"Flight Number: {flight.FlightNumber}");
                Console.WriteLine($"Airline Name: {(flight.Airline != null ? flight.Airline.Name : "Unknown Airline")}");
                Console.WriteLine($"Origin: {flight.Origin}");
                Console.WriteLine($"Destination: {flight.Destination}");
                Console.WriteLine($"Expected Time: {flight.ExpectedTime:dd/MM/yyyy hh:mm tt}");
                Console.WriteLine($"Special Request Code: {flight.SpecialRequestCode}");
                Console.WriteLine($"Assigned Boarding Gate: {assignedGate.GateName}");
                Console.WriteLine("-----------------------------------------");
            }
        }

        //summary 
        Console.WriteLine($"Total Flights Processed: {unassignedFlightsCount}");
        Console.WriteLine($"Total Flights Assigned: {assignedFlightsCount}");
        Console.WriteLine($"Processing Completion: {((assignedFlightsCount * 100) / Math.Max(1, unassignedFlightsCount))}%");

        //show menu
        DisplayMainMenu();
    }


    //Method b) calculate + display total fees 
    public void DisplayTotalFeesPerAirline()  //yuansheng
    {
        //check for boarding gate
        foreach (var flight in terminal.Flights.Values)
        {
            if (flight.BoardingGate == null)
            {
                Console.WriteLine("Error: Some flights do not have a boarding gate assigned.");
                Console.WriteLine("Please ensure all flights have a gate before running this feature.");
                return;
            }
        }

        double totalFees = 0;        //total revenue 
        double totalDiscounts = 0;   //total discoutns 

        Console.WriteLine("\n=====================================");
        Console.WriteLine("Total Fees Per Airline for the Day");
        Console.WriteLine("=====================================");

        //fee for each airline 
        foreach (var airline in terminal.Airlines.Values)
        {
            double airlineFees = 0;
            double discount = 0;

            //total fees for airlines 
            foreach (var flight in airline.Flights.Values)
            {
                double flightFee = 0;

                //find out flight arriving/departing
                if (flight.Origin == "Singapore (SIN)")
                {
                    flightFee += 800; //departing flights 
                }
                else if (flight.Destination == "Singapore (SIN)")
                {
                    flightFee += 500; //arriving flights 
                }

                //additional charges for special requests
                if (flight is DDJBIFlight) flightFee += 900;
                else if (flight is CFFTFlight) flightFee += 700;
                else if (flight is LWTTFlight) flightFee += 500 + ((LWTTFlight)flight).RequestFee;
                else flightFee += 500; //base flight fee 

                //gate base fee
                flightFee += 300;

                airlineFees += flightFee;
            }

            //promotonial discounts
            if (airline.Flights.Count > 5)
            {
                discount = airlineFees * 0.10;
                airlineFees -= discount;
            }

            //Display fee breakdown
            Console.WriteLine($"Airline: {airline.Name}");
            Console.WriteLine($"Total Fees Before Discount: ${airlineFees + discount:F2}");
            Console.WriteLine($"Discount Applied: -${discount:F2}");
            Console.WriteLine($"Final Fees: ${airlineFees:F2}");
            Console.WriteLine("-----------------------------------------");

            //update totals
            totalFees += airlineFees;
            totalDiscounts += discount;
        }

        //display summary
        Console.WriteLine("\n=====================================");
        Console.WriteLine("Final Summary for Terminal 5");
        Console.WriteLine("=====================================");
        Console.WriteLine($"Total Fees Before Discounts: ${totalFees + totalDiscounts:F2}");
        Console.WriteLine($"Total Discounts Applied: -${totalDiscounts:F2}");
        Console.WriteLine($"Final Fees Collected: ${totalFees:F2}");
        Console.WriteLine($"Discount Percentage: {(totalDiscounts / Math.Max(1, totalFees + totalDiscounts)) * 100:F2}%");

        //Show the menu
        DisplayMainMenu();
    }

    //mainmenu method
    private void DisplayMainMenu()
    {
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
        Console.WriteLine("8. Process Unassigned Flights");
        Console.WriteLine("9. Display Total Fees Per Airline");
        Console.WriteLine("0. Exit");
        Console.Write("Please select your option: ");
    }
}
