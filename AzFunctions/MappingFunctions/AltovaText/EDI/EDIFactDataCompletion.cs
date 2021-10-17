////////////////////////////////////////////////////////////////////////
//
// EDIFactDataCompletion.cs
//
// This file was generated by MapForce 2021r3.
//
// YOU SHOULD NOT MODIFY THIS FILE, BECAUSE IT WILL BE
// OVERWRITTEN WHEN YOU RE-RUN CODE GENERATION.
//
// Refer to the MapForce Documentation for further details.
// http://www.altova.com/mapforce
//
////////////////////////////////////////////////////////////////////////

using System.Globalization;

namespace Altova.TextParser.EDI
{
	/// <summary>
	/// Encapsulates auto completing data in a <see cref="ITextNode"/> hierarchy
	/// representing EDIFact data.
	/// </summary>
	public class EDIFactDataCompletion : DataCompletion
	{
		#region Implementation Detail:
		private EDIFactSettings mSettings = null;
		#endregion

		#region Public Interface:
		/// <summary>
		/// Constructs an instance of this class.
		/// </summary>
		/// <param name="document">textdocument to complete</param>
		/// <param name="settings">the settings to use</param>
		/// <param name="structurename">the name of the EDIFact structure which will be completed with this instance</param>
		public EDIFactDataCompletion(TextDocument document, EDIFactSettings settings, string structurename)
			: base(document, structurename)
		{
			mSettings = settings;
		}
		/// <summary>
		/// Completes/fills in missing data in a <see cref="ITextNode"/> hierarchy representing
		/// EDIFact data.
		/// </summary>
		/// <param name="dataroot">the root node of the hierarchy to complete</param>
		/// <param name="rootParticle">the structural root</param>
		public override void CompleteData(ITextNode dataroot, Particle rootParticle)
		{
			CompleteMandatory(dataroot, rootParticle);
			CompleteEnvelope(dataroot, rootParticle);
		}
		#endregion
		
		#region Implementation Detail

		/// <summary>
		/// Completes envelope
		/// </summary>
		protected void CompleteEnvelope(ITextNode envelope, Particle rootParticle)
		{
			if (envelope.Name != rootParticle.Name)
				throw new MappingException("CompleteEnvelope: root node is not an envelope");
			
			Particle interchangeParticle = rootParticle.Node.Children[0];
			MakeSureExists(envelope, interchangeParticle.Name);

			ITextNode[] interchanges = envelope.Children.FilterByName(interchangeParticle.Name);
			for (int i=0; i< interchanges.Length; ++i)
				CompleteInterchange(interchanges[i], interchangeParticle);
		}

		/// <summary>
		/// Completes interchange
		/// </summary>
		protected void CompleteInterchange(ITextNode interchange, Particle interchangeParticle)
		{
			Particle interchangeHeader = interchangeParticle.GetFirstChildByName("UNB") ?? interchangeParticle.GetFirstChildByName("UIB");
			Particle interchangeTrailer = interchangeParticle.GetFirstChildByName("UNZ") ?? interchangeParticle.GetFirstChildByName("UIZ");
			Particle group = interchangeParticle.GetFirstChildByName("Group");

			if (interchangeHeader != null && interchangeTrailer != null)
			{
				ITextNode header = interchange.Children.GetFirstNodeByName(interchangeHeader.Name);
				ITextNode trailer = interchange.Children.GetLastNodeByName(interchangeTrailer.Name);
				if (header == null && trailer == null)
				{
					if (group != null)
						MakeSureExists(interchange, group.Name);
				}
				else
				{
					header = MakeSureExists(interchange, interchangeHeader.Name);
					if (group != null)
						MakeSureExists(interchange, group.Name);
					trailer = MakeSureExists(interchange, interchangeTrailer.Name);
				}

				if (group != null)
				{
					ITextNode[] groups = interchange.Children.FilterByName("Group");
					for (int i = 0; i < groups.Length; ++i)
						CompleteGroup(groups[i], group);
				}
				else
				{
					CompleteGroup(interchange, interchangeParticle);
				}

				if (header == null && trailer == null)
					return;

				CompleteInterchangeHeader(header);
				CompleteInterchangeTrailer(trailer);
			}
		}

