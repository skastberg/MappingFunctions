////////////////////////////////////////////////////////////////////////
//
// EDISemanticValidator.cs
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

namespace Altova.TextParser.EDI
{
	/// <summary>
	/// Responsible for validation of EDI
	/// </summary>
	public class EDISemanticValidator
	{
		#region Implementation Detail:
		long mnSegmentCount;
		long mnGroupCount;
		long mnMessageCount;
		EDISettings mSettings;
		System.Collections.Hashtable mMapEquality = 
			new System.Collections.Hashtable();
		string msCurrentSegment;
		private string msCurrentMessageType;
		#endregion

		#region Public Interface:
		/// <summary>
		/// Constructs an instance of this class.
		/// </summary>
		/// <param name="settings">the settings to use</param>
		public EDISemanticValidator(EDISettings settings)
		{
			mSettings = settings;
			mnSegmentCount = 0;
			mnGroupCount = 0;
			mnMessageCount = 0;
		}
		/// <summary>
		/// Validates a Field.
		/// </summary>
		/// <param name="sField">Field identifier.</param>
		/// <param name="sValue">Value of the field.</param>
		public string Validate( string sParent, string sField, string sValue )
		{
			if( mSettings.Standard == EDIStandard.EDIFact )
				return ValidateEDIFACT( sParent, sField, sValue );
			if( mSettings.Standard == EDIStandard.EDISCRIPT )
				return ValidateSCRIPT( sParent, sField, sValue );
			if (mSettings.Standard == EDIStandard.EDIX12)
				return ValidateX12( sParent, sField, sValue );
			if (mSettings.Standard == EDIStandard.EDIHL7)
				return ValidateHL7( sParent, sField, sValue );
			if (mSettings.Standard == EDIStandard.EDITRADACOMS)
				return ValidateTRADACOMS( sParent, sField, sValue );

			return "";
		}
		
		public void Segment( string sSegment )
		{
			switch (mSettings.Standard)
			{
				case EDIStandard.EDIFact:
				case EDIStandard.EDISCRIPT:
					if (sSegment == "UNB")
					{
						mnGroupCount = 0;
					}
					else if (sSegment == "UIB")
					{
						mnMessageCount = 0;
					}
					else if (sSegment == "UNG")
					{
						mnGroupCount++;
						mnMessageCount = 0;
					}
					else if (sSegment == "UNH" || sSegment == "UIH")
					{
						mnMessageCount++;
						mnSegmentCount = 1;
					}
					else
					{
						mnSegmentCount++;
					}
					break;
				case EDIStandard.EDIX12:
					if (sSegment == "ISA")
					{
						mnGroupCount = 0;
					}
					else if (sSegment == "GS")
					{
						mnGroupCount++;
						mnMessageCount = 0;
					}
					else if (sSegment == "ST")
					{
						mnMessageCount++;
						mnSegmentCount = 1;
					}
					else
					{
						mnSegmentCount++;
					}
					break;
				case EDIStandard.EDITRADACOMS:
					if (sSegment == "STX")
					{
						mnMessageCount = 0;
					}
					else if (sSegment == "BAT")
					{
						mnGroupCount = 0; // count messages in batch
					}
					else if (sSegment == "MHD")
					{
						mnMessageCount++;
						mnGroupCount++;
						mnSegmentCount = 1;
					}
					else
					{
						mnSegmentCount++;
					}
					break;

			}

			msCurrentSegment = sSegment;
		}

		public string CurrentMessageType
		{
			set { msCurrentMessageType = value; }
		}
		#endregion

