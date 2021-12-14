using Verse;

namespace Dragonian
{
    [StaticConstructorOnStartup]

    public class MeleeModifier : DefModExtension
    {
        public float meleeDamageMultiplier = 1f;
        public float meleeCooldownMultiplier = 1f;
    }
}