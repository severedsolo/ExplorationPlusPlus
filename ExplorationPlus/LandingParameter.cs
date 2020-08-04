using System.Linq;
using Contracts;

namespace ExplorationPlus
{
    public class LandingParameter : ContractParameter
    {
        private CelestialBody targetBody;

        public LandingParameter()
        {

        }

        public LandingParameter(CelestialBody cb)
        {
            targetBody = cb;
        }

        protected override void OnRegister()
        {
            GameEvents.onVesselSituationChange.Add(CheckSituation);
        }

        private void CheckSituation(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
        {
            if (data.to != Vessel.Situations.LANDED && data.to != Vessel.Situations.SPLASHED) return;
            if(data.host.mainBody == targetBody) SetComplete();
        }

        protected override void OnUnregister()
        {
            GameEvents.onVesselSituationChange.Remove(CheckSituation);
        }

        protected override string GetTitle()
        {
            return "Land on " + targetBody.displayName;
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