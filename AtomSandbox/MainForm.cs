using System.Drawing.Drawing2D;
using System.Numerics;
using System.Xml.Serialization;

using AtomSandbox.Export;
using AtomSandbox.Simulation;
using AtomSandbox.Tools;
using AtomSandbox.UI;

using WindowsFormsTools.Core;
using WindowsFormsTools.InputControls;
using WindowsFormsTools.Miscellaneous;

namespace AtomSandbox
{
    public partial class MainForm : Form
    {
        #region Variables classes

        private class EnvironmentVariables(MainForm host) : IEnvironmentVariables
        {
            private MainForm particlesForm = host;

            public BoundState BoundState
            {
                get => (BoundState)particlesForm.boundStateButton.StateIndex;
                set => particlesForm.boundStateButton.StateIndex = (int)value;
            }

            public ForceFieldState ForceFieldState
            {
                get => (ForceFieldState)particlesForm.fieldStateButton.StateIndex;
                set => particlesForm.fieldStateButton.StateIndex = (int)value;
            }

            public float BouncityFactor => particlesForm.bouncityRangeControl.Value;
            public float DampingFactor => particlesForm.dampingRangeControl.Value;
            public float TimeFactor => particlesForm.timeRangeControl.Value;
            public bool IsRandomTrajectoryEnabled => particlesForm.randomizationRangeControl.IsChecked;
            public float TrajectoryRandomizationFactor => particlesForm.randomizationRangeControl.Value;
            public float CollisionDampingFactor => particlesForm.collisionDampingRangeControl.Value;
            public float JointElasticityFactor => particlesForm.elasticityRangeControl.Value;
            public bool UseMassProportionalStabilization => particlesForm.propStabilizationCheckBox.Checked;
            public bool IsReflectionEnabled => particlesForm.reflectionCheckBox.Checked;
            public float BoundSize => particlesForm.boundSizeRangeControl.Value;
            public bool ForceFieldEnabled => particlesForm.controlVariables.IsMouseLeftPressing;
            public Vector2 ForceFieldCenter => particlesForm.controlVariables.WorldMousePosition;
            public float ForceFieldRadius => particlesForm.fieldRadiusRangeControl.Value;
            public float ForceFieldPower => particlesForm.fieldPowerRangeControl.Value;
            public float ForceFieldAngle => particlesForm.fieldAngleRangeControl.Value;
            public bool IsInteractionEnabled => particlesForm.interactionCheckBox.Checked;
            public float ForceRadius => particlesForm.forceRadiusRangeControl.Value;
            public Vector2 LinearForce => particlesForm.linearGravityVectorControl.Value;
            public bool IsStrongForceEnabled => particlesForm.strongForceRangeControl.IsChecked;
            public float StrongForcePower => particlesForm.strongForceRangeControl.Value;
            public float StrongForceRadius => particlesForm.strongForceRadiusRangeControl.Value;
            public float StrongForceEffectiveRadius => particlesForm.strongForceEffectiveRadiusRangeControl.Value;
            public float StrongForceMinParticleRadius => particlesForm.strongForceStartRadiusRangeControl.Value;
            public bool UseStrongForceSurfaceDistance => particlesForm.strongForceEffectiveRadiusRangeControl.IsChecked;
            public bool IsQuadForceEnabled => particlesForm.squareForcePowerRangeControl.IsChecked;
            public float QuadForcePower => particlesForm.squareForcePowerRangeControl.Value;
            public float QuadForceRadius => particlesForm.squareForceRadiusRangeControl.Value;
            public float QuadForceMinParticleRadius => particlesForm.squareForceStartRadiusRangeControl.Value;
            public float QuadForceEffectiveRadius => particlesForm.squareForceEffectiveRadiusRangeControl.Value;
            public bool IsGravityForceEnabled => particlesForm.gravityRangeControl.IsChecked;
            public float GravityForcePower => particlesForm.gravityRangeControl.Value;
            public float GravityForceRadius => particlesForm.gravityRadiusRangeControl.Value;
            public bool IsGravityToCenterEnabled => particlesForm.gravityToCenterRangeControl.IsChecked;
            public float GravityToCenterPower => particlesForm.gravityToCenterRangeControl.Value;
            public bool IsMagneticForceEnabled => particlesForm.magneticForceRangeControl.IsChecked;
            public float MagneticForcePower => particlesForm.magneticForceRangeControl.Value;
            public float MagneticForceRadius => particlesForm.magneticForceRadiusRangeControl.Value;
            public bool IsDecayEnabled => particlesForm.decayCheckBox.Checked;
            public int DecayNumber => (int)particlesForm.numOfDecayedPacticlesRangeControl.Value;
            public float DecayAngle => particlesForm.decayAngleRangeControl.Value;
            public bool IsStabilizationEnabled => particlesForm.stabilizationCheckBox.Checked;
            public bool UseExtraStabilization => particlesForm.stabilizationRangeControl.IsChecked;
            public float ExtraStabilizationDistance => particlesForm.stabilizationRangeControl.Value;
            public float ExtraStabilizationStartRadius => particlesForm.stabilizationStartRadiusRangeControl.Value;
        }

