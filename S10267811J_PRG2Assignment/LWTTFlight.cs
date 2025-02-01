//==========================================================
// Student Number : S10267811J
// Student Name : Gurveer Singh
// Partner Name : BOO yuan sheng
//==========================================================
public class LWTTFlight : Flight
{
    public double RequestFee { get; set; } = 200;

    public override double CalculateFees()
    {
        return 500 + RequestFee;
    }
}
