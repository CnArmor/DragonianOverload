using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace Dragonian
{
    public class IncidentWorker_WildDragonianTribe : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!base.CanFireNowSub(parms))
            {
                return false;
            }

            Map map = (Map)parms.target;
            IntVec3 intVec;
            return !map.GameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && map.mapTemperature.SeasonAcceptableFor(DragonianRaceDefOf.Dragonian_Female) && this.TryFindEntryCell(map, out intVec);
        }

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 loc;

			if (!this.TryFindEntryCell(map, out loc))
			{
				return false;
			}

            int spawnNumber = Rand.RangeInclusive(2, 5);
            int stayTime = Rand.RangeInclusive(5000, 7500);//3~7days

            Pawn pawn = null;

            for(int i = 0; i < spawnNumber; i++)
            {
                pawn = PawnGenerator.GeneratePawn(DragonianPawnKindDefOf.Dragonian_Female, Find.FactionManager.FirstFactionOfDef(DragonianFactionDefOf.Dragonians_Hidden));
                pawn.SetFaction(null, null);
                GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
                pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + stayTime;
            }

            base.SendStandardLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef, parms, pawn, Array.Empty<NamedArgument>());
            return true;
		}
        private bool TryFindEntryCell(Map map, out IntVec3 cell)
        {
            return RCellFinder.TryFindRandomPawnEntryCell(out cell, map, CellFinder.EdgeRoadChance_Neutral, false, null);
        }
    }
}
