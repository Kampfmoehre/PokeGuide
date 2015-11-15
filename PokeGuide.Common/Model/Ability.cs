namespace PokeGuide.Model
{
    public class Ability : ModelBase
    {
        string _description;
        string _effect;
        string _flavorText;
        string _effectChange;
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
        public string Effect
        {
            get { return _effect; }
            set { Set(() => Effect, ref _effect, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string FlavorText
        {
            get { return _flavorText; }
            set { Set(() => FlavorText, ref _flavorText, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string EffectChange
        {
            get { return _effectChange; }
            set { Set(() => EffectChange, ref _effectChange, value); }
        }
    }
}
