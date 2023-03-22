using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;

namespace TabloidCLI.Repositories
{
    public class PostRepository : DatabaseConnector, IRepository<Post>
    {
        public PostRepository(string connectionString) : base(connectionString) { }

        public List<Post> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id,
                                               Title,
                                               Url,
                                               PublishDateTime,
                                               AuthorId, 
                                               BlogId
                                                
                                          FROM Post";

                    List<Post> posts = new List<Post>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Post post = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Url = reader.GetString(reader.GetOrdinal("Url")),
                            PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            Blog = new Blog
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                         
                            },
                            Author = new Author
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                
                            }
                        };
                        posts.Add(post);
                    }

                    reader.Close();

                    return posts;
                }
            }
        }

        public List<Post> GetByBlog(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.id AS postId,
                                               p.Title AS postTitle,
                                               p.Url AS postUrl,
                                               p.PublishDateTime,
                                               p.AuthorId AS authorId, 
                                               p.BlogId AS blogId,
                                                Blog.Title AS blogTitle,
                                                Blog.Url AS blogUrl,
                                                Author.FirstName,
                                                 Author.LastName,
                                                Author.Bio
                                          FROM Post p
                                        LEFT JOIN Blog on Blog.Id = p.BlogId
                                        LEFT JOIN Author on Author.Id = p.AuthorId
                                            WHERE p.BlogId = @BlogId";

                    cmd.Parameters.AddWithValue("@BlogId", id);

                    List<Post> postsByBlog = new List<Post>();
                

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                            Post post = new Post()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("postId")),
                                Title = reader.GetString(reader.GetOrdinal("postTitle")),
                                Url = reader.GetString(reader.GetOrdinal("postUrl")),
                                PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                                Blog = new Blog()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("blogId")),
                                    Title = reader.GetString(reader.GetOrdinal("blogTitle")),
                                    Url = reader.GetString(reader.GetOrdinal("blogUrl"))
                                },
                                Author = new Author()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("authorId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Bio = reader.GetString(reader.GetOrdinal("Bio"))
                                }
                            };
                        
                        postsByBlog.Add(post);
                    }

                    reader.Close();

                    return postsByBlog;
                }
            }
        }

        public List<Post> GetByAuthor(int authorId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.id,
                                               p.Title As PostTitle,
                                               p.URL AS PostUrl,
                                               p.PublishDateTime,
                                               p.AuthorId,
                                               p.BlogId,
                                               a.FirstName,
                                               a.LastName,
                                               a.Bio,
                                               b.Title AS BlogTitle,
                                               b.URL AS BlogUrl
                                          FROM Post p 
                                               LEFT JOIN Author a on p.AuthorId = a.Id
                                               LEFT JOIN Blog b on p.BlogId = b.Id 
                                         WHERE p.AuthorId = @authorId";
                    cmd.Parameters.AddWithValue("@authorId", authorId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Post> posts = new List<Post>();
                    while (reader.Read())
                    {
                        Post post = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("PostTitle")),
                            Url = reader.GetString(reader.GetOrdinal("PostUrl")),
                            PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            Author = new Author()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Bio = reader.GetString(reader.GetOrdinal("Bio")),
                            },
                            Blog = new Blog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                                Title = reader.GetString(reader.GetOrdinal("BlogTitle")),
                                Url = reader.GetString(reader.GetOrdinal("BlogUrl")),
                            }
                        };
                        posts.Add(post);
                    }

                    reader.Close();

                    return posts;
                }
            }
        }

        public void Insert(Post post)

        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Post (Title, Url, PublishDateTime, AuthorId, BlogId )
                                                     VALUES (@Title, @Url, @PublishDateTime, @AuthorId, @BlogId)";
                    cmd.Parameters.AddWithValue("@Title", post.Title);
                    cmd.Parameters.AddWithValue("@Url", post.Url);
                    cmd.Parameters.AddWithValue("@AuthorId", post.Author.Id);
                    cmd.Parameters.AddWithValue("@BlogId", post.Blog.Id);
                    cmd.Parameters.AddWithValue("@PublishDateTime", post.PublishDateTime);


                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Post post)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Post
                                           SET Title = @title,
                                               Url = @Url,
                                               PublishDateTime = @publishdatetime,
                                               AuthorId = @authorId,
                                               BlogId = @blogId
                                         WHERE id = @id";

                    cmd.Parameters.AddWithValue("@title", post.Title);
                    cmd.Parameters.AddWithValue("@Url", post.Url);
                    cmd.Parameters.AddWithValue("@publishdatetime", post.PublishDateTime);
                    cmd.Parameters.AddWithValue("@authorId", post.Author.Id);
                    cmd.Parameters.AddWithValue("@blogId", post.Blog.Id);
                    cmd.Parameters.AddWithValue("@id", post.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Post WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Post Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.id,
                                               p.Title AS postTitle,
                                               p.Url AS postUrl,
                                               p.PublishDateTime,
                                               p.AuthorId AS authorId, 
                                               p.BlogId AS blogId,
                                                Blog.Title AS blogTitle,
                                                Blog.Url AS blogUrl,
                                                Author.FirstName,
                                                 Author.LastName,
                                                Author.Bio
                                          FROM Post p
                                        LEFT JOIN Blog on Blog.Id = p.BlogId
                                        LEFT JOIN Author on Author.Id = p.AuthorId
                                            WHERE p.id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    Post post = null;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        if (post == null)
                        {
                            post = new Post
                            {
                                Id = id,
                                Title = reader.GetString(reader.GetOrdinal("postTitle")),
                                Url = reader.GetString(reader.GetOrdinal("postUrl")),
                                PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                                Blog = new Blog
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("blogId")),
                                    Title = reader.GetString(reader.GetOrdinal("blogTitle")),
                                    Url = reader.GetString(reader.GetOrdinal("blogUrl"))
                                },
                                Author = new Author
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("authorId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Bio = reader.GetString(reader.GetOrdinal("Bio"))
                                }
                            };
                        }
                    }

                    reader.Close();

                    return post;
                }
            }
        }
    }
}
