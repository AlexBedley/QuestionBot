using System;
using NUnit.Framework;
using QuestionBot.Model;
using Moq;


namespace QuestionBot.UnitTests.Model {
    [TestFixture]
    class AnswerMessageListenerTests {
        private IMessageListener _testListener;
        private Mock< IStore > _storeTest;

        [SetUp]
        public void Setup() {
            _storeTest = new Mock< IStore >( MockBehavior.Strict );
            _testListener = new AnswerMessageListener( _storeTest.Object );
        }

        [Test]
        [TestCase( "The answer is 3." )]
        [TestCase( "I forget the /answer" )]
        public void Valid_id_and_answer_updates_record( string answerText ) {
            const int id = 15;
            string fullAnswer = "/answer " + id + " " + answerText;

            IRecord record = new Record( id, "What is 1+2?", DateTime.Now );

            _storeTest.Setup( x => x.TryUpdateRecord( id, answerText, out record ) )
                .Returns( true );

            string response = _testListener.ReceiveMessage( fullAnswer );

            Assert.AreEqual( "Question with ID <" + id + "> has been updated with your answer: " +
                             answerText, response );

            _storeTest.Verify( x => x.TryUpdateRecord( id, answerText, out record ), Times.Exactly( 1 ) );
        }

        [Test]
        [TestCase( "/answer 1" )]
        [TestCase( "/answer 1 \n\t     " )]
        public void Blank_answer_does_not_update_record_returns_error( string emptyAnswer ) {
            string response = _testListener.ReceiveMessage( emptyAnswer );

            Assert.AreEqual( AnswerMessageListener.ErrorMessage, response );
        }

        [Test]
        [TestCase( null )]
        [TestCase( "this doesn't start with /answer" )]
        [TestCase( "" )]
        [TestCase( " /answer there is a space before /answer" )]
        public void Null_or_bad_input_does_nothing( string badInput ) {
            string response = _testListener.ReceiveMessage( badInput );

            Assert.IsNull( response );
        }


        [Test]
        [TestCase( "    The answer is 3.    ", "The answer is 3." )]
        [TestCase( "\tThe answer is 3.\t", "The answer is 3." )]
        [TestCase( "\nThe answer is 3.\n", "The answer is 3." )]
        public void Starting_and_trailing_whitespace_is_removed_from_answer( string actualAnswer,
            string actualAnswerTrimmed ) {
            const int id = 15;
            string fullAnswer = "/answer " + id + " " + actualAnswer;
            IRecord record = new Record( id, "What is 1+2?", DateTime.Now );

            _storeTest.Setup( x => x.TryUpdateRecord( id, actualAnswerTrimmed, out record ) ).Returns( true );

            string response = _testListener.ReceiveMessage( fullAnswer );

            Assert.AreEqual( "Question with ID <" + id + "> has been updated with your answer: " + actualAnswerTrimmed,
                response );
            _storeTest.Verify( x => x.TryUpdateRecord( id, actualAnswerTrimmed, out record ), Times.Exactly( 1 ) );
        }

        [Test]
        [TestCase( "/answer The answer is 3." )]
        [TestCase( "/answer 1The answer is 3." )]
        public void Update_question_with_no__or_bad_answer_id_returns_error( string fullAnswer ) {
            string response = _testListener.ReceiveMessage( fullAnswer );

            Assert.AreEqual( AnswerMessageListener.ErrorMessage, response );
        }
    }
}