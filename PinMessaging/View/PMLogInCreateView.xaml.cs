using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PinMessaging.Controller;
using PinMessaging.Model;
using PinMessaging.Resources;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;
using PinMessaging.Other;

namespace PinMessaging.View
{
    public partial class PMSignInCreateStructure : PhoneApplicationPage
    {
        private StepNumber _currentStep;
        private readonly PMLogInCreateStructureModel.ActionType _currentActionType;
        private readonly PMLogInModel _pmLogInModel = new PMLogInModel();

        private enum StepNumber
        {
            StepDefault,
            StepEmailClick,
            StepPasswordClick,
            StepPasswordRetypedClick
        }

        public PMSignInCreateStructure()
        {
            InitializeComponent();

            MovePageTitle.Begin();

            _currentStep = StepNumber.StepDefault;

            ChangeVisibility(Visibility.Collapsed);
            ButtonValidate.Click += ButtonValidateClick;

            var model = PhoneApplicationService.Current.State[Paths.ApplicationDico.SignInUpParams] as PMLogInCreateStructureModel;

            if (model != null)
            {
                PageTitle.Text = model.PageTitle;
                PageSubTitle.Text = model.PageSubTitle;
                ButtonValidate.Content = model.ButtonTitle;
                _currentActionType = model.ActionTypeVar;
            }
        }

        private void ChangeVisibility(Visibility visibility)
        {
            TextBoxPassword.Visibility = visibility;
        }

        private void ResetError()
        {
            TextBlockError.Text = string.Empty;
        }

        private void ChangeText(string subTitle, string buttonContent)
        {
            if (subTitle != null)
                PageSubTitle.Text = subTitle;
            if (buttonContent != null)
                ButtonValidate.Content = buttonContent;
        }

        private bool EmailSyntaxCheck()
        {
            if (EmailChecker.IsEmailValid(TextBoxEmail.Text) == false)
            {
                TextBlockError.Text = AppResources.PMWrongEmailSyntax;
                _currentStep = StepNumber.StepDefault;
                return false;
            }
            return true;
        }

        private bool PasswordSyntaxCheck()
        {
            if (TextBoxPassword.Password.Length < 6 || TextBoxPassword.Password.Length > 20)
            {
                TextBlockError.Text = AppResources.PMWrongPasswordSyntax;
                _currentStep = StepNumber.StepEmailClick;
                return false;
            }
            return true;
        }

        private bool ArePasswordsSame()
        {
            if (_pmLogInModel.Password.Equals(_pmLogInModel.PasswordRetyped) == false)
            {
                TextBlockError.Text = AppResources.PMPasswordsNotSame;
                return false;   
            }
            return true;
        }

        private void ResetModelPasswords()
        {
            _pmLogInModel.Password = "";
            _pmLogInModel.PasswordRetyped = "";
        }

        private void ButtonValidateClick(Object sender, EventArgs e)
        {
            _currentStep += 1;

            switch (_currentStep)
            {
                //email only
                case StepNumber.StepEmailClick:

                    if (EmailSyntaxCheck() == false)
                        return;

                    _pmLogInModel.Email = TextBoxEmail.Text;
                    EmailCheckControllerWrapper(_currentActionType);

                    break;

                //password
                case StepNumber.StepPasswordClick:

                    if (PasswordSyntaxCheck() == false)
                        return;

                    _pmLogInModel.Password = TextBoxPassword.Password;
                    PasswordTypedToRetypedOrToMap();
                    break;

                //Password retyped
                case StepNumber.StepPasswordRetypedClick:

                    _pmLogInModel.PasswordRetyped = TextBoxPassword.Password;
                    _pmLogInModel.PhoneSimId = Phone.GetPhoneUniqueId();
                    PasswordRetypedToMap();
                    break;
            }
        }

