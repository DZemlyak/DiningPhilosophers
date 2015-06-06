using System.IO;
using DiningPhilosophers.Contracts.Interfaces;
namespace DiningPhilosophers.DataLayer {
    public class TextWriter : IWriter {
        public void Write(string fileName, string textToWrite) {
            File.WriteAllText(fileName, textToWrite);
        }
    }
}
