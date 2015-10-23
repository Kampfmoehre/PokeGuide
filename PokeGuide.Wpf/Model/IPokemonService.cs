using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PokeGuide.Wpf.Model
{
    public interface IPokemonService
    {
        /// <summary>
        /// Loads a list of all Pokémon species for a generation
        /// </summary>
        /// <param name="generation">The generation</param>
        /// <param name="language">The ID of the language</param>
        /// <param name="progress">An interface to report progress</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <returns>A list of all Pokémon species for the generation</returns>
        Task<List<Species>> LoadAllSpeciesAsync(int generation, int language, IProgress<double> progress, CancellationToken token);
        /// <summary>
        /// Loads all Pokémon forms for a species
        /// </summary>
        /// <param name="species">The Pokémon species</param>
        /// <param name="versionGroup">The ID of the game version group</param>
        /// <param name="language">The ID of the language</param>
        /// <param name="progress">An interface to report progress</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <returns>A list of all forms for the given species</returns>
        Task<List<PokemonForm>> LoadPokemonFormsAsync(Species species, int versionGroup, int language, IProgress<double> progress, CancellationToken token);
    }
}
