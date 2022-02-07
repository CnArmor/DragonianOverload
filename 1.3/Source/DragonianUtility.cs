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

        public static PoweredArmorPowerSource FindPowerSource(this ThingWithComps thing)
        {
            if (thing != null)
            {
                if (thing is Apparel apparel && apparel.Wearer != null)
                {
                    if (apparel is PoweredArmorPowerSource)
                    {
                        return (PoweredArmorPowerSource)apparel;
                    }
                    else
                    {
                        foreach (Apparel ap in apparel.Wearer.apparel.WornApparel)
                        {
                            if (ap is PoweredArmorPowerSource)
                            {
                                return (PoweredArmorPowerSource)ap;
                            }
                        }
                    }
                }
                if (thing.TryGetComp<CompEquippable>() != null && thing.ParentHolder != null)
                {
                    Pawn_EquipmentTracker pawn_EquipmentTracker = thing.ParentHolder as Pawn_EquipmentTracker;
                    if (pawn_EquipmentTracker != null && pawn_EquipmentTracker.pawn?.apparel?.WornApparelCount > 0)
                    {
                        foreach (Apparel ap in pawn_EquipmentTracker.pawn.apparel.WornApparel)
                        {
                            if (ap is PoweredArmorPowerSource)
                            {
                                return (PoweredArmorPowerSource)ap;
                            }
                        }
                    }
                }
                if (thing is Pawn pawn)
                {
                    if(pawn.apparel?.WornApparelCount > 0)
                    {
                        foreach (Apparel ap in pawn.apparel.WornApparel)
                        {
                            if(ap is PoweredArmorPowerSource)
                            {
                                return (PoweredArmorPowerSource)ap;
                            }
                        } 
                    }
                }
            }
            return null;
        }
        public static EffecterDef DirectionalBlushEffecterDef(this Pawn pawn)
        {
            if (pawn != null)
            {
                //Log.Message("Pawn: " + pawn.Name + ", Rotation: " + pawn.Rotation + " (South: " + Rot4.South + " North: " + Rot4.North + " East: " + Rot4.East + " West: " + Rot4.West + ")");
                if (pawn.Rotation == Rot4.South)
                {
                    //Log.Message("Return south def");
                    return DragonianEffecterDefOf.Dragonian_Effecter_Blush_south;
                }
                if (pawn.Rotation == Rot4.North)
                {
                    //Log.Message("Return north def");
                    return DragonianEffecterDefOf.Dragonian_Effecter_Blush_north;
                }
                if (pawn.Rotation == Rot4.East)
                {
                    //Log.Message("Return east def");
                    return DragonianEffecterDefOf.Dragonian_Effecter_Blush_east;
                }
                if (pawn.Rotation == Rot4.West)
                {
                    //Log.Message("Return west def");
                    return DragonianEffecterDefOf.Dragonian_Effecter_Blush_west;
                }
                //Log.Message("Error no rotation found");
                return null;
            }
            //Log.Message("Error pawn is null");
            return null;
        }
    }
}