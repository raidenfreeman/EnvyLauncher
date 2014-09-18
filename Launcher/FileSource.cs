/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * 
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Launcher.Graphics;
using EnumerableExtensions;

namespace Launcher
{
    class FileSource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool HasFile
        {
            get { return !string.IsNullOrEmpty(File) && System.IO.File.Exists(File); }
        }
        public string File
        {
            get { return _file; }
            set
            {
                _file = value;
                OnPropertyChanged("File");
                OnPropertyChanged("HasFile");
                OnPropertyChanged("FileNameWithoutPath");
                OnPropertyChanged("Icon");
                OnPropertyChanged("IconImageSource");
            }
        }
        public Icon Icon
        {
            get
            {
                if (File == null || !System.IO.File.Exists(File)) return null;
                return Icon.ExtractAssociatedIcon(File);
            }
        }

        private string _file;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) 
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
