using Lab_2_1;

namespace Lab2_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (Lab_2_1.AppContext context = new())
            {
                var course2 = new Course
                {
                    Title = "Python for Data Science",
                    Duration = 45,
                    Description = "Введение в Data Science с использованием Python"
                };

                context.Courses.Add(course2);
                context.SaveChanges();

                Console.WriteLine($"Добавлен курс с Id = {course2.Id}");

                var courseFromDb = context.Courses.FirstOrDefault(c => c.Title == "Python for Data Science");

                if (courseFromDb != null)
                {
                    Console.WriteLine($"Найден курс: {courseFromDb.Title}, Длительность: {courseFromDb.Duration}");

                    courseFromDb.Duration = 50;
                    courseFromDb.Description = "Расширенный курс Data Science на Python";
                    context.SaveChanges();

                    Console.WriteLine("Курс обновлен");
                    Console.WriteLine($"Обновленный курс: {courseFromDb.Title}, Длительность: {courseFromDb.Duration}");
                }
                else
                {
                    Console.WriteLine("Курс не найден."); 
                }
            }
        }
    }
}