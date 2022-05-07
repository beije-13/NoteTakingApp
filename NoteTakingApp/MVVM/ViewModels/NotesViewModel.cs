using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NoteTakingApp.MVVM.Models;

namespace NoteTakingApp.MVVM.ViewModels
{
    class NotesViewModel
    {
        public Note SelectedNote { get; set; } = new Note();
        public NoteModel MyNoteModel { get; set; } = new NoteModel();

        public ICommand AddCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        public NotesViewModel()
        {
            AddCommand = new RelayCommand(
                e =>
                {
                    MyNoteModel.Add(new Note { Name = "New Note "+DateTime.Now.ToString(), DateCreated = DateTime.Now, DateUpdated = DateTime.Now});
                }, c => true
                );

            RemoveCommand = new RelayCommand(
                e =>
                {
                    MyNoteModel.Remove(SelectedNote);
                }, c => SelectedNote != null
                );


            UpdateCommand = new RelayCommand(
                e =>
                {
                    MyNoteModel.Update(SelectedNote);
                }, c => SelectedNote != null
                );
        }
    }
}
