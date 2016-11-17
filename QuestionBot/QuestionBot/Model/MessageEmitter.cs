using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestionBot.Model {
    public class MessageEmitter : IMessageEmitter {
        private IConsole _messageConsole;
        private readonly IList<IMessageListener> _listeners;

        public MessageEmitter( IConsole messageConsole) {
            _messageConsole = messageConsole;
            _listeners = new List<IMessageListener>();
        }
        public void Start() {
            string lineInput = "";

            while (lineInput != "/exitQuestionBot") {
                lineInput = _messageConsole.ReadLine();

                if ( lineInput != "/exitQuestionBot" ) {
                    NotifyAllListeners( lineInput );
                }
            }
        }

        public void Add(IMessageListener listener) {       
            if(listener != null) {
                _listeners.Add(listener);
            }
        }

        private void NotifyAllListeners(string newInput) {
            string response;

            foreach (var listener in _listeners) {
                response = listener.ReceiveMessage(newInput);
                _messageConsole.WriteLine(response);
            }
        }
    }
}