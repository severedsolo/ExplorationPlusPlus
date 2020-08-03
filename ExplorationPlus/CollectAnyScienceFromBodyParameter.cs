using System;
using System.Linq;
using Contracts;
using Steamworks;

namespace ExplorationPlus
{
    public class CollectAnyScienceFromBodyParameter : ContractParameter
    {
        private CelestialBody targetBody;

        public CollectAnyScienceFromBodyParameter()
        {
            
        }
        public CollectAnyScienceFromBodyParameter(CelestialBody target)
        {
            targetBody = target;
        }
        
        protected override void OnRegister()
        {
            GameEvents.OnScienceRecieved.Add(ScienceReceived);
        }

        protected override void OnUnregister()
        {
            GameEvents.OnScienceRecieved.Remove(ScienceReceived);
        }

        private void ScienceReceived(float data0, ScienceSubject data1, ProtoVessel data2, bool data3)
        {
            if(data1.IsFromBody(targetBody)) SetComplete();
        }

        protected override string GetTitle()
        {
            return "Recover or Transmit Science from " + targetBody.name;
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
                break;
            }
        }
    }
}