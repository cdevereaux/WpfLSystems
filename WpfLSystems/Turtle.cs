using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace WpfLSystems
{
    class Turtle
    {
        private class DrawState
        {
            public double line_length = 20;
            public double line_width = 2;
            public double turning_angle = Math.PI / 2;
            
            public double x = 0;
            public double y = 0;
            public double theta = 0;
        }
        
        List<Line> lines = new List<Line>();
        Stack<DrawState> stack = new Stack<DrawState>();
        DrawState drawState = new DrawState();

        private void MoveForward()
        {
            drawState.x += drawState.line_length * Math.Cos(drawState.theta);
            drawState.y += drawState.line_length * Math.Sin(drawState.theta);
        }

        private void DrawLine()
        {
            Line line = new Line();
            
            line.X1 = drawState.x;
            line.Y1 = drawState.y;
            
            MoveForward();
            line.X2 = drawState.x;
            line.Y2 = drawState.y;
            
            line.StrokeThickness = drawState.line_width;
            line.Stroke = System.Windows.Media.Brushes.Black;
            lines.Add(line);
        }


        private void Reset()
        {
            lines.Clear();
            stack.Clear();
            drawState.x = drawState.y = drawState.theta = 0;
        }
        
        public List<Line> InterpretString(String input)
        {
            Reset();
            foreach (char character in input) {
                switch (character) {
                    case 'f':
                        MoveForward();
                        break;
                    case 'F':
                        DrawLine();
                        break;
                    case '+':
                        drawState.theta += drawState.turning_angle;
                        break;
                    case '-':
                        drawState.theta -= drawState.turning_angle;
                        break;
                    default: break;
                }
            }
            return lines;
        }
    }
}
