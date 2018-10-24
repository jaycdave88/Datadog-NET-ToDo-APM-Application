﻿using System.Collections.Specialized;
using System.Configuration;
using System.Dynamic;

namespace personal_website.v3.Controllers
{
    public class AppSettingsWrapper : DynamicObject
    {
        private NameValueCollection _items;

        public AppSettingsWrapper()
        {
            _items = ConfigurationManager.AppSettings;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _items[binder.Name];
            return result != null;
        }
    }
}