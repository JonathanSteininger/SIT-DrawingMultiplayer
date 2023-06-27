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
    internal class GameManager : GroupBox
    {
        public MyCanvas Canvas { get; set; }

        private ColorBoxesBox _colorOptions;

        public string PlayerName { get { return Canvas.PlayerName; } set { Canvas.PlayerName = value; } }
        public new Size Size { get { return base.Size; } set { base.Size = value; UpdateSizes(); } }


        public static float DEFAULT_FRAMERATE = 30f;

        public GameManager(Size size, Point location, string playerName)
        {
            Size = size;
            Location = location;

            Canvas = new MyCanvas(DEFAULT_FRAMERATE);
            PlayerName = playerName;
            Canvas.BorderStyle = BorderStyle.FixedSingle;

            Controls.Add(Canvas);

            AddColorBoxes();

            UpdateSizes();

            Canvas.Start();
        }

        private PictureBox _currentColorBox;
        private void AddColorBoxes()
        {
            int gap = 5;
            int size = 20;
            int columns = 10;

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

        }

        private void UpdateCanvasSize()
        {
            if (Canvas == null || _colorOptions == null) return;
            int heightOffset = _colorOptions.Height;
            Canvas.Height = Height - heightOffset;
            Canvas.Width = Width;
            Canvas.Location = new Point(0, heightOffset);
        }

        


    }
}
