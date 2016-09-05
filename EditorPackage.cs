/*  Most of this code was copied from the Editor_With_Toolbox Microsoft Visual Studio Extensions sample
	located at https://github.com/Microsoft/VSSDK-Extensibility-Samples.
*/

using Microsoft.VisualStudio.Shell;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace VsLink
{
	/// <summary>
	/// This class implements Visual studio package that is registered within Visual Studio IDE.
	/// The EditorPackage class uses a number of registration attribute to specify integration parameters.
	/// </summary>
	/// <remarks>
	/// <para>A description of the different attributes used here is given below:</para>
	/// <para>PackageRegistration: Used to determine if the package registration tool should look for additional
	///                      attributes. We currently specify that the package commands are described in a
	///                      managed package and not in a separate satellite UI binary.</para>
	/// <para>ProvideMenuResource: Provides information about menu resources.
	///     We specify ResourceId=1000 and version=1.</para>
	/// <para>ProvideEditorLogicalView: Indicates that our editor supports LOGVIEWID_Designer logical view and
	///     registers EditorFactory class to manage this view.</para>
	/// <para>ProvideEditorExtension: With this attribute we register our AddNewItem Templates
	///     for specified project types.</para>
	/// </remarks>

	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#100", "#102", "10.0", IconResourceID = 400)]
	// We register our AddNewItem Templates the Miscellaneous Files Project:
	[ProvideEditorExtension(typeof(EditorFactory), ".vslink", 32,
			  ProjectGuid = "{AD2DA5B6-6B0A-4BD8-BAE4-804590784AAF}",
			  TemplateDir = "Templates",
			  NameResourceID = 106)]
	// We register that our editor supports LOGVIEWID_Designer logical view
	[ProvideEditorLogicalView(typeof(EditorFactory), "{D2514416-22B5-4BF9-B7B2-AE8B07EA2DF4}")]
	[Guid(GuidStrings.GuidClientPackage)]
	public class EditorPackage : Package, IDisposable
	{
		public EditorPackage()
		{
		}

		protected override void Initialize()
		{
			base.Initialize();
			editorFactory = new EditorFactory();
			RegisterEditorFactory(editorFactory);
		}

		private EditorFactory editorFactory;

		#region IDisposable Pattern

		/// <summary>
		/// Releases the resources used by the Package object.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		/// Releases the resources used by the Package object.
		/// </summary>
		/// <param name="disposing">This parameter determines whether the method has been called directly or indirectly by a user's code.</param>
		protected override void Dispose(bool disposing)
		{
			try
			{
				Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Dispose() of: {0}", ToString()));
				if (disposing)
				{
					if (editorFactory != null)
					{
						editorFactory.Dispose();
						editorFactory = null;
					}
					GC.SuppressFinalize(this);
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		#endregion IDisposable Pattern
	}
}