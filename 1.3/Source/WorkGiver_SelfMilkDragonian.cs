using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace Dragonian
{
    public class WorkGiver_SelfMilkDragonian : WorkGiver_GatherDragonianBodyResources
    {
        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.InteractionCell;
            }
        }

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            yield return pawn;
            yield break;
        }

        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            if (pawn.IsDragonian())
                return false;
            else
                return true;
        }

        public override bool HasJobOnThing(Pawn pawn, Thing thing, bool forced = false)
        {
            if (base.HasJobOnThing(pawn, thing, forced) && pawn == thing)
            {
                //Log.Message("Found self milk target: " + pawn.Name);
                return true;
            }
            return false;
        }

        protected override JobDef JobDef
        {
            get
            {
                return DragonianJobDefOf.SelfMilkingDragonian;
            }
        }

        protected override CompHasGatherableBodyResource GetComp(Pawn dragonian)
        {
            return ThingCompUtility.TryGetComp<CompMilkable>(dragonian);
        }
    }
}
