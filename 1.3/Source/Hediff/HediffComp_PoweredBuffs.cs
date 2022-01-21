using System.Linq;
using RimWorld;
using Verse;

namespace Dragonian
{
    [StaticConstructorOnStartup]
    public class HediffComp_PoweredBuffs : HediffComp
    {
        protected PoweredArmorPowerSource powerSource;
        private bool isBuffActive;
        protected virtual PoweredArmorPowerSource PowerSource
        {
            get
            {
                if(powerSource is null)
                {
                    powerSource = this.parent.TryGetComp<HediffComp_RemoveIfApparelDropped>().wornApparel.TryGetComp<Comp_PoweredPassiveBuffs>().PowerSource;
                }
                return powerSource;
            }
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            CheckBuffStatus();
        }

        private void CheckBuffStatus() 
        {
            if(PowerSource.IsActivated && isBuffActive != true)
            {
                parent.Severity = 0.6f;
                isBuffActive = true;
            }
            if(!PowerSource.IsActivated && isBuffActive == true)
            {
                parent.Severity = 0.1f;
                isBuffActive = false;
            }
        }
    }
}