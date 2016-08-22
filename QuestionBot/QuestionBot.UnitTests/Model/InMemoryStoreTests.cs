using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using QuestionBot.Model;

namespace QuestionBot.UnitTests.Model {

    [TestFixture]
    public class InMemoryStoreTests{
        private IStore _storeTest;
        private IEnumerable<IRecord> _retreivedRecords = new List<IRecord>();

        [SetUp]
        public void Setup(){
            _storeTest = new InMemoryStore();
        }

        [Test]
        public void New_question_creates_record(){
            string question = "What is 1+2?";

            IRecord questionRecord = _storeTest.CreateRecord(question);

            Assert.AreEqual(questionRecord.Question, question);
            Assert.AreNotEqual(questionRecord.TimeAsked, DateTime.MinValue);
            Assert.AreNotEqual(questionRecord.ID,0);
        }

        [Test]
        public void Updating_question_with_answer_updates_the_record(){
            string question = "What is 2+3?";
            string answer = "5";

            IRecord questionRecord = _storeTest.CreateRecord(question);
            IRecord answeredRecord = _storeTest.UpdateRecord(questionRecord.ID, answer);

            Assert.AreEqual(answeredRecord.Question, questionRecord.Question);
            Assert.AreEqual(answeredRecord.Answer, questionRecord.Answer);
            Assert.AreEqual(answeredRecord.ID, questionRecord.ID);
            Assert.AreEqual(answeredRecord.TimeAsked, questionRecord.TimeAsked);
            Assert.AreEqual(answeredRecord.TimeAnswered, questionRecord.TimeAnswered);
        }

        [Test]
        public void GetRecords_returns_all_question_records(){
            string question1 = "What is 1+2";
            string question2 = "What is 2+3?";
            string answer1 = "3";
            string answer2 = "five";

            IRecord record1 = _storeTest.CreateRecord(question1);
            IRecord record2 = _storeTest.CreateRecord(question2);

            _storeTest.UpdateRecord(record1.ID, answer1);
            _storeTest.UpdateRecord(record2.ID, answer2);

            _retreivedRecords = _storeTest.GetRecords();

            Assert.AreEqual(_retreivedRecords.ElementAt(0).ID, record1.ID);
            Assert.AreEqual(_retreivedRecords.ElementAt(0).Question, record1.Question);
            Assert.AreEqual(_retreivedRecords.ElementAt(0).Answer, record1.Answer);
            Assert.AreEqual(_retreivedRecords.ElementAt(0).TimeAsked, record1.TimeAsked);
            Assert.AreEqual(_retreivedRecords.ElementAt(0).TimeAnswered, record1.TimeAnswered);

            Assert.AreEqual(_retreivedRecords.ElementAt(1).ID, record2.ID);
            Assert.AreEqual(_retreivedRecords.ElementAt(1).Question, record2.Question);
            Assert.AreEqual(_retreivedRecords.ElementAt(1).Answer, record2.Answer);
            Assert.AreEqual(_retreivedRecords.ElementAt(1).TimeAsked, record2.TimeAsked);
            Assert.AreEqual(_retreivedRecords.ElementAt(1).TimeAnswered, record2.TimeAnswered);
        }

        [Test]
        public void GetRecords_returns_empty_list_if_there_are_no_records(){
            _retreivedRecords = _storeTest.GetRecords();

            Assert.That(_retreivedRecords, Is.EqualTo(new List<IRecord>()));
        }
    }
}
