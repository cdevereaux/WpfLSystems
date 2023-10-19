using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfLSystems
{
    class LSystem
    {
        public String Axiom { get; set; }

        public List<(char, String)> productions = new List<(char, string)>();

        public LSystem()
        {
            Axiom = "X";
            AddProduction('F', "FF");
            AddProduction('X', "F-[[X]+X]+F[+FX]-X");
        }

        public void AddProduction(char predecessor, String successor)
        {
            productions.Add((predecessor, successor));
        }

        public String GenerateIteration(uint n)
        {
            String currWord = Axiom;

            for (uint i = 0; i < n; i++)
            {
                String nextWord = "";
                foreach (char character in currWord) {
                    if (productions.Exists(x => x.Item1 == character)) {
                        nextWord += productions.Find(x => x.Item1 == character).Item2;
                    }
                    else
                    {
                        nextWord += character.ToString();
                    }
                }
                currWord = nextWord;
            }
            return currWord;
        }
    }
}
