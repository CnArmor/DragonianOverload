using Verse;

namespace Dragonian
{
    [StaticConstructorOnStartup]
    public class HediffComp_PoweredBuffs : HediffComp
    {
        private bool isBuffActive;
        private PoweredArmorPowerSource PowerSource
        {
            get
            {    
                return this.parent.TryGetComp<HediffComp_RemoveIfApparelDropped>().wornApparel.TryGetComp<Comp_PoweredPassiveBuffs>().PowerSource;
            }
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            CheckBuffStatus();
        }

        private void CheckBuffStatus() 
        {
            if(PowerSource == null && isBuffActive != false)
            {
                parent.Severity = 0.1f;
                isBuffActive = false;
            }
            if(PowerSource != null)
            {
                if (PowerSource.IsActivated && isBuffActive != true)
                {
                    parent.Severity = 0.6f;
                    isBuffActive = true;
                }
                if (!PowerSource.IsActivated && isBuffActive == true)
                {
                    parent.Severity = 0.1f;
                    isBuffActive = false;
                }
            }
        }
    }
}