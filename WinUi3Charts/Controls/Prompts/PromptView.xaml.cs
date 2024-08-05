using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Linq;
using Windows.UI;

namespace WinUi3Charts.Controls
{
    public sealed partial class PromptView : UserControl
    {
        public static readonly DependencyProperty SelectionColorProperty =
            DependencyProperty.Register(nameof(SelectionColor), typeof(Color), typeof(PromptView),
                new PropertyMetadata(Colors.LightGreen, OnSelectionColorChanged));

        public static readonly DependencyProperty PromptsProperty =
            DependencyProperty.Register(nameof(Prompts), typeof(PromptCollection), typeof(PromptView),
                new PropertyMetadata(new PromptCollection(), OnPromptsChanged));

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(PromptView),
                new PropertyMetadata(null, OnBackgroundColorChanged));

        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register(nameof(TextColor), typeof(Brush), typeof(PromptView),
                new PropertyMetadata(new SolidColorBrush(Colors.Black), OnTextColorChanged));

        public Color SelectionColor
        {
            get => (Color)GetValue(SelectionColorProperty);
            set => SetValue(SelectionColorProperty, value);
        }

        public PromptCollection Prompts
        {
            get => (PromptCollection)GetValue(PromptsProperty);
            set => SetValue(PromptsProperty, value);
        }

        public Brush BackgroundColor
        {
            get => (Brush)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public Brush TextColor
        {
            get => (Brush)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public event EventHandler<PromptValidationEventArgs> PromptValidationFailed;
        public delegate void PromptsCompletedEventHandler(object sender, EventArgs e);
        public event PromptsCompletedEventHandler PromptsCompleted;

        private int _currentPromptIndex = 0;

        public PromptView()
        {
            this.InitializeComponent();
            UpdateTextColor();
        }

        private static void OnSelectionColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var promptView = (PromptView)d;
            promptView.UpdatePromptUI();
        }

        private static void OnPromptsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var promptView = (PromptView)d;
            promptView.UpdatePromptUI();
        }

