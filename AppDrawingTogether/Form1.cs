using AppDrawingTogether.Game;
using AppDrawingTogether.Network.Client;
using AppDrawingTogether.Network.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace AppDrawingTogether
{
    public partial class Form1 : Form
    {

        private static string FILE_DIRECTORY = "./Saved";

        private StageHandler _stageHandler;
        public Form1()
        {
            InitializeComponent();
            Resize += Form1_Resize;
            FormClosed += Form1_FormClosed;
            Size = Screen.PrimaryScreen.Bounds.Size;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            server?.Stop();

            ConnectionToServer?.Stop();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (_stageHandler != null)
            {
                _stageHandler.ForEachStage(stage => stage.Size = Size);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateStages();
            _stageHandler.SetStage(Stages.MainMenu);
        }

        private void CreateStages()
        {
            _stageHandler = new StageHandler();
            _stageHandler.Add(Stages.MainMenu, CreateMenuStage());
            _stageHandler.Add(Stages.GameSelectSinglePlayer, CreateSelectionStageSingle());
            _stageHandler.Add(Stages.GameSelectMultiPlayer, CreateSelectionStageMulti());
            _stageHandler.Add(Stages.Game, CreateGameStage());
        }

        private Stage CreateGameStage()
        {
            Stage stage = new Stage(Size);
            Controls.Add(stage);

            Button ExitButton = new Button();
            ExitButton.Click += ExitButtonGameClicked;
            ExitButton.Click += (sender, e) =>
            {
                server?.Stop();

                ConnectionToServer?.Stop();
            };
            ExitButton.Name = "ExitButton";
            ExitButton.Text = "Exit";
            stage.AddControl(ExitButton);


            Label lable = new Label();
            lable.Text = "port num";
            lable.Name = "PortLabel";
            lable.Location = new Point(200, 25);
            stage.AddControl(lable);

            stage.Shown += (s, e) => lable.Text = $"Port Num: {(_stageHandler[Stages.GameSelectMultiPlayer]["PortTextBox"] as TextBox).Text}";

            //GameManager manager = CreateGameManager();
            //stage.AddControl(manager);
            stage.Resize += (sender, evt) =>
            {
                if (stage.Contains("Manager"))
                {
                    stage["Manager"].Size = new Size(Width, Height - 100);
                }
            };

            return stage;
        }
        private Stage CreateSelectionStageSingle()
        {
            Stage stage = new Stage(Size);
            Controls.Add(stage);

            Button ExitButton = new Button();
            ExitButton.Click += ExitButtonGameClicked;
            ExitButton.Location = new Point(0, 0);
            ExitButton.Name = "ExitButton";
            ExitButton.Text = "Exit";
            stage.AddControl(ExitButton);

            Button newButton = new Button();
            newButton.Click += CreateSinglePlayerGameButtonClicked;
            newButton.Location = new Point(250, 0);
            newButton.Name = "NewButton";
            newButton.Width = 200;
            newButton.Height = 50;
            newButton.Text = "New SinglePlayer Game";
            stage.AddControl(newButton);

            CanvasList list = new CanvasList(FILE_DIRECTORY);
            list.Name = "CanvasList";
            list.Location = new Point(0, 100);
            list.Size = new Size(Width, Height - 100);
            stage.Shown += (sender, evet) => list.UpdateList();
            stage.Resize += (sender, evet) =>
            {
                list.Width = Width;
                list.Height = Height - 100;
            };
            stage.AddControl(list);

            return stage;
        }
        private static Random random = new Random();
        private Stage CreateSelectionStageMulti()
        {
            Stage stage = new Stage(Size);
            Controls.Add(stage);

            Button ExitButton = new Button();
            ExitButton.Click += ExitButtonGameClicked;
            ExitButton.Location = new Point(0, 0);
            ExitButton.Name = "ExitButton";
            ExitButton.Text = "Exit";
            stage.AddControl(ExitButton);

            Button newButton = new Button();
            newButton.Click += CreateMultiPlayerGameButtonClicked;
            newButton.Location = new Point(200, 0);
            newButton.Width = 200;
            newButton.Height = 50;
            newButton.Name = "NewButton";
            newButton.Text = "New Multiplayer Game";
            stage.AddControl(newButton);

            TextBox portBox = new TextBox();
            portBox.Location = new Point(400, 25);
            portBox.Width = 100;
            portBox.Name = "PortTextBox";
            portBox.Text = (2000 + random.Next(500)).ToString();
            stage.AddControl(portBox);

            Label portLable = new Label();
            portLable.Width = 100;
            portLable.Name = "PortLabel";
            portLable.Text = "Port Number:";
            portLable.Location = new Point(400, 0);
            stage.AddControl(portLable);

            Button joinButton = new Button();
            joinButton.Click += JoinMultiPlayerGameButtonClicked;
            joinButton.Location = new Point(500, 0);
            joinButton.Width = 200;
            joinButton.Height = 50;
            joinButton.Name = "JoinButton";
            joinButton.Text = "Join Multiplayer Game";
            stage.AddControl(joinButton);


            CanvasList list = new CanvasList(FILE_DIRECTORY);
            list.Name = "CanvasList";
            list.Location = new Point(0, 100);
            list.Size = new Size(Width, Height - 100);
            stage.Shown += (sender, evet) => list.UpdateList();
            stage.Resize += (sender, evet) =>
            {
                list.Width = Width;
                list.Height = Height - 100;
            };
            stage.AddControl(list);



            return stage;
        }

        

        private Stage CreateMenuStage()
        {
            Stage stage = new Stage(Size);
            Controls.Add(stage);

            Button ExitButton = new Button();
            ExitButton.Click += (sender, evt) => Close();
            ExitButton.Location = new Point(0, 0);
            ExitButton.Name = "ExitButton";
            ExitButton.Text = "Exit App";
            stage.AddControl(ExitButton);

            Button button = new Button();
            button.Name = "SinglePlayerButton";
            button.Location = new Point(100, 100);
            button.Size = new Size(100, 60);
            button.Click += SinglePlayerClicked;
            button.Text = "Singleplayer";
            stage.AddControl(button);

            Button button2 = new Button();
            button2.Name = "MultiplayerButton";
            button2.Location = new Point(200, 100);
            button2.Size = new Size(100, 60);
            button2.Click += MultiplayerPlayerClicked;
            button2.Text = "Multiplayer";
            stage.AddControl(button2);

            return stage;
        }


        private GameManager CreateGameManager()
        {
            GameManager manager = new GameManager(new Size(Width, Height - 100), new Point(0, 100), "Jono");
            manager.Name = "Manager";
            return manager;
        }

        private GameManager CreateGame()
        {
            _stageHandler[Stages.Game].RemoveControl("Manager");
            GameManager game = CreateGameManager();
            _stageHandler[Stages.Game].AddControl(game);
            return game;
        }


        private void ExitButtonGameClicked(object sender, EventArgs e)
        {
            Stage st = _stageHandler[Stages.Game];
            if (st.Contains("Manager"))
            {
                (st["Manager"] as GameManager)?.StopGame();
            }
            _stageHandler.SetStage(Stages.MainMenu);
        }


        //Events
        private void JoinMultiPlayerGameButtonClicked(object sender, EventArgs e)
        {
            TextBox box = _stageHandler[Stages.GameSelectMultiPlayer]["PortTextBox"] as TextBox;
            int output;
            if (int.TryParse(box.Text, out output))
            {
                JoinServer(output);
            }
            else
            {
                MessageBox.Show($"Port number must be a valid whole number.\nEntered number:\"{box.Text}\"", "Invalid Port Number", MessageBoxButtons.OK);
            }
        }

        private void JoinServer(int port)
        {
            server?.Stop();
            ConnectionToServer?.Stop();
            try
            {
                    GameManager game = CreateGame();
                    game.Canvas.Server = false;
                    ConnectionToServer = new Connection(port, game.LineManager, "jono", 5);
                    _stageHandler.SetStage(Stages.Game);
                    
            }
            catch (Exception e)
            {
                _stageHandler.SetStage(Stages.GameSelectMultiPlayer);
                ConnectionToServer?.Stop();
            }
        }
        private Connection ConnectionToServer;

        private void CreateMultiPlayerGameButtonClicked(object sender, EventArgs e)
        {
            TextBox box = _stageHandler[Stages.GameSelectMultiPlayer]["PortTextBox"] as TextBox;
            int output;
            if (int.TryParse(box.Text, out output))
            {
                StartServer(output);
            }
            else
            {
                MessageBox.Show($"Port number must be a valid whole number.\nEntered number:\"{box.Text}\"", "Invalid Port Number", MessageBoxButtons.OK);
            }
        }

        private void StartServer(int port)
        {
            server?.Stop();

            ConnectionToServer?.Stop();
            GameManager game = CreateGame();
            server = new ServerManager(port,game.LineManager);

            _stageHandler.SetStage(Stages.Game);
        }

        private ServerManager server;

        private void CreateSinglePlayerGameButtonClicked(object sender, EventArgs e)
        {
            _stageHandler.SetStage(Stages.Game);
            CreateGame();
        }

        private void MultiplayerPlayerClicked(object sender, EventArgs e)
        {
            _stageHandler.SetStage(Stages.GameSelectMultiPlayer);
        }

        private void SinglePlayerClicked(object sender, EventArgs e)
        {
            _stageHandler.SetStage(Stages.GameSelectSinglePlayer);
        }
    }
}
