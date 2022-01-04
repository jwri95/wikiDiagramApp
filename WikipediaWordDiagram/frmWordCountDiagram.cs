using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;
using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace WikipediaWordDiagram
{
    public partial class frmWordCountDiagram : Form
    {
        //variables
        int maxWordCount = 100;
        bool includeTitleWord = true;
        bool includeHeader = true;

        Random rnd = new Random();
        public Header main;

        //List of the words to draw
        List<Word> words = new List<Word>();

        List<Box> shapes = new List<Box>();

        Rectangle header;

        public Color mainColour = Color.White;

        public WordDictionary oDict;
        public static Spelling oSpell;

        private string stopWordStrings = "";
        public List<string> stopWords = new List<string>();

        int totalWidth = 0;
        int totalHeight = 0;
        int margin = 10;

        public frmWordCountDiagram(Header main, bool includeTitleWord, bool includeHeader)
        {
            InitializeComponent();

            Cursor = Cursors.WaitCursor;

            //Set the stop word to the resource file
            stopWordStrings = Properties.Resources.stop_words_english_new;

            this.includeTitleWord = includeTitleWord;
            this.includeHeader = includeHeader;

            //Resizes the form to fit the screen size
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            pictureBox1.Refresh();

            this.main = main;

            oDict = new WordDictionary();
            oDict.DictionaryFile = "en-GB.dic";
            oDict.Initialize();

            oSpell = new Spelling();
            oSpell.Dictionary = oDict;

            GetStopWords();

            GetDrawData();

            DrawWords();

            Cursor = Cursors.Arrow;
        }

        //Gets the data (words) to draw
        public void GetDrawData()
        {
            //Draw main header at the top
            txtMain.Text = main.name + " - Word Frequency Diagram";

            //Get the list of strings from the data
            List<string> strings = GetWordsFromData();

            //get the most common words and organise them in order of frequency
            words = GetMostCommonWords(strings);

            //Check if there are more words than the MAX word count
            if (words.Count > maxWordCount)
            {
                //Remove all the excess words
                words = words.GetRange(0, maxWordCount);
            }

            foreach (Word w in words)
            {
                Debug.WriteLine(w.word + " | " + w.count);
            }
        }

        //method to draw each word on the screen
        public void DrawWords()
        {
            //Clear shapes list 
            shapes = new List<Box>();

            int totalFreq = 0;

            foreach (Word word in words)
            {
                totalFreq += word.count;
            }

            if (totalFreq == 0)
            {
                MessageBox.Show("No words to draw");
                return;
            }

            Debug.WriteLine("Total freq = " + totalFreq);

            //If include header is true then add it in
            if (includeHeader == true)
            {
                header = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height / 20);
                shapes.Add(new Box(header, Color.LightGray, main.name + " - Word Frequency Diagram", "MAIN"));
            }

            Debug.WriteLine("Total space = (" + pictureBox1.Width + ", " + pictureBox1.Height + ")");

            //Set the total width & height (can change this to a smaller fraction of the picturebox if needed)
            totalWidth = pictureBox1.Width - (margin * 2);
            totalHeight = pictureBox1.Height - (margin * 2) - header.Height;

            Debug.WriteLine("Total space given = (" + totalWidth + ", " + totalHeight + ")");

            //double formArea = pictureBox1.Width * pictureBox1.Height;
            double formArea = totalWidth * totalHeight;

            //Set unit area to some fraction of the actual unit area to make up for the fact there will be space inbetween the shapes that cannot be filled
            double unitArea = (0.65 * formArea / totalFreq);

            int cumulativeArea = 0;

            for (int i = 0; i < words.Count; i++)
            {
                //we can also try packing them as circles into a square and working backwards from that, but not sure how that works for different sized shapes 

                Word word = words[i];

                //Work out the area for this word depending on word count, round down to make sure it fits in the space
                int area = (int)Math.Floor(word.count * unitArea);

                cumulativeArea += area;

                //width & height of the square will be square root of the area, round down to make sure it fits in the space
                int width = (int)Math.Floor(Math.Sqrt(area));

                //Debug.WriteLine(word.word + ": width = " + width + ", area = " + area);

                Rectangle r = new Rectangle(margin, margin, width, width);

                Point pos = GetBestPosition(r);

                //Skip this one if we cannot find a position for it
                if (pos.X == -1 && pos.Y == -1)
                {
                    Debug.WriteLine("Position was null for " + word.word);
                    continue;
                }

                r.X = pos.X;
                r.Y = pos.Y;

                Debug.WriteLine(word.word + " pos = " + r.X + ", " + r.Y);

                Box box = new Box(r, GetRandColour(), word.word, "");
                shapes.Add(box);
            }

            Debug.WriteLine("Total area = " + cumulativeArea + ", total space = " + formArea);
        }

        public Point GetBestPosition(Rectangle r)
        {
            //Debug.WriteLine("original pos = " + r.X + ", " + r.Y);

            //NEED TO MAKE SURE WE DO NOT GO INTO AN INFINITE LOOP IF WE CANNOT FIND A SPACE
            //try 100-100000 random pos for each word? if it cannot be found then give up
            int maxTries = 100000;
            int counter = 0;

            int maxX = margin + totalWidth - r.Width;
            int maxY = header.Height + margin + totalHeight - r.Height;

            if (maxX < 0 || maxY < 0)
            {
                Debug.WriteLine("no valid location for rectangle as maxX = " + maxX + " and maxY = " + maxY);
            }

            //Choose random x & y
            r.X = rnd.Next(margin, maxX);
            r.Y = rnd.Next(margin + header.Height, maxY);

            //see if it collides with anything, if not then go for it
            while (CheckCollision(r) && counter < maxTries)
            {
                //Choose random x & y
                r.X = rnd.Next(margin, maxX);
                r.Y = rnd.Next(margin + header.Height, maxY);

                counter++;
            }

            //if we have exceeded the max tries then ignore it
            if (counter >= maxTries)
            {
                Debug.WriteLine("count not find a place for word after " + maxTries + " tries");
                //No point found in the form so return null 
                return new Point(-1, -1);
            }
            //Otherwise return the valid rectangle point
            else
            {
                return new Point(r.X, r.Y);
            }
        }

        //Check collision between the rectangle and all the other shapes that are in place so far
        public bool CheckCollision(Rectangle r)
        {
            foreach (Box b in shapes)
            {
                //if intersection occurs then return true
                if (Rectangle.Intersect(r, b.rectangle) != Rectangle.Empty)
                {
                    if (Rectangle.Intersect(r, b.rectangle).Width > 0 && Rectangle.Intersect(r, b.rectangle).Height > 0)
                    {
                        //Debug.WriteLine("collision between " + r.X + ", " + r.Y + " and " + b.header + ": " + b.rectangle.X + ", " + b.rectangle.Y);
                        return true;
                    }
                }
            }

            //if no intersections then return false
            return false;
        }

        public List<string> GetWordsFromData()
        {
            //Create list of strings
            List<string> strings = new List<string>();

            //Get all h1 strings
            List<string> h1Strings = ExtractWordsFromHeader(main);

            if (h1Strings != null)
            {
                if (h1Strings.Count > 0)
                {
                    strings.AddRange(h1Strings);
                }
            }

            //Get all h2 strings, if they exist
            for (int i = 0; i < main.children.Count; i++)
            {
                Header h2 = main.children[i];

                List<string> h2Strings = ExtractWordsFromHeader(h2);

                if (h2Strings != null)
                {
                    if (h2Strings.Count > 0)
                    {
                        strings.AddRange(h2Strings);
                    }
                }

                //Get all h3 strings, if they exist
                for (int j = 0; j < h2.children.Count; j++)
                {
                    Header h3 = h2.children[j];

                    List<string> h3Strings = ExtractWordsFromHeader(h3);

                    if (h3Strings != null)
                    {
                        if (h3Strings.Count > 0)
                        {
                            strings.AddRange(h3Strings);
                        }
                    }

                    //Get all h4 strings, if they exist
                    for (int k = 0; k < h3.children.Count; k++)
                    {
                        Header h4 = h3.children[k];

                        List<string> h4Strings = ExtractWordsFromHeader(h4);

                        if (h4Strings != null)
                        {
                            if (h4Strings.Count > 0)
                            {
                                strings.AddRange(h4Strings);
                            }
                        }
                    }
                }
            }

            return strings;
        }

        public List<string> ExtractWordsFromHeader(Header header)
        {
            List<string> words = new List<string>();

            if (string.IsNullOrEmpty(header.name) == false)
            {
                string[] hWords = header.name.Split(' ');

                if (hWords.Length > 0)
                {
                    words.AddRange(hWords);
                }
            }

            if (string.IsNullOrEmpty(header.description) == false)
            {
                string[] pWords = header.description.Split(' ');

                if (pWords.Length > 0)
                {
                    words.AddRange(pWords);
                }
            }

            return words;
        }

        public List<Word> GetMostCommonWords(List<string> strings)
        {
            List<Word> words = new List<Word>();

            foreach (string word in strings)
            {
                //Validate and format the word correctly, make sure it has no punctuation or anything (just the letters/numbers) and make sure we check for plurals
                string s = FormatString(word);

                //If the formatted string is null, then it is not valid, so continue to the next word and ignore this one
                if (s == null)
                {
                    continue;
                }

                //Check the validity of the word 
                if (IsValid(s) == false)
                {
                    continue;
                }

                //Checking if the word is a stop word
                bool isStopWord = false;
                foreach (string stopword in stopWords)
                {
                    if (s.ToLower().Trim() == stopword.ToLower().Trim())
                    {
                        isStopWord = true;
                    }
                }

                //If it is a stop word then move on to the next string (ignore it)
                if (isStopWord)
                {
                    continue;
                }

                //Now check if the string is plural, if so it will return it as singular
                s = Singular(s);

                //Check the validity of the word again (some errors are made by the singular function that create invalid words)
                if (IsValid(s) == false)
                {
                    continue;
                }

                bool exists = false;

                //Checking if the word already exists, if so add to the frequency, otherwise create the word
                if (words.Count > 0)
                {
                    foreach (Word w in words)
                    {
                        if (w.word == s)
                        {
                            w.count++;
                            exists = true;
                        }
                    }

                    if (exists == false)
                    {
                        Word w2 = new Word(s, 1);
                        words.Add(w2);
                    }
                }
                else
                {
                    Word w2 = new Word(s, 1);
                    words.Add(w2);
                }
            }

            bool changes = true;

            while (changes == true)
            {
                changes = false;

                //Organise list from most frequent to least freq
                for (int i = 0; i < words.Count; i++)
                {
                    if (i != words.Count - 1)
                    {
                        //Swap them round
                        if (words[i].count < words[i + 1].count)
                        {
                            Word tmpWord = words[i];

                            words[i] = words[i + 1];
                            words[i + 1] = tmpWord;
                            changes = true;
                        }
                    }
                }
            }

            return words;
        }

        //Method to remove plurals and only have singular words
        public string Singular(string str)
        {
            PluralizationService ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-GB"));

            if (ps.IsPlural(str))
            {
                //Debug.WriteLine(str + " is plural -> singular = '" + ps.Singularize(str) + "'");
                return ps.Singularize(str);
            }

            return str;
        }

        public bool IsValid(string str)
        {
            //Check if word is in english dictionary (if not, if it starts with a capital then assume it is a name)
            //If neither then get rid

            //Only accept 2 letter words or more ('a' is not a word)
            if (str.Length <= 1)
            {
                return false;
            }

            //First character is not upper case, so not a name
            if (char.IsUpper(str[0]) == false)
            {
                try
                {
                    //Also does not exist in dictionary, so not valid
                    if (!oSpell.TestWord(str))
                    {
                        //Now test if it is in the dictionary with a capital at the start (english isn't but English is)
                        str = str[0].ToString().ToUpper() + str.Substring(1, str.Length - 1);

                        if (!oSpell.TestWord(str))
                        {
                            //Debug.WriteLine("word '" + formatted + "' does not exist in the dictionary");

                            //Word does not exist in dictionary so return null
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    //Exception occurred so return null
                    return false;
                }
            }

            //Some key terms that pop up that we don't want as they are generated for every page
            if (str.ToUpper().Contains("WIKI") || str.ToUpper().Contains("MAIN") || str.ToUpper().Contains("CONTENTS") || str.ToUpper().Contains("MENU")
                || str.ToUpper().Contains("REFERENCE") || str.ToUpper().Contains("SEE") || str.ToUpper().Contains("ALSO") || str.ToUpper().Contains("NAVIGATION") || str.ToUpper().Contains("LINKS")
                || str.ToUpper().Contains("BIBLIOGRAPHY") || str.ToUpper().Contains("NOTES") || str.ToUpper().Contains("IMAGES") || str.ToUpper().Contains("TOOLS")
                || str.ToUpper().Contains("NAMESPACE") || str.ToUpper().Contains("VIEW") || str.ToUpper().Contains("SEARCH") || str.ToUpper().Contains("CONTRIBUTE")
                || str.ToUpper().Contains("PROJECTS") || str.ToUpper().Contains("LANGUAGES") || str.ToUpper().Contains("VARIANTS") || str.ToUpper().Contains("MORE"))
            {
                Debug.WriteLine("Word '" + str + "' is a common term for wikipedia, so will be ignored.");
                //We do not want any of these words as they are part of the wikipedia page
                return false;
            }

            //Checking if the word is the same as (or contained in) the title word if we do not want to include it in the count
            if (includeTitleWord == false)
            {
                if (main.name.Trim().ToLower().Contains(str.Trim().ToLower()))
                {
                    return false;
                }
            }

            //If we have got to this point, it has passed the tests so return true
            return true;
        }

        public string FormatString(string str)
        {
            string formatted = str.ToLower().Trim();

            foreach (char c in str.ToCharArray())
            {
                //If string has any characters other than letters/numbers or dashes, then remove them from the string
                if (!char.IsLetterOrDigit(c) & c != '-')
                {
                    //Debug.WriteLine("invalid character found '" + c + "'");
                    formatted = formatted.Replace(c, '#');
                }
            }

            //Debug.WriteLine("new string = '" + formatted + "'");

            //Remove all the unwanted chars
            formatted = formatted.Replace("#", "");

            //Debug.WriteLine("new string = '" + formatted + "'");

            //Check we still have valid characters to check, if not then return null
            if (formatted.Trim() == "")
            {
                Debug.WriteLine(str + " is null/empty");
                return null;
            }

            //return the final string
            return formatted;
        }

        //Method to get each individual stop word from the string
        public void GetStopWords()
        {
            //Clear the list
            stopWords = new List<string>();

            //Split string by each line
            string[] words = stopWordStrings.Split('\n');

            //Loop through each string in the array and remove the empty text then add to the list
            foreach (string word in words)
            {
                stopWords.Add(word.Trim());
            }
        }

        public Color GetRandColour()
        {
            int r = rnd.Next(Enum.GetValues(typeof(KnownColor)).Length);

            KnownColor randCol = (KnownColor)Enum.GetValues(typeof(KnownColor)).GetValue(r);

            return Color.FromKnownColor(randCol);
        }

        private Font FindBestFitFont(Graphics g, String text, Font font, Size proposedSize)
        {
            // Compute actual size, shrink if needed
            while (true)
            {
                SizeF size = g.MeasureString(text, font);

                //Debug.WriteLine("size of '" + text + "' = " + size + " | size of box = " + proposedSize);

                // It fits, back out
                if (size.Height <= proposedSize.Height &&
                     size.Width <= proposedSize.Width) { return font; }

                // Try a smaller font (90% of old size)
                Font oldFont = font;
                font = new Font(font.Name, (float)(font.Size * .9), font.Style);
                oldFont.Dispose();
            }
        }

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

        private void btnReRollCol_Click(object sender, EventArgs e)
        {
            foreach (Box b in shapes)
            {
                b.col = GetRandColour();
            }

            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //Get the variables for font size
            float height = ((float)this.ClientRectangle.Height);

            float emSize = height;
            float lineThickness = emSize / 500;

            foreach (Box b in shapes)
            {
                Font font = new Font("Segoe UI", emSize, FontStyle.Bold);

                //Do rectangle for main header
                if (b.text == "MAIN")
                {
                    using (SolidBrush brush = new SolidBrush(b.col))
                    {
                        e.Graphics.FillRectangle(brush, b.rectangle);
                    }
                    using (Pen pen = new Pen(Color.Black, 1))
                    {
                        e.Graphics.DrawRectangle(pen, b.rectangle);
                    }
                }
                //Do circle for everything else
                else
                {
                    using (SolidBrush brush = new SolidBrush(b.col))
                    {
                        //e.Graphics.FillRectangle(brush, b.rectangle);
                        e.Graphics.FillEllipse(brush, b.rectangle);
                    }
                    using (Pen pen = new Pen(Color.Black, (int)(b.rectangle.Width * 0.01f)))
                    {
                        //e.Graphics.DrawRectangle(pen, b.rectangle);
                        e.Graphics.DrawEllipse(pen, b.rectangle);
                    }

                    font = new Font("Garamond", emSize, FontStyle.Bold);
                }

                font = FindBestFitFont(e.Graphics, b.header, font, b.rectangle.Size);

                //Font font = new Font("Arial", 20, FontStyle.Bold);

                //Get luminance value for the colour
                double Y = 0.2126 * b.col.R + 0.7152 * b.col.G + 0.0722 * b.col.B;

                //Choose text colour between black or white depending on luminance (we want black for high luminescence and white for low)
                Color textCol = Y < 128 ? Color.White : Color.Black;

                int horPadding = (int)Math.Round((b.rectangle.Width - e.Graphics.MeasureString(b.header, font).Width) / 2);

                e.Graphics.DrawString(b.header, font, new SolidBrush(textCol), b.rectangle.Location.X + horPadding, (b.rectangle.Location.Y + b.rectangle.Height / 2 - font.Height / 2));

                //Debug.WriteLine("finished drawing " + b.header);

                //e.Graphics.DrawString(b.text, new Font("Arial", textSize, FontStyle.Bold), new SolidBrush(Color.Black), b.rectangle.Location);
            }

            //if (bounds != Rectangle.Empty)
            //{
            //    using (Pen pen = new Pen(Color.Red, 2f))
            //    {
            //        e.Graphics.DrawRectangle(pen, bounds);
            //    }
            //}
        }
    }
}
