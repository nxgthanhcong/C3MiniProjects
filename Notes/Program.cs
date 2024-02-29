
using Notes;
using System.Text.Json;

Dictionary<int, string> menu = new Dictionary<int, string>
{
    { 1, "Show list note" },
    { 2, "Add new note" },
    { 3, "Update a note" },
    { 4, "Delete a note" },
};

int selectedMenu = 0;
List<Note> notes = GetFromTxt();

do
{
    Console.WriteLine("Here is our menu: ");
    foreach (var item in menu)
    {
        Console.WriteLine($"{item.Key}: {item.Value}");
    }

    Console.Write("Select your action (0 to exit): ");
    int.TryParse(Console.ReadLine(), out selectedMenu);

    switch (selectedMenu)
    {
        case 1:
            ShowListNote(notes);
            break;
        case 2:
            AddNewNote(notes);
            break;
        case 3:
            UpdateNote(notes);
            break;
        case 4:
            DeleteNote(notes);
            break;
        default:
            SaveToTxtFile(notes);
            Console.WriteLine("Invalid selection. Please try again.");
            break;
    }

    Console.WriteLine("\n");

} while (selectedMenu != 0);

static void SaveToTxtFile(List<Note> notes)
{
    string fileName = "dbtemp.txt";
    File.WriteAllText(fileName, JsonSerializer.Serialize(notes));
}

static List<Note> GetFromTxt()
{
    string fileName = "dbtemp.txt";
    if (!File.Exists(fileName))
    {
        return new List<Note>();
    }

    return JsonSerializer.Deserialize<List<Note>>(File.ReadAllText(fileName));
}

static void ShowListNote(List<Note> notes)
{
    foreach (var note in notes)
    {
        Console.WriteLine($"Id: {note.Id}, Title: {note.Title}, UpdateDate: {note.UpdateDate}");
    }
}

static void AddNewNote(List<Note> notes)
{
    Console.WriteLine("Enter new note: ");
    string newNote = Console.ReadLine();
    notes.Add(new Note
    {
        Id = notes.Count > 0 ? (notes.Max(x => x.Id) + 1) : 1,
        Title = newNote,
        UpdateDate = DateTime.Now,
    });
}

static void UpdateNote(List<Note> notes)
{
    Console.WriteLine("Enter noteId: ");
    int updateNoteId = int.Parse(Console.ReadLine());

    Console.WriteLine("Enter new title: ");
    string updateNoteTitle = Console.ReadLine();

    Note updateNote = notes.FirstOrDefault(s => s.Id == updateNoteId);
    if (updateNote != null)
    {
        updateNote.Title = updateNoteTitle;
        updateNote.UpdateDate = DateTime.Now;
    }
    else
    {
        Console.WriteLine("Note not found.");
    }
}

static void DeleteNote(List<Note> notes)
{
    Console.WriteLine("Enter noteId: ");
    int updateNoteId = int.Parse(Console.ReadLine());

    Note noteToDelete = notes.FirstOrDefault(s => s.Id == updateNoteId);
    if (noteToDelete != null)
    {
        notes.Remove(noteToDelete);
    }
    else
    {
        Console.WriteLine("Note not found.");
    }
}