		#region Implementation Detail
		private string ValidateX12( string sParent, string sField, string sValue )
		{
			System.Text.StringBuilder sError = new System.Text.StringBuilder();
			string sExpectedValue;

			if ( sField == "F96" ) // Segment count in group
			{
				long nValue = Altova.CoreTypes.CastToInt( sValue );
				if ( nValue != mnSegmentCount )
				{
					sError.Append( "Field does not contain the correct number of segments (field value = ");
					sError.Append( sValue);
					sError.Append( ", counted ");
					sError.Append( mnSegmentCount);
					sError.Append( " ).");
				}
			}

			else if ( sField == "F97" ) // Message count in group
			{
				if ( msCurrentSegment == "GE" )
				{
					long nValue = Altova.CoreTypes.CastToInt( sValue );
					if ( nValue != mnMessageCount )
					{
						sError.Append( "Field does not contain the correct number of messages (field value = ");
						sError.Append( sValue);
						sError.Append( ", counted ");
						sError.Append(mnMessageCount);
						sError.Append( " ).");
					}
				}
			}

			else if ( sField == "F28" )
			{
				if ( msCurrentSegment == "GS" || msCurrentSegment == "GE" )
				{
					sExpectedValue = ValidateEquality( sField, sValue );
					if ( sExpectedValue != "" )
						sError.AppendFormat("Field does not contain the same value as Interchange/Group/GS/F28 ('{0}' instead of '{1}').", sValue, sExpectedValue);
				}
			}

			else if ( sField == "F329" )
			{
				if ( msCurrentSegment == "ST" || msCurrentSegment == "SE" )
				{
					sExpectedValue = ValidateEquality( sField, sValue );
					if ( sExpectedValue != "" )
						sError.AppendFormat( "Field does not contain the same value as Interchange/Group/Message/ST/F329 ('{0}' instead of '{1}').", sValue, sExpectedValue);
				}
			}

			else if ( sField == "F143" ) // message type
			{
				if ( msCurrentSegment == "ST" )
					if ( msCurrentMessageType != null && msCurrentMessageType.Substring(0, 3) != sValue )
					{
						sError.AppendFormat("'{0}' is not a correct message type specifier.", sValue);
					}
			}


			else if ( sField == "FI16" ) // Group count in interchange
			{
				long nValue = Altova.CoreTypes.CastToInt( sValue );
				if ( nValue != mnGroupCount )
				{
					sError.AppendFormat("Field does not contain the correct number of function groups (field value = {0}, counted {1} ).", sValue, mnGroupCount);
				}
			}

			else if ( sField == "FI12" && ( msCurrentSegment == "ISA" || msCurrentSegment == "IEA" ) )
			{
				sExpectedValue = ValidateEquality( sField, sValue );
				if ( sExpectedValue != "" )
					sError.AppendFormat( "Field does not contain the same value as Interchange/Group/Message/ISA/FI12 ('{0}' instead of '{1}').", sValue, sExpectedValue);
			}

			return sError.ToString();
		}

