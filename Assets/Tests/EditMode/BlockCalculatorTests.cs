using System;
using System.Collections.Generic;
using NUnit.Framework;
using CardSelectionSystem.Core.Distribution;
using CardSelectionSystem.Core.Models;

namespace CardSelectionSystem.Tests
{
    [TestFixture]
    public class BlockCalculatorTests
    {
        private BlockCalculator _calculator;

        [SetUp]
        public void SetUp()
        {
            _calculator = new BlockCalculator();
        }

        [Test]
        public void TwoCards_ReturnsCorrectBlocks()
        {
            var blocks = _calculator.CalculateBlocks(2, 50);

            Assert.AreEqual(2, blocks.Count);
            Assert.AreEqual(0, blocks[0].Start);
            Assert.AreEqual(24, blocks[0].End);
            Assert.AreEqual(25, blocks[1].Start);
            Assert.AreEqual(49, blocks[1].End);
        }

        [Test]
        public void FiveCards_ReturnsEvenBlocks()
        {
            var blocks = _calculator.CalculateBlocks(5, 50);

            Assert.AreEqual(5, blocks.Count);
            Assert.AreEqual(0, blocks[0].Start);
            Assert.AreEqual(9, blocks[0].End);
            Assert.AreEqual(10, blocks[1].Start);
            Assert.AreEqual(19, blocks[1].End);
            Assert.AreEqual(20, blocks[2].Start);
            Assert.AreEqual(29, blocks[2].End);
            Assert.AreEqual(30, blocks[3].Start);
            Assert.AreEqual(39, blocks[3].End);
            Assert.AreEqual(40, blocks[4].Start);
            Assert.AreEqual(49, blocks[4].End);
        }

        [Test]
        public void SevenCards_FirstAndLastBlockCorrect()
        {
            var blocks = _calculator.CalculateBlocks(7, 50);

            Assert.AreEqual(7, blocks.Count);
            Assert.AreEqual(0, blocks[0].Start);
            Assert.AreEqual(6, blocks[0].End);
            Assert.AreEqual(42, blocks[6].Start);
            Assert.AreEqual(49, blocks[6].End);
        }

        [Test]
        public void AllBlocks_CoverFullRange_NoGaps()
        {
            int[] counts = { 2, 3, 4, 5, 7 };

            foreach (int count in counts)
            {
                var blocks = _calculator.CalculateBlocks(count, 50);

                Assert.AreEqual(0, blocks[0].Start, $"Count {count}: first block should start at 0");
                Assert.AreEqual(49, blocks[blocks.Count - 1].End, $"Count {count}: last block should end at 49");

                for (int i = 1; i < blocks.Count; i++)
                {
                    Assert.AreEqual(
                        blocks[i - 1].End + 1,
                        blocks[i].Start,
                        $"Count {count}: gap between block {i - 1} and {i}");
                }
            }
        }

        [Test]
        public void SingleCard_OneBlockCoversAll()
        {
            var blocks = _calculator.CalculateBlocks(1, 50);

            Assert.AreEqual(1, blocks.Count);
            Assert.AreEqual(0, blocks[0].Start);
            Assert.AreEqual(49, blocks[0].End);
        }

        [Test]
        public void CountEqualsPositions_EachBlockWidthOne()
        {
            var blocks = _calculator.CalculateBlocks(50, 50);

            Assert.AreEqual(50, blocks.Count);

            for (int i = 0; i < 50; i++)
            {
                Assert.AreEqual(i, blocks[i].Start, $"Block {i} start");
                Assert.AreEqual(i, blocks[i].End, $"Block {i} end");
                Assert.AreEqual(1, blocks[i].Width, $"Block {i} width");
            }
        }

        [Test]
        public void ZeroCount_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _calculator.CalculateBlocks(0, 50));
        }

        [Test]
        public void NegativeCount_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _calculator.CalculateBlocks(-1, 50));
        }

        [Test]
        public void CountExceedsPositions_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _calculator.CalculateBlocks(51, 50));
        }

        [Test]
        public void ThreeCards_BlocksMatchExpected()
        {
            var blocks = _calculator.CalculateBlocks(3, 50);

            Assert.AreEqual(3, blocks.Count);
            Assert.AreEqual(0, blocks[0].Start);
            Assert.AreEqual(15, blocks[0].End);
            Assert.AreEqual(16, blocks[1].Start);
            Assert.AreEqual(32, blocks[1].End);
            Assert.AreEqual(33, blocks[2].Start);
            Assert.AreEqual(49, blocks[2].End);
        }

        [Test]
        public void FourCards_BlocksMatchExpected()
        {
            var blocks = _calculator.CalculateBlocks(4, 50);

            Assert.AreEqual(4, blocks.Count);
            Assert.AreEqual(0, blocks[0].Start);
            Assert.AreEqual(11, blocks[0].End);
            Assert.AreEqual(12, blocks[1].Start);
            Assert.AreEqual(24, blocks[1].End);
            Assert.AreEqual(25, blocks[2].Start);
            Assert.AreEqual(36, blocks[2].End);
            Assert.AreEqual(37, blocks[3].Start);
            Assert.AreEqual(49, blocks[3].End);
        }
    }
}
