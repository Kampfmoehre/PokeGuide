namespace PokeGuide.Data.Model
{
    public class PokemonMove : ModelBase
    {
        public Move Move { get; set; }
        public int Level { get; set; }
        public MoveLearnMethod LearnMethod { get; set; }
    }
}
