using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CLIC.Models
{
    public class CLIQuestion
    {
        public int Id { get; set; }
        public CLIQuestionType QuestionType { get; set; }
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public string Answer { get; set; }

        public CLIQuestion()
        {
            Options = new List<string>();
        }

        public string GetQuestionText()
        {
            var t = Text;

            if (Options.Count > 0) // to prevent unwanted empty new line
            {
                t += Environment.NewLine;
                var ot = String.Join(Environment.NewLine, Options.Select(x => $"{Options.IndexOf(x) + 1}) {x}"));
                t += ot;
            }

            return t;
        }

        public string AskQuestion()
        {
            Console.WriteLine();
            Console.WriteLine(GetQuestionText());
            return ReadAnswer();
        }

        private string ReadAnswer()
        {
            Console.Write("-> ");
            var answer = Console.ReadLine();

            if (IsValidAnswer(answer))
            {
                if (QuestionType == CLIQuestionType.NumericSelection)
                    Answer = string.IsNullOrEmpty(answer) ? "1" : answer;
                else if (QuestionType == CLIQuestionType.AlphabeticInput || QuestionType == CLIQuestionType.AlphabeticSelection)
                    Answer = answer;
            }
            else
            {
                if (QuestionType == CLIQuestionType.NumericSelection)
                    Console.WriteLine(" is an invalid selection. Try again.");
                else
                    Console.WriteLine("Invalid input. Try again.");

                ReadAnswer();
            }

            return answer;
        }

        private bool IsValidAnswer(string answer)
        {
            var isValid = true;

            if (IsQuit(answer))
                return true;

            if (QuestionType == CLIQuestionType.NumericSelection)
            {
                answer = Regex.Replace(answer, "[^0-9]", "");
                var notNull = !string.IsNullOrEmpty(answer);
                var isInRange = NumericRangeValidation(answer);
                // add more as required
                isValid = notNull && isInRange;
            }

            if (string.IsNullOrEmpty(answer))
                return false;

            return isValid;
        }

        private bool IsQuit(string answer)
        {
            var a = answer.Trim().ToUpper();
            return a == "Q" || a == "QUIT";
        }

        private bool NumericRangeValidation(string answer)
        {
            bool isValid;

            if (string.IsNullOrEmpty(answer))
                return false;

            var t = Options.Count;
            var ai = int.Parse(answer);
            isValid = ai > 0 && ai <= t;
            return isValid;
        }
    }

    public enum CLIQuestionType
    {
        AlphabeticInput,
        AlphabeticSelection,
        NumericInput,
        NumericSelection
    }

}
