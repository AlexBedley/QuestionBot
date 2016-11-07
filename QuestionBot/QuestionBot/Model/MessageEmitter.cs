using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestionBot.Model {
    public class MessageEmitter : IMessageEmitter {
        private static readonly IList<IMessageListener> _listeners = new List<IMessageListener>();

        public void Start(IConsole messageConsole) {
            string lineInput = messageConsole.ReadLine();
            NotifyAllListeners(lineInput, messageConsole);
        }

        public void Add(IMessageListener listener) {
            if (listener != null) {
                _listeners.Add(listener);
            }
        }

        private static void NotifyAllListeners(string newInput, IConsole outputConsole) {
            string response;

            foreach (var listener in _listeners) {
                response = listener.ReceiveMessage(newInput);
                outputConsole.WriteLine(response);
            }
        }
    }
}