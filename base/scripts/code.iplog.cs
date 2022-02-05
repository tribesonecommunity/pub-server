function IPLog::setupLogFile()
{
	%suffix = zadmin::getFileTimeStamp();
//
//	// Eric's code :)
//	%serverName = "";
//	for (%i = 0; %i < String::Len($Server::HostName); %i++)
//	{
//		%char = String::getSubStr($Server::hostName, %i, 1);
//		%result = String::iCompare(%char, "z");
//
	//	if((%result >= -42 && %result <= -33) || (%result >= -25 && %result <= 0))
	//		%serverName = %serverName @ %char;
	//	else
	//		%serverName = %serverName @ "_";
	//}

	return "config\\ip.log";
}
      
function IPLog::setw(%text, %width, %trim)
{
   %length = String::Len(%text);
      
   if( %trim && %length > %width ) // truncate if too long
   {
      %text = string::getSubStr( %text, 0, %width - 1 );
      %length = %width - 1;
   }
      
   for( %i = %length; %i < %width; %i++ )
      %text = %text @ " ";
      
   return %text;
}

function IPLog::createEntry(%client)
{
   if( %client != "" && %client > 0 ) // prevent fuckupsssssss
   {
      %date = zadmin::getTimeStamp_Patched();
      IPLog::logAddress(%client, %date);
   }
}

function IPLog::logAddress(%client, %date)
{
   %date    =  IPLog::setw( %date, 22, 0);
   %name    =  IPLog::setw( Client::getName(%client), 22, 1 ); // names are 16 character max, I believe.  truncate if they don't fit
   %ip      =  IPLog::setw( Client::getTransportAddress(%client), 30, 0 ); // IP:xxx.xxx.xxx.xxx:xxxxx. do NOT truncate IPs

   $IPLogEntry = %date @ "Name: " @ %name @ " IP: " @ %ip ;
   
   export("$IPLogEntry", $IPLogFile, true); // append the entries

}


$IPLogFile = IPLog::setupLogFile();