using System;
using Contracts;
using Contracts.Agents;
using FinePrint.Utilities;

namespace ExplorationPlus
{
    public class ExplorationPlusOrbitalManoeuvres : Contract
    {
        private CelestialBody targetBody = Planetarium.fetch.Home;
        protected override bool Generate()
        {
            if (!ProgressUtilities.HaveModuleTech("ModuleDockingNode")) return false;
            if (ExplorationPlusUtilities.ContractIsOffered(GetTitle())) return false;
            SetExpiry(1, 7);
            SetScience(ExplorationPlusUtilities.SetCurrency(3.9f, prestige), targetBody);
            agent = AgentList.Instance.GetAgent("Kerbin World-Firsts Record-Keeping Society");
            SetDeadlineYears(500, targetBody);
            SetReputation(ExplorationPlusUtilities.SetCurrency(7.95f, prestige), 0f, targetBody);
            if(!ProgressUtilities.GetBodyProgress(ProgressType.RENDEZVOUS, targetBody))AddParameter(new RendezvousParameter());
            if(!ProgressUtilities.GetBodyProgress(ProgressType.DOCKING, targetBody))AddParameter(new DockingParameter());
            if (!ProgressUtilities.GetBodyProgress(ProgressType.SPACEWALK, targetBody)) throw new NotImplementedException(); //TODO: Spacewalk Parameter
            SetFunds(0f, ExplorationPlusUtilities.SetCurrency(ParameterCount * 4000f, prestige), 0f, targetBody);
            if (ParameterCount == 0) return false;
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
            return "ExplorationPlusDocking"+targetBody;
        }

        protected override string GetTitle()
        {
            return "Perform a series of Orbital Manoeuvres in orbit of "+targetBody.displayName;
        }

        protected override string GetDescription()
        {
            return "First things first, we need to test our equipment - be a good Kerbal and collect some science. Preferably while moving very fast";
        }

        protected override string GetSynopsys()
        {
            return "Start a space program";
        }

        protected override string MessageCompleted()
        {
            return "Huzzah! There is no such thing as the final frontier.";
        }
    }
}