        private class ControlVariables(MainForm host) : IControlVariables
        {
            private MainForm particlesForm = host;

            public ControlCommands ControlCommand { get; set; }
            public bool IsMouseLeftPressing { get; set; }
            public bool IsMouseRightPressing { get; set; }
            public Vector2 WorldMousePosition { get; set; }
            public Vector2 ScreenMouseDown { get; set; }
            public Vector2 WorldMouseDown { get; set; }

            public CircularVelocityState CircularVelocityState
            {
                get => (CircularVelocityState)particlesForm.circularVelocityStateButton.StateIndex;
                set => particlesForm.circularVelocityStateButton.StateIndex = (int)value;
            }

            public bool IsPushDirectionReversed => particlesForm.pushDirCheckBox.Checked;
            public bool CalcReverseImpulse => particlesForm.reverseImpulseCheckBox.Checked;
            public bool PredictParticleTrajectory => particlesForm.predictionRangeControl.IsChecked;
            public float PredictionTime => particlesForm.predictionRangeControl.Value;
            public bool UsePipette => particlesForm.dropperCheckBox.Checked;
            public bool IsPushTemplateUsed => particlesForm.pushTemplateCheckBox.Checked;
            public float TemplatePointRadius => particlesForm.templatePointRadiusRangeControl.Value;
            public float Zoom => particlesForm.canvas.Zoom;
        }

        private class ParticleInitVariables(MainForm host) : IParticleInitVariables
        {
            private MainForm particlesForm = host;

            public float Mass => particlesForm.massRangeControl.Value;
            public float Radius => particlesForm.ballSizeRangeControl.Value;
            public float Charge => particlesForm.chargeRangeControl.Value;
            public float LifeTime => particlesForm.lifeRangeControl.Value;
            public int TailLength => (int)particlesForm.tailLengthRangeControl.Value;
            public Color Color => particlesForm.particleColorControl.Value;
            public float SpeedFactor => particlesForm.speedRangeControl.Value;
        }

        #endregion

        #region Constants

        private const string ResourcesDirectoryPath = "Resources";
        private const string ParticleInfoListFilePath = @"Resources\particleInfo.xml";
        private const string SavedSessionsDirectoryPath = @"Resources\Sessions";
        private const string SavedVariablesDirectoryPath = @"Resources\EnvironmentVariables";

        private static readonly List<Type> exportTypes = new List<Type>
        {
            typeof(RangeControl),
            typeof(CheckBox),
            typeof(VectorControl),
            typeof(ColorControl),
            typeof(StateButton)
        };

        #endregion

        #region Variables

        private Dictionary<string, ParticleInfo> particleInfos;
        private Button? clickedParticleInfoButton;

        private EnvironmentVariables environmentVariables;
        private ControlVariables controlVariables;
        private ParticleInitVariables particleInitVariables;

        private Scene scene;
        private SceneUI sceneUI;
        private SceneLogger sceneLogger;

        #endregion

