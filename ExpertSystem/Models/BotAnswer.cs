namespace ExpertSystem.Models
{
    public class BotAnswer
    {
        public BotAnswer(string response, string currentTopic)
        {
            Response = response;
            CurrentTopic = currentTopic;
        }

        public string Response;
        public string CurrentTopic;
    }
}
