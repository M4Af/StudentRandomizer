using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Models
{
    public class StudentClass
    {
        public string ClassName { get; set; }
        public List<Student> Students { get; set; }
        public int LuckyNumber { get; set; }

        public StudentClass(string className)
        {
            ClassName = className;
            Students = new List<Student>();
            LuckyNumber = GenerateRandomLuckyNumber();
        }

        private int GenerateRandomLuckyNumber()
        {
            return new Random().Next(1, 36);
        }

        public override string ToString()
        {
            return ClassName;
        }

        public string ToFileFormat()
        {
            return $"CLASS:{ClassName}|LUCKY:{LuckyNumber}";
        }

        public static StudentClass ParseFromFile(string line)
        {
            var parts = line.Split('|');
            if (parts.Length >= 1)
            {
                var className = parts[0].Replace("CLASS:", "");
                var studentClass = new StudentClass(className);

                if (parts.Length > 1)
                {
                    var luckyPart = parts[1].Replace("LUCKY:", "");
                    studentClass.LuckyNumber = int.Parse(luckyPart);
                }

                return studentClass;
            }
            return null;
        }
    }
}
