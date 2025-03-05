using System.Numerics;

using AtomSandbox.Tools;

namespace AtomSandbox.Simulation
{
    /// <summary>
    /// The environment that contain particles and handles their interactions
    /// </summary>
    internal class Scene
    {
        private static Random rand;

        public List<Particle> Particles {  get; private set; }

        public List<Particle> Points { get; private set; }

        public JointContainer JointedParticles { get; private set; }

        private IEnvironmentVariables enVars;
        private Vector2 origin;

        static Scene()
        {
            rand = new Random();
        }

        public Scene(IEnvironmentVariables environmentVariables)
        {
            Particles = new List<Particle>();
            Points = new List<Particle>();

            JointedParticles = new JointContainer();

            enVars = environmentVariables;
            origin = Vector2.Zero;
        }

        public void Render(Graphics g)
        {
            foreach (var particle in Particles)
            {
                particle.Render(g);
            }

            foreach (var point in Points)
            {
                point.Render(g);
            }
        }

        public void Update(float delta)
        {
            for (int i = 0; i < Particles.Count; ++i)
            {
                var particle = Particles[i];
                if (particle.Mass != 0)
                    updateParticle(i, particle, delta);
            }

            foreach (var joint in JointedParticles.Joints)
            {
                joint.ApplyJoint(delta, enVars.JointElasticityFactor).LengthSquared();
            }

            foreach (var p in Particles)
            {
                postUpdateParticle(p);
                p.Update(delta);
            }
        }

        public void Clear()
        {
            Particles.Clear();
            Points.Clear();
            JointedParticles.Links.Clear();
            JointedParticles.Joints.Clear();
        }

        #region Physics interactions

        private void stabilization(Particle p1, Particle p2, bool affectSecond,
            float sqLen, double dist, Vector2 diff)
        {
            float necessaryDist = p1.Radius + p2.Radius;

            if (enVars.UseExtraStabilization)
            {
                if (p1.Radius >= enVars.ExtraStabilizationStartRadius &&
                    p2.Radius >= enVars.ExtraStabilizationStartRadius)
                    necessaryDist += enVars.ExtraStabilizationDistance;
            }

            if (necessaryDist * necessaryDist > sqLen)
            {
                Vector2 stabilization;
                if (sqLen == 0)
                    stabilization = Vector2.UnitX;
                else
                {
                    dist = dist != -1 ? dist : Math.Sqrt(sqLen);
                    stabilization = -(float)((necessaryDist - dist) / dist) * diff;
                }

                float psk1 = 0.51f, psk2 = 0.51f;
                if (enVars.UseMassProportionalStabilization)
                {
                    if (p1.Mass > p2.Mass)
                    {
                        psk1 = 0;
                        psk2 = 1;
                    }
                    else if (p1.Mass < p2.Mass)
                    {
                        psk1 = 1;
                        psk2 = 0;
                    }
                }
                
                var n = stabilization.ToUnit();
                var v1p = Vector2.Dot(p1.Velocity, n) * n;
                var v2p = Vector2.Dot(p2.Velocity, n) * n;

                float iMassSum = 1 / (p1.Mass + p2.Mass);
                var post_v1p = ((p1.Mass - enVars.BouncityFactor * p2.Mass) * v1p + (1 + enVars.BouncityFactor) * p2.Mass * v2p) * iMassSum;
                var post_v2p = ((p2.Mass - enVars.BouncityFactor * p1.Mass) * v2p + (1 + enVars.BouncityFactor) * p1.Mass * v1p) * iMassSum;

                if (enVars.IsReflectionEnabled)
                    p1.Velocity = enVars.CollisionDampingFactor * (p1.Velocity + (post_v1p - v1p));

                if (affectSecond)
                {
                    if (enVars.IsReflectionEnabled)
                        p2.Velocity = enVars.CollisionDampingFactor * (p2.Velocity + (post_v2p - v2p));
                    p2.Position -= psk2 * stabilization;
                }
                p1.Position += psk1 * stabilization;
            }
        }

