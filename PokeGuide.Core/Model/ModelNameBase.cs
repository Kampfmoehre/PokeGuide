namespace PokeGuide.Core.Model
{
    /// <summary>
    /// Base model with a name to display mostly in comboboxes
    /// </summary>
    public class ModelNameBase : ModelBase
    {
        string _name;
        /// <summary>
        /// Sets and gets the name of the object
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }
    }
}
