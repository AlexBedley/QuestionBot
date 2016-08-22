using System;

namespace QuestionBot.Model {

    public interface IRecord {
        string Question { get; set; }
        int ID { get; set; }
        DateTime TimeAsked { get; set; }
        string Answer { get; set; }
        DateTime TimeAnswered { get; set; }
    }
}
