using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace NoteTakingApp.MVVM.Models
{
    class Database
    {
        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); 
            }
            else
            {
                return (T)obj;
            }
        }

        static String DBPATH = "./MVVM/Models/Database.accdb";

        internal static int AddNewNote(Note note)
        {
            using (OleDbConnection connection = new OleDbConnection(string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}", DBPATH)))
            {
                using (OleDbCommand insertCommand = new OleDbCommand("INSERT INTO NOTES ([NOTENAME]) VALUES (?)", connection))
                {
                    connection.Open();

                    insertCommand.Parameters.AddWithValue("@NOTENAME", note.Name);

                    insertCommand.ExecuteNonQuery();
                }

                using (OleDbCommand selectCommand = new OleDbCommand("SELECT * FROM NOTES WHERE [NOTENAME] = ?", connection))
                {
                    DataTable table = new DataTable();
                    OleDbDataAdapter adapter = new OleDbDataAdapter();
                    selectCommand.Parameters.AddWithValue("@NOTENAME", note.Name);
                    adapter.SelectCommand = selectCommand;
                    adapter.Fill(table);

                    DataRow row = table.Rows[0];
                    return (int)row["ID"];
                }
            }
        }

        internal static void RemoveNote(Note note)
        {
            using (OleDbConnection connection = new OleDbConnection(string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}", DBPATH)))
            {
                using (OleDbCommand deleteCommand = new OleDbCommand("DELETE FROM NOTES WHERE [ID] = ?", connection))
                {
                    connection.Open();

                    deleteCommand.Parameters.AddWithValue("@ID", note.Id);

                    deleteCommand.ExecuteNonQuery();
                }
            }
        }

        internal static ObservableCollection<Note> GetAllNotes()
        {
            ObservableCollection<Note> notes = new ObservableCollection<Note>();
            using (OleDbConnection connection = new OleDbConnection(string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}", DBPATH)))
            {
                using (OleDbCommand selectCommand = new OleDbCommand("SELECT * FROM NOTES", connection))
                {
                    connection.Open();

                    DataTable table = new DataTable();
                    OleDbDataAdapter adapter = new OleDbDataAdapter();
                    adapter.SelectCommand = selectCommand;
                    adapter.Fill(table);

                    foreach (DataRow row in table.Rows)
                    {
                        Note N = new Note();
                        N.Id = (int)row["ID"];
                        N.Name = ConvertFromDBVal<String>(row["NOTENAME"]);
                        N.Text = ConvertFromDBVal<String>(row["NOTETEXT"]);
                        N.DateCreated = (DateTime)row["DATECREATED"];
                        N.DateUpdated = (DateTime)row["DATEUPDATED"];
                        notes.Add(N);
                    }
                    return notes;
                }
            }
        }

        internal static void UpdateNote(Note note)
        {
            using (OleDbConnection connection = new OleDbConnection(string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}", DBPATH)))
            {
                using (OleDbCommand updateCommand = new OleDbCommand("UPDATE NOTES SET [NOTENAME] = ?, [NOTETEXT] = ?, DATEUPDATED = @DATEUPDATED WHERE [ID] = ?", connection))
                {
                    connection.Open();
                    updateCommand.Parameters.AddWithValue("@NOTENAME", note.Name);
                    updateCommand.Parameters.AddWithValue("@NOTETEXT", note.Text);
                    updateCommand.Parameters.AddWithValue("@DATEUPDATED", DbType.DateTime).Value = note.DateUpdated.ToString();
                    updateCommand.Parameters.AddWithValue("@ID", note.Id);

                    updateCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