        private void processNaturalForces(Particle p1, Particle p2, bool affectSecond, float delta, Vector2 diff,
         float sqLen, out double dist)
        {
            dist = -1;

            if (delta == 0)
                return;

            var forceRadiusSQ = enVars.ForceRadius * enVars.ForceRadius;

            if (sqLen > 0 &&
                (enVars.ForceRadius < 0 && forceRadiusSQ <= sqLen ||
                enVars.ForceRadius > 0 && forceRadiusSQ >= sqLen))
            {
                var iMass1 = 1 / p1.Mass;
                var iMass2 = 1 / p2.Mass;
                var iSqForce = delta * diff / sqLen;

                if (enVars.IsStrongForceEnabled)
                    processStrongForce(p1, p2, iMass1, iMass2, affectSecond, diff, sqLen, ref dist);

                if(enVars.IsQuadForceEnabled)
                    processQuadForce(p1, p2, iMass1, iMass2, affectSecond, diff, sqLen);

                if (enVars.IsGravityForceEnabled)
                    processGravityForce(p1, p2, iMass1, iMass2, affectSecond, diff, sqLen, iSqForce);

                if (enVars.IsMagneticForceEnabled)
                    processMagneticForce(p1, p2, iMass1, iMass2, affectSecond, diff, sqLen, iSqForce);
            }
        }

        private void processStrongForce(Particle p1, Particle p2, float iMass1, float iMass2,
            bool affectSecond, Vector2 diff, float sqLen, ref double dist)
        {
            var sfRadius = enVars.StrongForceRadius;
            var sfBallRadius = enVars.StrongForceMinParticleRadius;

            if (enVars.IsStrongForceEnabled
                && (p1.Radius > sfBallRadius || p2.Radius > sfBallRadius))
            {
                float sfDist = -1;
                bool applyStrongForce = false;
                if (enVars.UseStrongForceSurfaceDistance)
                {
                    dist = Math.Sqrt(sqLen);
                    sfDist = (float)dist - p1.Radius - p2.Radius;
                    if (sfDist > 0 &&
                            (sfRadius < 0 && -sfRadius <= sfDist ||
                            sfRadius > 0 && sfRadius >= sfDist))
                        applyStrongForce = true;
                }
                else
                {
                    var sfRadiusSQ = sfRadius * sfRadius;
                    if (sfRadius < 0 && sfRadiusSQ <= sqLen || sfRadius > 0 && sfRadiusSQ >= sqLen)
                    {
                        dist = Math.Sqrt(sqLen);
                        sfDist = (float)dist;
                        applyStrongForce = true;
                    }
                }

                if (applyStrongForce)
                {
                    var strongForce = (float)(enVars.StrongForcePower *
                        Math.Exp(-sfDist / enVars.StrongForceEffectiveRadius) /
                            sfDist) * diff;

                    p1.Velocity += iMass1 * strongForce;
                    if (affectSecond)
                        p2.Velocity -= iMass2 * strongForce;
                }
            }
        }

        private void processQuadForce(Particle p1, Particle p2, float iMass1, float iMass2,
            bool affectSecond, Vector2 diff, float sqLen)
        {
            var quadForceRadius = enVars.QuadForceRadius;
            var quadForceRadiusSQ = quadForceRadius * quadForceRadius;
            if (p1.Radius > enVars.QuadForceMinParticleRadius && p2.Radius > enVars.QuadForceMinParticleRadius &&
                    (quadForceRadius < 0 && quadForceRadiusSQ <= sqLen ||
                        quadForceRadius > 0 && quadForceRadiusSQ >= sqLen))
            {
                var denom = enVars.QuadForceEffectiveRadius * enVars.QuadForceEffectiveRadius;
                var forceValue = enVars.QuadForcePower * (1 / sqLen - 1 / denom);
                if (!(enVars.QuadForcePower > 0 && forceValue < 0 ||
                    enVars.QuadForcePower < 0 && forceValue > 0))
                {
                    var newForce = forceValue * diff;
                    p1.Velocity += iMass1 * newForce;
                    if (affectSecond)
                        p2.Velocity -= iMass2 * newForce;

                }
            }
        }

