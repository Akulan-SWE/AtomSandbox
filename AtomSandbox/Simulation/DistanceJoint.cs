using System.Numerics;
using System.Xml.Serialization;

namespace AtomSandbox.Simulation
{
    public class DistanceJoint
    {
        private static float MinError = 0.001f;

        [XmlIgnore]
        public Particle FirstParticle { get; set; }
        [XmlIgnore]
        public Particle SecondParticle { get; set; }

        public Guid FirstParticleID { get; set; }
        public Guid SecondParticleID { get; set; }

        public float Distance { get; set; }

        public DistanceJoint()
        {

        }

        public DistanceJoint(Particle p1, Particle p2, float d)
        {
            FirstParticle = p1;
            SecondParticle = p2;
            FirstParticleID = p1.ParticleID;
            SecondParticleID = p2.ParticleID;
            Distance = d;
        }

        public DistanceJoint(Particle p1, Particle p2)
            : this(p1, p2, (p2.Position - p1.Position).Length())
        {

        }

        public bool Joints(Particle p1, Particle p2)
        {
            return FirstParticle == p1 && SecondParticle == p2 || FirstParticle == p2 && SecondParticle == p1;
        }

        public bool Joints(Guid pid1, Guid pid2)
        {
            return FirstParticle.ParticleID == pid1 && SecondParticle.ParticleID == pid2
                || FirstParticle.ParticleID == pid2 && SecondParticle.ParticleID == pid1;
        }

        public Vector2 ApplyJoint(float delta, float e)
        {
            var diff = SecondParticle.Position - FirstParticle.Position;
            var sqDist = diff.LengthSquared();

            if (Math.Abs(sqDist - Distance * Distance) > MinError)
            {
                var dist = (float)Math.Sqrt(sqDist);
                var relDist = dist - Distance;
                var unit = diff / dist;

                var relVel = Vector2.Dot(SecondParticle.Velocity - FirstParticle.Velocity, unit) + relDist / delta;
                float imc = 1 / (FirstParticle.Mass + SecondParticle.Mass);
                var impulse = (1 + e) * unit * relVel * imc;

                FirstParticle.Velocity += SecondParticle.Mass * impulse;
                SecondParticle.Velocity -= FirstParticle.Mass * impulse;

                return impulse;
            }

            return Vector2.Zero;
        }
    }
}
