using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSKMOModel;

namespace ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WSKMOEntities service = new WSKMOEntities(new Uri("http://localhost:15015/Service1.svc/"));

            Console.WriteLine("===================ПОЛУЧЕНИЕ СТУДЕНТОВ С ОЦЕНКАМИ======================\n");

            var students = service.student
                .Select(s => new
                {
                    s.name,
                    Notes = s.note.ToList()
                })
                .ToList();

            foreach (var student in students)
            {
                Console.WriteLine($"Студент: {student.name}");

                if (student.Notes.Any())
                {
                    foreach (var note in student.Notes)
                    {
                        Console.WriteLine($"{note.subject}: {note.note1}");
                    }
                }
                else
                {
                    Console.WriteLine("Нет оценок");
                }

                Console.WriteLine();
            }

            Console.WriteLine("=====================================================================\n");


            Console.WriteLine("Введите имя нового студента:");
            string studentName = Console.ReadLine();

            student newStudent = new student
            {
                name = studentName
            };

            service.AddTostudent(newStudent);
            service.SaveChanges();

            Console.WriteLine($"Студент {studentName} был успешно добавлен.");

            Console.WriteLine("=====================================================================\n");
        }
    }
}
