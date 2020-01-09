using System.Collections.Generic;

namespace ExpertSystem
{
    public class Response
    {
        public Response()
        {

        }

        public string Pattern;
        public string Topic;
        public List<string> Templates;
        public List<string> Learn;
        public List<string> Requires;
        public int priority;
    }
}