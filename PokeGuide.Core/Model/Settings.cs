namespace PokeGuide.Core.Model
{
    /// <summary>
    /// Represents system settings
    /// </summary>
    public class Settings : ModelBase
    {
        DisplayLanguage _currentLanguage;
        GameVersion _currentVersion;

        /// <summary>
        /// Sets and gets the currently selected language
        /// </summary>
        public DisplayLanguage CurrentLanguage
        {
            get { return _currentLanguage; }
            set { Set(() => CurrentLanguage, ref _currentLanguage, value); }
        }
        /// <summary>
        /// Sets and gets the currently selected version
        /// </summary>
        public GameVersion CurrentVersion
        {
            get { return _currentVersion; }
            set { Set(() => CurrentVersion, ref _currentVersion, value); }
        }
    }
}
