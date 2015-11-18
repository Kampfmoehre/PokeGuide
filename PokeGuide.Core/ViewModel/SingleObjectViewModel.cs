using System.Threading;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

using PokeGuide.Core.Model;

namespace PokeGuide.Core.ViewModel
{
    /// <summary>
    /// base view model for views that display a single object, such as a move or an ability
    /// </summary>
    public class SingleObjectViewModel : ViewModelBase
    {
        /// <summary>
        /// A source for cancellation tokens
        /// </summary>
        protected CancellationTokenSource TokenSource;
        /// <summary>
        /// The language that the user selected to display all names
        /// </summary>
        protected DisplayLanguage CurrentLanguage;
        /// <summary>
        /// The current version that the user selected
        /// </summary>
        protected GameVersion CurrentVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleObjectViewModel"/> class
        /// </summary>
        public SingleObjectViewModel()
        {
            TokenSource = new CancellationTokenSource();

            Messenger.Default.Register<DisplayLanguage>(this, (language) => ChangeLanguage(language));
            Messenger.Default.Register<GameVersion>(this, (gameVersion) => ChangeVersion(gameVersion));
        }

        /// <summary>
        /// Changes the current language
        /// </summary>
        /// <param name="newLanguage">The new language</param>
        protected virtual void ChangeLanguage(DisplayLanguage newLanguage)
        {
            CurrentLanguage = newLanguage;
        }
        /// <summary>
        /// Changes the current version
        /// </summary>
        /// <param name="newVersion">The new version</param>
        protected virtual void ChangeVersion(GameVersion newVersion)
        {
            CurrentVersion = newVersion;
        }
    }
}
