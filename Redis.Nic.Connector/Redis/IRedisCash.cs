using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis.Nic.Connector.Redis
{
    internal interface IRedisCash
    {
        T GetData<T>(string key);
        bool SetData<T>(string key, T value);
        bool RemoveData(string key);


    }
}
