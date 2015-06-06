using System.Collections.Generic;
using DiningPhilosophers.Contracts.Interfaces;
namespace DiningPhilosophers.GUI.AlgorithmConfig {
    class RandomAlgorithmConfig : IAlgorithmConfig {
        public List<int> OneStepLines { private set; get; }
        public Dictionary<int, int> ReturnLines { private set; get; }
        public Dictionary<int, int> PhilosopherStateLines { private set; get; }
        public Dictionary<int, int> ChoiceLines { private set; get; }

        public RandomAlgorithmConfig() {
            OneStepLines = new List<int> {
                0, 1, 2, 3, 5, 6, 7, 9, 11, 12, 13, 15, 16, 17, 19, 20, 21
            };
            ReturnLines = new Dictionary<int, int> {
                { 4, 2 },
                { 8, 6 },
                { 10, 21 },
                { 14, 12 },
                { 18, 16 },
                { 22, 0 },
            };
            PhilosopherStateLines = new Dictionary<int, int> {  
                { 2, 5 },
                { 6, 9 },
                { 10, 21 },
                { 20, 21 },
                { 12, 15 },
                { 16, 19 },
                { 21, 22 },
            };
            ChoiceLines = new Dictionary<int, int> {
                { 1, 0 },
                { 11, 0 },
            };
        }
    }
}
