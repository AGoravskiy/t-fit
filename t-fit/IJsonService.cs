using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t_fit
{
    public interface IJsonService
    {
        void Serialize<T>(string fileName, T t);
        ObservableCollection<T> Deserialize<T>(string path);
    }
}
