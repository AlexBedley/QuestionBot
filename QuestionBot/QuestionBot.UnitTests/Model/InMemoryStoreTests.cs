using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using QuestionBot.Model;

namespace QuestionBot.UnitTests.Model {

    [TestFixture]
    public class InMemoryStoreTests{
        private IStore _storeTest;
        private IEnumerable<IRecord> _retrievedRecords = new List<IRecord>();

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

            _retrievedRecords = _storeTest.GetRecords();
            IRecord firstRecord = _retrievedRecords.ElementAt(0);
            IRecord secondRecord = _retrievedRecords.ElementAt(1);

            Assert.AreEqual(firstRecord.ID, record1.ID);
            Assert.AreEqual(firstRecord.Question, record1.Question);
            Assert.AreEqual(firstRecord.Answer, record1.Answer);
            Assert.AreEqual(firstRecord.TimeAsked, record1.TimeAsked);
            Assert.AreEqual(firstRecord.TimeAnswered, record1.TimeAnswered);

            Assert.AreEqual(secondRecord.ID, record2.ID);
            Assert.AreEqual(secondRecord.Question, record2.Question);
            Assert.AreEqual(secondRecord.Answer, record2.Answer);
            Assert.AreEqual(secondRecord.TimeAsked, record2.TimeAsked);
            Assert.AreEqual(secondRecord.TimeAnswered, record2.TimeAnswered);
        }

        [Test]
        public void GetRecords_returns_empty_list_if_there_are_no_records(){
            _retrievedRecords = _storeTest.GetRecords();

            Assert.That(_retrievedRecords, Is.EqualTo(new List<IRecord>()));
        }
    }
}
