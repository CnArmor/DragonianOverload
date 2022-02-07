using Verse;

namespace Dragonian
{
    [StaticConstructorOnStartup]
    public class HediffCompProperties_RemoveIfPowerSourceNullOrInactive : HediffCompProperties
    {
        public HediffCompProperties_RemoveIfPowerSourceNullOrInactive()
        {
            this.compClass = typeof(HediffComp_RemoveIfPowerSourceNullOrInactive);
        }
    }
    public class HediffComp_RemoveIfPowerSourceNullOrInactive : HediffComp
    {
        public override bool CompShouldRemove
        {
            get
            {
                if (parent.pawn.FindPowerSource() == null)
                    return true;
                else if (parent.pawn.FindPowerSource().IsActivated != true)
                    return true;
                return false;
            }
        }
    }
}