namespace PokeGuide.Core.Model
{
    /// <summary>
    /// Represents a Pokémon Ability
    /// </summary>
    public class PokemonAbility : ModelBase
    {
        ModelNameBase _pokemon;
        int _slot;
        bool _isHidden;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ModelNameBase Pokemon
        {
            get { return _pokemon; }
            set
            {
                Set(() => Pokemon, ref _pokemon, value);
                if (Pokemon != null)
                    Id = Pokemon.Id;
            }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int Slot
        {
            get { return _slot; }
            set { Set(() => Slot, ref _slot, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public bool IsHidden
        {
            get { return _isHidden; }
            set { Set(() => IsHidden, ref _isHidden, value); }
        }
    }
}
