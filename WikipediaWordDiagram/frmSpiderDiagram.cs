using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace WikipediaWordDiagram
{
    public partial class frmSpiderDiagram : Form
    {
        Random rnd = new Random();
        public Header main;
        List<Box> shapes = new List<Box>();
        List<Line> lines = new List<Line>();

        int sizeFactor = 16;
        int margin = 0;
        int w = 0;
        int h = 0;

        int h2Count = 0;
        int h3Count = 0;
        int h4Count = 0;

        List<Color> colours = new List<Color>();

        public Color mainColour = Color.White;

        public frmSpiderDiagram(Header main)
        {
            InitializeComponent();

            this.main = main;

            RemoveUncheckedHeaders();

            //altering the size of the grid based on the number of headers 

            Debug.WriteLine(main.name + " | " + main.description);
            foreach (Header h2 in main.children)
            {
                h2Count++;

                if (h2.children.Count == 0)
                {
                    Debug.WriteLine(h2.parent.name + " -> " + h2.name);
                }

                foreach (Header h3 in h2.children)
                {
                    h3Count++;

                    if (h3.children.Count == 0)
                    {
                        Debug.WriteLine(h2.parent.name + " -> " + h3.parent.name + " -> " + h3.name);
                    }

                    foreach (Header h4 in h3.children)
                    {
                        h4Count++;
                        Debug.WriteLine(h2.parent.name + " -> " + h3.parent.name + " -> " + h4.parent.name + " -> " + h4.name);
                    }
                }
            }

            if (h2Count > 0)
            {
                sizeFactor = 11;
            }

            if (h3Count > 0)
            {
                sizeFactor = 15;
            }

            if (h4Count > 0)
            {
                sizeFactor = 17;
            }

            for (int i = 0; i < h2Count; i++)
            {
                Color randCol = GetRandColor();

                //Make sure we do not get any duplicates
                if (colours.Contains(randCol))
                {
                    while (colours.Contains(randCol))
                    {
                        randCol = GetRandColor();
                    }
                }

                colours.Add(randCol);
            }

            //Resizes the screen and also draws the data onto the screen due to size changed event
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            pictureBox1.Refresh();
        }

        public Color GetRandColor()
        {
            List<Color> possibleColours = new List<Color>();

            possibleColours.Add(Color.Red);
            possibleColours.Add(Color.DarkBlue);
            possibleColours.Add(Color.Cyan);
            possibleColours.Add(Color.Lime);
            possibleColours.Add(Color.Purple);
            possibleColours.Add(Color.DarkOrange);
            possibleColours.Add(Color.Yellow);
            possibleColours.Add(Color.DeepPink);
            possibleColours.Add(Color.SeaGreen);
            possibleColours.Add(Color.CornflowerBlue);
            possibleColours.Add(Color.SlateGray);
            possibleColours.Add(Color.Plum);

            //If more than possible colours count (12) headers then we need to add some more random colours to fill the void
            if (h2Count > possibleColours.Count)
            {
                int nExtra = h2Count - possibleColours.Count;

                //Get X number of random colours
                for (int i = 0; i < nExtra; i++)
                {
                    int r = rnd.Next(Enum.GetValues(typeof(KnownColor)).Length);

                    KnownColor randCol = (KnownColor)Enum.GetValues(typeof(KnownColor)).GetValue(r);

                    possibleColours.Add(Color.FromKnownColor(randCol));
                }
            }

            int index = rnd.Next(possibleColours.Count);

            return possibleColours[index];
        }

        public bool CheckCollision(int[,] grid, int prevX, int prevY, int X, int Y)
        {
            //Need to find if there are any objects inbetween grid[prevX, prevY] & grid[X,Y]
            //e.g. grid[10,10] -> grid[6,6] -> check if anything at grid[7,7] grid[8,8] and grid[9,9]
            //e.g. grid[10,10] -> grid[8,6] -> check if any at grid[9,8]
            //e.g. grid[11,11] -> grid[8,9] -> nothing to check?
            //e.g. grid[10,7] -> grid[10,10] -> check if any at grid[10,8] grid[10,9]
            //e.g. grid[10,10] -> grid[6,5] -> nothing to check?

            int xDiff = X - prevX;
            int yDiff = Y - prevY;

            //make sure there is an actual difference in one of them
            if (Math.Abs(xDiff) > 0 || Math.Abs(yDiff) > 0)
            {
                if (X == prevX || Y == prevY)
                {
                    if (X == prevX)
                    {
                        //down
                        if (yDiff > 0)
                        {
                            for (int i = 1; i <= Math.Abs(yDiff); i++)
                            {
                                int y = prevY + i;
                                if (y < sizeFactor && y >= 0)
                                {
                                    if (grid[X, y] == 1)
                                    {
                                        Debug.WriteLine("Collision (down) between (" + prevX + ", " + prevY + ") and (" + X + ", " + Y + ") at (" + X + ", " + y + ")");
                                        return true;
                                    }
                                }
                            }
                        }
                        //up
                        else
                        {
                            for (int i = 1; i <= Math.Abs(yDiff); i++)
                            {
                                int y = prevY - i;
                                if (y < sizeFactor && y >= 0)
                                {
                                    if (grid[X, y] == 1)
                                    {
                                        Debug.WriteLine("Collision (up) between (" + prevX + ", " + prevY + ") and (" + X + ", " + Y + ") at (" + X + ", " + y + ")");
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //right
                        if (xDiff > 0)
                        {
                            for (int i = 1; i <= Math.Abs(xDiff); i++)
                            {
                                int x = prevX + i;
                                if (x < sizeFactor && x >= 0)
                                {
                                    if (grid[x, Y] == 1)
                                    {
                                        Debug.WriteLine("Collision (right) between (" + prevX + ", " + prevY + ") and (" + X + ", " + Y + ") at (" + x + ", " + Y + ")");
                                        return true;
                                    }
                                }
                            }
                        }
                        //left
                        else
                        {
                            for (int i = 1; i <= Math.Abs(xDiff); i++)
                            {
                                int x = prevX - i;
                                if (x < sizeFactor && x >= 0)
                                {
                                    if (grid[x, Y] == 1)
                                    {
                                        Debug.WriteLine("Collision (left) between (" + prevX + ", " + prevY + ") and (" + X + ", " + Y + ") at(" + x + ", " + Y + ")");
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Making sure it is a value we can divide by 2 (even)
                    if (Math.Abs(xDiff) % 2 == 0 && Math.Abs(yDiff) % 2 == 0)
                    {
                        int x = Math.Abs(xDiff) / 2;
                        int y = Math.Abs(yDiff) / 2;

                        //right
                        if (xDiff > 0)
                        {
                            x = prevX + x;
                        }
                        //left
                        else
                        {
                            x = prevX - x;
                        }

                        //down
                        if (yDiff > 0)
                        {
                            y = prevY + y;
                        }
                        //up
                        else
                        {
                            y = prevY - y;
                        }

                        if (x < sizeFactor && x >= 0 && y < sizeFactor && y >= 0)
                        {
                            if (grid[x, y] == 1)
                            {
                                Debug.WriteLine("Collision (diagonal) between (" + prevX + ", " + prevY + ") and (" + X + ", " + Y + ") at (" + x + ", " + y + ")");
                                return true;
                            }
                            else
                            {
                                if (x % 2 == 0 && y % 2 == 0)
                                {
                                    x = x / 2;
                                    y = y / 2;

                                    //right
                                    if (xDiff > 0)
                                    {
                                        x = prevX + x;
                                    }
                                    //left
                                    else
                                    {
                                        x = prevX - x;
                                    }

                                    //down
                                    if (yDiff > 0)
                                    {
                                        y = prevY + y;
                                    }
                                    //up
                                    else
                                    {
                                        y = prevY - y;
                                    }

                                    if (x < sizeFactor && x >= 0 && y < sizeFactor && y >= 0)
                                    {
                                        if (grid[x, y] == 1)
                                        {
                                            Debug.WriteLine("Collision (diagonal) between (" + prevX + ", " + prevY + ") and (" + X + ", " + Y + ") at (" + x + ", " + y + ")");
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //if no collision found then return false
            return false;
        }

        public Tuple<int, int> GetIndex(int[,] grid, int prevW, int prevH, int xDiff, int yDiff)
        {
            int wIndex = -1;
            int hIndex = -1;

            bool starter = false;

            //right = positive X
            bool right = true;
            //left = negative X
            bool left = true;
            //up = negative y
            bool up = true;
            //down = positive y
            bool down = true;

            //input 0s for both if it is the first header (starter)
            if (xDiff == 0 & yDiff == 0)
            {
                starter = true;
            }
            else
            {
                //Right
                if (xDiff > 0)
                {
                    left = false;
                }
                else if (xDiff < 0)
                {
                    right = false;
                }
            }

            #region margin1

            //Check margin of 1 is allowed in each direction

            if (right && prevW + margin >= sizeFactor)
            {
                Debug.WriteLine("(" + prevW + ", " + prevH + ") cannot go right by " + margin + " grid space(s).");
                //cannot go right
                right = false;
            }

            if (left && prevW - margin < 0)
            {
                Debug.WriteLine("(" + prevW + ", " + prevH + ") cannot go left by " + margin + " grid space(s).");
                //cannot go left
                left = false;
            }

            if (up && prevH - margin < 0)
            {
                Debug.WriteLine("(" + prevW + ", " + prevH + ") cannot go up by " + margin + " grid space(s).");
                //cannot go left
                up = false;
            }

            if (down && prevH + margin >= sizeFactor)
            {
                Debug.WriteLine("(" + prevW + ", " + prevH + ") cannot go down by " + margin + " grid space(s).");
                //cannot go left
                down = false;
            }

            if (starter)
            {
                if (left)
                {
                    if (grid[prevW - margin, prevH] == 0)
                    {
                        wIndex = prevW - margin;
                        hIndex = prevH;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (right)
                {
                    if (grid[prevW + margin, prevH] == 0)
                    {
                        wIndex = prevW + margin;
                        hIndex = prevH;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (down)
                {
                    if (grid[prevW, prevH + margin] == 0)
                    {
                        wIndex = prevW;
                        hIndex = prevH + margin;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (up)
                {
                    if (grid[prevW, prevH - margin] == 0)
                    {
                        wIndex = prevW;
                        hIndex = prevH - margin;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (right & down)
                {
                    if (grid[prevW + margin, prevH + margin] == 0)
                    {
                        wIndex = prevW + margin;
                        hIndex = prevH + margin;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (left & up)
                {
                    if (grid[prevW - margin, prevH - margin] == 0)
                    {
                        wIndex = prevW - margin;
                        hIndex = prevH - margin;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (right & up)
                {
                    if (grid[prevW + margin, prevH - margin] == 0)
                    {
                        wIndex = prevW + margin;
                        hIndex = prevH - margin;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (left & down)
                {
                    if (grid[prevW - margin, prevH + margin] == 0)
                    {
                        wIndex = prevW - margin;
                        hIndex = prevH + margin;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }
            }
            else
            {
                if (up)
                {
                    if (grid[prevW, prevH - margin] == 0)
                    {
                        wIndex = prevW;
                        hIndex = prevH - margin;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (down)
                {
                    if (grid[prevW, prevH + margin] == 0)
                    {
                        wIndex = prevW;
                        hIndex = prevH + margin;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                //right
                if (right)
                {
                    if (grid[prevW + margin, prevH] == 0)
                    {
                        wIndex = prevW + margin;
                        hIndex = prevH;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }

                    if (up)
                    {
                        if (grid[prevW + margin, prevH - margin] == 0)
                        {
                            wIndex = prevW + margin;
                            hIndex = prevH - margin;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }
                    }

                    if (down)
                    {
                        if (grid[prevW + margin, prevH + margin] == 0)
                        {
                            wIndex = prevW + margin;
                            hIndex = prevH + margin;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }
                    }
                }

                //left
                if (left)
                {
                    if (grid[prevW - margin, prevH] == 0)
                    {
                        wIndex = prevW - margin;
                        hIndex = prevH;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }

                    if (up)
                    {
                        if (grid[prevW - margin, prevH - margin] == 0)
                        {
                            wIndex = prevW - margin;
                            hIndex = prevH - margin;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }
                    }

                    if (down)
                    {
                        if (grid[prevW - margin, prevH + margin] == 0)
                        {
                            wIndex = prevW - margin;
                            hIndex = prevH + margin;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }
                    }
                }
            }

            #endregion

            #region margin2

            int m2 = margin * 2;

            //Check margin of 2 is allowed in each direction

            if (right && prevW + m2 >= sizeFactor)
            {
                Debug.WriteLine("(" + prevW + ", " + prevH + ") cannot go right by " + m2 + " grid space(s).");
                //cannot go right
                right = false;
            }

            if (left && prevW - m2 < 0)
            {
                Debug.WriteLine("(" + prevW + ", " + prevH + ") cannot go left by " + m2 + " grid space(s).");
                //cannot go left
                left = false;
            }

            if (up && prevH - m2 < 0)
            {
                Debug.WriteLine("(" + prevW + ", " + prevH + ") cannot go up by " + m2 + " grid space(s).");
                //cannot go left
                up = false;
            }

            if (down && prevH + m2 >= sizeFactor)
            {
                Debug.WriteLine("(" + prevW + ", " + prevH + ") cannot go down by " + m2 + " grid space(s).");
                //cannot go left
                down = false;
            }

            if (starter == true)
            {
                if (up)
                {
                    if (grid[prevW, prevH - m2] == 0)
                    {
                        wIndex = prevW;
                        hIndex = prevH - m2;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (down)
                {
                    if (grid[prevW, prevH + m2] == 0)
                    {
                        wIndex = prevW;
                        hIndex = prevH + m2;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (right)
                {
                    if (grid[prevW + m2, prevH] == 0)
                    {
                        wIndex = prevW + m2;
                        hIndex = prevH;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (right & up)
                {
                    if (grid[prevW + m2, prevH - m2] == 0)
                    {
                        wIndex = prevW + m2;
                        hIndex = prevH - m2;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }

                    if (grid[prevW + m2, prevH - margin] == 0)
                    {
                        wIndex = prevW + m2;
                        hIndex = prevH - margin;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }

                    if (grid[prevW + margin, prevH - m2] == 0)
                    {
                        wIndex = prevW + margin;
                        hIndex = prevH - m2;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (right & down)
                {
                    if (grid[prevW + m2, prevH + m2] == 0)
                    {
                        wIndex = prevW + m2;
                        hIndex = prevH + m2;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }

                    if (grid[prevW + m2, prevH + margin] == 0)
                    {
                        wIndex = prevW + m2;
                        hIndex = prevH + margin;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }

                    if (grid[prevW + margin, prevH + m2] == 0)
                    {
                        wIndex = prevW + margin;
                        hIndex = prevH + m2;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (left)
                {
                    if (grid[prevW - m2, prevH] == 0)
                    {
                        wIndex = prevW - m2;
                        hIndex = prevH;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (left & up)
                {
                    if (grid[prevW - m2, prevH - m2] == 0)
                    {
                        wIndex = prevW - m2;
                        hIndex = prevH - m2;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }

                    if (grid[prevW - m2, prevH - margin] == 0)
                    {
                        wIndex = prevW - m2;
                        hIndex = prevH - margin;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }

                    if (grid[prevW - margin, prevH - m2] == 0)
                    {
                        wIndex = prevW - margin;
                        hIndex = prevH - m2;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (left & down)
                {
                    if (grid[prevW - m2, prevH + m2] == 0)
                    {
                        wIndex = prevW - m2;
                        hIndex = prevH + m2;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }

                    if (grid[prevW - m2, prevH + margin] == 0)
                    {
                        wIndex = prevW - m2;
                        hIndex = prevH + margin;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }

                    if (grid[prevW - margin, prevH + m2] == 0)
                    {
                        wIndex = prevW - margin;
                        hIndex = prevH + m2;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }
            }
            else
            {
                if (up)
                {
                    if (grid[prevW, prevH - m2] == 0)
                    {
                        wIndex = prevW;
                        hIndex = prevH - m2;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (down)
                {
                    if (grid[prevW, prevH + m2] == 0)
                    {
                        wIndex = prevW;
                        hIndex = prevH + m2;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                //right
                if (right)
                {
                    if (grid[prevW + m2, prevH] == 0)
                    {
                        wIndex = prevW + m2;
                        hIndex = prevH;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }

                    if (up)
                    {
                        if (grid[prevW + m2, prevH - m2] == 0)
                        {
                            wIndex = prevW + m2;
                            hIndex = prevH - m2;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW + m2, prevH - margin] == 0)
                        {
                            wIndex = prevW + m2;
                            hIndex = prevH - margin;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW + margin, prevH - m2] == 0)
                        {
                            wIndex = prevW + margin;
                            hIndex = prevH - m2;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }
                    }

                    if (down)
                    {
                        if (grid[prevW + m2, prevH + m2] == 0)
                        {
                            wIndex = prevW + m2;
                            hIndex = prevH + m2;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW + m2, prevH + margin] == 0)
                        {
                            wIndex = prevW + m2;
                            hIndex = prevH + margin;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW + margin, prevH + m2] == 0)
                        {
                            wIndex = prevW + margin;
                            hIndex = prevH + m2;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }
                    }
                }

                //left
                if (left)
                {
                    if (grid[prevW - m2, prevH] == 0)
                    {
                        wIndex = prevW - m2;
                        hIndex = prevH;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }

                    if (up)
                    {
                        if (grid[prevW - m2, prevH - m2] == 0)
                        {
                            wIndex = prevW - m2;
                            hIndex = prevH - m2;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW - m2, prevH - margin] == 0)
                        {
                            wIndex = prevW - m2;
                            hIndex = prevH - margin;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW - margin, prevH - m2] == 0)
                        {
                            wIndex = prevW - margin;
                            hIndex = prevH - m2;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }
                    }

                    if (down)
                    {
                        if (grid[prevW - m2, prevH + m2] == 0)
                        {
                            wIndex = prevW - m2;
                            hIndex = prevH + m2;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW - m2, prevH + margin] == 0)
                        {
                            wIndex = prevW - m2;
                            hIndex = prevH + margin;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW - margin, prevH + m2] == 0)
                        {
                            wIndex = prevW - margin;
                            hIndex = prevH + m2;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }
                    }
                }
            }

            #endregion

            #region margin3

            int m3 = margin * 3;

            //Check margin of 3 is allowed in each direction

            if (right && prevW + m3 >= sizeFactor)
            {
                Debug.WriteLine("(" + prevW + ", " + prevH + ") cannot go right by " + m3 + " grid space(s).");
                //cannot go right
                right = false;
            }

            if (left && prevW - m3 < 0)
            {
                Debug.WriteLine("(" + prevW + ", " + prevH + ") cannot go left by " + m3 + " grid space(s).");
                //cannot go left
                left = false;
            }

            if (up && prevH - m3 < 0)
            {
                Debug.WriteLine("(" + prevW + ", " + prevH + ") cannot go up by " + m3 + " grid space(s).");
                //cannot go left
                up = false;
            }

            if (down && prevH + m3 >= sizeFactor)
            {
                Debug.WriteLine("(" + prevW + ", " + prevH + ") cannot go down by " + m3 + " grid space(s).");
                //cannot go left
                down = false;
            }

            if (starter != true)
            {
                if (up)
                {
                    if (grid[prevW, prevH - m3] == 0)
                    {
                        wIndex = prevW;
                        hIndex = prevH - m3;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                if (down)
                {
                    if (grid[prevW, prevH + m3] == 0)
                    {
                        wIndex = prevW;
                        hIndex = prevH + m3;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }
                }

                //right
                if (right)
                {
                    if (grid[prevW + m3, prevH] == 0)
                    {
                        wIndex = prevW + m3;
                        hIndex = prevH;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }

                    if (up)
                    {
                        if (grid[prevW + m3, prevH - m3] == 0)
                        {
                            wIndex = prevW + m3;
                            hIndex = prevH - m3;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW + m3, prevH - m2] == 0)
                        {
                            wIndex = prevW + m3;
                            hIndex = prevH - m2;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW + m3, prevH - margin] == 0)
                        {
                            wIndex = prevW + m3;
                            hIndex = prevH - margin;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW + m2, prevH - m3] == 0)
                        {
                            wIndex = prevW + m2;
                            hIndex = prevH - m3;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW + margin, prevH - m3] == 0)
                        {
                            wIndex = prevW + margin;
                            hIndex = prevH - m3;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }
                    }

                    if (down)
                    {
                        if (grid[prevW + m3, prevH + m3] == 0)
                        {
                            wIndex = prevW + m3;
                            hIndex = prevH + m3;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW + m3, prevH + m2] == 0)
                        {
                            wIndex = prevW + m3;
                            hIndex = prevH + m2;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW + m3, prevH + margin] == 0)
                        {
                            wIndex = prevW + m3;
                            hIndex = prevH + margin;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW + m2, prevH + m3] == 0)
                        {
                            wIndex = prevW + m2;
                            hIndex = prevH + m3;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW + margin, prevH + m3] == 0)
                        {
                            wIndex = prevW + margin;
                            hIndex = prevH + m3;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }
                    }
                }

                //left
                if (left)
                {
                    if (grid[prevW - m3, prevH] == 0)
                    {
                        wIndex = prevW - m3;
                        hIndex = prevH;
                        if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                        {
                            return new Tuple<int, int>(wIndex, hIndex);
                        }
                    }

                    if (up)
                    {
                        if (grid[prevW - m3, prevH - m3] == 0)
                        {
                            wIndex = prevW - m3;
                            hIndex = prevH - m3;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW - m3, prevH - m2] == 0)
                        {
                            wIndex = prevW - m3;
                            hIndex = prevH - m2;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW - m3, prevH - margin] == 0)
                        {
                            wIndex = prevW - m3;
                            hIndex = prevH - margin;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW - m2, prevH - m3] == 0)
                        {
                            wIndex = prevW - m2;
                            hIndex = prevH - m3;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW - margin, prevH - m3] == 0)
                        {
                            wIndex = prevW - margin;
                            hIndex = prevH - m3;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }
                    }

                    if (down)
                    {
                        if (grid[prevW - m3, prevH + m3] == 0)
                        {
                            wIndex = prevW - m3;
                            hIndex = prevH + m3;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW - m3, prevH + m2] == 0)
                        {
                            wIndex = prevW - m3;
                            hIndex = prevH + m2;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW - m3, prevH + margin] == 0)
                        {
                            wIndex = prevW - m3;
                            hIndex = prevH + margin;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW - m2, prevH + m3] == 0)
                        {
                            wIndex = prevW - m2;
                            hIndex = prevH + m3;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }

                        if (grid[prevW - margin, prevH + m3] == 0)
                        {
                            wIndex = prevW - margin;
                            hIndex = prevH + m3;
                            if (CheckCollision(grid, prevW, prevH, wIndex, hIndex) == false)
                            {
                                return new Tuple<int, int>(wIndex, hIndex);
                            }
                        }
                    }
                }
            }

            #endregion

            //If not returned by now, then no option found so return null
            return null;
        }

        public void GetDrawData()
        {
            //Clear shapes list & lines list
            shapes = new List<Box>();
            lines = new List<Line>();

            //split up the form into a grid, sized based on the screensize. 
            //Then I can place everything based on the grid

            //Create a new clear grid
            int[,] grid = new int[sizeFactor, sizeFactor];

            margin = 2;

            //Initialise the width & height variables for the rectangles
            w = pictureBox1.Width / sizeFactor;
            h = pictureBox1.Height / sizeFactor;

            int centre = sizeFactor / 2;

            Point exactCentre = new Point((centre * w) + w / 2, (centre * h) + h / 2);

            //Create rectangle variable
            Rectangle r = new Rectangle(centre * w, centre * h, w, h);

            //Mark that grid space as filled
            grid[centre, centre] = 1;

            Box box = new Box(r, mainColour, main.name, main.description);

            shapes.Add(box);

            txtMain.Text = main.name + " - Spider Diagram";

            int h2Count = main.children.Count;

            for (int i = 0; i < h2Count; i++)
            {
                Header h2 = main.children[i];

                Tuple<int, int> indexes = GetIndex(grid, centre, centre, 0, 0);

                if (indexes == null)
                {
                    Debug.WriteLine("H2: " + h2.name + " ignored...");
                    //ignore this header if it cannot fit
                    continue;
                }

                int wIndex = indexes.Item1;
                int hIndex = indexes.Item2;

                Color col = colours[i];

                Rectangle rect = new Rectangle(wIndex * w, hIndex * h, w, h);
                shapes.Add(new Box(rect, col, h2.name, h2.description));

                Line l = new Line(exactCentre, new Point((wIndex * w) + w / 2, (hIndex * h) + h / 2), col);
                lines.Add(l);

                grid[wIndex, hIndex] = 1;
            }

            for (int i = 0; i < h2Count; i++)
            {
                Header h2 = main.children[i];

                int h3Count = h2.children.Count;

                Box parent = null;

                foreach (Box b in shapes)
                {
                    if (b.header == h2.name && b.text == h2.description)
                    {
                        parent = b;

                        Debug.WriteLine(h2.name + " = " + b.header);
                    }
                }

                if (parent == null)
                {
                    continue;
                }

                int h2W = parent.rectangle.X / w;
                int h2H = parent.rectangle.Y / h;

                int xDiff = h2W - centre;
                int yDiff = h2H - centre;

                for (int j = 0; j < h3Count; j++)
                {
                    Header h3 = h2.children[j];

                    Tuple<int, int> indexes = GetIndex(grid, h2W, h2H, xDiff, yDiff);

                    if (indexes == null)
                    {
                        Debug.WriteLine("H3: " + h3.name + " ignored...");
                        //ignore this header if it cannot fit
                        continue;
                    }

                    int wIndex = indexes.Item1;
                    int hIndex = indexes.Item2;

                    Debug.WriteLine(wIndex + ", " + hIndex);

                    Rectangle rect = new Rectangle(wIndex * w, hIndex * h, w, h);
                    shapes.Add(new Box(rect, parent.col, h3.name, h3.description));

                    Line l = new Line(new Point((h2W * w) + w / 2, (h2H * h) + h / 2), new Point((wIndex * w) + w / 2, (hIndex * h) + h / 2), parent.col);
                    lines.Add(l);

                    grid[wIndex, hIndex] = 1;
                }
            }

            for (int i = 0; i < h2Count; i++)
            {
                Header h2 = main.children[i];

                int h3Count = h2.children.Count;

                for (int j = 0; j < h3Count; j++)
                {
                    Header h3 = h2.children[j];

                    int h4Count = h3.children.Count;

                    Box parent = null;

                    foreach (Box b in shapes)
                    {
                        if (b.header == h3.name && b.text == h3.description)
                        {
                            parent = b;
                        }
                    }

                    if (parent == null)
                    {
                        Debug.WriteLine("no parent found for " + h3.name);
                        continue;
                    }

                    int h3W = parent.rectangle.X / w;
                    int h3H = parent.rectangle.Y / h;

                    int xDiff = h3W - centre;
                    int yDiff = h3H - centre;

                    for (int k = 0; k < h4Count; k++)
                    {
                        Header h4 = h3.children[k];

                        Tuple<int, int> indexes = GetIndex(grid, h3W, h3H, xDiff, yDiff);

                        if (indexes == null)
                        {
                            Debug.WriteLine("H4: " + h4.name + " ignored...");
                            //ignore this header if it cannot fit
                            continue;
                        }

                        int wIndex = indexes.Item1;
                        int hIndex = indexes.Item2;

                        Rectangle rect = new Rectangle(wIndex * w, hIndex * h, w, h);
                        shapes.Add(new Box(rect, parent.col, h4.name, h4.description));

                        Line l = new Line(new Point((h3W * w) + w / 2, (h3H * h) + h / 2), new Point((wIndex * w) + w / 2, (hIndex * h) + h / 2), parent.col);
                        lines.Add(l);

                        grid[wIndex, hIndex] = 1;
                    }
                }
            }
        }

        //Edits the headers and fixes the issues
        public void RemoveUncheckedHeaders()
        {
            List<Header> tmpChildren = main.children;

            //Resetting list, so as not to mess up original data
            main.children = new List<Header>();

            if (tmpChildren.Count > 0)
            {
                //Removing any unchecked h2 headers
                for (int i = 0; i < tmpChildren.Count; i++)
                {
                    Header tmpH2 = tmpChildren[i];

                    //Copy data from h2 so we don't edit the actual values of the header, which will destroy previous form data
                    Header h2 = new Header(tmpH2.index) { children = tmpH2.children, name = tmpH2.name, description = tmpH2.description, isChecked = tmpH2.isChecked, parent = tmpH2.parent };

                    if (h2.isChecked == true)
                    {
                        Debug.WriteLine("Adding h2 '" + h2.name);
                        main.children.Add(h2);
                    }
                }
            }

            //Removing any unchecked h3 headers from the h2s still remaining
            for (int i = 0; i < main.children.Count; i++)
            {
                Header h2 = main.children[i];

                tmpChildren = h2.children;

                if (tmpChildren.Count > 0)
                {
                    h2.children = new List<Header>();

                    for (int j = 0; j < tmpChildren.Count; j++)
                    {
                        Header tmpH3 = tmpChildren[j];

                        //Copy data from h3 so we don't edit the actual values of the header, which will destroy previous form data
                        Header h3 = new Header(tmpH3.index) { children = tmpH3.children, name = tmpH3.name, description = tmpH3.description, isChecked = tmpH3.isChecked, parent = tmpH3.parent };

                        if (h3.isChecked == true)
                        {
                            Debug.WriteLine("Adding h3 '" + h3.name);
                            h2.children.Add(h3);
                        }
                    }
                }
            }

            //Removing any unchecked h4 headers from the h3s still remaining
            for (int i = 0; i < main.children.Count; i++)
            {
                Header h2 = main.children[i];

                for (int j = 0; j < h2.children.Count; j++)
                {
                    Header h3 = h2.children[j];

                    tmpChildren = h3.children;

                    if (tmpChildren.Count > 0)
                    {
                        h3.children = new List<Header>();

                        for (int k = 0; k < tmpChildren.Count; k++)
                        {
                            Header tmpH4 = tmpChildren[k];

                            //Copy data from h4 so we don't edit the actual values of the header, which will destroy previous form data
                            Header h4 = new Header(tmpH4.index) { children = null, name = tmpH4.name, description = tmpH4.description, isChecked = tmpH4.isChecked, parent = tmpH4.parent };

                            if (h4.isChecked == true)
                            {
                                Debug.WriteLine("Adding h4 '" + h4.name);
                                h3.children.Add(h4);
                            }
                        }
                    }
                }
            }
        }

        private Font FindBestFitFont(Graphics g, String text, Font font, Size proposedSize)
        {
            // Compute actual size, shrink if needed
            while (true)
            {
                string[] words = text.Split(' ');

                string newText = "";

                if (words.Length > 1)
                {
                    string line = "";

                    foreach (string word in words)
                    {
                        string tmpLine = line.Trim() + " " + word.Trim();
                        SizeF tmpSize = g.MeasureString(tmpLine, font);

                        if (tmpSize.Width < proposedSize.Width)
                        {
                            line = tmpLine;
                        }
                        else
                        {
                            if (line != "")
                            {
                                newText += line.Trim() + Environment.NewLine;
                            }

                            line = word.Trim();
                        }
                    }

                    if (line.Length > 0)
                    {
                        newText += line.Trim();
                    }
                }
                else
                {
                    newText = text;
                }

                SizeF size = g.MeasureString(newText, font);

                //Debug.WriteLine("size of '" + newText + "' = " + size + " | size of box = " + proposedSize);

                // It fits, back out
                if (size.Height <= proposedSize.Height &&
                     size.Width <= proposedSize.Width) { return font; }

                // Try a smaller font (90% of old size)
                Font oldFont = font;
                font = new Font(font.Name, (float)(font.Size * .9), font.Style);
                oldFont.Dispose();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //Get the variables for font size
            float width = ((float)this.ClientRectangle.Width);
            float height = ((float)this.ClientRectangle.Width);

            float emSize = height;
            float lineThickness = emSize / 500;

            foreach (Line l in lines)
            {
                e.Graphics.DrawLine(new Pen(l.Colour, emSize / 500), l.Start, l.End);
            }

            foreach (Box b in shapes)
            {
                using (SolidBrush brush = new SolidBrush(b.col))
                {
                    e.Graphics.FillRectangle(brush, b.rectangle);
                }
                using (Pen pen = new Pen(Color.Black, lineThickness))
                {
                    e.Graphics.DrawRectangle(pen, b.rectangle);
                }

                Font font = new Font("Arial", emSize, FontStyle.Bold);
                font = FindBestFitFont(e.Graphics, b.header, font, b.rectangle.Size);

                //Get luminance value for the colour
                double Y = 0.2126 * b.col.R + 0.7152 * b.col.G + 0.0722 * b.col.B;

                //Choose text colour between black or white depending on luminance (we want black for high luminescence and white for low)
                Color textCol = Y < 128 ? Color.White : Color.Black;

                e.Graphics.DrawString(b.header, font, new SolidBrush(textCol), b.rectangle);

                //e.Graphics.DrawString(b.text, new Font("Arial", textSize, FontStyle.Bold), new SolidBrush(Color.Black), b.rectangle.Location);
            }
        }

        private void frmResult_SizeChanged(object sender, EventArgs e)
        {
            GetDrawData();
            pictureBox1.Refresh();
        }

        private void btnReRollCol_Click(object sender, EventArgs e)
        {
            //Reset colour list
            colours = new List<Color>();

            for (int i = 0; i < h2Count; i++)
            {
                Color randCol = GetRandColor();

                //Make sure we do not get any duplicates
                if (colours.Contains(randCol))
                {
                    while (colours.Contains(randCol))
                    {
                        randCol = GetRandColor();
                    }
                }

                colours.Add(randCol);
            }

            GetDrawData();
            pictureBox1.Refresh();
        }

        //Save diagram as image
        private void btnSave_Click(object sender, EventArgs e)
        {
            //Get user to select file name 
            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                string path = saveFileDialog1.FileName;
                Debug.WriteLine("File name = '" + path + "'");

                //Double check the path ends in png
                if (!path.EndsWith(".png"))
                {
                    MessageBox.Show("Please choose png file format for the image. No other formats available.");
                    return;
                }

                try
                {
                    //Get bitmap of the area of the form we want (picturebox) and save to file
                    Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    pictureBox1.DrawToBitmap(bmp, new Rectangle(Point.Empty, bmp.Size));
                    bmp.Save(path, ImageFormat.Png);

                    DialogResult r = MessageBox.Show("Save successful. Do you want to view the image file?", "Save Image", MessageBoxButtons.YesNoCancel);

                    if (r == DialogResult.Yes)
                    {
                        //Open the file
                        if (File.Exists(path))
                        {
                            Process.Start(path);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Save failed due to error: " + ex.Message);
                }
            }
        }
    }
}
