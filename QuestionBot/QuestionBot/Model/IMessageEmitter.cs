namespace QuestionBot.Model {
    internal interface IMessageEmitter {
        void Add(IMessageListener listener);
        void Start();
    }
}