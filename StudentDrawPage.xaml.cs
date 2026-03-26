using MauiApp3.Models;

namespace MauiApp3.Views;

public partial class StudentDrawPage : ContentPage
{
    private StudentService _studentService;
    private DrawingService _drawingService;
    private List<string> _availableClasses;
    private StudentClass _currentClass;
    private Student _lastDrawnStudent;

    public StudentDrawPage()
    {
        InitializeComponent();
        _studentService = new StudentService();
        _drawingService = new DrawingService();
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
        ClassPicker.ItemsSource = _availableClasses;
    }

    private async void OnClassSelected(object sender, EventArgs e)
    {
        if (ClassPicker.SelectedIndex >= 0)
        {
            string selectedClass = _availableClasses[ClassPicker.SelectedIndex];
            await _studentService.LoadClassAsync(selectedClass);
            _currentClass = _studentService.GetCurrentClass();

            LuckyNumberLabel.Text = $"Szczęśliwy numerek: {_currentClass.LuckyNumber}";

            DrawBtn.IsEnabled = true;
            _drawingService.ClearHistory();
            HistoryCollectionView.ItemsSource = null;
        }
    }

    private async void OnDrawClicked(object sender, EventArgs e)
    {
        if (_currentClass == null)
        {
            await DisplayAlert("Błąd", "Wybierz klasę", "OK");
            return;
        }

        _lastDrawnStudent = _drawingService.DrawStudent(_currentClass);

        if (_lastDrawnStudent != null)
        {
            DrawnNameLabel.Text = _lastDrawnStudent.Name;

            bool isLucky = _drawingService.IsLuckyNumber(_lastDrawnStudent, _currentClass);
            IsLuckyLabel.IsVisible = isLucky;

            if (isLucky)
            {
                await DisplayAlert("SZCZĘŚLIWY NUMEREK!",
                    $"{_lastDrawnStudent.Name} ma szczęśliwy numerek {_currentClass.LuckyNumber}!",
                    "OK");
            }

            var history = _drawingService.GetDrawnHistory();
            HistoryCollectionView.ItemsSource = null;
            HistoryCollectionView.ItemsSource = history;
        }
        else
        {
            await DisplayAlert("Info", "Wszyscy uczniowie są nieobecni lub nie ma uczniów", "OK");
        }
    }

    private void OnResetClicked(object sender, EventArgs e)
    {
        _drawingService.ClearHistory();
        DrawnNameLabel.Text = "---";
        IsLuckyLabel.IsVisible = false;
        HistoryCollectionView.ItemsSource = null;
    }
}