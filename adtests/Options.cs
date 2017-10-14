using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adtests
{
    class Options
    {
        [Option('d', "domain", Required = true,
          HelpText = "Name of the domain to check.")]
        public string Domain { get; set; }

        [Option('u', "username", Required = true,
          HelpText = "The user name to check.")]
        public string Username { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