		private string ValidateEDIFACT( string sParent, string sField, string sValue )
		{
			System.Text.StringBuilder sError = new System.Text.StringBuilder();
			string sExpectedValue;

			if ( sParent == "S302" )
			{
				if ( msCurrentSegment == "UIB" )
				{
					if ( sField == "F0300" )
					{
						mMapEquality[sField] = sValue;
						mMapEquality["F0303"] = "";
						mMapEquality["F0051"] = "";
						mMapEquality["F0304"] = "";
					}

					else if ( sField == "F0303" )
					{
						mMapEquality[sField] = sValue;
					}

					else if ( sField == "F0051" )
					{
						mMapEquality[sField] = sValue;
					}

					else if ( sField == "F0304" )
					{
						mMapEquality[sField] = sValue;
					}
				}
				else if ( msCurrentSegment == "UIH" || msCurrentSegment == "UIZ" )
				{
					if ( mMapEquality.ContainsKey("F0300") )
					{
						if ( sField == "F0300" )
						{
							if ( (string)mMapEquality[sField] != sValue )
							{
								sError.AppendFormat( "Field does not contain the same value as Interchange/UIB/S302/F0300 ('{0}' instead of '{1}').", sValue, mMapEquality[sField] );
							}
						}

						else if ( sField == "F0303" )
						{
							if ( (string)mMapEquality[sField] != sValue )
							{
								sError.AppendFormat( "Field does not contain the same value as Interchange/UIB/S302/F0303 ('{0}' instead of '{1}').", sValue, mMapEquality[sField] );
							}
						}

						else if ( sField == "F0051" )
						{
							if ( (string)mMapEquality[sField] != sValue )
							{
								sError.AppendFormat( "Field does not contain the same value as Interchange/UIB/S302/F0051 ('{0}' instead of '{1}').", sValue, mMapEquality[sField] );
							}
						}

						else if ( sField == "F0304" )
						{
							if ( (string)mMapEquality[sField] != sValue )
							{
								sError.AppendFormat( "Field does not contain the same value as Interchange/UIB/S302/F0304 ('{0}' instead of '{1}').", sValue, mMapEquality[sField] );
							}
						}
					}
				}
			}

			else if ( sField == "F0074" && (msCurrentSegment == "UNT" || msCurrentSegment == "UIT") ) // segment count in message
			{
				long nValue = Altova.CoreTypes.CastToInt( sValue );
				if ( nValue != mnSegmentCount )
				{
					sError.AppendFormat( "Field does not contain the correct number of segments (field value = {0}, counted {1} ).", sValue, mnSegmentCount);
				}
			}

			if ( sField == "F0060" && msCurrentSegment == "UNE" ) // message count in group
			{
				long nValue = Altova.CoreTypes.CastToInt( sValue );
				if ( nValue != mnMessageCount )
				{
					sError.AppendFormat( "Field does not contain the correct number of messages (field value = {0}, counted {1} ).", sValue, mnMessageCount);
				}
			}

			if ( sField == "F0036" && ( msCurrentSegment == "UNZ" || msCurrentSegment == "UIZ" ) ) // message or group count in interchange
			{
				// if we have at least one group, we count groups, otherwise messages
				if ( mnGroupCount > 0) // count groups
				{
					long nValue = Altova.CoreTypes.CastToInt( sValue );
					if ( nValue != mnGroupCount )
					{
						sError.AppendFormat( "Field does not contain the correct number of function group (field value = {0}, counted {1} ).", sValue, mnGroupCount);
					}
				}
				else // count messages
				{
					long nValue = Altova.CoreTypes.CastToInt( sValue );
					if ( nValue != mnMessageCount )
					{
						sError.AppendFormat( "Field does not contain the correct number of messages (field value = {0}, counted {1} ).", sValue, mnMessageCount);
					}
				}
			}

			else if ( sField == "F0062" && (msCurrentSegment == "UNH" || msCurrentSegment == "UNT") ) // message reference number
			{
				sExpectedValue = ValidateEquality( sField, sValue );
				if ( sExpectedValue != "" )
				{
					sError.AppendFormat( "Field does not contain the same value as Interchange/Group/Message/UNH/F0062 ('{0}' instead of '{1}').", sValue, sExpectedValue);
				}
			}

			else if ( sField == "F0340" && ( msCurrentSegment == "UIH" || msCurrentSegment == "UIT" ) ) // message reference number
			{
				sExpectedValue = ValidateEquality(sField, sValue);
				if ( sExpectedValue != "" )
				{
					sError.AppendFormat( "Field does not contain the same value as Interchange/Message/UIH/F0340 ('{0}' instead of '{1}').", sValue, sExpectedValue );
				}
			}

			else if ( sField == "F0020" && ( msCurrentSegment == "UNZ" || msCurrentSegment == "UNB" ) ) // interchange reference number
			{
				sExpectedValue = ValidateEquality( sField, sValue );
				if ( sExpectedValue != "" )
				{
					sError.AppendFormat( "Field does not contain the same value as Interchange/UNB/F0020 ('{0}' instead of '{1}').", sValue, sExpectedValue);
				}
			}

			else if ( sField == "F0048" ) // group reference number
			{
				sExpectedValue = ValidateEquality( sField, sValue );
				if ( sExpectedValue != "" )
				{
					sError.AppendFormat( "Field does not contain the same value as Interchange/Group/UNG/F0048 ('{0}' instead of '{1}').", sValue, sExpectedValue);
				}
			}

			else if ( sField == "F0052" ) // version
			{
				if (sValue != mSettings.Version )
				{
					sError.AppendFormat( "Field value '{0}' does not match expected message version number ('{1}')", sValue, mSettings.Version);
				}
			}

			else if ( sField == "F0054" ) // release
			{
				if (sValue != mSettings.Release )
				{
					sError.AppendFormat( "Field value '{0}' does not match expected release number ('{1}')", sValue, mSettings.Release);
				}
			}

/*			else if ( sField == "F0051" ) // agency
			{
				if (sValue != mSettings.ControllingAgency )
				{
					sError.AppendFormat( "Field value '{0}' does not match expected control agency code ('{1}')", sValue, mSettings.ControllingAgency);
				}
			}
*/
			else if ( sField == "F0065" && ( msCurrentSegment == "UNH" || msCurrentSegment == "UIH" ) ) // message type
			{
				if ( msCurrentMessageType != null && msCurrentMessageType != sValue )
					sError.AppendFormat( "'{0}' is not a correct message type specifier.", sValue);
			}

			else if ( sField == "F0017" ) // date
			{
				if ( !EDIDateTimeHelpers.IsDateCorrect( sValue ) )
				{
					sError.AppendFormat( "'{0}' is not an EDI formatted date value.", sValue);
				}
			}

			else if ( sField == "F0019" ) // time
			{
				if ( !EDIDateTimeHelpers.IsTimeCorrect( sValue ) )
				{
					sError.AppendFormat( "'{0}' is not an EDI formatted time value.", sValue);
				}
			}

			return sError.ToString();
		}

