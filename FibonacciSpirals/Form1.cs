using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FibonacciSpirals
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double tau = (1 + Math.Sqrt(5)) / 2;

            double[] temp = new double[400];
            double[] theta = new double[400];
            double[] rad = new double[400];

            double[] deltaX = new double[400];
            double[] deltaY = new double[400];

            double[] ptsX = new double[400];
            double[] ptsY = new double[400];

            double origX = 508000;
            double origY = 4475000;

            double minx = 0, maxx = 0, miny = 0, maxy = 0;

            // Add the origin
            graph1.Series[1].Points.AddXY(origX, origY);

            for (int i=1; i<=400; i++)
            {
                temp[i-1] = i;
                theta[i-1] = temp[i-1]*(2*Math.PI)/tau;
                rad[i - 1] = Math.Sqrt(temp[i - 1]);

                deltaX[i - 1] = (Math.Cos(theta[i - 1])) * rad[i - 1];
                deltaY[i - 1] = (Math.Sin(theta[i - 1])) * rad[i - 1];

                ptsX[i - 1] = deltaX[i - 1] + origX;
                ptsY[i - 1] = deltaY[i - 1] + origY;

                if (i == 1)
                {
                    minx = ptsX[i-1];
                    maxx = ptsX[i - 1];
                    miny = ptsY[i - 1];
                    maxy = ptsY[i - 1];
                }else
                {
                    if (ptsX[i - 1] < minx) minx = ptsX[i - 1];
                    if (ptsX[i - 1] < maxx) maxx = ptsX[i - 1];
                    if (ptsY[i - 1] < miny) miny = ptsY[i - 1];
                    if (ptsY[i - 1] < maxy) maxy = ptsY[i - 1];
                }

                graph1.Series[0].Points.AddXY(ptsX[i-1], ptsY[i-1]);
                graph1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            }

            //textBox1.Text = "minx = " + minx.ToString() + " | maxx = " + maxx.ToString() + " | miny = " + miny.ToString() + " | maxy = " + maxy.ToString();

            graph1.ChartAreas[0].AxisX.IsStartedFromZero = false;
            graph1.ChartAreas[0].AxisY.IsStartedFromZero = false;

            // Set automatic zooming
            graph1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            graph1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;

            // Set automatic scrolling 
            graph1.ChartAreas[0].CursorX.AutoScroll = true;
            graph1.ChartAreas[0].CursorY.AutoScroll = true;

            // Allow user selection for Zoom
            graph1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            graph1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;

        }
    }
}