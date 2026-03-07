using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System.Collections;

class Program
{
    static void Main()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            ShowOptions();

            while (true)
            {
                string[] mainEnter = Console.ReadLine()!.Split(' ');

                if (mainEnter[0] == "exit")
                {
                    break;
                }
                else if (mainEnter[0] == "add")
                {
                    if (mainEnter[1] == "worker")
                    {
                        Console.WriteLine("enter Workers:  |  Age  |   Name   |   Surname   |\n");

                        while (true)
                        {
                            string[] enter = Console.ReadLine()!.Split(' ');

                            if (enter[0] == "exit")
                            {
                                Console.WriteLine("exit from 'add worker'");
                                db.SaveChanges();
                                break;
                            }
                            else
                            {
                                var newWorker = AddWorker(enter);
                                db.Workers.Add(newWorker);
                            }
                        }
                    }
                    else if (mainEnter[1] == "event")
                    {
                        Console.WriteLine("enter Events:  |    Adress   |     Date     |   \n");

                        while (true)
                        {
                            string[] enter = Console.ReadLine()!.Split(' ');

                            if (enter[0] == "exit")
                            {
                                Console.WriteLine("exit from 'add event'");
                                db.SaveChanges();
                                break;
                            }
                            else
                            {
                                var newMeasure = AddMeasure(enter);
                                db.Measures.Add(newMeasure);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"error in 2 word of command : " + mainEnter[1]);
                    }
                }
                else if (mainEnter[0] == "set" && mainEnter[2] == "to")
                {
                    if (mainEnter[1] == "worker")
                    {
                        if (mainEnter[3] == "event")
                        {
                            Console.WriteLine("enter event ID");
                            int eventID = int.Parse(Console.ReadLine()!);
                            Console.WriteLine("enter workers of event with ID ", eventID);

                            while (true)
                            {
                                string[] enter = Console.ReadLine()!.Split(' ');

                                if (enter[0] == "exit")
                                {
                                    Console.WriteLine("exit from 'set worker to event'");
                                    db.SaveChanges();
                                    break;
                                }
                                else
                                {

                                    for (int i = 0; i < enter.Length; i++)
                                    {
                                        var newSchedule = new Schedule { WorkerId = int.Parse(enter[i]), MeasureId = eventID };
                                        db.Schedules.Add(newSchedule);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"error in 3 word of command : " +mainEnter[3]);
                        }
                    }
                    else if (mainEnter[1] == "event")
                    {
                        if (mainEnter[3] == "worker")
                        {

                        }
                        else
                        {
                            Console.WriteLine($"error in 3 word of command : " + mainEnter[3]);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"error in 2 word of command : " + mainEnter[2]);
                    }
                }
                else if (mainEnter[0] == "cls")
                {
                    Console.Clear();
                    ShowOptions();
                }
                else if (mainEnter[0] == "print")
                {
                    if (mainEnter[1] == "workers")
                    {
                        Console.WriteLine("Workers:  |  Age  |   Name   |   Surname   |\n");
                        foreach (var worker in db.Workers)
                        {
                            Console.WriteLine(worker.Id.ToString() + "  " + worker.Name.ToString() + "  " + worker.Surname.ToString() + "  " + worker.Age.ToString());
                        }
                    }
                    else if (mainEnter[1] == "events")
                    {
                        Console.WriteLine("Events:  |    Adress   |     Date     |   \n");
                        foreach (var measure in db.Measures)
                        {
                            Console.WriteLine(measure.Id.ToString() + "  " + measure.Date.ToString() + "  " + measure.Adress.ToString());
                        }
                    }
                    else if (mainEnter[1] == "schedules")
                    {
                        if (mainEnter.Length > 2 && mainEnter[2] == "by")
                        {
                            if (mainEnter[3] == "workers")
                            {
                                Console.WriteLine("worker:  | worker Id | worker Name | worker Surname |");
                                Console.WriteLine("       event:   | event Id | event Adress | event Date |\n");

                                var workers = db.Workers.Select(e => new
                                {
                                    id = e.Id,
                                    name = e.Name,
                                    surname = e.Surname,
                                    schedule = (db.Schedules.Where(p => p.WorkerId == e.Id)).ToList(),

                                }).ToList();

                                //Console.WriteLine(workers.GetType().ToString());

                                foreach (var worker in workers)
                                {
                                    Console.WriteLine($"{worker.id} {worker.name} {worker.surname}");

                                    foreach (var i in worker.schedule)
                                    {
                                        var measure = (from meas in db.Measures where meas.Id == i.MeasureId select meas).ToList();
                                        Console.WriteLine($"    {measure[0].Id} {measure[0].Adress} {measure[0].Date}");
                                    }
                                }
                            }
                            else if (mainEnter[3] == "events")
                            {

                            }
                            else
                            {
                                Console.WriteLine($"error in 4 word of command : " + mainEnter[3]);
                            }
                        }
                        else 
                        {
                            Console.WriteLine("Schedules:  | worker Id | worker Name | worker Surname | event Id | event Adress | event Date | \n");
                            var schedulesW = db.Schedules.Join(db.Workers, // второй набор
                                sched => sched.WorkerId, // свойство-селектор объекта из первого набора
                                worker => worker.Id, // свойство-селектор объекта из второго набора
                                (sched, worker) => new // результат
                                {
                                    measureId = sched.MeasureId,
                                    workerId = worker.Id,
                                    name = worker.Name,
                                    surname = worker.Surname
                                });
                            var schedulesE = schedulesW.Join(db.Measures, // второй набор
                                sched => sched.measureId, // свойство-селектор объекта из первого набора
                                measure => measure.Id, // свойство-селектор объекта из второго набора
                                (sched, measure) => new // результат
                                {
                                    measureId = sched.measureId,
                                    workerId = sched.workerId,
                                    name = sched.name,
                                    surname = sched.surname,
                                    date = measure.Date,
                                    adress = measure.Adress
                                });

                            
                            var list = schedulesE.OrderBy(e => e.workerId).ToList();
                            foreach (var sched in list)
                            {
                                Console.WriteLine($"{sched.workerId} {sched.name} {sched.surname} {sched.measureId} {sched.adress} {sched.date} ");
                            }
                            /*
                            var groups = list.GroupBy(p => p.workerId);
                            foreach (var group in groups)
                            {
                                foreach (var i in group)
                                {
                                    Console.Write($"{i}        ");
                                }
                            }*/
                        }
                    }
                }
                else if (mainEnter[0] == "serach")
                {

                }
            }

            db.SaveChanges();
        }
    }
    
    public static Worker AddWorker(string[] enter)
    {
        Worker newWorker = new Worker {Age = int.Parse(enter[0]), Name = (string)enter[1], Surname = (string)enter[2] };
        return newWorker;
    }
    public static Measure AddMeasure(string[] enter)
    {
        Measure newMeasure = new Measure {Adress = enter[0], Date = DateOnly.Parse(enter[1])};
        return newMeasure;
    }
    public static void ShowOptions()
    {
        Console.WriteLine("now date : " + DateTime.Now.ToString());
        Console.WriteLine("options: \n add - add 'something' \n print 'something'[s] - print in console (print workers) " +
            "\n exit - end operating" +
            "\n cls - clear console \n set - set 'something' to 'something'   (set worker to event / " +
            "set event to worker)");
        Console.WriteLine("entities: \n worker \n event");
    }
}
