using System;

namespace lab19v13
{
    public class InputProcessor
    {
        public int ParseInt(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new InvalidInputException("input", "Рядок не може бути порожнім або містити лише пробіли.");
            }

            if (!int.TryParse(input, out int parsedValue))
            {
                throw new InvalidInputException("input", $"Значення '{input}' не вдалося перетворити на ціле число.");
            }

            return parsedValue;
        }

        public Result<bool> ValidateString(string input, int minLength)
        {
            if (minLength < 0)
            {
                return Result<bool>.Failure("Мінімальна довжина не може бути від'ємною.");
            }

            if (string.IsNullOrEmpty(input))
            {
                return Result<bool>.Failure($"Вхідний рядок порожній (очікується щонайменше {minLength} симв.).");
            }

            if (input.Length < minLength)
            {
                return Result<bool>.Failure($"Довжина рядка '{input}' становить {input.Length}, що менше за мінімальні {minLength} симв.");
            }

            return Result<bool>.Success(true);
        }
    }
}