using System.Linq;
using Contracts;
using Contracts.Agents;
using FinePrint.Utilities;
using UnityEngine;

namespace ExplorationPlus
{
    public class ExplorationPlusApollo : Contract
    {
        private CelestialBody targetBody = null;
        protected override bool Generate()
        {
            //Let's find a body we've landed on
            for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
            {
                //Let's Find a body we've landed on
                CelestialBody cb = FlightGlobals.Bodies.ElementAt(i);
                //No landings if it doesn't have a surface!
                if (!cb.hasSolidSurface) continue;
                //Have we landed on it?
                if (!cb.progressTree.landing.IsComplete) continue;
                //Yeah? Great, have we returned?
                if (!cb.progressTree.returnFromSurface.IsComplete) continue;
                //Temporarily set targetBody so we can check the title.
                targetBody = cb;
                //If the contract already exists, keep searching, but reset targetBody to null so we know it's not valid
                if (ExplorationPlusUtilities.ContractIsOffered(GetTitle())) targetBody = null;
            }
            //If we didn't find a target, stop Contract Generation
            if (targetBody == null) return false;
            SetExpiry(1, 7);
            SetScience(ExplorationPlusUtilities.SetCurrency(13.5f, prestige), targetBody);
            agent = AgentList.Instance.GetAgent("Kerbin World-Firsts Record-Keeping Society");
            SetDeadlineYears(500, targetBody);
            SetReputation(ExplorationPlusUtilities.SetCurrency(27f, prestige), 0f, targetBody);
            SetFunds(0f,ExplorationPlusUtilities.SetCurrency(198000f, prestige), 0f, targetBody);
            AddParameter(new MannedLandingParameter(targetBody));
            AddParameter(new MannedReturnParameter(targetBody, Vessel.Situations.LANDED));
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
            return "ExplorationPlusApollo"+targetBody;
        }

        protected override string GetTitle()
        {
            return "Do a Crewed Landing and Return from "+targetBody.displayName;
        }

        protected override string GetDescription()
        {
            return "Here at KSC we like to think we are always open to exploring new frontiers. With this in mind, we know you landed on "+targetBody.displayName+", but you must also return!";
        }

        protected override string GetSynopsys()
        {
            return "Do a manned round trip from "+targetBody.displayName;
        }

        protected override string MessageCompleted()
        {
            return "Huzzah! There is no such thing as the final frontier.";
        }
    }
}