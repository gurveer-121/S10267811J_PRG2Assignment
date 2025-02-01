//==========================================================
// Student Number : S10267811J
// Student Name : Gurveer Singh
// Partner Name : BOO yuan sheng
//==========================================================
using System;

public abstract class Flight : IComparable<Flight>  
{
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime ExpectedTime { get; set; }
    public string Status { get; set; } = "On Time";
    public Airline Airline { get; set; }
    public BoardingGate BoardingGate { get; set; }
    public string SpecialRequestCode { get; set; }

    public abstract double CalculateFees();

    //start the comparetomethod
    public int CompareTo(Flight other)
    {
        if (other == null) return 1;

        
        return this.ExpectedTime.CompareTo(other.ExpectedTime);
    }
}
