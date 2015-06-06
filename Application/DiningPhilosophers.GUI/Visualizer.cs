using System.IO;
using System.Windows.Forms;
using DiningPhilosophers.Contracts.Enums;
using DiningPhilosophers.Contracts.Interfaces;
namespace DiningPhilosophers.GUI {
    class Visualizer {
        private readonly WebBrowser _browser;
        public Visualizer(WebBrowser browser) {
            _browser = browser;
            const string images = "/Temp/Images";
            var info = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Temp");
            info.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + images);
            Properties.Resources.Вилка1.Save(Directory.GetCurrentDirectory() + images + "/Вилка1.png");
            Properties.Resources.Вилка2.Save(Directory.GetCurrentDirectory() + images + "/Вилка2.png");
            Properties.Resources.Вилка3.Save(Directory.GetCurrentDirectory() + images + "/Вилка3.png");
            Properties.Resources.Вилка4.Save(Directory.GetCurrentDirectory() + images + "/Вилка4.png");
            Properties.Resources.Вилка5.Save(Directory.GetCurrentDirectory() + images + "/Вилка5.png");
            Properties.Resources.Больой.Save(Directory.GetCurrentDirectory() + images + "/Больой.png");
            Properties.Resources.Малый.Save(Directory.GetCurrentDirectory() + images + "/Малый.png");
            File.WriteAllText(Directory.GetCurrentDirectory() + "/Temp/PhilosophersTable.html",
                Properties.Resources.PhilosophersTable);
            File.WriteAllText(Directory.GetCurrentDirectory() + "/Temp/jquery-1.11.3.js",
                Properties.Resources.jquery_1_11_3);
            File.WriteAllText(Directory.GetCurrentDirectory() + "/Temp/style.css",
                Properties.Resources.style);
        }

        public void VisualizePage() {
            _browser.Navigate(Directory.GetCurrentDirectory() + "/Temp/PhilosophersTable.html");
        }

        public void MoveFork(int philosopherNumber, IPhilosopher philosopher, bool manualMode) {
            var parameters = new object[3];
            var clear = false;
            parameters[1] = philosopherNumber + 1;
            parameters[2] = false;
            object[] obj;
            switch (philosopher.State) {
                case PhilosopherState.HoldingLeftFork:
                    parameters[0] = philosopherNumber == 0
                        ?  5
                        : philosopherNumber;
                    break;
                case PhilosopherState.HoldingRightFork:
                    parameters[0] = philosopherNumber + 1;
                    break;
                case PhilosopherState.Eating:
                    if (!manualMode) return;
                    obj = new object[] {philosopherNumber + 1};
                    if(!((bool)_browser.Document.InvokeScript("IsSet", obj))) {
                        parameters[0] = philosopherNumber == 0
                            ? 5
                            : philosopherNumber;
                    }
                    else
                        parameters[0] = philosopherNumber + 1;
                    break;
                case PhilosopherState.HoldingLeftAndRightForks:
                    obj = new object[] {philosopherNumber + 1};
                    if(_browser.Document != null && !((bool)_browser.Document.InvokeScript("IsSet", obj))) {
                        parameters[0] = philosopherNumber == 0
                            ? 5
                            : philosopherNumber;
                    }
                    else
                        parameters[0] = philosopherNumber + 1;
                    break;
                case PhilosopherState.Waiting:
                    clear = true;
                    parameters[2] = true;
                    break;
            }
            if (_browser.Document == null) return;
            if (!clear)
                _browser.Document.InvokeScript("animateFork", parameters);
            else {
                parameters[0] = philosopherNumber == 0
                        ? parameters[0] = 5
                        : parameters[0] = philosopherNumber;
                _browser.Document.InvokeScript("animateFork", parameters);
                parameters[0] = philosopherNumber + 1;
                _browser.Document.InvokeScript("animateFork", parameters);
            }
        }

        public void ResetForks() {
            for (int i = 1; i <= 5; i++) {
                var obj = new object[] {i};
                if (_browser.Document != null) _browser.Document.InvokeScript("resetForks", obj);
            }
        }

        ~Visualizer() {
            const string images = "/Temp";
            Directory.Delete(Directory.GetCurrentDirectory() + images, true);
        }

    }
}
