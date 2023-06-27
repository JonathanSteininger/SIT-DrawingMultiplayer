using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace AppDrawingTogether.Game
{
    internal class ColorBoxesBox : GroupBox
    {
        private Color[] _colors;

        public static int DEFAULT_COLOR_SIZE = 8;
        public static int DEFAULT_GAP = 2;
        public static int DEFAULT_COLUMN_AMOUNT = 10;
        

        public static Color[] DEFAULT_COLORS = new Color[] {
            Color.Black,
            Color.White,
            Color.Gray,
            Color.Red,
            Color.Orange,
            Color.Yellow,
            Color.Green,
            Color.LightBlue,
            Color.Blue,
            Color.Purple,
            Color.Pink,
            Color.Violet
        };
        public ColorBoxesBox(EventHandler OnClickEvent) : this(OnClickEvent, DEFAULT_COLORS) { }
        public ColorBoxesBox(EventHandler OnClickEvent, Color[] Colors) : this(DEFAULT_COLOR_SIZE, DEFAULT_GAP, DEFAULT_COLUMN_AMOUNT, Colors, OnClickEvent) { }
        public ColorBoxesBox() : this(null) { }
        public ColorBoxesBox(int colorSize, int gap, int Columns, Color[] Colors, EventHandler OnClickEvent)
        {
            _colors = Colors;
            Height = colorSize * (_colors.Length / Columns + 1) + gap * ((_colors.Length / Columns + 1) + 1);
            Width = (colorSize + gap ) * Columns + gap;
            _columns = Columns;
            _colorBoxesSize = new Size(colorSize, colorSize);
            _gap = gap;
            GenerateBoxes(OnClickEvent);
        }
        private int _columns;
        private Size _colorBoxesSize;
        private int _gap;
        private void GenerateBoxes(EventHandler onClickEvent)
        {
            for(int i = 0; i< _colors.Length; i++)
            {
                int row = i / _columns;
                int col = i % _columns;
                PictureBox box = new PictureBox();
                box.BackColor = _colors[i];
                box.BorderStyle = BorderStyle.FixedSingle;
                if (onClickEvent != null) box.Click += onClickEvent;
                box.Size = _colorBoxesSize;
                box.Location = new Point(col * (box.Width + _gap) + _gap, row * (box.Height + _gap) + _gap);
                Controls.Add(box);
            }
        }
    }
}
