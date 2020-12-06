using Xunit;

namespace Puzzle5_tests
{
    public class UnitTest1
    {
        [Fact]
        public void TestGetTicketId()
        {
            Assert.Equal(actual: Puzzle5.Program.GetTicketId("FBFBBFFRLR"), expected:  357);
            Assert.Equal(actual: Puzzle5.Program.GetTicketId("BFFFBBFRRR"), expected:  567);
            Assert.Equal(actual: Puzzle5.Program.GetTicketId("FFFBBBFRRR"), expected:  119);
            Assert.Equal(actual: Puzzle5.Program.GetTicketId("BBFFBBFRLL"), expected:  820);

        }
    }
}
