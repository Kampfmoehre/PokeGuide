namespace PokeGuide.Data.Model
{
    public class EggGroup : ModelBase
    {
        internal override string GetListQuery()
        {
            return "SELECT e.id, en.name FROM pokemon_v2_egggroup as e\n" +
                "LEFT JOIN\n(SELECT def.egg_group_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_egggroupname def\n" +
                "LEFT JOIN pokemon_v2_egggroupname curr ON def.egg_group_id = curr.egg_group_id AND def.language_id = 9 AND curr.language_id = {0}\n" +
                "GROUP BY def.egg_group_id)\nAS en ON e.id = en.id";
        }
        internal override string GetSingleQuery()
        {
           return GetListQuery() + "\nWHERE e.id = {1}";
        }
    }
}
