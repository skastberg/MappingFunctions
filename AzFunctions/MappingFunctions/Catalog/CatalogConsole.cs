//
// CatalogConsole.cs
//
// This file was generated by MapForce 2021r3.
//
// YOU SHOULD NOT MODIFY THIS FILE, BECAUSE IT WILL BE
// OVERWRITTEN WHEN YOU RE-RUN CODE GENERATION.
//
// Refer to the MapForce Documentation for further details.
// http://www.altova.com/mapforce
//

using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using Altova.Types;
//using Altova.Functions;

namespace Catalog
{
	public class CatalogConsole 
	{

		public static void Main(string[] args) 
		{
			Console.Error.WriteLine("Catalog Application");
			
			try 
			{
				TraceTargetConsole ttc = new TraceTargetConsole();
				System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
				SummaryValueMapMapToSummary SummaryValueMapMapToSummaryObject = new SummaryValueMapMapToSummary();
				SummaryValueMapMapToSummaryObject.RegisterTraceTarget(ttc);
	


				// run mapping
				//
				// you have different options to provide mapping input and output:
				//
				// files using file names (available for XML, text, and Excel):
				//   Altova.IO.FileInput(string filename)
				//   Altova.IO.FileOutput(string filename)
				//
				// streams (available for XML, text, and Excel):
				//   Altova.IO.StreamInput(System.IO.Stream stream)
				//   Altova.IO.StreamOutput(System.IO.Stream stream)
				//
				// strings (available for XML and text):
				//   Altova.IO.StringInput(string content)
				//   Altova.IO.StringOutput(StringBuilder content)
				//
				// System.IO reader/writer (available for XML and text):
				//   Altova.IO.ReaderInput(System.IO.TextReader reader)
				//   Altova.IO.WriterOutput(System.IO.TextWriter writer)
				//
				// DOM documents (for XML only):
				//   Altova.IO.DocumentInput(System.Xml.XmlDocument document)
				//   Altova.IO.DocumentOutput(System.Xml.XmlDocument document)
				// 
				// By default, Run will close all inputs and outputs. If you do not want this,
				// set the following property:
				// SummaryValueMapMapToSummaryObject.CloseObjectsAfterRun = false;
				
				{
				Altova.IO.Input Catalog2Source = Altova.IO.StreamInput.createInput("../Books.xml");
				Altova.IO.Output Summary2Target = new Altova.IO.FileOutput("../Catalog.Summary.json");

				try
				{
					SummaryValueMapMapToSummaryObject.Run(
					Catalog2Source,
					Summary2Target
					);		

				}
				finally
				{
					Catalog2Source.Close();
					Summary2Target.Close();

				}
				}
				SummaryLookupMapToCatalog_Summary_Schema SummaryLookupMapToCatalog_Summary_SchemaObject = new SummaryLookupMapToCatalog_Summary_Schema();
				SummaryLookupMapToCatalog_Summary_SchemaObject.RegisterTraceTarget(ttc);
	


				// run mapping
				//
				// you have different options to provide mapping input and output:
				//
				// files using file names (available for XML, text, and Excel):
				//   Altova.IO.FileInput(string filename)
				//   Altova.IO.FileOutput(string filename)
				//
				// streams (available for XML, text, and Excel):
				//   Altova.IO.StreamInput(System.IO.Stream stream)
				//   Altova.IO.StreamOutput(System.IO.Stream stream)
				//
				// strings (available for XML and text):
				//   Altova.IO.StringInput(string content)
				//   Altova.IO.StringOutput(StringBuilder content)
				//
				// System.IO reader/writer (available for XML and text):
				//   Altova.IO.ReaderInput(System.IO.TextReader reader)
				//   Altova.IO.WriterOutput(System.IO.TextWriter writer)
				//
				// DOM documents (for XML only):
				//   Altova.IO.DocumentInput(System.Xml.XmlDocument document)
				//   Altova.IO.DocumentOutput(System.Xml.XmlDocument document)
				// 
				// By default, Run will close all inputs and outputs. If you do not want this,
				// set the following property:
				// SummaryLookupMapToCatalog_Summary_SchemaObject.CloseObjectsAfterRun = false;
				
				{
				Altova.IO.Input Catalog3Source = Altova.IO.StreamInput.createInput("C:/TransformMapforce/Books.xml");
				Altova.IO.Input Shelfs2Source = Altova.IO.StreamInput.createInput("../Shelfs.json");
				Altova.IO.Output Catalog_Summary_SchemaTarget = new Altova.IO.FileOutput("../Catalog.Summary.json");

				try
				{
					SummaryLookupMapToCatalog_Summary_SchemaObject.Run(
					Catalog3Source,
					Shelfs2Source,
					Catalog_Summary_SchemaTarget
					);		

				}
				finally
				{
					Catalog3Source.Close();
					Shelfs2Source.Close();
					Catalog_Summary_SchemaTarget.Close();

				}
				}

				Console.Error.WriteLine("Finished");
			} 
			catch (Altova.UserException ue)
			{
				Console.Error.Write("USER EXCEPTION: ");
				Console.Error.WriteLine( ue.Message );
				System.Environment.Exit(1);
			}
			catch (Exception e) 
			{
				Console.Error.Write("ERROR: ");
				Console.Error.WriteLine( e.Message );
				if (e.InnerException != null)
				{
					Console.Error.Write("Inner Exception: ");
					Console.Error.WriteLine(e.InnerException.Message);
				}
				Console.Error.WriteLine("\nStack Trace: ");
				Console.Error.WriteLine( e.StackTrace );
				System.Environment.Exit(1);
			}
		}
	}


	class TraceTargetConsole : Altova.ITraceTarget
	{
		public void WriteTrace(string info)
		{
			Console.Error.WriteLine(info);
		}
	}
}
