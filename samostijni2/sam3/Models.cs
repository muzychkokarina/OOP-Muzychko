namespace lab9_self_v13
{
    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class Student
    {
        public string FullName { get; set; } = string.Empty;
        public int Course { get; set; }
        public double AverageGrade { get; set; }
    }
}