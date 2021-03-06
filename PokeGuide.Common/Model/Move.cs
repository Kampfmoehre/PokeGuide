﻿namespace PokeGuide.Model
{
    public class Move : ModelBase
    {
        int? _power;
        int _powerPoints;
        int? _accuracy;
        int _priority;
        ElementType _type;
        DamageClass _damageClass;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int? Power
        {
            get { return _power; }
            set { Set(() => Power, ref _power, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int PowerPoints
        {
            get { return _powerPoints; }
            set { Set(() => PowerPoints, ref _powerPoints, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int? Accuracy
        {
            get { return _accuracy; }
            set { Set(() => Accuracy, ref _accuracy, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int Priority
        {
            get { return _priority; }
            set { Set(() => Priority, ref _priority, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ElementType Type
        {
            get { return _type; }
            set { Set(() => Type, ref _type, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public DamageClass DamageClass
        {
            get { return _damageClass; }
            set { Set(() => DamageClass, ref _damageClass, value); }
        }
    }
}