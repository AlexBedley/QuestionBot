using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestionBot.Model {

    public class InMemoryStore : IStore {
        private IList<IRecord> allRecords = new List<IRecord>();
        private static int _questionId = 0;

        public IRecord CreateRecord(string question){
            _questionId++;

            Record newQuestion = new Record(_questionId, question, DateTime.Now);
            allRecords.Add(newQuestion);

            return newQuestion;
        }

        public IRecord UpdateRecord(int id, string answer){
            IRecord recordToUpdate = allRecords.FirstOrDefault(recordItem => recordItem.ID == id);

            recordToUpdate.Answer = answer;
            recordToUpdate.TimeAnswered = DateTime.Now;

            return recordToUpdate;
        }

        public IEnumerable<IRecord> GetRecords(){
            return allRecords;
        }
    }
}
