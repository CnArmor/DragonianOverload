using System.Linq;
using RimWorld;
using Verse;

namespace Dragonian
{
    [StaticConstructorOnStartup]

    public class HediffCompProperties_AutoRecovery : HediffCompProperties
    {
        public IntRange healScarInterval = new IntRange(5000,7500);
        public FloatRange healScarAmount = new FloatRange(0.1f,0.3f);

        public HediffCompProperties_AutoRecovery() 
        {
            this.compClass = typeof(HediffComp_AutoRecovery);
        }
    }
    public class HediffComp_AutoRecovery : HediffComp
    {
        protected HediffCompProperties_AutoRecovery Props
        {
            get
            {
                return (HediffCompProperties_AutoRecovery)this.props;
            }
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            if(Find.TickManager.TicksGame >= nextCheckTick)
            {
                //only checks active state every second instead of every tick
                SetActiveState();
                SetNextCheckTick();
            }
            if (Find.TickManager.TicksGame >= nextHealTick)
            { 
                TryHealScar();
                SetNextHealTick();
            }
        }

        public override void CompPostMake()
        {
            base.CompPostMake();
            SetNextCheckTick();
            SetNextHealTick();
        }

        public override void CompExposeData()
        {
            Scribe_Values.Look<int>(ref nextHealTick, "DragonianOverload.AutoRecovery.nextHealTick", 0, false);
            Scribe_Values.Look<int>(ref nextCheckTick, "DragonianOverload.AutoRecovery.nextCheckTick", 0, false);
        }

        private void SetNextHealTick()
        {
            nextHealTick = Find.TickManager.TicksGame + Props.healScarInterval.RandomInRange;
            //Log.Message("Pawn name: "+ Pawn.Name+", current tick: "+Find.TickManager.TicksGame+", next tick set at: "+nextTick+", which is "+ (nextTick- Find.TickManager.TicksGame)+" ticks later.");
            healAmount = Props.healScarAmount.RandomInRange;
        }
        private void SetNextCheckTick()
        {
            nextCheckTick = Find.TickManager.TicksGame + 60;
        }
        private void TryHealScar()
        {
            Hediff scarToHeal;
            if(isActive == true)
            {
                //To see if permanent scars exist. If exist, select a random scar; if don't exist, stop the method.
                if(!(from h in Pawn.health.hediffSet.GetHediffs<Hediff>()
                     where h.def != HediffDefOf.MissingBodyPart && h.IsPermanent() && h.def.isBad
                     select h).TryRandomElement(out scarToHeal))
                {
                    return;
                }
                //Log.Message("Try heal scar of " + Pawn.Name + ", scar to heal: " + scarToHeal.ToString()+", heal amount: "+healAmount);
                scarToHeal.Severity -= healAmount;
            }
        }

        private void SetActiveState()
        {
            Hediff scar = GetPermanentScar();
            Hediff autoRecovery = Pawn.health.hediffSet.GetFirstHediffOfDef(Def);
            if (scar != null && autoRecovery.Severity <= 0.5f)
            {
                isActive = true;
                //Log.Message("Scar found: " + scar.ToString() + ", activate auto recovery.");
                autoRecovery.Severity = 0.6f;
            }

            if (scar == null && autoRecovery.Severity >= 0.5f)
            {
                isActive = false;
                //Log.Message("No scar found, deactivate auto recovery");
                autoRecovery.Severity = 0.1f;
            }
        }

        private Hediff GetPermanentScar()
        {
            foreach (Hediff h in Pawn.health.hediffSet.GetHediffs<Hediff>())
            {
                if (h.def != HediffDefOf.MissingBodyPart && h.IsPermanent() && h.def.isBad)
                    return h;
            }
            return null;
        }

        private int nextHealTick;
        private int nextCheckTick;
        private float healAmount;
        private bool isActive;
    }

}