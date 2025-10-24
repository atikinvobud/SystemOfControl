using BackEnd.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Models;

public class Context : DbContext
{
    public DbSet<User> userEntities { get; set; }
    public DbSet<UserInfo> userInfoEntities { get; set; }
    public DbSet<Role> roleEntities { get; set; }
    public DbSet<RefreshToken> refreshTokensEntities { get; set; }
    public DbSet<UserRole> userRoleEntities { get; set; }
    public Context(DbContextOptions<Context> options) : base(options) { }
}