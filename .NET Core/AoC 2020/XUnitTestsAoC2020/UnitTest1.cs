using System;
using Xunit;
using MemoryGame;


namespace XUnitTestsAoC2020
{
    public class AOC2020Tests
    {
        [Fact]
        public void Puzzlle15TestsShort()
        {
            Assert.Equal(1,         Program.GetNumber(2020, "1,3,2"));
            Assert.Equal(10,        Program.GetNumber(2020, "2,1,3"));
            Assert.Equal(27,        Program.GetNumber(2020, "1,2,3"));
            Assert.Equal(78,        Program.GetNumber(2020, "2,3,1"));
            Assert.Equal(438,       Program.GetNumber(2020, "3,2,1"));
            Assert.Equal(1836,      Program.GetNumber(2020, "3,1,2"));
        }

        [Fact]
        public void Puzzlle15TestsLong()
        {
            Assert.Equal(175594,    Program.GetNumber(30000000, "0,3,6"));
            Assert.Equal(2578,      Program.GetNumber(30000000, "1,3,2"));
            Assert.Equal(3544142,   Program.GetNumber(30000000, "2,1,3"));
            Assert.Equal(261214,    Program.GetNumber(30000000, "1,2,3"));
            Assert.Equal(6895259,   Program.GetNumber(30000000, "2,3,1"));
            Assert.Equal(18,        Program.GetNumber(30000000, "3,2,1"));
            Assert.Equal(362,       Program.GetNumber(30000000, "3,1,2"));
        }
    }
}
