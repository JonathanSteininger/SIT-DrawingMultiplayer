using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace AppDrawingTogether.Game
{
    internal class Stage : GroupBox
    {
        private new MyControlManager Controls = new MyControlManager();
        public Stage(Size size)
        {
            Size = size;
            Location = new Point(0, 0);
        }

        public delegate void ShownEvent(object sender, EventArgs e);

        public ShownEvent Shown = (sender, evt) => { };
        public new void Show()
        {
            Shown.Invoke(this, EventArgs.Empty);
            base.Show();
        }

        /// <summary>
        /// Adds a control to the controller and controller dictionary
        /// </summary>
        /// <param name="control">controll being added</param>
        public void AddControl(Control control)
        {
            Controls.Add(control.Name, control);
            base.Controls.Add(control);
        }
        /// <summary>
        /// Removes control from the stage controler.
        /// removes control from the controls dictionary
        /// </summary>
        /// <param name="control">control being removed</param>
        public void RemoveControl(string key)
        {
            if (!Contains(key)) return;
            RemoveControl(this[key]);
        }
        public void RemoveControl(Control control)
        {
            if (!Contains(control)) return;
            Controls.Remove(control.Name);
            base.Controls.Remove(control);
        }

        /// <summary>
        /// Checks if The controll passed is contained in the dictionary.
        /// </summary>
        /// <param name="control">control being checked for</param>
        /// <returns>true if found. false otherwise.</returns>
        public new bool Contains(Control control) => Contains(control.Name);
        /// <summary>
        /// Checks if the controls contains that key
        /// </summary>
        /// <param name="Key">key</param>
        /// <returns>true if it contains the key. false otherwise</returns>
        public bool Contains(string Key)
        {
            return Controls.ContainsKey(Key);
        }

        /// <summary>
        /// returns the control based off the key which is the controls name.
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>contorl with that key</returns>
        public Control this[string key] => GetControl(key);
        /// <summary>
        /// Returns the control based off the key which is the controls name.
        /// </summary>
        /// <param name="name">Key</param>
        /// <returns>Control with that key</returns>
        public Control GetControl(string name)
        {
            return Controls[name];
        }

        public delegate void ControlAction(Control control);
        /// <summary>
        /// Loops through every control. can add a custom method to be looped through
        /// </summary>
        /// <param name="action">Method used for each control</param>
        public void ForEachControl(ControlAction action)
        {
            foreach(KeyValuePair<string, Control> pair in Controls)
            {
                action.Invoke(pair.Value);
            }
        }
        /// <summary>
        /// Used to store all controls stored in this class
        /// </summary>
        internal class MyControlManager : Dictionary<string, Control>
        {
            public MyControlManager() : base() { }
            /// <summary>
            /// Uses the Controls Name as a key
            /// </summary>
            /// <param name="control">control to be added</param>
            public void Add(Control control)
            {
                base.Add(control.Name, control);
            }
        }
    }
}
