using System.Xml.Serialization;

using AtomSandbox.Simulation;

namespace AtomSandbox.Export
{
    public class SceneInfo
    {
        public List<Particle>? Particles { get; set; }

        public List<Particle>? Points { get; set; }

        [XmlElement(Type = typeof(XmlJointContainer))]
        public JointContainer? JointContainer { get; set; }
    }
}
