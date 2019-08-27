using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using System.Collections.Specialized;

/*

Проверить наличие допустимой сложности пароля, в котором должно быть:
1. Длина от 8 до 32-х символов
2. Два и более символов алфавита в верхнем регистре
3. Два и более символов алфавита в нижнем регистре
4. Две и более цифры
5. Запрет на 3 одинаковых символа, идущих подряд
6. Отсутствие пробела
7. Хотя бы 1 специальный символ
 ! @ # $ % ^ & * ( ) - _ = + \ | [ ] { } ; : / ? . > <

*/

namespace Regex_проверка_сложности_пароля_ООП
{
    class HandleRegex
    {
        public HandleRegex(int minQuantity, int? maxQuantity, string pattern, string message)
        {
            MinQuantity = minQuantity;
            MaxQuantity = maxQuantity;
            Pattern = pattern;
            Message = message;
        }

        public int MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public string Pattern { get; set; }
        public string Message { get; set; }
    }

    abstract class AbstractPassword
    {
        static string password;
                
        public static string Password { get; set; }

        public AbstractPassword(string password)
        {
            Password = password;
        }

        List<HandleRegex> listRegex = new List<HandleRegex>
        {
            new HandleRegex(8, 32, @"(.)", @"1. Длина от 8 до 32-х символов"),
            new HandleRegex(2, null, @"[A-Z]", @"2. Два и более символов алфавита в верхнем регистре"),
            new HandleRegex(2, null, @"[a-z]", @"3. Два и более символов алфавита в нижнем регистре"),
            new HandleRegex(2, null, @"[0-9]", @"4. Две и более цифры"),
            new HandleRegex(0, 0, @"(?<symb>.)\k<symb>\k<symb>", @"5. Запрет на 3 одинаковых символа, идущих подряд")
    };

        


        public List<HandleRegex> PropRegex
        {
            get
            {
                return listRegex;
            }
            set
            {
                listRegex = value;
            }
        }




        //// 1. Длина от 8 до 32-х символов
        //public abstract bool Length();

        //// 2. Два и более символов алфавита в верхнем регистре
        //// 3. Два и более символов алфавита в нижнем регистре  

        //public abstract bool Symbols(int minQuantity, bool upperCase);

        //// 4. Две и более цифры              
        //public abstract bool Digits(int minQuantity);


        //// 5. Запрет на 3 одинаковых символа, идущих подряд
        //public abstract bool SameSymb(int minQuantity);

        //// 6. Отсутствие пробела
        //public abstract bool Spaces();

        //// 7. Наличие хотя бы одного специального символа 
        //// ! @ # $ % ^ & * ( ) - _ = + \ | [ ] { } ; : / ? . > <
        //public abstract bool SpecialSymbols();


        public abstract bool Symbols(HandleRegex demands, out int quantity);

        public abstract void Message(string message, bool isCorrect, int quantity);

        public void TemplateMethod(List<HandleRegex> demands)
        {
                        
            for (int i=0; i < demands.Count; i++)
            {
                int quantity = 0;                                

                Symbols(demands[i], out quantity);
            }
            
        }               
    }

    class ConcretePassword : AbstractPassword
    {       
        public ConcretePassword(string password)
            : base(password)
        {
        }
        
        public override bool Symbols(HandleRegex handleRegex, out int quantity)
        {
            bool result = false;            

            Regex regex = new Regex(handleRegex.Pattern);

            MatchCollection match = regex.Matches(Password);

            quantity = match.Count;

            if (handleRegex.MaxQuantity== null)
            {
                if (quantity >= handleRegex.MinQuantity)
                {
                    result = true;
                }
            }
            else
            {
                if (quantity >= handleRegex.MinQuantity & quantity <= handleRegex.MaxQuantity)
                {
                    result = true;
                }
            }
            

            Message(handleRegex.Message, result, quantity);

            return result;
        }

        public override void Message(string message, bool isCorrect, int currentSymbols)
        {            
            if (isCorrect)
            {                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\t\t{0}\n\nВыполняется: ", message);
                Console.WriteLine("их количество в пароле: {0}", currentSymbols);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\t\t{0}\n\nНе выполняется: ", message);
                Console.WriteLine("их количество в пароле: {0}", currentSymbols);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine(new string ('-', 30) + "\n");
        }

    }


    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("\n\t\tВведите Ваш пароль:\n\n");
            
            string password = Console.ReadLine();

            Console.WriteLine("\n");

            AbstractPassword instance = new ConcretePassword(password); 
            
            
            instance.PropRegex.Add(new HandleRegex(0, 0, @"[ ]", @"6. Отсутствие пробела"));

            string specSymbols = @"\^|\#|\$|\%|\&|\*|\(|\)|\-|_|=|\+|\\|\[|\]|{|}|;|:|/|\?|\.|>|<";

            instance.PropRegex.Add(new HandleRegex(1, null, specSymbols , @"7. Наличие хотя бы одного специального символа"));




            instance.TemplateMethod(instance.PropRegex);


            
            Console.ReadKey();
        }
    }
}
