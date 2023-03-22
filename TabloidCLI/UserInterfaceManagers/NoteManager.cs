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

        public NoteManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _noteRepository = new NoteRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Note Menu");
            Console.WriteLine(" 1) List all Notes");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice) 
            {
                case "1":
                    List();
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
    }

}