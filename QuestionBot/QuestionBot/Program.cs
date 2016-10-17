using System;

namespace QuestionBot.Model {
    internal class Program {
        private static void Main( string[] args ) {
            IStore localStore = new InMemoryStore();
            IMessageListener questionListener = new QuestionMessageListener( localStore );

            IMessageEmitter emitter = new MessageEmitter();
            emitter.Add( questionListener );
            emitter.Start();
        }
    }
}