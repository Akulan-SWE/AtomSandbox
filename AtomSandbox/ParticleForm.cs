namespace AtomSandbox
{
    public partial class ParticleForm : Form
    {
        public enum ParticleFormMode
        {
            NewParticle,
            EditParticle
        }

        public ParticleForm()
        {
            InitializeComponent();
        }

        public ParticleFormMode Mode { get; set; } = ParticleFormMode.NewParticle;

        private void NewParticleForm_Load(object sender, EventArgs e)
        {
            if (Mode == ParticleFormMode.EditParticle)
            {
                Text = "Edit particle";
                addButton.Text = "Edit";
            }
        }

        public string ParticleName
        {
            get => particleNameTextBox.Text;
            set => particleNameTextBox.Text = value;
        }

        public float Radius
        {
            get => (float)radiusNumericTextBox.Value;
            set => radiusNumericTextBox.Value = (decimal)value;
        }

        public float Mass
        {
            get => (float)massNumericTextBox.Value;
            set => massNumericTextBox.Value = (decimal)value;
        }

        public float Charge
        {
            get => (float)chargeNumericTextBox.Value;
            set => chargeNumericTextBox.Value = (decimal)value;
        }

        public float LifeTime
        {
            get => (float)lifeTimeNumericTextBox.Value;
            set => lifeTimeNumericTextBox.Value = (decimal)value;
        }

        public int TailLength
        {
            get => (int)tailLengthNumericUpDown.Value;
            set => tailLengthNumericUpDown.Value = value;
        }

        public Color Color
        {
            get => colorControl.Value;
            set => colorControl.Value = value;
        }

        public ICollection<string>? BannedNames { get; set; }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (BannedNames != null && BannedNames.Contains(ParticleName))
            {
                MessageBox.Show("Particle with this name already exists. Try another name.");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
