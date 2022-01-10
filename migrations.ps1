param(
    [Parameter(Mandatory = $True)]
    [ValidateSet('AddInitial', 'RemoveAll', 'Reinitialize')]
    [string]$Action
)

$projects = @(
'Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.Postgres'

'Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.LocalDB'
'Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.Postgres'
)

foreach ($project in $projects)
{
    Push-Location $project

    $exists = Test-Path 'Migrations'
    if (($Action -eq 'RemoveAll' -or $Action -eq 'Reinitialize') -and $exists)
    {
        Remove-Item 'Migrations' -Recurse
    }
    
    if (($Action -eq 'AddInitial' -and (-not $exists)) -or $Action -eq 'Reinitialize')
    {
        dotnet.exe ef migrations add Initial
    }

    Pop-Location
}



