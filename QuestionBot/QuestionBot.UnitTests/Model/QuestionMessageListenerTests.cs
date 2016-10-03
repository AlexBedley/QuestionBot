using System;
using NUnit.Framework;
using QuestionBot.Model;
using Moq;

namespace QuestionBot.UnitTests.Model{

    [TestFixture]
    class QuestionMessageListenerTests{
        private IMessageListener _testListener;
        private Mock<IStore> _storeTest;

        [SetUp]
        public void Setup(){
            _storeTest = new Mock<IStore>(MockBehavior.Strict);
            _testListener = new QuestionMessageListener(_storeTest.Object);
        }

        [Test]
        public void New_question_creates_record()
        {
            const string actualQuestion = "What is 1+2?";
            const string question = "/question " + actualQuestion;
            const int id = 15; 

            _storeTest.Setup(x => x.CreateRecord(actualQuestion)).Returns(new Record(id, actualQuestion, DateTime.Now));

            string response = _testListener.ReceiveMessage(question);

            Assert.AreEqual("Question has been created with ID " + id + ". Question: " + actualQuestion, response);
            _storeTest.Verify(x => x.CreateRecord(actualQuestion), Times.Exactly(1));
        }

        [Test]
        [TestCase("/question")]
        [TestCase("/question \n\t     ")]
        public void Blank_question_creates_no_record(string emptyQuestion)
        {
            string response = _testListener.ReceiveMessage(emptyQuestion);

            Assert.AreEqual(QuestionMessageListener.ErrorMessage, response);
        }

        [Test]
        [TestCase (null)]
        [TestCase("this doesn't start with /question")]
        [TestCase("")]
        [TestCase(" /question there is a space before /question")]
        public void Null_or_bad_input_does_nothing(string badInput)
        {
            string response = _testListener.ReceiveMessage(badInput);

            Assert.IsNull(response);
        }


        [Test]
        [TestCase("    What is 1+2?    ", "What is 1+2?")]
        [TestCase("\tWhat is 1+2\t", "What is 1+2?")]
        [TestCase("\nWhat is 1+2\n", "What is 1+2?")]
        public void Starting_and_trailing_whitespace_is_removed_from_question(string actualQuestion, string actualQuestionTrimmed)
        {
            string question = "/question " + actualQuestion;
            const int id = 15;

            _storeTest.Setup(x => x.CreateRecord(actualQuestionTrimmed)).Returns(new Record(id, actualQuestionTrimmed, DateTime.Now));

            string response = _testListener.ReceiveMessage(question);

            Assert.AreEqual("Question has been created with ID " + id + ". Question: " + actualQuestionTrimmed, response);
            _storeTest.Verify(x => x.CreateRecord(actualQuestionTrimmed), Times.Exactly(1));
        }

    }
}