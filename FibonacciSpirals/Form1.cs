using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace FibonacciSpirals
{
    public partial class Form1 : Form
    {
        private bool hasComputed = false;

        public Form1()
        {
            InitializeComponent();

            button2.Enabled = false;
            setButton1(0);
        }

        private void textBoxError()
        {
            MessageBox.Show("Error: Please check inputs.");
        }

        // Return true if the input string can be parsed into a double
        private bool canParseStringToDouble(string str)
        {
            double number;
            bool canConvert = double.TryParse(str, out number);
            if (canConvert) return true;
            else return false;
        }

        private bool canParseStringToInt(string str)
        {
            int number;
            bool canConvert = int.TryParse(str, out number);
            if (canConvert) return true;
            else return false;
        }

        private bool canParseTextBoxes()
        {
            bool test1 = canParseStringToDouble(originTextBoxX.Text);
            bool test2 = canParseStringToDouble(originTextBoxY.Text);
            bool test3 = canParseStringToDouble(radiusTextBox.Text);
            bool test4 = canParseStringToDouble(multiplierBox.Text);

            if (test1 && test2 && test3 && test4) return true;
            else return false;
        }

        private int getIntFromString(string str)
        {
            int number;
            int.TryParse(str, out number);
            return number;
        }

        private double getDoubleFromString(string str)
        {
            double number;
            double.TryParse(str, out number);
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
            // Reset the progress bar value to 0
            progressBar1.Value = 0;

            // Rescale the progress bar
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
                // Open the file
                System.IO.File.WriteAllText(outfile, "");

                // Reset and rescale the progress bar
                rescaleResetProgressBar();

                // Show the progress bar
                progressBar1.Visible = true;

                int numPoints = graph1.Series[0].Points.Count();

                // Set progress bar maximum
                progressBar1.Maximum = numPoints;

                DataPoint point;
                for (int i = 0; i < numPoints; i++)
                {
                    point = graph1.Series[0].Points.ElementAt<DataPoint>(i);
                    double coordX = point.XValue;
                    double coordY = point.YValues[0];

                    appendToFile(outfile, coordX.ToString() + " " + coordY.ToString() + Environment.NewLine);
                    progressBar1.Increment(1);
                }

                // Hide the progress bar
                progressBar1.Visible = false;
                MessageBox.Show("Done.");
            }
            else
            {
                MessageBox.Show("Error: Invalid filename. Aborting.");
            }
        }

        private void compute(object sender, EventArgs e)
        {
            if (checkBox1.Checked) generatePreview();
            else computeWithoutPreview();

            hasComputed = true;
            button2.Enabled = true;
            button2.ForeColor = Color.Green;
            setButton1(0);
        }

        private void computeWithoutPreview()
        {
            // Make sure the inputs are all valid integer values
            bool inputsOkay = canParseTextBoxes();
            if (!(inputsOkay))
            {
                textBoxError();
                return;
            }

            double radius = getDoubleFromString(radiusTextBox.Text);
            int arraySize = Convert.ToInt32(Math.Floor(radius * radius));
            numPoints.Text = arraySize.ToString();

            // Retrieve the multiplier
            double scaler = getDoubleFromString(multiplierBox.Text);

            double tau = (1 + Math.Sqrt(5)) / 2;

            double[] temp = new double[arraySize];
            double[] theta = new double[arraySize];
            double[] rad = new double[arraySize];

            double[] deltaX = new double[arraySize];
            double[] deltaY = new double[arraySize];

            double[] ptsX = new double[arraySize];
            double[] ptsY = new double[arraySize];

            // Retrieve the coordinates for the origin
            double origX = getDoubleFromString(originTextBoxX.Text);
            double origY = getDoubleFromString(originTextBoxY.Text);

            // Reset and rescale the progress bar
            rescaleResetProgressBar();

            // Show the progress bar
            progressBar1.Visible = true;

            // Set progress bar maximum
            progressBar1.Maximum = arraySize;

            for (int i = 1; i <= arraySize; i++)
            {
                temp[i - 1] = i;
                theta[i - 1] = temp[i - 1] * (2 * Math.PI) / tau;
                rad[i - 1] = Math.Sqrt(temp[i - 1]);

                deltaX[i - 1] = (Math.Cos(theta[i - 1])) * rad[i - 1];
                deltaY[i - 1] = (Math.Sin(theta[i - 1])) * rad[i - 1];

                ptsX[i - 1] = (scaler * deltaX[i - 1]) + origX;
                ptsY[i - 1] = (scaler * deltaY[i - 1]) + origY;

                progressBar1.Increment(1);
            }

            // Hide the progress bar
            progressBar1.Visible = false;
        }

        private void generatePreview()
        {
            // Clear the coordinates before generating new ones
            graph1.Series[0].Points.Clear();
            graph1.Series[1].Points.Clear();

            // Make sure the inputs are all valid integer values
            bool inputsOkay = canParseTextBoxes();
            if (!(inputsOkay))
            {
                textBoxError();
                return;
            }

            double radius = getDoubleFromString(radiusTextBox.Text);
            int arraySize = Convert.ToInt32(Math.Floor(radius * radius));
            numPoints.Text = arraySize.ToString();

            // Retrieve the multiplier
            double scaler = getDoubleFromString(multiplierBox.Text);

            double tau = (1 + Math.Sqrt(5)) / 2;

            double[] temp = new double[arraySize];
            double[] theta = new double[arraySize];
            double[] rad = new double[arraySize];

            double[] deltaX = new double[arraySize];
            double[] deltaY = new double[arraySize];

            double[] ptsX = new double[arraySize];
            double[] ptsY = new double[arraySize];

            // Retrieve the coordinates for the origin
            double origX = getDoubleFromString(originTextBoxX.Text);
            double origY = getDoubleFromString(originTextBoxY.Text);

            graph1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;

            // Add the origin
            graph1.Series[1].Points.AddXY(origX, origY);

            // Reset and rescale the progress bar
            rescaleResetProgressBar();

            // Show the progress bar
            progressBar1.Visible = true;

            // Set progress bar maximum
            progressBar1.Maximum = arraySize;

            for (int i = 1; i <= arraySize; i++)
            {
                temp[i - 1] = i;
                theta[i - 1] = temp[i - 1] * (2 * Math.PI) / tau;
                rad[i - 1] = Math.Sqrt(temp[i - 1]);

                deltaX[i - 1] = (Math.Cos(theta[i - 1])) * rad[i - 1];
                deltaY[i - 1] = (Math.Sin(theta[i - 1])) * rad[i - 1];

                ptsX[i - 1] = (scaler * deltaX[i - 1]) + origX;
                ptsY[i - 1] = (scaler * deltaY[i - 1]) + origY;

                graph1.Series[0].Points.AddXY(ptsX[i - 1], ptsY[i - 1]);
                progressBar1.Increment(1);
            }

            // Hide the progress bar
            progressBar1.Visible = false;

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

        Point? prevPosition = null;
        ToolTip tooltip = new ToolTip();

        // Display the coordinates of a point when the user hovers over one
        void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            if (prevPosition.HasValue && pos == prevPosition.Value)
                return;
            tooltip.RemoveAll();
            prevPosition = pos;
            var results = graph1.HitTest(pos.X, pos.Y, false,
                                            ChartElementType.DataPoint);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    var prop = result.Object as DataPoint;
                    if (prop != null)
                    {
                        var pointXPixel = result.ChartArea.AxisX.ValueToPixelPosition(prop.XValue);
                        var pointYPixel = result.ChartArea.AxisY.ValueToPixelPosition(prop.YValues[0]);

                        // Check if the cursor is really close to the point (2 pixels around the point)
                        if (Math.Abs(pos.X - pointXPixel) < 2 &&
                            Math.Abs(pos.Y - pointYPixel) < 2)
                        {
                            tooltip.Show("X=" + prop.XValue + ", Y=" + prop.YValues[0], this.graph1,
                                            pos.X, pos.Y - 15);
                        }
                    }
                }
            }
        }

        // Helper function to color the compute button
        private void setButton1(int flag)
        {
            if (flag == 0) button1.ForeColor = Color.Green; // Color the text green
            else button1.ForeColor = Color.Red; // Color the text red
        }

        // When any of the input text fields are changed, color the Compute button in red to imply the changed arguments

        private void originTextBoxX_TextChanged(object sender, EventArgs e)
        {
            setButton1(1);
        }

        private void originTextBoxY_TextChanged(object sender, EventArgs e)
        {
            setButton1(1);
        }

        private void radiusTextBox_TextChanged(object sender, EventArgs e)
        {
            setButton1(1);
        }

        private void multiplierBox_TextChanged(object sender, EventArgs e)
        {
            setButton1(1);
        }
    }
}