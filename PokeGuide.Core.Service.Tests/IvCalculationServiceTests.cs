using System.Collections.Generic;

using NUnit.Framework;

namespace PokeGuide.Core.Calculations.Tests
{
    class IvCalculationServiceTests
    {
        IIvCalculationService _service;
        private List<byte> oldIvs = new List<byte>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        private List<byte> newIvs = new List<byte>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };

        [SetUp]
        public void SetUp()
        {
            _service = new IvCalculationService(new StatCalculationService());
        }
        
        [Test]
        public void TestCalculateHitPointsIv_Level92Rhydon()
        {
            List<byte> result = _service.CalculateHitPointsIv(105, 348, 92, 12345, oldIvs, 1);
            Assert.AreEqual(new List<byte> { 15 }, result);
        }
        [Test]
        public void TestCalculateHitPointsIv_Level96Voltorb()
        {
            List<byte> result = _service.CalculateHitPointsIv(40, 219, 96, 9543, oldIvs, 1);
            Assert.AreEqual(new List<byte> { 7 }, result);
        }
        [Test]
        public void TestCalculateHitPointsIv_Level20Lapras()
        {
            List<byte> result = _service.CalculateHitPointsIv(130, 82, 20, 123, oldIvs, 1);
            Assert.AreEqual(new List<byte> { 0 }, result);
        }
        [Test]
        public void TestCalculateHitPointsIv_Level53Ariados()
        {
            List<byte> result = _service.CalculateHitPointsIv(70, 150, 53, 4576, oldIvs, 2);
            Assert.AreEqual(new List<byte> { 4 }, result);
        }
        [Test]
        public void TestCalculateHitPointsIv_Level78Sunflora()
        {
            List<byte> result = _service.CalculateHitPointsIv(75, 265, 78, 56213, oldIvs, 2);
            Assert.AreEqual(new List<byte> { 9 }, result);
        }
        [Test]
        public void TestCalculateHitPointsIv_Level24Lunatone()
        {
            List<byte> result = _service.CalculateHitPointsIv(70, 76, 24, 16, newIvs, 3);
            Assert.AreEqual(new List<byte> { 31 }, result);
        }
        [Test]
        public void TestCalculateHitPointsIv_Level96Kadabra()
        {
            List<byte> result = _service.CalculateHitPointsIv(40, 207, 96, 70, newIvs, 4);
            Assert.AreEqual(new List<byte> { 9 }, result);
        }

        [Test]
        public void TestCalculateAttackIv_Level88Cubone()
        {
            List<byte> result = _service.CalculateIv(50, 138, 88, 12345, oldIvs, 1);
            Assert.AreEqual(new List<byte> { 12 }, result);
        }
        [Test]
        public void TestCalculateDefenseIv_Level84Scyther()
        {
            List<byte> result = _service.CalculateIv(80, 173, 84, 6574, oldIvs, 1);
            Assert.AreEqual(new List<byte> { 10 }, result);
        }
        [Test]
        public void TestCalculateSpecialIv_Level38Magmar()
        {
            List<byte> result = _service.CalculateIv(85, 79, 38, 5413, oldIvs, 1);
            Assert.AreEqual(new List<byte> { 4 }, result);
        }
        [Test]
        public void TestCalculateSpecialAttackIv_Level68Poliwhirl()
        {
            List<byte> result = _service.CalculateIv(50, 97, 68, 93, newIvs, 4, 0.9);
            Assert.AreEqual(new List<byte> { 29 }, result);
        }
        [Test]
        public void TestCalculateSpeedIv_Level99Pupitar()
        {
            List<byte> result = _service.CalculateIv(51, 150, 99, 121, newIvs, 3, 1.1);
            Assert.AreEqual(new List<byte> { 2 }, result);
        }
        [Test]
        public void TestCalculateSpecialDefenseIv_Level58Starmie()
        {
            List<byte> result = _service.CalculateIv(85, 121, 58, 102, newIvs, 5, 0.9);
            Assert.AreEqual(new List<byte> { 30 }, result);
        }
        [Test]
        public void TestCalculateSpeedIv_Level60Talonflame()
        {
            List<byte> result = _service.CalculateIv(126, 228, 60, 252, newIvs, 6, 1.1);
            Assert.AreEqual(new List<byte> { 24 }, result);
        }
    }
}
