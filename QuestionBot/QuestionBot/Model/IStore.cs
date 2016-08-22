using System.Collections.Generic;

namespace QuestionBot.Model {

    public interface IStore {
        IRecord CreateRecord(string question);
        IRecord UpdateRecord(int id, string answer);
        IEnumerable<IRecord> GetRecords();
        void DeleteRecord(int id);
    }
}
