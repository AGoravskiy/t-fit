using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace t_fit.Models
{
    public class User : INotifyPropertyChanged
    {
        private int rank;
        private string user;
        private string status;
        private int steps;
        private int day;

        public int Rank
        {
            get { return rank; }
            set 
            {
                rank = value;
                OnPropertyChanged(nameof(Rank));
            }
        }

        [JsonProperty("User")]
        public string Name
        {
            get { return user; }
            set 
            {
                user = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Status
        {
            get { return status; }
            set 
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public int Steps
        {
            get { return steps; }
            set 
            {
                steps = value;
                OnPropertyChanged(nameof(Steps));
            }
        }

        [JsonIgnore]
        public int Day
        {
            get { return day; }
            set 
            { 
                day = value;
                OnPropertyChanged(nameof(Day));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
