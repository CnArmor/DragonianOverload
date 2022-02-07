/*using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Dragonian
{
    [StaticConstructorOnStartup] 
    public class CompProperties_PoweredStaggerImmunity : CompProperties
    {
        public CompProperties_PoweredStaggerImmunity()
        {
            this.compClass = typeof(Comp_PoweredStaggerImmunity);
        }
        public float powerCostPerHit = 10f;
        public float incommingDamageMutiplier = 0.95f;
        public int coolDownTicks = 600;
        [MustTranslate]
        public string label;
        [MustTranslate]
        public string description;
        public string iconPathActive;
        public string iconPathInactive;
        public SoundDef activateSound;
        public SoundDef deactivateSound;
    }

    public class Comp_PoweredStaggerImmunity : ThingComp
    {
        public CompProperties_PoweredStaggerImmunity Props
        {
            get
            {
                return (CompProperties_PoweredStaggerImmunity)this.props;
            }
        }
        public PoweredArmorPowerSource PowerSource
        {
            get
            {
                return parent.FindPowerSource();
            }
        }
        public Texture2D ActiveIcon
        {
            get
            {
                if (activeIcon == null)
                {
                    activeIcon = ContentFinder<Texture2D>.Get(Props.iconPathActive, true);
                }
                return activeIcon;
            }
        }
        public Texture2D InactiveIcon
        {
            get
            {
                if (inactiveIcon == null)
                {
                    inactiveIcon = ContentFinder<Texture2D>.Get(Props.iconPathInactive, true);
                }
                return inactiveIcon;
            }
        }
        public bool IsInCoolDown
        {
            get
            {
                return cooldownTicksRemaining > 0; 
            }
        }
        public string ResolveDeiscription
        {
            get
            {
                finalDescription = Props.description + "\n\n" + "DragonianIncommingDamageMultiplier".Translate() + Props.incommingDamageMutiplier.ToStringByStyle(ToStringStyle.PercentZero) + "\n" + "DragonianPowerDrainPerHit".Translate() + Props.powerCostPerHit.ToStringByStyle(ToStringStyle.FloatOne);
                return finalDescription;
            }
        }
        public override void PostExposeData()
        {
            Scribe_Values.Look<int>(ref cooldownTicksRemaining, "DragonianOverload.Comp_PoweredStaggerImmunity.cooldownTicksRemaining", 0, false);
        }
        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetWornGizmosExtra())
            {
                yield return gizmo;
            }
            yield return CreateCommand();
            yield break;
        }
        public override void PostPostMake()
        {
            base.PostPostMake();
            isActivated = false;
        }
        public Command_PoweredAbilityAction CreateCommand()
        {
            Command_PoweredAbilityAction command = new Command_PoweredAbilityAction();
            command.defaultLabel = Props.label;
            command.defaultDesc = ResolveDeiscription;
            command.action = delegate ()
            {
                if (isActivated)
                {
                    Props.deactivateSound.PlayOneShot(new TargetInfo(PowerSource.Wearer.Position, PowerSource.Wearer.Map, false));
                }
                if (!isActivated)
                {
                    Props.activateSound.PlayOneShot(new TargetInfo(PowerSource.Wearer.Position,PowerSource.Wearer.Map, false));
                }
                isActivated = !isActivated;
                cooldownTicksRemaining = Props.coolDownTicks;
            };
            command.icon = InactiveIcon;
            if (isActivated)
            {
                command.icon = ActiveIcon;
            }
            if (!PowerSource.Wearer.IsColonistPlayerControlled)
            {
                command.Disable(null);
            }
            if (IsInCoolDown || !PowerSource.IsActivated)
            {
                command.Disable(DisableReason());
            }
            command.comp = this;
            
            return command;
        }
        public string DisableReason()
        {
            string result = "Error no disable reason found";
            if (IsInCoolDown)
            {
                result = "DragonianPowerCoolDown".Translate();
            }
            if (!PowerSource.IsActivated)
            {
                result = "DragonianPowerDiactivated".Translate();
            }
            return result;
        }
        public override void CompTick()
        {
            base.CompTick();
            if (cooldownTicksRemaining > 0)
                cooldownTicksRemaining--;
        }

        public bool isActivated = false;
        public int cooldownTicksRemaining;
        public Texture2D activeIcon;
        public Texture2D inactiveIcon;
        [MustTranslate]
        public string finalDescription;
    }
}*/