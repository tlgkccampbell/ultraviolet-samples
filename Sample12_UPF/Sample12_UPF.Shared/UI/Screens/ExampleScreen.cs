using System;
using Ultraviolet.Content;
using Ultraviolet.UI;

namespace Sample12_UPF.UI.Screens
{
    public class ExampleScreen : UIScreen
    {
        public ExampleScreen(ContentManager globalContent, UIScreenService uiScreenService)
            : base("Content/UI/Screens/ExampleScreen", "ExampleScreen", globalContent)
        {

        }

        protected override Object CreateViewModel(UIView view)
        {
            return new ExampleViewModel(Ultraviolet);
        }
    }
}
