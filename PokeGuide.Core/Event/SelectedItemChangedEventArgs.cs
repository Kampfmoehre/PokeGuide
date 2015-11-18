using System;

namespace PokeGuide.Core.Event
{
    /// <summary>
    /// Event args for user selection in a list
    /// </summary>
    /// <typeparam name="T">The type of the selected object</typeparam>
    public class SelectedItemChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedItemChangedEventArgs{T}"/> class
        /// </summary>
        /// <param name="oldItem">The item that was selected, before the user selected a new one</param>
        /// <param name="newItem">The item that the user selected</param>
        public SelectedItemChangedEventArgs(T oldItem, T newItem)
        {
            OldItem = oldItem;
            NewItem = newItem;
        }
        /// <summary>
        /// The old item, may be null
        /// </summary>
        public T OldItem { get; private set; }
        /// <summary>
        /// The new item
        /// </summary>
        public T NewItem { get; private set; }
    }

    /// <summary>
    /// Event handler for handling SelectedItemChanged events
    /// </summary>
    /// <typeparam name="T">The type of the selected object</typeparam>
    /// <param name="sender">The sender of the event</param>
    /// <param name="e">The event args</param>
    public delegate void SelectedItemChangedEventHandler<T>(object sender, SelectedItemChangedEventArgs<T> e);
}
