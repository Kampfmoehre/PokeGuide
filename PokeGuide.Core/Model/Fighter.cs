namespace PokeGuide.Core.Model
{
    public class Fighter : ModelBase
    {
        TeamPokemon _pokemon;
        bool _isTraded;
        bool _holdsLuckyEgg;
        bool _hasParticipated;
        int _earnedExperience;
        bool _holdsExpShare;

        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public TeamPokemon Pokemon
        {
            get { return _pokemon; }
            set { Set(() => Pokemon, ref _pokemon, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public bool IsTraded
        {
            get { return _isTraded; }
            set { Set(() => IsTraded, ref _isTraded, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public bool HoldsLuckyEgg
        {
            get { return _holdsLuckyEgg; }
            set { Set(() => HoldsLuckyEgg, ref _holdsLuckyEgg, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public bool HoldsExpShare
        {
            get { return _holdsExpShare; }
            set { Set(() => HoldsExpShare, ref _holdsExpShare, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public bool HasParticipated
        {
            get { return _hasParticipated; }
            set { Set(() => HasParticipated, ref _hasParticipated, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int EarnedExperience
        {
            get { return _earnedExperience; }
            set { Set(() => EarnedExperience, ref _earnedExperience, value); }
        }
    }
}
