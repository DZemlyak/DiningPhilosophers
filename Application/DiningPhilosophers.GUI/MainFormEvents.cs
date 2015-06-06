using System;
using System.IO;
using System.Windows.Forms;
using DiningPhilosophers.GUI.AlgorithmConfig;
using DiningPhilosophers.GUI.Properties;
using DiningPhilosophers.Model.Algorithms;

namespace DiningPhilosophers.GUI {
    public partial class MainForm {
        private void MainForm_Load(object sender, EventArgs e) {
            _visualizer = new Visualizer(wb_philosophers);
            _visualizer.VisualizePage();
        }

        // Button click events for all philosopher
        private void btn_phil1_step_Click(object sender, EventArgs e) {
            var line = lb_phil_1.SelectedIndex;
            string message;
            line = MakeStep(line, 0, out message);
            if (message == null)
                rtb_log.Text = _philosophers[0].ToString() + rtb_log.Text;
            else
                rtb_log.Text = message + rtb_log.Text;
            lb_phil_1.SelectedIndex = line;
        }

        private void btn_phil2_step_Click(object sender, EventArgs e) {
            var line = lb_phil_2.SelectedIndex;
            string message;
            line = MakeStep(line, 1, out message);
            if (message == null)
                rtb_log.Text = _philosophers[1].ToString() + rtb_log.Text;
            else
                rtb_log.Text = message + rtb_log.Text;
            lb_phil_2.SelectedIndex = line;
        }

        private void btn_phil3_step_Click(object sender, EventArgs e) {
            var line = lb_phil_3.SelectedIndex;
            string message;
            line = MakeStep(line, 2, out message);
            if (message == null)
                rtb_log.Text = _philosophers[2].ToString() + rtb_log.Text;
            else
                rtb_log.Text = message + rtb_log.Text;
            lb_phil_3.SelectedIndex = line;
        }

        private void btn_phil4_step_Click(object sender, EventArgs e) {
            var line = lb_phil_4.SelectedIndex;
            string message;
            line = MakeStep(line, 3, out message);
            if (message == null)
                rtb_log.Text = _philosophers[3].ToString() + rtb_log.Text;
            else
                rtb_log.Text = message + rtb_log.Text;
            lb_phil_4.SelectedIndex = line;
        }

        private void btn_phil5_step_Click(object sender, EventArgs e) {
            var line = lb_phil_5.SelectedIndex;
            string message;
            line = MakeStep(line, 4, out message);
            if (message == null)
                rtb_log.Text = _philosophers[4].ToString() + rtb_log.Text;
            else
                rtb_log.Text = message + rtb_log.Text;
            lb_phil_5.SelectedIndex = line;
        }

        // Events for changing button text according to fork state

        private void rb_p1_fork_CheckedChanged(object sender, EventArgs e) {
            btn_p1.Text = CheckForkState(_philosophers[0], rb_p1_left_fork);
        }

        private void rb_p2_fork_CheckedChanged(object sender, EventArgs e) {
            btn_p2.Text = CheckForkState(_philosophers[1], rb_p2_left_fork);
        }

        private void rb_p3_fork_CheckedChanged(object sender, EventArgs e) {
            btn_p3.Text = CheckForkState(_philosophers[2], rb_p3_left_fork);
        }

        private void rb_p4_fork_CheckedChanged(object sender, EventArgs e) {
            btn_p4.Text = CheckForkState(_philosophers[3], rb_p4_left_fork);
        }

        private void rb_p5_fork_CheckedChanged(object sender, EventArgs e) {
            btn_p5.Text = CheckForkState(_philosophers[4], rb_p5_left_fork);
        }


        // Button Click events in manual mode

        private void btn_p1_Click(object sender, EventArgs e) {
            rtb_log.Text = MakeManualMove(_philosophers[0], rb_p1_left_fork) + rtb_log.Text;
            rb_p1_fork_CheckedChanged(sender, e);
            _visualizer.MoveFork(0, _philosophers[0], true);
        }

        private void btn_p2_Click(object sender, EventArgs e) {
            rtb_log.Text = MakeManualMove(_philosophers[1], rb_p2_left_fork) + rtb_log.Text;
            rb_p2_fork_CheckedChanged(sender, e);
            _visualizer.MoveFork(1, _philosophers[1], true);
        }

        private void btn_p3_Click(object sender, EventArgs e) {
            rtb_log.Text = MakeManualMove(_philosophers[2], rb_p3_left_fork) + rtb_log.Text;
            rb_p3_fork_CheckedChanged(sender, e);
            _visualizer.MoveFork(2, _philosophers[2], true);
        }

        private void btn_p4_Click(object sender, EventArgs e) {
            rtb_log.Text = MakeManualMove(_philosophers[3], rb_p4_left_fork) + rtb_log.Text;
            rb_p4_fork_CheckedChanged(sender, e);
            _visualizer.MoveFork(3, _philosophers[3], true);
        }

        private void btn_p5_Click(object sender, EventArgs e) {
            rtb_log.Text = MakeManualMove(_philosophers[4], rb_p5_left_fork) + rtb_log.Text;
            rb_p5_fork_CheckedChanged(sender, e);
            _visualizer.MoveFork(4, _philosophers[4], true);
        }

        // Automatic mode start method
        private void btn_automatic_mode_Click(object sender, EventArgs e) {
            _autoModeCheck = !_autoModeCheck;
            ChangeAutomaticModeUI();
            IAsyncResult result = null;
            if (_autoModeCheck) {
                _autoModeThread = () => AutomaticMode(sender, e);
                result = _autoModeThread.BeginInvoke(null, null);
            }
            else {
                if(result != null)
                    _autoModeThread.EndInvoke(result);
            }
        }

        // Changing the philosophers algorithm
        private void cb_algorithm_SelectedIndexChanged(object sender, EventArgs e) {
            switch (cb_algorithm.SelectedIndex) {
                case 0:
                    _algorithm = new SimpleAlgorithm();
                    _config = new SimpleAlgorithmConfig();
                    tlp_manual.Visible = false;
                    tlp_auto.Visible = true;
                    btn_automatic_mode.Visible = true;
                    DefaultInitialization();
                    _visualizer.ResetForks();
                    break;
                case 1:
                    _algorithm = new RandomAlgorithm();
                    _config = new RandomAlgorithmConfig();
                    tlp_manual.Visible = false;
                    tlp_auto.Visible = true;
                    btn_automatic_mode.Visible = true;
                    DefaultInitialization();
                    _visualizer.ResetForks();
                    break;
                case 2:
                    tlp_manual.Visible = true;
                    tlp_auto.Visible = false;
                    btn_automatic_mode.Visible = false;
                    _manualModeUiReset(sender, e);
                    if(_autoModeCheck)
                        btn_automatic_mode_Click(sender, e);
                    break;
            }
        }

        private void btn_reset_Click(object sender, EventArgs e) {
            _visualizer.ResetForks();
            DefaultInitialization();
            _manualModeUiReset(sender, e);
            if (_autoModeCheck)
                btn_automatic_mode_Click(sender, e);
        }

        private void btn_save_log_Click(object sender, EventArgs e) {
            saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            try {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                    _logWriter.Write(saveFileDialog1.FileName, rtb_log.Text);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Resources.FileSaveError);
            }
        }
    }
}
