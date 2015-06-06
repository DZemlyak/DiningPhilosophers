using System.Collections.Generic;
using DiningPhilosophers.Contracts.Interfaces;
namespace DiningPhilosophers.GUI.AlgorithmConfig {
    public class SimpleAlgorithmConfig : IAlgorithmConfig {
        public List<int> OneStepLines { private set; get; }
        public Dictionary<int, int> ReturnLines { private set; get; }
        public Dictionary<int, int> PhilosopherStateLines { private set; get; }
        public Dictionary<int, int> ChoiceLines { private set; get; }

        public SimpleAlgorithmConfig() {
            ChoiceLines = new Dictionary<int, int>();
            OneStepLines = new List<int> {
                0, 1, 3, 4, 5, 8
            };
            ReturnLines = new Dictionary<int, int> {
                { 2, 0 },
                { 6, 4 },
                { 9, 0 },
            };
            PhilosopherStateLines = new Dictionary<int, int> {
                { 0, 3 },
                { 4, 7 },
                { 7, 8 },
                { 8, 9 },
            };
        }
    }
}