		private string ValidateSCRIPT( string sParent, string sField, string sValue )
		{
			System.Text.StringBuilder sError = new System.Text.StringBuilder();
			string sExpectedValue;

			if ( sParent == "S300" )
			{
				if ( sField == "F0017" ) // date
				{
					if ( !EDIDateTimeHelpers.IsDateCorrect( sValue ) )
					{
						sError.AppendFormat( "'{0}' is not an EDI formatted date value.", sValue);
					}
				}

				else if ( sField == "F0114" || sField == "F0314" ) // time
				{
					if ( !EDIDateTimeHelpers.IsTimeCorrect( sValue ) )
					{
						sError.AppendFormat( "'{0}' is not an EDI formatted time value.", sValue);
					}
				}
			}
			else if ( msCurrentSegment == "UIB" )
			{
				if ( sField == "F0001" )
				{
					if (sValue != mSettings.ControllingAgency + ((EDIScriptSettings)mSettings).SyntaxLevel )
					{
						sError.AppendFormat( "Field value '{0}' does not match expected syntax identifier ('{1}')", sValue, mSettings.ControllingAgency + ((EDIScriptSettings)mSettings).SyntaxLevel);
					}
				}
				else if ( sField == "F0002" )
				{
					if (sValue != ((EDIScriptSettings)mSettings).SyntaxVersionNumber.ToString() )
					{
						sError.AppendFormat( "Field value '{0}' does not match expected syntax version number ('{1}')", sValue, ((EDIScriptSettings)mSettings).SyntaxVersionNumber);
					}
				}
			}
			else if ( msCurrentSegment == "UIH" )
			{
				if ( sField == "F0062" )
				{
					mMapEquality[sField] = sValue;
				}
				else if ( sParent == "S306" )
				{
					if ( sField == "F0329" )
					{
						if ( sValue != "SCRIPT" )
						{
							sError.AppendFormat( "Field value '{0}' does not match expected message type specifier ('SCRIPT')", sValue);
						}
					}
					else if ( sField == "F0316" )
					{
						if (sValue != mSettings.Version )
						{
							sError.AppendFormat( "Field value '{0}' does not match expected version number ('{1}')", sValue, mSettings.Version);
						}
					}
					else if ( sField == "F0318" )
					{
						if (sValue != mSettings.Release )
						{
							sError.AppendFormat( "Field value '{0}' does not match expected release number ('{1}')", sValue, mSettings.Release);
						}
					}
					else if ( sField == "F0326" )
					{
						if ( msCurrentMessageType != null && msCurrentMessageType != sValue )
						{
							sError.AppendFormat( "'{0}' is not a correct message function specifier.", sValue);
						}
					}
				}
			}
			else if ( msCurrentSegment == "UIT" )
			{
				if ( sField == "F0062" )
				{
					sExpectedValue = ValidateEquality(sField, sValue);
					if ( sExpectedValue != "" )
					{
						sError.AppendFormat( "Field does not contain the same value as Interchange/Message/UIH/F0062 ('{0}' instead of '{1}').", sValue, sExpectedValue );
					}
				}
				if ( sField == "F0074" ) // segment count in message
				{
					long nValue = Altova.CoreTypes.CastToInt( sValue );
					if ( nValue != mnSegmentCount )
					{
						sError.AppendFormat( "Field does not contain the correct number of segments (field value = {0}, counted {1} ).", sValue, mnSegmentCount);
					}
				}
			}
			else if ( msCurrentSegment == "UIZ" )
			{
				if ( sField == "F0036" ) // message or group count in interchange
				{
					long nValue = Altova.CoreTypes.CastToInt( sValue );
					if ( nValue != mnMessageCount )
					{
						sError.AppendFormat( "Field does not contain the correct number of messages (field value = {0}, counted {1} ).", sValue, mnMessageCount);
					}
				}
			}

			return sError.ToString();
		}

