using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace WikipediaWordDiagram
{
    public partial class frmEditData : Form
    {
        public Header main;

        public frmEditData(Header main)
        {
            InitializeComponent();

            this.main = main;

            FillList(main, listView1);
        }

        public void FillList(Header parent, ListView list)
        {
            List<Header> subHeaders = parent.children;

            foreach (Header h in subHeaders)
            {
                ListViewItem item = new ListViewItem(h.name);
                item.Checked = h.isChecked;
                list.Items.Add(item);

                Debug.WriteLine("added item " + h.name + " index " + h.index + " at index no. " + item.Index);
            }
        }

        public Header FindHeader(int h2Index, int h3Index, int h4Index)
        {
            if (h4Index == -1 & h3Index == -1 && h2Index >= 0)
            {
                foreach (Header h2 in main.children)
                {
                    if (h2.index == h2Index)
                    {
                        return h2;
                    }
                }
            }
            else if (h4Index == -1 && h3Index >= 0 && h2Index >= 0)
            {
                foreach (Header h2 in main.children)
                {
                    if (h2.index == h2Index)
                    {
                        foreach (Header h3 in h2.children)
                        {
                            if (h3.index == h3Index)
                            {
                                return h3;
                            }
                        }
                    }
                }
            }
            else if (h4Index >= 0 && h3Index >= 0 && h2Index >= 0)
            {
                foreach (Header h2 in main.children)
                {
                    if (h2.index == h2Index)
                    {
                        foreach (Header h3 in h2.children)
                        {
                            if (h3.index == h3Index)
                            {
                                foreach (Header h4 in h3.children)
                                {
                                    if (h4.index == h4Index)
                                    {
                                        return h4;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            listView3.Items.Clear();

            txtDesc1.Text = "";
            txtDesc2.Text = "";
            txtDesc3.Text = "";

            //Show all of the data in list 2
            if (listView1.SelectedItems.Count > 0)
            {
                Header h = FindHeader(listView1.SelectedItems[0].Index, -1, -1);

                if (h != null)
                {
                    FillList(h, listView2);

                    txtDesc1.Text = h.description;
                    txtDesc2.Text = "";
                    txtDesc3.Text = "";
                }
                else
                {
                    MessageBox.Show("Couldn't find item '" + listView1.SelectedItems[0].Name + "'");
                }
            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView3.Items.Clear();

            txtDesc2.Text = "";
            txtDesc3.Text = "";

            //Show all of the data in list 3
            if (listView2.SelectedItems.Count > 0)
            {
                Header h = FindHeader(listView1.SelectedItems[0].Index, listView2.SelectedItems[0].Index, -1);

                if (h != null)
                {
                    FillList(h, listView3);

                    txtDesc2.Text = h.description;
                    txtDesc3.Text = "";
                }
                else
                {
                    MessageBox.Show("Couldn't find item '" + listView2.SelectedItems[0].Name + "'");
                }
            }
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                Header h = FindHeader(listView1.SelectedItems[0].Index, listView2.SelectedItems[0].Index, listView3.SelectedItems[0].Index);

                if (h != null)
                {
                    txtDesc3.Text = h.description;
                }
                else
                {
                    MessageBox.Show("Couldn't find item '" + listView3.SelectedItems[0].Name + "'");
                }
            }
            else
            {
                txtDesc3.Text = "";
            }
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            Header h = FindHeader(e.Item.Index, -1, -1);
            if (h != null)
            {
                h.isChecked = e.Item.Checked;
            }
            else
            {
                MessageBox.Show("Couldn't find item '" + e.Item.Name + "'");
            }
        }

        private void listView2_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            Header h = FindHeader(listView1.SelectedItems[0].Index, e.Item.Index, -1);
            if (h != null)
            {
                h.isChecked = e.Item.Checked;
            }
            else
            {
                MessageBox.Show("Couldn't find item '" + e.Item.Name + "'");
            }
        }

        private void listView3_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            Header h = FindHeader(listView1.SelectedItems[0].Index, listView2.SelectedItems[0].Index, e.Item.Index);
            if (h != null)
            {
                h.isChecked = e.Item.Checked;
            }
            else
            {
                MessageBox.Show("Couldn't find item '" + e.Item.Name + "'");
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Header copy = new Header(main.index) { children = main.children, name = main.name, description = main.description, isChecked = main.isChecked, parent = main.parent };

            frmSpiderDiagram result = new frmSpiderDiagram(copy);
            result.ShowDialog();
        }
    }
}
