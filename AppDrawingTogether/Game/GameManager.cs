using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawingTogether;
using DrawingTogether.Net;

namespace AppDrawingTogether.Game
{
    internal class GameManager
    {
        private long _startTick;
        public long StartTick { get { return _startTick; } }

        public string PlayerName { get; set; }

        public PictureBox Canvas;
        private int _MsTick;
        private int _MsTickDrawing;

        private Dictionary<Color, Pen> PenCache;

        private Color _currentColor = Color.Black;

        private Point pastPoint;

        private List<LinePortion> _lines;

        public bool StopDrawing = false;

        private int pastCount;

        private bool MouseDown = false;
        public float FrameRate { get { return 1000f / _MsTick; } set { _MsTick = (int)(1000f * (1 / value)); } }
        public float DrawingFrameRate { get { return 1000f / _MsTickDrawing; } set { _MsTickDrawing = (int)(1000f * (1 / value)); } }

        public GameManager(PictureBox Canvas, float frameRate, string playerName)
        {
            PlayerName = playerName;
            _lines = new List<LinePortion>();
            this.Canvas = Canvas;
            FrameRate = frameRate;
            _startTick = DateTime.Now.Ticks;
            
            AddEvents();

            StartCanvasRefresh();
        }
        /// <summary>
        /// Adds all the events for object in the class after initialising.
        /// </summary>
        private void AddEvents()
        {
            Canvas.Paint += Canvas_Paint;
            Canvas.MouseDown += OnMouseDown;
            Canvas.MouseUp += OnMouseUp;
            Canvas.MouseMove += OnMouseMove;
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
            if(MouseDown) AddLine(pastPoint, GetRelativeMousePos(e.Location));
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
            pastPoint = GetRelativeMousePos(e.Location);

        }
        /// <summary>
        /// Gets the mouse position relative to the canvas picturebox. inputs the global form mouse position.
        /// </summary>
        /// <param name="absolutePos"></param>
        /// <returns></returns>
        private Point GetRelativeMousePos(Point absolutePos)
        {
            return new Point(absolutePos.X - Canvas.Location.X, absolutePos.Y - Canvas.Location.Y);
        }
        /// <summary>
        /// Starts the async background loop that draws the canvas.
        /// </summary>
        private async void StartCanvasRefresh()
        {
            while(!StopDrawing)
            {
                Canvas.Refresh();
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
            LinePortion line = new LinePortion(age, PlayerName, _currentColor, pastMousePos, mousePos);
            _lines.Add(line);
        }
        /// <summary>
        /// draws the canvas.
        /// paints all lines stored in the gameManager instance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if(PenCache == null ) PenCache = new Dictionary<Color, Pen>();
            SortLines();
            foreach(LinePortion line in _lines)
            {
                if (!line.IsVisible) continue;
                if(!PenCache.ContainsKey(line.Color)) PenCache.Add(line.Color, new Pen(line.Color));
                g.DrawLine(PenCache[line.Color], line.StartPos, line.EndPos);
            }
        }
        /// <summary>
        /// Sorts lines if the amount of lines have changed.
        /// normally lines amount only changes if a line gets added.
        /// </summary>
        private void SortLines()
        {
            if(pastCount != _lines.Count)
            {
                _lines.Sort();
                pastCount = _lines.Count;
            }
        }
    }
}
