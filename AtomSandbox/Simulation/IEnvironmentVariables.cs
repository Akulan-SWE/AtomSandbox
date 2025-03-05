using System.Numerics;

namespace AtomSandbox.Simulation
{
    /// <summary>
    /// The set of variables dictating the behavior of the simulation
    /// </summary>
    interface IEnvironmentVariables
    {
        #region Inter-particle interactions

        /// <summary>
        /// If false, all interactions between particles (reflection, stabilization, natural forces) are disabled
        /// </summary>
        bool IsInteractionEnabled { get; }

        /// <summary>
        /// If true, the particles bounce against the each other
        /// </summary>
        bool IsReflectionEnabled { get; }

        /// <summary>
        /// If true, the particles do not enter the boundaries of the other particles
        /// </summary>
        bool IsStabilizationEnabled { get; }

        /// <summary>
        /// If true, the displacement neccessary to move two particles outside their boundaries is applied to each particle
        /// proportionally to the mass of the particle. Helpful when particles vary in mass significantly and the smaller
        /// particle hit the bigger one frequently, thus causing the visible push too powerful for the smaller particle.
        /// </summary>
        bool UseMassProportionalStabilization { get; }

        /// <summary>
        /// If true, adds an invisible skin, extra distance, between particles.
        /// Affected by <see cref="ExtraStabilizationDistance"/> and <see cref="ExtraStabilizationStartRadius"/>.
        /// </summary>
        bool UseExtraStabilization { get; }

        /// <summary>
        /// The distance of the extra stabilization.
        /// See <see cref="UseExtraStabilization"/>.
        /// </summary>
        float ExtraStabilizationDistance { get; }

        /// <summary>
        /// The minimum radius of the particle affected by the extra stabilization.
        /// Filters out small particles. See <see cref="UseExtraStabilization"/>."/>
        /// </summary>
        float ExtraStabilizationStartRadius { get; }

        #endregion

        #region Random trajectory

        /// <summary>
        /// If true, slightly rotates the velocity of the particle in random directions
        /// </summary>
        bool IsRandomTrajectoryEnabled { get; }

        /// <summary>
        /// The maximum random angle the trajectory is rotated.
        /// See <see cref="IsRandomTrajectoryEnabled"/>.
        /// </summary>
        float TrajectoryRandomizationFactor { get; }

        #endregion

        #region Coefficients

        /// <summary>
        /// The coefficient of restitution, a measure of the elasticity of a collision between two particles
        /// </summary>
        float BouncityFactor { get; }

        /// <summary>
        /// The movement friction coefficient, saps the particle's velocity over time
        /// </summary>
        float DampingFactor { get; }

        /// <summary>
        /// The multiplier of the time delta, affects the speed of the simulation.
        /// Negative values DO NOT reverse the simulation, unfortunately.
        /// </summary>
        float TimeFactor { get; }

        /// <summary>
        /// The factor by which the velocity of the particle is multiplied upon the collision with walls or other particles
        /// </summary>
        float CollisionDampingFactor { get; }

        /// <summary>
        /// The elasticity of the joint between jointed particles
        /// </summary>
        float JointElasticityFactor { get; }

        #endregion

        #region Bounding

        /// <summary>
        /// The shape of the area in which the particles are bounded
        /// </summary>
        BoundState BoundState { get; }

        /// <summary>
        /// The size of the bounding area: radius for circular bound, half-size for rectangular bound
        /// </summary>
        float BoundSize { get; }

        #endregion

        #region Force Field

        /// <summary>
        /// States what type of force field is applied on particles
        /// </summary>
        ForceFieldState ForceFieldState { get; }

        /// <summary>
        /// If true, the force field is enabled
        /// </summary>
        bool ForceFieldEnabled { get; }

        /// <summary>
        /// The center of the force field
        /// </summary>
        Vector2 ForceFieldCenter { get; }

        /// <summary>
        /// The radius of the force field
        /// </summary>
        float ForceFieldRadius { get; }

