/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * 
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EnumerableExtensions;
using Launcher.Properties;
using System.Configuration;
using Path = System.IO.Path;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal LaunchData LaunchData { get { return (DataContext as LaunchData); } }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpgradeSettingsIfRequired();
            ExecutableSelector.SelectedFileChanged += selector => LoadConfigurationFor(selector.File);
            LoadFileFromCommandlineArgumentsIfAny();
        }

        private void LoadFileFromCommandlineArgumentsIfAny()
        {
            if (Arguments.Any())
            {
                ExecutableSelector.File = Arguments.GetFile();
                if (Settings.Default.Has(ExecutableSelector.File))
                {
                    if (Arguments.IsSilent()) Hide();
                    Launch();
                    if (Arguments.IsSilent()) Application.Current.Shutdown();
                }
            }
            else if (!string.IsNullOrEmpty(Settings.Default.LastLaunch))
                ExecutableSelector.File = Settings.Default.LastLaunch;
            
        }

        private void UpgradeSettingsIfRequired()
        {
            if (!Settings.Default.Upgraded)
            {
                Settings.Default.Upgrade();
                Settings.Default.Upgraded = true;
                Settings.Default.Save();
            }
        }

        private void LoadConfigurationFor(string file)
        {
            LaunchData.Executable = ExecutableSelector.File;
            if (Settings.Default.Has(file))
                LaunchData.FromString(Settings.Default.Get(file));
            else
                LaunchData.Reset();
        }

        private void Launch()
        {
            var startInfo = new ProcessStartInfo { FileName = ExecutableSelector.File, UseShellExecute = false };
            startInfo.EnvironmentVariables.Clear();
            LaunchData.Variables.ForEach(x => startInfo.EnvironmentVariables.Add(x.Name, x.Value));

            var process = new Process { StartInfo = startInfo };
            process.Start();

            Settings.Default.Set(LaunchData.Executable, LaunchData.Variables.Serialize());
            Settings.Default.LastLaunch = ExecutableSelector.File;
            Settings.Default.Save();
        }
        private void Launch(object sender, RoutedEventArgs e)
        {
            Launch();
        }
    }
}
