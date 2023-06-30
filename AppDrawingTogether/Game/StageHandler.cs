using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppDrawingTogether.Game
{
    public enum Stages
    {
        MainMenu,
        GameSelect,
        Game,
        Blank
    }
    internal class StageHandler : Dictionary<Stages, Stage>
    {
        private Stages _currentStage;
        public GroupBox ActiveStage => this[_currentStage];
        public StageHandler() {
            Add(Stages.Blank, new Stage());
            _currentStage = Stages.Blank;
            EnableStage();
        }
        /// <summary>
        /// Adds a stage to the stageHandler instance.
        /// provide a unique key and any groupbox object.
        /// </summary>
        /// <param name="stageEnum">Key</param>
        /// <param name="Stage"></param>
        public new void Add(Stages Key, Stage Stage)
        {
            base.Add(Key, Stage);
            SetCurrentStage();
        }
        /// <summary>
        /// Will show and enable current stage.
        /// Will hide and disable all other stages.
        /// </summary>
        public void SetCurrentStage() => SetStage(_currentStage);
        /// <summary>
        /// change active stage to chosen stage.
        /// will hide and disable all other stages, and show and enable the chosen stage.
        /// </summary>
        /// <param name="stage">stage to set to</param>
        public void SetStage(Stages stage)
        {
            DisableAllStages();
            EnableStage(stage);
            _currentStage = stage;
        }
        /// <summary>
        /// Enables and shows the current active stage
        /// </summary>
        /// <returns>if succeful or not</returns>
        public bool EnableStage() => EnableStage(_currentStage);
        /// <summary>
        /// Enables and shows specified stage
        /// </summary>
        /// <param name="stage">stage to be enabled</param>
        /// <returns>if succeful or not</returns>
        public bool EnableStage(Stages stage)
        {
            this[stage].Show();
            this[stage].Enabled = true;
            return true;
        }
        /// <summary>
        /// Hides and disables all stages in current instance.
        /// </summary>
        public void DisableAllStages()
        {
            foreach(KeyValuePair<Stages, Stage> item in this)
            {
                GroupBox box = item.Value;
                box.Enabled = false;
                box.Hide();
            }
        }
    }
}
