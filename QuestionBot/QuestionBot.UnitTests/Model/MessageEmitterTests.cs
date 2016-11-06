﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuestionBot.Model;
using Moq;

namespace QuestionBot.UnitTests.Model {
    [TestFixture]
    internal class MessageEmitterTests {
        private MessageEmitter _testEmitter;
        private Mock<IMessageListener> _testListener;
        private Mock<IConsole> _testConsole;

        [SetUp]
        public void Setup() {
            _testEmitter = new MessageEmitter();
            _testListener = new Mock<IMessageListener>(MockBehavior.Strict);
            _testConsole = new Mock<IConsole>();
        }

        [Test]
        public void Receiving_Message_Will_Notify_Listener() {
            const string message = "/question What is 1+1?";
            const string output = "Thank you";

            _testConsole.Setup(x => x.ReadLine()).Returns(message);
            _testListener.Setup(x => x.ReceiveMessage(message)).Returns(output);

            _testEmitter.Add(_testListener.Object);
            _testEmitter.Start(_testConsole.Object);

            _testListener.Verify(x => x.ReceiveMessage(message), Times.Exactly(1));
        }
    }
}