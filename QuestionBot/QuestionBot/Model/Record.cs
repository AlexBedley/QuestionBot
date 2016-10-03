using System;

namespace QuestionBot.Model {

    public class Record : IRecord {
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Id { get; set; }
        public DateTime TimeAsked { get; set; }
        public DateTime TimeAnswered { get; set; }

        public Record(int id, string qstn, DateTime qstnTime){
            Id = id;
            Question = qstn;
            TimeAsked = qstnTime;
        }
    }
}
