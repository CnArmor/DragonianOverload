using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.Sound;

namespace Dragonian
{
    [StaticConstructorOnStartup]
    public class CompProperties_PoweredPassiveBuffs : CompProperties
    {
        public CompProperties_PoweredPassiveBuffs()
        {
            this.compClass = typeof(Comp_PoweredPassiveBuffs);
        }
        public HediffDef hediff;
    }
    public class Comp_PoweredPassiveBuffs : ThingComp
    {
        public CompProperties_PoweredPassiveBuffs Props
        {
            get
            {
                return (CompProperties_PoweredPassiveBuffs) this.props;
            }
        }
        public PoweredArmorPowerSource PowerSource
        {
            get
            {
                return parent.FindPowerSource();
            }
        }
        public override void Notify_Equipped(Pawn pawn)
        {
            if (!pawn.health.hediffSet.HasHediff(Props.hediff))
            {
                HediffComp_RemoveIfApparelDropped hdc = pawn.health.AddHediff(Props.hediff).TryGetComp<HediffComp_RemoveIfApparelDropped>();
                if(hdc != null)
                {
                    hdc.wornApparel = (Apparel)this.parent;
                }
            }
        }
    }
}