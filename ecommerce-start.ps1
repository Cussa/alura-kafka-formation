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

function StartService($service, $NewWindow = $True, $arguments)
{
	if (CheckDockerIsRunning)
	{
		$path = '.\src\{0}\bin\Debug\netcoreapp3.1\{0}.exe' -f $service
		
		if ($NewWindow)
		{
			Write-Host "$service will execute in new window"
			if ($arguments)
			{
				Start-Process $path -ArgumentList $arguments
			}
			else 
			{
				Start-Process $path
			}
		}
		else 
		{
			invoke-expression $path
		}
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
	Write-Host "6`tStart User Service"
	Write-Host "7`tStart Batch User Service"
	Write-Host "8`tStart Reading Report Service"
	Write-Host "9`tStart Email New Order Service"
	Write-Host "88`tDelete Users table"
	Write-Host "99`tStop Kafka Docker"
	Write-Host "cls`tClear the console"
	$ValueInput = Read-Host -Prompt "What do you wanna do"

	switch($ValueInput)
	{
		{$_ -eq "1"} { docker-compose up -d }
		{$_ -eq "99"} { docker-compose down }
		{$_ -eq "2"} { StartService "Ecommerce.Service.NewOrder" -NewWindow $False }
		{$_ -eq "3"} { StartService "Ecommerce.Service.Log" }
		{$_ -eq "4"} { StartService "Ecommerce.Service.Email" }
		{$_ -eq "5"} { StartService "Ecommerce.Service.FraudDetector" }
		{$_ -eq "6"} { StartService "Ecommerce.Service.Users" -Arguments "1" }
		{$_ -eq "7"} { StartService "Ecommerce.Service.Users" -Arguments "2" }
		{$_ -eq "8"} { StartService "Ecommerce.Service.ReadingReport" }
		{$_ -eq "9"} { StartService "Ecommerce.Service.EmailNewOrder" }
		{$_ -eq "88"} { DeleteUserServiceDatabase }
		{$_ -eq "cls"} { cls }
		{$_ -eq "build"} {
			dotnet build
			cls
		}
		default { continue }
	}
}
