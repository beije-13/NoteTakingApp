using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NoteTakingApp.MVVM.Models
{
    class Note : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string text;
        private string document;
        private DateTime dateCreated;
        private DateTime dateUpdated;

        public int Id
        { 
            get 
            {
                return id;
            }
            set 
            {
                id = value;
            } 
        }

        public string Document 
        { 
            get 
            {
                return document;
            } 
            set 
            {
                document = value;
                RaisePropertyChanged(nameof(Document));
            } 
        }


        public string Name 
        {
            get
            {
                if (name is null || name == "")
                {
                    return "New Note "+DateTime.Now.ToString();
                }
                else
                {
                    return name;
                }
            }
            set
            {
                name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string Text
        {
            get 
            {
                if (text is null)
                {
                    return "";
                }
                else 
                {
                    return text;
                }
            }
            set
            {
                text = value;
                RaisePropertyChanged(nameof(Text));
            }
        }

        public DateTime DateCreated
        {
            get => dateCreated; 
            set
            {
                dateCreated = value;
                RaisePropertyChanged(nameof(dateCreated));
            }
        }

        public DateTime DateUpdated
        {
            get => dateUpdated; 
            set
            {
                dateUpdated = value;
                RaisePropertyChanged(nameof(dateUpdated));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
