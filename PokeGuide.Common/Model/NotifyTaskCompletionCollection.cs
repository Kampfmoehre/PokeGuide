using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Nito.AsyncEx;

namespace PokeGuide.Model
{
    public static class NotifyTaskCompletionCollection<T> where T : ModelBase
    {
        public static INotifyTaskCompletionCollection<T> Create(Task<ObservableCollection<T>> task, int? itemToSelect = null)
        {
            return new NotifyTaskCompletionCollectionImplementation<T>(task, itemToSelect);
        }

        public static INotifyTaskCompletionCollection<T> Create(Func<Task<ObservableCollection<T>>> asyncAction, int? itemToSelect = null)
        {
            return Create(asyncAction(), itemToSelect);
        }

        private sealed class NotifyTaskCompletionCollectionImplementation<TResult> : INotifyTaskCompletionCollection<TResult> where TResult : ModelBase
        {
            TResult _selectedItem;
            public event PropertyChangedEventHandler PropertyChanged;
            public event SelectedItemChangedEventHandler<TResult> SelectedItemChanged;

            public NotifyTaskCompletionCollectionImplementation(Task<ObservableCollection<TResult>> task, int? itemToSelect = null)
            {
                Task = task;
                if (task.IsCompleted)
                {
                    TaskCompleted = TaskConstants.Completed;
                    return;
                }

                var scheduler = (SynchronizationContext.Current == null) ? TaskScheduler.Current : TaskScheduler.FromCurrentSynchronizationContext();
                TaskCompleted = task.ContinueWith(t =>
                {
                    var propertyChanged = PropertyChanged;
                    if (propertyChanged == null)
                        return;

                    propertyChanged(this, new PropertyChangedEventArgs("Status"));
                    propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
                    propertyChanged(this, new PropertyChangedEventArgs("IsNotCompleted"));
                    if (t.IsCanceled)
                        propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
                    else if (t.IsFaulted)
                    {
                        propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
                        propertyChanged(this, new PropertyChangedEventArgs("Exception"));
                        propertyChanged(this, new PropertyChangedEventArgs("InnerException"));
                        propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
                    }
                    else
                    {
                        propertyChanged(this, new PropertyChangedEventArgs("Collection"));
                        if (itemToSelect != null)
                            SelectedItem = Collection.FirstOrDefault(f => f.Id == (int)itemToSelect);
                        else if (Collection.Count == 1)
                            SelectedItem = Collection.First();
                        propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                    }
                }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, scheduler);
            }

            Task INotifyTaskCompletion.Task
            {
                get { return Task; }
            }
            public ObservableCollection<TResult> Collection
            {
                get { return (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : null; }
            }
            public TResult SelectedItem
            {
                get { return _selectedItem; }
                set
                {
                    TResult oldItem = _selectedItem;
                    _selectedItem = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("SelectedItem"));
                    if (SelectedItemChanged != null)
                        SelectedItemChanged(this, new SelectedItemChangedEventArgs<TResult>(oldItem, _selectedItem));
                }
            }

            public Task<ObservableCollection<TResult>> Task { get; private set; }
            public Task TaskCompleted { get; private set; }
            public TaskStatus Status
            {
                get { return Task.Status; }
            }
            public bool IsCompleted
            {
                get { return Task.IsCompleted; }
            }
            public bool IsNotCompleted
            {
                get { return !Task.IsCompleted; }
            }
            public bool IsSuccessfullyCompleted
            {
                get { return Task.Status == TaskStatus.RanToCompletion; }
            }
            public bool IsCanceled
            {
                get { return Task.IsCanceled; }
            }
            public bool IsFaulted
            {
                get { return Task.IsFaulted; }
            }
            public AggregateException Exception
            {
                get { return Task.Exception; }
            }
            public Exception InnerException
            {
                get { return (Exception == null) ? null : Exception.InnerException; }
            }
            public string ErrorMessage
            {
                get { return (InnerException == null) ? null : InnerException.Message; }
            }
        }
    }
}
