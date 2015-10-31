﻿using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using Nito.AsyncEx;
using PokeGuide.Model;

namespace PokeGuide.ViewModel
{
    public interface IPokemonViewModel
    {
        INotifyTaskCompletionCollection<Language> Languages { get; set; }
        INotifyTaskCompletionCollection<GameVersion> Versions { get; set; }
        INotifyTaskCompletionCollection<SpeciesName> SpeciesList { get; set; }
        INotifyTaskCompletionCollection<PokemonForm> Forms { get; set; }
        INotifyTaskCompletion<PokemonForm> CurrentForm { get; set; }
        INotifyTaskCompletion<ObservableCollection<PokemonMove>> CurrentMoveSet { get; set; }
        INotifyTaskCompletion<ObservableCollection<Stat>> CurrentStats { get; set; }
        INotifyTaskCompletion<ObservableCollection<PokemonEvolution>> CurrentEvolutions { get; set; }
        INotifyTaskCompletion<ObservableCollection<PokemonLocation>> CurrentLocations { get; set; }
        RelayCommand<int> LoadFormCommand { get; }
    }
}