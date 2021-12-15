using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace Dragonian
{
    public abstract class WorkGiver_GatherDragonianBodyResources : WorkGiver_Scanner
    {
        protected abstract JobDef JobDef { get; }

        protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.mapPawns.FreeColonistsAndPrisonersSpawned;
        }

        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            List<Pawn> potentialTargets = pawn.Map.mapPawns.FreeColonistsAndPrisonersSpawned;
            foreach (Pawn target in potentialTargets)
            {
                if (target.def == DragonianRaceDefOf.Dragonian_Female)
                    return false;
            }
            return true;
        }

        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

        public override bool HasJobOnThing(Pawn pawn, Thing thing, bool forced = false)
        {
            Pawn targetPawn = thing as Pawn;
            if (targetPawn == null || targetPawn.def != DragonianRaceDefOf.Dragonian_Female)
            {
                return false;
            }
            CompHasGatherableBodyResource comp = GetComp(targetPawn);
            if (comp != null && comp.ActiveAndFull && PawnUtility.CanCasuallyInteractNow(targetPawn, false, false, true) 
                && pawn.CanReserve(targetPawn, 1, -1, null, forced))
            {
                    return true;
            }
            return false;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(JobDef, t);
        }
    }  
    public class WorkGiver_MilkDragonian : WorkGiver_GatherDragonianBodyResources
    {
        protected override JobDef JobDef
        {
            get
            {
                return DragonianJobDefOf.MilkingDragonian;
            }
        }

        public override bool HasJobOnThing(Pawn pawn, Thing thing, bool forced = false)
        {
            return (base.HasJobOnThing(pawn, thing, forced) && pawn != thing);
        }

        protected override CompHasGatherableBodyResource GetComp(Pawn animal)
        {
            return ThingCompUtility.TryGetComp<CompMilkable>(animal);
        }
    }
    
    public class WorkGiver_ShearDragonian : WorkGiver_GatherDragonianBodyResources
    {
        protected override JobDef JobDef
        {
            get
            {
                return DragonianJobDefOf.ShearingDragonian;
            }
        }
        public override bool HasJobOnThing(Pawn pawn, Thing thing, bool forced = false)
        {
            return (base.HasJobOnThing(pawn, thing, forced) && pawn != thing);
        }

        protected override CompHasGatherableBodyResource GetComp(Pawn animal)
        {
            return ThingCompUtility.TryGetComp<CompShearable>(animal);
        }
    }

}
