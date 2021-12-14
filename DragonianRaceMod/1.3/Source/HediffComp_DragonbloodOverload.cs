using Verse;
using RimWorld;

namespace Dragonian
{
    [StaticConstructorOnStartup]
    public class HediffCompProperties_DragonbloodOverload : HediffCompProperties
    {
        public IntRange burnInterval = new IntRange(180, 300);
        public FloatRange burnAmount = new FloatRange(0.2f, 0.4f);
        public float brainDamageChancePerTick = 0.33f;
        public float stopDamageThreshold = 0.2f;
        public HediffCompProperties_DragonbloodOverload()
        {
            compClass = typeof(HediffComp_DragonbloodOverload);
        }
    }
    public class HediffComp_DragonbloodOverload : HediffComp
    {
        protected HediffCompProperties_DragonbloodOverload Props
        {
            get 
            {
                return (HediffCompProperties_DragonbloodOverload)this.props;
            }
        }

        public override void CompPostTick(ref float severityAdjustment) 
        { 
            if(Find.TickManager.TicksGame >= nextBurnTick)
            {
                SetNextBurnTick();
                TryLeaveBurnScar();
            }
        }

        public override void CompPostMake()
        {
            base.CompPostMake();
            SetNextBurnTick();
        }

        public override void CompExposeData()
        {
            Scribe_Values.Look<int>(ref nextBurnTick, "DragonianOverload.DragonbloodOverload.nextBurnTick", 0, false);
        }
        private void SetNextBurnTick()
        {
            nextBurnTick = Find.TickManager.TicksGame + Props.burnInterval.RandomInRange;
            burnAmount = Props.burnAmount.RandomInRange;
        }

        private void TryLeaveBurnScar()
        {
            BodyPartRecord brain = Pawn.health.hediffSet.GetBrain();
            BodyPartRecord heart = GetHeart();
            Hediff brainBurnScar = GetBrainBurnScar();
            Hediff heartBurnScar = GetHeartBurnScar();
            float brainStopDamageSeverity = 10 * Pawn.def.race.baseHealthScale * (1 - Props.stopDamageThreshold);
            float heartStopDamageSeverity = 15 * Pawn.def.race.baseHealthScale * (1 - Props.stopDamageThreshold);
            if (Rand.Value <= Props.brainDamageChancePerTick)
            {
                if(brainBurnScar != null && brainBurnScar.Severity <= brainStopDamageSeverity)
                {
                    brainBurnScar.Severity += burnAmount;
                    return;
                }
                if(brainBurnScar == null)
                {
                    Pawn.TakeDamage(new DamageInfo(DragonianDamageDefOf.Dragonian_DragonbloodBurn, burnAmount, 2f, -1f, null, brain));
                    return;
                }
                return;
            }
            else
            {
                if(heartBurnScar != null && heartBurnScar.Severity <= heartStopDamageSeverity)
                {
                    heartBurnScar.Severity += burnAmount;
                    return;
                }
                if(heartBurnScar == null)
                {
                    Pawn.TakeDamage(new DamageInfo(DragonianDamageDefOf.Dragonian_DragonbloodBurn, burnAmount, 2f, -1f, null, heart));
                    return;
                }
                return;
            }   
        }

        private BodyPartRecord GetHeart()
        {
            foreach (BodyPartRecord bPR in Pawn.health.hediffSet.GetNotMissingParts())
            {
                if (bPR.def.tags.Contains(BodyPartTagDefOf.BloodPumpingSource))
                    return bPR;
            }
            return null;
        }

        private Hediff GetBrainBurnScar()
        {
            foreach (Hediff hd in Pawn.health.hediffSet.GetHediffs<Hediff>())
            {
                if (hd.Part == Pawn.health.hediffSet.GetBrain() && hd.IsPermanent() && hd.def == DragonianHediffDefOf.Dragonian_DragonbloodBurn)
                    return hd;
            }
            return null;
        }

        private Hediff GetHeartBurnScar()
        {
            foreach (Hediff hd in Pawn.health.hediffSet.GetHediffs<Hediff>())
            {
                if (hd.Part == GetHeart() && hd.IsPermanent() && hd.def == DragonianHediffDefOf.Dragonian_DragonbloodBurn)
                    return hd;
            }
            return null;
        }
        private int nextBurnTick;
        private float burnAmount;
    }
}