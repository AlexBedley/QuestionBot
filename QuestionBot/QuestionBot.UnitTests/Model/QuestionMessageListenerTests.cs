using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using QuestionBot.Model;

namespace QuestionBot.UnitTests.Model{

    [TestFixture]
    class QuestionMessageListenerTests{
        private IMessageListener _testListener;
        private IStore _storeTest;

        [SetUp]
        public void Setup(){
            _storeTest = new InMemoryStore();
            _testListener = new QuestionMessageListener(_storeTest);
        }

        [Test]
        public void New_question_creates_record(){
            string question = "/question What is 1+2?";
            IEnumerable<IRecord> recordList;
            IRecord newRecord;

            _testListener.ReceiveMessage(question);
            recordList = _storeTest.GetRecords();
            newRecord = recordList.ElementAt(0);

            Assert.AreEqual(newRecord.Question,question.Substring(10));
            Assert.AreNotEqual(newRecord.Id,default(int));
            Assert.AreNotEqual(newRecord.TimeAsked,default(DateTime));
            Assert.AreEqual(newRecord.Answer,default(string));
            Assert.AreEqual(newRecord.TimeAnswered,default(DateTime));
        }

        [Test]
        public void Blank_question_creates_no_record(){
            string question = "/question ";
            IEnumerable<IRecord> recordList;

            _testListener.ReceiveMessage(question);
            recordList = _storeTest.GetRecords();
            
            Assert.IsEmpty(recordList);
        }

        [Test]
        public void No_question_or_answer_does_nothing(){
            string statement = "Testing a string";
            IEnumerable<IRecord> recordList;

            _testListener.ReceiveMessage(statement);
            recordList = _storeTest.GetRecords();

            Assert.IsEmpty(recordList);
        }

        [Test]
        public void Updating_question_with_wrong_id_does_not_update_record()
        {
            string question = "/question What is 1+2?";
            IEnumerable<IRecord> recordList;
            IRecord newRecord;

            _testListener.ReceiveMessage(question);
            recordList = _storeTest.GetRecords();
            newRecord = recordList.ElementAt(0);
            string answer = "/answer [" + (newRecord.Id+1) + "] three";
            _testListener.ReceiveMessage(answer);

            recordList = _storeTest.GetRecords();
            newRecord = recordList.ElementAt(0);

            Assert.AreEqual(newRecord.Question, question.Substring(10));
            Assert.AreNotEqual(newRecord.Id, default(int));
            Assert.AreNotEqual(newRecord.TimeAsked, default(DateTime));
            Assert.AreEqual(newRecord.Answer, default(string));
            Assert.AreEqual(newRecord.TimeAnswered, default(DateTime));
        }

        [Test]
        public void Multi_question_answering_updates_records()
        {
            string question1 = "/question What is 1+2?";
            string question2 = "/question What is 2+3?";
            string strippedAnswer1 = "three";
            string strippedAnswer2 = "five";
            IEnumerable<IRecord> questionRecords;
            IEnumerable<IRecord> questionAnswerRecords;
            IRecord newRecord1;
            IRecord newRecord2;
            IRecord updatedRecord1;
            IRecord updatedRecord2;

            _testListener.ReceiveMessage(question1);
            _testListener.ReceiveMessage(question2);
            questionRecords = _storeTest.GetRecords();

            newRecord1 = questionRecords.ElementAt(0);
            newRecord2 = questionRecords.ElementAt(1);

            string answer1 = "/answer [" + newRecord1.Id + "]" + strippedAnswer1;
            string answer2 = "/answer [" + newRecord2.Id + "]" + strippedAnswer2;

            _testListener.ReceiveMessage(answer1);
            _testListener.ReceiveMessage(answer2);
            questionAnswerRecords = _storeTest.GetRecords();

            updatedRecord1 = questionAnswerRecords.ElementAt(0);
            updatedRecord2 = questionAnswerRecords.ElementAt(1);

            Assert.AreEqual(updatedRecord1.Question, question1.Substring(10)); //question text only, no tag
            Assert.AreEqual(updatedRecord2.Question, question2.Substring(10));
            Assert.AreEqual(newRecord1.Id, updatedRecord1.Id);
            Assert.AreEqual(newRecord2.Id, updatedRecord2.Id);
            Assert.AreNotEqual(updatedRecord1.TimeAsked, default(DateTime));
            Assert.AreNotEqual(updatedRecord2.TimeAsked, default(DateTime));
            Assert.AreEqual(updatedRecord1.Answer, strippedAnswer1);
            Assert.AreEqual(updatedRecord2.Answer, strippedAnswer2);
            Assert.Greater(updatedRecord1.TimeAnswered, updatedRecord1.TimeAsked);
            Assert.Greater(updatedRecord2.TimeAnswered, updatedRecord2.TimeAsked);
        }
    }
}
