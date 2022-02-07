using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Dragonian
{
    [StaticConstructorOnStartup]

    public class HediffCompProperties_PoweredHediff : HediffCompProperties
    {
        public HediffCompProperties_PoweredHediff()
        {
            this.compClass = typeof(HediffComp_PoweredHediff);
        }
       // public List<AbilityDef> abilities;
    }
    public class HediffComp_PoweredHediff : HediffComp
    {
        public HediffCompProperties_PoweredHediff Props
        {
            get
            {
                return (HediffCompProperties_PoweredHediff)this.props;
            }
        }
        public PoweredArmorPowerSource PowerSource
        {
            get 
            {
                return parent.pawn.FindPowerSource();
            }
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            CheckBuffStatus();
        }

        private void CheckBuffStatus() 
        {
            if(PowerSource == null && parent.Severity >= 0.5f)
            {
                parent.Severity = 0.1f;
            }
            if(PowerSource != null)
            {
                if (PowerSource.IsActivated && parent.Severity <= 0.5f)
                {
                    parent.Severity = 0.6f;
                }
                if (!PowerSource.IsActivated && parent.Severity >= 0.5f)
                {
                    parent.Severity = 0.1f;
                }
            }
        }
      /*public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            if (!Props.abilities.NullOrEmpty())
            {
                foreach(AbilityDef ability in Props.abilities)
                {
                    if (parent.pawn.abilities.GetAbility(ability) != null)
                    {
                        parent.pawn.abilities.GainAbility(ability);
                    }
                }
            }
        }
        public override void CompPostPostRemoved()
        {
            if (!Props.abilities.NullOrEmpty())
            {
                foreach (AbilityDef ability in Props.abilities)
                {
                    parent.pawn.abilities.RemoveAbility(ability);
                }
            }
        }*/
    }
}