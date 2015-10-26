namespace PokeGuide.Model
{
    public class PokemonMove : ModelBase
    {
        Move _move;
        int _level;
        MoveLearnMethod _learnMethod;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public Move Move
        {
            get { return _move; }
            set { Set(() => Move, ref _move, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int Level
        {
            get { return _level; }
            set { Set(() => Level, ref _level, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public MoveLearnMethod LearnMethod
        {
            get { return _learnMethod; }
            set { Set(() => LearnMethod, ref _learnMethod, value); }
        }
    }
}