        /// <summary>
        /// The intencity of the force field.
        /// When negative, invert the effect on particles: attract instead of repel,
        /// clockwise rotation instead of counterclockwise.
        /// </summary>
        float ForceFieldPower { get; }

        /// <summary>
        /// When force field is enabled, adds the extra angle to force field effect.
        /// Vital for <see cref="ForceFieldState.Circular"/> to set the direction of the rotation.
        /// </summary>
        float ForceFieldAngle { get; }

        #endregion

        #region Natural forces

        /// <summary>
        /// The range within the natural forces take effect.
        /// If positive, the particles interract only INSIDE this radius.
        /// If negative, the forces work only OUTSIDE this radius.
        /// </summary>
        float ForceRadius { get; }

        /// <summary>
        /// The linear force applied to all particles
        /// </summary>
        Vector2 LinearForce { get; }

        #region Strong force

        /// <summary>
        /// If true, the strong force is enabled
        /// </summary>
        bool IsStrongForceEnabled { get; }

        /// <summary>
        /// The intencity of the strong force
        /// </summary>
        float StrongForcePower { get; }

        /// <summary>
        /// The range within the strong force takes effect.
        /// Similar to <see cref="ForceRadius"/>.
        /// </summary>
        float StrongForceRadius { get; }

        /// <summary>
        /// A component of the strong force. The bigger the value, the longer the reach of the strong force.
        /// </summary>
        float StrongForceEffectiveRadius { get; }

        /// <summary>
        /// The minimum radius of the particle affected by the strong force.
        /// </summary>
        float StrongForceMinParticleRadius { get; }

        /// <summary>
        /// If true, the strong force calculation uses the surface distance instead of the center distance.
        /// </summary>
        bool UseStrongForceSurfaceDistance { get; }

        #endregion

        #region Quad force

        /// <summary>
        /// If true, the quad force is enabled
        /// </summary>
        bool IsQuadForceEnabled { get; }

        /// <summary>
        /// The intencity of the quad force
        /// </summary>
        float QuadForcePower { get; }

        /// <summary>
        /// The range within the quad force takes effect.
        /// Similar to <see cref="ForceRadius"/>.
        /// </summary>
        float QuadForceRadius { get; }

        /// <summary>
        /// The minimum radius of the particle affected by the quad force.
        /// </summary>
        float QuadForceMinParticleRadius { get; }

        /// <summary>
        /// An obsolete component of the quad force equation
        /// </summary>
        float QuadForceEffectiveRadius { get; }

        #endregion

        #region Gravity

        /// <summary>
        /// If true, the gravity force is enabled
        /// </summary>
        bool IsGravityForceEnabled { get; }

        /// <summary>
        /// The intencity of the gravity force
        /// </summary>
        float GravityForcePower { get; }

        /// <summary>
        /// The range within the gravity force takes effect.
        /// Similar to <see cref="ForceRadius"/>.
        /// </summary>
        float GravityForceRadius { get; }

        /// <summary>
        /// If true, the particles are attracted to the origin point (0, 0)
        /// </summary>
        bool IsGravityToCenterEnabled { get; }

        /// <summary>
        /// The intencity of the gravity force to the origin point
        /// </summary>
        float GravityToCenterPower { get; }

        #endregion

        #region Electromagnetism

        /// <summary>
        /// If true, the electromagnetic force is enabled
        /// </summary>
        bool IsMagneticForceEnabled { get; }

        /// <summary>
        /// The intencity of the electromagnetic force
        /// </summary>
        float MagneticForcePower { get; }

        /// <summary>
        /// The range within the electromagnetic force takes effect.
        /// Similar to <see cref="ForceRadius"/>.
        /// </summary>
        float MagneticForceRadius { get; }

        #endregion

        #endregion

        #region Decay

        /// <summary>
        /// If true, the particles decay over time
        /// </summary>
        bool IsDecayEnabled { get; }

        /// <summary>
        /// The number of particles left after the parent particle decay
        /// </summary>
        int DecayNumber { get; }

        /// <summary>
        /// The angle of the decayed particle expulsion
        /// </summary>
        float DecayAngle { get; }

        #endregion
    }
}
