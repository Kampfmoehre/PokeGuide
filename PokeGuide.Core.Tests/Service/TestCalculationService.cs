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
    }
}
