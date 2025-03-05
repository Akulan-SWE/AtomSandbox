using System.Numerics;

namespace AtomSandbox.Export
{
    internal class FormInfo
    {
        public RangeControlInfo[]? RangeControls { get; set; }
        public CheckBoxInfo[]? CheckBoxControls { get; set; }
        public ColorInfo[]? ColorControls { get; set; }
        public VectorInfo[]? VectorControls { get; set; }
        public StateInfo[]? StateControls { get; set; }

        public Vector2 StartPushPoint { get; set; }
        public Vector2 EndPushPoint { get; set; }

        public float ZoomPow { get; set; }
        public Vector2 SceneOffset { get; set; }
    }
}
