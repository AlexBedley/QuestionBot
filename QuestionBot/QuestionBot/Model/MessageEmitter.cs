using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestionBot.Model {
    public class MessageEmitter : IMessageEmitter {
        private string _lineInput = "";
        private static IList<IMessageListener> _listeners = new List<IMessageListener>();

        public void Start() {
            while( _lineInput != "/exitQuestionbot" ) {
                _lineInput = Console.ReadLine();
                NotifyAllListeners( _lineInput );
            }
        }

        public void Add( IMessageListener listener ) {
            _listeners.Add( listener );
        }

        public static void NotifyAllListeners( string newInput ) {
            foreach (var listener in _listeners) {
                listener.ReceiveMessage( newInput );
            }
        }
    }
}