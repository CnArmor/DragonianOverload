using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Dragonian
{
    public class PoweredAbility : Ability
    {
        public PoweredAbility(Pawn pawn) : base(pawn)
        {

        }
        public PoweredAbility(Pawn pawn, AbilityDef def) : base(pawn, def)
        {

        }
        public PoweredArmorPowerSource PowerSource 
        {
            get
            {
                return pawn.FindPowerSource();
            }
        }
        public bool IsToggleable
        {
            get
            {
                return def.GetModExtension<DefModExtension_PoweredAbility>().toggleable;
            }
        }

        private Texture2D inactiveIcon;
        public Texture2D InactiveIcon
        {
            get 
            {
                if(inactiveIcon == null && def.GetModExtension<DefModExtension_PoweredAbility>()?.inactiveIcon != null)
                {
                    inactiveIcon = ContentFinder<Texture2D>.Get(def.GetModExtension<DefModExtension_PoweredAbility>().inactiveIcon, true);
                }
                return inactiveIcon; 
            }
        }
        private SoundDef deactivateSound;
        public SoundDef DeactivateSound
        {
            get
            {
                if(deactivateSound == null && def.GetModExtension<DefModExtension_PoweredAbility>()?.deactivateSound != null)
                {
                    deactivateSound = def.GetModExtension<DefModExtension_PoweredAbility>().deactivateSound;
                }
                return deactivateSound;
            }
        }
        public override bool GizmoDisabled(out string reason)
        {
            if (PowerSource == null)
            {
                reason = "DragonianPowerSourceNotFound".Translate();
                return true;
            }
            if (!PowerSource.IsActivated)
            {
                reason = "DragonianPowerDiactivated".Translate();
                return true;
            }
            return base.GizmoDisabled(out reason);
        }
        public override IEnumerable<Command> GetGizmos()
        {
            if (gizmo == null)
            {
                gizmo = new Command_PoweredAbility(this);
            }
            yield return gizmo;
            yield break;
        }
        public override void AbilityTick()
        {
            if (IsToggleable)
            {
                if(PowerSource == null || !PowerSource.IsActivated)
                {
                    this.StartCooldown(this.def.cooldownTicksRange.RandomInRange);
                    isActive = false;
                }
            }
            base.AbilityTick();
        }

        public bool isActive = false;
    }
}
