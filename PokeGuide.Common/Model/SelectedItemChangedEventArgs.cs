using System;

namespace PokeGuide.Model
{
    public class SelectedItemChangedEventArgs<T> : EventArgs
    {
        public SelectedItemChangedEventArgs(T oldItem, T newItem)
        {
            OldItem = oldItem;
            NewItem = newItem;
        }
        public T OldItem { get; private set; }
        public T NewItem { get; private set; }
    }

    public delegate void SelectedItemChangedEventHandler<T>(object sender, SelectedItemChangedEventArgs<T> e);
}
