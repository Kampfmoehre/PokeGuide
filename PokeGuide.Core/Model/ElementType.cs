﻿using System;

namespace PokeGuide.Core.Model
{
    /// <summary>
    /// Represents a type a Pokémon or a move can have
    /// </summary>
    public class ElementType : ModelUriBase
    {        
        int _generation;
        int _damageClassId;
        string _identifier;        
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int Generation
        {
            get { return _generation; }
            set { Set(() => Generation, ref _generation, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int DamageClassId
        {
            get { return _damageClassId; }
            set { Set(() => DamageClassId, ref _damageClassId, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string Identifier
        {
            get { return _identifier; }
            set { Set(() => Identifier, ref _identifier, value); }
        }
    }
}
