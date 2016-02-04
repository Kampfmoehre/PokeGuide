using System;
using NUnit.Framework;
using PokeGuide.Core.Service;
using PokeGuide.Core.Service.Interface;

namespace PokeGuide.Core.Tests.Service
{
    public class TestCalculationService
    {
        ICalculationService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new CalculationService();
        }

        [TestCase((byte)35, (byte)81, (ushort)22850, (byte)7, (byte)1, ExpectedResult = 189, Description = "HitPoints of Level 81 Pikachu in Gen I")]
        [TestCase((byte)108, (byte)78, (ushort)74, (byte)24, (byte)3, ExpectedResult = 289, Description = "HitPoints of Level 78 Garchomp in Gen III")]
        [TestCase((byte)95, (byte)100, (ushort)156, (byte)31, (byte)4, ExpectedResult = 370, Description = "HitPoints of Level 100 Gyarados in Gen IV")]
        public ushort ShouldCalculateHitPointsCorrect(byte baseHitPoints, byte level, ushort ev, byte hpIv, byte generation)
        {
            return _service.CalculateHitPoints(baseHitPoints, level, ev, hpIv, generation);
        }

        [Test]
        public void CalculateHitPointsShouldThrowExceptionIfWrongGeneration()
        {
            byte wrongGen = 7;
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _service.CalculateHitPoints(1, 2, 3, 4, wrongGen));
            // Todo find out how to remove default .NET exception that is added to the custom message
            Assert.True(exception.Message.StartsWith("Generation must be between 1 and 6"));
            Assert.AreEqual("generation", exception.ParamName);
            Assert.AreEqual(wrongGen, exception.ActualValue);
        }

        [TestCase((byte)55, (byte)81, (ushort)23140, (byte)8, (byte)1, 1.0, ExpectedResult = 137, Description = "Calculate Attack of Level 81 Pikachu in Gen I")]
        [TestCase((byte)30, (byte)81, (ushort)17280, (byte)13, (byte)1, 1.0, ExpectedResult = 101, Description = "Calculate Defense of Level 81 Pikachu in Gen I")]
        [TestCase((byte)50, (byte)81, (ushort)19625, (byte)9, (byte)2, 1.0, ExpectedResult = 128, Description = "Calculate Special Attack of Level 81 Pikachu in Gen II")]
        [TestCase((byte)90, (byte)81, (ushort)24795, (byte)5, (byte)2, 1.0, ExpectedResult = 190, Description = "Calculate Speed of Level 81 Pikachu in Gen II")]
        [TestCase((byte)135, (byte)100, (ushort)252, (byte)31, (byte)3, 1.1, ExpectedResult = 405, Description = "Calculate Attack of Level 100 Salamence with Adamant nature in Gen III")]
        [TestCase((byte)95, (byte)78, (ushort)86, (byte)30, (byte)4, 1.0, ExpectedResult = 193, Description = "Calculate Defense of Level 78 Garchomp in Gen IV")]
        [TestCase((byte)80, (byte)78, (ushort)48, (byte)16, (byte)5, 0.9, ExpectedResult = 136, Description = "Calculate Attack of Level 78 Garchomp with Adamant nature in Gen V")]
        public ushort ShouldCalculateStatCorrect(byte baseStat, byte level, ushort ev, byte iv, byte generation, double natureMod)
        {
            return _service.CalculateStat(baseStat, level, ev, iv, generation, natureMod);
        }

        [Test]
        public void CalculateStatShouldThrowExceptionIfWrongGeneration()
        {
            byte wrongGen = 7;
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _service.CalculateStat(1, 2, 3, 4, wrongGen, 1));
            // Todo find out how to remove default .NET exception that is added to the custom message
            Assert.True(exception.Message.StartsWith("Generation must be between 1 and 6"));
            Assert.AreEqual("generation", exception.ParamName);
            Assert.AreEqual(wrongGen, exception.ActualValue);
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
            return _service.CalculateExperience(1, baseExperience, enemyLevel, participatedPokemon, isWild, isTraded, false, useExpShare, teamCount);
        }

        [TestCase((ushort)57, 3, 1, true, false, false, false, ExpectedResult = 24, Description = "Wild Sentret Level 3 defeated by one Pokémon")]
        [TestCase((ushort)189, 33, 1, true, false, false, false, ExpectedResult = 891, Description = "Wild Donphan Level 33 defeated by one Pokémon")]
        [TestCase((ushort)189, 33, 1, true, false, false, true, ExpectedResult = 443, Description = "Wild Donphan Level 33 defeated by one Pokémon")]
        public int ShouldCalculateExperienceForGen2(ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild, bool isTraded, bool holdsLuckyEgg, bool useExpShare)
        {
            return _service.CalculateExperience(2, baseExperience, enemyLevel, participatedPokemon, isWild, isTraded, holdsLuckyEgg, useExpShare);
        }


        //
        ////[TestCase((byte)2, (ushort)54, (byte)2, (byte)1, true, false, false, false, 0, ExpectedResult = 11, Description = "Wild Spinarak Level 2 in Gen II")]
        //[TestCase((byte)2, (ushort)86, (byte)3, (byte)1, true, false, false, false, 0, ExpectedResult = 36, Description = "Wild Geodude Level 3 in Gen II")]
        //[TestCase((byte)3, (ushort)55, (byte)2, (byte)1, true, false, false, false, 0, ExpectedResult = 15, Description = "Wild Poochyena Level 2 in Gen III")]
        //[TestCase((byte)3, (ushort)60, (byte)3, (byte)1, true, false, false, false, 0, ExpectedResult = 25, Description = "Wild Zigzagoon Level 3 in Gen III")]
        //[TestCase((byte)3, (ushort)74, (byte)4, (byte)1, true, false, false, false, 0, ExpectedResult = 42, Description = "Wild Mankey Level 4 in Gen III")]
        //[TestCase((byte)3, (ushort)65, (byte)5, (byte)1, false, false, false, false, 0, ExpectedResult = 69, Description = "Trainer Torchic Level 5 in Gen III")]
        //[TestCase((byte)4, (ushort)58, (byte)2, (byte)1, true, false, false, false, 0, ExpectedResult = 16, Description = "Wild Bidoof Level 2 in Gen IV")]
        //[TestCase((byte)4, (ushort)63, (byte)5, (byte)1, false, false, false, false, 0, ExpectedResult = 67, Description = "Trainer Turtwig Level 5 in Gen IV")]
    }
}
