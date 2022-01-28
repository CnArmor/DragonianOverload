using Verse;
using HarmonyLib;
using RimWorld;

namespace Dragonian
{
    [HarmonyPatch(typeof(Pawn_StanceTracker),"StaggerFor")]
    public class Patch_Pawn_StanceTracker
    {
        [HarmonyPrefix]
        static bool Pawn_StanceTrackerPrefix(Pawn_StanceTracker __instance, ref int ticks)
        {
            //Log.Message("Trying to interfere with pawn stagger" + __instance.pawn?.Name);
            if( __instance.pawn?.apparel != null)
            {
                foreach(Apparel apparel in __instance.pawn.apparel.WornApparel)
                {
                    if (apparel.TryGetComp<Comp_PoweredStaggerImmunity>() != null && apparel.TryGetComp<Comp_PoweredStaggerImmunity>().isActivated)
                    {
                        //Log.Message("Stagger immunity for:" + __instance.pawn?.Name);
                        return false;
                    }
                        
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Thing),"TakeDamage")]
    public class Patch_Thing_TakeDamage
    {
        [HarmonyPrefix]
        static bool TakeDamagePrefix(Thing __instance, ref DamageInfo dinfo)
        {
            if(__instance is Pawn)
            {
                Pawn pawn = (Pawn)__instance;
                {
                    if(pawn.apparel != null)
                    {
                        foreach (Apparel apparel in pawn.apparel.WornApparel)
                        {
                            if (apparel.TryGetComp<Comp_PoweredStaggerImmunity>()!=null && apparel.TryGetComp<Comp_PoweredStaggerImmunity>().isActivated)
                            {
                                dinfo.SetAmount(dinfo.Amount * apparel.TryGetComp<Comp_PoweredStaggerImmunity>().Props.incommingDamageMutiplier);
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}