namespace Demo.Files.FilesManagement.Presentation.Common;

public sealed class FilesManagementPersistenceOptions
{
	public string? Server { get; set; }
	public uint? Port { get; set; }
	public string? Database { get; set; }
	public string? Username { get; set; }
	public string? Password { get; set; }
}