//==========================================================
// Student Number : S10270138
// Student Name : Boo Yuan Sheng
// Partner Name : Gurveer Singh
//==========================================================
public class BoardingGate
{
    public string GateName { get; set; }
    public bool SupportsDDJB { get; set; }
    public bool SupportsCFFT { get; set; }
    public bool SupportsLWTT { get; set; }

    // Flight assigned to this gate
    public Flight AssignedFlight { get; set; } // This is where the gate links to the flight

    public double CalculateFees()
    {
        return 300; // Base fee for using a gate
    }
}
