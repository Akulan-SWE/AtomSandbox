using System.Xml.Serialization;

namespace AtomSandbox.Export
{
    [Serializable]
    public class ParticleInfo
    {
        public required string Name { get; set; }
        public float LifeTime { get; set; }
        public float Charge { get; set; }

        [XmlIgnore]
        public Color Color { get; set; }

        [XmlElement("Color")]
        public int ParticleColorAsArgb
        {
            get => Color.ToArgb();
            set => Color = Color.FromArgb(value);
        }

        public int TailLength { get; set; }
        public float Radius { get; set; }
        public float Mass { get; set; }
    }
}
