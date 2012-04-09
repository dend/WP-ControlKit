using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlKit
{
    public class TapMenuButton : Button
    {
        public static readonly DependencyProperty TextProperty =
          DependencyProperty.Register("Text", typeof(string), typeof(TapMenuButton), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ImageSourceProperty =
          DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(TapMenuButton), null);

        public TapMenuButton()
        {
            DefaultStyleKey = typeof(TapMenuButton);
        }

        public ImageSource ImageSource
        {
            get { return GetValue(ImageSourceProperty) as ImageSource; }
            set { SetValue(ImageSourceProperty, value); }
        }

        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { SetValue(TextProperty, value); }
        }
    }
}
