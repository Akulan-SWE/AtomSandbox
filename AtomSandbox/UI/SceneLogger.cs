using System.Globalization;
using System.Numerics;
using AtomSandbox.Simulation;

namespace AtomSandbox.UI
{
    /// <summary>
    /// The collector of simulation information to be displayed in the UI
    /// </summary>
    internal class SceneLogger
    {
        private Scene scene;
        private TextBox[] logTextBoxes;
        private IEnvironmentVariables enVars;
        private IControlVariables controlVars;
        private long lastTime = DateTime.Now.Ticks;
        private const int fpsCountPeriod = 10;
        private int fpsTics;

        public SceneLogger(Scene scene, IEnvironmentVariables enVars,
            IControlVariables controlVars, params TextBox[] logTextBoxes)
        {
            this.scene = scene;
            this.logTextBoxes = logTextBoxes;
            this.enVars = enVars;
            this.controlVars = controlVars;
        }

        private float getDeltaTime()
        {
            long now = DateTime.Now.Ticks;
            long dT = now - lastTime;
            lastTime = now;
            return enVars.TimeFactor * (float)TimeSpan.FromTicks(dT).TotalSeconds;
        }

        public void LogInfo()
        {
            int totalCount = 0;
            float totalCharge = 0;
            Vector2 totalImpulse = Vector2.Zero;

            foreach (var point in scene.Points)
            {
                ++totalCount;
                totalCharge += point.Charge;
            }

            foreach (var particle in scene.Particles)
            {
                ++totalCount;
                totalCharge += particle.Charge;
                totalImpulse += particle.Mass * particle.Velocity;
            }

            logTextBoxes[0].Text = totalCount.ToString(CultureInfo.InvariantCulture);
            logTextBoxes[1].Text = totalCharge.ToString(CultureInfo.InvariantCulture);

            var lastParticle = scene.Particles.Count == 0
               ? scene.Points.LastOrDefault()
               : scene.Particles.Last();

            var d = lastParticle == null ? 0 : (controlVars.WorldMousePosition - lastParticle.Position).Length();

            logTextBoxes[2].Text = d.ToString();

            float dt = 1.0f / getDeltaTime();
            if (--fpsTics <= 0)
            {
                logTextBoxes[3].Text = dt.ToString("F2");
                fpsTics = fpsCountPeriod;
            }

            logTextBoxes[4].Text = totalImpulse.ToString();
        }
    }
}
