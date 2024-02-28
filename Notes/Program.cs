
using Notes;

Dictionary<int, string> menu = new Dictionary<int, string>
{
    { 1, "Show list note" },
    { 2, "Add new note" },
    { 3, "Update a note" },
    { 4, "Delete a note" },
};

int selectedMenu = 0;
List<Note> notes = new List<Note>();

do
{
    Console.WriteLine("Here is our menu: ");
    foreach (var item in menu)
    {
        Console.WriteLine(item);
    }
    Console.Write("select your action, (0 to exist): ");
    int.TryParse(Console.ReadLine(), out selectedMenu);

    if(selectedMenu == 1)
    {
        for(int i = 0; i < notes.Count; i++)
        {
            Console.WriteLine($"Id: {notes[i].Id}, Title: {notes[i].Title}, UpdateDate: {notes[i].UpdateDate}");
        }
    }

    if (selectedMenu == 2)
    {
        Console.WriteLine("Enter new note: ");
        string newNote = Console.ReadLine();
        notes.Add(new Note
        {
            Id = notes.Count > 0 ? notes.Max(x => x.Id) : 0,
            Title = newNote,
            UpdateDate = DateTime.Now,
        });
    }

    Console.WriteLine();
    Console.WriteLine();

} while (selectedMenu != 0);