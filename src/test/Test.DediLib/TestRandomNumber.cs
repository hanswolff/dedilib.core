﻿using System.Linq;
using DediLib;
using Xunit;

namespace Test.DediLib
{
    public class TestRandomNumber
    {
        [Fact]
        public void Next_PositiveInteger()
        {
            var randomNumbers = Enumerable.Range(0, 10000).Select(i => RandomNumber.Next()).ToList();

            randomNumbers.ForEach(r => Assert.True(r >= 0));
        }

        [Fact]
        public void Next_LowerMaxValue()
        {
            const int maxValue = 5;

            var randomNumbers = Enumerable.Range(0, 10000).Select(i => RandomNumber.Next(maxValue)).ToList();

            randomNumbers.ForEach(r => Assert.True(r >= 0 && r < maxValue));
            Assert.True(randomNumbers.Any(r => r == maxValue - 1));
        }

        [Fact]
        public void Next_MinValueMaxValue_GreaterThanMinValueAndLowerThanMaxValue()
        {
            const int minValue = -5;
            const int maxValue = 5;

            var randomNumbers = Enumerable.Range(0, 10000).Select(i => RandomNumber.Next(minValue, maxValue)).ToList();

            randomNumbers.ForEach(r => Assert.True(r >= minValue && r < maxValue));
            Assert.True(randomNumbers.Any(r => r == minValue));
            Assert.True(randomNumbers.Any(r => r == maxValue - 1));
        }
    }
}
