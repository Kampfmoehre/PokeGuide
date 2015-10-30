using System.Collections.ObjectModel;

namespace PokeGuide.Model
{
    public class PokemonForm : ModelBase
    {
        Species _species;
        double _weight;
        double _height;
        int _baseExperience;
        ElementType _type1;
        ElementType _type2;
        Ability _ability1;
        Ability _ability2;
        Ability _hiddenAbility;
        ObservableCollection<Stat> _stats;
        ObservableCollection<PokemonMove> _moveSet;
        Item _heldItem;
        int? _heldItemRarity;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public Species Species
        {
            get { return _species; }
            set { Set(() => Species, ref _species, value); }
        } 
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public double Weight
        {
            get { return _weight; }
            set { Set(() => Weight, ref _weight, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public double Height
        {
            get { return _height; }
            set { Set(() => Height, ref _height, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int BaseExperience
        {
            get { return _baseExperience; }
            set { Set(() => BaseExperience, ref _baseExperience, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ElementType Type1
        {
            get { return _type1; }
            set { Set(() => Type1, ref _type1, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ElementType Type2
        {
            get { return _type2; }
            set { Set(() => Type2, ref _type2, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public Ability Ability1
        {
            get { return _ability1; }
            set { Set(() => Ability1, ref _ability1, value); }
        }                      
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public Ability Ability2
        {
            get { return _ability2; }
            set { Set(() => Ability2, ref _ability2, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public Ability HiddenAbility
        {
            get { return _hiddenAbility; }
            set { Set(() => HiddenAbility, ref _hiddenAbility, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ObservableCollection<Stat> Stats
        {
            get { return _stats; }
            set { Set(() => Stats, ref _stats, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ObservableCollection<PokemonMove> MoveSet
        {
            get { return _moveSet; }
            set { Set(() => MoveSet, ref _moveSet, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public Item HeldItem
        {
            get { return _heldItem; }
            set { Set(() => HeldItem, ref _heldItem, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int? HeldItemRarity
        {
            get { return _heldItemRarity; }
            set { Set(() => HeldItemRarity, ref _heldItemRarity, value); }
        }
    }
}