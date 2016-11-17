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
        private string _exitCommand;


        [SetUp]
        public void Setup() {
            _testListener = new Mock<IMessageListener>( MockBehavior.Strict );
            _testConsole = new Mock<IConsole>();
            _exitCommand = "/exitQuestionBot";
        }

        [Test]
        public void Receiving_Message_Will_Notify_Listener() {
            const string message = "/question What is 1+1?";
            const string output = "Thank you";
            const string nullOutput = null;


            _testConsole.SetupSequence( x => x.ReadLine() )
                .Returns( message )
                .Returns( _exitCommand );

            _testListener.Setup( x => x.ReceiveMessage( message ) ).Returns( output );
            _testListener.Setup( x => x.ReceiveMessage( _exitCommand ) ).Returns( nullOutput );

            _testEmitter = new MessageEmitter( _testConsole.Object );
            _testEmitter.Add( _testListener.Object );
            _testEmitter.Start();

            _testListener.Verify( x => x.ReceiveMessage( message ), Times.Exactly( 1 ) );
            _testListener.Verify( x => x.ReceiveMessage( _exitCommand ), Times.Exactly( 1 ) );
            _testConsole.Verify( x => x.WriteLine( output ), Times.Exactly( 1 ) );
        }

        [Test]
        public void Receiving_Message_With_No_Listeners_Will_Not_Call_ReceiveMessage() {
            const string message = "/question What is 1+1?";
            const string output = "Thank you";

            _testConsole.SetupSequence( x => x.ReadLine() )
                .Returns( message )
                .Returns( _exitCommand );

            _testListener.Setup( x => x.ReceiveMessage( message ) ).Returns( output );

            _testEmitter = new MessageEmitter( _testConsole.Object );
            _testEmitter.Start();

            _testListener.Verify( x => x.ReceiveMessage( message ), Times.Never );
        }

        [Test]
        public void Adding_Null_Listener_Will_Not_Throw() {
            const string message = "/question What is 1+1?";

            _testConsole.SetupSequence( x => x.ReadLine() )
                .Returns( message )
                .Returns( _exitCommand );

            _testEmitter = new MessageEmitter( _testConsole.Object );
            _testEmitter.Add( null );

            Assert.DoesNotThrow( () => _testEmitter.Start() );
        }
    }
}