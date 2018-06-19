using System;
using VCPA;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VPCAUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void MainWindow()
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Voice recognition failed");
            }

        }
    }
}
