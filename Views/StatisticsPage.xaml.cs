namespace MauiApp3.Views;

public partial class StatisticsPage : ContentPage
{
    private StudentService _studentService;
    private List<string> _availableClasses;

    public StatisticsPage()
    {
        InitializeComponent();
        _studentService = new StudentService();
        _availableClasses = new List<string>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadClasses();
    }

    private async Task LoadClasses()
    {
        _availableClasses = await _studentService.GetClassesAsync();
        ClassPickerStats.ItemsSource = _availableClasses;
    }

    private async void OnClassSelectedStats(object sender, EventArgs e)
    {
        if (ClassPickerStats.SelectedIndex >= 0)
        {
            string selectedClass = _availableClasses[ClassPickerStats.SelectedIndex];
            await _studentService.LoadClassAsync(selectedClass);

            var statistics = _studentService.GetClassStatistics();
            StatisticsCollectionView.ItemsSource = null;
            StatisticsCollectionView.ItemsSource = statistics;
        }
    }
}