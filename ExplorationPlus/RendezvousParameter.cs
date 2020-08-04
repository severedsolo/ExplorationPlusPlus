using Contracts;
using UniLinq;
using UnityEngine.Experimental.PlayerLoop;

namespace ExplorationPlus
{
    public class RendezvousParameter : ContractParameter
    {
        private CelestialBody targetBody = Planetarium.fetch.Home;

        public RendezvousParameter()
        {

        }

        protected override void OnRegister()
        {
            ExplorationPlusUtilities.OnRendezvousComplete.Add(OnRendezvousComplete);
        }

        private void OnRendezvousComplete(CelestialBody body, Vessel v)
        {
            if(body == targetBody) SetComplete();
        }

        protected override void OnUnregister()
        {
            ExplorationPlusUtilities.OnRendezvousComplete.Remove(OnRendezvousComplete);
        }
        
        protected override string GetTitle()
        {
            return "Rendezvous Any Two Vessels around " + targetBody.displayName;
        }
    }
}