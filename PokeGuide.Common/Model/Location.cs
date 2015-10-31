namespace PokeGuide.Model
{
    public class Location : ModelBase
    {
        int _areaId;
        string _areaName;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int AreaId
        {
            get { return _areaId; }
            set { Set(() => AreaId, ref _areaId, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string AreaName
        {
            get { return _areaName; }
            set { Set(() => AreaName, ref _areaName, value); }
        }
    }
}
