using System;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace WebDavClientConsole {
	public class Options {
		[Option("h", "host", Required = true, HelpText = "the host-address for the WebDav account (ie: https://mediacenter.gmx.net/).")]
		public string Host { get; set; }
		
		[Option("u", "username", HelpText = "the username for the WebDav account.")]
		public string Username { get; set; }
		
		[Option("p", "password", HelpText = "the password for the WebDav account.")]
		public string Password { get; set; }
		
		[HelpOption("h", "help", HelpText = "Display this help screen.")]
		public string GetUsage() {
			HelpText help = new HelpText();
			help.AdditionalNewLineAfterOption = true;
			//help.Copyright = new CopyrightInfo("locrmap", 2009, 2012);
			help.AddOptions(this);
			
			return help;
		}
		
		public Options () {
			
		}
	}
}

