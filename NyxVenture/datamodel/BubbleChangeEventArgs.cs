using System.ComponentModel;

namespace NyxVenture.datamodel
{
    /// <summary>
    /// This class belongs to the customized event system of this application. 
    /// It contains information about the changed property of a model object.
    /// It is bubbled up to the root node and with each node it passes,
    /// the path inside is updated.
    /// </summary>
    public class BubbleChangeEventArgs : EventArgs
    {
        public PropertyChangedEventArgs PropertyInformation { get; private set; }
        public List<ModelBase> PathInformation { get; private set; }

        /// <summary>
        /// Constructor of the class BubbleCHangeEventArgs
        /// </summary>
        /// <param name="propertyInformation">Info about the PropertyChangeEvent that is the origin of this event</param>
        /// <param name="origin">The model object that has caused this event</param>
        public BubbleChangeEventArgs(PropertyChangedEventArgs propertyInformation, ModelBase origin)
        {
            PropertyInformation = propertyInformation;
            PathInformation = new List<ModelBase>();
        }

        /// <summary>
        /// Adds a new node to the PathInformation. 
        /// </summary>
        /// <param name="node">The model node to be added</param>
        public void AddNodeToPath(ModelBase node)
        {
            PathInformation.Insert(0, node);            
        }
    }
}
