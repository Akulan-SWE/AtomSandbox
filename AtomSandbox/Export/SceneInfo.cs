using AtomSandbox.Simulation;

namespace AtomSandbox.Export
{
    public class SceneInfo
    {
        public List<Particle>? Particles { get; set; }

        public List<Particle>? Points { get; set; }

        public XmlJointContainer? JointContainer { get; set; }
    }
}
