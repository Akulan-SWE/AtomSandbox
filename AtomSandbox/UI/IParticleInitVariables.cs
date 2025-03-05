namespace AtomSandbox.UI
{
    /// <summary>
    /// Information used during the creation of the particle
    /// </summary>
    internal interface IParticleInitVariables
    {
        public float Mass { get; }
        public float Radius { get; }
        public float LifeTime { get; }
        public float Charge { get; }
        public float SpeedFactor { get; }
        public Color Color { get; }
        public int TailLength { get; }
    }
}
