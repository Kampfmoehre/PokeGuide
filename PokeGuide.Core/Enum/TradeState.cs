using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeGuide.Core.Enum
{
    /// <summary>
    /// Indicates the trading state of a Pokémon
    /// </summary>
    public enum TradeState
    {
        /// <summary>
        /// Indicates that the Trainer ID of the Pokémon matches the player's Trainer ID
        /// </summary>
        Original,
        /// <summary>
        /// Indicates that the Trainer ID of the Pokémon doesn't match the player's Trainer ID
        /// </summary>
        TradedNational,
        /// <summary>
        /// Indicates that the Pokémon was traded and the original trainer is from another country than the player.
        /// This applies only for fourth gen onwards
        /// </summary>
        TradedInternational
    }
}
