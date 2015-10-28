using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using PokeGuide.Model;

namespace PokeGuide.ViewModel
{
    public class SelectableCollection<T> : ObservableObject 
        where T : ModelBase, new()
    {
        public SelectableCollection()
        {

        }

        public SelectableCollection(ObservableCollection<T> collection)
        {
            Collection = collection;
            if (collection != null)
                SelectedItem = collection.FirstOrDefault();
        }

        ObservableCollection<T> _collection;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ObservableCollection<T> Collection
        {
            get { return _collection; }
            set { Set(() => Collection, ref _collection, value); }
        }
        T _selectedItem;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public T SelectedItem
        {
            get { return _selectedItem; }
            set { Set(() => SelectedItem, ref _selectedItem, value); }
        }
    }
}