        private void EmailCheckControllerWrapper(PMLogInCreateStructureModel.ActionType actiontype)
        {
            var pmLogInController = new PMSignInController(AdaptUI, null, RequestType.CheckEmail, actiontype);

            IsUIEnabled(false);
            ResetError();
            pmLogInController.CheckEmailExists(_pmLogInModel);
        }

        private void LogInControllerWrapper()
        {
            var pmLogInController = new PMSignInController(AdaptUI, null, RequestType.SignIn, _currentActionType);

            IsUIEnabled(false);
            pmLogInController.LogIn(_pmLogInModel);
        }

        private void SignUpControllerWrapper()
        {
            var pmSignontroller = new PMSignUpController(AdaptUI, RequestType.SignUp, _currentActionType);

            IsUIEnabled(false);
            pmSignontroller.SignUp(_pmLogInModel);
        }

        private void GoPreviousPage()
        {
            try
            {
                NavigationService.Navigate(Paths.FirstLaunch);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.Critical);
            }
        }   

        private void ButtonPreviousClick(object sender, RoutedEventArgs e)
        {
            if (_currentStep == StepNumber.StepDefault)
                GoPreviousPage();
            else
            {
                _currentStep -= 1;
                TextBlockError.Text = string.Empty;
                ButtonPrevious.IsEnabled = false;

                if (_currentActionType == PMLogInCreateStructureModel.ActionType.SignIn && _currentStep == StepNumber.StepDefault)
                    MoveProgressBarSignInPart1Reverse.Begin();

                if (_currentActionType == PMLogInCreateStructureModel.ActionType.Create)
                {
                    switch (_currentStep)
                    {
                        case StepNumber.StepDefault:
                            MoveProgressBarSignUpPart1Reverse.Begin();
                            break;

                        case StepNumber.StepEmailClick:
                            MoveProgressBarSignUpPart2Reverse.Begin();
                            break;
                    }
                }

                if (_currentStep == StepNumber.StepDefault)
                {
                    ChangeVisibility(Visibility.Collapsed);
                    MoveTextBoxEmailDown.Begin();
                }
                else
                {
                    switch (_currentStep)
                    {
                        case StepNumber.StepEmailClick:
                            ResetModelPasswords();
                            PageSubTitle.Text = AppResources.PMPassword;
                            ButtonValidate.Content = AppResources.ButtonValidate;
                            TextBoxEmail.IsReadOnly = true;
                            ButtonPrevious.IsEnabled = true;
                            break;
                    }
                }
            }
        }

        private void IsUIEnabled(bool lockStatus)
        {
            ProgressBarLoading.IsIndeterminate = !lockStatus;

            ProgressBarLoading.Visibility = ProgressBarLoading.IsIndeterminate ? Visibility.Visible : Visibility.Collapsed;

            TextBoxPassword.IsEnabled = lockStatus;
            ButtonPrevious.IsEnabled = lockStatus;
            ButtonValidate.IsEnabled = lockStatus;
            TextBoxEmail.IsReadOnly = !lockStatus;
        }

        private void PasswordTypedToRetypedOrToMap()
        {
            ButtonPrevious.IsEnabled = true;
            ResetError();

            switch (_currentActionType)
            {
                case PMLogInCreateStructureModel.ActionType.Create:
                    MoveProgressBarSignUpPart2.Begin();
                    ChangeText(AppResources.PMRetypePassword, AppResources.ButtonCreateAccount);
                    if (_currentStep == StepNumber.StepPasswordClick)
                        TextBoxPassword.Password = string.Empty;
                    break;

                case PMLogInCreateStructureModel.ActionType.SignIn:
                    _currentStep = StepNumber.StepEmailClick;

                    LogInControllerWrapper();
                    break;
            }
        }

        private void PasswordRetypedToMap()
        {
            if (_currentActionType == PMLogInCreateStructureModel.ActionType.Create)
            {
                _currentStep = StepNumber.StepPasswordClick;

                if (ArePasswordsSame() == false)
                    return;

                SignUpControllerWrapper();
            }
        }

