using DiningPhilosophers.Contracts.Enums;
namespace DiningPhilosophers.Contracts.Interfaces {
    public interface IPhilosopher {
        bool IsLeftForkFree { get; }
        bool IsRightForkFree { get; }
        bool IsHoldingLeftFork { get; }
        bool IsHoldingRightFork { get; }
        PhilosopherState State { get; }
        void ChangeLeftForkState();
        void ChangeRightForkState();
        string ToString();
    }
}