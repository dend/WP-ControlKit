using System.Windows;
using System.Windows.Controls;

namespace ControlKit
{
    public class TapMenu : ItemsControl
    {

        public TapMenu()
        {
            DefaultStyleKey = typeof(TapMenu);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
