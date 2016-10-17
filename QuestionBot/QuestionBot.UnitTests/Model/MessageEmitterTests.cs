using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuestionBot.Model;
using Moq;

namespace QuestionBot.UnitTests.Model {
    [TestFixture]
    internal class MessageEmitterTests {
        private MessageEmitter _testEmitter;
        private Mock<IMessageListener> _testMessageListener;
        private Mock<IStore> _storeTest;
        private Mock<QuestionMessageListener> _questionListener;

        [SetUp]
        public void Setup() {
            _testEmitter = new MessageEmitter();
            _storeTest = new Mock<IStore>(MockBehavior.Strict);
        }

        [Test]
        public void Calling_Add_Will_Add_Listener_To_List() {

        }
    }
}