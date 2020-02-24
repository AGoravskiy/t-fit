using LiveCharts;
using LiveCharts.Wpf;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Data;
using t_fit.Models;

namespace t_fit.ViewModels
{
    public class TFitViewModels : INotifyPropertyChanged
    {
        private static string status = "Finished";

        private StatsOfUser selectedUser;
        private IJsonService _jsonService;
        private List<DataPoint> points;
        private RelayCommand saveCommand;
        private RelayCommand showChartCommand;

        public ObservableCollection<User> Users { get; set; }
        public List<string> UniqueUserNames { get; set; }
        public ObservableCollection<StatsOfUser> StatsOfUsers { get; set; }
        public List<DataPoint> Points
        {
            get { return points; }
            set
            {
                points = value;
                OnPropertyChanged(nameof(List<DataPoint>));
            }
        }

        public StatsOfUser SelectedUser
        {
            get { return selectedUser; }
            set 
            {
                selectedUser = value;
                OnPropertyChanged(nameof(StatsOfUser));
            }
        }
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      if (selectedUser != null)
                      {
                          SaveSelectedUserStats();
                      }
                  }));
            }
        }

        public RelayCommand ShowChartCommand
        {
            get
            {
                return showChartCommand ??
                  (showChartCommand = new RelayCommand(obj =>
                  {
                      if (selectedUser != null)
                      {
                          points = FillChartPoints();
                          //update OxyPlot
                      }
                  }));
            }
        }
        public TFitViewModels() { }
        public TFitViewModels(IJsonService jsonService)
        {
            _jsonService = jsonService ?? throw new ArgumentNullException(nameof(jsonService));

            Users = Initialize();

            UniqueUserNames = new List<string>();

            UniqueUserNames = GetUniqueUserNames(Users);

            StatsOfUsers = new ObservableCollection<StatsOfUser>();

            StatsOfUsers = GetStatsOfUsers(UniqueUserNames);
        }

        public void SaveSelectedUserStats()
        {
            _jsonService.Serialize<StatsOfUser>("..\\..\\resources\\data_out\\" 
                + selectedUser.Name.Split()[0] 
                + DateTime.Now.ToString("yyyyMMddHHmmss") 
                + ".json", selectedUser);
        }

        public ObservableCollection<User> Initialize()
        {
            ObservableCollection<User> allDayUsers = new ObservableCollection<User>();

            for (int day = 1; day < 31; day++)
            {
                ObservableCollection<User> oneDayUsers =
                    new ObservableCollection<User>(
                        _jsonService.Deserialize<User>("..\\..\\resources\\data_in\\day" + day + ".json"));

                foreach (var user in oneDayUsers)
                {
                    user.Day = day;
                    allDayUsers.Add(user);
                }
            }
            return allDayUsers;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public List<string> GetUniqueUserNames(ObservableCollection<User> users)
        {
            return users.Select(u => u.Name).Distinct().ToList();
        }

        public ObservableCollection<StatsOfUser> GetStatsOfUsers(List<string> names)
        {
            var statsOfUsers = new ObservableCollection<StatsOfUser>();

            foreach (var name in names)
            {
                statsOfUsers.Add(new StatsOfUser 
                { 
                    Name = name, 
                    AverageSteps = CountAverageSteps(name),
                    MaxSteps = FindMaxSteps(name),
                    MinSteps = FindMinSteps(name),
                    Status = status,
                    IsResultStable = CheckIsResultStable(name)
                });
            }

            statsOfUsers = new ObservableCollection<StatsOfUser>(statsOfUsers
                .OrderByDescending(s => s.AverageSteps)
                .ToList());

            AddRank(statsOfUsers);

            return statsOfUsers;
        }

        public List<int> FindUserStepsByName(string name)
        {
            return Users
                .Where(u => u.Name == name)
                .Select(u => u.Steps)
                .ToList();
        }

        public int CountAverageSteps(string name)
        {
            return (int) FindUserStepsByName(name).Average();
        }

        public int FindMinSteps(string name)
        {
            return FindUserStepsByName(name).Min();
        }

        public int FindMaxSteps(string name)
        {
            return FindUserStepsByName(name).Max();
        }

        public void AddRank(ObservableCollection<StatsOfUser> statsOfUsers)
        {
            for (int i = 0; i < statsOfUsers.Count; i++)
            {
                statsOfUsers[i].Rank = i + 1;
            }
        }

        public bool CheckIsResultStable(string name)
        {
            return CountAverageSteps(name) * 1.2 > FindMaxSteps(name) &
                CountAverageSteps(name) * 0.8 < FindMinSteps(name);
        }

        public List<DataPoint> FillChartPoints()
        {
            var dataPoints = new List<DataPoint>();
            var keyDayValueSteps = Users
                .Where(u => u.Name == selectedUser.Name)
                .ToDictionary(u => u.Day, u => u.Steps);

            foreach (var keyValue in keyDayValueSteps)
            {
                dataPoints.Add(new DataPoint(keyValue.Key, keyValue.Value));
            }

            return dataPoints;
        }
    }
}
  