        public MainForm()
        {
            InitializeComponent();

            environmentVariables = new EnvironmentVariables(this);
            controlVariables = new ControlVariables(this);
            particleInitVariables = new ParticleInitVariables(this);

            scene = new Scene(environmentVariables);
            sceneUI = new SceneUI(scene, controlVariables, particleInitVariables,
                environmentVariables, forceFieldCollapsiblePanel.HeaderColor);
            sceneLogger = new SceneLogger(scene, environmentVariables, controlVariables,
                logTextBox1, logTextBox2, logTextBox3, logTextBox4, logTextBox5, logTextBox6);

            sceneUI.ParticleSelected += sceneController_ParticleSelected;

            initRepositories();
            loadParticleInfo();
        }

        #region Control methods

        private void initControlsFromInfo(ParticleInfo info)
        {
            particleColorControl.Value = info.Color;
            ballSizeRangeControl.Value = info.Radius;
            massRangeControl.Value = info.Mass;
            chargeRangeControl.Value = info.Charge;
            lifeRangeControl.Value = info.LifeTime;
            tailLengthRangeControl.Value = info.TailLength;
        }

        private void initControlsFromParticle(Particle particle)
        {
            particleColorControl.Value = particle.Color;
            ballSizeRangeControl.Value = particle.Radius;
            massRangeControl.Value = particle.Mass;
            chargeRangeControl.Value = particle.Charge;
            lifeRangeControl.Value = particle.LifeTime;
            tailLengthRangeControl.Value = particle.TailLength;
        }

        private void loadListOfParticleButtons()
        {
            if (particlesInfoTablePanel.Controls.Count != 0)
                particlesInfoTablePanel.Controls.Clear();

            int buttonWidth = (particleInfoFlowLayoutPanel.Width - SystemInformation.VerticalScrollBarWidth) / 2 -
                              addParticleInfoButton.Margin.Left - 4;

            foreach (var particleInfo in particleInfos)
            {
                var button = new Button
                {
                    BackColor = particleInfo.Value.Color,
                    Text = particleInfo.Value.Name,
                    Width = buttonWidth
                };
                button.Click += (sender, e) =>
                {
                    if(sender is Button)
                    {
                        var currentButton = (Button)sender;
                        initControlsFromInfo(particleInfos[currentButton.Text]);
                    }
                };
                button.MouseDown += (sender, e) =>
                {
                    if (e.Button == MouseButtons.Right && sender is Button)
                    {
                        clickedParticleInfoButton = (Button?)sender;
                        particleButtonContextMenuStrip.Show(MousePosition);
                    }
                };
                particlesInfoTablePanel.Controls.Add(button);
            }
        }

        private void initRangeControl(RangeControl rangeControl, RangeControlInfo settings)
        {
            if (settings != null)
            {
                rangeControl.A = settings.A;
                rangeControl.B = settings.B;
                rangeControl.Value = settings.Value;
                rangeControl.IsChecked = settings.IsChecked;
            }
        }

        #endregion

        #region Save and Load

        private void initRepositories()
        {
            if (!Directory.Exists(ResourcesDirectoryPath))
                Directory.CreateDirectory(ResourcesDirectoryPath);

            if (!Directory.Exists(SavedVariablesDirectoryPath))
                Directory.CreateDirectory(SavedVariablesDirectoryPath);

            if (!Directory.Exists(SavedSessionsDirectoryPath))
                Directory.CreateDirectory(SavedSessionsDirectoryPath);
        }

        private SimulationInfo getSimulationInfo()
        {
            return new SimulationInfo
            {
                SceneInfo = new SceneInfo
                {
                    Particles = scene.Particles,
                    Points = scene.Points,
                    JointContainer = scene.JointedParticles
                },
                FormInfo = getFormInfo()
            };
        }

        private Dictionary<Type, List<Control>> getExportControls()
        {
            return flowLayoutPanel.GetAllChildren(exportTypes, particlePropertyControl);
        }

