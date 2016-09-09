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

        public bool TryUpdateRecord(int id, string answer, out IRecord recordToUpdate){
            IEnumerable<IRecord> recordMatchingId = allRecords.Where(recordItem => recordItem.ID == id);

            if (recordMatchingId.Count() == 0){
                recordToUpdate = null;
                return false;
            }

            if (String.IsNullOrEmpty(answer)){
                recordToUpdate = null;
                return false;
            }

            recordToUpdate = recordMatchingId.First();
            recordToUpdate.Answer = answer;
            recordToUpdate.TimeAnswered = DateTime.Now;

            return true;
        }

        public IEnumerable<IRecord> GetRecords(){
            return allRecords;
        }
    }
}
