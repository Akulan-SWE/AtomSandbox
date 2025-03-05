namespace AtomSandbox.Simulation
{
    internal class ParticleSearchResult
    {
        /// <summary>
        /// The particle that was found
        /// </summary>
        public Particle Particle { get; set; }

        /// <summary>
        /// The container where the particle belongs to
        /// </summary>
        public List<Particle> Container { get; set; }

        /// <summary>
        /// The index of the particle in the container
        /// </summary>
        public int Index { get; set; }

        public ParticleSearchResult(Particle particle, List<Particle> container, int index)
        {
            Particle = particle;
            Container = container;
            Index = index;
        }
    }
}
