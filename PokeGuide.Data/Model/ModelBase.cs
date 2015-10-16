namespace PokeGuide.Data.Model
{
    /// <summary>
    /// Base class for all database models
    /// </summary>
    public class ModelBase
    {
        public ModelBase()
        {
            
        }

        /// <summary>
        /// The id of the object
        /// </summary>
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
