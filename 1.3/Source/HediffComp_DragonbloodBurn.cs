using RimWorld;
using Verse;

namespace Dragonian
{
    [StaticConstructorOnStartup]

    public class HediffCompProperties_DragonbloodBurn : HediffCompProperties
    {
        public HediffCompProperties_DragonbloodBurn()
        {
            compClass = typeof(HediffComp_DragonbloodBurn);
        }
    }

    public class HediffComp_DragonbloodBurn : HediffComp
    {
        public override void CompPostMake()
        {
            HediffUtility.TryGetComp<HediffComp_GetsPermanent>(parent).IsPermanent = true;
        }
    }
}