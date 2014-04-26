using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDesk.Options;
using System.IO;

namespace AnimeRecs.UpdateStreams
{
    internal enum Operation
    {
        CreateCsv,
        GenerateSql
    }
    
    internal class CommandLineArgs
    {
        private bool m_showHelp = false;
        public bool ShowHelp { get { return m_showHelp; } private set { m_showHelp = value; } }

        public string InputFile { get; private set; }
        public string OutputFile { get; private set; }

        private Operation m_op = Operation.CreateCsv;
        public Operation Op { get { return m_op; } set { m_op = value; } }

        public OptionSet GetOptionSet()
        {
            OptionSet optionSet = new OptionSet()
            {
                { "?|h|help", "Show this message and exit.", argExistence => ShowHelp = (argExistence != null) },
                { "i|input=", "Input file. Omit to create a new csv.", arg => InputFile = arg },
                { "o|output=", "Output file.", arg => OutputFile = arg },
                { "sql", "Generate SQL from the input csv file. All mappings in the file are expected to be completed.", argExistence => { if(argExistence != null) Op = Operation.GenerateSql; } }
            };

            return optionSet;
        }

        public CommandLineArgs(string[] args)
        {
            OptionSet optionSet = GetOptionSet();
            optionSet.Parse(args);

            if (ShowHelp)
            {
                DisplayHelp(Console.Out);
                Environment.Exit(0);
            }

            if (OutputFile == null)
            {
                throw new OptionException("Output file not specified.", "o");
            }

            if (Op == Operation.GenerateSql && InputFile == null)
            {
                throw new OptionException("Input file not specified.", "i");
            }
        }

        public void DisplayHelp(TextWriter writer)
        {
            writer.WriteLine("Usage: {0} [OPTIONS]", GetProgramName());
            writer.WriteLine();
            writer.WriteLine("Parameters:");
            GetOptionSet().WriteOptionDescriptions(writer);
        }

        public static string GetProgramName()
        {
            string[] argsWithProgramName = System.Environment.GetCommandLineArgs();
            string programName;
            if (argsWithProgramName[0].Equals(string.Empty))
            {
                // "If the file name is not available, the first element is equal to String.Empty."
                // Doesn't say why that would happen, but ok...
                programName = (new System.Reflection.AssemblyName(System.Reflection.Assembly.GetExecutingAssembly().FullName).Name) + ".exe";
            }
            else
            {
                programName = Path.GetFileName(argsWithProgramName[0]);
            }

            return programName;
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.UpdateStreams
//
// AnimeRecs.UpdateStreams is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.UpdateStreams is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.UpdateStreams.  If not, see <http://www.gnu.org/licenses/>.
//
//  If you modify AnimeRecs.UpdateStreams, or any covered work, by linking 
//  or combining it with HTML Agility Pack (or a modified version of that 
//  library), containing parts covered by the terms of the Microsoft Public 
//  License, the licensors of AnimeRecs.UpdateStreams grant you additional 
//  permission to convey the resulting work. Corresponding Source for a non-
//  source form of such a combination shall include the source code for the parts 
//  of HTML Agility Pack used as well as that of the covered work.