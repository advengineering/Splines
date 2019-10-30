using System;
using System.Drawing;
using System.Windows.Forms;

namespace Splines {
    internal class CSplineSubinterval : IDraw {
        public double A { get; }
        public double B { get; }
        public double C { get; }
        public double D { get; }

        private readonly CPoint point1;
        private readonly CPoint point2;

        private readonly Pen pen = new Pen(Color.Red, 2);

        public CSplineSubinterval(CPoint p1, CPoint p2, double df, double ddf) {
            point1 = p1;
            point2 = p2;

            B = ddf;
            C = df;
            D = point1.Y;
            A = (point2.Y - B * Math.Pow(point2.X - point1.X, 2) - C * (point2.X - point1.X) - D) / Math.Pow(point2.X - point1.X, 3);
        }


        public double F(int x) {
            return A * Math.Pow(x - point1.X, 3) + B * Math.Pow(x - point1.X, 2) + C * (x - point1.X) + D;
        }


        public double Df(int x) {
            return 3 * A * Math.Pow(x - point1.X, 2) + 2 * B * (x - point1.X) + C;
        }


        public double Ddf(int x) {
            return 6 * A * (x - point1.X) + 2 * B;
        }


        public void Draw(Graphics canvas) {
            System.Console.WriteLine("subinterval draw");

            for (int x = point1.X; x < point2.X; x++) {
                canvas.DrawLine(pen, x, (int) F(x), x + 1, (int) F(x + 1));
            }
        }
    }
}
