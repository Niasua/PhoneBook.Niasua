using PhoneBook.Data;
using PhoneBook.Models;

namespace PhoneBook.Services;

public class CategoryService
{
    private readonly PhoneBookContext db = new();

    public bool AddCategory(Category category)
    {
        if (category == null || string.IsNullOrWhiteSpace(category.Name))
            return false;

        var exists = db.Categories.Any(c => c.Name.ToLower() == category.Name.ToLower());
        if (exists)
            return false;

        db.Categories.Add(category);
        db.SaveChanges();
        return true;
    }

    public List<Category> GetAllCategories()
    {
        return db.Categories
            .OrderBy(c => c.Id)
            .ToList();
    }

    public Category? GetCategoryByName(string name)
    {
        return db.Categories
            .FirstOrDefault(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    public Category? GetCategoryById(int? id)
    {
        return db.Categories
            .FirstOrDefault(c => c.Id == id);
    }

    public bool ModifyCategory(string name, string newName)
    {
        var category = db.Categories.FirstOrDefault(c => c.Name == name);

        if (category == null)
        {
            Console.WriteLine($"No category found with name {name}");
            return false;
        }

        var nameExists = db.Categories.Any(c => c.Name.ToLower() == newName.ToLower() && c.Id != category.Id);
        if (nameExists)
        {
            Console.WriteLine($"A category with name {newName} already exists.");
            return false;
        }

        category.Name = newName;
        db.SaveChanges();

        return true;
    }

    public bool DeleteCategory(int id)
    {
        var category = db.Categories.FirstOrDefault(c => c.Id == id);

        if (category == null)
        {
            Console.WriteLine($"No category found wih ID {id}");
            return false;
        }

        if (db.Contacts.Any(c => c.Category.Id == id))
        {
            Console.WriteLine("Cannot delete category. It is in use by one or more contacts.");
            return false;
        }

        db.Remove(category);

        db.SaveChanges();

        return true;
    }
}
