using PhoneBook.Data;
using PhoneBook.UI;

using (var context = new PhoneBookContext())
{
    DbInitializer.SeedCategories(context);
}

Menu menu = new();
menu.Show();