namespace PokeGuide.Model
{
    public class ElementType : ModelBase
    {
        int _damageClassId;
        int _generation;
        /// <summary>
        /// Sets and gets the
        /// </summary>
        public int DamageClassId
        {
            get { return _damageClassId; }
            set { Set(() => DamageClassId, ref _damageClassId, value); }
        }
        public int Generation
        {
            get { return _generation; }
            set { Set(() => Generation, ref _generation, value); }
        }
    }
}