using System.Collections.Generic;

namespace QuestionBot.Model {

    public interface IStore {
        IRecord CreateRecord(string question);
        bool TryUpdateRecord(int id, string answer);
        IEnumerable<IRecord> GetRecords();
    }
}
