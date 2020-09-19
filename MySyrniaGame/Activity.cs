using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySyrniaGame
{
    public class Activity
    {
        public enum ActivityType { chop, fish, fight, mine, clean};

        public string TriggerPhrase;
        public ActivityType Type;
        public Result Result;
        public string Message;

        public Activity(string triggerPhrase, ActivityType type, string message, Result result)
        {
            TriggerPhrase = triggerPhrase;
            Type = type;
            Message = message;
            Result = result;
        }
    }

    public class Result
    {
        public enum ResultType { GetItem, MessageOnly };
        public enum Item { wood, fish, gold, coal, copper, iron };


        public ResultType Type { get; }
        public Item ResultItem { get; }
        public string ResultMessage { get; }

        public Result(Item resultItem, string resultMessage)
        {
            Type = ResultType.GetItem;
            ResultItem = resultItem;
            ResultMessage = resultMessage;
        }

        public Result(string resultMessage)
        {
            Type = ResultType.MessageOnly;
            ResultMessage = resultMessage;
        }
    }


}
