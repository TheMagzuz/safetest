using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeTest
{
    class ConsoleProgressCounter : ConsoleCounter
    {

        protected int endValue;
        protected int progressBarWidth;

        public ConsoleProgressCounter(string format, int endValue, int progressBarWidth, int initialValue = 0) : base(format, initialValue)
        {
            this.endValue = endValue;
            this.progressBarWidth = progressBarWidth;
        }

        protected override string FormatDisplay()
        {
            return string.Format(format, counter, endValue, GetProgressBar());
        }

        protected virtual string GetProgressBar()
        {
            int progressCharacterCount = (int)Math.Round((counter / (float) endValue) * progressBarWidth);
            string progressCharacters = new string('#', progressCharacterCount);
            string emptyCharacters = new string('_', progressBarWidth-progressCharacterCount);
            return "[" + progressCharacters + emptyCharacters + "]";
        }

    }
}
