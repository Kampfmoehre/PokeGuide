using GalaSoft.MvvmLight;

namespace PokeGuide.Wpf.Model
{
    public class ModelBase : ObservableObject
    {
        int _id;
        string _name;

        /// <summary>
        /// Sets and gets the ID
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { Set(() => Id, ref _id, value); }
        }
        /// <summary>
        /// Sets and gets Name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }
    }
}
