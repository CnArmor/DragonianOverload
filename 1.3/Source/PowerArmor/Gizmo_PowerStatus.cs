using RimWorld;
using Verse;
using UnityEngine;

namespace Dragonian
{
    [StaticConstructorOnStartup]
    public class Gizmo_PowerStatus : Gizmo
    {
        public Gizmo_PowerStatus()
        {
            this.order = -100f;
        }
        public override float GetWidth(float maxWidth)
        {
            return 140f;
        }
        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
            Rect rect2 = rect.ContractedBy(6f);
            Widgets.DrawWindowBackground(rect);
            Rect rect3 = rect2;
            rect3.height = rect.height / 2f;
            Text.Font = GameFont.Tiny;
            Widgets.Label(rect3, this.powerSource.LabelCap);
            Rect rect4 = rect2;
            rect4.yMin = rect2.y + rect2.height / 2f;
            float fillPercent = this.powerSource.Power / Mathf.Max(1f, this.powerSource.GetStatValue(DragonianStatDefOf.DRO_PowerMax, true));
            if (powerSource.IsActivated)
            {
                Widgets.FillableBar(rect4, fillPercent, powerBarActivatedTex, powerBarEmptyTex, false);
            }
            if (!powerSource.IsActivated)
            {
                Widgets.FillableBar(rect4, fillPercent, powerBarDeactivatedTex, powerBarEmptyTex, false);
            }
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(rect4, (this.powerSource.Power).ToString("F0") + " / " + (this.powerSource.GetStatValue(DragonianStatDefOf.DRO_PowerMax, true)).ToString("F0"));
            Text.Anchor = TextAnchor.UpperLeft;
            return new GizmoResult(GizmoState.Clear);
        }
        public PoweredArmorPowerSource powerSource;
        private static readonly Texture2D powerBarActivatedTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.63f, 0.96f, 0.97f));
        private static readonly Texture2D powerBarDeactivatedTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.34f, 0.54f, 0.55f));
        private static readonly Texture2D powerBarEmptyTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

    }
}