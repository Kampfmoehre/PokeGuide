using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PokeGuide.Core.Model;
using PokeGuide.Core.Service;
using PokeGuide.Core.Service.Interface;

namespace PokeGuide.Core.Tests.Service
{
    public class TestCalculationHelperService
    {
        ICalculationHelperService _service;

        [SetUp]
        public void SetUp()
        {
            ICalculationService calculator = new CalculationService();
            _service = new CalculationHelperService(calculator);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForFirstGen1()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 164, // Slowbro
                EnemyIsWild = false,
                EnemyLevel = 54,
                ExpAllActive = false,
                Team = SetUpFirstGenTeam(true, false, false, false, true, false)
            };

            IList<Fighter> result = _service.CalculateBattleResult(1, fightInfo);

            Assert.AreEqual(1422, result[0].EarnedExperience);
            Assert.AreEqual(0, result[1].EarnedExperience);
            Assert.AreEqual(0, result[2].EarnedExperience);
            Assert.AreEqual(0, result[3].EarnedExperience);
            Assert.AreEqual(1422, result[4].EarnedExperience);
            Assert.AreEqual(0, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForFirstGen2()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 219, // Lapras
                EnemyIsWild = false,
                EnemyLevel = 56,
                ExpAllActive = false,
                Team = SetUpFirstGenTeam(true, false, true, false, true, true)
            };

            IList<Fighter> result = _service.CalculateBattleResult(1, fightInfo);

            Assert.AreEqual(972, result[0].EarnedExperience);
            Assert.AreEqual(0, result[1].EarnedExperience);
            Assert.AreEqual(648, result[2].EarnedExperience);
            Assert.AreEqual(0, result[3].EarnedExperience);
            Assert.AreEqual(972, result[4].EarnedExperience);
            Assert.AreEqual(972, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForFirstGen3()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 151, // Weepinbell
                EnemyIsWild = true,
                EnemyLevel = 30,
                ExpAllActive = true,
                Team = SetUpFirstGenTeam(true, false, false, false, false, false)
            };

            IList<Fighter> result = _service.CalculateBattleResult(1, fightInfo);

            Assert.AreEqual(557, result[0].EarnedExperience);
            Assert.AreEqual(76, result[1].EarnedExperience);
            Assert.AreEqual(51, result[2].EarnedExperience);
            Assert.AreEqual(51, result[3].EarnedExperience);
            Assert.AreEqual(76, result[4].EarnedExperience);
            Assert.AreEqual(76, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForFirstGen4()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 203, // Cloyster
                EnemyIsWild = false,
                EnemyLevel = 53,
                ExpAllActive = true,
                Team = SetUpFirstGenTeam(true, false, true, false, false, false)
            };

            IList<Fighter> result = _service.CalculateBattleResult(1, fightInfo);

            Assert.AreEqual(985, result[0].EarnedExperience);
            Assert.AreEqual(135, result[1].EarnedExperience);
            Assert.AreEqual(657, result[2].EarnedExperience);
            Assert.AreEqual(90, result[3].EarnedExperience);
            Assert.AreEqual(135, result[4].EarnedExperience);
            Assert.AreEqual(135, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForSecondGen1()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 189, // Donphan
                EnemyIsWild = true,
                EnemyLevel = 33,
                Team = SetUpSecondGenTeam1()
            };
            fightInfo.Team[4].HasParticipated = true;
            fightInfo.Team[4].HoldsExpShare = true;

            IList<Fighter> result = _service.CalculateBattleResult(2, fightInfo);

            Assert.AreEqual(0, result[0].EarnedExperience);
            Assert.AreEqual(0, result[1].EarnedExperience);
            Assert.AreEqual(0, result[2].EarnedExperience);
            Assert.AreEqual(0, result[3].EarnedExperience);
            Assert.AreEqual(886, result[4].EarnedExperience);
            Assert.AreEqual(0, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForSecondGen2()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 108, // Onix
                EnemyIsWild = true,
                EnemyLevel = 36,
                Team = SetUpSecondGenTeam1()
            };
            fightInfo.Team[2].HoldsExpShare = true;
            fightInfo.Team[5].HasParticipated = true;

            IList<Fighter> result = _service.CalculateBattleResult(2, fightInfo);

