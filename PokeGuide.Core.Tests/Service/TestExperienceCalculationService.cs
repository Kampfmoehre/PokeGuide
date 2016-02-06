using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PokeGuide.Core.Enum;
using PokeGuide.Core.Service;
using PokeGuide.Core.Service.Interface;

namespace PokeGuide.Core.Tests.Service
{
    public class TestExperienceCalculationService
    {
        IExperienceCalculationService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new ExperienceCalculationService();
        }

        [TestCase((ushort)66, 5, 1, false, false, false, 0, ExpectedResult = 70, Description = "Trainer Tortoise Level 5")]
        [TestCase((ushort)60, 3, 2, true, false, false, 0, ExpectedResult = 12, Description = "Wild Nidoran M Level 3 defeated by 2 Pokémon")]
        [TestCase((ushort)55, 9, 2, false, false, false, 0, ExpectedResult = 51, Description = "Trainer Pidgey Level 9 defeated by 2 Pokémon")]
        [TestCase((ushort)53, 6, 1, false, false, false, 0, ExpectedResult = 67, Description = "Trainer Caterpie Level 6")]
        [TestCase((ushort)64, 5, 1, false, false, false, 0, ExpectedResult = 67, Description = "Trainer Bulbasaur Level 5")]
        [TestCase((ushort)91, 5, 1, false, false, false, 0, ExpectedResult = 97, Description = "Trainer Eevee Level 5")]
        [TestCase((ushort)59, 6, 2, false, false, false, 0, ExpectedResult = 36, Description = "Trainer Nidoran F Level 6 defeated by 2 Pokémon")]
        [TestCase((ushort)151, 30, 1, true, true, false, 0, ExpectedResult = 970, Description = "Wild Weepinbell Level 30 defeated by one traded Pokémon")]
        [TestCase((ushort)75, 28, 1, true, false, false, 0, ExpectedResult = 300, Description = "Wild Venonat Level 28 defeated by one Pokémon")]
        [TestCase((ushort)176, 54, 1, false, false, false, 0, ExpectedResult = 2035, Description = "Trainer Dewgong Level 54 defeated by one Pokémon")]
        [TestCase((ushort)203, 53, 1, false, true, false, 0, ExpectedResult = 3457, Description = "Trainer Cloyster Level 53 defeated by one traded Pokémon")]
        [TestCase((ushort)164, 54, 2, false, true, false, 0, ExpectedResult = 1422, Description = "Trainer Slowbro Level 54 defeated by 2 traded Pokémon")]
        [TestCase((ushort)137, 56, 3, false, true, false, 0, ExpectedResult = 810, Description = "Trainer Jynx Level 56 defeated by 3 traded Pokémon")]
        [TestCase((ushort)219, 56, 4, false, true, false, 0, ExpectedResult = 972, Description = "Trainer Lapras Level 56 defeated by 3 traded and one non-traded Pokémon (output for traded Pokémon)")]
        [TestCase((ushort)219, 56, 4, false, false, false, 0, ExpectedResult = 648, Description = "Trainer Lapras Level 56 defeated by 3 traded and one non-traded Pokémon (output for non-traded Pokémon)")]
        [TestCase((ushort)151, 30, 1, true, true, true, 0, ExpectedResult = 481, Description = "Wild Weepinbell Level 30 defeated by one traded Pokémon with Exp.All")]
        [TestCase((ushort)75, 28, 1, true, false, true, 0, ExpectedResult = 148, Description = "Wild Venonat Level 28 defeated by one Pokémon with Exp.All")]
        [TestCase((ushort)176, 54, 1, false, false, true, 0, ExpectedResult = 1017, Description = "Trainer Dewgong Level 54 defeated by one Pokémon in with Exp.All")]
        [TestCase((ushort)203, 53, 2, false, true, true, 0, ExpectedResult = 850, Description = "Trainer Cloyster Level 53 defeated by one traded Pokémon with Exp.All")]
        [TestCase((ushort)164, 54, 3, false, true, true, 0, ExpectedResult = 468, Description = "Trainer Slowbro Level 54 defeated by 3 traded Pokémon with Exp.All")]
        [TestCase((ushort)137, 56, 1, false, true, true, 0, ExpectedResult = 1224, Description = "Trainer Jynx Level 56 defeated by 1 traded Pokémon with Exp.All")]
        [TestCase((ushort)219, 56, 4, false, true, true, 0, ExpectedResult = 486, Description = "Trainer Lapras Level 56 defeated by one traded Pokémon with Exp.All (output for traded Pokémon)")]
        [TestCase((ushort)219, 56, 4, false, false, true, 0, ExpectedResult = 324, Description = "Trainer Lapras Level 56 defeated by 3 traded and one non-traded Pokémon with Exp.All (output for non-traded Pokémon)")]
        [TestCase((ushort)219, 56, 4, false, false, true, 6, ExpectedResult = 48, Description = "Trainer Lapras Level 56 defeated by 3 traded and one non-traded Pokémon with Exp.All (output from Exp.All)")]
        [TestCase((ushort)164, 54, 3, false, true, true, 6, ExpectedResult = 67, Description = "Trainer Slowbro Level 54 defeated by 3 traded Pokémon with Exp.All (output from Exp.All.)")]
        [TestCase((ushort)116, 27, 1, true, true, true, 0, ExpectedResult = 334, Description = "Wild Raticate Level 27 defeated by one traded Pokémon with Exp.All")]
        [TestCase((ushort)116, 27, 1, true, true, true, 5, ExpectedResult = 63, Description = "Wild Raticate Level 27 defeated by one traded Pokémon with Exp.All and 5 Team members (output from Exp.All.)")]
        [TestCase((ushort)116, 27, 1, true, true, true, 6, ExpectedResult = 51, Description = "Wild Raticate Level 27 defeated by one traded Pokémon with Exp.All (output from Exp.All.)")]
        [TestCase((ushort)58, 20, 1, true, true, true, 3, ExpectedResult = 37, Description = "Wild Spearow Level 20 defeated by one traded Pokémon with Exp.All and 3 Team members (output from Exp.All.)")]
        public int ShouldCalculateExperienceForGen1(ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild, bool isTraded, bool useExpShare, byte teamCount)
        {
            return _service.CalculateExperienceForFirstGen(baseExperience, enemyLevel, participatedPokemon, isWild, isTraded, useExpShare, teamCount);
        }

