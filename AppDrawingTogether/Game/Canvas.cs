using AppDrawingTogether.Network;
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

        private Dictionary<Color, Dictionary<float, Pen>> PenCache;

        public Color _currentColor = Color.Black;

        private Point pastPoint;

        private List<LinePortion> _lines;

        private HashSet<LinePortion> _pastLines;

        public List<LinePortion> AddedLines { get; set; }


        private bool StopDrawing = false;

        private new bool MouseDown = false;


        public float LineWidth => _lineSizes[_currentLineWidth];


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


        public float FrameRate
        {
            get { return 1000f / _MsTick; }
            set
            {
                if (value > 0) _MsTick = (int)(1000f * (1 / value));
                else _MsTick = 0;
            }
        }
        private LineManager _lineManager;
        public MyCanvas(float frameRate, LineManager LineManager)
        {
            _lineManager = LineManager;
            _lines = new List<LinePortion>();
            _pastLines = new HashSet<LinePortion>();
            AddedLines = new List<LinePortion>();

            FrameRate = frameRate;
            _startTick = DateTime.Now.Ticks;

            AddEvents();
        }

        public void SetLineSize(LineThickness size)
        {
            _currentLineWidth = size;
        }
        public void SetCustomLineSize(float size)
        {
            _lineSizes[LineThickness.Custom] = size;
            _currentLineWidth = LineThickness.Custom;
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


        private bool _redrawEverything = false;
        public void FullRedraw() => _redrawEverything = true;


        /// <summary>
        /// draws the canvas.
        /// paints all lines stored in the gameManager instance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            AddedLines = _lineManager.ReadLinesFromClient();
            if (_redrawEverything) PaintFresh(g);
            else if (AddedLines.Count <= 0) PaintAdditive(g);
            else PaintMixed(g);

            _lineManager.AddAllLinesFromGame(_pastLines.ToList());
        }

        private void PaintMixed(Graphics g)
        {
            List<LinePortion> linesToDraw = GetLinesToDraw();
            PaintLines(linesToDraw, g);
            AddLinesToPast(linesToDraw);
        }

        private void PaintAdditive(Graphics g)
        {

            PaintLines(_lines,g);
            AddLinesToPast(_lines);
            _lines.Clear();
        }

        private void AddLinesToPast(List<LinePortion> lines)
        {
            foreach(LinePortion line in _lines) _pastLines.Add(line);
        }

        private List<LinePortion> GetLinesToDraw()
        {
            List<LinePortion> lines = new List<LinePortion>();

            lines.AddRange(_lines);
            lines.AddRange(AddedLines);
            lines.Sort();
            long tick = lines[lines.Count - 1].Tick;

            List<LinePortion> tempPast = _pastLines.ToList();
            int StartIndex = GetStartIndexPastTick(tick, tempPast);
            lines.AddRange(tempPast.GetRange(StartIndex, _pastLines.Count - StartIndex - 1));

            return lines;
        }

        private int GetStartIndexPastTick(long tick, List<LinePortion> lines)
        {
            lines.Sort();
            return lines.FindIndex(item => item.Tick > tick);
        }

        private void PaintLines(List<LinePortion> lines, Graphics g)
        {
            //lines.Sort();
            foreach (LinePortion line in lines)
            {
                if (!line.IsVisible) line.Color = BackColor;

                VerifyPenCache(line);

                g.DrawLine(PenCache[line.Color][line.Width], line.StartPos, line.EndPos);
            }
        }

        private void PaintFresh(Graphics g)
        {
            _redrawEverything = false;
            //g.FillRectangle(new SolidBrush(BackColor), new Rectangle(new Point(0,0), Size));
            AddLinesToPast(_lines);
            _lines.Clear();

            PaintLines(_pastLines.ToList(), g);
        }

        private void VerifyPenCache(LinePortion line)
        {
            if (PenCache == null) PenCache = new Dictionary<Color, Dictionary<float, Pen>>();
            //verifies if penColor exsists in cache
            if (!PenCache.ContainsKey(line.Color))
            {
                PenCache.Add(line.Color, new Dictionary<float, Pen>() {
                    {line.Width, MakePen(line) }
                });
                return;
            }
            //verifies if pen size exists in cache
            if (!PenCache[line.Color].ContainsKey(line.Width))
            {
                PenCache[line.Color].Add(line.Width, MakePen(line));
            }
        }
        private Pen MakePen(LinePortion line) => new Pen(line.Color, line.Width);

        /// <summary>
        /// Sorts lines if the amount of lines have changed.
        /// normally lines amount only changes if a line gets added.
        /// </summary>


        /// <summary>
        /// Starts the async background loop that draws the canvas.
        /// </summary>
        private async void StartCanvasRefresh()
        {
            while (!StopDrawing)
            {
                //Refresh();
                if (IsDisposed) return;
                InvokePaint(this, new PaintEventArgs(CreateGraphics(), new Rectangle(Location, Size)));
                if (_MsTick <= 0) continue;
                await Task.Delay(_MsTick);
            }
        }

        public void Stop() => StopDrawing = true;
        internal void Start()
        {
            StartCanvasRefresh();
        }








        //events


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
            if (MouseDown) AddLine(pastPoint, e.Location);
            pastPoint = e.Location;
        }
        /// <summary>
        /// Stops the ability to draw lines
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (MouseDown) AddLine(pastPoint, e.Location);
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
    }
}
