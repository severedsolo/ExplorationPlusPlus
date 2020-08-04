using System.Linq;
using Contracts;

namespace ExplorationPlus
{
    public class MannedOrbitParameter : ContractParameter
    {
        private CelestialBody targetBody;

        public MannedOrbitParameter()
        {
            
        }
        
        public MannedOrbitParameter(CelestialBody parameterTarget)
        {
            targetBody = parameterTarget;
        }

        protected override void OnRegister()
        {
            GameEvents.onVesselSituationChange.Add(CheckSituation);
        }

        private void CheckSituation(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
        {
            //Not crewed? Not Complete
            if (data.host.GetCrewCount() == 0) return;
            //Not orbited? Not complete
            if (data.to == Vessel.Situations.ORBITING && data.host.mainBody == targetBody) SetComplete();
        }

        protected override void OnUnregister()
        {
            GameEvents.onVesselSituationChange.Remove(CheckSituation);
        }

        protected override string GetTitle()
        {//TODO: Make this a bit clearer as to which bits are outstanding
            return "Perform a Manned Landing on "+targetBody.displayName;
        }
        
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("targetBody", targetBody.name);
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
        }
    }
}