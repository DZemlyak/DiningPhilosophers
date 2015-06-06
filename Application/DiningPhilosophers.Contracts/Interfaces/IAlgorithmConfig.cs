using System.Collections.Generic;
namespace DiningPhilosophers.Contracts.Interfaces {
    public interface IAlgorithmConfig {
        List<int> OneStepLines { get; }
        Dictionary<int, int> ReturnLines { get; }
        Dictionary<int, int> PhilosopherStateLines { get; }
        Dictionary<int, int> ChoiceLines { get; }
    }
}