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
        public int LastMinuteNetRevenue { get; private set; }
        public int CampaignMinutesRemaining { get; private set; }
        public int MarketingCost { get { return 45 + Attractions * 10; } }

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
            CampaignMinutesRemaining = 0;
        }

        public bool TryBuildRide()
        {
            if (Attractions >= 12 || Coins < BuildCost) return false;
            Coins -= BuildCost;
            Attractions += 1;
            Happiness = Math.Min(100, Happiness + 3);
            return true;
        }

        /// <summary>Runs a five-minute in-game marketing campaign if affordable.</summary>
        public bool TryLaunchCampaign()
        {
            if (CampaignMinutesRemaining > 0 || Coins < MarketingCost) return false;
            Coins -= MarketingCost;
            CampaignMinutesRemaining = 5;
            return true;
        }

        public void SimulateMinute()
        {
            bool campaignActive = CampaignMinutesRemaining > 0;
            int newVisitors = Math.Max(0, Attractions * 3 - 1) + (campaignActive ? 4 : 0);
            Visitors = Math.Min(150, Visitors + newVisitors);
            int grossIncome = Visitors * 3;
            int operatingCosts = Attractions * 4;
            LastMinuteNetRevenue = grossIncome - operatingCosts;
            // Calculate with long to prevent int overflow when the park has a large balance.
            long nextBalance = (long)Coins + LastMinuteNetRevenue;
            Coins = (int)Math.Max(0L, Math.Min(int.MaxValue, nextBalance));
            Happiness = Math.Max(30, Math.Min(100, Happiness + (Attractions >= 3 ? 1 : -1) + (campaignActive ? 2 : 0)));
            if (campaignActive) CampaignMinutesRemaining -= 1;
            if (MinutesElapsed < int.MaxValue) MinutesElapsed += 1;
        }
    }
}
