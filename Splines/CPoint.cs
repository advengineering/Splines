﻿using System;
using System.Drawing;

namespace Splines {
    internal class CPoint : IDraw {
        public int X { get; }
        public int Y { get; }

        public double Df { get; set; }
        public double Ddf { get; set; }

        public CPoint(int x, int y) {
            X = x;
            Y = y;
        }

        public void Draw(Graphics canvas) {
            System.Console.WriteLine("point draw");

            double angleDf = Math.Atan(Df);

            canvas.DrawLine(new Pen(Color.Blue), (int) (X - Math.Cos(angleDf) * 50), (int) (Y - Math.Sin(angleDf) * 50), (int) (X + Math.Cos(angleDf) * 50), (int) (Y + Math.Sin(angleDf) * 50));
            canvas.DrawEllipse(new Pen(Color.BlueViolet, 5), new Rectangle(X - 1, Y - 1, 3, 3));
        }
    }
}
