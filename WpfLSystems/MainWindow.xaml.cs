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
        LSystem lSystem = new LSystem("F+F+F+F");
        public MainWindow()
        {
            InitializeComponent();
        }

        public void CommonClickHandler(object sender, RoutedEventArgs e)
        {
            FrameworkElement feSource = e.Source as FrameworkElement;
            switch (feSource.Name)
            {
                case "Generate":
                    Canvas drawingCanvas = FindName("DrawingCanvas") as Canvas;
                    var lines = turtle.InterpretString(lSystem.Word);
                    drawingCanvas.Children.Clear();
                    foreach (Line line in lines)
                    {
                        drawingCanvas.Children.Add(line);
                    }
                    RecenterCanvasLines(drawingCanvas);
                    lSystem.GenerateNextWord();
                    break;
                case "NoButton":
                    // do something ...
                    break;
                case "CancelButton":
                    // do something ...
                    break;
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


