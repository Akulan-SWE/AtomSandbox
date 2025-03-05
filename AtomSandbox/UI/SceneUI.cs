using System.Drawing.Drawing2D;
using System.Numerics;

using AtomSandbox.Simulation;
using AtomSandbox.Tools;
using WindowsFormsTools.Core;

namespace AtomSandbox.UI
{
    /// <summary>
    /// The bridge between main form and the simulation scene
    /// </summary>
    internal class SceneUI
    {
        public event EventHandler<ParticleEventArgs>? ParticleSelected;

        public event EventHandler? ParticleUnselected;

        private const float SelectionVisualRadius = 10;

        public Particle? SelectedParticle
        {
            get => selectedParticle;
            private set
            {
                if (selectedParticle == value)
                    return;

                selectedParticle = value;
                if (selectedParticle == null)
                    ParticleUnselected?.Invoke(this, new EventArgs());
                else
                    ParticleSelected?.Invoke(this, new ParticleEventArgs(selectedParticle));
            }
        }

        public Vector2 StartPushPoint
        {
            get => startPushPoint.Position;
            set => startPushPoint.Position = value;
        }

        public Vector2 EndPushPoint
        {
            get => endPushPoint.Position;
            set => endPushPoint.Position = value;
        }

        private RectangleF copyingArea;
        private CopyInfo copyInfo;

        private Particle? pulledParticle;
        private Vector2 pulledParticleOffset;
        private Particle? selectedParticle;
        private HashSet<Guid> selectedParticles;

        private readonly Pen arrowPen;
        private readonly Pen predictionPen;
        private readonly Pen copyPen;
        private readonly Pen pastePen;
        private readonly Pen erasePen;
        private readonly Pen cutPen;
        private readonly Pen selectPen;
        private readonly Brush forceFieldBrush;
        private readonly Particle oracleBall, startPushPoint, endPushPoint;
        private float selectedParticleVisualRadius;

        private ZoomPen[] zoomPens;

        private Scene scene;
        private IControlVariables controlVars;
        private IEnvironmentVariables enVars;
        private IParticleInitVariables particleVars;

        private float timeDelta;

        public SceneUI(Scene scene, IControlVariables controlVariables, IParticleInitVariables particleInitVariables,
            IEnvironmentVariables environmentVariables, Color forceFieldColor)
        {
            this.scene = scene;
            controlVars = controlVariables;
            particleVars = particleInitVariables;
            enVars = environmentVariables;

            startPushPoint = new Particle(-10, 0, 0, 0, Color.LawnGreen, 0.1f, 1, 1, 0, 0);
            endPushPoint = new Particle(10, 0, 0, 0, Color.Red, 0.1f, 1, 1, 0, 0);


            copyInfo = new CopyInfo();
            selectedParticles = [];

            arrowPen = new Pen(Color.FromArgb(100, 255, 255, 255), 1)
            {
                EndCap = LineCap.ArrowAnchor,
                StartCap = LineCap.RoundAnchor
            };

            predictionPen = new Pen(Color.FromArgb(100, 255, 0, 0), 1)
            {
                EndCap = LineCap.ArrowAnchor,
                StartCap = LineCap.RoundAnchor
            };

            copyPen = new Pen(Color.White, 0.1f);
            pastePen = new Pen(Color.LawnGreen, 0.1f);
            cutPen = new Pen(Color.Yellow, 0.1f);
            erasePen = new Pen(Color.Red, 0.1f);
            selectPen = new Pen(Color.Orange, 5f);
            forceFieldBrush = new SolidBrush(Color.FromArgb(50, forceFieldColor));

            zoomPens = [
                new ZoomPen(arrowPen),
                new ZoomPen(predictionPen),
                new ZoomPen(copyPen),
                new ZoomPen(pastePen),
                new ZoomPen(cutPen),
                new ZoomPen(erasePen),
                new ZoomPen(selectPen)
            ];

            oracleBall = new Particle(-100, -100, 0, 0, Color.FromArgb(100, Color.Red), 1.5f, 1.5f, -100, 0, 0);
        }

