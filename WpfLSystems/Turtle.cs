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
        public class DrawState
        {
            public double line_length = 5;
            public double line_width = 1;
            public double angle_increment = 22.5;
            
            public double x = 0;
            public double y = 0;
            public double theta = Math.PI / 2;

            public DrawState Clone()
            {
                return this.MemberwiseClone() as DrawState;
            }
        }
        
        List<Line> lines = new List<Line>();
        Stack<DrawState> stack = new Stack<DrawState>();
        public DrawState drawState = new DrawState();
        public uint n = 0;

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

        private void MirrorAboutXAxis()
        {
            foreach (Line line in lines)
            {
                line.Y1 *= -1;
                line.Y2 *= -1;
            }
        }

        private void Reset()
        {
            lines.Clear();
            stack.Clear();
            drawState.x = drawState.y = 0;
            drawState.theta = Math.PI / 2;
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
                        drawState.theta += drawState.angle_increment * Math.PI / 180;
                        break;
                    case '-':
                        drawState.theta -= drawState.angle_increment * Math.PI / 180;
                        break;
                    case '[':
                        stack.Push(drawState.Clone());
                        break;
                    case ']':
                        if (stack.Count != 0)
                        {
                            drawState = stack.Pop();
                        }
                        break;
                    default: break;
                }
            }
            MirrorAboutXAxis();
            return lines;
        }
    }
}
