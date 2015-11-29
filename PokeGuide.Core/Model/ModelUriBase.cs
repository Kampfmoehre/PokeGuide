using System;

namespace PokeGuide.Core.Model
{
    /// <summary>
    /// A base class for a model with an uri
    /// </summary>
    public class ModelUriBase : ModelNameBase
    {
        Uri _iconUri;
        /// <summary>
        /// Sets and gets the URI to the icon or image
        /// </summary>
        public Uri IconUri
        {
            get { return _iconUri; }
            set { Set(() => IconUri, ref _iconUri, value); }
        }
    }
}
