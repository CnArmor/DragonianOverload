using RimWorld;
using Verse;

namespace Dragonian
{
    public class CompProperties_ToggleableHediff : CompProperties_AbilityEffect
    {
        public CompProperties_ToggleableHediff()
        {
            this.compClass = typeof(CompAbilityEffect_ToggleableHediff);
        }
        public HediffDef hediffDef;
    }
    public class CompAbilityEffect_ToggleableHediff : CompAbilityEffect
    {
        public new CompProperties_ToggleableHediff Props
        {
            get
            {
                return (CompProperties_ToggleableHediff)this.props;
            }
        }
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if(target.Pawn != null && Props.hediffDef != null)
            {
                if (target.Pawn.health.hediffSet.HasHediff(Props.hediffDef))
                {
                    target.Pawn.health.RemoveHediff(target.Pawn.health.hediffSet.GetFirstHediffOfDef(Props.hediffDef));
                }
                else
                {
                    HediffComp_RemoveIfAbilityRemoved hediffComp = 
                    target.Pawn.health.AddHediff(Props.hediffDef).TryGetComp<HediffComp_RemoveIfAbilityRemoved>();
                    if (hediffComp != null)
                    {
                        hediffComp.ability = this.parent;
                    }
                }
            } 
        }
    }
}