        [TestCase((ushort)57, 3, 1, true, false, false, 0, false, ExpectedResult = 24, Description = "Wild Sentret Level 3 defeated by one Pokémon")]
        [TestCase((ushort)189, 33, 1, true, false, false, 0, false, ExpectedResult = 891, Description = "Wild Donphan Level 33 defeated by one Pokémon")]
        [TestCase((ushort)189, 33, 1, true, false, false, 1, false, ExpectedResult = 443, Description = "Wild Donphan Level 33 defeated by one Pokémon while another Pokémon holds Exp.Share")]
        [TestCase((ushort)86, 3, 1, true, false, false, 0, false, ExpectedResult = 36, Description = "Wild Geodude Level 3 defeated by one Pokémon")]
        public int ShouldCalculateExperienceForGen2(ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild, bool isTraded, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare)
        {
            return _service.CalculateExperienceForSecondGen(baseExperience, enemyLevel, participatedPokemon, isWild, isTraded, holdsLuckyEgg, expShareCount, holdsExpShare);
        }

        [TestCase((ushort)55, 2, 1, true, false, false, 0, false, ExpectedResult = 15, Description = "Wild Poochyena Level 2 defeated by one Pokémon")]
        [TestCase((ushort)60, 3, 1, true, false, false, 0, false, ExpectedResult = 25, Description = "Wild Zigzagoon Level 3 defeated by one Pokémon")]
        [TestCase((ushort)74, 4, 1, true, false, false, 0, false, ExpectedResult = 42, Description = "Wild Mankey Level 4 defeated by one Pokémon")]
        [TestCase((ushort)65, 5, 1, false, false, false, 0, false, ExpectedResult = 69, Description = "Trainer Torchic Level 5 defeated by one Pokémon")]
        public int ShouldCalculateExperienceForGen3(ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild, bool isTraded, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare)
        {
            return _service.CalculateExperienceForThirdGen(baseExperience, enemyLevel, participatedPokemon, isWild, isTraded, holdsLuckyEgg, expShareCount, holdsExpShare);
        }

