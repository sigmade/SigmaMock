namespace SigmaMock
{
    public class MethodData
    {
        public required string Name { get; set; }
        public object? ReturnedValue { get; set; }
        public int CallNumberExpected { get; set; }
        public int CallNumberActual { get; set; }
    }
}
