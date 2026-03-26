using MauiApp3.Models;

namespace MauiApp3.Views;

public partial class ClassManagementPage : ContentPage
{
    private StudentService _studentService;
    private List<StudentClass> _classes;

    public ClassManagementPage()
    {
        InitializeComponent();
        _studentService = new StudentService();
        _classes = new List<StudentClass>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadClasses();
    }

    private async Task LoadClasses()
    {
        try
        {
            var classNames = await _studentService.GetClassesAsync();
            _classes.Clear();

            foreach (var className in classNames)
            {
                await _studentService.LoadClassAsync(className);
                var cls = _studentService.GetCurrentClass();
                if (cls != null)
                    _classes.Add(cls);
            }

            ClassesCollectionView.ItemsSource = null;
            ClassesCollectionView.ItemsSource = _classes;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            await DisplayAlert("Błąd", $"Błąd podczas wczytywania klas: {ex.Message}", "OK");
        }
    }

    private async void OnCreateClassClicked(object sender, EventArgs e)
    {
        try
        {
            string className = NewClassNameEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(className))
            {
                await DisplayAlert("Błąd", "Podaj nazwę klasy", "OK");
                return;
            }

            if (_classes.Any(c => c.ClassName == className))
            {
                await DisplayAlert("Błąd", "Klasa o tej nazwie już istnieje", "OK");
                return;
            }

            _studentService.CreateNewClass(className);
            await _studentService.SaveCurrentClassAsync();

            NewClassNameEntry.Text = string.Empty;
            await LoadClasses();

            await DisplayAlert("Sukces", $"Klasa {className} została utworzona", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd", $"Błąd: {ex.Message}", "OK");
        }
    }

    private async void OnEditClassClicked(object sender, EventArgs e)
    {
        try
        {
            var button = sender as Button;
            string className = button?.CommandParameter as string;

            if (!string.IsNullOrWhiteSpace(className))
            {
                System.Diagnostics.Debug.WriteLine($"Navigating to class: {className}");
                await Shell.Current.GoToAsync($"StudentEditPage?name={Uri.EscapeDataString(className)}");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd", $"Błąd nawigacji: {ex.Message}", "OK");
        }
    }

    private async void OnDeleteClassClicked(object sender, EventArgs e)
    {
        try
        {
            var button = sender as Button;
            string className = button?.CommandParameter as string;

            if (string.IsNullOrWhiteSpace(className))
                return;

            bool confirm = await DisplayAlert("Potwierdzenie",
                $"Czy na pewno chcesz usunąć klasę {className}?",
                "Tak", "Nie");

            if (confirm)
            {
                await _studentService.DeleteClassAsync(className);
                await LoadClasses();
                await DisplayAlert("Sukces", $"Klasa {className} została usunięta", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd", $"Błąd: {ex.Message}", "OK");
        }
    }

    private async void OnLoadClassClicked(object sender, EventArgs e)
    {
        try
        {
            var classNames = await _studentService.GetClassesAsync();

            if (classNames.Count == 0)
            {
                await DisplayAlert("Info", "Brak dostępnych klas", "OK");
                return;
            }

            string selected = await DisplayActionSheet("Wybierz klasę", "Anuluj", null, classNames.ToArray());

            if (!string.IsNullOrWhiteSpace(selected) && selected != "Anuluj")
            {
                System.Diagnostics.Debug.WriteLine($"Loading class: {selected}");
                await Shell.Current.GoToAsync($"class?name={Uri.EscapeDataString(selected)}");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd", $"Błąd: {ex.Message}", "OK");
        }
    }
}