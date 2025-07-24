using PhoneBook.Services;
using Spectre.Console;

namespace PhoneBook.UI;

public static class CategoriesMenu
{
    public static ContactService ContactService { get; set; } = new();
    public static CategoryService CategoryService { get; set; } = new();

    public static void Show()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[blue]Categores Menu[/]")
                .AddChoices(new[]
                {
                    "Add Category", "View Categories", "Edit Category","Delete Category", "Back"
                }));

            switch (option)
            {
                case "Add Category":

                    AddCategory();

                    break;

                case "View Categories":

                    ViewCategories();

                    break;

                case "Edit Category":

                    EditCategory();

                    break;

                case "Delete Category":

                    DeleteCategory();

                    break;

                case "Back":

                    exit = true;

                    break;
            }
        }
    }

    private static void AddCategory()
    {
        while (true)
        {
            AnsiConsole.MarkupLine("[green]Add Category Menu [red](Type 'zzz' to return to menu)[/][/]\n");

            AnsiConsole.MarkupLine("[green]Name:[/]");
            var name = Console.ReadLine();
            if (name.ToLower() == "zzz") break;
            if (string.IsNullOrEmpty(name)) continue;

            var category = new Models.Category
            {
                Name = name
            };

            if (CategoryService.AddCategory(category))
            {
                AnsiConsole.MarkupLine("\n[green]Category succesfully added![/]");
                AnsiConsole.MarkupLine("[grey]Press any key to return to menu...[/]");
                Console.ReadKey();
                break;
            }
            else
            {
                AnsiConsole.MarkupLine("\n[red]Category cannot be added![/]");
            }
        }
    }
    private static void ViewCategories()
    {
        var exit = false;
        while (!exit)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[green]View Category Menu[/]\n");

            var submenu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .AddChoices(new[]
                {
                    "View all Categories", "Search Category", "Back"
                }));

            switch (submenu)
            {
                case "View all Categories":

                    var categories = CategoryService.GetAllCategories();

                    if (categories != null)
                    {
                        Display.ShowCategories(categories);
                        AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
                        Console.ReadKey();
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Categories not found.[/]");
                        AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
                        Console.ReadKey();
                    }

                    break;

                case "Search Category":

                    AnsiConsole.MarkupLine("[green]Type the category's [yellow]name[/] [red](Type 'zzz' to return to menu)[/]:[/]");
                    var name = Console.ReadLine();
                    if (name.ToLower() == "zzz") break;
                    if (string.IsNullOrEmpty(name)) continue;

                    var category = CategoryService.GetCategoryByName(name);

                    if (category != null)
                    {
                        Display.ShowCategory(category);
                        AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
                        Console.ReadKey();
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Category not found.[/]");
                        AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
                        Console.ReadKey();
                    }

                    break;

                case "Back":

                    exit = true;

                    break;
            }
        }
    }
    private static void EditCategory()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[green]Edit Category Menu [red](Type 'zzz' to return to menu)[/][/]\n");

            AnsiConsole.MarkupLine("[green]Name:[/]");
            var name = Console.ReadLine();
            if (name.ToLower() == "zzz") break;
            if (string.IsNullOrEmpty(name)) continue;

            var category = CategoryService.GetCategoryByName(name);

            if (category == null)
            {
                AnsiConsole.MarkupLine("[red]Category not found.[/]");
                AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
                Console.ReadKey();
                continue;
            }

            AnsiConsole.MarkupLine("\n[green]You've selected this category:[/]");

            Display.ShowCategory(category);

            AnsiConsole.MarkupLine("\n[bold]Do you want to edit this category? (y/n)[/]");
            var input = Console.ReadLine();
            if (input.ToLower() != "y") break;

            AnsiConsole.MarkupLine("[green]\nName (press Enter to keep current):[/]");
            var newName = Console.ReadLine();
            if (newName.ToLower() == "zzz") break;
            if (newName == "") newName = category.Name;
            if (string.IsNullOrEmpty(newName)) continue;

            if (CategoryService.ModifyCategory(name, newName))
            {
                AnsiConsole.MarkupLine("\n[green]Category succesfully modified![/]");
                AnsiConsole.MarkupLine("[grey]Press any key to return to menu...[/]");
                Console.ReadKey();
                break;
            }
            else
            {
                AnsiConsole.MarkupLine("\n[red]Category cannot be modified![/]");
                AnsiConsole.MarkupLine("[grey]Press any key to return to menu...[/]");
                Console.ReadKey();
                break;
            }
        }
    }
    private static void DeleteCategory()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[green]Delete Category Menu [red](Type 'zzz' to return to menu)[/][/]\n");

            AnsiConsole.MarkupLine("[green]Name:[/]");
            var name = Console.ReadLine();
            if (name.ToLower() == "zzz") break;
            if (string.IsNullOrEmpty(name)) continue;

            var category = CategoryService.GetCategoryByName(name);

            if (category == null)
            {
                AnsiConsole.MarkupLine("[red]Category not found.[/]");
                AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
                Console.ReadKey();
                continue;
            }

            AnsiConsole.MarkupLine("\n[green]You've selected this category:[/]");

            Display.ShowCategory(category);

            AnsiConsole.MarkupLine("\n[bold]Are you sure you want to remove this category? (y/n)[/]");
            var input = Console.ReadLine();
            if (input.ToLower() != "y")
            {
                AnsiConsole.MarkupLine("[yellow]Deletion cancelled.[/]");
                Console.ReadKey();
                continue;
            }

            if (category.Contacts.Any())
            {
                AnsiConsole.MarkupLine("[red]Cannot delete category with assigned contacts.[/]");
                Console.ReadKey();
                continue;
            }

            if (CategoryService.DeleteCategory(category.Id))
            {
                AnsiConsole.MarkupLine("\n[green]Category succesfully removed![/]");
                AnsiConsole.MarkupLine("[grey]Press any key to return to menu...[/]");
                Console.ReadKey();
                break;
            }
            else
            {
                AnsiConsole.MarkupLine("\n[red]Category cannot be removed![/]");
                AnsiConsole.MarkupLine("[grey]Press any key to return to menu...[/]");
                Console.ReadKey();
                break;
            }
        }
    }
}
