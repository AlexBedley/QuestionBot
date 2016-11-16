using System;

namespace QuestionBot.Model {
    internal class Program {
        private static void Main( string[] args ) {
            IStore localStore = new InMemoryStore();
            IMessageListener questionListener = new QuestionMessageListener(localStore);
            IConsole consoleWrapper = new ConsoleWrapper();
            IMessageEmitter emitter = new MessageEmitter(consoleWrapper);

            emitter.Add(questionListener);
            emitter.Start();
        }
    }
}