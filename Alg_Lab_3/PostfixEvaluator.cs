using System;
using System.IO;
using System.Globalization;

namespace Alg_Lab_3
{
    public class PostfixEvaluator
    {
        // Метод для вычисления постфиксного выражения
        public static double Evaluate(string expression)
        {
            var stack = new Stack<double>();
            
            // Разбиваем строку на токены по пробелам
            var tokens = expression.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                // Если токен - число, парсим и кладем в стек
                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
                {
                    stack.Push(number);
                }
                else
                {
                    // Обработка операторов
                    switch (token)
                    {
                        case "+":
                            {
                                double b = stack.Pop();
                                double a = stack.Pop();
                                stack.Push(a + b);
                                break;
                            }
                        case "-":
                            {
                                double b = stack.Pop();
                                double a = stack.Pop();
                                stack.Push(a - b);
                                break;
                            }
                        case "*":
                            {
                                double b = stack.Pop();
                                double a = stack.Pop();
                                stack.Push(a * b);
                                break;
                            }
                        case "/":
                            {
                                double b = stack.Pop();
                                double a = stack.Pop();
                                if (b == 0) throw new DivideByZeroException("Деление на ноль!");
                                stack.Push(a / b);
                                break;
                            }
                        case "^":
                            {
                                double b = stack.Pop();
                                double a = stack.Pop();
                                stack.Push(Math.Pow(a, b));
                                break;
                            }
                        case "<<":
                            {
                                // Битовый сдвиг влево (приводим к int, так как для double он не работает)
                                int b = (int)stack.Pop();
                                int a = (int)stack.Pop();
                                stack.Push(a << b);
                                break;
                            }
                        // Унарные операторы
                        case "ln":
                            {
                                double a = stack.Pop();
                                stack.Push(Math.Log(a));
                                break;
                            }
                        case "cos":
                            {
                                double a = stack.Pop();
                                stack.Push(Math.Cos(a));
                                break;
                            }
                        case "sin":
                            {
                                double a = stack.Pop();
                                stack.Push(Math.Sin(a));
                                break;
                            }
                        case "sqrt":
                            {
                                double a = stack.Pop();
                                stack.Push(Math.Sqrt(a));
                                break;
                            }
                        default:
                            throw new ArgumentException($"Неизвестный оператор: {token}");
                    }
                }
            }

            if (stack.Count != 1)
                throw new InvalidOperationException("Некорректное постфиксное выражение.");

            return stack.Pop();
        }

        // Метод для чтения из файла и вычисления
        public static void ProcessFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Файл {filePath} не найден.");
                return;
            }

            string expression = File.ReadAllText(filePath).Trim();
            Console.WriteLine($"Прочитано из файла: {expression}");
            
            try
            {
                double result = Evaluate(expression);
                Console.WriteLine($"Результат: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при вычислении: {ex.Message}");
            }
        }
    }
}