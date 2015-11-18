namespace PokeGuide.Core.ViewModel.Interface
{
    /// <summary>
    /// Help for viewmodels that are for views that handle single object views
    /// </summary>
    public interface INavigable
    {
        /// <summary>
        /// Should handle when the page is navigated to
        /// </summary>
        /// <param name="parameter">The argument</param>
        void Activate(object parameter);
        /// <summary>
        /// Should handle when the page is left
        /// </summary>
        /// <param name="parameter">The argument</param>
        void Deactivate(object parameter);
    }
}
