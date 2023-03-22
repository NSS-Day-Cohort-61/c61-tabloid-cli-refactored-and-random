using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI
{
    public class NoteRepository : DatabaseConnector, IRepository<Note>
    {
        public NoteRepository(string connectionString) : base(connectionString) { }

        public void Delete(int id)
        {
            using(SqlConnection conn = Connection);
            {
                Connection.Open();
                using (SqlCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Note WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Note Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Note> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id,
                                               Title,
                                               Content,
                                               CreateDateTime
                                          FROM Note";

                    List<Note> notes = new List<Note>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Note note = new Note()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                        };
                        notes.Add(note);
                    }

                    reader.Close();

                    return notes;
                }
            }
        }

        public void Insert(Note note)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                { 
                    cmd.CommandText = @"INSERT INTO Note (Title, Content,CreateDateTime, PostId)
                                        VALUES (@Title, @Content , @CreateDateTime, @PostId)";
                    cmd.Parameters.AddWithValue("@Title", note.Title);
                    cmd.Parameters.AddWithValue("@Content", note.Content);
                    cmd.Parameters.AddWithValue("@CreateDateTime", note.CreateDateTime);
                    cmd.Parameters.AddWithValue("@PostId", note.Post);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Note entry)
        {
            throw new NotImplementedException();
        }
    }
}