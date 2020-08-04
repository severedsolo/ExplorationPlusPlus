using Contracts;

namespace ExplorationPlus
{
    public class DockingParameter : ContractParameter
    {
        private CelestialBody targetBody = Planetarium.fetch.Home;
        private string vesselTarget = "Docking Target (TBD)";

        public DockingParameter()
        {
            
        }

        protected override void OnRegister()
        {
            ExplorationPlusUtilities.OnRendezvousComplete.Add(OnRendezvousComplete);
            GameEvents.onDockingComplete.Add(DockingComplete);
        }

        private void OnRendezvousComplete(CelestialBody body, Vessel v)
        {
            vesselTarget = v.vesselName;
        }

        protected override void OnUnregister()
        {
            ExplorationPlusUtilities.OnRendezvousComplete.Remove(OnRendezvousComplete);
            GameEvents.onDockingComplete.Remove(DockingComplete);
        }

        private void DockingComplete(GameEvents.FromToAction<Part, Part> data)
        {
            if(data.from.vessel.vesselName == vesselTarget || data.to.vessel.vesselName == vesselTarget) SetComplete();
        }

        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("target", vesselTarget);
        }

        protected override void OnLoad(ConfigNode node)
        {
            vesselTarget = node.GetValue("target");
        }

        protected override string GetTitle()
        {
            return "Dock with " + vesselTarget;
        }
    }
}