using MauiWithAPI.ViewModels;

namespace MauiWithAPI.Pages;

public partial class AuthenPage : ContentPage
{
    private readonly AuthPageViewModel _authPageViewModel;
    public AuthenPage(AuthPageViewModel authPageViewModel)
    {
        InitializeComponent();
        _authPageViewModel = authPageViewModel;
    }

    private async void loginButton_Clicked(object sender, EventArgs e)
    {
        await Task.Delay(2000);

        if (!string.IsNullOrWhiteSpace(usernameTextField.Text)
            && !string.IsNullOrWhiteSpace(passwordTextField.Text))
        {
            _authPageViewModel.LoginForm.Username = usernameTextField.Text;
            _authPageViewModel.LoginForm.Password = passwordTextField.Text;
            if (await _authPageViewModel.Login())
            {
                await AppConstants.NavigateToDashboard();
                usernameTextField.Text = string.Empty;
                passwordTextField.Text = string.Empty;
            }
        }
        else
        {
            await DisplayAlert("Alert!!!", "Please enter your username and password", "Ok");
        }
    }

    private void OnSignUpTapped(object sender, TappedEventArgs e)
    {
        loginFrame.IsVisible = false;
        registerFrame.IsVisible = true;
    }

    private void OnSignInTapped(object sender, TappedEventArgs e)
    {
        registerFrame.IsVisible = false;
        loginFrame.IsVisible = true;
    }
    private async Task CheckConfirmPasswordAsync(string password, string confirmPassword)
    {
        if (password == confirmPassword)
        {
            _authPageViewModel.RegisterForm.Username = usernameRegister.Text;
            _authPageViewModel.RegisterForm.Email = emailRegister.Text;
            _authPageViewModel.RegisterForm.Password = passwordRegister.Text;
            if (await _authPageViewModel.Register())
            {
                registerFrame.IsVisible = false;
                loginFrame.IsVisible = true;
                usernameRegister.Text = string.Empty;
                emailRegister.Text = string.Empty;
                passwordRegister.Text = string.Empty;
                confirmPWRegister.Text = string.Empty;
            }
        }
        else
            await DisplayAlert("Alert!!!", "Password and Confirm Password isn't match", "Ok");
    }

    private async void registerButton_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(usernameRegister.Text)
            && !string.IsNullOrWhiteSpace(emailRegister.Text)
            && !string.IsNullOrWhiteSpace(passwordRegister.Text)
            && !string.IsNullOrWhiteSpace(confirmPWRegister.Text))
        {
            if (emailValidator.IsNotValid)
            {
                await DisplayAlert("Error", "The Email field is not a valid e-mail address", "OK");
            }
            else
            {
                await CheckConfirmPasswordAsync(passwordRegister.Text, confirmPWRegister.Text);
            }
        }
        else
        {
            await DisplayAlert("Alert!!!", "Please enter register form", "Ok");
        }
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        passwordTextField.IsPassword = !e.Value;
    }

    private void CheckBoxPasswordRegister_CheckedChanged(Object sender, CheckedChangedEventArgs e)
    {
        passwordRegister.IsPassword = !e.Value;
    }

    private void CheckBoxConfirmPasswordRegister_CheckedChanged(Object sender, CheckedChangedEventArgs e)
    {
        confirmPWRegister.IsPassword = !e.Value;
    }
}