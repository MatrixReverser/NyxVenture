using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyxVenture.datamodel
{
    public class Skill : ModelBase
    {
        private string? _name;
        private string? _description;
        private int _minValue;
        private int _maxValue;

        public string? Name { get => _name; set => SetProperty(ref _name, value); }
        public string? Description { get => _description; set => SetProperty(ref _description, value); }
        public int MinValue { get => _minValue; set => SetProperty(ref _minValue, value); }
        public int MaxValue { get => _maxValue; set => SetProperty(ref _maxValue, value); }

        /// <summary>
        /// Constructor of the class Skill
        /// </summary>
        public Skill() 
        { 
        }

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
