using System;

namespace WebScanner
{
    class Node
    {
        public Uri Url { get; set; }
        public NodeState State { get; set; }
        public string ErrorMsg { get; set; }

        public int Level { get; set; }

        public Node(Uri url, int level)
        {
            Url = url;
            Level = level;
        }
    }

}
