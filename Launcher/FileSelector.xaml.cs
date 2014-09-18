/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * 
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Launcher.Graphics;
using Microsoft.Win32;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for FileSelector.xaml
    /// </summary>
    public partial class FileSelector : UserControl
    {
        public event Action<FileSelector> SelectedFileChanged;

        private FileSource Source { get { return DataContext as FileSource; } }

        public string File
        {
            get { return Source.File; }
            set
            {
                Source.File = value;
                OnSelectedFileChanged();
            }
        }

        public string Extension { get; set; }

        public FileSelector()
        {
            InitializeComponent();
        }

        private void SelectFile(object sender, RoutedEventArgs e)
        {
            SelectFile();
        }
        private void SelectFile()
        {
            var dialog = new OpenFileDialog { Filter = "Files (." + Extension + ") | *." + Extension };
            dialog.ShowDialog();
            Source.File = string.IsNullOrEmpty(dialog.FileName) ? null : dialog.FileName;
            OnSelectedFileChanged();
        }
        protected void OnSelectedFileChanged()
        {
            if (SelectedFileChanged != null)
                SelectedFileChanged(this);
        }
    }
}
