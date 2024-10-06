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
        public Chapter? StartChapter { get => _startChapter; set => SetProperty(ref _startChapter, value); }
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
        /// Adds a new feature to the features available in this game
        /// </summary>
        /// <param name="feature">The feature to be added</param>
        public void AddFeature(Feature feature)
        {
            _features.Add(feature);
            OnPropertyChanged(nameof(Features));
        }

        /// <summary>
        /// Removes a feature that is available in this game
        /// </summary>
        /// <param name="feature">The feature to be removed</param>
        public void RemoveFeature(Feature feature)
        {
            _features.Remove(feature);
            OnPropertyChanged(nameof(Features));
        }
    }
}
