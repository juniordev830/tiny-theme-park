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
var baseline = new ParkEconomy(300, 2);
var promoted = new ParkEconomy(300, 2);
Check(promoted.MarketingCost == 65, "marketing price scales with attractions");
Check(promoted.TryLaunchCampaign(), "campaign can be purchased");
Check(promoted.Coins == 235, "campaign charges coins once");
Check(promoted.CampaignMinutesRemaining == 5, "campaign lasts five simulated minutes");
Check(!promoted.TryLaunchCampaign(), "active campaign cannot be purchased twice");
for (var i = 0; i < 5; i++)
{
    baseline.SimulateMinute();
    promoted.SimulateMinute();
}
Check(promoted.CampaignMinutesRemaining == 0, "campaign expires on fifth minute");
Check(promoted.Visitors > baseline.Visitors, "advertising increases arrivals");
Check(promoted.Happiness > baseline.Happiness, "campaign improves satisfaction");
Check(promoted.TryLaunchCampaign(), "expired campaign can be purchased again");
var poor = new ParkEconomy(10, 1);
Check(!poor.TryLaunchCampaign(), "cannot spend more coins than available");
Check(poor.Coins == 10 && poor.CampaignMinutesRemaining == 0, "failed purchase does not change state");
var wealthy = new ParkEconomy(int.MaxValue, 1);
wealthy.SimulateMinute();
Check(wealthy.Coins == int.MaxValue, "income is capped rather than overflowing the balance");
Check(wealthy.LastMinuteNetRevenue == 2, "net revenue is visitors times price less upkeep");
var empty = new ParkEconomy(0, 0);
empty.SimulateMinute();
Check(empty.Coins == 0 && empty.LastMinuteNetRevenue == 0, "empty park has no operating revenue");
Check(park.LastMinuteNetRevenue == park.Visitors * 3 - park.Attractions * 4, "net revenue reflects costs");
Console.WriteLine("Park economy checks passed, including revenue reporting and overflow protection.");
