using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bagira.Server
{
    public class LetterCounter : IMessageStatsProcessor
    {
        ConcurrentDictionary<char, int> _letterCounter = new();
        
        public void RecordMessage(string message)
        {
            foreach (var letter in message)
            {
                if (char.IsLetter(letter))
                {
                    _letterCounter.AddOrUpdate(char.ToLower(letter), 1, (key, value) => value + 1);
                }
            }
            PrintLetterCount();
        }

        private void PrintLetterCount()
        {
            var orderedLetterCounter = _letterCounter.OrderBy(kvp => kvp.Key);
            foreach (var kvp in orderedLetterCounter)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }

    }
}
