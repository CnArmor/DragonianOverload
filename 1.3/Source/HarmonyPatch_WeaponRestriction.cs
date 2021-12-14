using Verse;
using AlienRace;
using HarmonyLib;

namespace Dragonian
{
    [HarmonyPatch(typeof(RaceRestrictionSettings),"CanEquip")]
    public class Patch_RaceRestrictionSettings
    {
        [HarmonyPostfix]
        private static void Postfix(ref bool __result, ThingDef weapon, ThingDef race)
        {
            if (race == DragonianRaceDefOf.Dragonian_Female && weapon.IsMeleeWeapon)
                __result = true;
        }
    }
}