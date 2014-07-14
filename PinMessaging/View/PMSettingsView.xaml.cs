using System.Windows;
using Microsoft.Phone.Controls;
using PinMessaging.Controller;
using PinMessaging.Utils.WebService;

namespace PinMessaging.View
{
    public partial class PMSettings : PhoneApplicationPage
    {
        public PMSettings()
        {
            InitializeComponent();
        }

        private void ModifyPwdButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Utils.Utils.PasswordSyntaxCheck(NewPwdPasswordBox.Password) == true)
            {
                var sc = new PMSettingsController(RequestType.ChangePassword);
                sc.ChangePassword(OldPwdPasswordBox.Password, NewPwdPasswordBox.Password);       
            }
        }

        private void ModifyEmailButton_Click(object sender, RoutedEventArgs e)
        {
            if (Utils.Utils.IsEmailValid(NewEmailTextBox.Text) == true)
            {
                var sc = new PMSettingsController(RequestType.ChangeEmail);
                sc.ChangeEmail(NewEmailTextBox.Text);
            }
        }

        private void PivotPins_OnSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}