using AppDrawingTogether.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppDrawingTogether
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Resize += Form1_Resize;
        }
        GameManager Manager { get; set; }
        private void Form1_Load(object sender, EventArgs e)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = Size;
            pictureBox.Location = new Point(0,0);
            Controls.Add(pictureBox);
            Manager = new GameManager(pictureBox, 30, "jono");
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (Manager == null) return;
            Manager.Canvas.Size = Size;
        }
    }
}
