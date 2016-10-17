using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestionBot.Model {
    public class MessageEmitter : IMessageEmitter {
        private string _lineInput = "";
        public static IList<IMessageListener> Listeners = new List<IMessageListener>();

        public void Start() {
            while( _lineInput != "/exitQuestionbot" ) {
                _lineInput = Console.ReadLine();
                NotifyAllListeners( _lineInput );
            }
        }

        public void Add( IMessageListener listener ) {
            Listeners.Add( listener );
        }

        public static void NotifyAllListeners( string newInput ) {
            for( int i = 0; i < Listeners.Count(); i++ ) {
                Listeners[i].ReceiveMessage( newInput );
            }
        }
    }
}