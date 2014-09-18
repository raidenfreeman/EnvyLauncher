/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * 
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnumerableExtensions;

namespace Launcher
{
    class LaunchData : INotifyPropertyChanged
    {
        public ObservableCollection<EnvironmentVariable> Variables { get; set; }

        public LaunchData()
        {
            Variables = new ObservableCollection<EnvironmentVariable>();
            Reset();
        }

        public void Reset()
        {
            Variables.Clear();
            EnvironmentVariable.Current
                .ApplySorting()
                .ForEach(Variables.Add);
        }

        public string Executable
        {
            get { return _executable; }
            set
            {
                _executable = value;
                OnPropertyChanged("Executable");
            }
        }
        private string _executable;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public void FromFile(string file)
        {
            FromString(File.ReadAllText(file));
        }
        public void FromString(string text)
        {
            Variables.Clear();
            text.Deserialize()
                .ApplySorting()
                .ForEach(Variables.Add);
        }

    }

    static class LaunchDataExtensions
    {
        public static IEnumerable<EnvironmentVariable> ApplySorting(this IEnumerable<EnvironmentVariable> sequence)
        {
            return sequence.OrderByDescending(x => x.HasCustomValue).ThenBy(x => x.Relation);
        }

    }
}
