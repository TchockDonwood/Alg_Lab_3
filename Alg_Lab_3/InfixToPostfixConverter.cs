using System;
using System.Collections.Generic;
using System.Text;

namespace Alg_Lab_3
{
    public class InfixToPostfixConverter
    {
        // Приоритет операторов
        private static int GetPrecedence(string op)
        {
            return op switch
            {
                "+" or "-" => 1,
                "*" or "/" or "<<" => 2,
                "^" => 3,
                "sin" or "cos" or "ln" or "sqrt" => 4,
                _ => 0
            };
        }

        // Проверка на право-ассоциативность (степень)
        private static bool IsRightAssociative(string op) => op == "^";

        public static string Convert(string infix)
        {
            // Выходная очередь/список 
            var output = new List<string>();
            var stack = new Stack<string>();

            // Токенизация (разбиваем строку на числа и операторы)
            // Регулярное выражение или простой парсер. Здесь упрощенный парсер для "2+2" или "2 + 2"
            var tokens = Tokenize(infix);

            foreach (var token in tokens)
            {
                if (double.TryParse(token, out _))
                {
                    // Если число - сразу в выход
                    output.Add(token);
                }
                else if (token == "(")
                {
                    stack.Push(token);
                }
                else if (token == ")")
                {
                    while (!stack.IsEmpty() && stack.Top() != "(")
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Pop(); // Удаляем "(" из стека
                }
                else // Если это оператор или функция
                {
                    while (!stack.IsEmpty() && stack.Top() != "(" &&
                           (GetPrecedence(stack.Top()) > GetPrecedence(token) ||
                           (GetPrecedence(stack.Top()) == GetPrecedence(token) && !IsRightAssociative(token))))
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Push(token);
                }
            }

            // Извлекаем оставшиеся операторы
            while (!stack.IsEmpty())
            {
                output.Add(stack.Pop());
            }

            return string.Join(" ", output);
        }

        // Простой токенизатор для отделения чисел от символов (например, "2+2" -> "2", "+", "2")
        private static List<string> Tokenize(string input)
        {
            var tokens = new List<string>();
            var currentToken = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (char.IsWhiteSpace(c)) continue;

                if (char.IsDigit(c) || c == '.')
                {
                    currentToken.Append(c);
                }
                else if (char.IsLetter(c))
                {
                    // Считываем функции типа sin, cos, ln
                    if (currentToken.Length > 0 && !char.IsLetter(currentToken[0]))
                    {
                        tokens.Add(currentToken.ToString());
                        currentToken.Clear();
                    }
                    currentToken.Append(c);
                }
                else
                {
                    // Операторы и скобки
                    if (currentToken.Length > 0)
                    {
                        tokens.Add(currentToken.ToString());
                        currentToken.Clear();
                    }

                    // Обработка <<
                    if (c == '<' && i + 1 < input.Length && input[i + 1] == '<')
                    {
                        tokens.Add("<<");
                        i++;
                    }
                    else
                    {
                        tokens.Add(c.ToString());
                    }
                }
            }

            if (currentToken.Length > 0)
                tokens.Add(currentToken.ToString());

            return tokens;
        }
    }
}