        private void processGravityForce(Particle p1, Particle p2, float iMass1, float iMass2,
            bool affectSecond, Vector2 diff, float sqLen, Vector2 iSqForce)
        {
            var gravityForceRadius = enVars.GravityForceRadius;
            var gravityForceRadiusSQ = gravityForceRadius * gravityForceRadius;
            if (gravityForceRadius < 0 && gravityForceRadiusSQ <= sqLen ||
                gravityForceRadius > 0 && gravityForceRadiusSQ >= sqLen)
            {
                var gravityForce = enVars.GravityForcePower * iSqForce;
                p1.Velocity += p2.Mass * gravityForce;
                if (affectSecond)
                    p2.Velocity -= p1.Mass * gravityForce;
            }
        }

        private void processMagneticForce(Particle p1, Particle p2, float iMass1, float iMass2,
            bool affectSecond, Vector2 diff, float sqLen, Vector2 iSqForce)
        {
            var magneticForceRadius = enVars.MagneticForceRadius;
            var magneticForceRadiusSQ = magneticForceRadius * magneticForceRadius;
            if (magneticForceRadius < 0 && magneticForceRadiusSQ <= sqLen ||
                 magneticForceRadius > 0 && magneticForceRadiusSQ >= sqLen)
            {
                var electroMagneticForce = p2.Charge * p1.Charge * enVars.MagneticForcePower * iSqForce;
                p1.Velocity -= iMass1 * electroMagneticForce;
                if (affectSecond)
                    p2.Velocity += iMass2 * electroMagneticForce;
            }
        }

        private void updateParticle(int index, Particle p, float delta)
        {
            bool affectOthers = index != -1;

            if (enVars.IsGravityToCenterEnabled)
            {
                var force = delta * (origin - p.Position).ToSqUnit();
                p.Velocity += enVars.GravityToCenterPower * p.Mass * force;
            }

            if (enVars.ForceFieldEnabled && enVars.ForceFieldState != ForceFieldState.None)
            {
                processForceField(p, delta);
            }

            if (enVars.LinearForce != Vector2.Zero)
            {
                var force = delta * enVars.LinearForce;
                p.Velocity += p.Mass * force;
            }

            if (enVars.IsInteractionEnabled)
            {
                for (int i = index + 1; i < Particles.Count; ++i)
                {
                    var p2 = Particles[i];
                    if (JointedParticles.AreParticlesJointed(p, p2))
                        continue;

                    var diff = Particles[i].Position - p.Position;
                    var sqLen = diff.LengthSquared();
                    if (Particles[i].Mass != 0)
                    {
                        processNaturalForces(p, p2, affectOthers, delta, diff, sqLen, out double dist);

                        if(enVars.IsStabilizationEnabled)
                            stabilization(p, p2, affectOthers, sqLen, dist, diff);
                    }
                }
            }

            foreach (Particle point in Points)
            {
                var diff = point.Position - p.Position;
                var sqLen = diff.LengthSquared();

                processNaturalForces(p, point, false, delta, diff, sqLen, out double dist);

                if (enVars.IsStabilizationEnabled)
                    stabilization(p, point, false, sqLen, dist, diff);
            }

            if (enVars.IsRandomTrajectoryEnabled)
            {
                p.Velocity = p.Velocity.Rotate(
                    enVars.TrajectoryRandomizationFactor *
                        (float)(rand.NextDouble() - 0.5));
            }

            if (index != -1)
            {
                if (enVars.IsDecayEnabled)
                {
                    if(p.Age(delta))
                        decayParticle(p);
                }
            }
        }

        private void processForceField(Particle p, float delta)
        {
            var diff = enVars.ForceFieldCenter - p.Position;
            var sqLen = diff.LengthSquared();
            var r = enVars.ForceFieldRadius;
            if (sqLen <= r * r)
            {
                switch (enVars.ForceFieldState)
                {
                    case ForceFieldState.Radial:
                        p.Velocity += delta * enVars.ForceFieldPower
                            * diff.Rotate(enVars.ForceFieldAngle) / (p.Mass * sqLen);
                        break;
                    case ForceFieldState.Circular:
                        var len = (float)Math.Sqrt(sqLen);
                        var unit = diff / len;
                        var tangent = unit.Rotate(-enVars.ForceFieldAngle);
                        p.Velocity += delta * enVars.ForceFieldPower * tangent / p.Mass;
                        p.Velocity += delta * p.Velocity.LengthSquared() / len * unit;
                        break;
                }
            }
        }

