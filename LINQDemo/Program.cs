using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace LINQDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var people = ReadPeopleFromJsonFile();

            // Get the first element 
            //var firstPerson = people.First();
            var firstPerson = people.FirstOrDefault(); // REcommended 
            if(firstPerson == null)
            {
                // Do something 
                Console.WriteLine("Person not existing"); 
            }
            else
            {
                // Process
            }
            Console.WriteLine(firstPerson);
            var lastPerson = people.LastOrDefault();
            Console.WriteLine(lastPerson);

            var firstPersonStartsWithA = people.FirstOrDefault(p => p.FirstName.StartsWith("A"));
            Console.WriteLine(firstPersonStartsWithA);

            var getSinglePerson = people.SingleOrDefault(p => p.Id == 10004); // Return null
            Console.WriteLine(getSinglePerson);

            // Max Averge Min 
            var maxSalarty = people.Max(p => p.Salary);
            Console.WriteLine($"Highest salary is {maxSalarty}");
            var minSalary = people.Min(p => p.Salary);
            Console.WriteLine($"Min salary is {minSalary}");
            var avergeSalary = people.Average(p => p.Salary);
            Console.WriteLine($"Avg Salary is {avergeSalary}");

            // Any
            if (people.Any())
            {
                Console.WriteLine("There are people in the list");
            }
            else
                Console.WriteLine("No people in the list");

            // Order 
            Console.WriteLine("People By Age -----------------"); 
            var peopleSortedByAge = people.OrderBy(p => p.Birthdate).ThenBy(p => p.FirstName).ToArray();
            //foreach (var item in peopleSortedByAge)
            //{
            //    Console.WriteLine(item); 
            //}

            // Take and Skip 
            var firstTenPeople = people.Take(10);
            var skipTenPeople = people.Skip(10); // 11 tell 200

            // Paging 
            int pageSize = 10;
            int pageNumber = 2;

            Console.WriteLine("Paging -------------------------"); 
            var peopleByPaging = people.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            foreach (var item in peopleByPaging)
            {
                Console.WriteLine(item); 
            }

            // Where 
            Console.WriteLine("Where -------------------------");
            //var filteredData = people.Where(p => p.Birthdate > new DateTime(1990, 1, 1) && p.Salary > 3500);
            //foreach (var item in filteredData)
            //{
            //    Console.WriteLine(item); 
            //}

            // Grouping 
            Console.WriteLine("Grouping -------------------------");
            var peopleByCity = people.GroupBy(p => p.City);

            foreach (var cityGroup in peopleByCity)
            {
                Console.WriteLine($"{cityGroup.Key}");

                foreach (var person in cityGroup)
                {
                    Console.WriteLine($"\t{person}");
                }
            }

            // Query Syntax 
            var peopleWithHighSalaries = (from p in people
                                          where p.Salary > 5000 && p.City == "Cairo"
                                          orderby p.Id descending
                                          select p);
            
            // Projection 
            var olderPeopleIds = people.Where(p => p.Birthdate < new DateTime(1990, 1, 1)).Select(p => p.Id);
            foreach (var item in olderPeopleIds)
            {
                Console.Write($"{item} ");
            }

            var projectionPeople = people.Select(p => new
            {
                Id = p.Id,
                FullName = p.FirstName + " " + p.LastName
            });
            foreach (var item in projectionPeople)
            {
                Console.WriteLine(item.FullName);
            }
            Console.ReadLine();
        }


        static Person[] ReadPeopleFromJsonFile()
        {
            using (var reader = new StreamReader("people.json"))
            {
                string jsonData = reader.ReadToEnd();
                var people = JsonConvert.DeserializeObject<Person[]>(jsonData);
                return people;
            }
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public decimal Salary { get; set; }
        public DateTime Birthdate { get; set; }

        public override string ToString()
        {
            return $"{Id}- {FirstName} {LastName} | {City} | {Birthdate.ToShortDateString()} | ${Salary}";
        }
    }
}
