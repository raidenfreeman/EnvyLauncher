/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * 
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;
using EnumerableExtensions;

namespace Launcher
{
    class EnvironmentVariable : INotifyPropertyChanged
    {
        public static IEnumerable<EnvironmentVariable> Current
        {
            get
            {
                return Environment.GetEnvironmentVariables().Keys
                    .Cast<string>()
                    .Select(key => new EnvironmentVariable(key, Environment.GetEnvironmentVariable(key)));
            }
        }

        public bool HasCustomValue { get { return Environment.GetEnvironmentVariable(Name) != Value; } }
        public bool IsKnownVariable { get { return VariableInfoProvider.Has(Name); } }
        public string VariableInfo { get { return IsKnownVariable ? VariableInfoProvider.Get(Name).Info : Name; } }
        public bool HasKnownRelation { get { return IsKnownVariable && VariableInfoProvider.Get(Name).Related != null; } }
        public string Relation { get { return HasKnownRelation ? VariableInfoProvider.Get(Name).Related : string.Empty; } }

        public string Name
        {
            get { return _name ?? string.Empty; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("HasCustomValue");
            }
        }
        private string _name;

        public string Value
        {
            get { return _value ?? string.Empty; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
                OnPropertyChanged("HasCustomValue");
            }
        }
        private string _value;

        public EnvironmentVariable()
        {            
        }
        public EnvironmentVariable(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) 
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    static class EnvironmentVariableExtensions
    {
        public static IEnumerable<EnvironmentVariable> Distinct(this IEnumerable<EnvironmentVariable> sequence)
        {
            return sequence.Distinct((x, y) => x.Name == y.Name);
        }
        public static string Serialize(this IEnumerable<EnvironmentVariable> sequence)
        {
            return string.Join("\n", sequence.Select(x => x.Name + "\t" + x.Value));
        }
        public static IEnumerable<EnvironmentVariable> Deserialize(this string sequence)
        {
            return sequence.Split(new[] { '\n' }, StringSplitOptions.None)
                .Select(x =>
                {
                    var kv = x.Split('\t');
                    return new EnvironmentVariable(kv[0], kv[1]);
                });
        }
    }
}
