using System;
using System.Runtime.Serialization;

namespace lab19v13
{
    [Serializable]
    public class InvalidInputException : Exception
    {
        public string FieldName { get; }
        public string Reason { get; }

        public InvalidInputException()
            : base("Вхідні дані мають некоректний формат.")
        {
            FieldName = "Unknown";
            Reason = "Невідома помилка формату";
        }

        public InvalidInputException(string message)
            : base(message)
        {
            FieldName = "Unknown";
            Reason = message;
        }

        public InvalidInputException(string message, Exception innerException)
            : base(message, innerException)
        {
            FieldName = "Unknown";
            Reason = message;
        }

        public InvalidInputException(string fieldName, string reason)
            : base($"Помилка у полі '{fieldName}': {reason}")
        {
            FieldName = fieldName;
            Reason = reason;
        }

        public InvalidInputException(string fieldName, string reason, Exception innerException)
            : base($"Помилка у полі '{fieldName}': {reason}", innerException)
        {
            FieldName = fieldName;
            Reason = reason;
        }

        [Obsolete("This API supports obsolete formatter-based serialization.")]
        protected InvalidInputException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            FieldName = info.GetString(nameof(FieldName)) ?? "Unknown";
            Reason = info.GetString(nameof(Reason)) ?? string.Empty;
        }
    }
}