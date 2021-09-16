using System;


namespace Redis.ConsoleApp
{
    [Serializable]
    public class Employee
    {
        public int ID { get; set; }
        public string EmpName { get; set; }

    }
}
