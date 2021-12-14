using System.Reflection;
using RimWorld;
using Verse;
using HarmonyLib;

namespace Dragonian
{
    [StaticConstructorOnStartup]

    public static class Startup
    {
        //must have for Harmony to actually patch things
        static Startup()
        {
            Harmony harmony = new Harmony("Dragonian");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [DefOf]

    public class DragonianRaceDefOf
    {
        public static ThingDef Dragonian_Female;
    }

    
    [DefOf]
    public class DragonianBodyDefOf
    {
        public static BodyDef Dragonian;
    }
    [DefOf]
    public class DragonianPawnKindDefOf
    {
        public static PawnKindDef Dragonian_Female;
    }

    [DefOf]
    public class DragonianAbilityDefOf
    {
        public static AbilityDef Dragonian_DragonbloodOverload;
    }

    [DefOf]
    public class DragonianHediffDefOf
    {
        public static HediffDef Dragonian_AutoRecovery;
        public static HediffDef Dragonian_DragonbloodBurn;
        public static HediffDef Dragonian_DragonbloodOverload;
    }

    [DefOf]

    public class DragonianDamageDefOf
    {
        public static DamageDef Dragonian_DragonbloodBurn;
    }

    [DefOf]
    public class DragonianJobDefOf
    {
        public static JobDef MilkingDragonian;
        public static JobDef ShearingDragonian;
    }

    [HarmonyPatch(typeof(ForbidUtility))]
    [HarmonyPatch("SetForbidden")]
    public static class FobidPatch
    {

        [HarmonyPrefix]
        static bool Prefix(this Thing t, bool value, bool warnOnFail = true)
        {
            if (t == null)
            {
                if (warnOnFail)
                {
                    Log.Error("Tried to SetForbidden on null Thing.");
                }
                return false;
            }
            ThingWithComps thingWithComps = t as ThingWithComps;
            if (thingWithComps == null)
            {
                if (warnOnFail)
                {
                    Log.Error("Tried to SetForbidden on non-ThingWithComps Thing " + t);
                }
                return false;
            }
            CompForbiddable comp = thingWithComps.GetComp<CompForbiddable>();
            if (comp == null)
            {
                return false;
            }
            comp.Forbidden = value;
            return false;
        }

    }
}