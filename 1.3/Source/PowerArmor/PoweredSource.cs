using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.Sound;

namespace Dragonian
{
	[StaticConstructorOnStartup]
	public class PoweredArmorPowerSource : Apparel
	{
		private float PowerMax
        {
            get
            {
                return this.GetStatValue(DragonianStatDefOf.DRO_PowerMax, true);
            }
        }
        private float PowerRechargeRate
        {
            get
            {
                return this.GetStatValue(DragonianStatDefOf.DRO_PowerRechargeRate, true) / 60f;
            }
        }
        public bool IsActivated
        {
            get
            {
                return this.activeStatus;
            }
        }
        public Comp_PoweredStaggerImmunity StaggerImmunity
        {
            get
            {
                foreach(Apparel apparel in this.Wearer.apparel.WornApparel)
                {
                    if (apparel.TryGetComp<Comp_PoweredStaggerImmunity>() != null)
                    {
                        return apparel.TryGetComp<Comp_PoweredStaggerImmunity>();
                    }
                }
                return null;
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref this.power, "Dragonian.PoweredArmorPowerSource.power", 0f, false);
            Scribe_Values.Look<int>(ref this.nextRebootTick, "Dragonian.PoweredArmorPowerSource.nextRebootTick", 0, false);
        }
        public override IEnumerable<Gizmo> GetWornGizmos()
        {
            foreach(Gizmo gizmo in base.GetWornGizmos())
            {
                yield return gizmo;
            }
            if (Find.Selector.SingleSelectedObject == this.Wearer)
            {
                yield return new Gizmo_PowerStatus { powerSource = this };
            }
            yield break;
        }
        public override void Tick()
        {
            base.Tick();
            if (base.Wearer == null)
            {
                power = 1f;
                return;
            }
            if (!IsActivated)
            {
                if(Find.TickManager.TicksGame >= nextRebootTick)
                {
                    Activate();
                }
                return;
            }
            if (power != PowerMax)
            {
                power += PowerRechargeRate;
                if (power > PowerMax)
                    power = PowerMax;
            }

            if (power <= 0f && IsActivated)
                Deactivate();
        }
        public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            base.PreApplyDamage(ref dinfo, out absorbed);
            if (dinfo.Def == DamageDefOf.EMP)
            {
                Deactivate();
                return;
            }
        }
        public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.PostApplyDamage(dinfo, totalDamageDealt);
            if (StaggerImmunity != null && StaggerImmunity.isActivated)
            {
                power -= StaggerImmunity.Props.powerCostPerHit;
                if (power < 0f)
                {
                    power = 0f;
                    StaggerImmunity.isActivated = false;
                    StaggerImmunity.cooldownTicksRemaining = StaggerImmunity.Props.coolDownTicks;
                }

            }
        }

        private void Activate()
        {
            if (base.Wearer.Spawned)
            {
                SoundDefOf.EnergyShield_Reset.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
                FleckMaker.ThrowLightningGlow(base.Wearer.TrueCenter(), base.Wearer.Map, 3f);
            }
            activeStatus = true;
            if (power == 0f)
                power = 1f;
        }
        private void Deactivate()
        {
            if (base.Wearer.Spawned)
            {
                DragonianSoundDefOf.Dragonian_Shutdown.PlayOneShot(new TargetInfo(Wearer.Position, Wearer.Map, false));
            }
            activeStatus = false;
            nextRebootTick = Find.TickManager.TicksGame + rebootTicks;
            if (StaggerImmunity != null)
            {
                StaggerImmunity.isActivated = false;
                StaggerImmunity.cooldownTicksRemaining = StaggerImmunity.Props.coolDownTicks;
            }
        }
        public float power = 1f;
        private bool activeStatus = true;
        private int rebootTicks = 1200;
        private int nextRebootTick;
	}
}