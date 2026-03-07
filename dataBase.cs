using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public DbSet<Worker> Workers { get; set; }
    public DbSet<Measure> Measures { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=1234");
    }
}


public class Worker
{
    public int Id { get; set; }
    public required int Age { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
}
public class Measure
{
    public int Id { get; set; }
    public required string Adress { get; set; }
    public required DateOnly Date { get; set; }
}

[PrimaryKey(nameof(WorkerId), nameof(MeasureId))]
public class Schedule
{
    public required int WorkerId { get; set; }
    public required int MeasureId { get; set; }
}