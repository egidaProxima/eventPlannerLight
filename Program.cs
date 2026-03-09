/*using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq;*/

class Program
{
    static void Main()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            ShowOptions();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                string[] mainEnter = Console.ReadLine()!.Split(' ');
                Console.ForegroundColor = ConsoleColor.White;

                if (mainEnter[0] == "exit")
                {
                    break;
                }
                else if (mainEnter[0] == "add")
                {
                    if (mainEnter.Length > 2 && mainEnter[1] == "worker")
                    {
                        PrintLn("enter Workers:  |  Age  |   Name   |   Surname   |\n");

                        while (true)
                        {
                            string[] enter = Console.ReadLine()!.Split(' ');

                            if (enter[0] == "exit")
                            {
                                PrintLn("exit from 'add worker'", ConsoleColor.Cyan);
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
                    else if (mainEnter.Length > 2 && mainEnter[1] == "event")
                    {
                        PrintLn("enter Events:  |    Adress   |     Date     |   \n");

                        while (true)
                        {
                            string[] enter = Console.ReadLine()!.Split(' ');

                            if (enter[0] == "exit")
                            {
                                PrintLn("exit from 'add event'", ConsoleColor.Cyan);
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
                        PrintLn($"error in 2 word of command : " + mainEnter[1]);
                    }
                }
                else if (mainEnter[0] == "set" && mainEnter[2] == "to")
                {
                    if (mainEnter[1] == "worker")
                    {
                        if (mainEnter[3] == "event")
                        {
                            PrintLn("enter event ID");
                            int eventID = int.Parse(Console.ReadLine()!);
                            PrintLn("enter workers of event with ID " + eventID.ToString());

                            while (true)
                            {
                                string[] enter = Console.ReadLine()!.Split(' ');

                                if (enter[0] == "exit")
                                {
                                    PrintLn("exit from 'set worker to event'", ConsoleColor.Cyan);
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
                            PrintLn($"error in 3 word of command : " + mainEnter[3], ConsoleColor.DarkRed);
                        }
                    }
                    else if (mainEnter[1] == "event")
                    {
                        if (mainEnter[3] == "worker")
                        {

                        }
                        else
                        {
                            PrintLn($"error in 3 word of command : " + mainEnter[3].ToString(), ConsoleColor.DarkRed);
                        }
                    }
                    else
                    {
                        PrintLn($"error in 2 word of command : " + mainEnter[2].ToString(), ConsoleColor.DarkRed);
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
                        PrintLn("Workers:  |  Age  |   Name   |   Surname   |\n");
                        foreach (var worker in db.Workers)
                        {
                            PrintLn(worker.Id.ToString() + "  " + worker.Name.ToString() + "  " + worker.Surname.ToString() + "  " + worker.Age.ToString());
                        }
                    }
                    else if (mainEnter[1] == "events")
                    {
                        PrintLn("Events:  |    Adress   |     Date     |   \n");
                        foreach (var measure in db.Measures)
                        {
                            PrintLn(measure.Id.ToString() + "  " + measure.Date.ToString() + "  " + measure.Adress.ToString());
                        }
                    }
                    else if (mainEnter[1] == "schedules")
                    {
                        if (mainEnter.Length > 2 && mainEnter[2] == "by")
                        {
                            if (mainEnter[3] == "workers")
                            {
                                PrintLn("worker:  | worker Id | worker Name | worker Surname |");
                                PrintLn("       event:   | event Id | event Adress | event Date |\n");

                                var workers = db.Workers.Select(e => new
                                {
                                    id = e.Id,
                                    name = e.Name,
                                    surname = e.Surname,
                                    schedule = (db.Schedules.Where(p => p.WorkerId == e.Id)).ToList(),

                                }).ToList();

                                //PrintLn(workers.GetType().ToString());

                                foreach (var worker in workers)
                                {
                                    PrintLn($"{worker.id} {worker.name} {worker.surname}");

                                    foreach (var i in worker.schedule)
                                    {
                                        var measure = (from meas in db.Measures where meas.Id == i.MeasureId select meas).ToList();
                                        PrintLn($"    {measure[0].Id} {measure[0].Adress} {measure[0].Date}");
                                    }
                                }
                            }
                            else if (mainEnter[3] == "events")
                            {

                            }
                            else
                            {
                                PrintLn($"error in 4 word of command : " + mainEnter[3], ConsoleColor.DarkRed);
                            }
                        }
                        else 
                        {
                            PrintLn("Schedules:  | worker Id | worker Name | worker Surname | event Id | event Adress | event Date | \n");
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
                                PrintLn($"{sched.workerId} {sched.name} {sched.surname} {sched.measureId} {sched.adress} {sched.date} ");
                            }
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
    
    
    public static void PrintLn(string enter, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(enter);
        Console.ForegroundColor = ConsoleColor.White;
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
        PrintLn("now date : " + DateTime.Now.ToString(), ConsoleColor.Blue);
        PrintLn("options: \n add - add 'something' \n print 'something'[s] - print in console (print workers) " +
            "\n exit - end operating" +
            "\n cls - clear console \n set - set 'something' to 'something'   (set worker to event / " +
            "set event to worker)", ConsoleColor.Blue);
        PrintLn("entities: \n worker \n event", ConsoleColor.Blue);
    }
}
