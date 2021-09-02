using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace TextStatistics
{
    public class TextStatistics : iTextStatistics
    {
        private string _text;
        private Dictionary<string, long> _wordDictionary = new Dictionary<string, long>();
        private List<WordFrequency> _wordFrequencies;
        private long _numberOfWords = 0;
        private long _numberOfLines = 0;

        public TextStatistics(string text)
        {
            this._text = text;
            this.readWords(text);
        }

        public TextStatistics(List<TextStatistics> textStatistics)
        {
            foreach(TextStatistics currTextStats in textStatistics)
            {
                foreach(KeyValuePair<string, long> kvp in currTextStats._wordDictionary)
                {
                    if(this._wordDictionary.ContainsKey(kvp.Key))
                    {
                        this._wordDictionary[kvp.Key] = this._wordDictionary[kvp.Key] + kvp.Value;
                    }
                    else
                    {
                        this._wordDictionary.Add(kvp.Key, kvp.Value);
                    }
                }

                this._numberOfLines += currTextStats.numberOfLines();
                this._numberOfWords += currTextStats.numberOfWords();
            }
            
            List<WordFrequency> wordFrequencies = new List<WordFrequency>();
            foreach(KeyValuePair<string, long> kvp in this._wordDictionary)
            {
                wordFrequencies.Add(new WordFrequency(kvp.Key, kvp.Value));
            }

            this._wordFrequencies = wordFrequencies;
        }

        private void readWords(string text)
        {
            Dictionary<string, long> wordDictionary = new Dictionary<string, long>();
            string pattern = "[^A-Za-zÀ-ɏ]";
            long wordCount = 0;

            string[] lines = text.Split('\n');

            foreach(string line in lines)
            {
                string[] words = Array.ConvertAll(Regex.Split(line, pattern), d => d.ToUpper());

                foreach(string word in words)
                {
                    if(String.IsNullOrWhiteSpace(word))
                    {
                        continue;
                    }

                    if(wordDictionary.ContainsKey(word))
                    {
                        long count = wordDictionary[word];
                        wordDictionary[word] = count + 1;
                    }
                    else
                    {
                        wordDictionary.Add(word, 1);
                    }
                    wordCount++;
                }
            }
            
            List<WordFrequency> wordFrequencies = new List<WordFrequency>();
            foreach(KeyValuePair<string, long> kvp in wordDictionary)
            {
                wordFrequencies.Add(new WordFrequency(kvp.Key, kvp.Value));
            }

            this._wordDictionary = wordDictionary;
            this._wordFrequencies = wordFrequencies;
            this._numberOfLines = lines.Length;
            this._numberOfWords = wordCount;
        }

        public List<WordFrequency> topWords(int n)
        {
            return this._wordFrequencies.OrderByDescending(x => x.frequency()).ToList().GetRange(0, Math.Min(n, this._wordFrequencies.Count));
        }

        public List<String> longestWords(int n)
        {
            return this._wordFrequencies.OrderByDescending(x => x.word().Length).ToList().Select(x => x.word()).ToList().GetRange(0, Math.Min(n, this._wordFrequencies.Count));
        }

        public long numberOfWords()
        {
            return this._numberOfWords;
        }

        public long numberOfLines()
        {
            return this._numberOfLines;
        }

        
        public void printTextStatistics(int numberOfTopWords = 20, int numberOfLongestWords = 10)
        {
            Console.WriteLine(numberOfTopWords + " Top Words: \n--------------------------");
            foreach(WordFrequency wf in this.topWords(numberOfTopWords)){
                Console.WriteLine("Word (count): " + wf.word() + " (" + wf.frequency() + ")");
            }

            Console.WriteLine("\n" + numberOfLongestWords + " Longest Words: \n--------------------------");
            foreach(string word in this.longestWords(numberOfLongestWords)){
                Console.WriteLine("- " + word);
            }

            Console.WriteLine("\n" + "There are " + this.numberOfWords() + " words spanning " + this.numberOfLines() + " lines.\n\n");
        }
    }

    public interface iTextStatistics {
        /**
        * Returns a list of the most frequented words of the text.
        * @param n how many items of the list
        * @return a list representing the top n frequent words of the text.
        */
        List<WordFrequency> topWords(int n);

        /**
        * Returns a list of the longest words of the text.
        * @param n how many items to return.
        * @return a list with the n longest words of the text.
        */
        List<String> longestWords(int n);

        /**
        * @return total number of words in the text.
        */
        long numberOfWords();
        
        /**
        * @return total number of line of the text.
        */
        long numberOfLines();

        /**
        * Displays the statistics of the text in the console.
        */
        void printTextStatistics(int numberOfTopWords, int numberOfLongestWords);
    }
}