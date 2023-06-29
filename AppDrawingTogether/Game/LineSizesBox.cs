using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppDrawingTogether.Game
{
    internal class LineSizesBox<T> : GroupBox where T : struct, Enum
    {
        public static int DEFAULT_BUTTON_WIDTH = 50;
        public static int DEFAULT_BUTTON_HEIGHT = 30;
        public static int DEFAULT_GAP = 3;

        private int _buttonWidth;
        private int _buttonHeight;
        private int _gap;

        public int ButtonWidth { get { return _buttonWidth; } }
        public int ButtonHeight { get { return _buttonHeight; } }
        public int Gap { get { return _gap; } }



        private int _enumAmount = Enum.GetNames(typeof(T)).Length;
        public int EnumAmount { get { return _enumAmount; } }

        public List<Button> Buttons { get; set; }

        public LineSizesBox() : this(DEFAULT_BUTTON_WIDTH, DEFAULT_BUTTON_HEIGHT, DEFAULT_GAP) { }
        public LineSizesBox(int ButtonWidth, int ButtonHeight, int Gap)
        {
            _buttonWidth = ButtonWidth;
            _buttonHeight = ButtonHeight;
            _gap = Gap;

            SetSize();
            GenerateButtons();
        }

        public void AddButtonClickMethod(EventHandler OnClickMethod)
        {
            foreach (Button button in Buttons) button.Click += OnClickMethod;
        }

        private void SetSize()
        {
            Width = (_gap + _buttonWidth) * _enumAmount + _gap;
            Height = _gap + _buttonHeight + _gap;
        }

        string[] _names = Enum.GetNames(typeof(T));

        private void GenerateButtons()
        {
            Buttons = new List<Button>();
            for(int i = 0; i < EnumAmount; i++)
            {
                T value = (T)Enum.Parse(typeof(T), _names[i]);

                Button button = new Button();
                button.Text = _names[i];
                button.Tag = value;

                button.Size = new Size(_buttonWidth, _buttonHeight);
                button.Location = new Point((_gap + _buttonWidth) * i, _gap);

                Controls.Add(button);
                Buttons.Add(button);
            }
        }
    }
}
