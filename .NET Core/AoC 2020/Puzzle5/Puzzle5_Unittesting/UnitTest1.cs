using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Program;

namespace Program.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetTicketId()
        {
            Program P = new Program();
            int nResult = GetTicketId("FBFBBFFRLR");

            Assert.AreEqual(nResult,44);

        }
    }
}
