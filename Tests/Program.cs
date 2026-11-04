using TinyThemePark;

static void Check(bool condition, string message)
{
    if (!condition) throw new Exception("Failed: " + message);
}
var park = new ParkEconomy();
Check(park.Coins == 300, "initial money");
Check(park.Attractions == 1, "starter attraction");
Check(park.BuildCost == 130, "first expansion cost");
Check(park.TryBuildRide(), "ride should be affordable");
Check(park.Attractions == 2, "second ride created");
Check(park.Coins == 170, "balance reduced");
Check(park.BuildCost == 160, "cost rises with attractions");
Check(park.TryBuildRide(), "third ride affordable");
Check(!park.TryBuildRide(), "cannot build with insufficient balance");
park.SimulateMinute();
Check(park.Visitors == 8, "visitor growth");
Check(park.MinutesElapsed == 1, "time progresses");
Check(park.Happiness >= 30 && park.Happiness <= 100, "bounded happiness");
try
{
    _ = new ParkEconomy(-1,1);
    throw new Exception("Negative initial funds were accepted.");
}
catch (ArgumentOutOfRangeException) { }
Console.WriteLine("All 13 park economy checks passed.");
