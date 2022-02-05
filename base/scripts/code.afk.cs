function zadmin::AFKDaemon()
{
	if (!$zadmin::pref::afk::enabled)
		return;
	%now = getSimTime();
	$zadmin::AFKDaemonTimestamp = %now;
	if (!$Server::TourneyMode)
	{
		%floor = ($accessLevel::Count-1);
		if (%floor<1)
			%floor = 1;

		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			%idletime = %now - %cl.lastActiveTimestamp;
			echo("zadmin::AFKDaemon Clients: " @ %cl @ " " @ Client::GetName(%cl) @ " idletime:" @ %idletime @ " al: " @ %cl.adminLevel);			
			
			if (%idletime >= $zadmin::pref::afk::timelimit && Client::GetTeam(%cl) != -1)
			{
				%exempt = false;
				//%exempt = %exempt || (%cl.adminLevel >= %floor);
				//%exempt = %exempt || ((%cl.adminLevel > 0) && (Client::GetTeam(%cl) == -1));
				%exempt = %exempt || (Client::GetTeam(%cl) == -1);
				
				if (!%exempt)
					//Net::kick(%cl, "If you are going to go AFK, please exit the server.");
					Observer::enterObserverMode(%cl);
					messageAll(0, Client::getName(%cl) @ " has been idle for too long." );
			}
		}
	}
	
	schedule("zadmin::AFKDaemon();", $zadmin::pref::afk::monitorInterval);
}


function zadmin::AFKStatus()
{
	echo("zadmin::AFKStatus [ $zadmin::pref::afk::enabled: ", $zadmin::pref::afk::enabled, " $zadmin::pref::afk::timelimit: ", $zadmin::pref::afk::timelimit, "$zadmin::pref::afk::monitorInterval :", $zadmin::pref::afk::monitorInterval, " ]");				
	if (!$Server::TourneyMode)
	{
		%now = getSimTime();
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			%idletime = %now - %cl.lastActiveTimestamp;
			echo("zadmin::AFKStatus Clients: " @ %cl @ " " @ Client::GetName(%cl) @ " idletime:" @ %idletime @ " al: " @ %cl.adminLevel);
		}
		
		if($zadmin::pref::afk::enabled)
		{		
			if($zadmin::AFKDaemonTimestamp == "") 
				$zadmin::AFKDaemonTimestamp = %now;
			echo("Next AFK Check in: " @ $zadmin::pref::afk::monitorInterval - (%now - $zadmin::AFKDaemonTimestamp) @ " seconds ");		
		}
	}
}

