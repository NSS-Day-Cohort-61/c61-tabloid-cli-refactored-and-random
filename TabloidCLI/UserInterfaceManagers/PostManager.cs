using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabloidCLI.Models;
using TabloidCLI.Repositories;


namespace TabloidCLI.UserInterfaceManagers
{
    internal class PostManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private string _connectionString;
        private AuthorRepository _authorRepository;
        private BlogRepository _blogRepository;

        public PostManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _connectionString = connectionString;
            _authorRepository = new AuthorRepository(connectionString);
            _blogRepository = new BlogRepository(connectionString);
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Post Management Menu");
            Console.WriteLine(" 1) List Posts");
            Console.WriteLine(" 2) Add Post");
            Console.WriteLine(" 3) Edit Post");
            Console.WriteLine(" 4) Remove Post");
            Console.WriteLine(" 5) View Post Details");
            Console.WriteLine(" 0) Return to Main Menu");

            Console.Write("> ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Add();
                    return this;
                case "3":
                    Edit();
                    return this;
                case "4":
                    Remove();
                    return this;
                case "5":
                    Post post = Choose();
                    if (post == null)
                    {
                        return this;
                    }
                    else
                    {
                        return new PostDetailManager(this, _connectionString, post.Id);
                    }
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }


        }
        private void List()
        {
            List<Post> posts = _postRepository.GetAll();
            foreach (Post post in posts)
            {
                Console.WriteLine($"{post.Id}");
                Console.WriteLine($"Title: {post.Title}");
                Console.WriteLine($"Url: {post.Url}");
            }

        }

        private Post Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Post:";
            }

            Console.WriteLine(prompt);

            List<Post> posts = _postRepository.GetAll();

            for (int i = 0; i < posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine($" {i + 1}) {post.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return posts[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        private void Add()
        {
            Console.WriteLine("New Post");
            Post post = new Post();

            Console.Write("Title: ");
            post.Title = Console.ReadLine();

            Console.Write("Url: ");
            post.Url = Console.ReadLine();


            post.PublishDateTime = DateTime.Now;

            Console.WriteLine("Select an author");
            List<Author> authors = _authorRepository.GetAll();
            foreach (Author a in authors)
            {
                Console.WriteLine($"{a.Id} - {a.FirstName} {a.LastName}");
            }
            Author author = new Author
            {
                Id = int.Parse(Console.ReadLine()),
            };
            post.Author = author;

            Console.WriteLine("Select a blog");
            List<Blog> blogs = _blogRepository.GetAll();
            foreach (Blog b in blogs)
            {
                Console.WriteLine($"{b.Id} - {b.Title}");
            }
            Blog blog = new Blog
            {
                Id = int.Parse(Console.ReadLine()),
            };
            post.Blog = blog;

            _postRepository.Insert(post);
        }

        private void Remove()
        {
            Post postToDelete = Choose("Which post would you like to remove?");
            if (postToDelete != null)
            {
                _postRepository.Delete(postToDelete.Id);
            }
        }

        private void Edit()
        {
            Post postToEdit = Choose("Which post would you like to edit?");
            if (postToEdit == null)
            {
                return;
            }
          
            Console.WriteLine();
            Console.Write("New title (blank to leave unchanged: ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                postToEdit.Title = title;
            }
            Console.Write("New url (blank to leave unchanged: ");
            string Url = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(Url))
            {
                postToEdit.Url = Url;
            }
            Console.WriteLine(postToEdit.Author.ToString());
            Console.WriteLine("New author (blank to leave unchanged: ");
            List<Author> authors = _authorRepository.GetAll();
            foreach (Author a in authors)
            {
                Console.WriteLine($"{a.Id} - {a.FirstName} {a.LastName}");
            }
            string authorId = (Console.ReadLine());
            if (!string.IsNullOrWhiteSpace(authorId))
            {
                {
                    Author author = new Author
                    {
                        Id = int.Parse(authorId),
                    };
                    postToEdit.Author = author;
                }
            }
            Console.WriteLine("New blog (blank to leave unchanged: ");
            List<Blog> blogs = _blogRepository.GetAll();
            foreach (Blog b in blogs)
            {
                Console.WriteLine($"{b.Id} - {b.Title}");
            }

            string blogId = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(blogId))
            {
                {
                    Blog blog = new Blog
                    {
                        Id = int.Parse(blogId),
                    };
                    postToEdit.Blog = blog;
                }
            }


            _postRepository.Update(postToEdit);
        }


    }
}
