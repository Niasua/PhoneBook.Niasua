using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PhoneBook.Services;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;

namespace PhoneBook.UI;

public class Menu
{
    public static ContactService ContactService { get; set; } = new();
    public static CategoryService CategoryService { get; set; } = new();

    public void Show()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[blue]Phone Book Menu[/]")
                .AddChoices(new[]
                {
                    "Add Contact", "View Contacts", "Edit Contact","Delete Contact", "Categories", "Exit"
                }));

            switch (option)
            {
                case "Add Contact":

                    AddContact();

                    break;

                case "View Contacts":

                    ViewContacts();

                    break;

                case "Edit Contact":

                    EditContact();

                    break;

                case "Delete Contact":

                    DeleteContact();

                    break;

                case "Categories":

                    CategoriesMenu.Show();

                    break;

                case "Exit":

                    exit = true;

                    break;
            }
        }
    }

    private static void AddContact()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[green]Add Contact Menu [red](Type 'zzz' to return to menu)[/][/]\n");

            AnsiConsole.MarkupLine("[green]Name:[/]");
            var name = Console.ReadLine();
            if (name.ToLower() == "zzz") break;
            if (string.IsNullOrEmpty(name)) continue;

            AnsiConsole.MarkupLine("\n[green]Email:[/]");
            var email = Console.ReadLine();
            if (email.ToLower() == "zzz") break;
            if (!Validator.Validator.IsValidEmail(email))
            {
                AnsiConsole.MarkupLine("[red]Invalid email format![/]");
                continue;
            }

            AnsiConsole.MarkupLine("\n[green]Phone Number:[/]");
            var pNumber = Console.ReadLine();
            if (pNumber.ToLower() == "zzz") break;
            if (!Validator.Validator.IsValidPhone(pNumber))
            {
                AnsiConsole.MarkupLine("[red]Invalid phone number! Use only digits, dashes, or spaces.[/]");
                continue;
            }

            AnsiConsole.MarkupLine("\n[green]Category:[/]");
            var category = Display.SelectCategory(CategoryService);

            var contact = new Models.Contact
            {
                Name = name,
                Email = email,
                PhoneNumber = pNumber,
                CategoryId = category.Id
            };

            if (ContactService.AddContact(contact))
            {
                AnsiConsole.MarkupLine("\n[green]Contact succesfully added![/]");
                AnsiConsole.MarkupLine("[grey]Press any key to return to menu...[/]");
                Console.ReadKey();
                break;   
            }
            else
            {
                AnsiConsole.MarkupLine("\n[red]Contact cannot be added![/]");
            }
        }
    }
    private static void ViewContacts()
    {
        var exit = false;
        while (!exit)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[green]View Contact Menu[/]\n");

            var submenu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .AddChoices(new[]
                {
                    "View all Contacts", "Search Contact", "Back"
                }));

            switch (submenu)
            {
                case "View all Contacts":

                    var contacts = ContactService.GetAllContacts();

                    if (contacts != null)
                    {
                        Display.ShowContacts(contacts);
                        AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
                        Console.ReadKey();
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Contacts not found.[/]");
                        AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
                        Console.ReadKey();
                    }

                    break;

                case "Search Contact":

                    AnsiConsole.MarkupLine("[green]Type your contact's [yellow]name[/] [red](Type 'zzz' to return to menu)[/]:[/]");
                    var name = Console.ReadLine();
                    if (name.ToLower() == "zzz") break;
                    if (string.IsNullOrEmpty(name)) continue;

                    var contact = ContactService.GetContactByName(name);

                    if (contact != null)
                    {
                        Display.ShowContact(contact);
                        AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
                        Console.ReadKey();
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Contact not found.[/]");
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
    private static void EditContact()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[green]Edit Contact Menu [red](Type 'zzz' to return to menu)[/][/]\n");

            AnsiConsole.MarkupLine("[green]Name:[/]");
            var name = Console.ReadLine();
            if (name.ToLower() == "zzz") break;
            if (string.IsNullOrEmpty(name)) continue;

            var contact = ContactService.GetContactByName(name);

            if (contact == null)
            {
                AnsiConsole.MarkupLine("[red]Contact not found.[/]");
                AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
                Console.ReadKey();
                continue;
            }

            AnsiConsole.MarkupLine("\n[green]You've selected this contact:[/]");

            Display.ShowContact(contact);

            AnsiConsole.MarkupLine("\n[bold]Do you want to edit this contact? (y/n)[/]");
            var input = Console.ReadLine();
            if (input.ToLower() != "y") break;

            AnsiConsole.MarkupLine("[green]\nName (press Enter to keep current):[/]");
            var newName = Console.ReadLine();
            if (newName.ToLower() == "zzz") break;
            if (newName == "") newName = contact.Name;
            if (string.IsNullOrEmpty(newName)) continue;

            AnsiConsole.MarkupLine("\n[green]Email (press Enter to keep current):[/]");
            var newEmail = Console.ReadLine();
            if (newEmail.ToLower() == "zzz") break;
            if (newEmail == "") newEmail = contact.Email;
            if (!Validator.Validator.IsValidEmail(newEmail))
            {
                AnsiConsole.MarkupLine("[red]Invalid email format![/]");
                continue;
            }

            AnsiConsole.MarkupLine("\n[green]Phone Number (press Enter to keep current):[/]");
            var newPNumber = Console.ReadLine();
            if (newPNumber.ToLower() == "zzz") break;
            if (newPNumber == "") newPNumber = contact.PhoneNumber;
            if (!Validator.Validator.IsValidPhone(newPNumber))
            {
                AnsiConsole.MarkupLine("[red]Invalid phone number! Use only digits, dashes, or spaces.[/]");
                continue;
            }

            AnsiConsole.MarkupLine("\n[green]Category:[/]");
            AnsiConsole.MarkupLine("\n[grey]Do you want to change category? (y/n):[/]");

            var changeCategory = Console.ReadLine();
            var categoryId = contact.CategoryId;

            if (changeCategory.ToLower() == "y")
            {
                categoryId = Display.SelectCategory(CategoryService).Id;
            }

            if (ContactService.ModifyContact(contact.Id, newName, newEmail, newPNumber, categoryId))
            {
                AnsiConsole.MarkupLine("\n[green]Contact succesfully modified![/]");
                AnsiConsole.MarkupLine("[grey]Press any key to return to menu...[/]");
                Console.ReadKey();
                break;
            }
            else
            {
                AnsiConsole.MarkupLine("\n[red]Contact cannot be modified![/]");
                AnsiConsole.MarkupLine("[grey]Press any key to return to menu...[/]");
                Console.ReadKey();
                break;
            }
        }
    }
    private static void DeleteContact()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[green]Delete Contact Menu [red](Type 'zzz' to return to menu)[/][/]\n");

            AnsiConsole.MarkupLine("[green]Name:[/]");
            var name = Console.ReadLine();
            if (name.ToLower() == "zzz") break;
            if (string.IsNullOrEmpty(name)) continue;

            var contact = ContactService.GetContactByName(name);

            if (contact == null)
            {
                AnsiConsole.MarkupLine("[red]Contact not found.[/]");
                AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
                Console.ReadKey();
                continue;
            }

            AnsiConsole.MarkupLine("\n[green]You've selected this contact:[/]");

            Display.ShowContact(contact);

            AnsiConsole.MarkupLine("\n[bold]Are you sure you want to remove this contact? (y/n)[/]");
            var input = Console.ReadLine();
            if (input.ToLower() != "y") break;

            if (ContactService.DeleteContact(contact.Id))
            {
                AnsiConsole.MarkupLine("\n[green]Contact succesfully removed![/]");
                AnsiConsole.MarkupLine("[grey]Press any key to return to menu...[/]");
                Console.ReadKey();
                break;
            }
            else
            {
                AnsiConsole.MarkupLine("\n[red]Contact cannot be removed![/]");
                AnsiConsole.MarkupLine("[grey]Press any key to return to menu...[/]");
                Console.ReadKey();
                break;
            }
        }
    }
}
