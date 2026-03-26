namespace MauiApp3.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnGoToClassesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ClassManagementPage");
    }

    private async void OnGoToDrawClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("StudentDrawPage");
    }
}