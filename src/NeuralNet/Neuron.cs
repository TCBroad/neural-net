namespace NeuralNet
{
    using System.Collections.Generic;

    public class Neuron
    {
        public List<double> Weights { get; private set; }

        public double Value { get; set; }

        public double Bias { get; set; }

        public double Delta { get; set; }

        public int NumWeights => this.Weights.Count;

        public Neuron()
        {
            this.Bias = CryptoRandom.Next();

            this.Weights = new List<double>();
        }
    }
}