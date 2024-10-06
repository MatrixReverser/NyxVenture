using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NyxVenture.datamodel
{
    /// <summary>
    /// The base class for all models. Provides basic functionality in form of the
    /// event handling.
    /// </summary>
    public class ModelBase : INotifyPropertyChanged
    {
        public bool IsObjectChanged { get; private set; } = false;
        
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
        protected void OnPropertyChanged(string propertyName)
        {
            IsObjectChanged = true;            
            PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, args); 
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
                OnPropertyChanged(nameof(property));
            }
        }

        /// <summary>
        /// Cleans all changed flags of this object. After calling this method it
        /// looks like the object and the underlying objects have not been changed
        /// </summary>
        public void CleanChangedFlags()
        {
            IsObjectChanged = false;
        }
    }
}
