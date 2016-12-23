using System;
using System.Text.RegularExpressions;

namespace QuestionBot.Model {
    public class AnswerMessageListener : IMessageListener {
        private const string AnswerCommand = "/answer";
        public const string ErrorMessage = "This answer or ID appears to be blank, please retry.";
        private IStore _answerDataStore;


        public AnswerMessageListener( IStore store ) {
            _answerDataStore = store;
        }

        public string ReceiveMessage( string message ) {
            IRecord questionRecord;
            int id;

            if ( String.IsNullOrEmpty( message ) || !message.StartsWith( AnswerCommand ) ) {
                return null;
            }

            string idWithAnswerText = message.Remove( 0, AnswerCommand.Length );
            idWithAnswerText = idWithAnswerText.Trim();

            Regex regexPattern = new Regex( @"(\d+)\s.*" );
            Match matches = regexPattern.Match( idWithAnswerText );

            if ( !matches.Success ) {
                return ErrorMessage;
            }

            Int32.TryParse(matches.Groups[1].ToString(), out id);
            string answerText = idWithAnswerText.Remove( 0, matches.Groups[1].ToString().Length ).Trim();

            if ( answerText.Equals( String.Empty ) || id == 0 ) {
                return ErrorMessage;
            }

            bool successfulUpdate = _answerDataStore.TryUpdateRecord( id, answerText, out questionRecord );

            if ( !successfulUpdate ) {
                return ErrorMessage;
            }

            string outputMessage = "Question with ID <" + id + "> has been updated with your answer: " +
                                   answerText;

            return outputMessage;
        }
    }
}