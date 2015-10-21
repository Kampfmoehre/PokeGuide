namespace PokeGuide.Wpf.Model
{
    public class Move : ModelBase
    {
        int? _power;
        int? _accuracy;
        int _powerPoints;
        int _priority;
        ElementType _type;
        DamageClass _damageClass;

        /// <summary>
        /// Sets and gets the power of the move
        /// </summary>
        public int? Power
        {
            get { return _power; }
            set { Set(() => Power, ref _power, value); }
        }
        /// <summary>
        /// Sets and gets the accuracy
        /// </summary>
        public int? Accuracy
        {
            get { return _accuracy; }
            set { Set(() => Accuracy, ref _accuracy, value); }
        }
        /// <summary>
        /// Sets and gets the power points
        /// </summary>
        public int PowerPoints
        {
            get { return _powerPoints; }
            set { Set(() => PowerPoints, ref _powerPoints, value); }
        }
        /// <summary>
        /// Sets and gets the priority
        /// </summary>
        public int Priority
        {
            get { return _priority; }
            set { Set(() => Priority, ref _priority, value); }
        }
        /// <summary>
        /// Sets and gets the type
        /// </summary>
        public ElementType Type
        {
            get { return _type; }
            set { Set(() => Type, ref _type, value); }
        }
        /// <summary>
        /// Sets and gets the damage class
        /// </summary>
        public DamageClass DamageClass
        {
            get { return _damageClass; }
            set { Set(() => DamageClass, ref _damageClass, value); }
        }
    }
}
