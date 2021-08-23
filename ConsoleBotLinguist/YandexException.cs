using System;

namespace ConsoleBotLinguist
{
    public class YandexException : Exception
    {
        public readonly int Code;

        public YandexException(YandexError error)
            : base(error.Message)
        {
            Code = error.Code;
        }

        public YandexException(int code, string message)
            : base(message)
        {
            Code = code;
        }

        public override string ToString()
        {
            return string.Format("code={0} message={1}", Code, Message);
        }
    }
}
