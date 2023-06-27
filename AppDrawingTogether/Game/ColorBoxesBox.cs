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
        public ColorBoxesBox() : this(8,2,6,2,DEFAULT_COLORS, null) { }
        public ColorBoxesBox(int colorSize, int gap, int Columns, int rows, Color[] Colors, EventHandler OnClickEvent)
        {
            Height = colorSize * rows + gap * (rows + 1);
            Width = (colorSize + gap ) * (Columns / rows) + gap;
            _rows = rows;
            _columns = Columns;
            _colorBoxesSize = new Size(colorSize, colorSize);
            _gap = gap;
            GenerateBoxes(OnClickEvent);
        }
        private int _rows, _columns;
        private Size _colorBoxesSize;
        private int _gap;
        private void GenerateBoxes(EventHandler onClickEvent)
        {
            for(int i = 0; i< _colors.Length; i++)
            {
                int row = i / (_columns);
                int col = i % (_columns);
                if (row > _rows) return;
                PictureBox box = new PictureBox();
                box.BackColor = _colors[i];
                box.BorderStyle = BorderStyle.FixedSingle;
                box.Click += onClickEvent;
                box.Size = _colorBoxesSize;
                box.Location = new Point(col * (box.Width + _gap) + _gap, row * (box.Height + _gap) + _gap);
                Controls.Add(box);
            }
        }
    }
}
