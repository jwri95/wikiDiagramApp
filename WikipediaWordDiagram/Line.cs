using System.Drawing;


namespace WikipediaWordDiagram
{
    //A set of data to be used for a line that will be written to a picturebox
    class Line
    {
        public Line(Point start, Point end, Color colour)
        {
            Start = start;
            End = end;
            Colour = colour;
        }

        public Point Start { get; }
        public Point End { get; }
        public Color Colour { get; }
    }
}
