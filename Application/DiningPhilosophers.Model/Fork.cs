using System;
namespace DiningPhilosophers.Model {
    public class Fork {
        public bool IsFree { get; private set; }
        public Fork() {
            IsFree = true;
        }
        public void ChangeForkState() {
            IsFree = !IsFree;
        }
        public override string ToString() {
            return string.Format("{1}. Вилка {0} в данный момент.", IsFree ? "свободна" : "занята", DateTime.Now.ToLongTimeString());
        }
    }
}
