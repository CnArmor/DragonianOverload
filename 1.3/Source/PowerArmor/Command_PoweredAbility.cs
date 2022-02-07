using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Dragonian
{
    [StaticConstructorOnStartup]

    public class Command_PoweredAbility : Command_Ability
    {
        public Command_PoweredAbility(PoweredAbility ability) : base(ability)
        {
            this.poweredAbility = ability;
            if(ability.IsToggleable && (!ability.isActive || ability.PowerSource == null))
            {
                icon = ability.InactiveIcon;
            }
        }
        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            if (poweredAbility.IsToggleable)
            {
                if (poweredAbility.isActive)
                {
                    if(poweredAbility.InactiveIcon != null)
                    {
                        icon = poweredAbility.InactiveIcon;
                    }
                    if (poweredAbility.DeactivateSound != null)
                    {
                        poweredAbility.DeactivateSound.PlayOneShot(new TargetInfo(poweredAbility.pawn.Position, poweredAbility.pawn.Map, false));
                    }
                }
                if (!poweredAbility.isActive)
                {
                    icon = poweredAbility.def.uiIcon;
                    if(poweredAbility.def.warmupStartSound != null)
                    {
                        poweredAbility.def.warmupStartSound.PlayOneShot(new TargetInfo(poweredAbility.pawn.Position, poweredAbility.pawn.Map, false));
                    }
                }
                poweredAbility.isActive = !poweredAbility.isActive;
            }
            
        }
        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            if(poweredAbility.PowerSource == null || poweredAbility.PowerSource?.IsActivated != true)
            {
                if (poweredAbility.InactiveIcon != null)
                    icon = poweredAbility.InactiveIcon;
            }
            return base.GizmoOnGUI(topLeft, maxWidth, parms);
        }
        private PoweredAbility poweredAbility;
    }
}