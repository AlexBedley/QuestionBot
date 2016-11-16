using System;
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

            _testConsole.SetupSequence( x => x.ReadLine() )
                .Returns( message )
                .Returns( "/exitQuestionBot" );

            _testListener.Setup(x => x.ReceiveMessage(message)).Returns(output);
            _testListener.Setup(x => x.ReceiveMessage("/exitQuestionBot")).Returns(output);

            _testEmitter.Add(_testListener.Object);
            _testEmitter.Start(_testConsole.Object);
            _testListener.Verify(x => x.ReceiveMessage(message), Times.Exactly(1));
        }

        [Test]
        public void Receiving_Message_With_No_Listeners_Will_Not_Call_ReceiveMessage() {
            const string message = "/question What is 1+1?";
            const string output = "Thank you";

            _testConsole.SetupSequence(x => x.ReadLine())
                .Returns(message)
                .Returns("/exitQuestionBot");

            _testListener.Setup(x => x.ReceiveMessage(message)).Returns(output);
            _testListener.Setup(x => x.ReceiveMessage("/exitQuestionBot")).Returns(output);

            _testEmitter.Start(_testConsole.Object);

            _testListener.Verify(x => x.ReceiveMessage(message), Times.Never);
        }

        [Test]
        public void Adding_Null_Listener_Will_Not_Throw() {
            Assert.DoesNotThrow(() => _testEmitter.Add(null));
        }
    }
}