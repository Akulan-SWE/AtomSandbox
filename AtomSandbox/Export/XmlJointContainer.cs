using AtomSandbox.Simulation;

namespace AtomSandbox.Export
{
    public class XmlJointContainer
    {
        public class JointInfo
        {
            public Guid ParticleID { get; set; }
            public List<Guid>? LinkedParticles { get; set; }
        }

        public class DistanceJointInfo
        {
            public Guid FirstParticleID { get; set; }
            public Guid SecondParticleID { get; set; }
        }

        public List<DistanceJoint>? Joints { get; set; }
        public JointInfo[]? Links { get; set; }

        public static implicit operator JointContainer(XmlJointContainer xjc)
        {
            return new JointContainer
            {
                Joints = xjc.Joints,
                Links = xjc.Links.ToDictionary(j => j.ParticleID, j => new HashSet<Guid>(j.LinkedParticles))
            };
        }

        public static implicit operator XmlJointContainer(JointContainer jx)
        {
            return new XmlJointContainer
            {
                Joints = jx.Joints,
                Links = jx.Links.Select(ji => new JointInfo
                {
                    ParticleID = ji.Key,
                    LinkedParticles = ji.Value.ToList()
                })
                .ToArray()
            };
        }
    }
}
