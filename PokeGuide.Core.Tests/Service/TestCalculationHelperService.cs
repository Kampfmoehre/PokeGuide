using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PokeGuide.Core.Enum;
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
            IExperienceCalculationService calculator = new ExperienceCalculationService();
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
                Team = new List<Fighter>
                {
                    new Fighter { HasParticipated = true, TradeState = TradeState.TradedNational },
                    new Fighter(),
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HasParticipated = true, TradeState = TradeState.TradedNational },
                    new Fighter()
                }
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
                Team = new List<Fighter>
                {
                    new Fighter { HasParticipated = true, TradeState = TradeState.TradedNational },
                    new Fighter(),
                    new Fighter { HasParticipated = true },
                    new Fighter(),
                    new Fighter { HasParticipated = true, TradeState = TradeState.TradedNational },
                    new Fighter { HasParticipated = true, TradeState = TradeState.TradedNational }
                }
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
                Team = new List<Fighter>
                {
                    new Fighter { HasParticipated = true, TradeState = TradeState.TradedNational },
                    new Fighter { TradeState = TradeState.TradedNational },
                    new Fighter(),
                    new Fighter(),
                    new Fighter { TradeState = TradeState.TradedNational },
                    new Fighter { TradeState = TradeState.TradedNational }
                }
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
                Team = new List<Fighter>
                {
                    new Fighter { HasParticipated = true, TradeState = TradeState.TradedNational },
                    new Fighter { TradeState = TradeState.TradedNational },
                    new Fighter { HasParticipated = true },
                    new Fighter(),
                    new Fighter { TradeState = TradeState.TradedNational },
                    new Fighter { TradeState = TradeState.TradedNational }
                }
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
                Team = new List<Fighter>
                {
                    new Fighter(),
                    new Fighter(),
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HasParticipated = true, HoldsExpShare = true },
                    new Fighter()
                }
            };

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
                Team = new List<Fighter>
                {
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HoldsExpShare = true, TradeState = TradeState.TradedNational },
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HasParticipated = true }
                }
            };

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
                Team = new List<Fighter>
                {
                    new Fighter { HoldsExpShare = true },
                    new Fighter { HasParticipated = true, TradeState = TradeState.TradedNational },
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HasParticipated = true },
                    new Fighter()
                }
            };

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
                Team = new List<Fighter>
                {
                    new Fighter { HoldsExpShare = true },
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HasParticipated = true, HoldsLuckyEgg = true, TradeState = TradeState.TradedNational },
                    new Fighter(),
                    new Fighter()
                }
            };

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
                Team = new List<Fighter>
                {
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HoldsExpShare = true, TradeState = TradeState.TradedNational },
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HasParticipated = true, HoldsLuckyEgg = true }
                }
            };

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
                Team = new List<Fighter>
                {
                    new Fighter { HasParticipated = true, HoldsLuckyEgg = true },
                    new Fighter(),
                    new Fighter(),
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HoldsExpShare = true }
                }
            };

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
                Team = new List<Fighter>
                {
                    new Fighter { HasParticipated = true, HoldsLuckyEgg = true },
                    new Fighter { HasParticipated = true, TradeState = TradeState.TradedNational },
                    new Fighter(),
                    new Fighter { HasParticipated = true },
                    new Fighter(),
                    new Fighter { HoldsExpShare = true }
                }
            };

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
                Team = new List<Fighter>
                {
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HasParticipated = true, TradeState = TradeState.TradedNational },
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HasParticipated = true, HoldsExpShare = true }
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(2, fightInfo);

            Assert.AreEqual(0, result[0].EarnedExperience);
            Assert.AreEqual(0, result[1].EarnedExperience);
            Assert.AreEqual(567, result[2].EarnedExperience);
            Assert.AreEqual(0, result[3].EarnedExperience);
            Assert.AreEqual(0, result[4].EarnedExperience);
            Assert.AreEqual(1143, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForThirdGen1()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 126, // Loudred
                EnemyIsWild = true,
                EnemyLevel = 40,
                Team = new List<Fighter>
                {
                    new Fighter { HasParticipated = true, HoldsLuckyEgg = true },
                    new Fighter(),
                    new Fighter { HasParticipated = true, HoldsLuckyEgg = true, TradeState = TradeState.TradedNational },
                    new Fighter(),
                    new Fighter { HoldsExpShare = true, TradeState = TradeState.TradedNational },
                    new Fighter()
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(3, fightInfo);

            Assert.AreEqual(270, result[0].EarnedExperience);
            Assert.AreEqual(0, result[1].EarnedExperience);
            Assert.AreEqual(405, result[2].EarnedExperience);
            Assert.AreEqual(0, result[3].EarnedExperience);
            Assert.AreEqual(540, result[4].EarnedExperience);
            Assert.AreEqual(0, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForThirdGen2()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 171, // Golbat
                EnemyIsWild = true,
                EnemyLevel = 40,
                Team = new List<Fighter>
                {
                    new Fighter(),
                    new Fighter { HasParticipated = true },
                    new Fighter(),
                    new Fighter { HasParticipated = true },
                    new Fighter { HasParticipated = true, HoldsExpShare = true, TradeState = TradeState.TradedNational },
                    new Fighter { HasParticipated = true, TradeState = TradeState.TradedNational }
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(3, fightInfo);

            Assert.AreEqual(0, result[0].EarnedExperience);
            Assert.AreEqual(122, result[1].EarnedExperience);
            Assert.AreEqual(0, result[2].EarnedExperience);
            Assert.AreEqual(122, result[3].EarnedExperience);
            Assert.AreEqual(915, result[4].EarnedExperience);
            Assert.AreEqual(183, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForThirdGen3()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 128, // Mightyena
                EnemyIsWild = false,
                EnemyLevel = 46,
                Team = new List<Fighter>
                {
                    new Fighter { HasParticipated = true, HoldsLuckyEgg = true },
                    new Fighter { HasParticipated = true },
                    new Fighter { HasParticipated = true, HoldsLuckyEgg = true, TradeState = TradeState.TradedNational },
                    new Fighter { HasParticipated = true },
                    new Fighter { HasParticipated = true, HoldsExpShare = true, TradeState = TradeState.TradedNational },
                    new Fighter { HasParticipated = true, TradeState = TradeState.TradedNational }
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(3, fightInfo);

            Assert.AreEqual(157, result[0].EarnedExperience);
            Assert.AreEqual(105, result[1].EarnedExperience);
            Assert.AreEqual(235, result[2].EarnedExperience);
            Assert.AreEqual(105, result[3].EarnedExperience);
            Assert.AreEqual(1102, result[4].EarnedExperience);
            Assert.AreEqual(157, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForThirdGen4()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 181, // Shiftry
                EnemyIsWild = false,
                EnemyLevel = 48,
                Team = new List<Fighter>
                {
                    new Fighter { HasParticipated = true, HoldsLuckyEgg = true },
                    new Fighter { HasParticipated = true },
                    new Fighter { HasParticipated = true, HoldsLuckyEgg = true, TradeState = TradeState.TradedNational },
                    new Fighter { HasParticipated = true },
                    new Fighter { HoldsExpShare = true, TradeState = TradeState.TradedNational },
                    new Fighter()
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(3, fightInfo);

            Assert.AreEqual(348, result[0].EarnedExperience);
            Assert.AreEqual(232, result[1].EarnedExperience);
            Assert.AreEqual(522, result[2].EarnedExperience);
            Assert.AreEqual(232, result[3].EarnedExperience);
            Assert.AreEqual(1395, result[4].EarnedExperience);
            Assert.AreEqual(0, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForFourthGen1()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 117, // Nidorina
                EnemyIsWild = true,
                EnemyLevel = 23,
                Team = new List<Fighter>
                {
                    new Fighter { HasParticipated = true },
                    new Fighter(),
                    new Fighter { HasParticipated = true },
                    new Fighter { HoldsExpShare = true, TradeState = TradeState.TradedNational },
                    new Fighter(),
                    new Fighter { HasParticipated = true, HoldsExpShare = true }
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(4, fightInfo);

            Assert.AreEqual(64, result[0].EarnedExperience);
            Assert.AreEqual(0, result[1].EarnedExperience);
            Assert.AreEqual(64, result[2].EarnedExperience);
            Assert.AreEqual(144, result[3].EarnedExperience);
            Assert.AreEqual(0, result[4].EarnedExperience);
            Assert.AreEqual(160, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForFourthGen2()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 90, // Grimer
                EnemyIsWild = true,
                EnemyLevel = 28,
                Team = new List<Fighter>
                {
                    new Fighter(),
                    new Fighter { HasParticipated = true },
                    new Fighter(),
                    new Fighter { HoldsExpShare = true, TradeState = TradeState.TradedNational },
                    new Fighter(),
                    new Fighter { HoldsExpShare = true }
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(4, fightInfo);

            Assert.AreEqual(0, result[0].EarnedExperience);
            Assert.AreEqual(180, result[1].EarnedExperience);
            Assert.AreEqual(0, result[2].EarnedExperience);
            Assert.AreEqual(135, result[3].EarnedExperience);
            Assert.AreEqual(0, result[4].EarnedExperience);
            Assert.AreEqual(90, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForFourthGen3()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 162, // Noctowl
                EnemyIsWild = true,
                EnemyLevel = 25,
                Team = new List<Fighter>
                {
                    new Fighter(),
                    new Fighter { HasParticipated = true },
                    new Fighter(),
                    new Fighter { HasParticipated = true, HoldsExpShare = true, TradeState = TradeState.TradedNational },
                    new Fighter { HasParticipated = true, TradeState = TradeState.TradedNational },
                    new Fighter { HoldsExpShare = true }
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(4, fightInfo);

            Assert.AreEqual(0, result[0].EarnedExperience);
            Assert.AreEqual(96, result[1].EarnedExperience);
            Assert.AreEqual(0, result[2].EarnedExperience);
            Assert.AreEqual(360, result[3].EarnedExperience);
            Assert.AreEqual(144, result[4].EarnedExperience);
            Assert.AreEqual(144, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForFourthGen4()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 91, // Growlithe
                EnemyIsWild = true,
                EnemyLevel = 18,
                Team = new List<Fighter>
                {
                    new Fighter { HasParticipated = true, HoldsExpShare = true },
                    new Fighter { HasParticipated = true },
                    new Fighter(),
                    new Fighter { HasParticipated = true, HoldsExpShare = true, TradeState = TradeState.TradedNational },
                    new Fighter(),
                    new Fighter()
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(4, fightInfo);

            Assert.AreEqual(97, result[0].EarnedExperience);
            Assert.AreEqual(39, result[1].EarnedExperience);
            Assert.AreEqual(0, result[2].EarnedExperience);
            Assert.AreEqual(145, result[3].EarnedExperience);
            Assert.AreEqual(0, result[4].EarnedExperience);
            Assert.AreEqual(0, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForFifthGen1()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 87, // Tangela
                EnemyIsWild = true,
                EnemyLevel = 36,
                Team = new List<Fighter>
                {
                    new Fighter { HasParticipated = true, Level = 31, TradeState = TradeState.TradedNational },
                    new Fighter(),
                    new Fighter { HasParticipated = true, HoldsLuckyEgg = true, Level = 66 },
                    new Fighter { HasParticipated = true, Level = 67 },
                    new Fighter { HasParticipated = true, Level = 76 },
                    new Fighter { HoldsExpShare = true, Level = 15, TradeState = TradeState.TradedNational }
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(5, fightInfo);

            Assert.AreEqual(138, result[0].EarnedExperience);
            Assert.AreEqual(0, result[1].EarnedExperience);
            Assert.AreEqual(54, result[2].EarnedExperience);
            Assert.AreEqual(35, result[3].EarnedExperience);
            Assert.AreEqual(29, result[4].EarnedExperience);
            Assert.AreEqual(984, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForFifthGen2()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 77, // Lickitung
                EnemyIsWild = false,
                EnemyLevel = 62,
                Team = new List<Fighter>
                {
                    new Fighter { HasParticipated = true, Level = 31, TradeState = TradeState.TradedNational },
                    new Fighter { HasParticipated = true, Level = 65 },
                    new Fighter(),
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HoldsExpShare = true, Level = 16, TradeState = TradeState.TradedNational }
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(5, fightInfo);

            Assert.AreEqual(1038, result[0].EarnedExperience);
            Assert.AreEqual(339, result[1].EarnedExperience);
            Assert.AreEqual(0, result[2].EarnedExperience);
            Assert.AreEqual(0, result[3].EarnedExperience);
            Assert.AreEqual(0, result[4].EarnedExperience);
            Assert.AreEqual(3073, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForFifthGen3()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 168, // Darmanitan
                EnemyIsWild = false,
                EnemyLevel = 62,
                Team = new List<Fighter>
                {
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HasParticipated = true, HoldsLuckyEgg = true, Level = 66 },
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HoldsExpShare = true, Level = 25, TradeState = TradeState.TradedNational }
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(5, fightInfo);

            Assert.AreEqual(0, result[0].EarnedExperience);
            Assert.AreEqual(0, result[1].EarnedExperience);
            Assert.AreEqual(2178, result[2].EarnedExperience);
            Assert.AreEqual(0, result[3].EarnedExperience);
            Assert.AreEqual(0, result[4].EarnedExperience);
            Assert.AreEqual(5256, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForFifthGen4()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 179, // Mienshao
                EnemyIsWild = true,
                EnemyLevel = 57,
                Team = new List<Fighter>
                {
                    new Fighter { HoldsExpShare = true, Level = 68 },
                    new Fighter { HasParticipated = true, Level = 65 },
                    new Fighter { HasParticipated = true, HoldsLuckyEgg = true, Level = 67 },
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HoldsExpShare = true, Level = 27, TradeState = TradeState.TradedNational }
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(5, fightInfo);

            Assert.AreEqual(413, result[0].EarnedExperience);
            Assert.AreEqual(437, result[1].EarnedExperience);
            Assert.AreEqual(631, result[2].EarnedExperience);
            Assert.AreEqual(0, result[3].EarnedExperience);
            Assert.AreEqual(0, result[4].EarnedExperience);
            Assert.AreEqual(1530, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForFifthGen5()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 125, // Tranquill
                EnemyIsWild = true,
                EnemyLevel = 55,
                Team = new List<Fighter>
                {
                    new Fighter { HoldsExpShare = true, Level = 68 },
                    new Fighter(),
                    new Fighter { HasParticipated = true, HoldsLuckyEgg = true, Level = 67 },
                    new Fighter { HasParticipated = true, Level = 34, TradeState = TradeState.TradedNational },
                    new Fighter { HasParticipated = true, Level = 76 },
                    new Fighter { HoldsExpShare = true, Level = 28, TradeState = TradeState.TradedNational }
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(5, fightInfo);

            Assert.AreEqual(266, result[0].EarnedExperience);
            Assert.AreEqual(0, result[1].EarnedExperience);
            Assert.AreEqual(271, result[2].EarnedExperience);
            Assert.AreEqual(556, result[3].EarnedExperience);
            Assert.AreEqual(154, result[4].EarnedExperience);
            Assert.AreEqual(973, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForFifthGen6()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 166, // Sawsbuck
                EnemyIsWild = true,
                EnemyLevel = 55,
                Team = new List<Fighter>
                {
                    new Fighter { HasParticipated = true, HoldsExpShare = true, Level = 68 },
                    new Fighter { HasParticipated = true, Level = 66 },
                    new Fighter { HasParticipated = true, HoldsLuckyEgg = true, Level = 67 },
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HoldsExpShare = true, Level = 28, TradeState = TradeState.TradedNational }
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(5, fightInfo);

            Assert.AreEqual(588, result[0].EarnedExperience);
            Assert.AreEqual(245, result[1].EarnedExperience);
            Assert.AreEqual(360, result[2].EarnedExperience);
            Assert.AreEqual(0, result[3].EarnedExperience);
            Assert.AreEqual(0, result[4].EarnedExperience);
            Assert.AreEqual(1294, result[5].EarnedExperience);
        }

        [Test]
        public void ShouldCalculateTeamResultCorrectForFifthGen7()
        {
            var fightInfo = new FightInformation
            {
                EnemyBaseExperience = 179, // Mienshao
                EnemyIsWild = true,
                EnemyLevel = 56,
                Team = new List<Fighter>
                {
                    new Fighter { HasParticipated = true, HoldsExpShare = true, Level = 68 },
                    new Fighter { HasParticipated = true, Level = 66 },
                    new Fighter(),
                    new Fighter(),
                    new Fighter(),
                    new Fighter { HasParticipated = true, HoldsExpShare = true, Level = 29, TradeState = TradeState.TradedNational }
                }
            };

            IList<Fighter> result = _service.CalculateBattleResult(5, fightInfo);

            Assert.AreEqual(661, result[0].EarnedExperience);
            Assert.AreEqual(275, result[1].EarnedExperience);
            Assert.AreEqual(0, result[2].EarnedExperience);
            Assert.AreEqual(0, result[3].EarnedExperience);
            Assert.AreEqual(0, result[4].EarnedExperience);
            Assert.AreEqual(2341, result[5].EarnedExperience);
        }
    }
}
