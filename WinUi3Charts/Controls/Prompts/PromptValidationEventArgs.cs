using System;

namespace WinUi3Charts.Controls
{
    public class PromptValidationEventArgs : EventArgs
    {
        public PromptModel Prompt { get; }
        public string ValidationMessage { get; }

        public PromptValidationEventArgs(PromptModel prompt, string validationMessage)
        {
            Prompt = prompt;
            ValidationMessage = validationMessage;
        }
    }
}