            Assert.AreEqual(0, result[0].EarnedExperience);
            Assert.AreEqual(0, result[1].EarnedExperience);
            Assert.AreEqual(415, result[2].EarnedExperience);
            Assert.AreEqual(0, result[3].EarnedExperience);
            Assert.AreEqual(0, result[4].EarnedExperience);
            Assert.AreEqual(277, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForSecondGen3()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 189, // Donphan
                EnemyIsWild = true,
                EnemyLevel = 33,
                Team = SetUpSecondGenTeam1()
            };
            fightInfo.Team[0].HoldsExpShare = true;
            fightInfo.Team[1].HasParticipated = true;
            fightInfo.Team[4].HasParticipated = true;

            IList<Fighter> result = _service.CalculateBattleResult(2, fightInfo);

            Assert.AreEqual(443, result[0].EarnedExperience);
            Assert.AreEqual(331, result[1].EarnedExperience);
            Assert.AreEqual(0, result[2].EarnedExperience);
            Assert.AreEqual(0, result[3].EarnedExperience);
            Assert.AreEqual(221, result[4].EarnedExperience);
            Assert.AreEqual(0, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForSecondGen4()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 171, // Golbat
                EnemyIsWild = true,
                EnemyLevel = 32,
                Team = SetUpSecondGenTeam1()
            };
            fightInfo.Team[0].HoldsExpShare = true;
            fightInfo.Team[3].HasParticipated = true;
            fightInfo.Team[3].HoldsLuckyEgg = true;

            IList<Fighter> result = _service.CalculateBattleResult(2, fightInfo);

            Assert.AreEqual(388, result[0].EarnedExperience);
            Assert.AreEqual(0, result[1].EarnedExperience);
            Assert.AreEqual(0, result[2].EarnedExperience);
            Assert.AreEqual(873, result[3].EarnedExperience);
            Assert.AreEqual(0, result[4].EarnedExperience);
            Assert.AreEqual(0, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForSecondGen5()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 134, // Graveler
                EnemyIsWild = true,
                EnemyLevel = 32,
                Team = SetUpSecondGenTeam1()
            };
            fightInfo.Team[2].HoldsExpShare = true;
            fightInfo.Team[5].HasParticipated = true;
            fightInfo.Team[5].HoldsLuckyEgg = true;

            IList<Fighter> result = _service.CalculateBattleResult(2, fightInfo);

            Assert.AreEqual(0, result[0].EarnedExperience);
            Assert.AreEqual(0, result[1].EarnedExperience);
            Assert.AreEqual(459, result[2].EarnedExperience);
            Assert.AreEqual(0, result[3].EarnedExperience);
            Assert.AreEqual(0, result[4].EarnedExperience);
            Assert.AreEqual(459, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForSecondGen6()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 171, // Xatu
                EnemyIsWild = false,
                EnemyLevel = 40,
                Team = SetUpSecondGenTeam2()
            };
            fightInfo.Team[0].HoldsLuckyEgg = true;
            fightInfo.Team[0].HasParticipated = true;
            fightInfo.Team[5].HoldsExpShare = true;

            IList<Fighter> result = _service.CalculateBattleResult(2, fightInfo);

            Assert.AreEqual(1090, result[0].EarnedExperience);
            Assert.AreEqual(0, result[1].EarnedExperience);
            Assert.AreEqual(0, result[2].EarnedExperience);
            Assert.AreEqual(0, result[3].EarnedExperience);
            Assert.AreEqual(0, result[4].EarnedExperience);
            Assert.AreEqual(727, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForSecondGen7()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 137, // Jynx
                EnemyIsWild = false,
                EnemyLevel = 41,
                Team = SetUpSecondGenTeam2()
            };
            fightInfo.Team[0].HoldsLuckyEgg = true;
            fightInfo.Team[0].HasParticipated = true;
            fightInfo.Team[1].HasParticipated = true;
            fightInfo.Team[3].HasParticipated = true;
            fightInfo.Team[5].HoldsExpShare = true;

            IList<Fighter> result = _service.CalculateBattleResult(2, fightInfo);

