using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Dragonian
{
    [StaticConstructorOnStartup]

    public class Command_PoweredAbilityAction : Command
    {
        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
            this.action();
        }

        public override void GizmoUpdateOnMouseover()
        {
            base.GizmoUpdateOnMouseover();
            if (this.onHover != null)
            {
				this.onHover();
            }
        }

		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			if (comp?.cooldownTicksRemaining > 0)
			{
				float num = Mathf.InverseLerp((float)comp.Props.coolDownTicks, 0f, (float)comp.cooldownTicksRemaining);
				Widgets.FillableBar(rect, num, cooldownBarTex, null, false);
				if (comp.cooldownTicksRemaining > 0)
				{
					Text.Font = GameFont.Tiny;
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect, num.ToStringPercent("F0"));
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
			if (result.State == GizmoState.Interacted)
			{
				return result;
			}
			return new GizmoResult(result.State);
		}

		public Action action;
        public Action onHover;
		private static Texture2D cooldownBarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(9, 203, 4, 64));
		public Comp_PoweredStaggerImmunity comp;
	}
}