using System;

namespace TextStatistics
{
    public class WordFrequency : iWordFrequency
    {
        private string _word;
        private long _frequency;

        public WordFrequency(string word, long frequency)
        {
            this._word = word;
            this._frequency = frequency;
        }

        public String word()
        {
            return this._word;
        }

        public long frequency()
        {
            return this._frequency;
        }
    }

    /**
    * Represents a word and its frequency.
    */
    public interface iWordFrequency {
        /**
        * The word.
        * @return the word as a string.
        */
        String word();

        /**
        * The frequency.
        * @return a long representing the frequency of the word.
        */
        long frequency();
    }
}