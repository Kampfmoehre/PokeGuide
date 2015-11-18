using Nito.AsyncEx;
using PokeGuide.Core.Model;

namespace PokeGuide.Core.ViewModel.Interface
{
    /// <summary>
    /// Interface for a simple ability view model
    /// </summary>
    public interface IAbilityViewModel
    {
        /// <summary>
        /// The selectable list to display
        /// </summary>
        INotifyTaskCompletionCollection<ModelNameBase> AbilityList { get; set; }
        /// <summary>
        /// The ability that was selected in the list
        /// </summary>
        INotifyTaskCompletion<Ability> CurrentAbility { get; set; }
    }
}
