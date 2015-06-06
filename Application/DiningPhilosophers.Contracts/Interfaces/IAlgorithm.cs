using System.Collections.Generic;
namespace DiningPhilosophers.Contracts.Interfaces {
    public interface IAlgorithm {
        bool MakeStep(int philosopherNumber, List<IPhilosopher> philosophers, int line = -1);
    }
}
