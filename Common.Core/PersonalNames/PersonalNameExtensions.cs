using Common.Core.Diagnostics;

namespace Common.Core.PersonalNames;

public static class PersonalNameExtensions
{
	public static string TagMatronymic => "Matronymic";

	public static PersonalName ChangeMatronymic(this PersonalName @this, string? value)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.ChangeExtraName(TagMatronymic, value);
	}

	public static string? GetMatronymic(this PersonalName @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.GetExtraName(TagMatronymic);
	}

	public static string TagPatronymic => "Patronymic";

	public static PersonalName ChangePatronymic(this PersonalName @this, string? value)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.ChangeExtraName(TagPatronymic, value);
	}

	public static string? GetPatronymic(this PersonalName @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.GetExtraName(TagPatronymic);
	}

	public static string? GetFirstDirectAncestorName(this PersonalName @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.GetPatronymic() ?? @this.GetMatronymic();
	}
}