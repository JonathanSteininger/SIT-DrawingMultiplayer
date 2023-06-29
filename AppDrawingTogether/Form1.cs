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
            Size = Screen.PrimaryScreen.Bounds.Size;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (Manager == null) return;
            Manager.Size = Size;
        }

        GameManager Manager { get; set; }
        private void Form1_Load(object sender, EventArgs e)
        {
            Manager = new GameManager(Size, new Point(0,0), "jono");
            Controls.Add(Manager);
        }
    }
}
