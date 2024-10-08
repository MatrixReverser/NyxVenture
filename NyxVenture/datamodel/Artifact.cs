using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyxVenture.datamodel
{
    /// <summary>
    /// An artifact is an item that can move from one place to another. It
    /// might be attached to a Chapter but can be taken and is then attached
    /// to the player. However, a complete list of artifacts is hold by the
    /// game.
    /// When the player uses an artifact (within an ArtifactLink), it might 
    /// be exhausted after the use and is not available for use anymore.
    /// </summary>
    public class Artifact : ModelBase
    {
        private string? _name;
        private string? _description;
        private bool _exhausted;

        public string? Name { get => _name; set => SetProperty(ref _name, value); }
        public string? Description { get => _description; set => SetProperty(ref _description, value); }
        public bool Exhausted { get => _exhausted; set => SetProperty(ref _exhausted, value); }

        /// <summary>
        /// Constructor of the class Artifact
        /// </summary>
        public Artifact()
        {
        }

        /// <summary>
        /// Recursivley cleans all Model changed flags and ObjectChange flags of the
        /// current object and all its child nodes
        /// </summary>
        public override void CleanChangedFlags()
        {
            CleanObjectChangedFlag();
            CleanModelChangedFlag();
        }
    }
}
