using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using HarmonyLib;
using System.Reflection.Emit;
using System.Reflection;

namespace Dragonian
{
    [HarmonyPatch(typeof(TameUtility), "CanTame")]
    public class Patch_TameUtility //fix to enable taming on wild dragonian
    {
        [HarmonyPostfix]
        private static void TameUtilityPostfix(ref bool __result, ref Pawn pawn)
        {
            if (pawn.IsWildDragonian() && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction))
                __result = true;
        }
    }

    [HarmonyPatch(typeof(Designator_Hunt), "CanDesignateThing")]
    public class Patch_Designator_HuntTranspiler //fix to allow designate hunt on wild dragonian. Why would you do this?
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Designator_HuntTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            //what this does is, find the original method called in the code(WildManUtility.AnimalOrWildMan), and swap it with my method(DragonianUtility.AnimalOrWildManOrWildDragonian)
            MethodInfo animalOrWildMan = AccessTools.Method(typeof(WildManUtility), "AnimalOrWildMan");
            MethodInfo animalOrWildManOrWildDragonian = AccessTools.Method(typeof(DragonianUtility), "AnimalOrWildManOrWildDragonian");
            List<CodeInstruction> codes = instructions.ToList();
            foreach (CodeInstruction code in codes)
            {
                if (code.Calls(animalOrWildMan))
                {
                    code.operand = animalOrWildManOrWildDragonian;
                }
                yield return code;
            }
        }
    }

    [HarmonyPatch(typeof(WorkGiver_HunterHunt), "HasJobOnThing")]
    public class Patch_WorkGiver_HunterHuntTranspiler //fix to allow hunter to hunt wild dragonian. You monster.
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> WorkGiver_HunterHuntTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo animalOrWildMan = AccessTools.Method(typeof(WildManUtility), "AnimalOrWildMan");
            MethodInfo animalOrWildManOrWildDragonian = AccessTools.Method(typeof(DragonianUtility), "AnimalOrWildManOrWildDragonian");
            List<CodeInstruction> codes = instructions.ToList();
            foreach (CodeInstruction code in codes)
            {
                if (code.Calls(animalOrWildMan))
                {
                    code.operand = animalOrWildManOrWildDragonian;
                }
                yield return code;
            }
        }
    }

    [HarmonyPatch(typeof(PawnColumnWorker_Hunt), "HasCheckbox")]
    public class Patch_PawnColumnWorker_Hunt //fix to allow designate hunt wild dragonian from wildlif tab
    {
        [HarmonyPostfix]
        private static void PawnColumnWorker_HuntPostfix(ref bool __result, ref Pawn pawn)
        {
            if (pawn.IsWildDragonian() && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction))
                __result = true;
        }
    }

    [HarmonyPatch(typeof(Pawn_MindState), "Notify_DamageTaken")]
    public class Patch_Pawn_MindStateTranspiler //fix for wild dragonian to start man-hunting when damaged
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Pawn_MindStateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo isWildMan = AccessTools.Method(typeof(WildManUtility), "IsWildMan");
            MethodInfo isWildManOrWildDragonian = AccessTools.Method(typeof(DragonianUtility), "IsWildManOrWildDragonian");
            List<CodeInstruction> codes = instructions.ToList();
            foreach (CodeInstruction code in codes)
            {
                if (code.Calls(isWildMan))
                {
                    code.operand = isWildManOrWildDragonian;
                }
                yield return code;
            }
        }
    }

    [HarmonyPatch(typeof(PawnNameColorUtility), "PawnNameColorOf")]
    public class Patch_PawnNameColorUtilityTranspiler //change wild dragonian name color
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> PawnNameColorUtilityTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo isWildMan = AccessTools.Method(typeof(WildManUtility), "IsWildMan");
            MethodInfo isWildManOrWildDragonian = AccessTools.Method(typeof(DragonianUtility), "IsWildManOrWildDragonian");
            List<CodeInstruction> codes = instructions.ToList();
            foreach (CodeInstruction code in codes)
            {
                if (code.Calls(isWildMan))
                {
                    code.operand = isWildManOrWildDragonian;
                }
                yield return code;
            }
        }
    }

    [HarmonyPatch(typeof(FoodUtility), "BestFoodSourceOnMap")]
    public class Patch_FoodUtilityTranspiler //fix for finding food on map for taming wild dragonian
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> FoodUtilityTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo nonHumanlikeOrWildMan = AccessTools.Method(typeof(WildManUtility), "NonHumanlikeOrWildMan");
            MethodInfo nonHumanlikeOrWildManOrWildDragonian = AccessTools.Method(typeof(DragonianUtility), "NonHumanlikeOrWildManOrWildDragonian");
            List<CodeInstruction> codes = instructions.ToList();
            foreach (CodeInstruction code in codes)
            {
                if (code.Calls(nonHumanlikeOrWildMan))
                {
                    code.operand = nonHumanlikeOrWildManOrWildDragonian;
                }
                yield return code;
            }
        }
    }

    [HarmonyPatch(typeof(MainTabWindow_Wildlife))]
    [HarmonyPatch("Pawns", MethodType.Getter)]
    public class Patch_MainTabWindow_Wildlife
    {
        [HarmonyPostfix]
        private static void MainTabWindow_WildlifePostfix(ref IEnumerable<Pawn> __result) //fix to add wild dragonian to wildlife tab
        {
            List<Pawn> alteredResult = __result.ToList();
            foreach (Pawn p in Find.CurrentMap.mapPawns.AllPawns)
            {
                if (p.Spawned && p.IsWildDragonian() && (p.Faction == null || !p.Faction.def.humanlikeFaction))
                    alteredResult.Add(p);
            }
            __result = alteredResult.AsEnumerable();
        }
    }

    [HarmonyPatch(typeof(FloatMenuMakerMap), "AddHumanlikeOrders")]
    public class Patch_FloatMenuMakerMapTranspiler
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> FloatMenuMakerMapTranspiler(IEnumerable<CodeInstruction> instructions) //fix to allow right click arrest wild dragonian
        {
            MethodInfo isWildMan = AccessTools.Method(typeof(WildManUtility), "IsWildMan");
            MethodInfo isWildManOrWildDragonian = AccessTools.Method(typeof(DragonianUtility), "IsWildManOrWildDragonian");
            List<CodeInstruction> codes = instructions.ToList();
            foreach (CodeInstruction code in codes)
            {
                if (code.Calls(isWildMan))
                {
                    code.operand = isWildManOrWildDragonian;
                }
                yield return code;
            }
        }
    }

    [HarmonyPatch(typeof(InteractionWorker_RecruitAttempt), "Interacted")]
    public class Patch_InteractionWorker_RecruitAttemptTranspiler
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> InteractionWorker_RecruitAttemptTranspiler(IEnumerable<CodeInstruction> instructions) //fix to make wildness actually affect taming chance
        {
            MethodInfo animalOrWildMan = AccessTools.Method(typeof(WildManUtility), "AnimalOrWildMan");
            MethodInfo animalOrWildManOrWildDragonian = AccessTools.Method(typeof(DragonianUtility), "AnimalOrWildManOrWildDragonian");
            List<CodeInstruction> codes = instructions.ToList();
            foreach (CodeInstruction code in codes)
            {
                if (code.Calls(animalOrWildMan))
                {
                    code.operand = animalOrWildManOrWildDragonian;
                }
                yield return code;
            }
        }
    }

    [HarmonyPatch(typeof(StatWorker), "ShouldShowFor")]
    public class Patch_StatWorkerTranspiler
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> StatWorkerTranspiler(IEnumerable<CodeInstruction> instructions) //fix to show minimum hadling skill stat on wild dragonians
        {
            MethodInfo isWildMan = AccessTools.Method(typeof(WildManUtility), "IsWildMan");
            MethodInfo isWildManOrWildDragonian = AccessTools.Method(typeof(DragonianUtility), "IsWildManOrWildDragonian");
            List<CodeInstruction> codes = instructions.ToList();
            foreach (CodeInstruction code in codes)
            {
                if (code.Calls(isWildMan))
                {
                    code.operand = isWildManOrWildDragonian;
                }
                yield return code;
            }
        }
    }

    [HarmonyPatch(typeof(ITab_Pawn_Visitor), "FillTab")]
    public class Patch_ITab_Pawn_VisitorTranspiler
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> ITab_Pawn_VisitorTranspiler(IEnumerable<CodeInstruction> instructions) //fix to disable certain prisoner interactions on wild dragonians
        {
            MethodInfo isWildMan = AccessTools.Method(typeof(WildManUtility), "IsWildMan");
            MethodInfo isWildManOrWildDragonian = AccessTools.Method(typeof(DragonianUtility), "IsWildManOrWildDragonian");
            List<CodeInstruction> codes = instructions.ToList();
            foreach (CodeInstruction code in codes)
            {
                if (code.Calls(isWildMan))
                {
                    code.operand = isWildManOrWildDragonian;
                }
                yield return code;
            }
        }
    }
}