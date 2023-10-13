using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace AudioGreeting.Views.Windows
{
    public partial class ConsoleWindow : Window
    {
        public delegate void SkipGreetingHandler();
        public event SkipGreetingHandler? OnGreetingSkipped;

        public bool IsParentClosed { get; set; }

        public ConsoleWindow()
        {
            InitializeComponent();
        }

        private void OnCloseWindow(object sender, object e)
        {
            if (!IsParentClosed && Keyboard.Modifiers == ModifierKeys.Alt && Keyboard.IsKeyDown(Key.F4))
            {
                if (e is CancelEventArgs ec)
                    ec.Cancel = true;
            }
            
            this.Hide();
        }

        public void ChangePlayingState(object sender, double percentage)
        {
            var console = ConsoleTextBox.Dispatcher;
            console.Invoke(() =>
            {
                StringBuilder result = new StringBuilder(ConsoleTextBox.Text);
                string audioState = $"\t[Audio] {percentage:P2}";

                int index = ConsoleTextBox.Text.LastIndexOf("\t[Audio]");
                bool isUndefined = index == -1;
                bool isLastAudioState = ConsoleTextBox.Text.Length - index < 20;
                if (!isUndefined && isLastAudioState)
                {
                    int count = result.Length - index;
                    result.Remove(index, count);
                }

                result.AppendLine(audioState);
                ConsoleTextBox.Text = result.ToString();
                ConsoleTextBox.CaretIndex = result.Length;
            });
        }
    }
}
