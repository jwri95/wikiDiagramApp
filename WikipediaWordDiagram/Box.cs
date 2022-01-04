using System.Drawing;

namespace WikipediaWordDiagram
{
    public class Box
    {
        //Shape
        public Rectangle rectangle;
        public Color col;

        //Data
        public string header;
        public string text;

        public Box(Rectangle rectangle, Color col, string header, string text)
        {
            this.rectangle = rectangle;
            this.col = col;
            this.header = header;
            this.text = text;
        }
    }
}
