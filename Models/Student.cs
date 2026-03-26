using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPresent { get; set; } = true;
        public int LuckyCount { get; set; } = 0;
        public int TimesAsked { get; set; } = 0;
        public DateTime? LastAskedTime { get; set; }

        public Student(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Id}|{Name}|{IsPresent}|{LuckyCount}|{TimesAsked}";
        }

        public static Student Parse(string line)
        {
            var parts = line.Split('|');
            if (parts.Length >= 2)
            {
                var student = new Student(int.Parse(parts[0]), parts[1]);
                if (parts.Length > 2) student.IsPresent = bool.Parse(parts[2]);
                if (parts.Length > 3) student.LuckyCount = int.Parse(parts[3]);
                if (parts.Length > 4) student.TimesAsked = int.Parse(parts[4]);
                return student;
            }
            return null;
        }
    }
}
