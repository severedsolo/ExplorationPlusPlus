using System;
using System.Linq;
using UnityEngine;

namespace ExplorationPlus
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class RendezvousHelper : MonoBehaviour
    {
        private void Start()
        {
            InvokeRepeating(nameof(CheckForRendezvous), 0.5f, 0.5f);
        }

        void CheckForRendezvous()
        {
            if (FlightGlobals.ActiveVessel == null) return;
            for (int i = 0; i < FlightGlobals.Vessels.Count; i++)
            {
                Vessel v = FlightGlobals.Vessels.ElementAt(i);
                if (v == FlightGlobals.ActiveVessel) continue;
                if (v.vesselType == VesselType.Debris || v.vesselType == VesselType.Flag || v.vesselType == VesselType.Unknown || v.vesselType == VesselType.SpaceObject || v.vesselType == VesselType.EVA) continue;
                if (!InRange(v)) continue;
                if (Math.Abs(v.speed - FlightGlobals.ActiveVessel.speed) > 100) continue;
                ExplorationPlusUtilities.OnRendezvousComplete.Fire(v.mainBody, v);
            }
        }
        
        private bool InRange(Vessel v)
        {
            float distanceToTarget = (FlightGlobals.ActiveVessel.transform.position - v.transform.position).magnitude;
            return distanceToTarget < 1000;
        }

    }
}