		private string ValidateTRADACOMS( string sParent, string sField, string sValue )
		{
			System.Text.StringBuilder sError = new System.Text.StringBuilder();
			string sExpectedValue;
		
			if ( msCurrentSegment == "STX" )
			{
				if ( sField == "SNRF" )
				{
					mMapEquality[sField] = sValue;
				}
				else if ( sField == "UNTO-1" )
				{
					mMapEquality[sField] = sValue;
				}
			}
			else if ( msCurrentSegment == "MHD" )
			{
				if ( sField == "MSRF" )
				{
					long nValue = Altova.CoreTypes.CastToInt( sValue.Replace(mSettings.ServiceChars.DecimalSeparator, '.') );
					if ( nValue != mnMessageCount )
					{
						sError.AppendFormat( "Field does not contain the correct messages reference number (field value = {0}, expected {1} ).", sValue, mnMessageCount );
					}
				}
			}
			else if ( msCurrentSegment == "MTR" )
			{
				if ( sField == "NOSG" )
				{
					long nValue = Altova.CoreTypes.CastToInt( sValue );
					if ( nValue != mnSegmentCount )
					{
						sError.AppendFormat( "Field does not contain the correct number of segments (field value = {0}, counted {1} ).", sValue, mnSegmentCount );
					}
				}
			}
			else if ( msCurrentSegment == "EOB" )
			{
				if ( sField == "NOLI" )
				{
					long nValue = Altova.CoreTypes.CastToInt( sValue );
					if ( nValue != mnGroupCount )
					{
						sError.AppendFormat( "Field does not contain the correct number of messages (field value = {0}, counted {1} ).", sValue, mnGroupCount );
					}
				}
			}
			else if ( msCurrentSegment == "RSG" )
			{
				if ( sField == "RSGA" )
				{
					if ( !mMapEquality.ContainsKey( "SNRF" ) )
					{
						sError.AppendFormat( "Field does not contain the same value as Interchange/STX/SNRF ('{0}' instead of '').", sValue );
					}
					sExpectedValue = ValidateEquality( "SNRF", sValue );
					if ( sExpectedValue != "" )
					{
						sError.AppendFormat( "Field does not contain the same value as Interchange/STX/SNRF ('{0}' instead of '{1}').", sValue, sExpectedValue );
					}
				}
				else if ( sField == "RSGB" )
				{
					if ( !mMapEquality.ContainsKey( "UNTO-1" ) )
					{
						sError.AppendFormat( "Field does not contain the same value as Interchange/STX/UNTO/UNTO-1 ('{0}' instead of '').", sValue );
					}
					sExpectedValue = ValidateEquality( "UNTO-1", sValue );
					if ( sExpectedValue != "" )
					{
						sError.AppendFormat( "Field does not contain the same value as Interchange/STX/UNTO/UNTO-1 ('{0}' instead of '{1}').", sValue, sExpectedValue );
					}
				}
			}
			else if ( msCurrentSegment == "END" )
			{
				if ( sField == "NMST" )
				{
					long nValue = Altova.CoreTypes.CastToInt( sValue );
					if ( nValue != mnMessageCount )
					{
						sError.AppendFormat( "Field does not contain the correct number of messages (field value = {0}, counted {1} ).", sValue, mnMessageCount );
					}
				}
			}

			return sError.ToString();
		}

		private string ValidateHL7( string sParent, string sField, string sValue )
		{
		
			return "";
		}

		string ValidateEquality( string sField, string sValue )
		{
			// if has no field, add it; if map has field compare it and remove it
			if (!mMapEquality.ContainsKey(sField)) // map has no field?
			{
				mMapEquality[sField] = sValue;
				return "";
			}

			// obviously map has the field
			string ret = "";
			string fieldValue = (string)mMapEquality[sField];
			if (fieldValue != sValue)
				ret = fieldValue;
			mMapEquality.Remove(sField);
			return ret;
		}
		#endregion
	}
}