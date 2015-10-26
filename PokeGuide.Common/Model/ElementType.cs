namespace PokeGuide.Model
{
    public class ElementType : ModelBase
    {
        int _damageClassId;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int DamageClassId
        {
            get { return _damageClassId; }
            set { Set(() => DamageClassId, ref _damageClassId, value); }
        }
    }
}