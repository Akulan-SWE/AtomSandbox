using System.Xml.Serialization;

namespace AtomSandbox.Export
{
    public class ColorInfo
    {
        public required string Name { get; set; }

        [XmlIgnore]
        public Color Color { get; set; }

        [XmlElement("Color")]
        public int ParticleColorAsArgb
        {
            get => Color.ToArgb();
            set => Color = Color.FromArgb(value);
        }
    }
}
