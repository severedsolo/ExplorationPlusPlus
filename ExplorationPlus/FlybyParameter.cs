using System.Linq;
using Contracts;
using FinePrint.Utilities;

namespace ExplorationPlus
{
    public class FlybyParameter : ContractParameter
    {
        private CelestialBody targetBody;

        public FlybyParameter()
        {
            
        }

        public FlybyParameter(CelestialBody cb)
        {
            targetBody = cb;
        }

        protected override void OnRegister()
        {
            GameEvents.onVesselSituationChange.Add(CheckSituation);
        }

        private void CheckSituation(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
        {
            if (data.to == Vessel.Situations.ESCAPING && data.host.mainBody == targetBody) SetComplete();
        }

        protected override void OnUnregister()
        {
            GameEvents.onVesselSituationChange.Remove(CheckSituation);
        }

        protected override string GetTitle()
        {
            return "Perform a Flyby of "+targetBody.displayName;
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