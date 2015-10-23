namespace PokeGuide.Data.Model
{
    public class Language : ModelBase
    {
        internal override string GetListQuery()
        {
            return "SELECT l.id, ln.name FROM pokemon_v2_language l\n" +
                "LEFT JOIN\n(SELECT e.language_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_languagename e\n" +
                "LEFT OUTER JOIN pokemon_v2_languagename o ON e.language_id = o.language_id and o.local_language_id = {0}\n" +
                "WHERE e.local_language_id = 9) AS ln ON l.id = ln.id";
        }
    }
}
