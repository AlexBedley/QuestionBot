using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionBot.Model{

    public class QuestionMessageListener : IMessageListener{

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
            questionText = questionText.TrimStart(' ','\u0009');
            questionText = questionText.TrimEnd(' ', '\u0009');

            if (questionText.Equals(String.Empty)){
                outputMessage = "This question appears to be blank, please retry.";
                return outputMessage;
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