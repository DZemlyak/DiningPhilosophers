using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DiningPhilosophers.Contracts.Interfaces;
using DiningPhilosophers.GUI.AlgorithmConfig;
using DiningPhilosophers.Model;
using DiningPhilosophers.Model.Algorithms;
using TextWriter = DiningPhilosophers.DataLayer.TextWriter;

namespace DiningPhilosophers.GUI
{
    public partial class MainForm : Form {
        private const int SleepTime = 1000;
        private List<IPhilosopher> _philosophers;
        private IAlgorithm _algorithm;
        private IAlgorithmConfig _config;
        private readonly IWriter _logWriter;
        private Visualizer _visualizer;

        private bool _autoModeCheck;
        // Allows to start automatic mode in atnother thread
        private Action _autoModeThread;

        private delegate void ManualModeUIReset(object sender, EventArgs e);
        // Allows to start events for UI c using 1 variable
        private readonly ManualModeUIReset _manualModeUiReset;

        public MainForm() {
            InitializeComponent();

            cb_algorithm.SelectedIndex = 0;
            cb_algorithm.SelectedIndexChanged += cb_algorithm_SelectedIndexChanged;

            _manualModeUiReset = rb_p1_fork_CheckedChanged;
            _manualModeUiReset += rb_p2_fork_CheckedChanged;
            _manualModeUiReset += rb_p3_fork_CheckedChanged;
            _manualModeUiReset += rb_p4_fork_CheckedChanged;
            _manualModeUiReset += rb_p5_fork_CheckedChanged;

            DefaultInitialization();
            _algorithm = new SimpleAlgorithm();
            _config = new SimpleAlgorithmConfig();
            _logWriter = new TextWriter();
        }

        /// <summary>
        /// Initialization with default values.
        /// </summary>
        private void DefaultInitialization() {
            var fork1 = new Fork();
            var fork2 = new Fork();
            var fork3 = new Fork();
            var fork4 = new Fork();
            var fork5 = new Fork();

            _philosophers = new List<IPhilosopher> {
                new Philosopher("First", fork5, fork1),
                new Philosopher("Ssecond", fork1, fork2),
                new Philosopher("Third", fork2, fork3),
                new Philosopher("Fourth", fork3, fork4),
                new Philosopher("Fifth", fork4, fork5)
            };

            rtb_log.Clear();
            foreach (var philosopher in _philosophers) {
                rtb_log.Text = philosopher.ToString() + rtb_log.Text;
            }

            if(tlp_manual.Visible) return;
            var controls = GetAll(this, typeof(ListBox));
            foreach (var control in controls.Cast<ListBox>()) {
                control.Items.Clear();
                FillAlgorithmText(control);
                control.SelectedIndex = 0;
            }
        }
    }
}
