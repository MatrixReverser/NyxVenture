namespace NyxVenture.datamodel
{
    /// <summary>
    /// The root class of a NyxVenture game
    /// </summary>
    public class Game : ModelBase
    {
        private string? _title;
        private string? _description;
        private string? _author;
        private string? _genre;
        private readonly List<Feature> _features;
        private readonly List<Skill> _skills;
        private readonly List<CharacterType> _characterTypes;
        private Chapter? _startChapter;
        
        public string? Title { get => _title; set => SetProperty(ref _title, value); }
        public string? Description { get => _description; set => SetProperty(ref _description, value); }
        public string? Author { get => _author; set => SetProperty(ref _author, value); }
        public string? Genre { get => _genre; set => SetProperty(ref _genre, value); }        
        public Chapter? StartChapter { get => _startChapter; }
        public Feature[] Features { get => [.. _features]; }        
        public Skill[] Skills { get => [.. _skills]; }
        public CharacterType[] CharacterTypes { get => [.. _characterTypes]; }
        
        /// <summary>
        /// Constructor of this class
        /// </summary>
        public Game() 
        { 
            _features = [];
            _skills = [];
            _characterTypes = [];
        }

        /// <summary>
        /// Sets a new startChapter. This is not registered for bubbling events
        /// </summary>
        /// <param name="startChapter"></param>
        public void SetStartChapter(Chapter startChapter)
        {
            RemoveStartChapter();                       
            _startChapter = startChapter;                                
            OnPropertyChanged(nameof(StartChapter));
        }

        /// <summary>
        /// Creates a new startChapter and registers it for bubble events
        /// </summary>
        /// <returns>The created chapter</returns>
        public Chapter CreateStartChapter()
        {
            Chapter chapter = new();

            RegisterSubnode(chapter);
            SetStartChapter(chapter);

            return chapter;
        }

        /// <summary>
        /// Unregisters and removes the current start chapter
        /// </summary>
        public void RemoveStartChapter()
        {
            if (_startChapter != null)
                UnregisterSubnode(_startChapter);
            _startChapter = null;
            OnPropertyChanged(nameof(StartChapter));
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
        /// <returns>The created feature</returns>>
        public Feature CreateFeature()
        {
            Feature feature = new();
            
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
            UnregisterSubnode(feature);
            _features.Remove(feature);
            OnPropertyChanged(nameof(Features));
        }

        /// <summary>
        /// Adds a skill to the skills available in this game
        /// </summary>
        /// <param name="skill">The skill to be added</param>
        public void AddSkill(Skill skill)
        {
            _skills.Add(skill);
            OnPropertyChanged(nameof(Skills));
        }

        /// <summary>
        /// Creates a new skill, register it for bubbling event and add it
        /// to the skills available in this game
        /// </summary>
        /// <returns>The created skill</returns>>
        public Skill CreateSkill()
        {
            Skill skill = new();

            RegisterSubnode(skill);
            AddSkill(skill);
            return skill;
        }

        /// <summary>
        /// Removes a skill that is available in this game
        /// </summary>
        /// <param name="skill">The skill to be removed</param>
        public void RemoveSkill(Skill skill)
        {
            UnregisterSubnode(skill);
            _skills.Remove(skill);
            OnPropertyChanged(nameof(Skills));
        }

        /// <summary>
        /// Adds a character type to this game
        /// </summary>
        /// <param name="characterType">The character type to add</param>
        public void AddCharacterType(CharacterType characterType)
        {
            _characterTypes.Add(characterType);
            OnPropertyChanged(nameof(CharacterTypes));
        }

        /// <summary>
        /// Creates a new character type for this game
        /// </summary>
        /// <returns>The new CharacterType</returns>
        public CharacterType CreateCharacterType()
        {
            CharacterType characterType = new();

            RegisterSubnode(characterType);
            AddCharacterType(characterType);
            return characterType;
        }

        /// <summary>
        /// Removes a CharacterType from this game
        /// </summary>
        /// <param name="characterType">The CharacterType to be removed</param>
        public void RemoveCharacterType(CharacterType characterType)
        {
            UnregisterSubnode(characterType);
            _characterTypes.Remove(characterType);
            OnPropertyChanged(nameof(CharacterTypes));
        }

        /// <summary>
        /// Recursivley cleans all Model changed flags and ObjectChange flags of the
        /// current object and all its child nodes
        /// </summary>
        public override void CleanChangedFlags()
        {
            CleanObjectChangedFlag();
            CleanModelChangedFlag();

            // child objects
            StartChapter?.CleanChangedFlags();

            foreach (Feature feature in Features)
                feature.CleanChangedFlags();

            foreach (Skill skill in Skills)
                skill.CleanChangedFlags();
        }
    }
}
