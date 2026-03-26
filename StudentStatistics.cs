using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Models
{
    public class StudentStatistics
    {
        public string StudentName { get; set; }
        public int Id { get; set; }
        public int TimesAsked { get; set; }
        public DateTime? LastAskedTime { get; set; }
        public int LuckyCount { get; set; }
        public bool IsPresent { get; set; }

        public StudentStatistics(Student student)
        {
            StudentName = student.Name;
            Id = student.Id;
            TimesAsked = student.TimesAsked;
            LastAskedTime = student.LastAskedTime;
            LuckyCount = student.LuckyCount;
            IsPresent = student.IsPresent;
        }
    }
}
