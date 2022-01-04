using System.Collections.Generic;

namespace WikipediaWordDiagram
{
    public class Header
    {
        //Link to other items
        public Header parent = null;
        public List<Header> children;

        //Info
        public int index;
        public string name;
        public string description;

        //Checkbox value on if the user wants to include the header, true as default for all
        public bool isChecked = true;

        public Header(int index)
        {
            this.index = index;
            children = new List<Header>();
        }

    }
}
