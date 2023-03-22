using System;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class NoteManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private NoteRepository _noteRepository;
        private string _connectionString;
        private PostRepository _Post;
        private int _postId;


        public NoteManager(IUserInterfaceManager parentUI, string connectionString, int PostId)
        {
            _parentUI = parentUI;
            _noteRepository = new NoteRepository(connectionString);
            _connectionString = connectionString;
            _Post = new PostRepository(connectionString);
            _postId = PostId;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Note Menu");
            Console.WriteLine(" 1) List Notes");
            Console.WriteLine(" 2) Add a Note");
            Console.WriteLine(" 3) Remove a Note");
            Console.WriteLine(" 4) Return");

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
                case "4":
                    return _parentUI;
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
            Console.WriteLine("New Note-----");
            Note note = new Note();

            Console.WriteLine("Enter the Title: ");
            string Title = Console.ReadLine();

            Console.WriteLine("Enter your note: ");
            string Content = Console.ReadLine();

            Console.WriteLine("Select a Post");
            List<Post> posts = _Post.GetAll();
            foreach (Post p in posts)
            {
                Console.WriteLine($"{p.Id} - {p.Title}");
            }
            Post post = new Post
            {
                Id = int.Parse(Console.ReadLine()),
            };
            note.Post = post;

            Note newNote = new Note()
            {
                Title = Title,
                Content = Content,
                CreateDateTime = DateTime.Now,
                Post = post
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