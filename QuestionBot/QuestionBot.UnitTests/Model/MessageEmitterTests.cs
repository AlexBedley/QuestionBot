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
        private const string ExitCommand = "/exitQuestionBot";
        private const string Message = "/question What is 1+1?";
        private const string Output = "Thank you";

        [SetUp]
        public void Setup() {
            _testListener = new Mock<IMessageListener>( MockBehavior.Strict );
            _testConsole = new Mock<IConsole>();
        }

        [Test]
        public void Receiving_Message_Will_Notify_Listener() {
            const string nullOutput = null;

            _testConsole.SetupSequence( x => x.ReadLine() )
                .Returns( Message )
                .Returns( ExitCommand );

            _testListener.Setup( x => x.ReceiveMessage( Message ) ).Returns( Output );
            _testListener.Setup( x => x.ReceiveMessage( ExitCommand ) ).Returns( nullOutput );

            _testEmitter = new MessageEmitter( _testConsole.Object );
            _testEmitter.Add( _testListener.Object );
            _testEmitter.Start();

            _testListener.Verify( x => x.ReceiveMessage( Message ), Times.Exactly( 1 ) );
            _testListener.Verify( x => x.ReceiveMessage( ExitCommand ), Times.Exactly( 1 ) );
            _testConsole.Verify( x => x.WriteLine( Output ), Times.Exactly( 1 ) );
            _testConsole.Verify( x => x.WriteLine( nullOutput ), Times.Exactly( 1 ) );
        }

        [Test]
        public void Receiving_Message_With_No_Listeners_Will_Not_Call_ReceiveMessage() {
            _testConsole.SetupSequence( x => x.ReadLine() )
                .Returns( Message )
                .Returns( ExitCommand );

            _testListener.Setup( x => x.ReceiveMessage( Message ) ).Returns( Output );

            _testEmitter = new MessageEmitter( _testConsole.Object );
            _testEmitter.Start();

            _testListener.Verify( x => x.ReceiveMessage( Message ), Times.Never );
        }

        [Test]
        public void Adding_Null_Listener_Will_Not_Throw() {
            _testConsole.SetupSequence( x => x.ReadLine() )
                .Returns( Message )
                .Returns( ExitCommand );

            _testEmitter = new MessageEmitter( _testConsole.Object );
            _testEmitter.Add( null );

            Assert.DoesNotThrow( () => _testEmitter.Start() );
        }
    }
}