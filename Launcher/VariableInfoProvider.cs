/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * 
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using EnumerableExtensions;

namespace Launcher
{
    static class VariableInfoProvider
    {
        private static readonly List<VariableInfo> Info = new List<VariableInfo>();

        private static readonly Func<VariableInfo, string, bool> eqComparer = (x, y) => String.Equals(x.Name, y, StringComparison.CurrentCultureIgnoreCase);

        private static void EnsureLoadedInfo()
        {
            if (Info.Any()) return;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Launcher.KnownVariables.xml"))
            {
                var doc = new XmlDocument();
                doc.Load(stream);

                var rootNode = doc.ChildNodes.Cast<XmlNode>().Single(x => x.Name == "Variables");
                var childNodes = rootNode.ChildNodes.Cast<XmlNode>().ToArray();
                childNodes.ForEach(x =>
                {
                    var name = x.Attributes.Cast<XmlAttribute>().Single(a => a.Name == "Name").Value;
                    var hasRelated = x.Attributes.Cast<XmlAttribute>().Any(a => a.Name == "Relation");
                    var related = hasRelated ? x.Attributes.Cast<XmlAttribute>().Single(a => a.Name == "Relation").Value : null;
                    var info = x.InnerText;
                    Info.Add(new VariableInfo{ Name = name, Info = info, Related = related });
                });                
            }
        }

        public static bool Has(string variableName)
        {
            EnsureLoadedInfo();
            return Info.Any(x => eqComparer.Invoke(x, variableName));
        }
        public static VariableInfo Get(string variableName)
        {
            EnsureLoadedInfo();
            return Info.Single(x => eqComparer.Invoke(x, variableName));
        }
    }

    class VariableInfo
    {
        public string Name { get; set; }
        public string Info { get; set; }
        public string Related { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