		/// <summary>
		/// Completes interchange header
		/// </summary>
		protected void CompleteInterchangeHeader(ITextNode header)
		{
			if (header.Name == "UNB")
			{
				ITextNode s001 = MakeSureExists(header, "S001");
				ITextNode s002 = MakeSureExists(header, "S002");
				ITextNode s003 = MakeSureExists(header, "S003");
				ITextNode s004 = MakeSureExists(header, "S004");
				ITextNode f0020 = MakeSureExists(header, "F0020");

				CompleteS001(s001);
				CompleteS002(s002);
				CompleteS003(s003);
				CompleteS004(s004);
			}
			else if (header.Name == "UIB")
			{
				ITextNode s001 = MakeSureExists(header, "S001");
				ITextNode s002 = MakeSureExists(header, "S002");
				ITextNode s003 = MakeSureExists(header, "S003");

				CompleteS001(s001);
				CompleteS002(s002);
				CompleteS003(s003);
			}
		}

		/// <summary>
		/// Completes interchange trailer
		/// </summary>
		protected void CompleteInterchangeTrailer(ITextNode trailer)
		{
			if (trailer.Name == "UNZ")
			{
				ITextNode f0036 = MakeSureExists(trailer, "F0036");
				ITextNode f0020 = MakeSureExists(trailer, "F0020");

				ConservativeSetValue(f0036, GetNumberOfFunctionGroupsOrMessages(trailer.Parent, false).ToString());
				ITextNode unb = trailer.Parent.Children.FilterByName("UNB")[0];
				ITextNode[] unbChildren = unb.Children.FilterByName("F0020");
				if (unbChildren.Length > 0)
				{
					string ctrlRef = unbChildren[unbChildren.Length - 1].Value;
					ConservativeSetValue(f0020, ctrlRef);
				}
			}
			else if (trailer.Name == "UIZ")
			{
				ITextNode f0036 = MakeSureExists(trailer, "F0036");

				ConservativeSetValue(f0036, GetNumberOfFunctionGroupsOrMessages(trailer.Parent, true).ToString());

				ITextNode uib = trailer.Parent.Children.FilterByName("UIB")[0];
				ITextNode[] uibChildren = uib.Children.FilterByName("S302");
				if (uibChildren.Length > 0)
				{
					ITextNode uib_s302 = uibChildren[0];
					ITextNode s302 = MakeSureExists(trailer, "S302");
					CompleteS302(s302, uib_s302);
				}
			}
		}

		/// <summary>
		/// Completes Group
		/// </summary>
		protected void CompleteGroup(ITextNode group, Particle groupParticle)
		{
			Particle groupHeader = groupParticle.GetFirstChildByName("UNG");
			Particle groupTrailer = groupParticle.GetFirstChildByName("UNE");

			ITextNode header = null;
			ITextNode trailer = null;
			if (groupHeader != null && groupTrailer != null)
			{
				header = group.Children.GetFirstNodeByName(groupHeader.Name);
				trailer = group.Children.GetFirstNodeByName(groupTrailer.Name);

				if (header != null)
				{
					trailer = MakeSureExists(group, groupTrailer.Name);
				}
				else if (trailer != null)
				{
					header = MakeSureExists(group, groupHeader.Name);
				}
			}

			foreach ( string sMessageType in mDocument.MessageTypes)
			{
				foreach (ITextNode node in group.Children.FilterByName("Message_" + sMessageType))
				{
					Particle messageParticle = mDocument.GetMessage(sMessageType).RootParticle;
					CompleteMandatory(node, messageParticle);
					CompleteMessage(sMessageType, node, messageParticle);
				}
			}

			foreach (ITextNode node in group.Children.FilterByName("Message") )
				CompleteMessage(mSettings.MessageType, node, mDocument.FirstMessage.RootParticle);

			if (header != null && trailer != null)
			{
				CompleteGroupHeader(header);
				CompleteGroupTrailer(trailer);
			}
		}

		/// <summary>
		/// Completes Group header
		/// </summary>
		protected void CompleteGroupHeader(ITextNode ung)
		{
			if (ung == null)
				return;

			ITextNode s006 = MakeSureExists(ung, "S006");
			ITextNode s007 = MakeSureExists(ung, "S007");
			ITextNode s004 = MakeSureExists(ung, "S004");
			ITextNode f0048 = MakeSureExists(ung, "F0048");
			ITextNode f0051 = MakeSureExists(ung, "F0051");
			ITextNode s008 = MakeSureExists(ung, "S008");
			ITextNode f0058 = MakeSureExists(ung, "F0058");
			
			CompleteS004(s004);
			if (ung.Parent.Children.FilterByName("UNE")[0].Children.FilterByName("F0048").Length > 0)
				ConservativeSetValue(f0048, ung.Parent.Children.FilterByName("UNE")[0].Children.FilterByName("F0048")[0].Value);
			ConservativeSetValue(f0051, mSettings.ControllingAgency.Substring(0,2));
		}

