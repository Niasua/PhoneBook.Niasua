using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using PhoneBook.Models;

namespace PhoneBook.Data;

public class PhoneBookContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=PhoneBookDb;Trusted_Connection=True;");
}
