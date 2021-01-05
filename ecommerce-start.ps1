function CheckDockerIsRunning
{ 
	$isRunning = docker container inspect -f '{{.State.Running}}' kafka-formation_kafka_1
	if ($isRunning)	{ return $isRunning	}
	else 
	{
		Write-Error "Kafka docker is not running."
		Write-Host "Use the command `"1`" before trying to execute the command" 
	}
	return $isRunning
}

function StartUserService
{
	$isRunning = ps Ecommerce.Service.User -ErrorAction SilentlyContinue
	if ($isRunning)
	{
		Write-Error "You can only run 1 instance of this service."
	}
	else
	{
		invoke-expression 'cmd /c start .\src\Ecommerce.Service.Users\bin\Debug\netcoreapp3.1\Ecommerce.Service.Users.exe'
	}
}

function DeleteUserServiceDatabase
{
	$isRunning = ps Ecommerce.Service.Users -ErrorAction SilentlyContinue
	if ($isRunning)
	{
		Write-Error "You cannot delete the database while the service is running."
	}
	else
	{
		Remove-Item .\src\Ecommerce.Service.Users\bin\Debug\netcoreapp3.1\users_database.db
		Write-Host "Database deleted with success."
	}
}

dotnet build
cls

while($true)
{
	Write-Host "============================================="
	Write-Host "Choose what do you wanna do:"
	Write-Host "1`tStart Kafka on Docker"
	Write-Host "2`tGenerate 10 orders"
	Write-Host "3`tStart Log Service"
	Write-Host "4`tStart Email Service"
	Write-Host "5`tStart Fraud Detector Service"
	Write-Host "88`tDelete Users table"
	Write-Host "99`tStop Kafka Docker"
	Write-Host "cls`tClear the console"
	$ValueInput = Read-Host -Prompt "What do you wanna do"

	switch($ValueInput)
	{
		{$_ -eq "1"} { docker-compose up -d }
		{$_ -eq "99"} { docker-compose down }
		{$_ -eq "2"} { if (CheckDockerIsRunning -eq $true) { .\src\Ecommerce.Service.NewOrder\bin\Debug\netcoreapp3.1\Ecommerce.Service.NewOrder.exe } }
		{$_ -eq "3"} { invoke-expression 'cmd /c start .\src\Ecommerce.Service.Log\bin\Debug\netcoreapp3.1\Ecommerce.Service.Log.exe'}
		{$_ -eq "4"} { invoke-expression 'cmd /c start .\src\Ecommerce.Service.Email\bin\Debug\netcoreapp3.1\Ecommerce.Service.Email.exe'}
		{$_ -eq "5"} { invoke-expression 'cmd /c start .\src\Ecommerce.Service.FraudDetector\bin\Debug\netcoreapp3.1\Ecommerce.Service.FraudDetector.exe'}
		{$_ -eq "6"} { StartUserService }
		{$_ -eq "88"} { DeleteUserServiceDatabase }
		{$_ -eq "cls"} { cls }
		{$_ -eq "build"} {
			dotnet build
			cls
		}
		default { continue }
	}
}
