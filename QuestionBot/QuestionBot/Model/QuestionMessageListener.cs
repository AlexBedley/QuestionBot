using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionBot.Model{

    public class QuestionMessageListener : IMessageListener{

        public const string ErrorMessage = "This question appears to be blank, please retry.";

        private IStore _questionDataStore;
        private const string QuestionCommand = "/question";

        public QuestionMessageListener(IStore store){
            _questionDataStore = store;
        }

        public string ReceiveMessage(string message){
            string outputMessage;

            if (String.IsNullOrEmpty(message) || !message.StartsWith(QuestionCommand)){
                return null;
            }

            string questionText = message.Remove(0, QuestionCommand.Length);
            questionText = questionText.Trim();

            if (questionText.Equals(String.Empty)){
                return ErrorMessage;
            }

            IRecord newQuestion = _questionDataStore.CreateRecord(questionText);
            outputMessage = "Question has been created with ID " +
                            newQuestion.Id +
                            ". Question: " +
                            newQuestion.Question;
            return outputMessage;
        }
    }
}