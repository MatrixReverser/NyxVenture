using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NyxVenture.datamodel
{
    /// <summary>
    /// The base class for all models. Provides basic functionality in form of the
    /// event handling.
    /// </summary>
    public abstract class ModelBase : INotifyPropertyChanged
    {
        public bool IsObjectChanged { get; private set; } = false;
        public bool IsModelChanged { get; private set; } = false;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        public event BubbleChangeEventHander? ModelChanged;

        /// <summary>
        /// Constructor of this class
        /// </summary>
        public ModelBase() 
        {
            IsObjectChanged = false;
        }

        /// <summary>
        /// Informs all listeners of the PropertyChanged event about a
        /// change of a property
        /// </summary>
        /// <param name="propertyName">Name of the property</param>        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            IsObjectChanged = true;
            IsModelChanged = true;
            PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, args); 
        }        

        /// <summary>
        /// Informs all listeners of the ModelChanged event about
        /// a change of a property in the hierarchy of objects
        /// </summary>
        protected virtual void OnModelChanged(BubbleChangeEventArgs args)
        {
            IsModelChanged = true;
            args.AddNodeToPath(this);
            ModelChanged?.Invoke(args);
        }

        /// <summary>
        /// Sets an property and fires appropriate events
        /// </summary>
        /// <param name="property">property to be set</param>
        /// <param name="value">value to be set</param>
        protected void SetProperty<T>(ref T? property, T? value, [CallerMemberName] string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(property, value))
            {                
                property = value;
                
                OnPropertyChanged(propertyName);

                PropertyChangedEventArgs propertyChangedArgs = new PropertyChangedEventArgs(propertyName);
                OnModelChanged(new BubbleChangeEventArgs(propertyChangedArgs, this));
            }
        }

        /// <summary>
        /// Registers a model object as a sub object, so that this object is
        /// informed about changes in the subnode and can bubble the event
        /// </summary>
        /// <param name="subnode">The subnode to be registered</param>
        protected void RegisterSubnode(ModelBase subnode)
        {
            subnode.ModelChanged += OnModelChanged;            
        }

        /// <summary>
        /// Unregisters a model object so that this object is no longer
        /// informed about changes in the subnode
        /// </summary>
        /// <param name="subnode"></param>
        protected void UnregisterSubnode(ModelBase subnode)
        {
            subnode.ModelChanged -= OnModelChanged;
        }

        /// <summary>
        /// Cleans the object changed flag of this object. 
        /// </summary>
        public void CleanObjectChangedFlag()
        {
            IsObjectChanged = false;
        }

        /// <summary>
        /// Cleans the model changed flag of this object. 
        /// </summary>
        public void CleanModelChangedFlag()
        {
            IsModelChanged = false;
        }

        /// <summary>
        /// Recursivley cleans all Model changed flags and ObjectChange flags of the
        /// current object and all its child nodes
        /// </summary>
        public abstract void CleanChangedFlags();
    }
}