		/// <summary>
		/// Completes Group trailer
		/// </summary>
		protected void CompleteGroupTrailer(ITextNode une)
		{
			if (une == null)
				return;

			ITextNode f0060 = MakeSureExists(une, "F0060");
			ITextNode f0048 = MakeSureExists(une, "F0048");

			int nMsg = une.Parent.Children.FilterByName("Message").Length;
			foreach ( string sMessageType in mDocument.MessageTypes)
			{
				nMsg += une.Parent.Children.FilterByName("Message_" + sMessageType).Length;
			}

			ConservativeSetValue(f0060, nMsg.ToString());
			if ( une.Parent.Children.FilterByName("UNG")[0].Children.FilterByName("F0048").Length > 0)
				ConservativeSetValue(f0048, une.Parent.Children.FilterByName("UNG")[0].Children.FilterByName("F0048")[0].Value);
		}

		/// <summary>
		/// Completes Message
		/// </summary>
		protected void CompleteMessage(string sMessageType, ITextNode message, Particle messageParticle) 
		{
			Particle messageHeader = messageParticle.GetFirstChildByName("UNH") ?? messageParticle.GetFirstChildByName("UIH");
			Particle messageTrailer = messageParticle.GetFirstChildByName("UNT") ?? messageParticle.GetFirstChildByName("UIT");

			ITextNode header = MakeSureExists(message, messageHeader.Name);
			ITextNode trailer = MakeSureExists(message, messageTrailer.Name);

			CompleteMessageHeader(sMessageType, header, messageHeader);
			CompleteMessageTrailer(trailer, messageTrailer);
		}

		/// <summary>
		/// Completes Message header
		/// </summary>
		protected void CompleteMessageHeader(string sMessageType, ITextNode header, Particle headerParticle) 
		{
			if (headerParticle.Name == "UNH")
			{
				ITextNode f0062 = MakeSureExists(header, "F0062");
				ITextNode s009 = MakeSureExists(header, "S009");

				string referenceNumber = header.Parent.Children.FilterByName("UNT")[0].Value;
				if (referenceNumber.Length == 0)
					referenceNumber = "0";
				ConservativeSetValue(f0062, referenceNumber);
				CompleteS009(sMessageType, s009);
			}
			else if (headerParticle.Name == "UIH")
			{
				ITextNode f0340 = MakeSureExists(header, "F0340");
				ITextNode s306 = MakeSureExists(header, "S306");

				string referenceNumber = header.Parent.Children.FilterByName("UIT")[0].Value;
				if (referenceNumber.Length == 0)
					referenceNumber = "0";
				ConservativeSetValue(f0340, referenceNumber);
				CompleteS306(sMessageType, s306);
				
				ITextNode[] interchange_uib = header.Parent.Parent.Children.FilterByName("UIB");
				if (interchange_uib.Length > 0)
				{
					ITextNode uib = interchange_uib[0];
					ITextNode[] uibChildren = uib.Children.FilterByName("S302");
					if (uibChildren.Length > 0)
					{
						ITextNode uib_s302 = uibChildren[0];
						ITextNode s302 = MakeSureExists(header, "S302");
						CompleteS302(s302, uib_s302);
					}
				}
			}
		}

		/// <summary>
		/// Completes message trailer
		/// </summary>
		protected void CompleteMessageTrailer(ITextNode trailer, Particle trailerParticle) 
		{
			if (trailerParticle.Name == "UNT")
			{
				ITextNode f0074 = MakeSureExists(trailer, "F0074");
				ITextNode f0062 = MakeSureExists(trailer, "F0062");

				ConservativeSetValue(f0074, GetSegmentChildrenCount(trailer.Parent).ToString());
				if (trailer.Parent.Children.FilterByName("UNH")[0].Children.FilterByName("F0062").Length > 0)
					ConservativeSetValue(f0062, trailer.Parent.Children.FilterByName("UNH")[0].Children.FilterByName("F0062")[0].Value.ToString());
			}
			else if (trailerParticle.Name == "UIT")
			{
				ITextNode f0340 = MakeSureExists(trailer, "F0340");
				ITextNode f0074 = MakeSureExists(trailer, "F0074");

				if (trailer.Parent.Children.FilterByName("UIH")[0].Children.FilterByName("F0340").Length > 0)
					ConservativeSetValue(f0340, trailer.Parent.Children.FilterByName("UIH")[0].Children.FilterByName("F0340")[0].Value.ToString());
				ConservativeSetValue(f0074, GetSegmentChildrenCount(trailer.Parent).ToString());
			}
		}

