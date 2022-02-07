using RimWorld;
using Verse;

namespace Dragonian
{
    [StaticConstructorOnStartup]
    public class HediffCompProperties_RemoveIfAbilityRemoved : HediffCompProperties
    {
        public HediffCompProperties_RemoveIfAbilityRemoved()
        {
            this.compClass = typeof(HediffComp_RemoveIfAbilityRemoved);
        }
    }
    public class HediffComp_RemoveIfAbilityRemoved : HediffComp 
    {
        public override bool CompShouldRemove
        {
            get
            {
                return parent.pawn.abilities.GetAbility(ability.def) == null;
            }
        }
        public Ability ability;
    }
}