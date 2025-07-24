using PhoneBook.Models;

namespace PhoneBook.Data;

public class DbInitializer
{
    public static void SeedCategories(PhoneBookContext context)
    {
        if (context.Categories.Any())
            return;

        var categories = new[]
        {
            new Category { Name = "Work" },
            new Category { Name = "Family" },
            new Category { Name = "Friends" },
            new Category { Name = "Others" }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();
    }
}
