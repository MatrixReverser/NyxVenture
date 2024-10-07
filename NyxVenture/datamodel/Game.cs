using System.Configuration;
using System.Runtime.Versioning;

namespace NyxVenture.datamodel
{
    /// <summary>
    /// The root class of a NyxVenture game
    /// </summary>
    public class Game : ModelBase
    {
        #region ------------------------- FIELDS ------------------------------
        private string? _title;
        private string? _description;
        private string? _author;
        private string? _genre;
        private List<Feature> _features; 
        private Chapter? _startChapter;
        #endregion

        #region ----------------------- PROPERTIES ----------------------------
        public string? Title { get => _title; set => SetProperty(ref _title, value); }
        public string? Description { get => _description; set => SetProperty(ref _description, value); }
        public string? Author { get => _author; set => SetProperty(ref _author, value); }
        public string? Genre { get => _genre; set => SetProperty(ref _genre, value); }        
        public Chapter? StartChapter { get => _startChapter; }
        public Feature[] Features { get => _features.ToArray(); }
        #endregion

        /// <summary>
        /// Constructor of this class
        /// </summary>
        public Game() 
        { 
            _features = new List<Feature>();
        }

        /// <summary>
        /// Sets a new startChapter and registers it for bubble events
        /// </summary>
        /// <param name="startChapter"></param>
        public void SetStartChapter(Chapter? startChapter)
        {
            RemoveStartChapter();

            if (startChapter != null)
            {
                _startChapter = startChapter;
                RegisterSubnode(startChapter);
                OnPropertyChanged(nameof(StartChapter));
            }
        }

        /// <summary>
        /// Unregisters and removes the current start chapter
        /// </summary>
        public void RemoveStartChapter()
        {
            if (_startChapter != null)
                UnregisterSubnode(_startChapter);
            _startChapter = null;
        }

        /// <summary>
        /// Adds a feature to the features available in this game
        /// </summary>
        /// <param name="feature">The feature to be added</param>
        public void AddFeature(Feature feature)
        {
            _features.Add(feature);
            OnPropertyChanged(nameof(Features));
        }

        /// <summary>
        /// Creates a new feature, register it for bubbling event and add it
        /// to the features available in this game
        /// </summary>
        /// <param name="feature">The feature to be added</param>
        public Feature CreateFeature()
        {
            Feature feature = new Feature();
            
            RegisterSubnode(feature);

            AddFeature(feature);
            return feature;
        }

        /// <summary>
        /// Removes a feature that is available in this game
        /// </summary>
        /// <param name="feature">The feature to be removed</param>
        public void RemoveFeature(Feature feature)
        {
            // TODO: in the bass class, the registering for the BubbleEvents has to be deleted, too!
            UnregisterSubnode(feature);

            _features.Remove(feature);
            OnPropertyChanged(nameof(Features));
        }
    }
}