        private void EmailToPasswordView()
        {
            ChangeVisibility(Visibility.Visible);
            TextBoxEmail.IsReadOnly = true;
            //TextBoxPassword.Focus();

            switch (_currentActionType)
            {
                case PMLogInCreateStructureModel.ActionType.Create:
                    ChangeText(AppResources.PMCreatePassword, null);
                    break;

                case PMLogInCreateStructureModel.ActionType.SignIn:
                    ChangeText(AppResources.PMPassword, AppResources.ButtonConnection);
                    break;
            }
        }

        private void NavigateToMap()
        {
            try
            {
                NavigationService.Navigate(Paths.MapView);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.Critical);
            }
        }

        public bool AdaptUI(RequestType currentType, PMLogInCreateStructureModel.ActionType parentRequestType, bool isOperationSuccessful)
        {
            if (PMData.NetworkProblem == false)
            {  
                switch (currentType)
                {              
                    case RequestType.CheckEmail:

                        if (isOperationSuccessful == false) // email exists
                        {
                            switch (parentRequestType)
                            {
                                case PMLogInCreateStructureModel.ActionType.SignIn:
                                    ResetError();
                                    MoveTextBoxEmailUp.Begin();
                                    MoveProgressBarSignInPart1.Begin();
                                    break;
                                case PMLogInCreateStructureModel.ActionType.Create:
                                    _currentStep = StepNumber.StepDefault;
                                    TextBlockError.Text = AppResources.PMEmailAlreadyExists;
                                    break;
                            }
                        }
                        else //email does not exist
                        {
                            switch (parentRequestType)
                            {
                                case PMLogInCreateStructureModel.ActionType.SignIn:
                                    _currentStep = StepNumber.StepDefault;
                                    TextBlockError.Text = AppResources.PMEmailDoesNotExist;
                                    break;
                                case PMLogInCreateStructureModel.ActionType.Create:
                                    ResetError();
                                    MoveTextBoxEmailUp.Begin();
                                    MoveProgressBarSignUpPart1.Begin();
                                    break;
                            }
                        }
                        break;

                    case RequestType.SignIn:

                        if (isOperationSuccessful)
                        {
                            RememberConnection.SaveLoginPwd(_pmLogInModel);
                            MoveProgressBarSignInPart2.Begin();                  
                        }
                        else
                        {
                            _currentStep = StepNumber.StepEmailClick;
                            TextBlockError.Text = AppResources.PMConnectionFailUser;
                        }
                        break;

                    case RequestType.SignUp:

                        if (isOperationSuccessful)
                        {
                            RememberConnection.SaveLoginPwd(_pmLogInModel);
                            MoveProgressBarSignUpPart3.Begin();
                        }
                        else
                        {
                            ResetModelPasswords();
                            _currentStep = StepNumber.StepEmailClick;
                            TextBlockError.Text = AppResources.PMSignUpError;
                        
                            PageSubTitle.Text = string.Empty;
                            TextBoxEmail.IsReadOnly = true;
                        }
                        break;
                }
                IsUIEnabled(true);
            }
            else
            {
                _currentStep = StepNumber.StepDefault;
                IsUIEnabled(true);
            }
            return true;
        }

        private void MoveTextBoxEmailUp_OnCompleted(object sender, EventArgs e)
        {
            EmailToPasswordView();  
        }

        private void MoveTextBoxEmailDown_OnCompleted(object sender, EventArgs e)
        {
            ButtonValidate.Content = AppResources.ButtonValidate;
            PageSubTitle.Text = AppResources.PMEmail;
            ButtonPrevious.IsEnabled = true;
            TextBoxEmail.IsReadOnly = false;
        }

        private void TextBoxEmail_OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonValidateClick(new object(), e);
                Focus();
            }
        }

        private void TextBoxPassword_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonValidateClick(new object(), e);
            }
        }

        private void SignInUp_OnCompleted(object sender, EventArgs e)
        {
            RememberConnection.SetFirstConnection();
            NavigateToMap(); 
        }
    }
}