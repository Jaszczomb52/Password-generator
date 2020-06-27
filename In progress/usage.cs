using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace losowanieHasla
{
    class Usage
    {
        static public void Checkboxes(Configuration config, string name, string value)
        {
            config.AppSettings.Settings[name].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
