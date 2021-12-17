using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace Dragonian
{ 
    public class DragonianEffecter
    {
        public EffecterDef DynamicShyEffectorDef(Pawn pawn)
        {
            //Log.Message("Pawn: " + pawn.Name + ", Rotation: " + pawn.Rotation + " (South: " + Rot4.South + " North: " + Rot4.North + " East: " + Rot4.East + " West: " + Rot4.West + ")");
            if (pawn?.Rotation == Rot4.South)
            {
                //Log.Message("Return south def");
                return DragonianEffecterDefOf.Dragonian_Effecter_Shy_south;
            }
            if (pawn?.Rotation == Rot4.North)
            {
                //Log.Message("Return north def");
                return DragonianEffecterDefOf.Dragonian_Effecter_Shy_north;
            }
            if (pawn?.Rotation == Rot4.East)
            {
                //Log.Message("Return east def");
                return DragonianEffecterDefOf.Dragonian_Effecter_Shy_east;
            }
            if (pawn?.Rotation == Rot4.West)
            {
                //Log.Message("Return west def");
                return DragonianEffecterDefOf.Dragonian_Effecter_Shy_west;
            }
            //Log.Message("Error no rotation found");
            return null;
            
        }
    }

}