        private FormInfo getFormInfo()
        {
            var writableControls = getExportControls();

            var rangeControls = writableControls.ContainsKey(typeof(RangeControl))
               ? writableControls[typeof(RangeControl)].Select(rc => getInfoFromRangeControl((RangeControl)rc))
                  .ToArray()
               : null;

            var checkBoxControls = writableControls.ContainsKey(typeof(CheckBox))
               ? writableControls[typeof(CheckBox)].Select(
                     cbc => new CheckBoxInfo { Name = cbc.Name, Checked = ((CheckBox)cbc).Checked })
                  .ToArray()
               : null;

            var vectorControls = writableControls.ContainsKey(typeof(VectorControl))
               ? writableControls[typeof(VectorControl)].Select(
                     cbc => new VectorInfo { Name = cbc.Name, Value = ((VectorControl)cbc).Value })
                  .ToArray()
               : null;

            var colorControls = writableControls.ContainsKey(typeof(ColorControl))
               ? writableControls[typeof(ColorControl)].Select(
                     cbc => new ColorInfo { Name = cbc.Name, Color = ((ColorControl)cbc).Value })
                  .ToArray()
               : null;

            var stateControls = writableControls.ContainsKey(typeof(StateButton))
               ? writableControls[typeof(StateButton)].Select(
                     sbc => new StateInfo { Name = sbc.Name, State = ((StateButton)sbc).StateIndex })
                  .ToArray()
               : null;

            return new FormInfo
            {
                RangeControls = rangeControls,
                CheckBoxControls = checkBoxControls,
                VectorControls = vectorControls,
                ColorControls = colorControls,
                StateControls = stateControls,

                StartPushPoint = sceneUI.StartPushPoint,
                EndPushPoint = sceneUI.EndPushPoint,
                ZoomPow = canvas.ZoomPow,
                SceneOffset = canvas.Offset
            };
        }

        private RangeControlInfo getInfoFromRangeControl(RangeControl rangeControl)
        {
            return new RangeControlInfo
            {
                Name = rangeControl.Name,
                A = rangeControl.A,
                B = rangeControl.B,
                Value = rangeControl.Value,
                IsChecked = rangeControl.IsChecked
            };
        }

        private void applySimulationInfo(SimulationInfo simulationInfo)
        {
            if(simulationInfo.FormInfo != null)
                applyFormInfo(simulationInfo.FormInfo);

            var sceneInfo = simulationInfo.SceneInfo;

            scene.Clear();
            sceneUI.Clear();

            if(sceneInfo != null)
            {
                if (sceneInfo.Particles != null)
                    scene.Particles.AddRange(sceneInfo.Particles);

                if (sceneInfo.Points != null)
                    scene.Points.AddRange(sceneInfo.Points);

                if (sceneInfo.JointContainer != null)
                    scene.JointedParticles.Copy(sceneInfo.JointContainer);
            }

            foreach (var joint in scene.JointedParticles.Joints)
            {
                joint.FirstParticle = scene.Particles.FirstOrDefault(p => p.ParticleID == joint.FirstParticleID) ??
                    scene.Points.First(p => p.ParticleID == joint.FirstParticleID);
                joint.SecondParticle = scene.Particles.FirstOrDefault(p => p.ParticleID == joint.SecondParticleID) ??
                    scene.Points.First(p => p.ParticleID == joint.SecondParticleID);
            }
        }

