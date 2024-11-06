using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public class Personnel
    {
        public int id;
        public string name, postion, number;
        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }
        public DateTime DateToday { get; set; }
        public double TotalHours { get; set; }
        public Personnel(String name, String postion, String number, int id)
        {
            this.id = id;
            this.name = name;
            this.number = number;
            this.postion = postion;


        }

        public Personnel(int id, string name, string postion, string number)
        {
            this.id = id;
            this.name = name;
            this.postion = postion;
            this.number = number;
        }
    }
}