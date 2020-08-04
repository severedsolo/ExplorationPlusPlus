using System.Linq;
using Contracts;
using Contracts.Agents;
using FinePrint.Utilities;

namespace ExplorationPlus
{
    public class ExplorationPlusLanding : Contract
    {
        private CelestialBody targetBody;

        protected override bool Generate()
        {
            for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
            {
                CelestialBody cb = FlightGlobals.Bodies.ElementAt(i);
                //Must have orbited but not landed
                if (cb == Planetarium.fetch.Home) continue;
                if (!ProgressUtilities.GetBodyProgress(ProgressType.ORBIT, cb)) continue;
                if (ProgressUtilities.GetBodyProgress(ProgressType.LANDING, cb) || ProgressUtilities.GetBodyProgress(ProgressType.SPLASHDOWN, cb)) continue;
                targetBody = cb;
                //If the contract already exists set targetBody to null so the search can continue
                if (ExplorationPlusUtilities.ContractIsOffered(GetTitle())) targetBody = null;
                if (targetBody == null) continue;
                break;
            }
            if (targetBody == null) return false;
            SetExpiry(1, 7);
            SetScience(ExplorationPlusUtilities.SetCurrency(9.0f, prestige), targetBody);
            agent = AgentList.Instance.GetAgent("Kerbin World-Firsts Record-Keeping Society");
            SetDeadlineYears(500, targetBody);
            SetReputation(ExplorationPlusUtilities.SetCurrency(4.5f, prestige), 0f, targetBody);
            SetFunds(0f, ExplorationPlusUtilities.SetCurrency(66000f, prestige), 0f, targetBody);
            AddParameter(new LandingParameter(targetBody));
            return true;
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
            return "ExplorationPlusOrbit" + targetBody;
        }

        protected override string GetTitle()
        {
            return "Orbit " + targetBody.displayName;
        }

        protected override string GetDescription()
        {
            return "Here at KSC we like to think we are always open to exploring new frontiers. With this in mind, we want you to head out to " + targetBody.displayName + " and see what we can see.";
        }

        protected override string GetSynopsys()
        {
            return "Reach orbit of " + targetBody.displayName;
        }

        protected override string MessageCompleted()
        {
            return "Huzzah! There is no such thing as the final frontier.";
        }
    }
}