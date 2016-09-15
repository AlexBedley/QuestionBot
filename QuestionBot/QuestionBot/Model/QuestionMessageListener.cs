using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionBot.Model{

    public class QuestionMessageListener : IMessageListener{

        private IStore _questionDataStore;
        private const string QuestionCommand = "/question";
        private const string AnswerTag = "/answer";

        public QuestionMessageListener(IStore store){
            _questionDataStore = store;
        }

        public string ReceiveMessage(string message){

            string outputMessage;

            if (message.StartsWith(QuestionCommand)){
                string questionText = message.Remove(0, QuestionCommand.Length);
                questionText = questionText.TrimStart(' ');

                if (!String.IsNullOrWhiteSpace(questionText)){
                    IRecord newQuestion = _questionDataStore.CreateRecord(questionText);
                    outputMessage ="Question has been created with ID " + 
                        newQuestion.Id + 
                        ". Question: " +
                        newQuestion.Question;
                    return outputMessage;
                }

                outputMessage = "This question appears to be blank, please retry.";
                return outputMessage;
            }else if (message.StartsWith(AnswerTag)){
                IRecord updatedRecord;
                KeyValuePair<int,string> idWithAnswer = GetIdWithAnswer(message, AnswerTag);
                
                if (_questionDataStore.TryUpdateRecord(idWithAnswer.Key, idWithAnswer.Value, out updatedRecord)){
                    outputMessage = "Question ID " +
                        updatedRecord.Id +
                        " has been updated with answer: " +
                        updatedRecord.Answer;
                    return outputMessage;
                }

                outputMessage = "Question ID " +
                    idWithAnswer.Key +
                    " either does not exist, or the answer is empty. Please try again.";
                return outputMessage;
            }
            return null;
        }

        private static KeyValuePair<int,string> GetIdWithAnswer(string fullAnswer, string ansTag){
            int firstIdTag;
            int secondIdTag;
            char[] openSquareBrace = { '[' };
            char[] closeSquareBrace = { ']' };

            string answerWithId = fullAnswer.Remove(0, ansTag.Length + 1);

            firstIdTag = answerWithId.IndexOfAny(openSquareBrace, 0);
            secondIdTag = answerWithId.IndexOfAny(closeSquareBrace, 0);

            int questionId = Convert.ToInt32(answerWithId.Substring(firstIdTag + 1, secondIdTag - 1));
            string answerText = answerWithId.Remove(0, secondIdTag + 1);
            answerText = answerText.TrimStart(' ');

            KeyValuePair<int, string> questionIdWithAnswer = new KeyValuePair<int,string>(questionId,answerText);
            return questionIdWithAnswer;
        }
    }
}