using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace game_of_life {
    public partial class MainWindow : Window {
        private DispatcherTimer timer;
        private BigBang.InitializationParams bbInitParams;
        private BigBang bb;
        private World world;
        private CellGeneration cellGen;
        private bool initialized; // necessary?
        private bool drawing;
        private int drawSize;
        private int delay; // TODO: make it variable at runtime

        private void InitSimulationObjects(BigBang.InitializationMode initMode) {
            bb = new BigBang(initMode);
            world = bb.GetNewWorld();
            cellGen = world.GetCells();
            delay = 80;
        }

        private void InitSimulationObjects(
            BigBang.InitializationParams initParams,
            BigBang.InitializationMode initMode
        ) {
            bb = new BigBang(initParams, initMode);
            world = bb.GetNewWorld();
            cellGen = world.GetCells();
            delay = 50;
        }

        private void InitCanvas() {
            canvas.Children.Clear();

            for(int i = 0; i < world.GetSize(); i++) {
                for(int j = 0; j < world.GetSize(); j++) {
                    world.GetField(i, j).Width = canvas.ActualWidth / world.GetSize();
                    world.GetField(i, j).Height = canvas.ActualHeight / world.GetSize();
                    canvas.Children.Add(world.GetField(i, j));
                    Canvas.SetTop(world.GetField(i, j), world.GetField(i, j).Height * i); // TODO: margin?
                    Canvas.SetLeft(world.GetField(i, j), world.GetField(i, j).Width * j);
                }
            }

            initialized = true;
            Render();
        }

        /*
        private void ClearCanvas() {
            for(int i = 0; i < world.GetWidth(); i++) {
                for(int j = 0; j < world.GetHeight(); j++) {
                    world.GetField(i, j).Fill = Brushes.White;
                }
            }
        }
        */

        private void Render() {
            for(int i = 0; i < world.GetSize(); i++) {
                for(int j = 0; j < world.GetSize(); j++) {
                    if(cellGen.GetCell(i, j).IsDead()) {
                        world.GetField(i, j).Fill = Brushes.White;
                    } else if(cellGen.GetCell(i, j).GetNumberOfSiblings() > 2) {
                        world.GetField(i, j).Fill = Brushes.DarkCyan;
                    } else {
                        world.GetField(i, j).Fill = Brushes.Blue;
                    }
                }
            }
        }

        private void Draw(MouseEventArgs e) {
            if(drawing) {
                List<Point> coords = new List<Point>();

                for(
                    double i = e.GetPosition(canvas).X - drawSize / 2.0;
                    i <= e.GetPosition(canvas).X + drawSize / 2.0;
                    i++
                ) {
                    for(
                        double j = e.GetPosition(canvas).Y - drawSize / 2.0;
                        j <= e.GetPosition(canvas).Y + drawSize / 2.0;
                        j++
                    ) {
                        coords.Add(new Point(
                            j / (canvas.ActualHeight / world.GetSize()),
                            i / (canvas.ActualWidth / world.GetSize())
                        ));
                    }
                }

                cellGen.ReviveCellsAtCoords(coords);
                Render();
            }
        }

        private void ButtonStartClick(object sender, RoutedEventArgs e) {
            drawing = false;
            timer.Start();
        }

        private void ButtonStopClick(object sender, RoutedEventArgs e) {
            timer.Stop();
        }

        private void ButtonNewClick(object sender, RoutedEventArgs e) {
            bbInitParams.worldSize = 150; // TODO: make it changeable through GUI
            bbInitParams.cellSpreadModifier = sliderCellSpread.Value;
            drawing = false;
            timer.Stop();
            InitSimulationObjects(bbInitParams, BigBang.InitializationMode.STANDARD);
            InitCanvas();
        }

        private void ButtonDrawClick(object sender, RoutedEventArgs e) {
            drawSize = Convert.ToInt32(drawSizeInput.Text);
            drawing = true;
            timer.Stop();
            InitSimulationObjects(BigBang.InitializationMode.CLEAR);
            InitCanvas();
        }

        private void DrawSizeInputTextChanged(object sender, TextChangedEventArgs e) {
            int oldValue = drawSize;
            TextBox textBox = sender as TextBox;

            if(textBox != null) {
                try {
                    drawSize = Convert.ToInt32(textBox.Text);
                } catch(FormatException) {
                    drawSizeInput.Text = Convert.ToString(oldValue);
                    drawSize = Convert.ToInt32(drawSizeInput.Text);
                }
            }
        }

        private void MouseMove(object sender, MouseEventArgs e) {
            if(e.LeftButton == MouseButtonState.Pressed) {
                Draw(e);
            }
        }

        private void MouseDown(object sender, MouseEventArgs e) {
            Draw(e);
        }

        private void MouseEnter(object sender, MouseEventArgs e) {
            if(drawing) {
                canvas.Cursor = Cursors.Cross;
            }
        }

        private void MouseLeave(object sender, MouseEventArgs e) {
            canvas.Cursor = Cursors.Arrow;
        }

        private void TimerTick(object sender, EventArgs e) {
            if(initialized) {
                cellGen.Evolve();
                Render();
            }
        }

        public MainWindow() {
            bbInitParams = new BigBang.InitializationParams();

            InitializeComponent();

            initialized = false;
            drawing = false;
            drawSizeInput.Text = Convert.ToString(1);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(delay);
            timer.Tick += TimerTick;
        }
    }
}