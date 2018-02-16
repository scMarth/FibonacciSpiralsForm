using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FibonacciSpirals
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void textBoxError()
        {
            MessageBox.Show("Error: Please check inputs.");
        }

        private bool canParseTextBoxToDouble(string str)
        {
            double number;
            bool canConvert = double.TryParse(str, out number);
            if (canConvert) return true;
            else return false;
        }

        private bool canParseTextBoxToInt(string str)
        {
            int number;
            bool canConvert = int.TryParse(str, out number);
            if (canConvert) return true;
            else return false;
        }

        private bool canParseTextBoxes()
        {
            bool test1 = canParseTextBoxToInt(originTextBoxX.Text);
            bool test2 = canParseTextBoxToInt(originTextBoxY.Text);
            bool test3 = canParseTextBoxToInt(radiusTextBox.Text);

            if (test1 && test2 && test3) return true;
            else return false;
        }

        private int getIntFromTextBox(string str)
        {
            int number;
            int.TryParse(str, out number);
            return number;
        }

        // Append string 'str' to output file 'filename'
        private static void appendToFile(string filename, string str)
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(filename, true))
            {
                file.Write(str);
                file.Flush();
                file.Close();
            }
        }

        private void rescaleResetProgressBar()
        {
            // reset the progress bar value to 0
            progressBar1.Value = 0;

            // rescale the progress bar
            int thirds = (int)(this.Width / 3);
            int marginTop = (int)(0.49 * this.Height);
            progressBar1.Top = marginTop;
            progressBar1.Left = thirds;
            progressBar1.Width = thirds;
        }

        private void printToText(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text|*.txt";
            sfd.Title = "Export Text";
            sfd.ShowDialog();

            var outfile = sfd.FileName;

            if (outfile != "")
            {
                // open the file
                System.IO.File.WriteAllText(outfile, "");

                // reset and rescale the progress bar
                rescaleResetProgressBar();

                // show the progress bar
                progressBar1.Visible = true;

                int numPoints = graph1.Series[0].Points.Count();

                // set progress bar maximum
                progressBar1.Maximum = numPoints;

                DataPoint point;
                for (int i = 0; i < numPoints; i++)
                {
                    point = graph1.Series[0].Points.ElementAt<DataPoint>(i);
                    double coordX = point.XValue;
                    double coordY = point.YValues[0];

                    //outBox.Text = "Point: " + point.ToString() + " coordX = " + coordX.ToString() + " coordY " + coordY.ToString();

                    appendToFile(outfile, coordX.ToString() + " " + coordY.ToString() + Environment.NewLine);
                    progressBar1.Increment(1);
                }

                // hide the progress bar
                progressBar1.Visible = false;
                MessageBox.Show("Done.");
            }
            else
            {
                MessageBox.Show("Error: Invalid filename. Aborting.");
            }
        }

        private void generatePreview(object sender, EventArgs e)
        {
            graph1.Series[0].Points.Clear();

            outBox.Text = "";
            bool inputsOkay = canParseTextBoxes();
            if (!(inputsOkay))
            {
                textBoxError();
                return;
            }

            int radius = getIntFromTextBox(radiusTextBox.Text);
            int arraySize = radius * radius;
            //outBox.Text = arraySize.ToString();

            double tau = (1 + Math.Sqrt(5)) / 2;

            double[] temp = new double[arraySize];
            double[] theta = new double[arraySize];
            double[] rad = new double[arraySize];

            double[] deltaX = new double[arraySize];
            double[] deltaY = new double[arraySize];

            double[] ptsX = new double[arraySize];
            double[] ptsY = new double[arraySize];

            double origX = getIntFromTextBox(originTextBoxX.Text);
            double origY = getIntFromTextBox(originTextBoxY.Text);

            double minx = 0, maxx = 0, miny = 0, maxy = 0;

            // Add the origin
            graph1.Series[1].Points.AddXY(origX, origY);

            for (int i=1; i<= arraySize; i++)
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