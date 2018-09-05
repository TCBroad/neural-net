namespace NeuralNet
{
    using System.Collections.Generic;

    public class Layer
    {
        public int Size { get; }

        public List<Neuron> Neurons { get; }

        public Layer(int size)
        {
            this.Size = size;
            this.Neurons = new List<Neuron>(size);
        }
    }
}