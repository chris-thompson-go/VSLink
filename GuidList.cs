/*  Most of this code was copied from the Editor_With_Toolbox Microsoft Visual Studio Extensions sample
	located at https://github.com/Microsoft/VSSDK-Extensibility-Samples.
*/

// TODO: Get rid of this

using System;

namespace VsLink
{
	/// <summary>
	/// This class contains a list of GUIDs specific to this sample,
	/// especially the package GUID and the commands group GUID.
	/// </summary>
	public static class GuidStrings
	{
		public const string GuidClientCmdSet = "9A48CC99-E5DF-41E9-A6A7-C0383F116E2B";
		public const string GuidClientPackage = "5924FB6D-A86D-470C-96B4-52D584E566B2";
		public const string GuidEditorFactory = "29AAEA4D-96C1-4D95-B011-91828CB0F05B";
	}

	/// <summary>
	/// List of the GUID objects.
	/// </summary>
	internal static class GuidList
	{
		public static readonly Guid guidEditorCmdSet = new Guid(GuidStrings.GuidClientCmdSet);
		public static readonly Guid guidEditorFactory = new Guid(GuidStrings.GuidEditorFactory);
	};
}