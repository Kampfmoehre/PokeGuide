namespace PokeGuide.Wpf.Model
{
    public class MoveLearnElement : ModelBase
    {
        Move _move;
        int _level;
        MoveLearnMethod _learnMethod;

        /// <summary>
        /// Sets and gets the move
        /// </summary>
        public Move Move
        {
            get { return _move; }
            set { Set(() => Move, ref _move, value); }
        }
        /// <summary>
        /// Sets and gets the level when the move is learned
        /// </summary>
        public int Level
        {
            get { return _level; }
            set { Set(() => Level, ref _level, value); }
        }
        /// <summary>
        /// Sets and gets the learn method
        /// </summary>
        public MoveLearnMethod LearnMethod
        {
            get { return _learnMethod; }
            set { Set(() => LearnMethod, ref _learnMethod, value); }
        }
    }
}
