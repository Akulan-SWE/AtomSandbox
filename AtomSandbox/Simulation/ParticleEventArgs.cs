namespace AtomSandbox.Simulation
{
    internal class ParticleEventArgs(Particle particle) : EventArgs
    {
        public Particle Particle { get; private set; } = particle;
    }
}
