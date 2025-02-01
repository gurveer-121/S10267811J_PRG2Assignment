//==========================================================
// Student Number : S10267811J
// Student Name : Gurveer Singh
// Partner Name : BOO yuan sheng
//==========================================================
using System;
    using System.Collections.Generic;

    public class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; } = new Dictionary<string, Airline>();
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
        public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new Dictionary<string, BoardingGate>();

        public bool AddAirline(Airline airline)
        {
            if (!Airlines.ContainsKey(airline.Code))
            {
                Airlines.Add(airline.Code, airline);
                return true;
            }
            return false;
        }

        public bool AddBoardingGate(BoardingGate gate)
        {
            if (!BoardingGates.ContainsKey(gate.GateName))
            {
                BoardingGates.Add(gate.GateName, gate);
                return true;
            }
            return false;
        }

        public Airline GetAirlineFromFlight(string flightNumber)
        {
            return Flights.ContainsKey(flightNumber) ? Flights[flightNumber].Airline : null;
        }

        public void AddFlight(Flight flight)
        {
            if (!Flights.ContainsKey(flight.FlightNumber))
            {
                Flights.Add(flight.FlightNumber, flight);
                if (flight.Airline != null)
                    flight.Airline.AddFlight(flight);
            }
        }

        public double CalculateFees(string airlineCode)
        {
            return Airlines.ContainsKey(airlineCode) ? Airlines[airlineCode].CalculateFees() : 0;
        }
    }
