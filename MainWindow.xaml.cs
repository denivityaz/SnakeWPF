using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace snake_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private int clickCount = 0;
        private DispatcherTimer doubleClickTimer;

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickCount++;
            if (clickCount == 1)
            {
                doubleClickTimer.Start();
            }
        }

        private void DoubleClickTimer_Tick(object sender, EventArgs e)
        {
            if (clickCount == 2)
            {
                Close();
            }

            clickCount = 0;
            doubleClickTimer.Stop();
        }


        private readonly Dictionary<GridValue, ImageSource> gridValToImage = new()
        {
            { GridValue.Empty, Images.Empty },
            { GridValue.Snake, Images.Body },
            { GridValue.Food, Images.Food }
        };

        // Наработки для динамической замены цветов
        //private readonly Dictionary<GridValue, ImageSource> gridValToImage = new()
        //{
        //    { GridValue.Empty, Images.Empty },
        //    { GridValue.Snake, (Images.Food == Images.LoadImage("FoodGreen.png")) ? Images.SnakeBodyGreen : Images.SnakeBodyOrange },
        //    { GridValue.FoodOrange, Images.FoodOrange },
        //    { GridValue.FoodGreen, Images.FoodGreen }
        //};



        private readonly Dictionary<Direction, int> dirToRotation = new()
        {
            { Direction.Up, 0 },
            { Direction.Right, 90 },
            { Direction.Down, 180 },
            { Direction.Left, 270 }
        };

        private int rows = 20, cols = 20;

        private void ChangeGrid(int rows, int cols)
        {

        }
        private readonly Image[,] gridImages;
        private GameState gameState;
        public bool gameRunning;
        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new GameState(rows, cols);
            doubleClickTimer = new DispatcherTimer();
            doubleClickTimer.Interval = TimeSpan.FromMilliseconds(250);
            doubleClickTimer.Tick += DoubleClickTimer_Tick;
        }

        private async Task Rungame()
        {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await ShowGameOver();
            gameState = new GameState(rows, cols);
        }
        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }

            if (!gameRunning)
            {
                gameRunning = true;
                await Rungame();
                gameRunning = false;
            }
        }
        private async Task ShowCountDown()
        {
            for (int i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }
        }
        private async Task ShowGameOver()
        {
            await DrawDead();
            await Task.Delay(1000);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "PRESS ANY KEY TO START" + Environment.NewLine + Environment.NewLine + "DOUBLE TAP TO EXIT";
            OverlayText.TextAlignment = TextAlignment.Center;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    gameState.ChangeDirection(Direction.Left);
                    break;
                case Key.Right:
                    gameState.ChangeDirection(Direction.Right);
                    break;
                case Key.Up:
                    gameState.ChangeDirection(Direction.Up);
                    break;
                case Key.Down:
                    gameState.ChangeDirection(Direction.Down);
                    break;
            }
        }

        private async Task GameLoop()
        {
            while (!gameState.GameOver)
            {
                await Task.Delay(75);
                gameState.Move();
                Draw();
            }
        }

        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;
            GameGrid.Width = GameGrid.Height * (cols / (double)rows);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    };

                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }

            return images;
        }

        private void Draw()
        {
            DrawGrid();
            DrawSnakeHead();
            ScoreText.Text = $"SCORE {gameState.Score}";
        }

        // Наработки для динамической замены цветов
        //private void DrawGrid()
        //{
        //    for (int r = 0; r < rows; r++)
        //    {
        //        for (int c = 0; c < cols; c++)
        //        {
        //            GridValue gridVal = gameState.Grid[r, c];

        //            // Устанавливаем правильные значения картинок для змеи в зависимости от ее цвета
        //            if (gridVal == GridValue.SnakeOrange)
        //            {
        //                gridImages[r, c].Source = Images.SnakeBodyOrange;
        //            }
        //            else if (gridVal == GridValue.SnakeGreen)
        //            {
        //                gridImages[r, c].Source = Images.SnakeBodyGreen;
        //            }
        //            else
        //            {
        //                gridImages[r, c].Source = gridValToImage[gridVal];
        //            }

        //            gridImages[r, c].RenderTransform = Transform.Identity;
        //        }
        //    }
        //}

        private void DrawGrid()
        {

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    GridValue gridVal = gameState.Grid[r, c];
                    gridImages[r, c].Source = gridValToImage[gridVal];
                    gridImages[r, c].RenderTransform = Transform.Identity;
                }
            }
        }

        // Динамическая замены цветов
        //private void DrawSnakeHead()
        //{
        //    Position headPos = gameState.HeadPosition();
        //    Image image = gridImages[headPos.Row, headPos.Col];

        //    // Устанавливаем правильные значения картинок для головы змеи в зависимости от цвета еды
        //    if (gameState.Grid[headPos.Row, headPos.Col] == GridValue.SnakeOrange)
        //    {
        //        image.Source = Images.SnakeHeadOrange;
        //    }
        //    else if (gameState.Grid[headPos.Row, headPos.Col] == GridValue.SnakeGreen)
        //    {
        //        image.Source = Images.SnakeHeadGreen;
        //    }
        //    else
        //    {
        //        image.Source = Images.Head;
        //    }

        //    int rotation = dirToRotation[gameState.Dir];
        //    image.RenderTransform = new RotateTransform(rotation);
        //}

        private void DrawSnakeHead()
        {
            Position headPos = gameState.HeadPosition();
            Image image = gridImages[headPos.Row, headPos.Col];
            image.Source = Images.Head;

            int rotation = dirToRotation[gameState.Dir];
            image.RenderTransform = new RotateTransform(rotation);
        }


        private async Task DrawDead()
        {
            List<Position> positions = new List<Position>(gameState.SnakePositions());
            for (int i = 0; i < positions.Count; i++)
            {
                Position pos = positions[i];
                ImageSource source = (i == 0) ? Images.DeadHead : Images.DeadBody;
                gridImages[pos.Row, pos.Col].Source = source;
                await Task.Delay(50);
            }
        }
    }
}

