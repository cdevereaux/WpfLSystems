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

namespace WpfLSystems
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Turtle turtle = new Turtle();
        LSystem lSystem = new LSystem();
        uint n = 5;
        public MainWindow()
        {
            InitializeComponent();
            InitializeProductionsPanel();
            InitializeConstantsPanel();
        }

        private void AddAxiomInput()
        {
            StackPanel stackPanel = ProductionsPanel;
            var dockPanel = new DockPanel();
            stackPanel.Children.Add(dockPanel);

            var label = new Label();
            label.Content = "Axiom:";
            dockPanel.Children.Add(label);

            var textBox = new TextBox();
            textBox.Name = "Axiom";
            textBox.Text = lSystem.Axiom;
            dockPanel.Children.Add(textBox);
        }

        private void AddProductionInput(int i)
        {
            StackPanel stackPanel = ProductionsPanel;
            var (predecessor, successor) = lSystem.productions[i];

            var wrapPanel = new WrapPanel();
            stackPanel.Children.Add(wrapPanel);

            var pred = new TextBox();
            pred.Name = "Predecessor" + i;
            pred.Text = predecessor.ToString();
            wrapPanel.Children.Add(pred);

            var label = new Label();
            label.Content = "->";
            wrapPanel.Children.Add(label);

            var succ = new TextBox();
            succ.Name = "Successor" + i;
            succ.Text = successor;
            wrapPanel.Children.Add(succ);
        }

        private void AddProductionInputs()
        {
            for (var i = 0; i < lSystem.productions.Count; i++)
            {
                AddProductionInput(i);
            }
        }
        private void InitializeProductionsPanel()
        {
            AddAxiomInput();
            AddProductionInputs();
        }

        private void InitializeConstantsPanel()
        {
            StackPanel stackPanel = ConstantsPanel;
            var constants = new[] {
                ("Angle Increment:", "Angle", turtle.drawState.angle_increment),
                ("Line Length:", "LineLength", turtle.drawState.line_length),
                ("Iterations:", "Iterations", n)
            };

            foreach (var (labelText, Name, Value) in constants)
            {
                var dockPanel = new DockPanel();
                stackPanel.Children.Add(dockPanel);

                var label = new Label();
                label.Content = labelText;
                dockPanel.Children.Add(label);
                
                var textBox = new TextBox();
                textBox.Name = Name;
                textBox.Text = Value.ToString();
                dockPanel.Children.Add(textBox);
            }
        }

        public void CommonClickHandler(object sender, RoutedEventArgs e)
        {
            FrameworkElement feSource = e.Source as FrameworkElement;
            switch (feSource.Name)
            {
                case "Generate":
                    Canvas drawingCanvas = DrawingCanvas;
                    var lines = turtle.InterpretString(lSystem.GenerateIteration(n));
                    drawingCanvas.Children.Clear();
                    foreach (Line line in lines)
                    {
                        drawingCanvas.Children.Add(line);
                    }
                    RecenterCanvasLines(drawingCanvas);
                    break;
                case "AddProduction":
                    var i = lSystem.productions.Count;
                    lSystem.AddProduction(' ', "");
                    AddProductionInput(i);
                    break;
                default:
                    break;
            }
            e.Handled = true;
        }


        private void TryUpdateDoubleConstant(TextBox textBox, ref double constant)
        {
            double temp;
            if (double.TryParse(textBox.Text, out temp))
            {
                constant = temp;
            }
            else
            {
                textBox.Text = constant.ToString();
            }
        }

        private void TryUpdatePredecessor(TextBox textBox)
        {
            char temp;
            var production = lSystem.productions[textBox.Name.Last() - '0'];
            if (char.TryParse(textBox.Text, out temp))
            {
                
                production.Item1 = temp;
            }
            else
            {
                textBox.Text = production.Item1.ToString();
            }
        }

        private void UpdateSuccessor(TextBox textBox)
        {
            var production = lSystem.productions[textBox.Name.Last() - '0'];
            production.Item2 = textBox.Text;
        }

        public void CommonInputHandler(object sender, RoutedEventArgs e) {
            TextBox textBox = e.Source as TextBox;
            switch (textBox.Name)
            {
                case "Angle":            
                    TryUpdateDoubleConstant(textBox, ref turtle.drawState.angle_increment);
                    break;
                case "LineLength":
                    TryUpdateDoubleConstant(textBox, ref turtle.drawState.line_length);
                    break;
                case "Iterations":
                    uint temp1;
                    if (uint.TryParse(textBox.Text, out temp1))
                    {
                        n = temp1;
                    }
                    else
                    {
                        textBox.Text = n.ToString();
                    }
                    break;
                case "Axiom":
                    lSystem.Axiom = textBox.Text;
                    break;
                case string x when x.StartsWith("Predecessor"):
                    TryUpdatePredecessor(textBox);
                    break;
                case string x when x.StartsWith("Successor"):
                    UpdateSuccessor(textBox);
                    break;
                default:
                    return;
            }
            e.Handled = true;
        }
        public void ResizeCanvasLines(object sender, SizeChangedEventArgs e)
        {
            if (e.Source is Canvas)
            {
                var drawingCanvas = e.Source as Canvas;
                RecenterCanvasLines(drawingCanvas);
            }
        }

        public void RecenterCanvasLines(Canvas canvas)
        {
            var lines = from child in canvas.Children.Cast<UIElement>()
                        where child is Line
                        select child as Line;
            if (lines.Count() == 0)
            {
                return;
            }
            var minX = Math.Min(lines.Min(line => line.X1), lines.Min(line => line.X2));
            var maxX = Math.Max(lines.Max(line => line.X1), lines.Max(line => line.X2));

            var minY = Math.Min(lines.Min(line => line.Y1), lines.Min(line => line.Y2));
            var maxY = Math.Max(lines.Max(line => line.Y1), lines.Max(line => line.Y2));

            var (oldX, oldY) = ((maxX + minX) / 2, (maxY + minY) / 2);
            var (newX, newY) = (canvas.ActualWidth / 2, canvas.ActualHeight / 2);
            var (dX, dY) = (newX - oldX, newY - oldY);

            foreach (Line line in lines)
            {
                line.X1 += dX;
                line.X2 += dX;
                line.Y1 += dY;
                line.Y2 += dY;
            }
        }
    }
}


