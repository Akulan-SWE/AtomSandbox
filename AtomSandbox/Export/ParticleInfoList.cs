namespace AtomSandbox.Export
{
    [Serializable]
    public class ParticleInfoList
    {
        #region Default particles

        public const float ElectronMass = 3;
        public const float ElectronRadius = 3;

        public static readonly ParticleInfoList DefauldParticlesList = new ParticleInfoList
        {
            ParticlesInfo = [
                new ParticleInfo
                {
                    Name = "Electron",
                    Color = Color.Aqua,
                    Radius = ElectronRadius,
                    Mass = ElectronMass,
                    TailLength = 100,
                    LifeTime = 10,
                    Charge = -1
                },
                new ParticleInfo
                {
                    Name = "Proton",
                    Color = Color.FromArgb(255, 255, 128, 255),
                    Radius = 7 * ElectronRadius,
                    Mass = 1836 * ElectronMass,
                    TailLength = 100,
                    LifeTime = 10,
                    Charge = 1
                },
                new ParticleInfo
                {
                    Name = "Antiproton",
                    Color = Color.FromArgb(255, 128, 255, 255),
                    Radius = 7 * ElectronRadius,
                    Mass = 1836 * ElectronMass,
                    TailLength = 100,
                    LifeTime = 10,
                    Charge = -1
                },
                new ParticleInfo
                {
                    Name = "Positron",
                    Color = Color.FromArgb(255, 255, 119, 255),
                    Radius = ElectronRadius,
                    Mass = ElectronMass,
                    TailLength = 100,
                    LifeTime = 10,
                    Charge = 1
                },
                new ParticleInfo
                {
                    Name = "Neutron",
                    Color = Color.Silver,
                    Radius = 7 * ElectronRadius,
                    Mass = 1838 * ElectronMass,
                    TailLength = 100,
                    LifeTime = 10,
                    Charge = 0
                },
                new ParticleInfo
                {
                    Name = "Neutrino",
                    Color = Color.LightGray,
                    Radius = 0.5f * ElectronRadius,
                    Mass = 0.1f,
                    TailLength = 100,
                    LifeTime = 10,
                    Charge = 0
                }
            ]
        };

        #endregion

        public ParticleInfo[]? ParticlesInfo { get; set; }
    }
}
