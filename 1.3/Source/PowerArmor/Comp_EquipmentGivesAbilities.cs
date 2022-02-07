using System.Collections.Generic;
using Verse;
using RimWorld;

namespace Dragonian
{
    [StaticConstructorOnStartup]
    public class CompProperties_EquipmentGivesAbilities : CompProperties
    {
        public CompProperties_EquipmentGivesAbilities() 
        {
            this.compClass = typeof(Comp_EquipmentGivesAbilities);
        }
        public List<AbilityDef> abilities;
    }
    public class Comp_EquipmentGivesAbilities : ThingComp
    {
        public CompProperties_EquipmentGivesAbilities Props
        {
            get
            {
                return (CompProperties_EquipmentGivesAbilities)this.props;
            }
        }
        public override void Notify_Equipped(Pawn pawn)
        {
            base.Notify_Equipped(pawn);
            if (!Props.abilities.NullOrEmpty())
            {
                foreach (AbilityDef ability in Props.abilities)
                {
                    //Log.Message("Found ability def: " + ability.defName);
                    if (pawn.abilities.GetAbility(ability) == null)
                    {
                        //Log.Message("Giving ability to pawn");
                        pawn.abilities.GainAbility(ability);
                    }
                    //else
                        //Log.Message("duplicated ability found, stop operation");
                }
            }
        }
        public override void Notify_Unequipped(Pawn pawn)
        {
            base.Notify_Unequipped(pawn);
            if (!Props.abilities.NullOrEmpty())
            {
                foreach (AbilityDef ability in Props.abilities)
                {
                    pawn.abilities.RemoveAbility(ability);
                }
            }
        }
    }
}