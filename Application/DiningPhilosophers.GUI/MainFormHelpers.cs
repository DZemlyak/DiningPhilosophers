using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DiningPhilosophers.Contracts.Enums;
using DiningPhilosophers.Contracts.Interfaces;
using DiningPhilosophers.GUI.Properties;

namespace DiningPhilosophers.GUI {
    public partial class MainForm {
        /// <summary>
        /// Returns all child elements of control of selected type
        /// </summary>
        public IEnumerable<Control> GetAll(Control control, Type type) {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        /// <summary>
        /// Change button text according to fork and it state.
        /// </summary>
        private static string CheckForkState(IPhilosopher philosopher, RadioButton radioButton) {
            if (radioButton.Checked) {
                return philosopher.IsHoldingLeftFork
                    ? String.Format("Положить вилку")
                    : String.Format("Поднять вилку");
            }
            return philosopher.IsHoldingRightFork
                ? String.Format("Положить вилку")
                : String.Format("Поднять вилку");
        }

        /// <summary>
        /// Changes UI controls according to the work mode.
        /// </summary>
        private void ChangeAutomaticModeUI() {
            var controls = GetAll(tlp_auto, typeof(Button));
            if (_autoModeCheck) {
                btn_automatic_mode.Text = Resources.ButtonAutomaticModeStop;
                foreach (var control in controls.Cast<Button>()) 
                    control.Enabled = false;
            }
            else {
                btn_automatic_mode.Text = Resources.ButtonAutomaticModeStart;
                foreach (var control in controls.Cast<Button>())
                    control.Enabled = true;
            }
        }

        /// <summary>
        /// Fills listboxes with algorithm code
        /// </summary>
        private void FillAlgorithmText(ListBox listBox) {
            switch (cb_algorithm.SelectedIndex) {
                case 0:
                    listBox.Items.Add(Resources.AlgorithmTextIsLeftFree);
                    listBox.Items.Add(Resources.AlgorithmTextWait);
                    listBox.Items.Add(Resources.AlgorithmTextBracket);
                    listBox.Items.Add(Resources.AlgorithmTextTakeLeft);
                    listBox.Items.Add(Resources.AlgorithmTextIsRightFree);
                    listBox.Items.Add(Resources.AlgorithmTextWait);
                    listBox.Items.Add(Resources.AlgorithmTextBracket);
                    listBox.Items.Add(Resources.AlgorithmTextTakeRight);
                    listBox.Items.Add(Resources.AlgorithmTextEat);
                    listBox.Items.Add(Resources.AlgorithmTextPut);
                    break;
                case 1:
                    listBox.Items.Add(Resources.AlgorithmTextRandom);
                    listBox.Items.Add(Resources.AlgorithmTextIsEqualLeft);
                    listBox.Items.Add("   " + Resources.AlgorithmTextIsLeftFree);
                    listBox.Items.Add("   " + Resources.AlgorithmTextWait);
                    listBox.Items.Add("   " + Resources.AlgorithmTextBracket);
                    listBox.Items.Add("   " + Resources.AlgorithmTextTakeLeft);
                    listBox.Items.Add("   " + Resources.AlgorithmTextIsRightFree);
                    listBox.Items.Add("   " + Resources.AlgorithmTextWait);
                    listBox.Items.Add("   " + Resources.AlgorithmTextBracket);
                    listBox.Items.Add("   " + Resources.AlgorithmTextTakeRight);
                    listBox.Items.Add(Resources.AlgorithmTextBracket);
                    listBox.Items.Add(Resources.AlgorithmTextElse);
                    listBox.Items.Add("   " + Resources.AlgorithmTextIsRightFree);
                    listBox.Items.Add("   " + Resources.AlgorithmTextWait);
                    listBox.Items.Add("   " + Resources.AlgorithmTextBracket);
                    listBox.Items.Add("   " + Resources.AlgorithmTextTakeRight);
                    listBox.Items.Add("   " + Resources.AlgorithmTextIsLeftFree);
                    listBox.Items.Add("   " + Resources.AlgorithmTextWait);
                    listBox.Items.Add("   " + Resources.AlgorithmTextBracket);
                    listBox.Items.Add("   " + Resources.AlgorithmTextTakeLeft);
                    listBox.Items.Add(Resources.AlgorithmTextBracket);
                    listBox.Items.Add(Resources.AlgorithmTextEat);
                    listBox.Items.Add(Resources.AlgorithmTextPut);
                    break;
            }
        }

        /// <summary>
        /// Makes philosopher step.
        /// </summary>
        private int MakeStep(int line, int philosopher, out string message) {
            message = String.Empty;
            if (_config.ChoiceLines.ContainsValue(line)) {
                var rand = new Random(DateTime.Now.Millisecond);
                line = _config.ChoiceLines.Keys.ElementAt(rand.Next(0, _config.ChoiceLines.Keys.Count));
                message = string.Format("{0}. Философу под номером \"{1}\" была выбрана вилка случайным образом.\n",
                    DateTime.Now.ToLongTimeString(), philosopher);
            }
            else if (_config.PhilosopherStateLines.ContainsKey(line)) {
                if (_algorithm.MakeStep(philosopher, _philosophers, line)) {
                    line = _config.PhilosopherStateLines[line];
                    _visualizer.MoveFork(philosopher, _philosophers[philosopher], false);
                    message = _philosophers[philosopher].ToString();
                }
                else {
                    if (_config.OneStepLines.Contains(line))
                        line++;
                    else
                        message = String.Format("Этот философ не может сейчас ничего сделать.\n");
                }
            }
            else if (_config.OneStepLines.Contains(line))
                line++;
            else if (_config.ReturnLines.ContainsKey(line))
                line = _config.ReturnLines[line];
            else
                throw new Exception("Something went wrong during line changing...\n");
            return line;
        }

        /// <summary>
        /// Changing philosopher states in manual mode.
        /// </summary>
        private static string MakeManualMove(IPhilosopher philosopher, RadioButton radioButton) {
            if (radioButton.Checked) {
                if (philosopher.IsLeftForkFree || philosopher.IsHoldingLeftFork)
                    philosopher.ChangeLeftForkState();
                else
                    return string.Format("{0}. Эта вилка уже занята.\n", DateTime.Now.ToLongTimeString());
            }
            else {
                if (philosopher.IsRightForkFree || philosopher.IsHoldingRightFork)
                    philosopher.ChangeRightForkState();
                else
                    return string.Format("{0}. Эта вилка уже занята.\n", DateTime.Now.ToLongTimeString());
            }
            if (philosopher.State != PhilosopherState.HoldingLeftAndRightForks) return philosopher.ToString();
            var message = philosopher.ToString();
            philosopher.ChangeLeftForkState();
            return philosopher.ToString() + message;
        }

        private delegate void AutoStep(object sender, EventArgs e);

        /// <summary>
        /// Makes philosophers steps every 2 seconds until stop button will be clicked.
        /// </summary>
        private void AutomaticMode(object sender, EventArgs e) {
            while (_autoModeCheck) {
                var rand = new Random(DateTime.Now.Millisecond);
                AutoStep del;
                object[] param = {sender, e};
                switch (rand.Next(0, 5)) {
                    case 0:
                        del = btn_phil1_step_Click;
                        Invoke(del, param);
                        //btn_phil1_step_Click(sender, e);
                        break;
                    case 1:
                        del = btn_phil2_step_Click;
                        Invoke(del, param);
                        //btn_phil2_step_Click(sender, e);
                        break;
                    case 2:
                        del = btn_phil3_step_Click;
                        Invoke(del, param);
                        //btn_phil3_step_Click(sender, e);
                        break;
                    case 3:
                        del = btn_phil4_step_Click;
                        Invoke(del, param);
                        //btn_phil4_step_Click(sender, e);
                        break;
                    case 4:
                        del = btn_phil5_step_Click;
                        Invoke(del, param);
                        //btn_phil5_step_Click(sender, e);
                        break;
                }
                Thread.Sleep(SleepTime);
            }
        }
    }
}
