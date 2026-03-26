using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Models
{
    public class DrawingHistory
    {
        public string ClassName { get; set; }
        public Student DrawnStudent { get; set; }
        public DateTime DrawnTime { get; set; }
        public bool IsLuckyNumber { get; set; }
        public bool StudentWasPresent { get; set; }

        public DrawingHistory(string className, Student student, bool isLucky = false, bool wasPresent = true)
        {
            ClassName = className;
            DrawnStudent = student;
            DrawnTime = DateTime.Now;
            IsLuckyNumber = isLucky;
            StudentWasPresent = wasPresent;
        }

        public override string ToString()
        {
            return $"{ClassName}|{DrawnTime:yyyy-MM-dd HH:mm:ss}|{DrawnStudent.Name}|{IsLuckyNumber}|{StudentWasPresent}";
        }
    }
}
