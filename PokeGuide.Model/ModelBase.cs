using GalaSoft.MvvmLight;

namespace PokeGuide.Model
{
    /// <summary>
    /// A base model for all models
    /// </summary>
    public class ModelBase : ObservableObject
    {
        int _id;
        /// <summary>
        /// Sets and gets the ID of the object
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { Set(() => Id, ref _id, value); }
        }
    }
}
