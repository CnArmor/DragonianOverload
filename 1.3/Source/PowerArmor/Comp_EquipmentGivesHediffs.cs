using System.Collections.Generic;
using Verse;

namespace Dragonian
{
    [StaticConstructorOnStartup]
    public class CompProperties_EquipmentGivesHediffs : CompProperties
    {
        public CompProperties_EquipmentGivesHediffs()
        {
            this.compClass = typeof(Comp_EquipmentGivesHediffs);
        }
        public List<HediffDef> hediffs;
    }
    public class Comp_EquipmentGivesHediffs : ThingComp
    {
        public CompProperties_EquipmentGivesHediffs Props
        {
            get
            {
                return (CompProperties_EquipmentGivesHediffs)this.props;
            }
        }
        public override void Notify_Equipped(Pawn pawn)
        {
            base.Notify_Equipped(pawn);
            if (!Props.hediffs.NullOrEmpty())
            {
                foreach(HediffDef hediff in Props.hediffs)
                {
                    if (!pawn.health.hediffSet.HasHediff(hediff))
                    {
                        pawn.health.AddHediff(hediff);
                    }
                }
            }
        }
        public override void Notify_Unequipped(Pawn pawn)
        {
            base.Notify_Unequipped(pawn);
            if (!Props.hediffs.NullOrEmpty())
            {
                foreach (HediffDef hediff in Props.hediffs)
                {
                    while (pawn.health.hediffSet.HasHediff(hediff))
                    {
                        pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(hediff));
                    }
                }
            }
        }
    }
}