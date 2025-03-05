using System.Numerics;

namespace AtomSandbox.UI
{
    /// <summary>
    /// The set of variables defining the user interaction with the simulation
    /// </summary>
    internal interface IControlVariables
    {
        #region Mouse info

        /// <summary>
        /// Indicates if the left mouse button is pressed
        /// </summary>
        public abstract bool IsMouseLeftPressing { get; }

        /// <summary>
        /// Indicates if the right mouse button is pressed
        /// </summary>
        public abstract bool IsMouseRightPressing { get; }

        /// <summary>
        /// The position of the mouse cursor in the world coordinate system
        /// </summary>
        public abstract Vector2 WorldMousePosition { get; }

        /// <summary>
        /// The position of the mouse position in the screen coordinate system when the mouse button was pressed
        /// </summary>
        public abstract Vector2 ScreenMouseDown { get; }

        /// <summary>
        /// The position of the mouse position in the world coordinate system when the mouse button was pressed
        /// </summary>
        public abstract Vector2 WorldMouseDown { get; }

        #endregion

        /// <summary>
        /// The zoom level of the canvas
        /// </summary>
        public abstract float Zoom { get; }

        /// <summary>
        /// If true, the particle is released from mouse down position towards current mouse position.
        /// Otherwise, in opposite direction.
        /// </summary>
        public abstract bool IsPushDirectionReversed { get; }

        /// <summary>
        /// If true, the particle's velocity is computed to nullify the overall impulse in the system
        /// </summary>
        public abstract bool CalcReverseImpulse { get; }

        /// <summary>
        /// Defines if the particle's velocity is computed to have a circular trajectory.
        /// Only works with the gravitational force enabled.
        /// </summary>
        public abstract CircularVelocityState CircularVelocityState { get; }

        /// <summary>
        /// If true, the particle's trajectory is shown before it is released
        /// </summary>
        public abstract bool PredictParticleTrajectory { get; }

        /// <summary>
        /// The time in seconds the particle's trajectory is predicted
        /// </summary>
        public abstract float PredictionTime { get; }

        /// <summary>
        /// When true, the clicked particle's is copied to the initial particle variables
        /// </summary>
        public abstract bool UsePipette { get; }

        /// <summary>
        /// The current user command over the simulation
        /// </summary>
        public abstract ControlCommands ControlCommand { get; }

        /// <summary>
        /// If true, the particles are released from two template points on the scene:
        /// green - the initial point, red - the direction point
        /// </summary>
        public abstract bool IsPushTemplateUsed { get; }

        /// <summary>
        /// The visual size of the template points
        /// </summary>
        public abstract float TemplatePointRadius { get; }
    }
}
