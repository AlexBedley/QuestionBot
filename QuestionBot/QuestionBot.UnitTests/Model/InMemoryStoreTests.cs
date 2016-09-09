using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using QuestionBot.Model;

namespace QuestionBot.UnitTests.Model {

    [TestFixture]
    public class InMemoryStoreTests{
        private IStore _storeTest;

        [SetUp]
        public void Setup(){
            _storeTest = new InMemoryStore();
        }

        [Test]
        public void New_question_creates_record(){
            string question = "What is 1+2?";

            IRecord questionRecord = _storeTest.CreateRecord(question);

            Assert.AreEqual(questionRecord.Question, question);
            Assert.AreNotEqual(questionRecord.TimeAsked, default(DateTime));
            Assert.AreNotEqual(questionRecord.ID,default(int));
        }

        [Test]
        public void Updating_question_with_answer_stores_my_answer(){
            string question = "What is 1+2?";
            string answer = "3";
            IRecord answerRecord;
            IEnumerable<IRecord> retrievedRecords;

            IRecord questionRecord = _storeTest.CreateRecord(question);
            bool updatedRecord = _storeTest.TryUpdateRecord(questionRecord.ID, answer,out answerRecord);
            retrievedRecords = _storeTest.GetRecords();
            IRecord myRecord = retrievedRecords.ElementAt(0);

            Assert.IsTrue(updatedRecord);
            Assert.AreEqual(myRecord.Question, question);
            Assert.AreEqual(myRecord.Answer, answer);
            Assert.AreEqual(myRecord.TimeAsked, questionRecord.TimeAsked);
            Assert.AreEqual(myRecord.TimeAnswered, questionRecord.TimeAnswered);
            Assert.AreEqual(myRecord.ID, questionRecord.ID);

            Assert.AreEqual(answerRecord.Question, question);
            Assert.AreEqual(answerRecord.Answer, answer);
            Assert.AreEqual(answerRecord.TimeAsked, questionRecord.TimeAsked);
            Assert.AreEqual(answerRecord.TimeAnswered, questionRecord.TimeAnswered);
            Assert.AreEqual(answerRecord.ID, questionRecord.ID);
        }

        [Test]
        public void GetRecords_returns_all_question_records(){
            string question1 = "What is 1+2";
            string question2 = "What is 2+3?";
            string answer1 = "3";
            string answer2 = "five";
            IRecord answerRecord;
            IEnumerable<IRecord> retrievedRecords;

            IRecord record1 = _storeTest.CreateRecord(question1);
            IRecord record2 = _storeTest.CreateRecord(question2);
            IEnumerable<IRecord> questionRecords = new List<IRecord>{record1,record2};

            _storeTest.TryUpdateRecord(record1.ID, answer1, out answerRecord);
            _storeTest.TryUpdateRecord(record2.ID, answer2, out answerRecord);

            retrievedRecords = _storeTest.GetRecords();

            CollectionAssert.AreEqual(questionRecords, retrievedRecords);
        }

        [Test]
        public void GetRecords_returns_empty_list_if_there_are_no_records(){
            IEnumerable<IRecord> retrievedRecords;
            retrievedRecords = _storeTest.GetRecords();

            Assert.IsEmpty(retrievedRecords);
        }

        [Test]
        public void Updating_record_with_blank_answer_returns_false(){
            string question1 = "What is 1+2";
            string question2 = "What is 2+3?";
            string answer1 = "";
            string answer2 = null;
            IRecord x;

            IRecord record1 = _storeTest.CreateRecord(question1);
            IRecord record2 = _storeTest.CreateRecord(question2);

            bool emptyAnswerResult = _storeTest.TryUpdateRecord(record1.ID, answer1, out x);
            bool nullAnswerResult = _storeTest.TryUpdateRecord(record2.ID, answer2, out x);

            Assert.IsFalse(emptyAnswerResult);
            Assert.IsFalse(nullAnswerResult);
        }

        [Test]
        public void Updating_record_with_nonexistant_ID_returns_false(){
            string question1 = "What is 1+2";
            string answer = "3";
            IRecord unusedRecord;

            IRecord myRecord = _storeTest.CreateRecord(question1);

            bool badIdResult = _storeTest.TryUpdateRecord(myRecord.ID + 1, answer,out unusedRecord);

            Assert.IsFalse(badIdResult);
        }

    }
}