        private void applyFormInfo(FormInfo formInfo)
        {
            var infoControls = getExportControls();

            if (infoControls.ContainsKey(typeof(RangeControl)) && formInfo.RangeControls != null)
            {
                var tuples = infoControls[typeof(RangeControl)].Join(formInfo.RangeControls, rc => rc.Name,
                   rci => rci.Name, Tuple.Create);
                foreach (var pair in tuples)
                {
                    initRangeControl((RangeControl)pair.Item1, pair.Item2);
                }
            }

            if (infoControls.ContainsKey(typeof(CheckBox)) && formInfo.CheckBoxControls != null)
            {
                var tuples = infoControls[typeof(CheckBox)].Join(formInfo.CheckBoxControls, cbc => cbc.Name,
                   cbi => cbi.Name, Tuple.Create);
                foreach (var pair in tuples)
                {
                    var chechBox = (CheckBox)pair.Item1;
                    chechBox.Checked = pair.Item2.Checked;
                }
            }

            if (infoControls.ContainsKey(typeof(VectorControl)) && formInfo.VectorControls != null)
            {
                var tuples = infoControls[typeof(VectorControl)].Join(formInfo.VectorControls, vc => vc.Name,
                   vci => vci.Name, Tuple.Create);
                foreach (var pair in tuples)
                {
                    var vectorControl = (VectorControl)pair.Item1;
                    vectorControl.Value = pair.Item2.Value;
                }
            }

            if (infoControls.ContainsKey(typeof(ColorControl)) && formInfo.ColorControls != null)
            {
                var tuples = infoControls[typeof(ColorControl)].Join(formInfo.ColorControls, cc => cc.Name,
                   rcp => rcp.Name, Tuple.Create);
                foreach (var pair in tuples)
                {
                    var colorControl = (ColorControl)pair.Item1;
                    colorControl.Value = pair.Item2.Color;
                }
            }

            if (infoControls.ContainsKey(typeof(StateButton)) && formInfo.StateControls != null)
            {
                var tuples = infoControls[typeof(StateButton)].Join(formInfo.StateControls, sbc => sbc.Name,
                   si => si.Name, Tuple.Create);
                foreach (var pair in tuples)
                {
                    var stateButton = (StateButton)pair.Item1;
                    stateButton.StateIndex = pair.Item2.State;
                }
            }

            sceneUI.StartPushPoint = formInfo.StartPushPoint;
            sceneUI.EndPushPoint = formInfo.EndPushPoint;

            canvas.ZoomPow = formInfo.ZoomPow;
            canvas.Offset = formInfo.SceneOffset;
        }

        private void loadParticleInfo()
        {
            ParticleInfoList? particlesList = null;

            var serializer = new XmlSerializer(typeof(ParticleInfoList));
            particleInfos = new Dictionary<string, ParticleInfo>();

            if (File.Exists(ParticleInfoListFilePath))
            {
                using (StreamReader reader = new StreamReader(ParticleInfoListFilePath))
                {
                    try
                    {
                        particlesList = serializer.Deserialize(reader) as ParticleInfoList;
                    }
                    catch
                    {
                        MessageBox.Show(
                           "Some error appeared while reading particles' info file. Default list of particles is loaded.");
                    }
                }
            }

            if (particlesList == null ||
                particlesList.ParticlesInfo == null ||
                particlesList.ParticlesInfo.Length == 0)
            {
                using (Stream fileStream = File.Open(ParticleInfoListFilePath, FileMode.Create))
                using (TextWriter textWriter = new StreamWriter(fileStream))
                {
                    serializer.Serialize(textWriter, ParticleInfoList.DefauldParticlesList);
                }
                particlesList = ParticleInfoList.DefauldParticlesList;
            }

            foreach (var particleInfo in particlesList.ParticlesInfo)
                particleInfos.Add(particleInfo.Name, particleInfo);

            loadListOfParticleButtons();
        }

        private void saveParticleInfos(ParticleInfo[] particleInfoList)
        {
            var serializer = new XmlSerializer(typeof(ParticleInfoList));
            using (TextWriter textWriter = new StreamWriter(ParticleInfoListFilePath))
            {
                serializer.Serialize(textWriter, new ParticleInfoList { ParticlesInfo = particleInfoList });
            }
        }

        #endregion

        #region Events

        #region Miscellaneous events

        private void MainForm_Load(object sender, EventArgs e)
        {
            canvas.ZoomChanged += sceneUI.OnZoomChanged;

            timer.Enabled = true;
        }

        private void sceneController_ParticleSelected(object? sender, ParticleEventArgs e)
        {
            particlePropertyControl.TargetObject = e.Particle;
        }

        private void ballSizeRangeControl_ValueChanged(object? sender, FloatValueChangedEventArgs e)
        {
            if (massRangeControl.IsChecked)
                massRangeControl.Value = (float)Math.PI * e.NewValue * e.NewValue;
        }

        private void dropperCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            controlVariables.ControlCommand = dropperCheckBox.Checked
                ? ControlCommands.Dropping
                : ControlCommands.None;
        }