        [TestCase((ushort)58, 2, 1, true, TradeState.Original, false, 0, false, ExpectedResult = 16, Description = "Wild Bidoof Level 2 defeated by one Pokémon")]
        [TestCase((ushort)63, 5, 1, false, TradeState.Original, false, 0, false, ExpectedResult = 67, Description = "Trainer Turtwig Level 5 defeated by one Pokémon")]
        //[TestCase((ushort)177, 23, 1, true, TradeState.Original, false, 0, false, ExpectedResult = 384, Description = "Wild Nidorina Level 23 defeated by one Pokémon")]
        public int ShouldCalculateExperienceForGen4(ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild, TradeState tradeState, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare)
        {
            return _service.CalculateExperienceForFourthGen(baseExperience, enemyLevel, participatedPokemon, isWild, tradeState, holdsLuckyEgg, expShareCount, holdsExpShare);
        }

        [TestCase((ushort)55, 4, 5, 1, true, TradeState.Original, false, 0, false, ExpPower.None, ExpectedResult = 39, Description = "Wild Lillipup Level 4 defeated by one Level 5 Pokémon")]
        [TestCase((ushort)56, 8, 8, 1, false, TradeState.Original, false, 0, false, ExpPower.None, ExpectedResult = 135, Description = "Trainer Purrloin Level 8 defeated by one Level 8 Pokémon")]
        [TestCase((ushort)51, 4, 7, 1, false, TradeState.Original, false, 0, false, ExpPower.None, ExpectedResult = 42, Description = "Trainer Patrat Level 4 defeated by one Level 7 Pokémon")]
        [TestCase((ushort)38, 5, 5, 1, true, TradeState.Original, false, 0, false, ExpPower.None, ExpectedResult = 39, Description = "Wild Azurill Level 5 defeated by one Level 5 Pokémon")]
        [TestCase((ushort)64, 5, 6, 1, true, TradeState.Original, false, 0, false, ExpPower.None, ExpectedResult = 57, Description = "Wild Psyduck Level 5 defeated by one Level 6 Pokémon")]
        [TestCase((ushort)163, 62, 67, 2, false, TradeState.Original, false, 0, false, ExpPower.None, ExpectedResult = 1383, Description = "Trainer Sawk Level 62 defeated by two Pokémon Level 67")]
        [TestCase((ushort)163, 62, 31, 2, false, TradeState.TradedNational, false, 0, false, ExpPower.None, ExpectedResult = 4387, Description = "Trainer Throh Level 62 defeated by two Pokémon traded Level 31")]
        [TestCase((ushort)163, 62, 76, 2, false, TradeState.Original, false, 0, false, ExpPower.None, ExpectedResult = 1182, Description = "Trainer Sawk Level 62 defeated by two Pokémon Level 67")]
        [TestCase((ushort)163, 62, 32, 2, false, TradeState.TradedNational, false, 0, false, ExpPower.None, ExpectedResult = 4282, Description = "Trainer Throh Level 62 defeated by two Pokémon traded Level 32")]
        [TestCase((ushort)166, 62, 66, 1, false, TradeState.Original, true, 0, false, ExpPower.None, ExpectedResult = 4303, Description = "Trainer Hariyama Level 62 defeated by one Level 66 Pokémon")]
        public int ShouldCalculateExperienceForGen5(ushort baseExperience, byte enemyLevel, byte ownLevel, byte participatedPokemon, bool isWild, TradeState tradeState, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare, ExpPower expPowerState)
        {
            return _service.CalculateExperienceForFifthGen(baseExperience, enemyLevel, ownLevel, participatedPokemon, isWild, tradeState, holdsLuckyEgg, expShareCount, holdsExpShare, expPowerState);
        }
    }
}
