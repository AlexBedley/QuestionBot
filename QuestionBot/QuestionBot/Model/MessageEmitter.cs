using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestionBot.Model {
    public class MessageEmitter : IMessageEmitter {
        private IConsole _messageConsole;
        private readonly IList<IMessageListener> _listeners;

        public MessageEmitter( IConsole messageConsole ) {
            _messageConsole = messageConsole;
            _listeners = new List<IMessageListener>();
        }

        public void Start() {
            const string exitCommand = "/exitQuestionBot";
            string lineInput = "";

            while( lineInput != exitCommand ) {
                lineInput = _messageConsole.ReadLine();
                NotifyAllListeners( lineInput );
            }
        }

        public void Add( IMessageListener listener ) {
          //  if( listener != null ) {
                _listeners.Add( listener );
         //   }
        }

        private void NotifyAllListeners( string newInput ) {
            foreach( var listener in _listeners ) {
                string response = listener.ReceiveMessage( newInput );
                _messageConsole.WriteLine( response );
            }
        }
    }
}