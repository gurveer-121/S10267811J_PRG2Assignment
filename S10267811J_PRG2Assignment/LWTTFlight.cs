//==========================================================
// Student Number : S10270138
// Student Name : Boo Yuan Sheng
// Partner Name : Gurveer Singh
//==========================================================
public class LWTTFlight : Flight
{
    public double RequestFee { get; set; } = 200;

    public override double CalculateFees()
    {
        return 500 + RequestFee;
    }
}
