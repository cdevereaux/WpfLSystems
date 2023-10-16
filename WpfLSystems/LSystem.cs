using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfLSystems
{
    class LSystem
    {
        public String Word { get; set; }

        Dictionary<char, String> productions = new Dictionary<char, string>();

        public LSystem(String axiom)
        {
            Word = axiom;
            AddProduction('F', "FF+F-F+F+FF");
        }

        public void AddProduction(char predecessor, String successor)
        {
            productions.Add(predecessor, successor);
        }

        public void GenerateNextWord()
        {
            String newWord = "";
            String buffer;
            foreach (char character in Word) {
                if (productions.TryGetValue(character, out buffer)) {
                    newWord += buffer;
                }
                else
                {
                    newWord += character.ToString();
                }
            }
            Word = newWord;
        }
    }
}