        private void pullCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            controlVariables.ControlCommand = pullCheckBox.Checked ? ControlCommands.Pull : ControlCommands.None;
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            scene.Render(g);
            sceneUI.Render(g);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9)
            {
                int infoIndex = e.KeyCode - Keys.D1;
                if (particleInfos.Count > infoIndex)
                {
                    initControlsFromInfo(particleInfos.Values.ElementAt(infoIndex));
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    #region Delete

                    case Keys.Delete:
                        {
                            sceneUI.DeleteLastParticle(e.Control);
                            break;
                        }

                    #endregion

                    #region B

                    case Keys.B:
                        {
                            environmentVariables.BoundState = environmentVariables.BoundState == BoundState.None
                               ? BoundState.RectangularBound
                               : BoundState.None;
                            break;
                        }

                    #endregion

                    #region Key D

                    case Keys.D:
                        {
                            sceneUI.DeleteUnderMouse();
                            break;
                        }

                    #endregion

                    #region Key X

                    case Keys.X:
                        if (e.Control)
                        {
                            controlVariables.ControlCommand = controlVariables.ControlCommand == ControlCommands.Cut
                               ? ControlCommands.None
                               : ControlCommands.Cut;
                        }
                        break;

                    #endregion

                    #region Key U

                    case Keys.U:
                        {
                            pushTemplateCheckBox.Checked = !pushTemplateCheckBox.Checked;

                            if (pushTemplateCheckBox.Checked &&
                                controlVariables.IsMouseLeftPressing &&
                                controlVariables.ControlCommand != ControlCommands.Pull)
                            {
                                if (controlVariables.IsPushDirectionReversed)
                                {
                                    sceneUI.StartPushPoint = controlVariables.WorldMouseDown;
                                    sceneUI.EndPushPoint = controlVariables.WorldMousePosition;
                                }
                                else
                                {
                                    sceneUI.StartPushPoint = controlVariables.WorldMousePosition;
                                    sceneUI.EndPushPoint = controlVariables.WorldMouseDown;
                                }
                            }
                            break;
                        }

                    #endregion

                    #region Key F

                    case Keys.F:
                        {
                            ++fieldStateButton.StateIndex;
                            break;
                        }

                    #endregion

                    #region Key N

                    case Keys.N:
                        {
                            sceneUI.CreateParticle(false);
                            break;
                        }

                    #endregion

                    #region Key I

                    case Keys.I:
                        {
                            controlVariables.ControlCommand = controlVariables.ControlCommand == ControlCommands.Pull
                               ? ControlCommands.None
                               : ControlCommands.Pull;
                            pullCheckBox.Checked = controlVariables.ControlCommand == ControlCommands.Pull;
                            break;
                        }

                    #endregion

                    #region Key E

                    case Keys.E:
                        if (e.Control)
                            controlVariables.ControlCommand = controlVariables.ControlCommand == ControlCommands.Erase
                               ? ControlCommands.None
                               : ControlCommands.Erase;
                        break;

                    #endregion

                    #region Key Q

                    case Keys.Q:
                        if (e.Control)
                        {
                            controlVariables.ControlCommand = controlVariables.ControlCommand == ControlCommands.Select
                               ? ControlCommands.None
                               : ControlCommands.Select;
                        }
                        break;

                    #endregion

                    #region Key J

                    case Keys.J:
                        sceneUI.JointSelectedParticles();
                        break;

                    #endregion

                    #region Key P

                    case Keys.P:
                        sceneUI.CreateParticle(true);
                        break;

                    #endregion

                    #region Key H

                    case Keys.H:
                        controlVariables.CircularVelocityState = controlVariables.CircularVelocityState != CircularVelocityState.None
                           ? CircularVelocityState.None
                           : CircularVelocityState.CommonMass;
                        break;

                    #endregion

                    #region Key C

                    case Keys.C:
                        if (e.Control)
                            controlVariables.ControlCommand = controlVariables.ControlCommand == ControlCommands.Copy
                               ? ControlCommands.None
                               : ControlCommands.Copy;
                        break;

                    #endregion

                    #region Key S

                    case Keys.S:
                        if (e.Control)
                            saveSceneButton_Click(null, null);
                        break;

                    #endregion

                    #region Key L

                    case Keys.L:
                        if (e.Control)
                            loadSceneButton_Click(null, null);
                        break;

                    #endregion

                    #region Key V

                    case Keys.V:
                        if (e.Control)
                            controlVariables.ControlCommand = controlVariables.ControlCommand == ControlCommands.Paste
                               ? ControlCommands.None
                               : ControlCommands.Paste;
                        break;

                    #endregion

                    #region Key Space

                    case Keys.Space:
                        {
                            foreach (var p in scene.Particles)
                                p.Velocity.Set(0, 0);
                            break;
                        }

                    #endregion
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            float timeDelta = 0.01f * environmentVariables.TimeFactor * timer.Interval; // getDeltaTime(); // better use stable delta

            sceneUI.Update(timeDelta);
            scene.Update(timeDelta);

            canvas.Invalidate();

            if (logCollapsiblePanel.PanelState == CollapsiblePanel.PanelStates.Expanded)
                sceneLogger.LogInfo();
        }

        #endregion

        #region Mouse Events

        public void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            controlVariables.WorldMousePosition = canvas.TransformToWorld(new Vector2(e.Location.X, e.Location.Y));
        }

        public void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            controlVariables.ScreenMouseDown = new Vector2(e.Location.X, e.Location.Y);
            controlVariables.WorldMouseDown = canvas.TransformToWorld(controlVariables.ScreenMouseDown);

            if (e.Button == MouseButtons.Left)
            {
                controlVariables.IsMouseLeftPressing = true;

                if (controlVariables.UsePipette)
                {
                    var info = scene.GetParticleInfoAtPoint(controlVariables.WorldMouseDown);
                    if (info != null)
                        initControlsFromParticle(info.Particle);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                controlVariables.IsMouseRightPressing = true;
            }

            sceneUI.OnMouseDown(e);
        }

        public void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    controlVariables.IsMouseLeftPressing = false;
                    break;
                case MouseButtons.Right:
                    controlVariables.IsMouseRightPressing = false;
                    break;
            }

            sceneUI.OnMouseUp(e);

            switch (e.Button)
            {
                case MouseButtons.Left:
                    if(controlVariables.ControlCommand == ControlCommands.Paste &&
                        reverseImpulseCheckBox.Checked)
                    {
                        reverseImpulseCheckBox.Checked = false;
                    }

                    if(controlVariables.ControlCommand != ControlCommands.Erase &&
                        controlVariables.ControlCommand != ControlCommands.Pull)
                    {
                        controlVariables.ControlCommand = ControlCommands.None;
                    }

                    break;

            }
        }

        #endregion

        #region Buttons

        private void resetLocationButton_Click(object sender, EventArgs e)
        {
            canvas.Offset = Vector2.Zero;
            canvas.Zoom = 1;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            scene.Clear();
        }

        private void saveSceneButton_Click(object sender, EventArgs e)
        {
            var scene = getSimulationInfo();
            saveFileDialog.Filter = "SandBox Particles session | *.sps";
            saveFileDialog.InitialDirectory = SavedSessionsDirectoryPath;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var serializer = new XmlSerializer(typeof(SceneInfo));
                using (TextWriter textWriter = new StreamWriter(saveFileDialog.FileName))
                {
                    serializer.Serialize(textWriter, scene);
                }
            }
        }

        private void loadSceneButton_Click(object sender, EventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SimulationInfo));

            openFileDialog.Filter = "SandBox Particles session | *.sps";
            openFileDialog.InitialDirectory = SavedSessionsDirectoryPath;

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            SimulationInfo? session = null;
            using (StreamReader reader = new StreamReader(openFileDialog.FileName))
            {
                session = serializer.Deserialize(reader) as SimulationInfo;
            }

            if (session != null)
                applySimulationInfo(session);

        }

        private void convertButton_Click(object sender, EventArgs e)
        {
            if (openMultipleFileDialog.ShowDialog() == DialogResult.OK)
            {
                var files = openMultipleFileDialog.FileNames;
                if (files != null && files.Length > 0)
                {
                    SimulationInfo currentSession = getSimulationInfo();
                    XmlSerializer serializer = new XmlSerializer(typeof(SceneInfo));

                    foreach (var file in files)
                    {
                        SimulationInfo? session;
                        using (StreamReader reader = new StreamReader(file))
                        {
                            session = serializer.Deserialize(reader) as SimulationInfo;
                        }

                        if (session != null)
                        {
                            applySimulationInfo(session);
                            session = getSimulationInfo();

                            using (TextWriter textWriter = new StreamWriter(file))
                            {
                                serializer.Serialize(textWriter, session);
                            }
                        }
                    }
                    applySimulationInfo(currentSession);
                }
            }
        }

        private void addParticleInfoButton_Click(object sender, EventArgs e)
        {
            var newParticleForm = new ParticleForm();
            newParticleForm.BannedNames = particleInfos.Keys;

            if (sceneUI.SelectedParticle != null)
            {
                newParticleForm.Radius = sceneUI.SelectedParticle.Radius;
                newParticleForm.Mass = sceneUI.SelectedParticle.Mass;
                newParticleForm.Charge = sceneUI.SelectedParticle.Charge;
                newParticleForm.LifeTime = sceneUI.SelectedParticle.LifeTime;
                newParticleForm.Color = sceneUI.SelectedParticle.Color;
                newParticleForm.TailLength = sceneUI.SelectedParticle.TailLength;
            }

            if (newParticleForm.ShowDialog() == DialogResult.OK)
            {
                particleInfos.Add(newParticleForm.ParticleName,
                   new ParticleInfo
                   {
                       Name = newParticleForm.ParticleName,
                       Radius = newParticleForm.Radius,
                       Mass = newParticleForm.Mass,
                       Charge = newParticleForm.Charge,
                       LifeTime = newParticleForm.LifeTime,
                       Color = newParticleForm.Color,
                       TailLength = newParticleForm.TailLength
                   });

                var particleInfoList = particleInfos.Values.ToArray();
                saveParticleInfos(particleInfoList);
                loadListOfParticleButtons();
            }
        }

        private void convertPointButton_Click(object sender, EventArgs e)
        {
            sceneUI.ConvertSelectedParticlesToPoints();
        }

        #endregion

        #region Context menu

        private void editParticleInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clickedParticleInfoButton == null)
                return;

            var oldName = clickedParticleInfoButton.Text;
            var particleInfo = particleInfos[oldName];

            var editParticleForm = new ParticleForm
            {
                Mode = ParticleForm.ParticleFormMode.EditParticle,
                Name = "Edit particle info",
                ParticleName = particleInfo.Name,
                Radius = particleInfo.Radius,
                Mass = particleInfo.Mass,
                Charge = particleInfo.Charge,
                LifeTime = particleInfo.LifeTime,
                Color = particleInfo.Color,
                TailLength = particleInfo.TailLength
            };

            if (editParticleForm.ShowDialog() == DialogResult.OK)
            {
                particleInfo = new ParticleInfo
                {
                    Name = editParticleForm.ParticleName,
                    Radius = editParticleForm.Radius,
                    Mass = editParticleForm.Mass,
                    Charge = editParticleForm.Charge,
                    LifeTime = editParticleForm.LifeTime,
                    Color = editParticleForm.Color,
                    TailLength = editParticleForm.TailLength
                };

                if (oldName == particleInfo.Name)
                    particleInfos[oldName] = particleInfo;
                else
                {
                    particleInfos.Remove(oldName);
                    particleInfos.Add(particleInfo.Name, particleInfo);
                }

                var particleInfoList = particleInfos.Values.ToArray();
                saveParticleInfos(particleInfoList);
                loadListOfParticleButtons();
            }
        }

        private void deleteParticleInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clickedParticleInfoButton == null)
                return;

            particleInfos.Remove(clickedParticleInfoButton.Text);
            var particleInfoList = particleInfos.Values.ToArray();
            saveParticleInfos(particleInfoList);
            loadListOfParticleButtons();
        }

        #endregion

        #endregion
    }
}
