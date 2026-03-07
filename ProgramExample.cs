using System;
using System.Linq;
/*
class ProgramExample
{
    static void MainExample(string[] args)
    {
        string[] sus = new string[3] { "dsd", "gfg", "ryr" };

        var sos = from susic in sus where (susic == "gfg" || susic == "ryr") select susic;
        foreach (var s in sos)
        {
            Console.WriteLine(s);
        }
        
        // добавление данных
        using (ApplicationContext db = new ApplicationContext())
        {
            // создаем два объекта User
            User user1 = new User { Name = "Tom", Age = 33 };
            User user2 = new User { Name = "Alice", Age = 26 };

            // добавляем их в бд
            db.Users.AddRange(user1, user2);
            db.SaveChanges();
        }
        // получение данных
        using (ApplicationContext db = new ApplicationContext())
        {
            //var users = db.Users.Select(x => x.Name).ToList();
            string? bobo = Console.ReadLine();
            var pisya = from bebe in db.Users where bebe.Name == bobo select bebe;

            foreach (var s in pisya)
            {
                Console.WriteLine(s.Age.ToString(), s.Id.ToString());
            }

            // получаем объекты из бд и выводим на консоль
            
            var users = db.Users.ToList();
            Console.WriteLine("Users list:");
            foreach (User u in users)
            {
                Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
            }
        }
    }
}
*/