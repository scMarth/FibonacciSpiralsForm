# FibonacciSpiralsForm

## Description

A Windows C# Form Application for generating coordinates on a Fibonacci Spiral

## Usage

Open the application:

![Alt text](https://raw.githubusercontent.com/scMarth/FibonacciSpiralsForm/master/screenshots/screenshot1.png)

The application takes in 4 decimal parameters:

1. The x-coordinate of the epicenter or origin point

2. The y-coordinate of the epicenter or origin point

3. The radius of the Fibonacci Spiral.

4. The multiplier, which will be multiplied to every resulting coordinate. This multiplier can be used to control the spacing between the Fibonacci Spiral coordinates.

Once the parameters are inputted, press the "Generate Preview" button to see a preview of the generated coordinates.

![Alt text](https://raw.githubusercontent.com/scMarth/FibonacciSpiralsForm/master/screenshots/screenshot2.png)

If you want to export the coordinates, you can click the "Export to Text" button after the parameters have been inputted. This will allow you to save a text file containing the coordinates. The resulting text file has 1 coordinate per line: each line contains first the x-coordinate of a point, then a space, then the y-coordinate of the point.

Note that you don't need to generate a preview before exporting the coordinates as a text file, which would be useful when dealing with large radii, where rendering many points could take a long time. There is also a checkbox to disable the preview, which also prevents rendering (or re-rendering) of the points.
