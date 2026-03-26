using MauiApp3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Services
{
    public class StudentService
    {
        private readonly FileService _fileService;
        private StudentClass _currentClass;

        public StudentService()
        {
            _fileService = new FileService();
        }

        public async Task LoadClassAsync(string className)
        {
            var (students, luckyNumber) = await _fileService.LoadClassAsync(className);
            _currentClass = new StudentClass(className) { LuckyNumber = luckyNumber };

            foreach (var studentLine in students)
            {
                var student = Student.Parse(studentLine);
                if (student != null)
                    _currentClass.Students.Add(student);
            }
        }

        public async Task SaveCurrentClassAsync()
        {
            if (_currentClass == null) return;

            var studentStrings = _currentClass.Students.Select(s => s.ToString()).ToList();
            await _fileService.SaveClassAsync(_currentClass.ClassName, studentStrings, _currentClass.LuckyNumber);
        }

        public void CreateNewClass(string className)
        {
            _currentClass = new StudentClass(className);
        }

        public void AddStudent(string name)
        {
            if (_currentClass == null) return;

            int newId = _currentClass.Students.Count > 0
                ? _currentClass.Students.Max(s => s.Id) + 1
                : 1;

            _currentClass.Students.Add(new Student(newId, name));
        }

        public void RemoveStudent(int studentId)
        {
            if (_currentClass == null) return;

            var student = _currentClass.Students.FirstOrDefault(s => s.Id == studentId);
            if (student != null)
                _currentClass.Students.Remove(student);
        }

        public void UpdateStudent(int studentId, string name, bool isPresent)
        {
            if (_currentClass == null) return;

            var student = _currentClass.Students.FirstOrDefault(s => s.Id == studentId);
            if (student != null)
            {
                student.Name = name;
                student.IsPresent = isPresent;
            }
        }

        public StudentClass GetCurrentClass()
        {
            return _currentClass;
        }

        public async Task<List<string>> GetClassesAsync()
        {
            return await _fileService.GetAvailableClassesAsync();
        }

        public async Task DeleteClassAsync(string className)
        {
            await _fileService.DeleteClassAsync(className);
        }

        public void GenerateNewLuckyNumber()
        {
            if (_currentClass != null)
            {
                _currentClass.LuckyNumber = new Random().Next(1, 36);
            }
        }

        public List<StudentStatistics> GetClassStatistics()
        {
            if (_currentClass == null)
                return new List<StudentStatistics>();

            return _currentClass.Students
                .Select(s => new StudentStatistics(s))
                .OrderByDescending(s => s.TimesAsked)
                .ToList();
        }
    }
}
