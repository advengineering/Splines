using System.Drawing;
using System.Windows.Forms;

namespace Splines {
    internal class CSpline : IDraw {
        private readonly CPoint[] points;
        private readonly CSplineSubinterval[] splineSubintervals;

        public CPoint[] Points { get { return points; } }
        /// <summary>
        /// Df первой точки
        /// </summary>
        public double Df1 {
            get => points[0].Df;
            set => points[0].Df = value;
        }


        /// <summary>
        /// Ddf первой точки
        /// </summary>
        public double Ddf1 {
            get => points[0].Ddf;
            set => points[0].Ddf = value;
        }


        /// <summary>
        /// Df последней точки
        /// </summary>
        public double Dfn {
            get => points[points.Length - 1].Df;
            set => points[points.Length - 1].Df = value;
        }


        /// <summary>
        /// Ddf последней точки
        /// </summary>
        public double Ddfn {
            get => points[points.Length - 1].Ddf;
            set => points[points.Length - 1].Ddf = value;
        }


        public CSpline(CPoint[] points) {
            this.points = points;
            splineSubintervals = (points != null) ? new CSplineSubinterval[points.Length - 1] : null;
        }


        /// <summary>
        /// Создание кривой
        /// </summary>
        public void GenerateSplines(double val) {
            const double x1 = 0;
            var y1 = BuildSplineSubintervals(x1);

            double x2 = val;
            var y2 = BuildSplineSubintervals(x2);

            points[0].Ddf = -y1 * (x2 - x1) / (y2 - y1);

            BuildSplineSubintervals(points[0].Ddf);

            points[points.Length - 1].Ddf = splineSubintervals[splineSubintervals.Length - 1].CalcDdf(points[points.Length - 1].X);
        }


        /// <summary>
        /// Создание интервалов
        /// </summary>
        /// <param name="ddf1"></param>
        /// <returns></returns>
        private double BuildSplineSubintervals(double ddf1) {
            double df = points[0].Df;
            double ddf = ddf1;

            for (var i = 0; i < splineSubintervals.Length; i++) {
                splineSubintervals[i] = new CSplineSubinterval(points[i], points[i + 1], df, ddf);

                df = splineSubintervals[i].CalcDf(points[i + 1].X);
                ddf = splineSubintervals[i].CalcDdf(points[i + 1].X);

                if (i < splineSubintervals.Length - 1) {
                    points[i + 1].Df = df;
                    points[i + 1].Ddf = ddf;
                }
            }

            return df - this.Dfn;
        }


        /// <summary>
        /// Отображение интервалов и точек на канве
        /// </summary>
        /// <param name="canvas"></param>
        public void Draw(Graphics canvas) {
            foreach (var splineSubinterval in splineSubintervals) {
                splineSubinterval.Draw(canvas);
            }

            foreach (var point in points) {
                point.Draw(canvas);
            }
        }
    }
}