        private static void OnBackgroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var promptView = (PromptView)d;
            promptView.UpdateBackgroundColor();
        }

        private static void OnTextColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var promptView = (PromptView)d;
            promptView.UpdateTextColor();
        }
        private void UpdateBackgroundColor()
        {
            RootGrid.Background = BackgroundColor;
        }

        private void UpdateTextColor()
        {
            PromptText.Foreground = TextColor;
            BackButton.Foreground = TextColor;
            NextButton.Foreground = TextColor;
            if (Prompts != null && Prompts.Any())
            {
                InputContentPresenter.Content = BuildInputUI(Prompts[_currentPromptIndex]);
            }
        }

        private void UpdatePromptUI()
        {
            if (Prompts != null && Prompts.Any())
            {
                var currentPrompt = Prompts[_currentPromptIndex];
                PromptText.Text = currentPrompt.Text;
                PromptText.FontSize = currentPrompt.FontSize;
                PromptText.FontStyle = currentPrompt.FontStyle;
                UpdateTextColor();

                InputContentPresenter.Content = BuildInputUI(currentPrompt);

                NextButton.Content = _currentPromptIndex == Prompts.Count - 1 ? "Finish" : "Next";
                BackButton.Visibility = _currentPromptIndex > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        private UIElement BuildInputUI(PromptModel prompt)
        {
            switch (prompt.Type)
            {
                case PromptType.Info:
                    return new TextBlock { Text = prompt.Text, Foreground = TextColor };
                case PromptType.YesNo:
                    return BuildYesNoUI(prompt);
                case PromptType.Textbox:
                    return BuildTextboxUI(prompt);
                case PromptType.Number:
                    return BuildNumberUI(prompt);
                case PromptType.MultipleChoice:
                    return BuildMultipleChoiceUI(prompt);
                case PromptType.Slider:
                    return BuildSliderUI(prompt);
                default:
                    return new TextBlock { Text = "Unsupported prompt type", Foreground = TextColor };
            }
        }

        private UIElement BuildYesNoUI(PromptModel prompt)
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            var yesButton = new Button { Content = "Yes", Margin = new Thickness(0, 0, 10, 0) };
            var noButton = new Button { Content = "No" };

            yesButton.Click += (s, e) => { prompt.YesSelected = true; prompt.NoSelected = false; prompt.Answer = "Yes"; UpdateButtonStates(); };
            noButton.Click += (s, e) => { prompt.YesSelected = false; prompt.NoSelected = true; prompt.Answer = "No"; UpdateButtonStates(); };

            stackPanel.Children.Add(yesButton);
            stackPanel.Children.Add(noButton);

            UpdateButtonStates();

            void UpdateButtonStates()
            {
                yesButton.Background = prompt.YesSelected == true ? new SolidColorBrush(SelectionColor) : null;
                noButton.Background = prompt.NoSelected == true ? new SolidColorBrush(SelectionColor) : null;
                yesButton.Foreground = TextColor;
                noButton.Foreground = TextColor;
            }

            return stackPanel;
        }

        private UIElement BuildTextboxUI(PromptModel prompt)
        {
            var textBox = new TextBox
            {
                Width = 200,
                Text = prompt.Answer ?? prompt.DefaultAnswer ?? "",
                Foreground = TextColor
            };
            textBox.TextChanged += (s, e) => prompt.Answer = textBox.Text;
            return textBox;
        }

        private UIElement BuildNumberUI(PromptModel prompt)
        {
            var numberBox = new NumberBox
            {
                Width = 200,
                Value = double.TryParse(prompt.Answer ?? prompt.DefaultAnswer, out double value) ? value : 0,
                Foreground = TextColor
            };
            numberBox.ValueChanged += (s, e) => prompt.Answer = numberBox.Value.ToString();
            return numberBox;
        }

        private UIElement BuildMultipleChoiceUI(PromptModel prompt)
        {
            var stackPanel = new StackPanel();
            foreach (var option in prompt.MultipleChoiceOptions)
            {
                var button = new Button
                {
                    Content = option,
                    Margin = new Thickness(0, 0, 0, 10),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Foreground = TextColor
                };
                button.Click += (s, e) =>
                {
                    prompt.Answer = option;
                    UpdateButtonStates();
                };
                stackPanel.Children.Add(button);
            }

            UpdateButtonStates();

            void UpdateButtonStates()
            {
                foreach (Button button in stackPanel.Children)
                {
                    button.Background = button.Content.ToString() == prompt.Answer ?
                        new SolidColorBrush(SelectionColor) : null;
                    button.Foreground = TextColor;
                }
            }

            return stackPanel;
        }

        private UIElement BuildSliderUI(PromptModel prompt)
        {
            var slider = new Slider
            {
                Minimum = prompt.SliderMin,
                Maximum = prompt.SliderMax,
                Width = 200,
                Value = double.TryParse(prompt.Answer ?? prompt.DefaultAnswer, out double value) ? value : prompt.SliderMin
            };
            prompt.SliderValue = (int)slider.Value;
            prompt.Answer = prompt.SliderValue.ToString();
            slider.ValueChanged += (s, e) =>
            {
                prompt.SliderValue = (int)slider.Value;
                prompt.Answer = prompt.SliderValue.ToString();
            };

            var valueTextBlock = new TextBlock
            {
                Text = slider.Value.ToString(),
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = TextColor
            };

            slider.ValueChanged += (s, e) =>
            {
                valueTextBlock.Text = ((int)slider.Value).ToString();
            };

            var stackPanel = new StackPanel();
            stackPanel.Children.Add(slider);
            stackPanel.Children.Add(valueTextBlock);

            return stackPanel;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                if (_currentPromptIndex < Prompts.Count - 1)
                {
                    _currentPromptIndex++;
                    UpdatePromptUI();
                }
                else
                {
                    PromptsCompleted?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPromptIndex > 0)
            {
                _currentPromptIndex--;
                UpdatePromptUI();
            }
        }

        private bool ValidateInput()
        {
            if (_currentPromptIndex < 0 || _currentPromptIndex >= Prompts.Count)
            {
                return false;
            }

            var currentPrompt = Prompts[_currentPromptIndex];

            if (!currentPrompt.IsRequired && string.IsNullOrWhiteSpace(currentPrompt.Answer))
            {
                return true;
            }

            bool isValid = true;
            string validationMessage = "";

            switch (currentPrompt.Type)
            {
                case PromptType.Info:
                    isValid = true;
                    break;

                case PromptType.YesNo:
                    isValid = !currentPrompt.IsRequired || currentPrompt.YesSelected.HasValue || currentPrompt.NoSelected.HasValue;
                    validationMessage = "Please select Yes or No.";
                    break;

                case PromptType.Textbox:
                    isValid = !currentPrompt.IsRequired || !string.IsNullOrWhiteSpace(currentPrompt.Answer);
                    validationMessage = "Please enter a value.";
                    break;

                case PromptType.Number:
                    isValid = !currentPrompt.IsRequired || (!string.IsNullOrWhiteSpace(currentPrompt.Answer) && int.TryParse(currentPrompt.Answer, out _));
                    validationMessage = "Please enter a valid number.";
                    break;

                case PromptType.MultipleChoice:
                    isValid = !currentPrompt.IsRequired || (!string.IsNullOrWhiteSpace(currentPrompt.Answer) && currentPrompt.MultipleChoiceOptions.Contains(currentPrompt.Answer));
                    validationMessage = "Please select an option.";
                    break;

                case PromptType.Slider:
                    isValid = true;
                    break;

                default:
                    isValid = false;
                    validationMessage = "Unknown prompt type.";
                    break;
            }

            if (!isValid)
            {
                PromptValidationFailed?.Invoke(this, new PromptValidationEventArgs(currentPrompt, validationMessage));
            }

            return isValid;
        }
    }
}
