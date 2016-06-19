using System;
using NUnit.Framework;

namespace PokeGuide.Core.Calculations.Tests
{
    class StatCalculationServiceTests
    {
        IStatCalculationService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new StatCalculationService();
        }

        [TestCase((byte)35, (byte)81, (ushort)22850, (byte)7, (byte)1, ExpectedResult = 189, Description = "HitPoints of Level 81 Pikachu in Gen I")]
        [TestCase((byte)35, (byte)5, (ushort)0, (byte)1, (byte)1, ExpectedResult = 18, Description = "HitPoints of Level 5 Pikachu in Gen I")]
        [TestCase((byte)85, (byte)32, (ushort)0, (byte)0, (byte)2, ExpectedResult = 96, Description = "HitPoints of Level 32 Crobat in Gen II")]
        [TestCase((byte)85, (byte)32, (ushort)0, (byte)4, (byte)2, ExpectedResult = 98, Description = "HitPoints of Level 32 Crobat in Gen II")]
        [TestCase((byte)45, (byte)5, (ushort)0, (byte)0, (byte)2, ExpectedResult = 19, Description = "HitPoints of Level 5 Chikorita in Gen II")]
        [TestCase((byte)45, (byte)100, (ushort)0, (byte)0, (byte)2, ExpectedResult = 200, Description = "HitPoints of Level 100 Chikorita in Gen II")]
        [TestCase((byte)45, (byte)100, (ushort)0, (byte)15, (byte)2, ExpectedResult = 230, Description = "HitPoints of Level 100 Chikorita with 15 HP IV in Gen II")]
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

        [Test]
        public void CalculateHitPointsShouldThrowExceptionIfWrongDvSecondGen()
        {
            byte wrongIv = 16;
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _service.CalculateHitPoints(1, 2, 3, wrongIv, 2));
            // Todo find out how to remove default .NET exception that is added to the custom message
            Assert.True(exception.Message.StartsWith("DV for 1st and 2nd generation can not be higher than 15"));
            Assert.AreEqual("dv", exception.ParamName);
            Assert.AreEqual(wrongIv, exception.ActualValue);
        }

        [Test]
        public void CalculateHitPointsShouldThrowExceptionIfWrongDvFourthGen()
        {
            byte wrongIv = 32;
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _service.CalculateHitPoints(1, 2, 3, wrongIv, 4));
            // Todo find out how to remove default .NET exception that is added to the custom message
            Assert.True(exception.Message.StartsWith("IV can not be higher than 31"));
            Assert.AreEqual("iv", exception.ParamName);
            Assert.AreEqual(wrongIv, exception.ActualValue);
        }

        [TestCase((byte)55, (byte)81, (ushort)23140, (byte)8, (byte)1, 1.0, ExpectedResult = 137, Description = "Calculate Attack of Level 81 Pikachu in Gen I")]
        [TestCase((byte)30, (byte)81, (ushort)17280, (byte)13, (byte)1, 1.0, ExpectedResult = 101, Description = "Calculate Defense of Level 81 Pikachu in Gen I")]
        [TestCase((byte)50, (byte)81, (ushort)19625, (byte)9, (byte)2, 1.0, ExpectedResult = 128, Description = "Calculate Special Attack of Level 81 Pikachu in Gen II")]
        [TestCase((byte)90, (byte)32, (ushort)0, (byte)10, (byte)2, 1.0, ExpectedResult = 69, Description = "Calculate Speed of Level 32 Crobat (Shiny) in Gen II")]
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

        [Test]
        public void CalculateStatShouldThrowExceptionIfWrongDvSecondGen()
        {
            byte wrongIv = 16;
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _service.CalculateStat(1, 2, 3, wrongIv, 2, 1));
            // Todo find out how to remove default .NET exception that is added to the custom message
            Assert.True(exception.Message.StartsWith("DV for 1st and 2nd generation can not be higher than 15"));
            Assert.AreEqual("dv", exception.ParamName);
            Assert.AreEqual(wrongIv, exception.ActualValue);
        }

        [Test]
        public void CalculateStatShouldThrowExceptionIfWrongIvFourthGen()
        {
            byte wrongIv = 32;
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _service.CalculateStat(1, 2, 3, wrongIv, 4, 1));
            // Todo find out how to remove default .NET exception that is added to the custom message
            Assert.True(exception.Message.StartsWith("IV can not be higher than 31"));
            Assert.AreEqual("iv", exception.ParamName);
            Assert.AreEqual(wrongIv, exception.ActualValue);
        }
    }
}
