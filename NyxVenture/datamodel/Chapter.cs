namespace NyxVenture.datamodel
{
    /// <summary>
    /// A chapter describes a situation or a place. However, it can describe
    /// anything that is part of the story. It has a minimum of one link
    /// that leads to another chapter to continue the story. If it does have no
    /// link at all, it's one of the end chapters and the story ends here.
    /// </summary>
    public class Chapter : ModelBase
    {
        #region ------------------------- FIELDS ------------------------------
        private string? _name;
        private string? _text;
        #endregion

        #region ----------------------- PROPERTIES ----------------------------
        public string? Name { get => _name; set => SetProperty(ref _name, value); }
        public string? Text { get => _text; set => SetProperty(ref _text, value); }
        #endregion

        /// <summary>
        /// Recursivley cleans all Model changed flags and ObjectChange flags of the
        /// current object and all its child nodes
        /// </summary>
        public override void CleanChangedFlags()
        {
            CleanModelChangedFlag();
            CleanObjectChangedFlag();
        }        
    }
}
