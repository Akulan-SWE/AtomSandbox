namespace AtomSandbox.UI
{
    /// <summary>
    /// Pen that keeps the visual width regardless of the zoom value
    /// </summary>
    internal class ZoomPen
    {
        public Pen Pen { get; private set; }

        private float originalWidth;

        public ZoomPen(Pen pen)
        {
            Pen = pen;
            originalWidth = pen.Width;
        }

        public void OnZoomChanged(float zoom)
        {
            Pen.Width = originalWidth / zoom;
        }
    }
}
