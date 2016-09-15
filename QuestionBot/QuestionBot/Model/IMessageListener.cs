using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionBot.Model{

    public interface IMessageListener{
        string ReceiveMessage(string message);
    }
}
