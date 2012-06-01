using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDesk.Options;
using System.IO;

namespace AnimeRecs.RecService.Client
{
    public enum ClientOperation
    {
        Ping
    }
    
    class CommandLineArgs
    {
        private bool m_showHelp = false;
        public bool ShowHelp { get { return m_showHelp; } set { m_showHelp = value; } }

        private bool m_operationSet = false;
        private ClientOperation m_operation;
        public ClientOperation Operation { get { return m_operation; } set { m_operation = value; m_operationSet = true; } }

        private string m_pingMessage = "ping";
        public string PingMessage { get { return m_pingMessage; } set { m_pingMessage = value; } }

        private int m_portNumber = 5541;
        public int PortNumber { get { return m_portNumber; } set { m_portNumber = value; } }

        public OptionSet GetOptionSet()
        {
            OptionSet optionSet = new OptionSet()
            {
                { "?|h|help", "Show this message and exit.", argExistence => ShowHelp = (argExistence != null) },
                { "c|command=", "Command", arg => SetCommand(arg) },
                { "p|port=", "Port the rec service is listening on. Defaults to 5541.", arg => PortNumber = int.Parse(arg) },
                { "ping_message=", "Message to send with a ping command. Only has meaning when the ping command is used. Defaults to \"ping\".", arg => PingMessage = arg },
            };

            return optionSet;
        }

        private void SetCommand(string command)
        {
            if (command.Equals("ping", StringComparison.OrdinalIgnoreCase))
            {
                Operation = ClientOperation.Ping;
            }
            else
            {
                throw new OptionException(string.Format("{0} is not a valid command.", command), "command");
            }
        }

        public CommandLineArgs(string[] args)
        {
            OptionSet optionSet = GetOptionSet();
            optionSet.Parse(args);

            if (!m_operationSet && !ShowHelp)
            {
                throw new OptionException("Command was not specified.", "command");
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
// This file is part of AnimeRecs.RecService.Client.
//
// AnimeRecs.RecService.Client is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService.Client is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.Client.  If not, see <http://www.gnu.org/licenses/>.