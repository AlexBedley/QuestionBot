using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionBot.Model {
    public interface IConsole {
        string ReadLine();
        void WriteLine(string message);
    }
}