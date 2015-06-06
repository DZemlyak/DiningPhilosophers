using System.Collections.Generic;
using DiningPhilosophers.Contracts.Enums;
using DiningPhilosophers.Contracts.Interfaces;
namespace DiningPhilosophers.Model.Algorithms {
    public class RandomAlgorithm : IAlgorithm {
        public bool MakeStep(int philosopherNumber, List<IPhilosopher> philosophers, int line = -1) {
            var philosopher = philosophers[philosopherNumber];
            switch (philosopher.State) {
                case PhilosopherState.Waiting:
                    switch (line) {
                        case 2:
                            if (philosopher.IsLeftForkFree) {
                                philosopher.ChangeLeftForkState();
                                return true;
                            }
                            break;
                        case 12:
                            if (philosopher.IsRightForkFree) {
                                philosopher.ChangeRightForkState();
                                return true;
                            }
                            break;
                        default:
                            return false;
                    }
                    return false;
                case PhilosopherState.HoldingLeftFork:
                    if (philosopher.IsRightForkFree){
                        philosopher.ChangeRightForkState();
                        return true;
                    }
                    return false;
                case PhilosopherState.HoldingRightFork:
                    if (philosopher.IsLeftForkFree) {
                        philosopher.ChangeLeftForkState();
                        return true;
                    }
                    return false;
                case PhilosopherState.HoldingLeftAndRightForks:
                    philosopher.ChangeRightForkState();
                    return true;
                case PhilosopherState.Eating:
                    philosopher.ChangeLeftForkState();
                    philosopher.ChangeRightForkState();
                    return true;
            }
            return false;
        }
    }
}
