using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using ControlKit;

namespace ControlKitExperimental
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            for (int i = 120; i < 130; i++)
            {
                Binder.Instance.RandomTextItems.Add(new Models.TextSet()
                {
                    TextOne = "Welcome",
                    TextTwo = "There are " + i.ToString() +
                        " items available.",
                    TextThree = i.ToString() + " subscribers."
                });
            }

            base.OnNavigatedTo(e);
        }

        private void Grid_Tap(object sender, GestureEventArgs e)
        {
            Grid mainGrid = sender as Grid;
            TapMenu menu = mainGrid.FindName("tapMenu") as TapMenu;
            if (menu.Visibility == System.Windows.Visibility.Collapsed)
                menu.Visibility = System.Windows.Visibility.Visible;
            else
                menu.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}