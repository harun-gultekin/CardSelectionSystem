using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using CardSelectionSystem.Core.Distribution;
using CardSelectionSystem.Core.Models;

namespace CardSelectionSystem.Tests
{
    [TestFixture]
    public class DistributionTests
    {
        private BlockCalculator _blockCalculator;
        private DistributionValidator _validator;
        private List<ItemConfig> _defaultPool;

        [SetUp]
        public void SetUp()
        {
            _blockCalculator = new BlockCalculator();
            _validator = new DistributionValidator(_blockCalculator);
            _defaultPool = ItemPoolFactory.CreateDefaultPool();
        }

        [Test]
        public void Generate_ProducesExactly50Cards()
        {
            var distributor = new CardDistributor(_blockCalculator, seed: 42);
            var result = distributor.Generate(_defaultPool);

            Assert.IsNotNull(result);
            Assert.AreEqual(50, result.Length);
        }

        [Test]
        public void Generate_EachItemAppearsCorrectNumberOfTimes()
        {
            var distributor = new CardDistributor(_blockCalculator, seed: 42);
            var result = distributor.Generate(_defaultPool);

            Assert.IsNotNull(result);

            foreach (var item in _defaultPool)
            {
                int count = result.Count(name => name == item.Name);
                Assert.AreEqual(item.CardsPerCycle, count,
                    $"{item.Name} should appear {item.CardsPerCycle} times, found {count}");
            }
        }

        [Test]
        public void Generate_AllInstancesWithinDesignatedBlocks()
        {
            var distributor = new CardDistributor(_blockCalculator, seed: 42);
            var result = distributor.Generate(_defaultPool);

            Assert.IsNotNull(result);

            var validation = _validator.Validate(result, _defaultPool, 50);
            Assert.IsTrue(validation.IsValid,
                $"Validation failed: {string.Join("; ", validation.Errors)}");
            Assert.AreEqual(50, validation.ValidCount);
        }

        [Test]
        [Timeout(10000)]
        public void Generate_20ConsecutiveRuns_AllValid()
        {
            for (int run = 0; run < 20; run++)
            {
                var distributor = new CardDistributor(_blockCalculator, seed: run);
                var result = distributor.Generate(_defaultPool);

                Assert.IsNotNull(result, $"Run {run}: Generate returned null");
                Assert.AreEqual(50, result.Length, $"Run {run}: wrong length");

                var validation = _validator.Validate(result, _defaultPool, 50);
                Assert.IsTrue(validation.IsValid,
                    $"Run {run}: {string.Join("; ", validation.Errors)}");
            }
        }

        [Test]
        public void Generate_TwoRuns_ProduceDifferentSequences()
        {
            var distributor1 = new CardDistributor(_blockCalculator, seed: 1);
            var distributor2 = new CardDistributor(_blockCalculator, seed: 2);

            var result1 = distributor1.Generate(_defaultPool);
            var result2 = distributor2.Generate(_defaultPool);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);

            bool anyDifference = false;
            for (int i = 0; i < result1.Length; i++)
            {
                if (result1[i] != result2[i])
                {
                    anyDifference = true;
                    break;
                }
            }

            Assert.IsTrue(anyDifference, "Two runs with different seeds should produce different sequences");
        }

        [Test]
        public void Generate_ImpossibleConfig_SumTo51_ReturnsNull()
        {
            var items = new List<ItemConfig>
            {
                new ItemConfig("A", 26, CardTier.Green, "#000000", "A"),
                new ItemConfig("B", 25, CardTier.Green, "#000000", "B"),
            };

            var distributor = new CardDistributor(_blockCalculator, seed: 42);
            var result = distributor.Generate(items, 50);

            Assert.IsNull(result);
        }

        [Test]
        public void Generate_ImpossibleConfig_SumTo49_ReturnsNull()
        {
            var items = new List<ItemConfig>
            {
                new ItemConfig("A", 25, CardTier.Green, "#000000", "A"),
                new ItemConfig("B", 24, CardTier.Green, "#000000", "B"),
            };

            var distributor = new CardDistributor(_blockCalculator, seed: 42);
            var result = distributor.Generate(items, 50);

            Assert.IsNull(result);
        }

        [Test]
        public void Generate_SingleItem_Count50_FillsAll()
        {
            var items = new List<ItemConfig>
            {
                new ItemConfig("OnlyItem", 50, CardTier.Gold, "#fcc325", "OI"),
            };

            var distributor = new CardDistributor(_blockCalculator, seed: 42);
            var result = distributor.Generate(items, 50);

            Assert.IsNotNull(result);
            Assert.AreEqual(50, result.Length);
            Assert.IsTrue(result.All(name => name == "OnlyItem"));
        }

        [Test]
        public void Generate_TwoItemsSplitEvenly_25And25()
        {
            var items = new List<ItemConfig>
            {
                new ItemConfig("Alpha", 25, CardTier.Blue, "#008aff", "Al"),
                new ItemConfig("Beta",  25, CardTier.Blue, "#008aff", "Be"),
            };

            var distributor = new CardDistributor(_blockCalculator, seed: 42);
            var result = distributor.Generate(items, 50);

            Assert.IsNotNull(result);
            Assert.AreEqual(50, result.Length);
            Assert.AreEqual(25, result.Count(n => n == "Alpha"));
            Assert.AreEqual(25, result.Count(n => n == "Beta"));

            var validation = _validator.Validate(result, items, 50);
            Assert.IsTrue(validation.IsValid,
                $"Validation failed: {string.Join("; ", validation.Errors)}");
        }
    }
}
