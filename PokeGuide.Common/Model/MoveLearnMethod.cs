namespace PokeGuide.Model
{
    public class MoveLearnMethod : ModelBase
    {
        string _description;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { Set(() => Description, ref _description, value); }
        }
    }
}
