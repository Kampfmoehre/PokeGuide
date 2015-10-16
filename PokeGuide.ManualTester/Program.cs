using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Data;
using PokeGuide.Data.Model;

namespace PokeGuide.ManualTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var fooBar = new FooBar();
            Task.Factory.StartNew(() => fooBar.DoAction());
            Console.ReadLine();
        }        
    }

    class FooBar
    {
        public async Task DoAction()
        {
            var source = new CancellationTokenSource();
            using (var dl = new DataLoader("database.sqlite3"))
            {
                try
                {
                    //List<GameVersion> list = await dl.LoadGamesAsync(6, source.Token);
                    //List<EggGroup> eggGroups = await dl.LoadEggGroupsAsync(6, source.Token);
                    ////List<ElementType> types = await dl.LoadTypesAsync(6, source.Token);
                    //Ability ability = await dl.LoadAbilityAsync(53, 7, 6, source.Token);
                    //Pokemon pokemon = await dl.LoadPokemonAsync(1, 25, 6, source.Token);
                    List<Ability> abilities = await dl.LoadAbilitiesAsync(7, 6, source.Token);
                    //foreach (GameVersion game in list)
                    //{
                    //    Console.WriteLine(game.Name);
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
