using System.Linq;
using Contracts;
using Contracts.Agents;
using Contracts.Parameters;
using FinePrint.Utilities;
using UnityEngine;

namespace ExplorationPlus
{
    public class ExplorationPlusFlyby : Contract
    {
        private CelestialBody targetBody;

        protected override bool Generate()
        {
            if (!ProgressUtilities.GetBodyProgress(ProgressType.ORBIT, Planetarium.fetch.Home)) return false;
            targetBody = ProgressUtilities.GetNextUnreached(1).FirstOrDefault();
            if (targetBody == null) return false;
            if (ExplorationPlusUtilities.ContractIsOffered(GetTitle())) return false;
            SetExpiry(1, 7);
            SetScience(ExplorationPlusUtilities.SetCurrency(9.0f, prestige), targetBody);
            agent = AgentList.Instance.GetAgent("Kerbin World-Firsts Record-Keeping Society");
            SetDeadlineYears(500, targetBody);
            SetReputation(ExplorationPlusUtilities.SetCurrency(4.5f, prestige), 0f, targetBody);
            SetFunds(0f, ExplorationPlusUtilities.SetCurrency(66000f, prestige), 0f, targetBody);
            if (!targetBody.isStar) AddParameter(new FlybyParameter(targetBody));
            else AddParameter(new OrbitParameter(targetBody));
            AddParameter(new CollectAnyScienceFromBodyParameter(targetBody));
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
            return "ExplorationPlusFlyby" + targetBody;
        }

        protected override string GetTitle()
        {
            return "Start exploring " + targetBody.displayName;
        }

        protected override string GetDescription()
        {
            return "Here at KSC we like to think we are always open to exploring new frontiers. With this in mind, we want you to head out to " + targetBody.displayName + " and see what we can see.";
        }

        protected override string GetSynopsys()
        {
            return "Begin exploring " + targetBody.displayName;
        }

        protected override string MessageCompleted()
        {
            return "Huzzah! There is no such thing as the final frontier.";
        }
    }
}