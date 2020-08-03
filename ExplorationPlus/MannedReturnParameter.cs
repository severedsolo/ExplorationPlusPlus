using System;
using System.Linq;
using Contracts;
using Expansions.Missions;

namespace ExplorationPlus
{

    public class MannedReturnParameter : ContractParameter
    {
        private CelestialBody targetBody;
        private string situation;

        public MannedReturnParameter(CelestialBody initialBody, Vessel.Situations situationToCheckFor)
        {
            targetBody = initialBody;
            switch (situationToCheckFor)
            {
                case Vessel.Situations.LANDED:
                    situation = "Land";
                    break;
                case Vessel.Situations.SPLASHED:
                    situation = "Land";
                    break;
                case Vessel.Situations.ORBITING:
                    situation = "Orbit";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(situationToCheckFor), situationToCheckFor, null);
            }

        }

        protected override void OnRegister()
        {
            GameEvents.onVesselSituationChange.Add(CheckSituation);
        }

        private void CheckSituation(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
        {
            //Not crewed? Not complete
            if (data.host.GetCrewCount() == 0) return;
            //Not at home? Not complete
            if (data.host.mainBody != Planetarium.fetch.Home || !Landed(data.to)) return;
            if (MissionComplete(data)) SetComplete();
        }

        private bool MissionComplete(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
        {
            //Find the Trip Logger
            ModuleTripLogger tripLogger = data.host.FindPartModuleImplementing<ModuleTripLogger>();
            if (tripLogger == null) return false;
            switch (situation)
            {
                case "Land":
                    return tripLogger.Log.HasEntry(FlightLog.EntryType.Land, targetBody.name);
                case "Orbit":
                    return tripLogger.Log.HasEntry(FlightLog.EntryType.Orbit, targetBody.name);
                default:
                    return false;
            }
        }

        private bool Landed(Vessel.Situations situationToCheck)
        {
            return situationToCheck == Vessel.Situations.LANDED || situationToCheck == Vessel.Situations.SPLASHED;
        }

        protected override void OnUnregister()
        {
            GameEvents.onVesselSituationChange.Remove(CheckSituation);
        }

        protected override string GetTitle()
        {
            if (situation == "Orbit") return "Return a crew from orbit of " + targetBody.displayName;
            return "Return a crew from landing on " + targetBody.displayName;
        }

        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("targetBody", targetBody.name);
            node.AddValue("situation", situation);
        }

        protected override void OnLoad(ConfigNode node)
        {
            string loadedName = node.GetValue("targetBody");
            for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
            {
                CelestialBody cb = FlightGlobals.Bodies.ElementAt(i);
                if (loadedName != cb.name) continue;
                targetBody = cb;
            }
            situation = node.GetValue("situation");
        }
    }
}