namespace PokeGuide.Wpf.Model
{
    public class Ability : ModelBase
    {
        string _effect;
        string _description;
        string _flavorText;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string Effect
        {
            get { return _effect; }
            set { Set(() => Effect, ref _effect, value); }
        }        
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { Set(() => Description, ref _description, value); }
        }        
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string FlavorText
        {
            get { return _flavorText; }
            set { Set(() => FlavorText, ref _flavorText, value); }
        }
    }
}
