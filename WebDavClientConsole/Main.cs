using System;
using System.Net;
using CommandLine;
using CommandLine.Text;
using WebDav;
using WebDav.Client;

namespace WebDavClientConsole {
	class MainClass {
		public static Options Options = new Options();
		
		public static void Main (string[] args) {
			ICommandLineParser parser = new CommandLineParser();
			if (!parser.ParseArguments(args, Options) || args.Length == 0) {
				Console.WriteLine(Options.GetUsage());
				return;
			}
			
			WebDavSession session = new WebDavSession();
			if (Options.Username != String.Empty && Options.Password != String.Empty) {
				session.Credentials = new NetworkCredential(Options.Username, Options.Password);
			}
			IFolder folder = session.OpenFolder(Options.Host);
			IHierarchyItem[] items = folder.GetChildren();
			foreach(IHierarchyItem item in items) {
				Console.WriteLine(item.DisplayName);
			}
			
			Console.WriteLine(Options.Host);
		}
	}
}
