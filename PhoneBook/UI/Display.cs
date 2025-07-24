using PhoneBook.Models;
using PhoneBook.Services;
using Spectre.Console;

namespace PhoneBook.UI;

public static class Display
{
    public static void ShowContacts(List<Contact> contacts)
    {
        var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("[yellow]Name[/]");
            table.AddColumn("[blue]Email[/]");
            table.AddColumn("[green]Phone Number[/]");
            table.AddColumn("[cyan]Category[/]");

        var categoryService = new CategoryService();

        foreach (var contact in contacts)
        {
            table.AddRow(
                contact.Name.ToString(),
                contact.Email.ToString(),
                contact.PhoneNumber.ToString(),
                categoryService.GetCategoryById(contact.CategoryId).Name.ToString()
                );
        }

        AnsiConsole.Write(table);
    }

    public static void ShowContact(Contact contact)
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("[yellow]Name[/]");
        table.AddColumn("[blue]Email[/]");
        table.AddColumn("[green]Phone Number[/]");
        table.AddColumn("[cyan]Category[/]");

        var categoryService = new CategoryService();

        table.AddRow(
            contact.Name.ToString(),
            contact.Email.ToString(),
            contact.PhoneNumber.ToString(),
            categoryService.GetCategoryById(contact.CategoryId).Name.ToString()
            );

        AnsiConsole.Write(table);
    }

    public static Category SelectCategory(CategoryService categoryService)
    {
        var categories = categoryService.GetAllCategories();

        if (!categories.Any())
        {
            AnsiConsole.MarkupLine("[red]No categories available.[/]");
            return null;
        }

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<Category>()
            .Title("[blue]Select a category[/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more categories)[/]")
            .UseConverter(cat => cat.Name)
            .AddChoices(categories)
            );

        return option;    
    }

    public static void ShowCategories(List<Category> categories)
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("[yellow]Category[/]");

        foreach (var category in categories)
        {
            table.AddRow(
                category.Name.ToString()
                );
        }

        AnsiConsole.Write(table);
    }

    public static void ShowCategory(Category category)
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("[yellow]Category[/]");

        table.AddRow(
            category.Name.ToString()
            );

        AnsiConsole.Write(table);
    }
}
