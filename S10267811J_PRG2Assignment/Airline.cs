//==========================================================
// Student Number : S10267811J
// Student Name : Gurveer Singh
// Partner Name : BOO yuan sheng
//==========================================================
using System;
using System.Collections.Generic;

public class Airline
{
    //airline full name
    public string Name { get; set; }

    //get airline codde
    public string Code { get; set; }

    //stores flights by using airline key
    public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();

    //method to add a flight to the airline
    public void AddFlight(Flight flight)
    {
        // Ensure we don't add duplicate flights
        if (!Flights.ContainsKey(flight.FlightNumber))
        {
            Flights.Add(flight.FlightNumber, flight);
        }
    }

    //removes a flight from the airline's records
    public bool RemoveFlight(string flightNumber)
    {
        return Flights.Remove(flightNumber);  //returns true if works
    }

    //Calculates total fees for all flights for each airline 
    public double CalculateFees()
    {
        double totalFees = 0;

        //Loop through each flight and add fees
        foreach (var flight in Flights.Values)
        {
            totalFees += flight.CalculateFees();
        }
        return totalFees; //Return the total
    }
}
