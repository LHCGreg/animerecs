using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.ClientLib;

namespace AnimeRecs.RecService.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArgs commandLine = new CommandLineArgs(args);

            if (commandLine.ShowHelp)
            {
                commandLine.DisplayHelp(Console.Out);
                return;
            }
            
            using (AnimeRecsClient client = new AnimeRecsClient(commandLine.PortNumber))
            {
                switch (commandLine.Operation)
                {
                    case ClientOperation.Ping:
                        string pingResponse = client.Ping(commandLine.PingMessage);
                        Console.WriteLine("The service replied: {0}", pingResponse);
                        break;
                    default:
                        throw new Exception(string.Format("Oops, missed an operation: {0}", commandLine.Operation));
                }
            }
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