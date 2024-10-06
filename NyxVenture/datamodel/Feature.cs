namespace NyxVenture.datamodel
{
    /// <summary>
    /// A feature is a property of a character that can change during time.
    /// Typical properties are healt, strength, mana, ...
    /// </summary>
    public class Feature : ModelBase
    {
        #region ------------------------- FIELDS ------------------------------
        private string? _name;
        private string? _description;
        private int _minValue;
        private int _maxValue;
        #endregion

        #region ----------------------- PROPERTIES ----------------------------
        public string? Name { get => _name; set => SetProperty(ref _name, value); }
        public string? Description { get => _description;set => SetProperty(ref _description, value); }
        public int MinValue { get => _minValue; set => SetProperty(ref _minValue, value); }
        public int MaxValue { get => _maxValue; set => SetProperty(ref _maxValue, value); }
        #endregion

        /// <summary>
        /// Constructor of the class Feature
        /// </summary>
        public Feature() { }
    }
}
