using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Nito.AsyncEx;

using PokeGuide.Core.Event;

namespace PokeGuide.Core.Model
{
    /// <summary>
    /// Interface for selectable collections that are loaded asynchronous and should indicate that they are beeing loaded
    /// </summary>
    /// <typeparam name="T">The type of the object that is in the collection</typeparam>
    public interface INotifyTaskCompletionCollection<T> : INotifyTaskCompletion
    {
        /// <summary>
        /// The loading task that returns the collection
        /// </summary>
        new Task<ObservableCollection<T>> Task { get; }
        /// <summary>
        /// The collection
        /// </summary>
        ObservableCollection<T> Collection { get; }
        /// <summary>
        /// The item that is currently selected
        /// </summary>
        T SelectedItem { get; set; }
        /// <summary>
        /// Fires, when the selected item changes
        /// </summary>
        event SelectedItemChangedEventHandler<T> SelectedItemChanged;
        /// <summary>
        /// Selects an item in the list
        /// </summary>
        /// <param name="id">The ID of the item</param>
        void SelectItem(int id);
    }
}
