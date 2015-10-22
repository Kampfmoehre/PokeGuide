namespace PokeGuide.Data.Model
{
    public class ElementType : ModelBase
    {
        internal override string GetListQuery()
        {
            return "SELECT t.id, tn.name FROM pokemon_v2_type as t\n" +
                "LEFT JOIN\n(SELECT def.type_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_typename def\n" +
                "LEFT JOIN pokemon_v2_typename curr ON def.type_id = curr.type_id AND def.language_id = 9 AND curr.language_id = {0}\n" +
                "GROUP BY def.type_id)\nAS tn ON t.id = tn.id\n" +
                "WHERE t.generation_id <= {1}";
        }
        internal override string GetSingleQuery()
        {
            return GetListQuery() + "  AND t.id = {2}";
        }
    }
}
