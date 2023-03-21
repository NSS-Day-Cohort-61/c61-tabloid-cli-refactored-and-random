//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TabloidCLI.Models;

//namespace TabloidCLI.UserInterfaceManagers
//{

//    internal class ColorManager : IUserInterfaceManager
//    {
//        private readonly IUserInterfaceManager _parentUI;
       

//        public ColorManager(IUserInterfaceManager parentUI)
//        {


//            _parentUI = parentUI;
           
//        }
//            public IUserInterfaceManager Execute()
//        {
//            Console.WriteLine("Author Menu");
//            Console.WriteLine(" 1) Purple");
//            Console.WriteLine(" 2) Red");
//            Console.WriteLine(" 3) Add Author");
//            Console.WriteLine(" 4) Edit Author");
//            Console.WriteLine(" 5) Remove Author");
//            Console.WriteLine(" 0) Go Back");

//            Console.Write("> ");
//            string choice = Console.ReadLine();
//            switch (choice)
//            {
//                case "1":
//                    List();
//                    return this;
//                case "2":
//                    Author author = Choose();
//                    if (author == null)
//                    {
//                        return this;
//                    }
//                    else
//                    {
//                        return new AuthorDetailManager(this, _connectionString, author.Id);
//                    }
//                case "3":
//                    Add();
//                    return this;
//                case "4":
//                    Edit();
//                    return this;
//                case "5":
//                    Remove();
//                    return this;
//                case "0":
//                    return _parentUI;
//                default:
//                    Console.WriteLine("Invalid Selection");
//                    return this;
//            }
//        }
//    }
//    }
//}
