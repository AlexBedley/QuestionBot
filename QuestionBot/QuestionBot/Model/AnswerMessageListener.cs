using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionBot.Model {
    class AnswerMessageListener : IMessageListener {
        private const string AnswerCommand = "/answer";
        public const string ErrorMessage = "This answer or ID appears to be blank, please retry.";
        private IStore _answerDataStore;


        public AnswerMessageListener( IStore store ) {
            _answerDataStore = store;
        }

        public string ReceiveMessage( string message ) {
            string words = "a";
            IRecord questionRecord;
            int id;

            if ( String.IsNullOrEmpty( message ) || !message.StartsWith( AnswerCommand ) ) {
                return null;
            }

            string idWithAnswerText = message.Remove( 0, AnswerCommand.Length );
            string[] splitIdWithAnswer = idWithAnswerText.Split( null );

            if ( splitIdWithAnswer.Count() < 2 ) {
                return ErrorMessage;
            }

            Int32.TryParse( splitIdWithAnswer[0], out id );
            string answer = splitIdWithAnswer[1];

            _answerDataStore.TryUpdateRecord( id, answer, out questionRecord );
            outputMessage = "Question with ID " + id + " has been updated with your answer: " +
                            answer;
            return "";
        }
    }
}