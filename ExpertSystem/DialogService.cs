using ExpertSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExpertSystem
{
    public class DialogService
    {
        TopicsData _topics;
        string _currentTopic;
        Dictionary<string, List<string>> _learnedVariables;

        public DialogService()
        {
            if (File.Exists("topics.json"))
            {
                _topics = JsonConvert.DeserializeObject<TopicsData>(File.ReadAllText("topics.json"));
            }
            _currentTopic = "Universal";
            _learnedVariables = new Dictionary<string, List<string>>();
        }

        public BotAnswer PrepareAnswer(string question)
        {
            var response = FindAnswerInCurrentTopic(question);

            string botResponse = response ?? FindAnswerInUniversalTopic(question);
            return new BotAnswer(botResponse, _currentTopic);
        }

        private string FindAnswerInUniversalTopic(string question)
        {
            var possibleAnswers = new List<(Response, List<string>)>();

            foreach (var topicResponse in _topics.Topics.First(t => t.Name == "Universal").Responses)
            {
                if (PatternMatches(topicResponse.Pattern, question, out List<string> foundVariables))
                {
                    if (RequirementsAreLearned(topicResponse, foundVariables))
                    {
                        possibleAnswers.Add((topicResponse, foundVariables));
                    }
                }
            }

            if (possibleAnswers.Count > 0)
            {
                var mostExactResponse = possibleAnswers.Where(e => e.Item2.Count == 0).FirstOrDefault();
                if (mostExactResponse == default)
                {
                    var variablesCount = possibleAnswers.OrderByDescending(e => e.Item2.Count).First().Item2.Count;
                    mostExactResponse = possibleAnswers.Where(c => c.Item2.Count == variablesCount).OrderByDescending(c => c.Item1.priority).First();
                }
                var topicResponse = mostExactResponse.Item1;
                var foundVariables = mostExactResponse.Item2;

                _currentTopic = topicResponse.Topic ?? _currentTopic;

                LearnFromCurrentQuestion(topicResponse.Learn, foundVariables);
                return ChooseRandomResponse(topicResponse, foundVariables);
            }

            return "no found";
        }

        private string FindAnswerInCurrentTopic(string question)
        {
            var possibleAnswers = new List<(Response, List<string>)>();

            foreach (var topicResponse in _topics.Topics.First(t => t.Name == _currentTopic).Responses)
            {
                if (PatternMatches(topicResponse.Pattern, question, out List<string> foundVariables))
                {
                    if (RequirementsAreLearned(topicResponse, foundVariables))
                    {
                        possibleAnswers.Add((topicResponse, foundVariables));
                    }
                }
            }

            if (possibleAnswers.Count > 0)
            {
                var mostExactResponse = possibleAnswers.Where(e => e.Item2.Count == 0).FirstOrDefault();
                if (mostExactResponse == default)
                {
                    var variablesCount = possibleAnswers.OrderByDescending(e => e.Item2.Count).First().Item2.Count;
                    mostExactResponse = possibleAnswers.Where(c => c.Item2.Count == variablesCount).OrderByDescending(c => c.Item1.priority).First();
                }
                var topicResponse = mostExactResponse.Item1;
                var foundVariables = mostExactResponse.Item2;

                _currentTopic = topicResponse.Topic ?? _currentTopic;

                LearnFromCurrentQuestion(topicResponse.Learn, foundVariables);
                return ChooseRandomResponse(topicResponse, foundVariables);
            }

            return null;
        }

        private string ChooseRandomResponse(Response topicResponse, List<string> foundVariables)
        {
            Random rnd = new Random();
            var response = topicResponse.Templates[rnd.Next(0, topicResponse.Templates.Count)];
            response = ReplaceVariables(response, foundVariables);
            return ReplaceLearnedVariables(response);
        }

        private string ReplaceVariables(string response, List<string> foundVariables)
        {
            foreach (string variable in foundVariables)
            {
                int pos = response.IndexOf('*');
                if (response.IndexOf('*') > 0)
                {
                    response = response.Substring(0, pos) + variable + response.Substring(pos + 1);
                }
            }

            return response;
        }

        private string ReplaceLearnedVariables(string response)
        {
            var foundVariables = new List<string>();
            for (int i = 0; i < response.Length; i++)
            {
                if (response[i] == '<')
                {
                    StringBuilder foundVariable = new StringBuilder();
                    i++;
                    while (response[i] != '>')
                    {
                        foundVariable.Append(response[i]);
                        i++;
                    }
                    foundVariables.Add(foundVariable.ToString());
                }
            }

            Random rnd = new Random();

            foreach (string foundVariable in foundVariables)
            {
                response = response.Replace("<" + foundVariable + ">", _learnedVariables[foundVariable][rnd.Next(0, _learnedVariables[foundVariable].Count)]);
            }

            return response;
        }

        private bool RequirementsAreLearned(Response topicResponse, List<string> foundVariables)
        {
            if (topicResponse.Requires == null)
            {
                return true;
            }

            //for (int i = 0; i < foundVariables.Count; i++)
            //{
            //    if (!_learnedVariables.ContainsKey(topicResponse.Requires[i].Replace("*", foundVariables[i])))
            //    {
            //        return false;
            //    }
            //}
            
            for (int i = 0; i < topicResponse.Requires.Count; i++)
            {
                if(topicResponse.Requires[i].Contains("*"))
                {
                    if (!_learnedVariables.ContainsKey(topicResponse.Requires[i].Replace("*", foundVariables[i])))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!_learnedVariables.ContainsKey(topicResponse.Requires[i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void LearnFromCurrentQuestion(List<string> variablesToLearn, List<string> foundVariables)
        {
            if (variablesToLearn == null)
            {
                return;
            }
            //* replace
            for (int i = 0; i < variablesToLearn.Count; i++)
            {
                if (variablesToLearn[i] != null)
                {
                    variablesToLearn[i] = variablesToLearn[i].Replace("*", foundVariables[i]);
                    if (!_learnedVariables.ContainsKey(variablesToLearn[i]))
                    {
                        _learnedVariables.Add(variablesToLearn[i], new List<string>());
                        _learnedVariables[variablesToLearn[i]].Add(foundVariables[i]);
                    }
                    else
                    {
                        _learnedVariables[variablesToLearn[i]].Add(foundVariables[i]);
                    }
                }
            }
        }

        private bool PatternMatches(string pattern, string question, out List<string> foundVariables)
        {
            question = RemovePunctation(question);
            pattern = RemovePunctation(pattern);

            foundVariables = new List<string>();

            int j = 0;
            try
            {
                for (int i = 0; i < pattern.Length; i++)
                {
                    if (pattern[i] != question[j] && pattern[i] != '*')
                    {
                        return false;
                    }
                    else if (pattern[i] != question[j] && pattern[i] == '*')
                    {
                        StringBuilder variable = new StringBuilder();
                        while (j < question.Length && question[j] != ' ' && Char.IsLetterOrDigit((char)question[j]))
                        {
                            variable.Append(question[j]);
                            j++;
                        }
                        foundVariables.Add(variable.ToString());
                        j--;
                    }
                    j++;
                }
                if (j < question.Length)
                {
                    //There is more text after starred word. ex: pattern: I like *, question: I like a and b. Found variable 'a', but it would not check rest of question.
                    return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        private string RemovePunctation(string question)
        {
            return new string(question.Where(c => !char.IsPunctuation(c) || c=='*').ToArray());
        }
    }
}
