using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using System.Diagnostics;
using System.Windows;


namespace VPCATEST
{
    [TestClass]
    public class UnitVPCATest
    {
        SpeechRecognitionEngine speechRecognitionEngine = new SpeechRecognitionEngine();
        SpeechSynthesizer synth = new SpeechSynthesizer();


        public void MainWindow()
        {
            //InitializeComponent();

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
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Voice recognition failed");
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
                //MessageBox.Show(ex.Message);
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

            }
        }

        private void engine_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            //progressBar.Value = e.AudioLevel;
        }

        [TestMethod]
        public void speechRecognitionEngine_IsNotNull()
        {
            Assert.IsNotNull(speechRecognitionEngine);
        }
        [TestMethod]
        public void synth_IsNotNull()
        {
            Assert.IsNotNull(synth);
        }
        [TestMethod]
        public void AreNotSame()
        {
            Assert.AreNotSame(synth,speechRecognitionEngine);
        }
    }


}
