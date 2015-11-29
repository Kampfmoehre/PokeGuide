using System;

namespace PokeGuide.Core.Model
{
    /// <summary>
    /// Represents a damage category a move can have
    /// </summary>
    public class DamageClass : ModelUriBase
    {
        string _description;
        string _identifier;
        /// <summary>
        /// Sets and gets the description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { Set(() => Description, ref _description, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string Identifier
        {
            get { return _identifier; }
            set { Set(() => Identifier, ref _identifier, value); }
        }
    }
}