		/// <summary>
		/// Completes S001 segment
		/// </summary>
		protected void CompleteS001(ITextNode s001)
		{
			ITextNode f0001 = MakeSureExists(s001, "F0001");
			ITextNode f0002 = MakeSureExists(s001, "F0002");

			ConservativeSetValue(f0001, mSettings.ControllingAgency + mSettings.SyntaxLevel);
			ConservativeSetValue(f0002, mSettings.SyntaxVersionNumber.ToString());
		}

		/// <summary>
		/// Completes S004 segment
		/// </summary>
		protected void CompleteS002(ITextNode s002) {
			ITextNode f0004 = MakeSureExists(s002, "F0004");
			ConservativeSetValue(f0004, "Sender");
		}
		
		/// <summary>
		/// Completes S003 segment
		/// </summary>
		protected void CompleteS003(ITextNode s003) {
			ITextNode f0010 = MakeSureExists(s003, "F0010");
			ConservativeSetValue(f0010, "Recipient");
		}
		
		/// <summary>
		/// Completes S004 segment
		/// </summary>
		protected void CompleteS004(ITextNode s004)
		{
			ITextNode f0017 = MakeSureExists(s004, "F0017");
			ITextNode f0019 = MakeSureExists(s004, "F0019");

			ConservativeSetValue(f0017, GetCurrentDateAsEDIString(mSettings.SyntaxVersionNumber));
			ConservativeSetValue(f0019, GetCurrentTimeAsEDIString());
		}

		/// <summary>
		/// Completes S009 segment
		/// </summary>
		protected void CompleteS009(string sMessageType, ITextNode s009)
		{
			ITextNode f0065 = MakeSureExists(s009, "F0065");
			ITextNode f0052 = MakeSureExists(s009, "F0052");
			ITextNode f0054 = MakeSureExists(s009, "F0054");
			ITextNode f0051 = MakeSureExists(s009, "F0051");
			
			ConservativeSetValue(f0065, sMessageType);
			ConservativeSetValue(f0051, mSettings.ControllingAgency.Substring(0, 2));
			ConservativeSetValue(f0052, mSettings.Version);
			ConservativeSetValue(f0054, mSettings.Release);
		}

		/// <summary>
		/// Completes S302 composite based on another S302
		/// </summary>
		protected void CompleteS302(ITextNode s302, ITextNode uib_s302)
		{
			foreach (ITextNode field in uib_s302.Children)
			{
				ITextNode node = MakeSureExists(s302, field.Name);
				ConservativeSetValue(node, field.Value);
			}
		}
		
		/// <summary>
		/// Completes S306 segment
		/// </summary>
		protected void CompleteS306(string sMessageType, ITextNode s306)
		{
			CompleteS009(sMessageType, s306);
		}

		/// <summary>
		/// Returns number of messages or groups 
		/// </summary>
		long GetNumberOfFunctionGroupsOrMessages(ITextNode node, bool interactive)
		{
			int nUNH =0;
			int nUNT =0;
			int nUNG =0;
			int nUNE =0;

			if (interactive)
			{
				foreach (string sMessageType in mDocument.MessageTypes)
				{
					foreach (ITextNode messageNode in node.Children.FilterByName("Message_" + sMessageType))
					{
						nUNH += messageNode.Children.FilterByName("UIH").Length;
						nUNT += messageNode.Children.FilterByName("UIT").Length;
					}
				}

				foreach (ITextNode messageNode in node.Children.FilterByName("Message"))
				{
					nUNH += messageNode.Children.FilterByName("UIH").Length;
					nUNT += messageNode.Children.FilterByName("UIT").Length;
				}
			}
			else
			{
				foreach( ITextNode groupNode in node.Children.FilterByName("Group") )
				{
					nUNG += groupNode.Children.FilterByName("UNG").Length;
					nUNE += groupNode.Children.FilterByName("UNE").Length;

					foreach (string sMessageType in mDocument.MessageTypes)
					{
						foreach (ITextNode messageNode in groupNode.Children.FilterByName("Message_" + sMessageType))
						{
							nUNH += messageNode.Children.FilterByName("UNH").Length;
							nUNT += messageNode.Children.FilterByName("UNT").Length;
						}
					}

					foreach (ITextNode messageNode in groupNode.Children.FilterByName("Message"))
					{
						nUNH += messageNode.Children.FilterByName("UNH").Length;
						nUNT += messageNode.Children.FilterByName("UNT").Length;
					}
				}
			}
			
			if (nUNH != nUNT)
				throw new MappingException("Message header-trailer mismatch");
			if (nUNG != nUNE)
				throw new MappingException("Group header-trailer mismatch");

			return nUNG == 0 ? nUNH : nUNG;
		}
		#endregion
	}
}