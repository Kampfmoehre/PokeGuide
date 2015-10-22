using System.Collections.ObjectModel;

namespace PokeGuide.Wpf.Model
{
    public class Pokemon : ModelBase
    {
        
        int _hitPoints;
        int _attack;
        int _defense;
        int _specialAttack;
        int _specialDefense;
        int _speed;
        int _evHitPoints;
        int _evAttack;
        int _evDefense;
        int _evSpecialAttack;
        int _evSpecialDefense;
        ObservableCollection<Stat> _baseStats;

        
        /// <summary>
        /// Sets and gets the HP
        /// </summary>
        public int HitPoints
        {
            get { return _hitPoints; }
            set { Set(() => HitPoints, ref _hitPoints, value); }
        }
        /// <summary>
        /// Sets and gets the attack
        /// </summary>
        public int Attack
        {
            get { return _attack; }
            set { Set(() => Attack, ref _attack, value); }
        }
        /// <summary>
        /// Sets and gets the defense
        /// </summary>
        public int Defense
        {
            get { return _defense; }
            set { Set(() => Defense, ref _defense, value); }
        }
        /// <summary>
        /// Sets and gets the Special Attack
        /// </summary>
        public int SpecialAttack
        {
            get { return _specialAttack; }
            set { Set(() => SpecialAttack, ref _specialAttack, value); }
        }        
        /// <summary>
        /// Sets and gets the Special Defense
        /// </summary>
        public int SpecialDefense
        {
            get { return _specialDefense; }
            set { Set(() => SpecialDefense, ref _specialDefense, value); }
        }
        /// <summary>
        /// Sets and gets the Speed
        /// </summary>
        public int Speed
        {
            get { return _speed; }
            set { Set(() => Speed, ref _speed, value); }
        }        
        /// <summary>
        /// Sets and gets the amount of HP EVs
        /// </summary>
        public int EvHitPoints
        {
            get { return _evHitPoints; }
            set { Set(() => EvHitPoints, ref _evHitPoints, value); }
        } 
        /// <summary>
        /// Sets and gets the amount of Attack EVs
        /// </summary>
        public int EvAttack
        {
            get { return _evAttack; }
            set { Set(() => EvAttack, ref _evAttack, value); }
        }
        /// <summary>
        /// Sets and gets the amount of Defense EVs
        /// </summary>
        public int EvDefense
        {
            get { return _evDefense; }
            set { Set(() => EvDefense, ref _evDefense, value); }
        }
        /// <summary>
        /// Sets and gets the amount of Special Attack EVs 
        /// </summary>
        public int EvSpecialAttack
        {
            get { return _evSpecialAttack; }
            set { Set(() => EvSpecialAttack, ref _evSpecialAttack, value); }
        }
        /// <summary>
        /// Sets and gets the amount of Special Defense EVs 
        /// </summary>
        public int EvSpecialDefense
        {
            get { return _evSpecialDefense; }
            set { Set(() => EvSpecialDefense, ref _evSpecialDefense, value); }
        }
        int _evSpeed;
        /// <summary>
        /// Sets and gets the amount of Speed EVs  
        /// </summary>
        public int EvSpeed
        {
            get { return _evSpeed; }
            set { Set(() => EvSpeed, ref _evSpeed, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ObservableCollection<Stat> BaseStats
        {
            get { return _baseStats; }
            set { Set(() => BaseStats, ref _baseStats, value); }
        }
    }
}
