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
        public PoweredAbility StaggerImmunity
        {
            get
            {
                if(Wearer?.abilities.GetAbility(DragonianAbilityDefOf.Dragonian_PoweredStaggerImmunity) != null)
                {
                    return (PoweredAbility)Wearer.abilities.GetAbility(DragonianAbilityDefOf.Dragonian_PoweredStaggerImmunity);
                }
                return null;
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref this.power, "PowerSource.power", 0f, false);
            Scribe_Values.Look<int>(ref this.nextRebootTick, "PowerSource.nextRebootTick", 0, false);
            Scribe_Values.Look<bool>(ref this.activeStatus, "PowerSource.activeStatus", true, false);
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
            if (power <= 0f && IsActivated)
                Deactivate();
            if (power != PowerMax)
            {
                power += PowerRechargeRate;
                if (power > PowerMax)
                    power = PowerMax;
            }
        }
        public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
        {
            if (dinfo.Def == DamageDefOf.EMP)
            {
                //Log.Message("EMP Damage recieved, deactivating");
                Deactivate();
            }
            return base.CheckPreAbsorbDamage(dinfo);
        }
        public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            if (StaggerImmunity != null && StaggerImmunity.isActive)
            {
                power -= StaggerImmunity.def.GetModExtension<DefModExtension_PoweredAbility>().powerCostPerHitDamageFactor * dinfo.Amount;
                if (power < 0f)
                {
                    power = 0f;
                }
            }
            base.PostApplyDamage(dinfo, totalDamageDealt);
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
        }
        public float power = 1f;
        private bool activeStatus = true;
        private int rebootTicks = 1200;
        private int nextRebootTick;
	}
}