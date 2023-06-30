using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppDrawingTogether.Network;
using DrawingTogether;
using DrawingTogether.Net;

namespace AppDrawingTogether.Game
{
    internal class GameManager : GroupBox
    {

        public LineManager LineManager;


        private PictureBox _currentColorBox;
        public MyCanvas Canvas { get; set; }

        private ColorBoxesBox _colorOptions;

        public string PlayerName { get { return Canvas.PlayerName; } set { Canvas.PlayerName = value; } }
        public new Size Size { get { return base.Size; } set { base.Size = value; UpdateSizes(); } }


        public static float DEFAULT_FRAMERATE = 30f;

        private LineSizesBox<LineThickness> _lineSizesBox;

        public void StopGame()
        {
            //save game.
            //stop multiplayer connectionn.
        }

        public GameManager(Size size, Point location, string playerName)
        {
            LineManager = new LineManager(this);
            Size = size;
            Location = location;

            Canvas = new MyCanvas(DEFAULT_FRAMERATE, LineManager);
            PlayerName = playerName;
            Canvas.BorderStyle = BorderStyle.FixedSingle;

            Controls.Add(Canvas);

            AddColorBoxes(_colorBoxesSize, _colorBoxColumns, _boxesGap);

            UpdateSizes();

            GenerateLineSizeBox( 70, _currentColorBox.Height + _boxesGap * 2, _boxesGap);

            Canvas.Start();
        }
        private int _boxesGap = 5;
        private int _colorBoxesSize = 20;
        private int _colorBoxColumns = 10;
        private void AddColorBoxes(int size, int columns, int gap)
        {
            _currentColorBox = new PictureBox();
            int SizeOfBigBox = size + gap + size;
            _currentColorBox.Size = new Size(SizeOfBigBox, SizeOfBigBox);
            _currentColorBox.Location = new Point(gap, gap);
            _currentColorBox.BorderStyle = BorderStyle.FixedSingle;
            _currentColorBox.BackColor = Canvas._currentColor;


            _colorOptions = new ColorBoxesBox(size, gap, columns, ColorBoxesBox.DEFAULT_COLORS, OnColorOptionClicked);
            _colorOptions.Location = new Point(SizeOfBigBox + gap * 2, 0);
            Controls.Add(_colorOptions);
            Controls.Add(_currentColorBox);
        }

        private void OnColorOptionClicked(object sender, EventArgs e)
        {
            PictureBox p = sender as PictureBox;
            if (p == null) return;
            ChooseColor(p.BackColor);
        }

        private void ChooseColor(Color color)
        {
            Canvas._currentColor = color;
            _currentColorBox.BackColor = color;
            UpdateCurrentColorBox();
        }

        private void UpdateCurrentColorBox()
        {

        }

        private void UpdateSizes()
        {
            UpdateCanvasSize();
            Canvas?.FullRedraw();
        }

        private void UpdateCanvasSize()
        {
            if (Canvas == null || _colorOptions == null) return;
            int heightOffset = _colorOptions.Height;
            Canvas.Height = Height - heightOffset;
            Canvas.Width = Width;
            Canvas.Location = new Point(0, heightOffset);
        }

        private TextBox LineThicknessBox;
        private void GenerateLineSizeBox(int width, int height, int gap)
        {
            _lineSizesBox = new LineSizesBox<LineThickness>(width, height, gap);
            _lineSizesBox.AddButtonClickMethod(OnLineSizeButtonClicked);
            _lineSizesBox.Location = new Point(_colorOptions.Location.X + _colorOptions.Width + _boxesGap, 0);
            Controls.Add(_lineSizesBox);

            GenerateCustomeLineWidthBox();
        }

        private void GenerateCustomeLineWidthBox()
        {
            TextBox box = new TextBox();
            box.Text = Canvas.LineWidth.ToString();
            box.Width = 50;
            box.Location = new Point(_lineSizesBox.Location.X + _lineSizesBox.Width, _lineSizesBox.Height / 2 - box.Height / 2 - 3);
            box.TextChanged += (sender, evt) => {
                float output;
                if (float.TryParse(box.Text, out output))
                {
                    Canvas?.SetCustomLineSize(output);
                }
            };
            box.Enabled = false;
            LineThicknessBox = box;
            Controls.Add(box);
        }

        private void OnLineSizeButtonClicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            LineThickness thickness = (LineThickness)btn.Tag;
            LineThicknessBox.Enabled = false;
            if(thickness == LineThickness.Custom)
            {
                float newSize;
                if(float.TryParse(LineThicknessBox.Text, out newSize))
                {
                    Canvas.SetCustomLineSize(newSize);
                }
                LineThicknessBox.Enabled = true;
            }
            Canvas.SetLineSize(thickness);
        }

        internal void AddLineManager(LineManager lineManager)
        {
            throw new NotImplementedException();
        }
    }
}
