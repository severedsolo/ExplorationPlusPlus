using System;
using System.Linq;
using Contracts;
using UnityEngine;

namespace ExplorationPlus
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class ExplorationPlusUtilities : MonoBehaviour
    {
        public static EventData<CelestialBody, Vessel> OnRendezvousComplete;
        private void Awake()
        {
            DontDestroyOnLoad(this);
            OnRendezvousComplete = new EventData<CelestialBody, Vessel>("onRendezvousComplete");
        }

        private void Start()
        {
            //TODO: Bump up contract weights.
            //TODO: Limit number of contracts that can generate
        }

        public static float SetCurrency(float actualReward, Contract.ContractPrestige prestige)
        {
            switch (prestige)
            {
                case Contract.ContractPrestige.Trivial:
                    return actualReward / 1.0f;
                case Contract.ContractPrestige.Significant:
                    return actualReward / 1.25f;
                case Contract.ContractPrestige.Exceptional:
                    return actualReward / 1.5f;
                default:
                    throw new ArgumentOutOfRangeException(nameof(prestige), prestige, null);
            }
        }

        public static bool ContractIsOffered(string contractTitle)
        {
            for (int i = 0; i < ContractSystem.Instance.Contracts.Count; i++)
            {
                Contract c = ContractSystem.Instance.Contracts.ElementAt(i);
                if (c.Title != contractTitle) continue;
                if (c.ContractState == Contract.State.Active || c.ContractState == Contract.State.Offered) return true;
            }
            return false;
        }
    }
}