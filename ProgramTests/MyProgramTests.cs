using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramNS;
using System;

namespace ProgramTests
{
    [TestClass]
    public class MyProgramTests
    {
        [TestMethod]
        public void RemoveEcho()
        {
            //Arrange
            var server1 = new Server();
            string expected = "Hello world!";

            //Act

            string actual = server1.Process("ECHO Hello world!");

            //Assert

            Assert.AreEqual(expected, actual, "ECHO not removed!");
        }

        [TestMethod]
        public void AnswerToPing()
        {
            //Arrange
            var server1 = new Server();
            string expected = "Pong";

            //Act

            string actual = server1.Process("PING");

            //Assert

            Assert.AreEqual(expected, actual, "Pong not responded!");
        }

        [TestMethod]
        public void AnswerBadRequest()
        {
            //Arrange
            var server1 = new Server();
            string expected = "Bad request";

            //Act

            string actual = server1.Process("whatever");

            //Assert

            Assert.AreEqual(expected, actual, "Bad Request not responded!");
        }

        [TestMethod]
        public void CapitalizeHelloWorld()
        {
            //Arrange
            var server1 = new Server("TOUPPER");
            string expected = "HELLO WORLD!";

            //Act

            string actual = server1.Process("ECHO Hello world!");

            //Assert

            Assert.AreEqual(expected, actual, "Hello world not capitalized!");
        }

        [TestMethod]
        public void CapitalizePong()
        {
            //Arrange
            var server1 = new Server("TOUPPER");
            string expected = "PONG";

            //Act

            string actual = server1.Process("PING");

            //Assert

            Assert.AreEqual(expected, actual, "PONG not capitalized!");
        }

        [TestMethod]
        public void AddTimeHelloWorld()
        {
            //Arrange
            var server1 = new Server("TIME");
            string localDate = DateTime.Now.ToString("h:mm:ss");
            string expected = localDate + " "+ "Hello world!";

            //Act

            string actual = server1.Process("ECHO Hello world!");

            //Assert

            Assert.AreEqual(expected, actual, "TimeStamp not added properly");
        }

        [TestMethod]
        public void AddTimePong()
        {
            //Arrange
            var server1 = new Server("TIME");
            string localDate = DateTime.Now.ToString("h:mm:ss");
            string expected = localDate + " " + "Pong";

            //Act

            string actual = server1.Process("PING");

            //Assert

            Assert.AreEqual(expected, actual, "TimeStamp not added properly");
        }

        [TestMethod]
        public void AddTimeAndCapitalizeHelloWorld()
        {
            //Arrange
            var server1 = new Server("TIME", "TOUPPER");
            string localDate = DateTime.Now.ToString("h:mm:ss");
            string expected = localDate + " " + "HELLO WORLD!";

            //Act

            string actual = server1.Process("ECHO Hello world!");

            //Assert

            Assert.AreEqual(expected, actual, "Capitalization and TimeStamp at once does not work with Echo");
        }

        [TestMethod]
        public void AddTimeAndCapitalizePong()
        {
            //Arrange
            var server1 = new Server("TIME", "TOUPPER");
            string localDate = DateTime.Now.ToString("h:mm:ss");
            string expected = localDate + " " + "PONG";

            //Act

            string actual = server1.Process("PING");

            //Assert

            Assert.AreEqual(expected, actual, "Capitalization and TimeStamp at once does not work with PONG");
        }
    }
}
