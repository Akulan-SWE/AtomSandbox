namespace AtomSandbox.UI
{
    /// <summary>
    /// Defines how the pre-computation of new particle velocity is done
    /// </summary>
    public enum CircularVelocityState
    {
        /// <summary>
        /// No computation is done
        /// </summary>
        None,
        /// <summary>
        /// The velocity is computed so the released particle would have
        /// a circular trajectory around the center of mass in the system
        /// </summary>
        CommonMass
    }

    /// <summary>
    /// Defines the user command over simulation
    /// </summary>
    internal enum ControlCommands
    {
        /// <summary>
        /// No command is active
        /// </summary>
        None,
        /// <summary>
        /// The user is copying a rectangular area with particles
        /// </summary>
        Copy,
        /// <summary>
        /// The user is pasting a copied rectangular area with particles
        /// </summary>
        Paste,
        /// <summary>
        /// The user is cutting a rectangular area with particles to paste somewhere else
        /// </summary>
        Cut,
        /// <summary>
        /// The user is erasing particles in rectangular area
        /// </summary>
        Erase,
        /// <summary>
        /// The user is selecting particles in rectangular area for jointing
        /// </summary>
        Select,
        /// <summary>
        /// The user is copying a particle info to initial particle variables
        /// </summary>
        Dropping,
        /// <summary>
        /// The user is moving a particle with a cursor
        /// </summary>
        Pull
    }
}
