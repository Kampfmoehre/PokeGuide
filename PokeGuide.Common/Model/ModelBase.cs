using GalaSoft.MvvmLight;

namespace PokeGuide.Model
{
    /// <summary>
    /// A base model
    /// </summary>
    public class ModelBase : ObservableObject
    {
        int __id;
        string _name;
        /// <summary>
        /// Sets and gets the id
        /// </summary>
        public int Id
        {
            get { return __id; }
            set { Set(() => Id, ref __id, value); }
        }
        /// <summary>
        /// Sets and gets the name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }
    }
}