        #region Prediction

        private void renderPredictedOrbit(Graphics g, Vector2 position, Vector2 velocity, float timeDelta)
        {
            if (controlVars.PredictionTime == 0 || timeDelta == 0)
                return;

            oracleBall.Charge = particleVars.Charge;
            oracleBall.Radius = particleVars.Radius;
            oracleBall.Mass = particleVars.Mass;
            oracleBall.Position = position;
            oracleBall.Velocity = velocity;

            float predTime = Math.Abs(controlVars.PredictionTime);
            List<Vector2> track = scene.PredictTrajectory(oracleBall, predTime, timeDelta);
            PointF[] trackPoints = [.. track.Select(p => (PointF)p)];

            g.DrawLines(predictionPen, trackPoints);
        }

        private void resetPredictionBall()
        {
            oracleBall.Position = new Vector2(-100, -100);
            oracleBall.Velocity = new Vector2(0);
        }

        #endregion

        #region Area methods

        private static RectangleF getRectangle(Vector2 start, Vector2 end)
        {
            return new RectangleF
            {
                X = start.X <= end.X ? start.X : end.X,
                Y = start.Y <= end.Y ? start.Y : end.Y,
                Width = Math.Abs(start.X - end.X),
                Height = Math.Abs(start.Y - end.Y)
            };
        }

        private void xcopyArea(Vector2 start, Vector2 end)
        {
            copyArea(start, end);
            eraseArea(start, end);
        }

        private void copyArea(Vector2 start, Vector2 end)
        {
            copyInfo.Clear();

            copyingArea = getRectangle(start, end);
            var areaCenter = 0.5f * (start + end);

            foreach (var p in scene.Particles)
            {
                if (copyingArea.Contains((PointF)p.Position))
                {
                    var copy = new Particle(p) { ParticleID = p.ParticleID };
                    copy.Position -= areaCenter;
                    copyInfo.Particles.Add(copy.ParticleID, copy);
                }
            }

            foreach (var p in scene.Points)
            {
                if (copyingArea.Contains((PointF)p.Position))
                {
                    var copy = new Particle(p)
                    {
                        ParticleID = p.ParticleID,
                        TailLength = 0
                    };
                    copy.Position -= areaCenter;
                    copyInfo.Points.Add(copy.ParticleID, copy);
                }
            }

            foreach (var p in copyInfo.Particles.Values)
            {
                if (scene.JointedParticles.ContainsParticle(p))
                {
                    var linkedParticles = scene.JointedParticles.GetJointedParticles(p);
                    foreach (var particleID in linkedParticles)
                    {
                        if (copyInfo.Particles.TryGetValue(particleID, out Particle? particle))
                            copyInfo.Joints.AddJointInfo(p, particle);

                        if (copyInfo.Points.TryGetValue(particleID, out Particle? point))
                            copyInfo.Joints.AddJointInfo(p, point);
                    }
                }
            }
        }

        private void eraseArea(Vector2 start, Vector2 end)
        {
            var erasingArea = getRectangle(start, end);

            scene.Particles.RemoveIf(
               (p, i) => erasingArea.Contains((PointF)p.Position),
               beforeParticleDelete);

            scene.Points.RemoveIf(
               (p, i) => erasingArea.Contains((PointF)p.Position),
               beforeParticleDelete);
        }

        private void selectArea(Vector2 start, Vector2 end)
        {
            var selectionArea = getRectangle(start, end);
            foreach (var p in scene.Particles)
            {
                if (selectionArea.Contains((PointF)p.Position))
                    selectedParticles.Add(p.ParticleID);
            }
            foreach (var p in scene.Points)
            {
                if (selectionArea.Contains((PointF)p.Position))
                    selectedParticles.Add(p.ParticleID);
            }
        }

