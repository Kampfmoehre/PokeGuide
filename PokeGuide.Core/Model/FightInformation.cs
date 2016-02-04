using System.Collections.Generic;

namespace PokeGuide.Core.Model
{
    public class FightInformation : ModelBase
    {
        ushort _enemyBaseExperience;
        byte _enemyLevel;
        bool _enemyIsWild;
        IList<Fighter> _team;
        bool _expAllActive;

        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ushort EnemyBaseExperience
        {
            get { return _enemyBaseExperience; }
            set { Set(() => EnemyBaseExperience, ref _enemyBaseExperience, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public byte EnemyLevel
        {
            get { return _enemyLevel; }
            set { Set(() => EnemyLevel, ref _enemyLevel, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public bool EnemyIsWild
        {
            get { return _enemyIsWild; }
            set { Set(() => EnemyIsWild, ref _enemyIsWild, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public IList<Fighter> Team
        {
            get { return _team; }
            set { Set(() => Team, ref _team, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public bool ExpAllActive
        {
            get { return _expAllActive; }
            set { Set(() => ExpAllActive, ref _expAllActive, value); }
        }
    }
}
