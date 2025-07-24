using PhoneBook.Data;
using PhoneBook.Models;

namespace PhoneBook.Services;

public class ContactService
{
    private readonly PhoneBookContext db = new();

    public bool AddContact(Contact contact)
    {
        if (contact == null || string.IsNullOrWhiteSpace(contact.Name))
            return false;

        var exists = db.Contacts.Any(c => c.Name.ToLower() == contact.Name.ToLower());
        if (exists)
            return false;

        db.Contacts.Add(contact);
        db.SaveChanges();
        return true;
    }

    public List<Contact> GetAllContacts()
    {
        return db.Contacts
            .OrderBy(c => c.Id)
            .ToList();
    }

    public Contact? GetContactByName(string name)
    {
        return db.Contacts
            .FirstOrDefault(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    public bool ModifyContact(int id, string name, string email, string phoneNumber, int? categoryId)
    {
        var contact = db.Contacts.FirstOrDefault(c => c.Id == id);

        if (contact == null)
        {
            Console.WriteLine($"No contact found wih ID {id}");
            return false;
        }

        contact.Name = name;
        contact.Email = email;
        contact.PhoneNumber = phoneNumber;
        contact.CategoryId = categoryId;

        db.SaveChanges();

        return true;
    }

    public bool DeleteContact(int id)
    {
        var contact = db.Contacts.FirstOrDefault(c => c.Id == id);

        if (contact == null)
        {
            Console.WriteLine($"No contact found wih ID {id}");
            return false;
        }

        db.Remove(contact);

        db.SaveChanges();

        return true;
    }
}
