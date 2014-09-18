/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * 
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnumerableExtensions;

namespace Launcher
{
    static class Arguments
    {
        private static IEnumerable<string> Parameters { get { return Environment.GetCommandLineArgs().ButFirst(); } }

        public static bool Any()
        {
            return Parameters.Any();
        }

        public static string GetFile()
        {
            if (Any()) return Parameters.First();
            else return string.Empty;
        }

        public static bool IsSilent()
        {
            return Parameters.Contains("/silent");
        }
    }
}
