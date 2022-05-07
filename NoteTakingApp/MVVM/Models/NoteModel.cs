using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NoteTakingApp.MVVM.Models
{
    class NoteModel
    {
        public ObservableCollection<Note> Notes { get; private set; } = new ObservableCollection<Note>();
        public Database accessDB = new Database();

        public NoteModel()
        {
            Notes = Database.GetAllNotes();
        }

        public void Add(Note noteToAdd)
        {
            int id;
            if (noteToAdd == null)
                return;
            id = Database.AddNewNote(noteToAdd);
            noteToAdd.Id = id;
            Notes.Add(noteToAdd);
        }

        public void Remove(Note noteToRemove)
        {
            if (noteToRemove == null)
                return;
            Notes.Remove(noteToRemove);
            Database.RemoveNote(noteToRemove);
        }

        public void Update(Note noteToUpdate)
        {
            noteToUpdate.DateUpdated = DateTime.Now;
            Database.UpdateNote(noteToUpdate);
        }
    }
}
