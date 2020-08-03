using System;
using System.Linq;
using Contracts;
using UnityEngine;

namespace ExplorationPlus
{
    public class LaunchFirstVesselParameter : ContractParameter
    {
        private CelestialBody targetBody = Planetarium.fetch.Home;
        private string bodyName = "Kerbin";

        public LaunchFirstVesselParameter()
        {
            
        }

        protected override void OnRegister()
        {
            GameEvents.onVesselSituationChange.Add(CheckSituation);
        }

        private void CheckSituation(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
        {
            if (data.to == Vessel.Situations.FLYING && data.host.mainBody == targetBody) SetComplete();
        }

        protected override void OnUnregister()
        {
            GameEvents.onVesselSituationChange.Remove(CheckSituation);
        }

        protected override string GetTitle()
        {
            return "Launch Your First Vessel";
        }

        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("targetBody", bodyName);
        }

        protected override void OnLoad(ConfigNode node)
        {
            string loadedName = node.GetValue("targetBody");
            for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
            {
                CelestialBody cb = FlightGlobals.Bodies.ElementAt(i);
                if (loadedName != cb.name) continue;
                targetBody = cb;
                bodyName = cb.name;
            }
        }
    }
}