            Assert.AreEqual(288, result[0].EarnedExperience);
            Assert.AreEqual(288, result[1].EarnedExperience);
            Assert.AreEqual(0, result[2].EarnedExperience);
            Assert.AreEqual(192, result[3].EarnedExperience);
            Assert.AreEqual(0, result[4].EarnedExperience);
            Assert.AreEqual(597, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForSecondGen8()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 171, // Xatu
                EnemyIsWild = false,
                EnemyLevel = 42,
                Team = SetUpSecondGenTeam2()
            };
            fightInfo.Team[2].HasParticipated = true;
            fightInfo.Team[5].HasParticipated= true;
            fightInfo.Team[5].HoldsExpShare = true;

            IList<Fighter> result = _service.CalculateBattleResult(2, fightInfo);

            Assert.AreEqual(0, result[0].EarnedExperience);
            Assert.AreEqual(0, result[1].EarnedExperience);
            Assert.AreEqual(567, result[2].EarnedExperience);
            Assert.AreEqual(0, result[3].EarnedExperience);
            Assert.AreEqual(0, result[4].EarnedExperience);
            Assert.AreEqual(1143, result[5].EarnedExperience);
        }

        List<Fighter> SetUpFirstGenTeam(bool part1, bool part2, bool part3, bool part4, bool part5, bool part6)
        {
            return new List<Fighter>
            {
                new Fighter
                {
                    HasParticipated = part1,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = true,
                    Pokemon = new TeamPokemon { Name = "Machoke" }
                },
                new Fighter
                {
                    HasParticipated = part2,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = true,
                    Pokemon = new TeamPokemon { Name = "Porenta" }
                },
                new Fighter
                {
                    HasParticipated = part3,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = false,
                    Pokemon = new TeamPokemon { Name = "Lapras" }
                },
                new Fighter
                {
                    HasParticipated = part4,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = false,
                    Pokemon = new TeamPokemon { Name = "Dugtrio" }
                },
                new Fighter
                {
                    HasParticipated = part5,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = true,
                    Pokemon = new TeamPokemon { Name = "Mr.Mime" }
                },
                new Fighter
                {
                    HasParticipated = part6,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = true,
                    Pokemon = new TeamPokemon { Name = "Scyther" }
                }
            };
        }

        List<Fighter> SetUpSecondGenTeam1()
        {
            return new List<Fighter>
            {
                new Fighter
                {
                    HasParticipated = false,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = false,
                    Pokemon = new TeamPokemon { Name = "Typhlosion" }
                },
                new Fighter
                {
                    HasParticipated = false,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = true,
                    Pokemon = new TeamPokemon { Name = "Parasect" }
                },
                new Fighter
                {
                    HasParticipated = false,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = true,
                    Pokemon = new TeamPokemon { Name = "Pidgeot" }
                },
                new Fighter
                {
                    HasParticipated = false,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = true,
                    Pokemon = new TeamPokemon { Name = "Seaking" }
                },
                new Fighter
                {
                    HasParticipated = false,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = false,
                    Pokemon = new TeamPokemon { Name = "Feraligatr" }
                },
                new Fighter
                {
                    HasParticipated = false,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = false,
                    Pokemon = new TeamPokemon { Name = "Meganium" }
                }
            };
        }

        List<Fighter> SetUpSecondGenTeam2()
        {
            return new List<Fighter>
            {
                new Fighter
                {
                    HasParticipated = false,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = false,
                    Pokemon = new TeamPokemon { Name = "Meganium" }
                },
                new Fighter
                {
                    HasParticipated = false,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = true,
                    Pokemon = new TeamPokemon { Name = "Seaking" }
                },
                new Fighter
                {
                    HasParticipated = false,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = true,
                    Pokemon = new TeamPokemon { Name = "Pidgeot" }
                },
                new Fighter
                {
                    HasParticipated = false,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = false,
                    Pokemon = new TeamPokemon { Name = "Typhlosion" }
                },
                new Fighter
                {
                    HasParticipated = false,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = false,
                    Pokemon = new TeamPokemon { Name = "Feraligatr" }
                },
                new Fighter
                {
                    HasParticipated = false,
                    HoldsExpShare = false,
                    HoldsLuckyEgg = false,
                    IsTraded = false,
                    Pokemon = new TeamPokemon { Name = "Raikou" }
                }                
            };
        }
    }
}
