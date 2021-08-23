using System.Collections.Generic;

namespace ConsoleBotLinguist.Predictor
{
    public class PredictorResponse
    {
        public bool EndOfWord { get; set; }

        public int Pos { get; set; }

        public List<string> Text { get; set; }
    }
}
