using System;

namespace Alg_Lab_3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Задание 1: Вычисление постфиксной записи ---");
            // Создайте файл "input.txt" в папке с exe и запишите туда: 7 8 + 3 2 + /
            // Или протестируем здесь строкой:
            string postfixExpr = "7 8 + 3 2 + /";
            Console.WriteLine($"Выражение: {postfixExpr}");
            try
            {
                Console.WriteLine($"Результат: {PostfixEvaluator.Evaluate(postfixExpr)}");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }


            Console.WriteLine("\n--- Задание 2: Инфиксная в постфиксную ---");
            string infixExpr = "2+2"; // Пример из задания
            Console.WriteLine($"Инфиксная: {infixExpr}");
            Console.WriteLine($"Постфиксная: {InfixToPostfixConverter.Convert(infixExpr)}");

            string infixExpr2 = "3 + 4 * 2 / (1 - 5) ^ 2 ^ 3";
            Console.WriteLine($"\nИнфиксная: {infixExpr2}");
            Console.WriteLine($"Постфиксная: {InfixToPostfixConverter.Convert(infixExpr2)}");
        }
    }
}