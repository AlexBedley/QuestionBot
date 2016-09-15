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
            string question = "/question";
            IEnumerable<IRecord> recordList;

            _testListener.ReceiveMessage(question);
            recordList = _storeTest.GetRecords();
            
            Assert.IsEmpty(recordList);
        }

        [Test]
        public void No_question_does_nothing(){
            string statement = "Testing a string";
            IEnumerable<IRecord> recordList;

            string output = _testListener.ReceiveMessage(statement);

            Assert.IsNull(output);
        }


    }
}
