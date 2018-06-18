using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using System.Diagnostics;

namespace VCPA
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SpeechRecognitionEngine speechRecognitionEngine = new SpeechRecognitionEngine();
        SpeechSynthesizer synth = new SpeechSynthesizer();
        

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                speechRecognitionEngine.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(engine_AudioLevelUpdated);
                speechRecognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(engine_SpeechRecognized);

                LoadGrammarAndCommands();

                speechRecognitionEngine.SetInputToDefaultAudioDevice();

                speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);

                synth.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(synth_SpeakCompleted);

                if (synth.State == SynthesizerState.Speaking)
                {
                    synth.SpeakAsyncCancelAll();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Voice recognition failed");
            }

        }

        private void LoadGrammarAndCommands()
        {
            try
            {
                Choices Text = new Choices();
                string[] Lines = File.ReadAllLines(Environment.CurrentDirectory + "\\Commands.txt");
                Text.Add(Lines);
                Grammar WordList = new Grammar(new GrammarBuilder(Text));
                speechRecognitionEngine.LoadGrammar(WordList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void synth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            if (synth.State == SynthesizerState.Speaking)
            {
                synth.SpeakAsyncCancelAll();
            }
        }

        private void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string Speech = e.Result.Text;

            Process[] AllProcesses = Process.GetProcesses();

            switch (Speech)
            {
                case "hello":
                    synth.SpeakAsync("Hello, sir");
                    break;
                case "what time":
                    DateTime now = DateTime.Now;
                    string time = now.GetDateTimeFormats('t')[0];
                    synth.SpeakAsync(time);
                    break;
                case "open google":
                    synth.SpeakAsync("Sure");
                    Process.Start("https://www.google.com");
                    break;
                case "open university":
                    synth.SpeakAsync("OK");
                    Process.Start("https://www.wsiz.rzeszow.pl/pl/Strony/WSIiZ.aspx");
                    break;
                case "open notepad":
                    synth.SpeakAsync("Sure");
                    Process.Start("notepad.exe");
                    break;
                case "open wordpad":
                    synth.SpeakAsync("OK");
                    Process.Start("wordpad.exe");
                    break;
                case "open firefox":
                    synth.SpeakAsync("OK");
                    Process.Start("firefox.exe");
                    break;
                case "open calculator":
                    synth.SpeakAsync("OK");
                    Process.Start("calc.exe");
                    break;
                case "close browser":
                    synth.SpeakAsync("OK");
                    AllProcesses = Process.GetProcesses();
                    foreach (var process in AllProcesses)
                    {
                        if (process.MainWindowTitle != "")
                        {
                            string s = process.ProcessName.ToLower();
                            if (s == "firefox" || s == "microsoft edge" || s == "opera" || s == "google chrome") 
                                process.Kill();
                        }
                    }
                    break;
                case "close wordpad":
                    synth.SpeakAsync("Doing it");
                    AllProcesses = Process.GetProcesses();
                    foreach (var process in AllProcesses)
                    {
                        if (process.MainWindowTitle != "")
                        {
                            string s = process.ProcessName.ToLower();
                            if (s == "wordpad")
                                process.Kill();
                        }
                    }
                    break;
                case "close notepad":
                    synth.SpeakAsync("Doing it");
                    AllProcesses = Process.GetProcesses();
                    foreach (var process in AllProcesses)
                    {
                        if (process.MainWindowTitle != "")
                        {
                            string s = process.ProcessName.ToLower();
                            if (s == "notepad")
                                process.Kill();
                        }
                    }
                    break;
                case "what day":
                    string day;
                    day = "today is" + DateTime.Now.ToString("dddd MMM", new System.Globalization.CultureInfo("en-US"));
                    synth.SpeakAsync(day);
                    break;
                case "stop":
                        synth.SpeakAsyncCancelAll();
                    break;
                case "pause":
                    if (synth.State == SynthesizerState.Speaking)
                        synth.Pause();
                    break;
                case "resume":
                    if (synth.State == SynthesizerState.Paused)
                        synth.Resume();
                    break;
            }
        }

        private void engine_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            progressBar.Value = e.AudioLevel;
        }
    }
}