        private void postUpdateParticle(Particle p)
        {
            if (enVars.BoundState != BoundState.None)
                boundParticle(p);

            if (enVars.DampingFactor != 1)
            {
                if (enVars.DampingFactor == 0)
                    p.Velocity = Vector2.Zero;
                else
                    p.Velocity *= enVars.DampingFactor;
            }
        }

        #endregion

        #region Particle methods

        public List<Vector2> PredictTrajectory(Particle particle, float predictionTime, float timeDelta)
        {
            var result = new List<Vector2>();

            while (predictionTime > 0)
            {
                result.Add(particle.Position);
                updateParticle(-1, particle, timeDelta);
                postUpdateParticle(particle);
                particle.Update(timeDelta);
                predictionTime -= timeDelta;
            }

            return result;
        }

        public ParticleSearchResult? GetParticleInfoAtPoint(Vector2 worldPosition)
        {
            // looking for a particle with a brutal force
            for (int i = 0; i < Particles.Count; i++)
            {
                var particle = Particles[i];
                if (particle.OverlapPoint(worldPosition))
                    return new ParticleSearchResult(particle, Particles, i);
            }

            for (int i = 0; i < Points.Count; i++)
            {
                var point = Points[i];
                if (point.OverlapPoint(worldPosition))
                    return new ParticleSearchResult(point, Points, i);
            }

            return null;
        }

        private void decayParticle(Particle p)
        {
            p.IsFrozen = true;
            int n = enVars.DecayNumber;
            if (p.Radius > 1)
            {
                float angleStep = enVars.DecayAngle;
                float radius = (float)(p.Radius / Math.Sqrt(n));
                float mass = p.Mass / n;
                for (int i = 0; i < n; ++i)
                {
                    var v = p.Velocity.Rotate(angleStep * (i - 1));
                    var position = p.Position + p.Radius * v.ToUnit();
                    Particle child = new Particle(
                        position.X,
                        position.Y,
                        v.X,
                        v.Y,
                        p.Color,
                        radius,
                        mass,
                        p.InitialLifeTime,
                        p.Charge,
                        p.Tail.Length);
                    Particles.Add(child);
                }
            }
            Particles.Remove(p);
        }

        private void boundParticle(Particle p)
        {
            switch (enVars.BoundState)
            {
                case BoundState.RectangularBound:
                    {
                        var r = new RectangleF(origin.X - enVars.BoundSize,
                            origin.Y - enVars.BoundSize,
                            2 * enVars.BoundSize,
                            2 * enVars.BoundSize);

                        if (p.Position.X - p.Radius < r.Left)
                        {
                            p.Position.X = r.Left + p.Radius;
                            p.Velocity.X = -enVars.CollisionDampingFactor * p.Velocity.X;
                        }
                        if (p.Position.X + p.Radius > r.Right)
                        {
                            p.Position.X = r.Right - p.Radius;
                            p.Velocity.X = -enVars.CollisionDampingFactor * p.Velocity.X;
                        }
                        if (p.Position.Y - p.Radius < r.Top)
                        {
                            p.Position.Y = r.Top + p.Radius;
                            p.Velocity.Y = -enVars.CollisionDampingFactor * p.Velocity.Y;
                        }
                        if (p.Position.Y + p.Radius > r.Bottom)
                        {
                            p.Position.Y = r.Bottom - p.Radius;
                            p.Velocity.Y = -enVars.CollisionDampingFactor * p.Velocity.Y;
                        }
                        break;
                    }
                case BoundState.CircularBound:
                    {
                        var r = enVars.BoundSize - p.Radius;
                        var sqr = r * r;
                        var diff = origin - p.Position;
                        var sqDist = diff.LengthSquared();
                        if (r > 0 && sqDist > sqr)
                        {
                            p.Position = origin - r * diff.ToUnit();
                            p.Velocity = p.Velocity.Reflection(origin, p.Position,
                                enVars.CollisionDampingFactor);
                        }
                        break;
                    }
            }
        }

        #endregion
    }
}
