using DrawingTogether;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppDrawingTogether.Game
{
    internal class MyCanvas : PictureBox
    {

        private long _startTick;
        public long StartTick { get { return _startTick; } }

        public string PlayerName { get; set; }

        private int _MsTick;

        private Dictionary<PenCacheKey, Pen> PenCache;

        public Color _currentColor = Color.Black;

        private Point pastPoint;

        private List<LinePortion> _lines;

        private bool StopDrawing = false;


        private int pastCount;

        private Dictionary<LineThickness, float> _lineSizes = new Dictionary<LineThickness, float>()
        {
            {LineThickness.Small, DEFAULT_WIDTH_SMALL },
            {LineThickness.Medium, DEFAULT_WIDTH_MEDIUM },
            {LineThickness.Large, DEFAULT_WIDTH_LARGE },
            {LineThickness.ExtraLarge, DEFAULT_WIDTH_EXTRA_LARGE},
            {LineThickness.Custom, DEFAULT_WIDTH_SMALL }
        };

        public static float DEFAULT_WIDTH_SMALL = 1f;
        public static float DEFAULT_WIDTH_MEDIUM = 2f;
        public static float DEFAULT_WIDTH_LARGE = 4f;
        public static float DEFAULT_WIDTH_EXTRA_LARGE = 8f;

        private LineThickness _currentLineWidth = LineThickness.Small;

        public void SetLineSize(LineThickness size)
        {
            _currentLineWidth = size;
        }
        public void SetCustomLineSize(float size)
        {
            _lineSizes[LineThickness.Custom] = size;
            _currentLineWidth = LineThickness.Custom;
        }

        private new bool MouseDown = false;
        public float FrameRate { 
            get { return 1000f / _MsTick; } 
            set {
                if (value > 0) _MsTick = (int)(1000f * (1 / value));
                else _MsTick = 0;
            } 
        }

        public MyCanvas(float frameRate)
        {
            _lines = new List<LinePortion>();

            FrameRate = frameRate;
            _startTick = DateTime.Now.Ticks;

            AddEvents();
        }


        /// <summary>
        /// Adds all the events for object in the class after initialising.
        /// </summary>
        private void AddEvents()
        {
            Paint += Canvas_Paint;
            base.MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            MouseMove += OnMouseMove;
        }
        /// <summary>
        /// runs when mouse moves. draws line if mouse button is held down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (MouseDown) AddLine(pastPoint, GetRelativeMousePos(e.Location));
            pastPoint = GetRelativeMousePos(e.Location);
        }
        /// <summary>
        /// Stops the ability to draw lines
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (MouseDown) AddLine(pastPoint, GetRelativeMousePos(e.Location));
            MouseDown = false;
        }
        /// <summary>
        /// MouseDown event, allows for lines to be drawn when mouse button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            MouseDown = true;
            pastPoint = e.Location;

        }
        private Point GetRelativeMousePos(Point absolutePos)
        {
            return absolutePos;
        }
        /// <summary>
        /// Starts the async background loop that draws the canvas.
        /// </summary>
        private async void StartCanvasRefresh()
        {
            while (!StopDrawing)
            {
                Refresh();
                if (_MsTick <= 0) continue;
                await Task.Delay(_MsTick);
            }
        }
        /// <summary>
        /// Adds a line into the lines list
        /// </summary>
        /// <param name="pastMousePos"></param>
        /// <param name="mousePos"></param>
        private void AddLine(Point pastMousePos, Point mousePos)
        {
            long age = DateTime.Now.Ticks - _startTick;
            LinePortion line = new LinePortion(age, PlayerName, _currentColor, pastMousePos, mousePos, LineWidth);
            _lines.Add(line);
        }

        public float LineWidth => _lineSizes[_currentLineWidth];
        /// <summary>
        /// draws the canvas.
        /// paints all lines stored in the gameManager instance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (PenCache == null) PenCache = new Dictionary<PenCacheKey, Pen>();
            SortLines();
            foreach (LinePortion line in _lines)
            {
                if (!line.IsVisible) continue;
                PenCacheKey key = new PenCacheKey(line.Color, line.Width);
                if (!PenCache.ContainsKey(key)) PenCache.Add(key, new Pen(line.Color, line.Width));

                g.DrawLine(PenCache[key], line.StartPos, line.EndPos);
            }
        }
        /// <summary>
        /// Used for a key in the cash dictionary. stores the line color and line width for the key.
        /// </summary>
        private struct PenCacheKey
        {
            public Color lineColor { get; set; }
            public float Thickness { get; set; }

            public PenCacheKey(Color color, float thickness)
            {
                lineColor = color;
                Thickness = thickness;
            }
        }
        /// <summary>
        /// Sorts lines if the amount of lines have changed.
        /// normally lines amount only changes if a line gets added.
        /// </summary>
        private void SortLines()
        {
            if (pastCount != _lines.Count)
            {
                _lines.Sort();
                pastCount = _lines.Count;
            }
        }


        public void Stop() => StopDrawing = true;
        internal void Start()
        {
            StartCanvasRefresh();
        }
    }
}
