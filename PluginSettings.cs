using System;
using System.Linq;
using GitUIPluginInterfaces;

namespace GitTfs.GitExtensions.Plugin
{
    public class PluginSettings
    {
        private StringSetting _tfsRemote = new StringSetting(SettingKeys.TfsRemote, "");
        private readonly StringSetting _shelveSetName = new StringSetting(SettingKeys.ShelvesetName, "");
        private readonly BoolSetting _overwrite = new BoolSetting(SettingKeys.OverwriteShelveset, false);

        public StringSetting ShelveSetName
        {
            get { return _shelveSetName; }
        }

        public BoolSetting Overwrite
        {
            get { return _overwrite; }
        }
 
        public StringSetting TfsRemote
        {
            get { return _tfsRemote; }
        }

        public PullBehaviour? PullBehaviour { get; set; }


        public PushBehaviour? PushBehaviour { get; set; }


//        private T? GetEnumSettingValue<T>(string key)
//            where T : struct
//        {
//            var type = typeof (T);
//            var value = _container.GetSetting(key);
//
//            return (from name in Enum.GetNames(type)
//                    where name == value
//                    select (T?) Enum.Parse(type, name)).FirstOrDefault();
//        }
//
//        private void SetEnumSettingValue<T>(string key, T? value)
//            where T : struct
//        {
//            _container.SetSetting(key, value.HasValue ? value.ToString() : string.Empty);
//        }
    }
}
