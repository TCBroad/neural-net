namespace NeuralNet
{
    using System;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            var nn = new NeuralNetwork(10, new [] { 2, 4, 2 });

            var input = new[] {0d, 1d};
            var expected = new[] {1d, 0d};

            for (var i = 0; i < 50; i++)
            {
                nn.Train(input, expected);
                nn.Train(expected, input);
            }

            var output1 = nn.Run(input.ToList());
            var output2 = nn.Run(expected.ToList());

            Console.WriteLine($"Input: [{string.Join(", ", input)}]");
            Console.WriteLine($"Output: [{string.Join(", ", output1)}]\n");

            Console.WriteLine($"Input: [{string.Join(", ", expected)}]");
            Console.WriteLine($"Output: [{string.Join(", ", output2)}]\n");
        }
    }
}