using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

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

            IntVec3 gatherSpot; 
            RCellFinder.TryFindRandomCellOutsideColonyNearTheCenterOfTheMap(loc, map, 10f, out gatherSpot);
            if (gatherSpot == null)
                gatherSpot = CellFinder.RandomNotEdgeCell((int)Math.Round(map.Size.x * 0.25), map);

            int stayTime = Rand.RangeInclusive(180000, 300000);//3~5days

            List<Pawn> pawnList = new List<Pawn>();

            int spawnNumber = Rand.RangeInclusive(2, 5);

            Pawn pawn;

            for(int i = 0; i < spawnNumber; i++)
            {
                IntVec3 spawnLoc = CellFinder.RandomClosewalkCellNear(loc, map, 5, null);
                pawn = PawnGenerator.GeneratePawn(DragonianPawnKindDefOf.Dragonian_Female, Find.FactionManager.FirstFactionOfDef(DragonianFactionDefOf.Dragonians_Hidden));
                pawn.SetFaction(null, null);
                GenSpawn.Spawn(pawn, spawnLoc, map, WipeMode.Vanish);
                pawnList.Add(pawn);
            }

            LordMaker.MakeNewLord(null, new LordJob_WildDragonianTribe(gatherSpot, stayTime), map, pawnList);

            base.SendStandardLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef, parms, pawnList, Array.Empty<NamedArgument>());
            return true;
		}
        private bool TryFindEntryCell(Map map, out IntVec3 cell)
        {
            return RCellFinder.TryFindRandomPawnEntryCell(out cell, map, CellFinder.EdgeRoadChance_Neutral, false, null);
        }
    }
}
