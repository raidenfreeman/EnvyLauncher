/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * 
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 */

using System.Collections.Generic;
using System.Configuration;
using System;
using System.Linq;
using EnumerableExtensions;

namespace Launcher.Properties
{
    partial class Settings
    {
        private void EnsureExistence()
        {
            if (Keys == null || Values == null)
            {
                Keys = new System.Collections.Specialized.StringCollection();
                Values = new System.Collections.Specialized.StringCollection();
            }
        }
        public bool Has(string key)
        {
            EnsureExistence();
            return Keys.Cast<string>().Contains(key);
        }
        public string Get(string key)
        {
            EnsureExistence();
            return Keys.Cast<string>().Corresponding(key, Values.Cast<string>());
        }
        public void Set(string key, string value)
        {
            EnsureExistence();
            if (Has(key))
            {
                var index = Keys.Cast<string>().IndexOf(key);
                Values[index] = value;
            }
            else
            {
                Keys.Add(key);
                Values.Add(value);
            }
        }
    }
}
