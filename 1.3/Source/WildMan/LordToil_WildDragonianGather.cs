using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace Dragonian
{
    public class LordToil_WildDragonianGather : LordToil
    {
        public LordToil_WildDragonianGather(IntVec3 gatherSpot)
        {
            this.gatherSpot = gatherSpot;
        }
        public override void UpdateAllDuties()
        {
            for (int i = 0; i < this.lord.ownedPawns.Count; i++)
            {
                PawnDuty duty = new PawnDuty(DragonianDutyDefOf.Dragonian_WildWanderNearPoint, this.gatherSpot, -1f);
                this.lord.ownedPawns[i].mindState.duty = duty;
            }
        }
        private IntVec3 gatherSpot;
    }
}