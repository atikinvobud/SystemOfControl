using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Models;

public class Context : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public Context(DbContextOptions options) : base(options) { }

}