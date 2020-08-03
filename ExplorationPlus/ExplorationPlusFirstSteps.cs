using System.Linq;
using System.Runtime.CompilerServices;
using Contracts;
using Contracts.Agents;
using Contracts.Parameters;
using FinePrint.Contracts.Parameters;
using KSPAchievements;
using LibNoise.Modifiers;
using UnityEngine;

namespace ExplorationPlus
{
    public class ExplorationPlusFirstSteps : Contract
    {
        private CelestialBody targetBody = Planetarium.fetch.Home;
        protected override bool Generate()
        {
            if (ExplorationPlusUtilities.ContractIsOffered(GetTitle())) return false;
            SetExpiry(1, 7);
            SetScience(ExplorationPlusUtilities.SetCurrency(3.9f, prestige), targetBody);
            agent = AgentList.Instance.GetAgent("Kerbin World-Firsts Record-Keeping Society");
            SetDeadlineYears(500, targetBody);
            SetReputation(ExplorationPlusUtilities.SetCurrency(7.95f, prestige), 0f, targetBody);
            SetFunds(0f,ExplorationPlusUtilities.SetCurrency(16000f, prestige), 0f, targetBody);
            AddParameter(new CollectAnyScienceFromBodyParameter(targetBody));
            AddParameter(new LaunchFirstVesselParameter());
            Debug.Log("[ExplorationPlus]: Generate FirstSteps: "+!ProgressTracking.Instance.firstLaunch.IsComplete);
            return !ProgressTracking.Instance.firstLaunch.IsComplete;
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
            return "ExplorationPlusFirstSteps"+targetBody;
        }

        protected override string GetTitle()
        {
            return "Do Some Science!";
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