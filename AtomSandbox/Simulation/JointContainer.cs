using AtomSandbox.Tools;

namespace AtomSandbox.Simulation
{
    public class JointContainer
    {
        private readonly List<Guid> EmptyResult = new List<Guid>();

        public List<DistanceJoint> Joints { get; set; }
        public Dictionary<Guid, HashSet<Guid>> Links { get; set; }

        public JointContainer()
        {
            Links = new Dictionary<Guid, HashSet<Guid>>();
            Joints = new List<DistanceJoint>();
        }

        public void AddJointInfo(Particle p1, Particle p2)
        {
            Links.TryGetValue(p1.ParticleID, out var jointInfo);

            if (jointInfo != null && jointInfo.Contains(p2.ParticleID))
            {
                jointInfo.Remove(p2.ParticleID);
                Links[p2.ParticleID].Remove(p1.ParticleID);
                Joints.RemoveIf((j, i) => j.Joints(p1, p2));
                return;
            }

            if (jointInfo == null)
                Links[p1.ParticleID] = new HashSet<Guid> { p2.ParticleID };
            else
                jointInfo.Add(p2.ParticleID);

            if (Links.TryGetValue(p2.ParticleID, out HashSet<Guid>? value))
                value.Add(p1.ParticleID);
            else
                Links[p2.ParticleID] = new HashSet<Guid> { p1.ParticleID };

            Joints.Add(new DistanceJoint(p1, p2));
        }

        public bool AreParticlesJointed(Particle p1, Particle p2)
        {
            return Links.ContainsKey(p1.ParticleID)
                && Links[p1.ParticleID].Contains(p2.ParticleID);
        }

        public bool ContainsParticle(Particle p)
        {
            return ContainsParticle(p.ParticleID);
        }

        public bool ContainsParticle(Guid pid)
        {
            return Links.ContainsKey(pid);
        }

        public HashSet<Guid> GetJointedParticles(Particle p)
        {
            return Links[p.ParticleID];
        }

        public HashSet<Guid> GetJointedParticles(Guid pid)
        {
            return Links[pid];
        }

        public List<Guid> DeleteParticleJoints(Particle p)
        {
            if (ContainsParticle(p))
            {
                var particles = GetJointedParticles(p);
                foreach (var jointedParticle in particles)
                {
                    Links[jointedParticle].Remove(p.ParticleID);
                    var particle = jointedParticle;
                    Joints.RemoveIf((j, i) => j.Joints(p.ParticleID, particle));
                }

                var result = Links[p.ParticleID].ToList();
                Links.Remove(p.ParticleID);
                return result;
            }

            return EmptyResult;
        }

        public void Clear()
        {
            Links.Clear();
            Joints.Clear();
        }

        public void Copy(JointContainer other)
        {
            Clear();

            Joints = [.. other.Joints];
            Links = new Dictionary<Guid, HashSet<Guid>>(other.Links);
        }
    }
}
