namespace MauiApp3.Views;

[QueryProperty(nameof(ClassName), "name")]
public partial class StudentEditPage : ContentPage
{
    private StudentService _studentService;
    private string _className;

    public string ClassName
    {
        get => _className;
        set
        {
            _className = Uri.UnescapeDataString(value ?? "");
            OnClassNameChanged();
        }
    }

    public StudentEditPage()
    {
        InitializeComponent();
        _studentService = new StudentService();
    }

    private async void OnClassNameChanged()
    {
        if (!string.IsNullOrWhiteSpace(_className))
        {
            await _studentService.LoadClassAsync(_className);
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        var currentClass = _studentService.GetCurrentClass();
        if (currentClass != null)
        {
            ClassNameLabel.Text = $"Klasa: {currentClass.ClassName} (Szczęśliwy nr: {currentClass.LuckyNumber})";
            StudentsCollectionView.ItemsSource = null;
            StudentsCollectionView.ItemsSource = currentClass.Students;
        }
    }

    private async void OnAddStudentClicked(object sender, EventArgs e)
    {
        string studentName = NewStudentNameEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(studentName))
        {
            await DisplayAlert("Błąd", "Podaj imię", "OK");
            return;
        }

        _studentService.AddStudent(studentName);
        NewStudentNameEntry.Text = "";
        RefreshUI();
    }

    private async void OnEditStudentClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: int studentId })
            return;

        var currentClass = _studentService.GetCurrentClass();
        var student = currentClass?.Students.FirstOrDefault(s => s.Id == studentId);

        if (student != null)
        {
            string newName = await DisplayPromptAsync("Edytuj", "Nowe imię:", initialValue: student.Name);

            if (!string.IsNullOrWhiteSpace(newName))
            {
                _studentService.UpdateStudent(studentId, newName, student.IsPresent);
                RefreshUI();
            }
        }
    }

    private async void OnDeleteStudentClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: int studentId })
            return;

        bool confirm = await DisplayAlert("Potwierdzenie", "Usunąć ucznia?", "Tak", "Nie");

        if (confirm)
        {
            _studentService.RemoveStudent(studentId);
            RefreshUI();
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        await _studentService.SaveCurrentClassAsync();
        await DisplayAlert("Sukces", "Zapisano", "OK");
    }

    private async void OnGenerateNewLuckyNumberClicked(object sender, EventArgs e)
    {
        _studentService.GenerateNewLuckyNumber();
        RefreshUI();
        await DisplayAlert("Nowy numerek", "Wygenerowano nowy szczęśliwy numerek!", "OK");
    }
}