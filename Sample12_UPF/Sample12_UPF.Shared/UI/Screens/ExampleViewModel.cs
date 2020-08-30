using System;
using Ultraviolet;
using Ultraviolet.Core;
using Ultraviolet.Presentation;
using Ultraviolet.Presentation.Controls;

namespace Sample12_UPF.UI.Screens
{
    public class ExampleViewModel
    {
        public ExampleViewModel(UltravioletContext uv)
        {
            this.uv = uv;
        }

        public void Exit(DependencyObject element, RoutedEventData data)
        {
            uv.Host.Exit();
        }

        public void Reset(DependencyObject element, RoutedEventData data)
        {
            this.Message = "Hello, world!";
        }

        public void ButtonClick(DependencyObject element, RoutedEventData data)
        {
            this.Message = "You clicked " + ((Button)element).Content;
        }

        public String Message { get; set; } = "Hello, world!";

        public Boolean IsExitButtonVisible
        {
            get { return uv?.Platform != UltravioletPlatform.iOS; }
        }

        private readonly UltravioletContext uv;
    }
}
