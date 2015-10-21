namespace PokeGuide.Wpf.Model
{
    public class Pokemon : ModelBase
    {
        ElementType _type1;
        ElementType _type2;

        /// <summary>
        /// Sets and gets the first type
        /// </summary>
        public ElementType Type1
        {
            get { return _type1; }
            set { Set(() => Type1, ref _type1, value); }
        }
        /// <summary>
        /// Sets and gets the second type
        /// </summary>
        public ElementType Type2
        {
            get { return _type2; }
            set { Set(() => Type2, ref _type2, value); }
        }
    }
}
