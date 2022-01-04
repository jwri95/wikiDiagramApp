using HtmlAgilityPack;
using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace WikipediaWordDiagram
{
    public partial class frmExtractData : Form
    {
        Header mainHeader = null;

        public WordDictionary oDict;
        public static Spelling oSpell;

        public frmExtractData()
        {
            InitializeComponent();

            oDict = new WordDictionary();
            oDict.DictionaryFile = "en-GB.dic";
            oDict.Initialize();

            oSpell = new Spelling();
            oSpell.Dictionary = oDict;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //txtURL.Text = "https://en.wikipedia.org/wiki/War";
        }

        //Method for returning the name, description, and links with a wikipedia page
        public void GetData(string url)
        {
            string html = null;
            mainHeader = null;

            try
            {
                html = GetHTML(url);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to download the html file of '" + url + "' due to error: " + ex.ToString());
                return;
            }

            try
            {
                //Read the webpage
                mainHeader = GetInfo(html);

                if (mainHeader == null)
                {
                    Debug.WriteLine("No info found for webpage '" + url + "'");
                    return;
                }
                else if (mainHeader.children.Count == 0)
                {
                    Debug.WriteLine("No info found for webpage '" + url + "'");
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to read webpage '" + url + "' due to error: " + ex.ToString());
                return;
            }
        }

        public string GetHTML(string url)
        {
            string html = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "C# console client";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            return html;
        }


        //Gets the page name (first header) and description (first paragraph)
        //Also gets every sub header and links in each header/subheader
        public Header GetInfo(string html)
        {
            Header main = new Header(0);

            int h2Index = 0;
            int h3Index = 0;
            int h4Index = 0;

            try
            {
                // using the html agility pack: http://www.codeplex.com/htmlagilitypack
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                //This removes a lot of the html/js/css jargon, so we are left only with the text on the page
                doc.DocumentNode.Descendants()
                    .Where(n => n.Name == "script" || n.Name == "style")
                    .ToList()
                    .ForEach(n => n.Remove());

                //Getting the main header for the page
                string header = doc.DocumentNode.SelectNodes("//h1")[0].InnerText;
                Debug.WriteLine("Name = " + header);
                main.name = header;

                string prevH = "h1";
                Header sub = null;

                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//body"))
                {
                    //Loop through each node in the paragraph and check for any links <a> if so, then check they are in the correct format 
                    foreach (HtmlNode child in node.Descendants())
                    {
                        if (child.NodeType == HtmlNodeType.Element && child.Name == "h2")
                        {
                            string name = child.InnerText.Trim();

                            //Common thing for headings to have [edit] tag with them, so just remove that if so
                            if (name.Contains("[edit]"))
                            {
                                name = name.Replace("[edit]", "");
                            }

                            Debug.WriteLine("H2:" + name);

                            if (ValidateString(name))
                            {
                                if (sub != null)
                                {
                                    if (prevH == "h1" || prevH == "h2")
                                    {
                                        main.children.Add(sub);
                                    }
                                }

                                sub = new Header(h2Index) { name = name, parent = main };

                                Debug.WriteLine("H2: " + sub.name + " -> index " + sub.index);

                                //increment h2 index
                                h2Index++;

                                prevH = child.Name;
                            }
                        }
                        else if (child.NodeType == HtmlNodeType.Element && child.Name == "h3")
                        {
                            string name = child.InnerText.Trim();

                            //Common thing for headings to have [edit] tag with them, so just remove that if so
                            if (name.Contains("[edit]"))
                            {
                                name = name.Replace("[edit]", "");
                            }

                            Debug.WriteLine("H3:" + name);

                            if (ValidateString(name))
                            {
                                Header tmpSub = new Header(h3Index) { name = name };

                                if (sub != null && prevH == "h2")
                                {
                                    //reset h3 index
                                    h3Index = 0;
                                    tmpSub.index = h3Index;

                                    tmpSub.parent = sub;
                                    sub.children.Add(tmpSub);
                                    main.children.Add(sub);
                                }
                                else if (sub != null && prevH == "h3")
                                {
                                    Header h2 = sub.parent;
                                    tmpSub.parent = h2;
                                    h2.children.Add(tmpSub);
                                }
                                else if (sub != null && prevH == "h4")
                                {
                                    Header h3 = sub.parent;
                                    Header h2 = h3.parent;
                                    tmpSub.parent = h2;
                                    h2.children.Add(tmpSub);
                                }

                                Debug.WriteLine("H3: " + tmpSub.name + " -> index " + tmpSub.index);

                                //increment h3 index
                                h3Index++;

                                sub = tmpSub;

                                prevH = child.Name;
                            }
                        }
                        else if (child.NodeType == HtmlNodeType.Element && child.Name == "h4")
                        {
                            string name = child.InnerText.Trim();

                            //Common thing for headings to have [edit] tag with them, so just remove that if so
                            if (name.Contains("[edit]"))
                            {
                                name = name.Replace("[edit]", "");
                            }

                            Debug.WriteLine("H4:" + name);

                            if (ValidateString(name))
                            {
                                Header tmpSub = new Header(h4Index) { name = name };

                                if (sub != null && prevH == "h3")
                                {
                                    //reset h4 index
                                    h4Index = 0;
                                    tmpSub.index = h4Index;

                                    tmpSub.parent = sub;
                                    sub.children.Add(tmpSub);
                                }
                                else if (sub != null && prevH == "h4")
                                {
                                    Header h3 = sub.parent;
                                    tmpSub.parent = h3;
                                    h3.children.Add(tmpSub);
                                }

                                Debug.WriteLine("H4: " + tmpSub.name + " -> index " + tmpSub.index);

                                //increment h4 index
                                h4Index++;

                                sub = tmpSub;

                                prevH = child.Name;
                            }
                        }
                        else if (child.NodeType == HtmlNodeType.Element && child.Name == "p")
                        {
                            List<string> referenceText = new List<string>();

                            //Removing all the references from the text <sup>
                            foreach (HtmlNode grandchild in child.Descendants())
                            {
                                if (grandchild.NodeType == HtmlNodeType.Element && grandchild.Name == "sup")
                                {
                                    if (grandchild.InnerText.Length > 0)
                                    {
                                        //Debug.WriteLine(grandchild.InnerText);
                                        referenceText.Add(grandchild.InnerText);
                                    }
                                }
                            }

                            string text = child.InnerText.Trim();

                            if (string.IsNullOrEmpty(text) == false)
                            {
                                foreach (string reference in referenceText)
                                {
                                    if (text.Contains(reference))
                                    {
                                        text = text.Replace(reference, "");
                                    }
                                }

                                text = text.Replace("&nspb;", "");
                                text = text.Replace("&#160;", "");

                                //Debug.WriteLine(prevH + " paragraph: " + text);

                                if (prevH == "h1")
                                {
                                    //We only want to set the main description if it is empty, otherwise we have already collected it.
                                    if (string.IsNullOrEmpty(main.description))
                                    {
                                        main.description = text;
                                    }
                                    else
                                    {
                                        main.description += Environment.NewLine + text;
                                    }
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(sub.description))
                                    {
                                        sub.description = text;
                                    }
                                    else
                                    {
                                        sub.description += Environment.NewLine + text;
                                    }
                                }
                            }
                        }
                        //JW - ignoring the bullet point text for now, as a lot of it isn't relevant...
                        //else if (child.NodeType == HtmlNodeType.Element && child.Name == "ul")
                        //{
                        //    string text = child.InnerText.Trim();

                        //    if (string.IsNullOrEmpty(text) == false)
                        //    {
                        //        //Debug.WriteLine("UL text = " + text);

                        //        if (prevH == "h1")
                        //        {
                        //            //We only want to set the main description if it is empty, otherwise we have already collected it.
                        //            if (string.IsNullOrEmpty(main.description))
                        //            {
                        //                main.description = text;
                        //            }
                        //            else
                        //            {
                        //                main.description += Environment.NewLine + text;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (string.IsNullOrEmpty(sub.description))
                        //            {
                        //                sub.description = text;
                        //                //Debug.WriteLine("Setting " + sub.name + " description as '" + text + "'");
                        //            }
                        //            else
                        //            {
                        //                sub.description += Environment.NewLine + text;
                        //                //Debug.WriteLine("Setting " + sub.name + " description as '" + sub.description + "'");
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }

                if (sub != null)
                {
                    Debug.WriteLine(sub.name + ", prev h = " + prevH);

                    if (prevH == "h2")
                    {
                        main.children.Add(sub);
                    }
                }

                return main;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        //Method to check if string is valid
        public bool ValidateString(string str)
        {
            foreach (char c in str.ToCharArray())
            {
                //If string has any characters other than letters or underscores (_ used for spaces) then return false
                if (!char.IsLetter(c) & c != '_' & c != ' ' & c != '-')
                {
                    Debug.WriteLine("invalid character found '" + c + "'");
                    return false;
                }
            }

            //Firstly, check it is not similar to the start url word

            //If it is the exact same word
            if (txtURL.Text.Trim().ToLower() == str.Trim().ToLower())
            {
                Debug.WriteLine("same word as starting word");
                return false;
            }

            //If the user has typed a plural word but the returned word is singular
            if (txtURL.Text.Trim().ToLower() == (str.Trim().ToLower() + "s"))
            {
                Debug.WriteLine("same word as starting word");
                return false;
            }

            //If it is plural version of the word the user has typed in
            if ((txtURL.Text.Trim().ToLower() + "s") == (str.Trim().ToLower()))
            {
                Debug.WriteLine("same word as starting word");
                return false;
            }

            //Check if word is in english dictionary (if not, if it starts with a capital then assume it is a name)
            //If neither then get rid

            //First character is not upper case, so not a name
            if (char.IsUpper(str[0]) == false)
            {
                try
                {
                    //Also does not exist in dictionary, so not valid
                    if (!oSpell.TestWord(str))
                    {
                        Debug.WriteLine("word does not exist in the dictionary");
                        //Word does not exist in dictionary
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            //Some key terms that pop up that we don't want as they are generated for every page
            if (str.ToUpper().Contains("WIKI") || str.ToUpper().Contains("MAIN") || str.ToUpper().Contains("CONTENTS") || str.ToUpper().Contains("MENU")
                || str.ToUpper().Contains("REFERENCE") || str.ToUpper().Contains("SEE ALSO") || str.ToUpper().Contains("NAVIGATION") || str.ToUpper().Contains("LINKS")
                || str.ToUpper().Contains("BIBLIOGRAPHY") || str.ToUpper().Contains("NOTES") || str.ToUpper().Contains("IMAGES") || str.ToUpper().Contains("TOOLS")
                || str.ToUpper().Contains("NAMESPACE") || str.ToUpper().Contains("VIEW") || str.ToUpper().Contains("SEARCH") || str.ToUpper().Contains("CONTRIBUTE")
                || str.ToUpper().Contains("PROJECTS") || str.ToUpper().Contains("LANGUAGES") || str.ToUpper().Contains("VARIANTS") || str.ToUpper().Contains("MORE"))
            {
                return false;
            }

            //if it has passed all these tests then return true
            return true;
        }

        private void btnExtractData_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            //Make sure one option has been checked
            if (!checkBoxSpider.Checked && !checkBoxWordFreq.Checked)
            {
                MessageBox.Show("Error: Please select the diagram type from the checkbox options.", "Extract Data");
                Cursor = Cursors.Arrow;
                return;
            }

            //Checking URL is valid
            if (string.IsNullOrEmpty(txtURL.Text.Trim()))
            {
                MessageBox.Show("Please enter a wikipedia URL.");

                Cursor = Cursors.Default;
                return;
            }
            else
            {
                if (!txtURL.Text.ToUpper().Contains("WIKIPEDIA"))
                {
                    MessageBox.Show("Please enter a wikipedia URL.");

                    Cursor = Cursors.Default;
                    return;
                }
            }

            string url = txtURL.Text.Trim();

            Debug.WriteLine("URL=" + url);

            GetData(url);

            if (mainHeader != null)
            {
                if (mainHeader.children.Count == 0)
                {
                    MessageBox.Show("No data found, or not enough data, to generate the diagram.");
                    Cursor = Cursors.Default;
                    return;
                }

                Debug.WriteLine(mainHeader.name + " | " + mainHeader.description);
                foreach (Header h2 in mainHeader.children)
                {
                    Debug.WriteLine("h2: " + h2.parent.name + " -> " + h2.name);
                    //Debug.WriteLine(h2.description);

                    foreach (Header h3 in h2.children)
                    {
                        if (h3.children.Count == 0)
                        {
                            Debug.WriteLine("h3: " + h2.parent.name + " -> " + h3.parent.name + " -> " + h3.name);
                            //Debug.WriteLine(h3.description);
                        }

                        foreach (Header h4 in h3.children)
                        {
                            Debug.WriteLine("h4: " + h2.parent.name + " -> " + h3.parent.name + " -> " + h4.parent.name + " -> " + h4.name);
                            //Debug.WriteLine(h4.description);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No data found, or not enough data, to generate the diagram.");
                Cursor = Cursors.Default;
                return;
            }

            if (checkBoxSpider.Checked)
            {
                frmEditData editData = new frmEditData(mainHeader);
                editData.ShowDialog();
            }
            else if (checkBoxWordFreq.Checked)
            {
                frmWordCountDiagram wordCount = new frmWordCountDiagram(mainHeader, checkBoxIncludeTitleWord.Checked, checkBoxHeader.Checked);
                wordCount.ShowDialog();
            }
            Cursor = Cursors.Default;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtURL.Text = "";
        }

        private void checkBoxSpider_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSpider.Checked == true)
            {
                if (checkBoxWordFreq.Checked)
                {
                    checkBoxWordFreq.Checked = false;
                }
            }
        }

        private void checkBoxWordFreq_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxWordFreq.Checked == true)
            {
                if (checkBoxSpider.Checked)
                {
                    checkBoxSpider.Checked = false;
                }

                checkBoxIncludeTitleWord.Visible = true;
                checkBoxHeader.Visible = true;
            }
            else
            {
                checkBoxIncludeTitleWord.Visible = false;
                checkBoxHeader.Visible = false;
            }
        }
    }
}
