using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;


namespace Dragonian
{
    public static class DragonianUtility
    {
        public static bool IsDragonian(this Pawn pawn)
        {
            return pawn.def == DragonianRaceDefOf.Dragonian_Female;
        }

        public static bool IsWildDragonian(this Pawn pawn)
        {
            return pawn.kindDef == DragonianPawnKindDefOf.Dragonian_Wild;
        }

        public static bool IsWildManOrWildDragonian(this Pawn pawn)
        {
            return (pawn.IsWildMan() || pawn.IsWildDragonian());
        }

        public static bool NonHumanlikeOrWildManOrWildDragonian(this Pawn pawn)
        {
            return (pawn.NonHumanlikeOrWildMan() || pawn.IsWildDragonian());
        }

        public static bool AnimalOrWildManOrWildDragonian(this Pawn pawn)
        {
            return (pawn.AnimalOrWildMan() || pawn.IsWildDragonian());
        }
    }
}