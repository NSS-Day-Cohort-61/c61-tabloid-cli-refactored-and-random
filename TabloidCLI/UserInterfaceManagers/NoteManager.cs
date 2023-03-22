using System;
using System.Collections.Generic;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class NoteManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private NoteRepository _noteRepository;
        private string _connectionString;
        private int _PostIdSelected;

        public NoteManager(IUserInterfaceManager parentUI, string connectionString, int PostId)
        {
            _parentUI = parentUI;
            _noteRepository = new NoteRepository(connectionString);
            _connectionString = connectionString;
            _PostIdSelected = PostId;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Note Menu");
            Console.WriteLine(" 1) List all Notes");
            Console.WriteLine(" 2) Add a Note");
            Console.WriteLine(" 3) Delete a Note");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice) 
            {
                case "1":
                    List();
                    return this;
                case "2":
                    AddNote();
                    return this;
                    case "3":
                    Remove(); 
                    return this;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void  List()
        {
            List<Note> notes = _noteRepository.GetAll();
            foreach (Note note in notes)
            {
                Console.WriteLine(note);
            }
        }

        private void  AddNote()
        {
            Console.WriteLine("Enter the Title: ");
            string Title = Console.ReadLine();

            Console.WriteLine("Enter your note: ");
            string Content = Console.ReadLine();

            Note newNote = new Note()
            {
                Title = Title,
                Content = Content,
                CreateDateTime = DateTime.Now,
                Post = _PostIdSelected
            };

            _noteRepository.Insert(newNote);
        }

        private void Remove()
        {
            foreach (Note note in _noteRepository.GetAll())
            Console.WriteLine($"{note.Id}) Title: {note.Title}");
            Console.Write("Enter the note number you would you like to delete");
            int noteToDelete = int.Parse(Console.ReadLine());
            _noteRepository.Delete(noteToDelete);

        }
    }

}