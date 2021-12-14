using RimWorld;
using Verse;

//Obsolete. Now I use Harmony Patch for the same result. Left for reference purpose.
/*
namespace Dragonian
{
    
    [StaticConstructorOnStartup]
    public class CompProperties_DragonianInitial : CompProperties
    {
        public AbilityDef abilityDef = DragonianAbilityDefOf.Dragonian_DragonbloodOverload;
        public HediffDef hediffDef = DragonianHediffDefOf.Dragonian_AutoRecovery;
        public CompProperties_DragonianInitial()
        {
            compClass = typeof(Comp_DragonianInitial);
        }

    }

    public class Comp_DragonianInitial: ThingComp
    {
        protected CompProperties_DragonianInitial Props
        {
            get
            {
                return (CompProperties_DragonianInitial)this.props;
            }
        }

        private bool done = false;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<bool>(ref this.done, "DragonianOverload.DragonianInitial.done", false, false);
        }

        public override void CompTickRare()
        {
            base.CompTickRare();

            if (!done)
            {
                Pawn pawn = this.parent as Pawn;
                if (!pawn.health.hediffSet.hediffs.Any(hd => hd.def == DragonianHediffDefOf.Dragonian_AutoRecovery))
                {
                    pawn.health.AddHediff(DragonianHediffDefOf.Dragonian_AutoRecovery);
                }
                if (!pawn.abilities.abilities.Any(ab => ab.def == DragonianAbilityDefOf.Dragonian_DragonbloodOverload))
                {
                    pawn.abilities.GainAbility(DragonianAbilityDefOf.Dragonian_DragonbloodOverload);
                }
            }
            done = true;
        }
    }
}
*/