        private void pasteArea(Vector2 position, Vector2 velocity)
        {
            var copyMap = new Dictionary<Guid, Particle>();

            foreach (var p in copyInfo.Particles)
            {
                var copy = new Particle(p.Value);
                copy.Position += position;
                copy.Velocity += velocity;
                copy.ResetTail();
                scene.Particles.Add(copy);

                copyMap.Add(p.Key, copy);
            }

            foreach (var p in copyInfo.Points)
            {
                var copy = new Particle(p.Value);
                copy.Position += position;
                copy.Velocity += velocity;
                copy.ResetTail();
                scene.Points.Add(copy);

                copyMap.Add(p.Key, copy);
            }

            foreach (var copyPair in copyMap)
            {
                var originalParticleID = copyPair.Key;
                if (copyInfo.Joints.ContainsParticle(originalParticleID))
                {
                    var linkedParticles = copyInfo.Joints.GetJointedParticles(originalParticleID);
                    foreach (var particleID in linkedParticles)
                    {
                        if (copyMap.TryGetValue(particleID, out Particle? copy1))
                        {
                            var copy2 = copyPair.Value;
                            if (!scene.JointedParticles.AreParticlesJointed(copy1, copy2))
                            {
                                scene.JointedParticles.AddJointInfo(copy1, copy2);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Particle methods

        private void beforeParticleDelete(Particle p, int index)
        {
            if (SelectedParticle?.ParticleID == p.ParticleID)
            {
                //particlePropertyControl.TargetObject = null;
                SelectedParticle = null;
            }

            if (selectedParticles.Contains(p.ParticleID))
                selectedParticles.Remove(p.ParticleID);

            if (scene.JointedParticles.ContainsParticle(p))
                scene.JointedParticles.DeleteParticleJoints(p);
        }

        private Vector2 getPushVelocity()
        {
            if (controlVars.CircularVelocityState != CircularVelocityState.None)
            {
                float mass = 0;
                var v = Vector2.Zero;

                if (controlVars.CircularVelocityState == CircularVelocityState.CommonMass)
                {
                    var middlePoint = Vector2.Zero;
                    if (scene.Particles.Count != 0)
                    {
                        foreach (var p in scene.Particles)
                        {
                            middlePoint += p.Mass * p.Position;
                            mass += p.Mass;
                        }
                    }
                    if (scene.Points.Count != 0)
                    {
                        foreach (var p in scene.Points)
                        {
                            middlePoint += p.Mass * p.Position;
                            mass += p.Mass;
                        }
                    }
                    if (mass != 0)
                    {
                        middlePoint /= mass;
                        v = (middlePoint - controlVars.WorldMousePosition).Rotate(90);
                    }
                }
                else
                {
                    var firstParticle = scene.Particles.Count == 0
                       ? scene.Points.FirstOrDefault()
                       : scene.Particles.First();

                    if (firstParticle != null)
                    {
                        v = (firstParticle.Position - controlVars.WorldMousePosition).Rotate(90);
                        mass = firstParticle.Mass;
                    }
                }

                if (mass != 0)
                {
                    v = Vector2.Normalize(v) * (float)Math.Sqrt(enVars.GravityForcePower * mass);
                    return controlVars.IsPushDirectionReversed ? -v : v;
                }
            }

            if (controlVars.CalcReverseImpulse)
            {

                var impulse = Vector2.Zero;
                scene.Particles.ForEach(p => impulse += p.Mass * p.Velocity);
                if (controlVars.ControlCommand == ControlCommands.Paste)
                {
                    var existingImpulse = Vector2.Zero;
                    float existingMass = 0;
                    foreach (var p in copyInfo.Particles.Values)
                    {
                        existingImpulse += p.Mass * p.Velocity;
                        existingMass += p.Mass;
                    }
                    return (-impulse - existingImpulse) / existingMass;
                }
                return -impulse / particleVars.Mass;
            }

            var k = controlVars.IsPushDirectionReversed
                ? -particleVars.SpeedFactor
                : particleVars.SpeedFactor;

            return k * (controlVars.WorldMouseDown - controlVars.WorldMousePosition);
        }

        private Particle createParticle(Vector2 position, Vector2 velocity)
        {
            return new Particle(
                position,
                velocity,
                particleVars.Color,
                particleVars.Radius,
                particleVars.Mass,
                particleVars.LifeTime,
                particleVars.Charge,
                particleVars.TailLength);
        }

        public Particle CreateParticle(bool isPoint)
        {
            if(isPoint)
            {
                var point = createParticle(controlVars.WorldMousePosition, Vector2.Zero);
                scene.Points.Add(point);
                return point;
            }

            Vector2 initPos, vel;
            if (controlVars.IsPushTemplateUsed)
            {
                initPos = startPushPoint.Position;
                vel = particleVars.SpeedFactor *
                    (endPushPoint.Position - startPushPoint.Position);
            }
            else
            {
                initPos = controlVars.WorldMousePosition;
                vel = Vector2.Zero;
            }

            var p = createParticle(initPos, vel);
            scene.Particles.Add(p);
            return p;
        }

        public void DeleteLastParticle(bool deletePoint)
        {
            if(deletePoint)
            {
                if (scene.Points.Count > 0)
                {
                    int lastIndex = scene.Points.Count - 1;
                    beforeParticleDelete(scene.Points[lastIndex], lastIndex);
                    scene.Points.RemoveAt(lastIndex);
                }
            }
            else
            {
                if (scene.Particles.Count > 0)
                {
                    int lastIndex = scene.Particles.Count - 1;
                    beforeParticleDelete(scene.Particles[lastIndex], lastIndex);
                    scene.Particles.RemoveAt(lastIndex);
                }
            }
        }

        public void DeleteUnderMouse()
        {
            var particleInfo = scene.GetParticleInfoAtPoint(controlVars.WorldMousePosition);
            if (particleInfo != null)
            {
                beforeParticleDelete(particleInfo.Particle, particleInfo.Index);
                particleInfo.Container.RemoveAt(particleInfo.Index);
            }
        }

        private Particle? findOverlappingParticle(Vector2 point)
        {
            var particleInfo = scene.GetParticleInfoAtPoint(point);

            if (particleInfo != null)
                return particleInfo.Particle;

            if (startPushPoint.OverlapPoint(controlVars.WorldMouseDown))
                return startPushPoint;

            else if (endPushPoint.OverlapPoint(controlVars.WorldMouseDown))
                return endPushPoint;

            return null;
        }

        public void ConvertSelectedParticlesToPoints()
        {
            if (selectedParticles.Count != 0)
            {
                var newPoints = scene.Particles.KeepRemoveIf(p => selectedParticles.Contains(p.ParticleID));
                foreach (var point in newPoints)
                {
                    point.TailLength = 0;
                    scene.Points.Add(point);
                }
                selectedParticles.Clear();
            }
        }

        public void JointSelectedParticles()
        {
            if (selectedParticles.Count != 0)
            {
                var selectedParticlesList =
                    selectedParticles.Select(
                        id => scene.Particles.FirstOrDefault(p => p.ParticleID == id) ??
                              scene.Points.First(p => p.ParticleID == id)).ToList();

                for (int i = 0; i < selectedParticlesList.Count; ++i)
                {
                    for (int j = i + 1; j < selectedParticlesList.Count; ++j)
                    {
                        var p1 = selectedParticlesList[i];
                        var p2 = selectedParticlesList[j];
                        scene.JointedParticles.AddJointInfo(p1, p2);
                    }
                }
                selectedParticles.Clear();
            }
        }

        public void Clear()
        {
            selectedParticles.Clear();
        }

        #endregion

        public void Render(Graphics g)
        {
            if(selectedParticles.Count != 0)
            {
                foreach(var p in scene.Particles)
                {
                    if (!selectedParticles.Contains(p.ParticleID))
                        continue;

                    g.DrawEllipse(selectPen, p.Position.X - p.Radius,
                       p.Position.Y - p.Radius, 2 * p.Radius, 2 * p.Radius);
                }
            }

            foreach (var point in scene.Points)
            {
                if (selectedParticles.Count != 0 && selectedParticles.Contains(point.ParticleID))
                    g.DrawEllipse(selectPen, point.Position.X - point.Radius,
                       point.Position.Y - point.Radius, 2 * point.Radius, 2 * point.Radius);
            }

            if (SelectedParticle != null)
            {
                float r = selectedParticleVisualRadius;
                if (r < SelectedParticle.Radius)
                    r = SelectedParticle.Radius;

                g.DrawEllipse(selectPen, SelectedParticle.Position.X - r,
                   SelectedParticle.Position.Y - r, 2 * r, 2 * r);
            }


            if (controlVars.UsePipette)
            {
                var particleInfo = scene.GetParticleInfoAtPoint(controlVars.WorldMousePosition);
                if (particleInfo != null)
                {
                    var particle = particleInfo.Particle;
                    g.DrawEllipse(selectPen, particle.Position.X - particle.Radius,
                       particle.Position.Y - particle.Radius, 2 * particle.Radius, 2 * particle.Radius);
                }
            }

            if (controlVars.ControlCommand == ControlCommands.Paste)
            {
                g.DrawRectangle(pastePen, controlVars.WorldMousePosition.X - 0.5f * copyingArea.Width,
                   controlVars.WorldMousePosition.Y - 0.5f * copyingArea.Height, copyingArea.Width, copyingArea.Height);
                foreach (var particleCopy in copyInfo.Particles.Values)
                    particleCopy.Draw(g, particleCopy.Position + controlVars.WorldMousePosition, 100);
                foreach (var pointCopy in copyInfo.Points.Values)
                    pointCopy.Draw(g, pointCopy.Position + controlVars.WorldMousePosition, 100);
            }

            if (controlVars.IsMouseLeftPressing)
            {
                switch (controlVars.ControlCommand)
                {
                    case ControlCommands.Copy:
                        g.DrawRectangle(copyPen, getRectangle(controlVars.WorldMouseDown, controlVars.WorldMousePosition));
                        break;
                    case ControlCommands.Erase:
                        g.DrawRectangle(erasePen, getRectangle(controlVars.WorldMouseDown, controlVars.WorldMousePosition));
                        break;
                    case ControlCommands.Cut:
                        g.DrawRectangle(cutPen, getRectangle(controlVars.WorldMouseDown, controlVars.WorldMousePosition));
                        break;
                    case ControlCommands.Select:
                        g.DrawRectangle(selectPen, getRectangle(controlVars.WorldMouseDown, controlVars.WorldMousePosition));
                        break;
                }
            }

            if (enVars.ForceFieldState != ForceFieldState.None)
            {
                float r = enVars.ForceFieldRadius;
                float dr = 2 * r;
                g.FillEllipse(forceFieldBrush, controlVars.WorldMousePosition.X - r,
                    controlVars.WorldMousePosition.Y - r, dr, dr);
            }

            bool isVelocityPrecomputed = controlVars.CalcReverseImpulse ||
                    controlVars.CircularVelocityState != CircularVelocityState.None;

            if (enVars.ForceFieldState == ForceFieldState.None &&
                (controlVars.ControlCommand == ControlCommands.None ||
                    controlVars.ControlCommand == ControlCommands.Paste) &&
                (controlVars.IsMouseLeftPressing || isVelocityPrecomputed))
            {
                var velocity = getPushVelocity();
                var start = (PointF)controlVars.WorldMousePosition;
                var end = (PointF)(isVelocityPrecomputed
                    ? controlVars.WorldMousePosition + velocity
                    : controlVars.WorldMouseDown);
                if (controlVars.IsPushDirectionReversed)
                {
                    start = end;
                    end = (PointF)controlVars.WorldMouseDown;
                }
                g.DrawLine(arrowPen, start, end);

                if (controlVars.ControlCommand != ControlCommands.Paste && controlVars.PredictParticleTrajectory)
                {
                    var position = controlVars.IsPushDirectionReversed
                        ? controlVars.WorldMouseDown
                        : controlVars.WorldMousePosition;
                    renderPredictedOrbit(g, position, velocity, timeDelta);
                }
            }

            if (controlVars.IsPushTemplateUsed)
            {
                startPushPoint.Radius = controlVars.TemplatePointRadius / controlVars.Zoom;
                endPushPoint.Radius = controlVars.TemplatePointRadius / controlVars.Zoom;
                startPushPoint.Render(g);
                endPushPoint.Render(g);

                if (startPushPoint.Position != endPushPoint.Position)
                {
                    var velocity = particleVars.SpeedFactor * (endPushPoint.Position - startPushPoint.Position);
                    renderPredictedOrbit(g, startPushPoint.Position, velocity, timeDelta);
                }
            }
        }

        public void Update(float timeDelta)
        {
            this.timeDelta = timeDelta;

            if (controlVars.IsMouseLeftPressing &&
                controlVars.ControlCommand == ControlCommands.Pull &&
                pulledParticle != null)
            {
                pulledParticle.Position = controlVars.WorldMousePosition + pulledParticleOffset;
            }
        }

        public void OnMouseDown(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (controlVars.ControlCommand != ControlCommands.Pull)
                        break;

                    var particle = findOverlappingParticle(controlVars.WorldMouseDown);
                    if (particle != null)
                    {
                        pulledParticle = particle;
                        pulledParticleOffset = pulledParticle.Position - controlVars.WorldMouseDown;
                    }
                    break;

                case MouseButtons.Middle:
                    SelectedParticle = findOverlappingParticle(controlVars.WorldMouseDown);
                    break;
            }
        }

        public void OnMouseUp(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        if (controlVars.ControlCommand != ControlCommands.None)
                        {
                            switch (controlVars.ControlCommand)
                            {
                                case ControlCommands.Copy:
                                    copyArea(controlVars.WorldMouseDown, controlVars.WorldMousePosition);
                                    break;
                                case ControlCommands.Cut:
                                    xcopyArea(controlVars.WorldMouseDown, controlVars.WorldMousePosition);
                                    break;
                                case ControlCommands.Erase:
                                    eraseArea(controlVars.WorldMouseDown, controlVars.WorldMousePosition);
                                    break;
                                case ControlCommands.Paste:
                                    pasteArea(controlVars.WorldMousePosition, getPushVelocity());
                                    break;
                                case ControlCommands.Select:
                                    selectArea(controlVars.WorldMouseDown, controlVars.WorldMousePosition);
                                    break;
                                case ControlCommands.Pull:
                                    if (pulledParticle != null)
                                    {
                                        pulledParticle.Velocity.Set(0, 0);
                                        pulledParticle = null;
                                    }
                                    break;
                            }
                        }
                        else if (enVars.ForceFieldState == ForceFieldState.None)
                        {
                            var p = createParticle(
                                controlVars.IsPushDirectionReversed
                                    ? controlVars.WorldMouseDown
                                    : controlVars.WorldMousePosition,
                                getPushVelocity());
                            scene.Particles.Add(p);
                            resetPredictionBall();
                        }
                    }
                    break;
            }
        }

        public void OnZoomChanged(object? sender, FloatValueChangedEventArgs e)
        {
            foreach (var pen in zoomPens)
                pen.OnZoomChanged(e.NewValue);

            selectedParticleVisualRadius = SelectionVisualRadius / e.NewValue;
        }
    }
}
