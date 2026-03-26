using MauiApp3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Services
{
    public class DrawingService
    {
        private Random _random = new Random();
        private List<Student> _drawnHistory = new();

        public Student DrawStudent(StudentClass studentClass)
        {
            if (studentClass?.Students.Count == 0)
                return null;

            var presentStudents = studentClass.Students
                .Where(s => s.IsPresent)
                .ToList();

            if (presentStudents.Count == 0)
                return null;

            var drawnStudent = presentStudents[_random.Next(presentStudents.Count)];
            drawnStudent.TimesAsked++;
            drawnStudent.LastAskedTime = DateTime.Now;

            if (drawnStudent.Id == studentClass.LuckyNumber)
            {
                drawnStudent.LuckyCount++;
            }

            _drawnHistory.Add(drawnStudent);
            return drawnStudent;
        }

        public bool IsLuckyNumber(Student student, StudentClass studentClass)
        {
            return student.Id == studentClass.LuckyNumber;
        }

        public List<Student> GetDrawnHistory()
        {
            return _drawnHistory;
        }

        public void ClearHistory()
        {
            _drawnHistory.Clear();
        }
    }
}
