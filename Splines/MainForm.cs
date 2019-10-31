using System;
using System.Drawing;
using System.Windows.Forms;

namespace Splines {
    public partial class MainForm : Form {
        private CSpline splineModel;
        private Bitmap bmp;


        public MainForm() {
            InitializeComponent();

            pictureBox1.Paint += PictureBox1_Paint;

            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }


        private void PictureBox1_Paint(object sender, PaintEventArgs e) {
            System.Console.WriteLine("pb paint");

            e.Graphics.DrawImage(bmp, 1, 1);
        }


        private void button1_Click(object sender, EventArgs e) {
            var random = new Random();

            int val = decimal.ToInt32(numericUpDown1.Value);

            CPoint[] points = new CPoint[val];
            var interval = (pictureBox1.Width - 20) / val;

            for (int i = 0; i < val; i++) {
                points[i] = new CPoint(random.Next(10 + interval * i, 10 + interval * (i + 1)), random.Next(10, pictureBox1.Height - 10));
            }

            splineModel = new CSpline(points);

            vScrollBar1.Value = 0;
            vScrollBar2.Value = 0;

            SetD1ToModel();
            GetDerivatesFromModel();
            Draw();
        }


        private void vScrollBar1_ValueChanged(object sender, EventArgs e) {
            SetD1ToModel();
            GetDerivatesFromModel();
            Draw();
        }


        private void SetD1ToModel() {
            if (splineModel != null) {
                splineModel.Df1 = vScrollBar1.Value / 1000.0f;
                splineModel.Dfn = vScrollBar2.Value / 1000.0f;

                splineModel.GenerateSplines();
            }
        }


        private void GetDerivatesFromModel() {
            if (splineModel != null) {                
                textBox_df1.Invoke(new Action(() => textBox_df1.Text = $"{-splineModel.Df1:0.0000}"));
                //textBox_df1.Text = $"{-splineModel.Df1:0.0000}";
                textBox_dfn.Text = $"{-splineModel.Dfn:0.0000}";
                textBox_ddf1.Text = $"{-splineModel.Ddf1:0.0000}";
                textBox_ddfn.Text = $"{-splineModel.Ddfn:0.0000}";

                Application.DoEvents();
            }
        }


        private void Draw() {
            if (splineModel != null) {
                System.Console.WriteLine("main form draw");

                Graphics canvas = Graphics.FromImage(bmp);
                splineModel.Draw(canvas);

                pictureBox1.Refresh();

                canvas.Clear(Color.White);
            }
        }


        private void MainForm_Load(object sender, EventArgs e) {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        private void MainForm_SizeChanged(object sender, EventArgs e) {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }
    }
}
