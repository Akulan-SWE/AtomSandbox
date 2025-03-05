namespace AtomSandbox.Simulation
{
    /// <summary>
    /// Defines the shape of the area in which the particles are bounded.
    /// </summary>
    public enum BoundState
    {
        /// <summary>
        /// No bound is applied, particles move freely
        /// </summary>
        None,
        /// <summary>
        /// Particles are constrained by a rectangular area
        /// </summary>
        RectangularBound,
        /// <summary>
        /// Particles are constrained by a circular area
        /// </summary>
        CircularBound
    }

    /// <summary>
    /// Defines the effect of the force field
    /// </summary>
    public enum ForceFieldState
    {
        /// <summary>
        /// No force field is applied
        /// </summary>
        None,
        /// <summary>
        /// Force field make particles within its radius move away or towards its center
        /// </summary>
        Radial,
        /// <summary>
        /// Force field make particles within its radius move around its center, clockwise or counterclockwise
        /// </summary>
        Circular
    }
}
