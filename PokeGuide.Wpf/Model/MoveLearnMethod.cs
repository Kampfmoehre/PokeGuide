namespace PokeGuide.Wpf.Model
{
    public class MoveLearnMethod : ModelBase
    {
        string _description;

        /// <summary>
        /// Sets and gets the description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { Set(() => Description, ref _description, value); }
        }
    }
}
