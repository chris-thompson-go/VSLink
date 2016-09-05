using ScintillaNET;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace VsLink
{
	// TODO: Weird corner case if two vslink files point to the same content
	public partial class LinkEditor : UserControl
	{
		public LinkEditor()
		{
			InitializeComponent();
		}

		public new event EventHandler TextChanged
		{
			add { scintilla.TextChanged += value; }
			remove { scintilla.TextChanged -= value; }
		}

		private bool m_TxtLinkFocused = false;

		public bool CanPaste
		{
			get
			{
				if (m_TxtLinkFocused)
					return txtLink.CanPaste;
				else
					return scintilla.CanPaste;
			}
		}

		public bool CanRedo
		{
			get
			{
				if (m_TxtLinkFocused)
					return txtLink.CanRedo;
				else
					return scintilla.CanRedo;
			}
		}

		public bool CanUndo
		{
			get
			{
				if (m_TxtLinkFocused)
					return txtLink.CanUndo;
				else
					return scintilla.CanUndo;
			}
		}

		public string SelectedText { get { return scintilla.SelectedText; } }

		public override string Text { get { return scintilla.Text; } set { scintilla.Text = Text; } }

		public void Copy()
		{
			if (m_TxtLinkFocused)
				txtLink.Copy();
			else
				scintilla.Copy();
		}

		public void Cut()
		{
			if (m_TxtLinkFocused)
				txtLink.Cut();
			else
				scintilla.Cut();
		}

		public bool IsValidFilename(string filename)
		{
			char[] invalidFilenameChars = Path.GetInvalidFileNameChars();
			foreach (char c in invalidFilenameChars)
			{
				if (filename.Contains(c))
					return false;
			}
			return true;
		}

		public bool IsValidPath(string path)
		{
			char[] invalidPathChars = Path.GetInvalidPathChars();
			foreach (char c in invalidPathChars)
			{
				if (path.Contains(c))
					return false;
			}
			return true;
		}

		public void LoadFile(string vslink)
		{
			try
			{
				// TODO: If this is ever called twice I might need to tweak the TextChanged event handlers
				m_VsLinkFile = new FileInfo(vslink);
				if (!(m_VsLinkFile.Exists))
				{
					txtLink.Text = "";
					scintilla.Text = "vslink not found";  // Note: Should never happen
					txtLink.Select();
					m_TxtLinkFocused = true;
					return;
				}
				string link = GetLink();
				if (link == null || link.Trim().Length == 0)
				{
					txtLink.Text = "";
					NotifyLinkError();
					string content = GetVsLinkContents();
					if (content == null || content.Trim().Length == 0)
					{
						scintilla.Text = "Specify target";
						txtLink.Select();
						m_TxtLinkFocused = true;
					}
					else
					{
						scintilla.Select();
						m_TxtLinkFocused = false;
					}
				}
				else
				{
					txtLink.Text = link;
					if (LoadContent(link))
					{
						scintilla.Select();
						m_TxtLinkFocused = false;
					}
					else
					{
						scintilla.Text = "Specify target";
						txtLink.Select();
						m_TxtLinkFocused = true;
					}
				}
				Initialize();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		public void Paste()
		{
			if (m_TxtLinkFocused)
				txtLink.Paste();
			else
				scintilla.Paste();
		}

		public void Redo()
		{
			scintilla.Redo();
		}

		public void SaveFile(string filename)
		{
			try
			{
				if (m_LinkChanged)
				{
					Debug.WriteLine($"StreamWrite {filename}");
					using (StreamWriter sw = new StreamWriter(filename))
					{
						if (txtLink.Text.Length > 0)
							sw.Write(txtLink.Text);
						else
							sw.Write(scintilla.Text);
					}
					m_LinkChanged = false;
				}
				if (m_ContentChanged)
				{
					string link = GetLink();
					if (link != null)
					{
						FileInfo location = GetLinkLocation(link);
						// TODO: This could use some improvement because it is redundant
						if (location == null)
						{
							try
							{
								string fullPath = GetFullPath(link);
								Debug.WriteLine($"StreamWrite {fullPath}");
								using (StreamWriter sw = new StreamWriter(fullPath))
								{
									sw.Write(scintilla.Text);
								}
								NotifyLinkSuccess();
							}
							catch (Exception ex)
							{
								NotifyLinkError();
								Debug.WriteLine(ex.Message);
							}
						}
						if (location != null)
						{
							try
							{
								Debug.WriteLine($"StreamWrite {location.FullName}");
								using (StreamWriter sw = new StreamWriter(location.FullName))
								{
									sw.Write(scintilla.Text);
								}
							}
							catch (Exception ex)
							{
								NotifyLinkError();
								Debug.WriteLine(ex.Message);
							}

						}
					}
					m_ContentChanged = false;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		public void SelectAll()
		{
			scintilla.SelectAll();
		}

		public void Undo()
		{
			if (m_TxtLinkFocused)
				txtLink.Undo();
			else
				scintilla.Undo();
		}

		private bool m_ContentChanged;

		private bool m_Initialized;

		private bool m_LinkChanged;

		private FileInfo m_VsLinkFile;
		private string GetLink()
		{
			string link;
			Debug.WriteLine($"StreamRead {m_VsLinkFile.FullName}");
			using (StreamReader srVsLink = new StreamReader(m_VsLinkFile.FullName))
			{
				while (!srVsLink.EndOfStream)
				{
					link = srVsLink.ReadLine();
					if (link != null)
					{
						link = link.Trim();
						if (link.Length > 0 && IsValidPath(link))
						{
							return link;
						}
					}
					link = null;
				}
			}
			return null;
		}

		private string GetFullPath(string link)
		{
			try
			{
				if (link != null && link.Trim().Length > 0 && IsValidPath(link))
					return Path.GetFullPath(Path.Combine(m_VsLinkFile.Directory.FullName, link));
				else
					return null;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return null;
			}
		}

		private FileInfo GetLinkLocation(string link)
		{
			FileInfo linkLocation = null;
			if (link != null)
			{
				link = link.Trim();
				if (link.Length > 0)
				{
					if (IsValidPath(link))
					{
						try
						{
							linkLocation = new FileInfo(GetFullPath(link));
							if (linkLocation.Exists)
								return linkLocation;
						}
						catch (Exception ex)
						{
							Debug.WriteLine(ex.Message);
							return null;
						}
					}
				}
			}
			return null;
		}

		private string GetVsLinkContents()
		{
			Debug.WriteLine($"StreamRead {m_VsLinkFile.FullName}");
			using (StreamReader srVsLink = new StreamReader(m_VsLinkFile.FullName))
			{
				return srVsLink.ReadToEnd();
			}
		}

		private void Initialize()
		{
			if (!m_Initialized)
			{
				scintilla.HScrollBar = true;
				scintilla.VScrollBar = true;

				scintilla.TextChanged += Scintilla_TextChanged;
				scintilla.Enter += scintilla_Enter;
				txtLink.TextChanged += txtLink_TextChanged;
				txtLink.Enter += txtLink_Enter;
				m_Initialized = true;
			}
		}

		private void scintilla_Enter(object sender, EventArgs e)
		{
			m_TxtLinkFocused = false;
			if (scintilla.Text == "Specify target" || scintilla.Text == "Target not found")
				scintilla.Text = "";
		}

		private void txtLink_Enter(object sender, EventArgs e)
		{
			m_TxtLinkFocused = true;
		}

		private bool LoadContent(string link)
		{
			bool isSuccess = false;
			if (link != null && link.Trim().Length != 0)
			{
				FileInfo linkLocation = GetLinkLocation(link);
				if (linkLocation != null)
				{
					Debug.WriteLine($"StreamRead {linkLocation.FullName}");
					using (StreamReader srText = new StreamReader(linkLocation.FullName))
					{
						scintilla.Text = srText.ReadToEnd();
					}
					SetStyle(linkLocation);
					isSuccess = true;
				}
				else
				{
					if (scintilla.Text != "Specify target")
						scintilla.Text = "Target not found";
					NotifyLinkError();
				}
			}
			m_ContentChanged = false;
			return isSuccess;
		}

		private void NotifyLinkError()
		{
			txtLink.SetBorderColor(Color.Red);
		}

		private void NotifyLinkSuccess()
		{
			txtLink.SetBorderColor(SystemColors.ControlDark);
		}

		private void Scintilla_TextChanged(object sender, EventArgs e)
		{
			m_ContentChanged = true;
		}

		private void SetStyle(Lexer lexer)
		{
			// TODO: Do I need to do a better job of resetting if the lexer switches?
			scintilla.Lexer = lexer;
			if (lexer == Lexer.Properties)
			{
				scintilla.Styles[ScintillaNET.Style.Properties.Default].Bold = true;
				scintilla.Styles[ScintillaNET.Style.Properties.Default].Weight = 700;
				scintilla.Styles[ScintillaNET.Style.Properties.Comment].ForeColor = Color.ForestGreen;
				scintilla.Styles[ScintillaNET.Style.Properties.Comment].Bold = false;
				scintilla.Styles[ScintillaNET.Style.Properties.Section].ForeColor = Color.LightSeaGreen;
				scintilla.Styles[ScintillaNET.Style.Properties.Section].Bold = false;
				scintilla.Styles[ScintillaNET.Style.Properties.Assignment].ForeColor = Color.Firebrick;
				scintilla.Styles[ScintillaNET.Style.Properties.Assignment].Bold = false;
				scintilla.Styles[ScintillaNET.Style.Properties.DefVal].Bold = true;
				scintilla.Styles[ScintillaNET.Style.Properties.DefVal].Weight = 700;
				scintilla.Styles[ScintillaNET.Style.Properties.Key].ForeColor = Color.RoyalBlue;
				scintilla.Styles[ScintillaNET.Style.Properties.Key].Bold = false;
			}
			else if (lexer == Lexer.Sql)
			{
				// Set the Styles
				scintilla.Styles[Style.Sql.Comment].ForeColor = Color.Green;
				scintilla.Styles[Style.Sql.CommentLine].ForeColor = Color.Green;
				scintilla.Styles[Style.Sql.CommentLineDoc].ForeColor = Color.Green;
				scintilla.Styles[Style.Sql.Number].ForeColor = Color.Maroon;
				scintilla.Styles[Style.Sql.Word].ForeColor = Color.Blue;
				scintilla.Styles[Style.Sql.Word2].ForeColor = Color.Fuchsia;
				scintilla.Styles[Style.Sql.User1].ForeColor = Color.Gray;
				scintilla.Styles[Style.Sql.User2].ForeColor = Color.FromArgb(255, 00, 128, 192);    //Medium Blue-Green
				scintilla.Styles[Style.Sql.String].ForeColor = Color.Red;
				scintilla.Styles[Style.Sql.Character].ForeColor = Color.Red;
				scintilla.Styles[Style.Sql.Operator].ForeColor = Color.Black;

				// Set keyword lists
				// Word = 0
				scintilla.SetKeywords(0, @"add alter as authorization backup begin bigint binary bit break browse bulk by cascade case catch check checkpoint close clustered column commit compute constraint containstable continue create current cursor cursor database date datetime datetime2 datetimeoffset dbcc deallocate decimal declare default delete deny desc disk distinct distributed double drop dump else end errlvl escape except exec execute exit external fetch file fillfactor float for foreign freetext freetexttable from full function goto grant group having hierarchyid holdlock identity identity_insert identitycol if image index insert int intersect into key kill lineno load merge money national nchar nocheck nocount nolock nonclustered ntext numeric nvarchar of off offsets on open opendatasource openquery openrowset openxml option order over percent plan precision primary print proc procedure public raiserror read readtext real reconfigure references replication restore restrict return revert revoke rollback rowcount rowguidcol rule save schema securityaudit select set setuser shutdown smalldatetime smallint smallmoney sql_variant statistics table table tablesample text textsize then time timestamp tinyint to top tran transaction trigger truncate try union unique uniqueidentifier update updatetext use user values varbinary varchar varying view waitfor when where while with writetext xml ");
				// Word2 = 1
				scintilla.SetKeywords(1, @"ascii cast char coalesce collate contains convert current_date current_time current_timestamp current_user isnull nullif object_id session_user system_user tsequal ");
				// User1 = 4
				scintilla.SetKeywords(4, @"all and any between cross exists in inner is join left like not null or outer pivot right some unpivot ( ) * ");
				// User2 = 5
				scintilla.SetKeywords(5, @"sys objects sysobjects ");
			}
		}

		private void SetStyle(string link)
		{
			try
			{
				FileInfo f = new FileInfo(GetFullPath(link));
				SetStyle(f);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		private void SetStyle(FileInfo f)
		{
			try
			{
				string ext = f.Extension;
				if (String.Compare(ext, ".ini", true) == 0)
					SetStyle(Lexer.Properties);
				else if (String.Compare(ext, ".sql", true) == 0)
					SetStyle(Lexer.Sql);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		private void txtLink_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (IsValidPath(txtLink.Text))
				{
					if (LoadContent(txtLink.Text))
						NotifyLinkSuccess();
					else
					{
						NotifyLinkError();
						SetStyle(txtLink.Text);
					}
				}
				else
				{
					NotifyLinkError();
					SetStyle(txtLink.Text);
				}
				m_LinkChanged = true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

	}
}