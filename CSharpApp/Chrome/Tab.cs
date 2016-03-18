using System;

namespace Chrome
{
    [Serializable]
    public class Tab : IComparable
    {
        public bool active;
        public int index;
        public string url;
        public int windowId;

        public void Print()
        {
            Console.WriteLine(active);
            Console.WriteLine(index);
            Console.WriteLine(url);
            Console.WriteLine(windowId);
            Console.WriteLine();
        }

        public int CompareTo(object obj)
        {
            Tab tab = obj as Tab;
            if (tab.windowId == windowId)
            {
                return index.CompareTo(tab.index);
            }
            return windowId.CompareTo(tab.windowId);
        }
    }
}