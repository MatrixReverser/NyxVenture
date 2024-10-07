using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyxVenture.datamodel
{
    /// <summary>
    /// This class describes a character type. The game object contains at least 1 type, the player
    /// can choose to play in the game.
    /// </summary>
    public class CharacterType : ModelBase
    {
        private string? _name;
        private string? _description;
        private readonly Dictionary<Feature, int> _baseFeaturePoints;

        public string? Name { get => _name; set => SetProperty(ref _name, value); }
        public string? Description { get => _description; set => SetProperty(ref _description, value); }
        public KeyValuePair<Feature, int>[] BaseFeaturePoints { get => [.. _baseFeaturePoints]; }

        /// <summary>
        /// Constructor of the class CharacterType
        /// </summary>
        public CharacterType()
        {
            _baseFeaturePoints = [];        
        }

        /// <summary>
        /// Returns the base point for a specified feature
        /// </summary>
        /// <param name="feature">The feature</param>
        /// <returns>Base points of the specified Feature or -1 if Feature is not available in this character</returns>
        public int GetBaseFeaturePoint(Feature feature)
        {
            int points = -1;

            if (_baseFeaturePoints.TryGetValue(feature, out int value))
                points = value;

            return points;
        }

        /// <summary>
        /// Sets the points for a feature. If the feature does not exist yet, it will
        /// be added to this CharacterType. The feature will not be registered as a 
        /// subnode!
        /// This method causes a PropertyChangeEvent on the BaseFeaturePoints property
        /// </summary>
        /// <param name="feature">The feature for which to set the points</param>
        /// <param name="points">points to be set</param>
        public void SetBaseFeaturePoint(Feature feature, int points)
        {
            if (!_baseFeaturePoints.TryAdd(feature, points))
                _baseFeaturePoints[feature] = points;

            OnPropertyChanged(nameof(BaseFeaturePoints));
        }

        /// <summary>
        /// Removes a feature an its points from this CharacterType
        /// </summary>
        /// <param name="feature">The feature to be removed</param>
        public void RemoveBaseFeaturePoint(Feature feature)
        {
            if (!_baseFeaturePoints.ContainsKey(feature))
                return;
            
            _baseFeaturePoints.Remove(feature);
            OnPropertyChanged(nameof(BaseFeaturePoints));
        }

        /// <summary>
        /// Cleans all changed flages recursively
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override void CleanChangedFlags()
        {
            CleanObjectChangedFlag();
            CleanModelChangedFlag();

            // TODO: recursively clean changed flags of subnodes
        }
    }
}
