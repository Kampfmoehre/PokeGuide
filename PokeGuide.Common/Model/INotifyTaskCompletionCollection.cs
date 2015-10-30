using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

using Nito.AsyncEx;

namespace PokeGuide.Model
{
    public interface INotifyTaskCompletionCollection<T> : INotifyTaskCompletion
    {
        new Task<ObservableCollection<T>> Task { get; }
        ObservableCollection<T> Collection { get; }
        T SelectedItem { get; set; }
        event SelectedItemChangedEventHandler<T> SelectedItemChanged;
    }
}
