using System;
using System.Collections.Generic;
using System.Text;

namespace RelearnMvvm.Services
{
    public interface IPreferencesStorage
    {
        void Set(string key, int value);

        int Get(string key, int defaultValue);

        //void Set<T>(string key, T value);

        //T Get<T>(string key, T defaultValue);
    }
}
