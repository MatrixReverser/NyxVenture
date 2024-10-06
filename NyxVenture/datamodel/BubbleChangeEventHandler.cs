using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyxVenture.datamodel
{
    /// <summary>
    /// This delegate handles events of the datamodel that bubble from the origin 
    /// in the direction of the root node (Game).
    /// </summary>
    /// <param name="bubbleChangeEvent">Information about this event</param>
    public delegate void BubbleChangeEventHander(BubbleChangeEventArgs bubbleChangeEvent);
}
