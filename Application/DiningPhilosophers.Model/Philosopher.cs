using System;
using DiningPhilosophers.Contracts.Enums;
using DiningPhilosophers.Contracts.Interfaces;
namespace DiningPhilosophers.Model {
    public sealed class Philosopher : IPhilosopher {
        private readonly string _name;
        private readonly Fork _leftFork;
        private readonly Fork _rightFork;
        public PhilosopherState State { get; private set; }
        public bool IsLeftForkFree { get { return _leftFork.IsFree; } }
        public bool IsRightForkFree { get { return _rightFork.IsFree; } }

        public bool IsHoldingLeftFork {
            get {
                return State == PhilosopherState.HoldingLeftFork 
                    || State == PhilosopherState.HoldingLeftAndRightForks
                    || State == PhilosopherState.Eating;
            }
        }
        public bool IsHoldingRightFork {
            get {
                return State == PhilosopherState.HoldingRightFork 
                    || State == PhilosopherState.HoldingLeftAndRightForks
                    || State == PhilosopherState.Eating;
            }
        }
        
        public Philosopher(string name, Fork leftFork, Fork rightFork) {
            _name = name;
            _leftFork = leftFork;
            _rightFork = rightFork;
            State = PhilosopherState.Waiting;
        }

        public void ChangeLeftForkState() {
            switch (State) {
                case PhilosopherState.Eating:
                    State = PhilosopherState.HoldingRightFork;
                    break;
                case PhilosopherState.HoldingLeftFork:
                    State = PhilosopherState.Waiting;
                    break;
                case PhilosopherState.Waiting:
                    State = PhilosopherState.HoldingLeftFork;
                    break;
                case PhilosopherState.HoldingRightFork:
                    State = PhilosopherState.HoldingLeftAndRightForks;
                    break;
                case PhilosopherState.HoldingLeftAndRightForks:
                    State = PhilosopherState.Eating;
                    return;
            }
            _leftFork.ChangeForkState();
        }

        public void ChangeRightForkState() {
            switch (State) {
                case PhilosopherState.Eating:
                    State = PhilosopherState.HoldingLeftFork;
                    break;
                case PhilosopherState.HoldingLeftFork:
                    State = PhilosopherState.HoldingLeftAndRightForks;
                    break;
                case PhilosopherState.Waiting:
                    State = PhilosopherState.HoldingRightFork;
                    break;
                case PhilosopherState.HoldingRightFork:
                    State = PhilosopherState.Waiting;
                    break;
                case PhilosopherState.HoldingLeftAndRightForks:
                    State = PhilosopherState.Eating;
                    return;
            }
            _rightFork.ChangeForkState();
        }

        public override string ToString() {
            switch (State) {
                case PhilosopherState.Eating:
                    return string.Format("{1}. \"{0}\" ест.\n", _name, DateTime.Now.ToLongTimeString());
                case PhilosopherState.HoldingLeftFork:
                    return string.Format("{1}. \"{0}\" держит левую вилку.\n", _name, DateTime.Now.ToLongTimeString());
                case PhilosopherState.Waiting:
                    return string.Format("{1}. \"{0}\" ждет.\n", _name, DateTime.Now.ToLongTimeString());
                case PhilosopherState.HoldingRightFork:
                    return string.Format("{1}. \"{0}\" держит правую вилку.\n", _name, DateTime.Now.ToLongTimeString());
                case PhilosopherState.HoldingLeftAndRightForks:
                    return string.Format("{1}. \"{0}\" держит обе вилки.\n", _name, DateTime.Now.ToLongTimeString());
                default:
                    return "Неизвестно чем занят.\n";
            }
        }
    }
}
