# Watches a directory for changes and copies changed .dart, .js, .cshtml, or .css files to an output directory.
# Requires inotifywait for windows (https://github.com/thekid/inotify-win) to be on your path.
# By default directory to watch ($baseDir) is the current directory
# By default directory to copy changed files to ($outDir) is $baseDir\bin\Debug
# Changes to files with \bin\ or \obj\ in their path are ignored so the copy doesn't trigger another copy which triggers another copy, etc.
# This is useful to run when debugging AnimeRecs.Web.
Param(
  [string]$baseDir,
  [string]$outDir
)

if([string]::IsNullOrEmpty($baseDir))
{
  $baseDir = Get-Location
}

if([string]::IsNullOrEmpty($outDir))
{
  $outDir = "$baseDir\bin\Debug"
}

# inotifywait can trigger twice on the same file for one file save. Since this just copies, it's ok, but might want to throttle somehow if doing something more expensive like dart2js
function ProcessChangedFile([string] $path)
{
  # Copy .cshtml, .dart, .css, .js
  # If not one of those, ignore
  if($path.EndsWith(".cshtml") -or $path.EndsWith(".dart") -or $path.EndsWith(".js") -or $path.EndsWith(".css"))
  {
    # Get path relative to base
    # Save cwd to variable so we can restore it
    $currentDir = Get-Location
    # Set cwd to base dir because resolve-path works with cwd to get a relative path
    Set-Location $baseDir
    # Get path relative to base
    $relativePath = Resolve-Path $path -Relative
    # Restore cwd to what it was before
    Set-Location $currentDir
    # Apply relative path to output dir
    $outputPath = "$outDir\$relativePath"
    # Copy
    Write-Output "Copy from $path to $outputPath"
    cp -LiteralPath $path -Force -Destination $outputPath
  }
}

Write-Output "Watching $baseDir, copying to $outDir"
inotifywait -r -m -e "modify,create,move" --format "%w\%f" --excludei "[/\\](bin|obj)[/\\]" $baseDir | % { ProcessChangedFile($_) }