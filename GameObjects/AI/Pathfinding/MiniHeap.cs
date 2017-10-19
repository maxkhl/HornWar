using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horn_War_II.GameObjects.AI.Pathfinding
{
    /// <summary>
    /// MinHeap from ZeraldotNet (http://zeraldotnet.codeplex.com/)
    /// Modified by Roy Triesscheijn (http://roy-t.nl)    
    /// -Moved method variables to class variables
    /// -Added English Exceptions and comments (instead of Chinese)    
    /// </summary>    
    public class MinHeap
    {
        private Node listHead;

        public bool HasNext()
        {
            return listHead != null;
        }

        public void Add(Node item)
        {
            if (listHead == null)
            {
                listHead = item;
            }
            else if (listHead.next == null && item.cost <= listHead.cost)
            {
                item.nextListElem = listHead;
                listHead = item;
            }
            else
            {
                Node ptr = listHead;
                while (ptr.nextListElem != null && ptr.nextListElem.cost < item.cost)
                    ptr = ptr.nextListElem;
                item.nextListElem = ptr.nextListElem;
                ptr.nextListElem = item;
            }
        }

        public Node ExtractFirst()
        {
            Node result = listHead;
            listHead = listHead?.nextListElem;
            return result;
        }
    }
}
