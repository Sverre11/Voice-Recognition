using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace Voice_Recognition
{
    public partial class Form1 : Form
    {
        private SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();
        SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsync(RecognizeMode.Multiple);
            btnDisable.Enabled = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Choices commands = new Choices();
            commands.Add(new string[] {"say hello", "print my name", "speak selected text", "open notepad", "close notepad"});

            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(commands);

            Grammar grammar = new Grammar(gBuilder);

            recEngine.LoadGrammarAsync(grammar);
            recEngine.SetInputToDefaultAudioDevice();            

            recEngine.SpeechRecognized += RecEngine_SpeechRecognized;
            

        }

        private void RecEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text)
            {
                case "say hello":
                    //PromptBuilder builder = new PromptBuilder();

                    //builder.StartSentence();
                    //builder.AppendText("Hello Jon");
                    //builder.EndSentence();

                    //builder.AppendBreak(PromptBreak.Small);
                    //builder.StartSentence();
                    //builder.AppendText("How are you", PromptEmphasis.Strong);
                    //builder.EndSentence();

                    synthesizer.SpeakAsync("Hello" + Environment.UserName);
                    break;
                case "print my name":
                    richTextBox1.Text += "\n" + Environment.UserName;
                    break;
                case "speak selected text":
                    synthesizer.SpeakAsync(richTextBox1.Text);
                    break;
                case "open notepad":
                    synthesizer.SpeakAsync("Opening Notepad.");                
                    System.Diagnostics.Process.Start(@"C:\Windows\System32\Notepad.exe");                    
                    break;
                case "close notepad":
                    synthesizer.SpeakAsync("Notepad Closed.");
                    Process myprc = GetProcess("notepad");
                    myprc.Kill();                    
                    break;
                
            }
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsyncStop();
            btnDisable.Enabled = false;
        }

        private Process GetProcess(string processname)
        {
            Process[] proc = Process.GetProcessesByName(processname);

            if (proc.Length > 0)
                return proc[0];
            else return null;
        }
    }
}
