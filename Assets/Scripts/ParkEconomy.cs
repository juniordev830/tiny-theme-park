using System;

namespace TinyThemePark
{
    /// <summary>Pure park rules, without any dependency on the Unity runtime.</summary>
    public sealed class ParkEconomy
    {
        public int Coins { get; private set; }
        public int Attractions { get; private set; }
        public int Visitors { get; private set; }
        public int Happiness { get; private set; }
        public int MinutesElapsed { get; private set; }

        public int BuildCost { get { return 100 + Attractions * 30; } }

        public ParkEconomy(int initialCoins = 300, int initialAttractions = 1)
        {
            if (initialCoins < 0 || initialAttractions < 0 || initialAttractions > 20)
                throw new ArgumentOutOfRangeException("Initial park values must be nonnegative and at most 20 rides.");
            Coins = initialCoins;
            Attractions = initialAttractions;
            Visitors = 0;
            Happiness = 70;
            MinutesElapsed = 0;
        }

        public bool TryBuildRide()
        {
            if (Attractions >= 12 || Coins < BuildCost) return false;
            Coins -= BuildCost;
            Attractions += 1;
            Happiness = Math.Min(100, Happiness + 3);
            return true;
        }

        public void SimulateMinute()
        {
            int newVisitors = Math.Max(0, Attractions * 3 - 1);
            Visitors = Math.Min(150, Visitors + newVisitors);
            int grossIncome = Visitors * 3;
            int operatingCosts = Attractions * 4;
            Coins = Math.Max(0, Coins + grossIncome - operatingCosts);
            Happiness = Math.Max(30, Math.Min(100, Happiness + (Attractions >= 3 ? 1 : -1)));
            MinutesElapsed += 1;
        }
    }
}
