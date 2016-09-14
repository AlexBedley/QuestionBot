using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionBot.Model{

    public class QuestionMessageListener : IMessageListener{

        private IStore _questionDataStore;

        public QuestionMessageListener(IStore store){
            _questionDataStore = store;
        }

        public void ReceiveMessage(string input){
            const string questionCommand = "/question";
            const string answerTag = "/answer";

            if (input.StartsWith(questionCommand)){
                string questionText = input.Remove(0, questionCommand.Length);
                questionText = questionText.TrimStart(' ');

                if (!String.IsNullOrWhiteSpace(questionText)){
                    IRecord newQuestion = _questionDataStore.CreateRecord(questionText);
                    Console.WriteLine("Question has been created with ID " + 
                        newQuestion.Id + 
                        ". Question: " +
                        newQuestion.Question);
                }else{
                    Console.WriteLine("This question appears to be blank, please retry.");
                }

            }else if (input.StartsWith(answerTag)){
                IRecord updatedRecord;
                KeyValuePair<int,string> idWithAnswer = GetIdWithAnswer(input, answerTag);
                
                if (_questionDataStore.TryUpdateRecord(idWithAnswer.Key, idWithAnswer.Value, out updatedRecord)){
                    Console.WriteLine("Question ID " +
                        updatedRecord.Id +
                        " has been updated with answer: " +
                        updatedRecord.Answer);
                }else{
                    Console.WriteLine("Question ID " +
                        idWithAnswer.Key +
                        " either does not exist, or the answer is empty. Please try again.");
                }
            }
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