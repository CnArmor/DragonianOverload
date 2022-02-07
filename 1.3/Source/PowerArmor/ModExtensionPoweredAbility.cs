using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Dragonian
{
    public class DefModExtension_PoweredAbility : DefModExtension
    {
        public bool toggleable = false;
        public float powerCost = 0f;
        public float powerCostPerHit = 0f;
        public float powerCostPerHitDamageFactor = 1f;
        public float powerDrain = 0f;
        public string inactiveIcon;
        public SoundDef deactivateSound;
    }
}