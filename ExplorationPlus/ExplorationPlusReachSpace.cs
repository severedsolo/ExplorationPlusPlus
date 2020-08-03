using Contracts;
using Contracts.Agents;
using Contracts.Parameters;
using KSPAchievements;
using UnityEngine;

namespace ExplorationPlus
{
    public class ExplorationPlusReachSpace : Contract
    {
        private CelestialBody targetBody = Planetarium.fetch.Home;
        protected override bool Generate()
        {
            if (ExplorationPlusUtilities.ContractIsOffered(GetTitle())) return false;
            SetExpiry(1, 7);
            SetScience(ExplorationPlusUtilities.SetCurrency(9.0f, prestige), targetBody);
            agent = AgentList.Instance.GetAgent("Kerbin World-Firsts Record-Keeping Society");
            SetDeadlineYears(500, targetBody);
            SetReputation(ExplorationPlusUtilities.SetCurrency(4.5f, prestige), 0f, targetBody);
            SetFunds(0f,ExplorationPlusUtilities.SetCurrency(66000f, prestige), 0f, targetBody);
            AddParameter(new ReachSituation(Vessel.Situations.SUB_ORBITAL, "Reach Space"));
            Debug.Log("[ExplorationPlus]: Generate ReachSpace: "+!ProgressTracking.Instance.reachSpace.IsComplete);
            return !ProgressTracking.Instance.reachSpace.IsComplete;
        }

        public override bool MeetRequirements()
        {
            return true;
        }

        public override bool CanBeCancelled()
        {
            return false;
        }

        public override bool CanBeDeclined()
        {
            return false;
        }
        
        protected override string GetHashString()
        {
            return "ExplorationPlusReachSpace"+targetBody;
        }

        protected override string GetTitle()
        {
            return "Reach For The Stars";
        }

        protected override string GetDescription()
        {
            return "It's time to slip the surly bonds of gravity, and touch the stars. What? Oh, I mean - let's go to space.";
        }

        protected override string GetSynopsys()
        {
            return "To call ourselves a space program, we have to actually go to space.";
        }

        protected override string MessageCompleted()
        {
            return "Huzzah! There is no such thing as the final frontier.";
        }
    }
}