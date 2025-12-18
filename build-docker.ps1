<#
Simple helper script to build the AnalysisOfAspose Docker image with correct context.
Usage:
  .\build-docker.ps1                    # builds with default tag 'analysis-of-aspose'
  .\build-docker.ps1 -Tag my-image:latest
#>
param(
    [string]$Tag = 'analysis-of-aspose'
)

Write-Host "Building Docker image for AnalysisOfAspose with tag: $Tag"
# Ensure we run the build from repo root so Docker context contains sibling projects
$RepoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $RepoRoot
Write-Host "Current directory (build context):" (Get-Location)

$Dockerfile = "AnalysisOfAspose\Dockerfile"
if (-not (Test-Path $Dockerfile)) {
    Write-Error "Dockerfile not found at $Dockerfile. Run this script from the repo root or adjust the path."
    exit 1
}

$cmd = "docker build -f $Dockerfile -t $Tag ."
Write-Host "Running: $cmd"
$proc = Start-Process -FilePath docker -ArgumentList @('build','-f',$Dockerfile,'-t',$Tag,'.') -NoNewWindow -Wait -PassThru
if ($proc.ExitCode -ne 0) {
    Write-Error "Docker build failed with exit code $($proc.ExitCode). See output above for errors."
    Write-Host "Common fixes: run Docker Desktop, ensure file sharing/access to the repository path, and run the script from a folder with full read access."
    exit $proc.ExitCode
}

Write-Host "Docker build succeeded: $Tag" -ForegroundColor Green

