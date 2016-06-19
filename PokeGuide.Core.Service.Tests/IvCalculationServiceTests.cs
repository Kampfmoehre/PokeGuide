using NUnit.Framework;

namespace PokeGuide.Core.Calculations.Tests
{
    class IvCalculationServiceTests
    {
        IIvCalculationService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new IvCalculationService(new StatCalculationService());
        }
    }
}
