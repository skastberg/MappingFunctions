////////////////////////////////////////////////////////////////////////
//
// EDITRASACOMSDataCompletion.cs
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

using System;
using System.Globalization;

namespace Altova.TextParser.EDI
{
	/// <summary>
	/// Encapsulates auto completing data in a <see cref="ITextNode"/> hierarchy
	/// representing EDITRADACOMS data.
	/// </summary>
	public class EDITradacomsDataCompletion : DataCompletion
	{
		#region Implementation Detail:
		private EDITradacomsSettings mSettings = null;
		private bool mWriteReconciliation = false;
		private int mMessageCounter = 0;
		private string mSenderReference;
		private string mRecieverCode;
		#endregion

		#region Public Interface:
		/// <summary>
		/// Constructs an instance of this class.
		/// </summary>
		/// <param name="document">textdocument to complete</param>
		/// <param name="settings">the settings to use</param>
		/// <param name="structurename">the name of the EDITRADACOMS structure which will be completed with this instance</param>
		public EDITradacomsDataCompletion(TextDocument document, EDITradacomsSettings settings, string structurename)
			: base(document, structurename)
		{
			mSettings = settings;
			mCompleteSingleValues = true;
		}
		/// <summary>
		/// Completes/fills in missing data in a <see cref="ITextNode"/> hierarchy representing
		/// EDITRADACOMS data.
		/// </summary>
		/// <param name="dataroot">the root node of the hierarchy to complete</param>
		/// <param name="rootParticle">the structural root</param>
		public override void CompleteData(ITextNode dataroot, Particle rootParticle)
		{
			CompleteMandatory(dataroot,rootParticle);
			CompleteEnvelope(dataroot, rootParticle);
		}
		#endregion
		
		#region Implementation Detail

		/// <summary>
		/// Completes envelope
		/// </summary>
		protected void CompleteEnvelope (ITextNode envelope, Particle rootParticle)
		{
			if (envelope.Name != "Envelope")
				throw new MappingException("CompleteEnvelope: root node is not an envelope");

			MakeSureExists(envelope, "Interchange", NodeClass.Group);

			ITextNode[] interchanges = envelope.Children.FilterByName("Interchange");
			Particle p = rootParticle.Node.Children[0];
			for (int i=0; i< interchanges.Length; ++i)
				CompleteInterchange(interchanges[i], p);
		}

		/// <summary>
		/// Completes interchange
		/// </summary>
		protected void CompleteInterchange(ITextNode interchange, Particle particle)
		{
			ITextNode stx = MakeSureExists(interchange, "STX", NodeClass.Segment);
			MakeSureExists(interchange, "Batch", NodeClass.Group);
			ITextNode end = MakeSureExists (interchange, "END", NodeClass.Segment);
			
			mWriteReconciliation = false;
			mMessageCounter = 0;

			CompleteInterchangeHeader(stx);

			ITextNode[] groups = interchange.Children.FilterByName("Batch");
			for (int i=0; i< groups.Length; ++i)
				CompleteBatch(groups[i]);
		
			if (mWriteReconciliation)
			{
				ITextNode rsgrsg = MakeSureExists (interchange, "RSGRSG", NodeClass.Group);
				CompleteReconciliationMessage(rsgrsg, GetParticleByPath(particle, rsgrsg.Name));
			}
			
			CompleteInterchangeTrailer(end);
		}

		/// <summary>
		/// Completes interchange header
		/// </summary>
		protected void CompleteInterchangeHeader(ITextNode stx)
		{
			ITextNode stds = MakeSureExists(stx, "STDS", NodeClass.Composite);
			ITextNode stds1 = MakeSureExists(stds, "STDS-1", NodeClass.DataElement);
			ITextNode stds2 = MakeSureExists(stds, "STDS-2", NodeClass.DataElement);
			ITextNode trdt = MakeSureExists(stx, "TRDT", NodeClass.Composite);
			ITextNode trdt1 = MakeSureExists(trdt, "TRDT-1", NodeClass.DataElement);
			ITextNode trdt2 = MakeSureExists(trdt, "TRDT-2", NodeClass.DataElement);
		
			ITextNode snrf = stx.Children.GetFirstNodeByName("SNRF");
			ITextNode unto = stx.Children.GetFirstNodeByName("UNTO");
			ITextNode unto1 = unto != null ? unto.Children.GetFirstNodeByName("UNTO-1") : null;
			
			ConservativeSetValue(stds1, "ANA");
			ConservativeSetValue(stds2, "1");
			ConservativeSetValue(trdt1, DateTime.Now.ToString("yyMMdd"));
			ConservativeSetValue(trdt2, DateTime.Now.ToString("HHmmss"));
			
			mWriteReconciliation = stds1.Value == "ANAA";
			if (mWriteReconciliation)
			{
				mSenderReference = snrf != null ? snrf.Value : "";
				mRecieverCode = unto1 != null ? unto1.Value : "";
			}
		}

