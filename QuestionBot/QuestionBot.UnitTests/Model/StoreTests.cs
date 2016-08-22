using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using QuestionBot.Model;

namespace QuestionBot.UnitTests.Model {

    [TestFixture]
    public class StoreTests
    {
        private IStore _storeTest;
        private IEnumerable<IRecord> _retreivedRecords = new List<IRecord>();

    [SetUp]
    public void Setup(){
        _storeTest = new InMemoryStore();
    }
        [Test]
        public void New_question_creates_record()
        {
            string question = "What is 1+2?";
            IRecord questionRecord = _storeTest.CreateRecord(question);
            Assert.AreEqual(questionRecord.Question, question);
            Assert.IsNotNull(questionRecord.TimeAsked);
            Assert.IsNotNull(questionRecord.ID);
        }
        [Test]
        public void Updating_question_with_answer_updates_the_record()
        {
            string question = "What is 2+3?";
            string answer = "5";

            IRecord answeredRecord = _storeTest.CreateRecord(question);
            _storeTest.UpdateRecord(answeredRecord.ID, answer);
            Assert.AreEqual(answeredRecord.Question, question);
            Assert.AreEqual(answeredRecord.Answer, answer);
            Assert.IsNotNull(answeredRecord.ID);
            Assert.IsNotNull(answeredRecord.TimeAsked);
            Assert.IsNotNull(answeredRecord.TimeAnswered);
        }
        [Test]
        public void GetRecords_returns_all_question_records()
        {
            string question1 = "What is 1+2";
            string question2 = "What is 2+3?";
            _storeTest.CreateRecord(question1);
            _storeTest.CreateRecord(question2);
            _retreivedRecords = _storeTest.GetRecords();
            Assert.AreEqual(_retreivedRecords.ElementAt(0).Question, question1);
            Assert.AreEqual(_retreivedRecords.ElementAt(1).Question, question2);
        }
        [Test]
        public void GetRecords_returns_empty_list_if_there_are_no_records()
        {
            _retreivedRecords = _storeTest.GetRecords();
            Assert.That(_retreivedRecords, Is.EqualTo(new List<IRecord>()));
        }
    }
}
