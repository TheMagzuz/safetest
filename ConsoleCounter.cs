using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeTest
{
    class ConsoleCounter
    {
        protected int counter;
        protected int top;
        protected virtual string format { get; set; }

        public ConsoleCounter(string format, int initialValue = 0)
        {
            counter = initialValue;
            top = Console.CursorTop;
            this.format = format;
            Console.WriteLine();
        }

        public virtual void Draw()
        {
            // Store the position of the cursor before writing
            int previousLeft = Console.CursorLeft;
            int previousTop = Console.CursorTop;

            // Go to the position of the counter
            Console.CursorTop = top;
            Console.CursorLeft = 0;
            
            // Write the text
            Console.WriteLine(FormatDisplay());

            // Restore the position of the cursor
            Console.CursorLeft = previousLeft;
            Console.CursorTop = previousTop;
        }

        protected virtual string FormatDisplay()
        {
            return string.Format(format, counter);
        }

        public void Increment(int size=1)
        {
            counter += size;
            Draw();
        }

    }
}