		/// <summary>
		/// Completes Reconciliation message
		/// </summary>
		protected void CompleteReconciliationMessage(ITextNode rsgrsg, Particle particle)
		{
			CompleteMandatory(rsgrsg, particle);
			CompleteMessage(rsgrsg);
			
			ITextNode rsg = MakeSureExists(rsgrsg, "RSG", NodeClass.Segment);
			ITextNode rsga = MakeSureExists(rsg, "RSGA", NodeClass.DataElement);
			ITextNode rsgb = MakeSureExists(rsg, "RSGB", NodeClass.DataElement);

			ConservativeSetValue(rsga, mSenderReference);
			ConservativeSetValue(rsgb, mRecieverCode);
			CompleteConditionsAndValues(rsgrsg, particle);
		}
		
		/// <summary>
		/// Completes interchange trailer
		/// </summary>
		protected void CompleteInterchangeTrailer(ITextNode end)
		{
			ITextNode nmst = MakeSureExists(end, "NMST", NodeClass.DataElement);

			ConservativeSetValue(nmst, mMessageCounter.ToString());
		}

		/// <summary>
		/// Completes Batch
		/// </summary>
		protected void CompleteBatch(ITextNode group)
		{
			ITextNode bat = null;
			ITextNode eob = null;

			if (HasKid(group, "BAT"))
			{
				eob = MakeSureExists(group, "EOB", NodeClass.Segment);
			}
			else if (HasKid(group, "EOB"))
			{
				bat = MakeSureExists(group, "BAT", NodeClass.Segment);
			}

			int nMsgCount = 0;
			
			foreach (ITextNode node in group.Children)
			{
				if (node.Name.StartsWith("Message_"))
				{
					string sMessageType = node.Name.Substring("Message_".Length);
					Particle particle = mDocument.GetMessage(sMessageType).RootParticle;
					CompleteMandatory(node, particle);
					CompleteFile(node);
					CompleteConditionsAndValues(node, particle);
					nMsgCount += node.Children.Count;
				}
			}

			if (eob != null)
				CompleteBatchTrailer(eob, nMsgCount);
		}

		/// <summary>
		/// Completes Batch trailer
		/// </summary>
		protected void CompleteBatchTrailer(ITextNode eob, int nMsgCount)
		{
			ITextNode noli = MakeSureExists(eob, "NOLI", NodeClass.DataElement);

			ConservativeSetValue(noli, nMsgCount.ToString());
		}

		/// <summary>
		/// Completes File that consists of header message, detail messages and trailer message
		/// </summary>
		protected void CompleteFile(ITextNode message) 
		{
			foreach (ITextNode node in message.Children)
			{
				CompleteMessage(node);
			}
		}

		/// <summary>
		/// Completes Message
		/// </summary>
		protected void CompleteMessage(ITextNode message) 
		{
			ITextNode mhd = MakeSureExists(message, "MHD", NodeClass.Segment);
			ITextNode mtr = MakeSureExists(message, "MTR", NodeClass.Segment);

			CompleteMessageHeader(mhd);
			CompleteMessageTrailer(mtr);
		}

		/// <summary>
		/// Completes Message header
		/// </summary>
		protected void CompleteMessageHeader(ITextNode mhd) 
		{
			ITextNode msrf = MakeSureExists(mhd, "MSRF", NodeClass.DataElement);
			++mMessageCounter;
			ConservativeSetValue(msrf, mMessageCounter.ToString());
		}

		/// <summary>
		/// Completes message trailer
		/// </summary>
		protected void CompleteMessageTrailer(ITextNode mtr) 
		{
			ITextNode nosg = MakeSureExists(mtr, "NOSG", NodeClass.DataElement);
			ConservativeSetValue(nosg, GetSegmentChildrenCount(mtr.Parent).ToString());
		}

		#endregion
	}
}