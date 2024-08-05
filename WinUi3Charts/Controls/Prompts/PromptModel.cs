using System.Collections.ObjectModel;

namespace WinUi3Charts.Controls
{
    public class PromptModel
    {
        public string Text { get; set; }
        public PromptType Type { get; set; }
        public double FontSize { get; set; } = 16;
        public Windows.UI.Text.FontStyle FontStyle { get; set; } = Windows.UI.Text.FontStyle.Normal;
        public string Answer { get; set; }
        public bool? YesSelected { get; set; }
        public bool? NoSelected { get; set; }
        public ObservableCollection<string> MultipleChoiceOptions { get; set; } = new ObservableCollection<string>();
        public int? SliderValue { get; set; }
        public int SliderMin { get; set; } = 0;
        public int SliderMax { get; set; } = 100;

        public bool IsRequired { get; set; } = true;
        public string DefaultAnswer { get; set; }

        public PromptModel() { }

        public PromptModel(string text, PromptType type)
        {
            Text = text;
            Type = type;
        }
    }
}
