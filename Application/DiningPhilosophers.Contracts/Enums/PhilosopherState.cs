using System;
namespace DiningPhilosophers.Contracts.Enums {
    [Flags]
    public enum PhilosopherState {
        Waiting = 1,
        Eating,
        HoldingLeftFork,
        HoldingRightFork,
        HoldingLeftAndRightForks
    }
}
