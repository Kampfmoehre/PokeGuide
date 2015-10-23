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
            var progress = new Progress<double>();
            progress.ProgressChanged += (s, e) => { Console.Write("\r {0}%", e); };
            var source = new CancellationTokenSource();
            try
            {
                using (var dl = new DataLoader("database.sqlite3"))
                {
                    //List<GameVersion> list = await dl.LoadGamesAsync(6, progress, source.Token);
                    List<Ability> abilities = await dl.LoadAbilitiesAsync(8, 6, progress, source.Token);
                    //Ability ability = await dl.LoadAbilityAsync(153, 11, 6, source.Token);
                    //List<EggGroup> eggGroups = await dl.LoadEggGroupsAsync(6, source.Token);
                    //EggGroup groupy = await dl.LoadEggGroupAsync(3, 6, source.Token);
                    //List<ElementType> types = await dl.LoadTypesAsync(6, 6, source.Token);
                    //ElementType type = await dl.LoadTypeAsync(17, 3, 6, source.Token);
                    List<Species> species = await dl.LoadAllSpeciesAsync(6, 6, progress, source.Token);
                    List<PokemonForm> forms = await dl.LoadFormsAsync(6, 16, 6, progress, source.Token);
                    //List<Pokemon> pokemon = await dl.LoadAllPokemonAsync(23, 6, source.Token);
                    //
                    //
                    //
                    //
                    //Pokemon pokemon = await dl.LoadPokemonAsync(1, 25, 6, source.Token);
                    //
                    //
                    //Move move = await dl.LoadMoveAsync(17, 4, 6, source.Token);
                    //List<PokemonMove> moveset = await dl.LoadPokemonMoveSetAsync(45, 23, 6, source.Token);
                    //foreach (GameVersion game in list)
                    //{
                    //    Console.WriteLine(game.Name);
                    //}

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Ferdsch");
        }
    }
}
