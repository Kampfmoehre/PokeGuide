using System;

namespace PokeGuide.Core.Model
{
    /// <summary>
    /// Represents a damage category a move can have
    /// </summary>
    public class DamageClass : ModelNameBase
    {
        Uri _iconUri;
        string _description;
        string _identifier;
        /// <summary>
        /// Sets and gets the URI to the icon
        /// </summary>
        public Uri IconUri
        {
            get { return _iconUri; }
            set { Set(() => IconUri, ref _iconUri, value); }
        }
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
