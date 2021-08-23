using System.Collections.Generic;

namespace ConsoleBotLinguist.Speller
{
    public class ListError
    {
        public List<Error> Errors { get; set; }
    }

    public class Error
    {
        public int Code { get; set; }

        public int Pos { get; set; }

        public int Row { get; set; }

        public int Col { get; set; }

        public int Len { get; set; }

        public string Word { get; set; }

        public List<S> Ss { get; set; }
    }

    public class S
    {
        public string Value { get; set; }
    }
}
