using System.Numerics;
using System.Xml.Serialization;

namespace AtomSandbox.Simulation
{
    public class Particle
    {
        public Guid ParticleID { get; set; }

        public Vector2 Position;
        public Vector2 Velocity;

        public float Radius
        {
            get => radius;
            set
            {
                radius = value;
                if (tailPen != null)
                    tailPen.Width = 0.5f * value;
            }
        }

        public float Mass { get; set; }

        public float Charge { get; set; }

        public float InitialLifeTime { get; set; }

        public float LifeTime { get; set; }

        public PointF[] Tail { get; set; }

        [XmlIgnore]
        public int TailLength
        {
            get => Tail != null ? Tail.Length : 0;
            set
            {
                if (Tail != null)
                {
                    var temp = Tail;
                    Tail = new PointF[value];
                    if (value > 0)
                    {
                        Array.Copy(temp, Tail, Math.Min(temp.Length, Tail.Length));
                        if (temp.Length < Tail.Length)
                        {
                            int last = temp.Length - 1;
                            for (int i = temp.Length; i < Tail.Length; ++i)
                                Tail[i] = temp[last];
                        }
                    }
                }
            }
        }

        [XmlIgnore]
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                colorBrush = new SolidBrush(color);
                if (tailPen != null)
                    tailPen.Color = Color.FromArgb(color.A / 2, color);
            }
        }

        [XmlElement("Color")]
        public int ParticleColorAsArgb
        {
            get => Color.ToArgb();
            set => Color = Color.FromArgb(value);
        }

        public bool IsFrozen { get; set; }

        private float radius;
        private Color color;
        private Brush colorBrush;
        private Pen tailPen;

        public Particle()
        {
            tailPen = new Pen(Color.FromArgb(100, color), 0.5f * Radius);
            tailPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            tailPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            colorBrush = new SolidBrush(color);

            Tail = [];
            ResetTail();
        }

        public Particle(Vector2 position, Vector2 velocity, Color color,
            float radius, float mass, float lifeTime, float charge, int tailLength)
        {
            ParticleID = Guid.NewGuid();
            Position = position;
            Velocity = velocity;
            Color = color;
            Radius = radius;
            Mass = mass;
            Charge = charge;
            InitialLifeTime = LifeTime = lifeTime;

            colorBrush = new SolidBrush(color);

            tailPen = new Pen(Color.FromArgb(100, color), 0.5f * Radius);
            tailPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            tailPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            Tail = new PointF[tailLength];
            ResetTail();
        }

        public Particle(float x, float y, float vx, float vy, Color color,
            float radius, float mass, float lifeTime, float charge, int tailLength)
            : this(new Vector2(x, y), new Vector2(vx, vy), color,
                  radius, mass, lifeTime, charge, tailLength)
        {

        }

        public Particle(Particle other)
            : this(
            other.Position,
            other.Velocity,
            other.color,
            other.Radius,
            other.Mass,
            other.LifeTime,
            other.Charge,
            other.Tail.Length)
        {
        }

        public void ResetTail()
        {
            var point = (PointF)Position;
            for (int i = 0; i < Tail.Length; ++i)
                Tail[i] = point;
        }

        public void Render(Graphics g)
        {
            if (!IsFrozen)
                g.FillEllipse(colorBrush, Position.X - Radius, Position.Y - Radius, 2 * Radius, 2 * Radius);
            if (Tail.Length > 1)
                g.DrawLines(tailPen, Tail);
        }

        public void Draw(Graphics g, Vector2 position, int alpha)
        {
            var tempBrush = new SolidBrush(Color.FromArgb(alpha, Color));
            g.FillEllipse(tempBrush, position.X - Radius, position.Y - Radius, 2 * Radius, 2 * Radius);
            if (Tail.Length > 1)
                g.DrawLines(tailPen, Tail);
        }

        public void Update(float delta)
        {
            Position += delta * Velocity;
            if (!IsFrozen)
            {
                for (int i = Tail.Length - 1; i > 0; --i)
                    Tail[i] = Tail[i - 1];
                if (Tail.Length != 0)
                    Tail[0] = (PointF)Position;
            }
        }

        public bool Age(float timeDelta)
        {
            if (LifeTime > 0)
            {
                LifeTime -= timeDelta;
                if (LifeTime < 0)
                {
                    LifeTime = 0;
                    return true;
                }
            }
            return false;
        }

        public bool OverlapPoint(Vector2 point)
        {
            return (Position - point).LengthSquared() <= Radius * Radius;
        }

        public override string ToString()
        {
            return ParticleID.ToString();
        }

        public override int GetHashCode()
        {
            return ParticleID.GetHashCode();
        }
    }
}
