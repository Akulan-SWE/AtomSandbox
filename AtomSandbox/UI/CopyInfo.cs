using AtomSandbox.Simulation;

namespace AtomSandbox.UI
{
    /// <summary>
    /// Auxiliary class for copying particles and joints
    /// </summary>
    internal class CopyInfo
    {
        public Dictionary<Guid, Particle> Particles { get; private set; }
        public Dictionary<Guid, Particle> Points { get; private set; }
        public JointContainer Joints { get; private set; }

        public CopyInfo()
        {
            Particles = new Dictionary<Guid, Particle>();
            Points = new Dictionary<Guid, Particle>();
            Joints = new JointContainer();
        }

        public void Clear()
        {
            Particles.Clear();
            Points.Clear();
            Joints.Clear();
        }
    }
}
