$MsgTypeSystem = 0;
$MsgTypeGame = 1;
$MsgTypeChat = 2;
$MsgTypeTeamChat = 3;
$MsgTypeCommand = 4;

function remoteSay(%clientId, %team, %message)
{

	if(String::containsCrash(%clientId, %message, "remoteSay"))
	{
		return;
	}
	//stop the \n and \t hacks + ~wdeath
	if (%clientId.megaMute || (String::FindSubStr(%message, "\n") != -1) || (String::FindSubStr(%message, "\t") != -1) || (String::FindSubStr(%message, "~wdeath") != -1))
		return;

	%msg = %clientId @ ": " @ escapeString(%message) @ "\"";

	// check for flooding if it's a broadcast OR if it's team in FFA
	if($Server::FloodProtectionEnabled && (!$Server::TourneyMode || !%team))
	{
		// we use getIntTime here because getSimTime gets reset.
		// time is measured in 32 ms chunks... so approx 32 to the sec
		%time = getIntegerTime(true) >> 5;
		if(%clientId.floodMute)
		{
			%delta = %clientId.muteDoneTime - %time;
			if(%delta > 0)
			{
				Client::sendMessage(%clientId, $MSGTypeGame, "FLOOD! You cannot talk for " @ %delta @ " seconds.");
				return;
			}
			%clientId.floodMute = "";
			%clientId.muteDoneTime = "";
		}
		%clientId.floodMessageCount++;
		// funky use of schedule here:
		schedule(%clientId @ ".floodMessageCount--;", 5, %clientId);
		if(%clientId.floodMessageCount > 4)
		{
			%clientId.floodMute = true;
			%clientId.muteDoneTime = %time + 10;
			Client::sendMessage(%clientId, $MSGTypeGame, "FLOOD! You cannot talk for 10 seconds.");
			return;
		}
	}
	
	//zadmin messaging
	if (%clientId.canSendPrivateMsgs && %clientId.selClient && %clientId.selClient != %clientId) 
	{	  
	  if($dedicated)
			Krayvok::storeChat(1, %message, %clientId, %clientId.selClient);

	  BottomPrint(%clientId, "<jc>(Sent to " @ Client::getName(%clientId.selClient) @ ") " @ %message);
	  if(!%clientId.selClient.muted[%clientId])
	  	CenterPrint(%clientId.selClient, "<jc>" @ Client::getName(%clientId) @ " (Private Message): " @ %message);		  		 
	}
	else if (%clientId.canSendPrivateMsgs && %clientId.selClient && %clientId.selClient == %clientId)
	{
	  if($dedicated)
			Krayvok::storeChat(4, %message, %clientId, false);

	  for (%i = 0; %i < getNumClients(); %i++)
		  CenterPrint(getClientByIndex(%i), "<jc>" @ Client::getName(%clientId) @ " (Broadcast): " @ %message);	 
	}
	else if(%team)
	{
		if($dedicated)
			Krayvok::storeChat(3, %message, %clientId, false);

		%team = Client::getTeam(%clientId);
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			if(Client::getTeam(%cl) == %team && !%cl.muted[%clientId])
				Client::sendMessage(%cl, $MsgTypeTeamChat, %message, %clientId);
	}
	else
	{
		if (%clientId.globalMute) {
			Client::sendMessage(%clientId, $MSGTypeGame, $zadmin::pref::msg::globalspam);
			return;
		} else if ( String::findSubStr( %message, "~wretflag" ) != -1 ) {
			Client::sendMessage(%clientId, $MSGTypeGame, "Have fun trying to exploit global messages with your global mute, retard.");
			%clientId.globalMute = true;
			return;
		}

		if($dedicated)
			Krayvok::storeChat(2, %message, %clientId, false);


		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			if(!%cl.muted[%clientId])
				Client::sendMessage(%cl, $MsgTypeChat, %message, %clientId);
	}
}

function Krayvok::storeChat(%type, %message, %clientId, %msgTo) {

    %playerName = escapeString(Client::getName(%clientId));

    if (%msgTo != false) {
        %playerMsgTo = escapeString(Client::getName(%msgTo));
    }

    if (%type == 1) {
        %type = "PRIVATE:";
        $chatMsg = %type @ " - " @ %playerName @ " sent " @ %playerMsgTo @ ": " @ escapeString(%message) @ "";
    }
    if (%type == 2) {
        %type = "GLOBAL:";
        $chatMsg = %type @ " " @ %playerName @ ": " @ escapeString(%message) @ "";
    }
    if (%type == 3) {
        %type = "TEAM:";
        $chatMsg = %type @ " " @ %playerName @ ": " @ escapeString(%message) @ "";
    }
    if (%type == 4) {
        %type = "BROADCAST:";
        $chatMsg = %type @ " " @ %playerName @ ": " @ escapeString(%message) @ "";
    }

    export("$chatMsg", "config\\chatMessages.log", true);
}

function remoteIssueCommand(%commander, %cmdIcon, %command, %wayX, %wayY,
		%dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14)
{
	if(String::containsCrash(%commander, %command, "remoteIssueCommand"))
	{
		return;
	}
	// issueCommandI takes waypoint 0-1023 in x,y scaled mission area
	// issueCommand takes float mission coords.
	for(%i = 1; %dest[%i] != ""; %i = %i + 1)
		if(!%dest[%i].muted[%commander])
			issueCommandI(%commander, %dest[%i], %cmdIcon, %command, %wayX, %wayY);
}

function remoteIssueTargCommand(%commander, %cmdIcon, %command, %targIdx, 
		%dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14)
{
	if(String::containsCrash(%commander, %command, "remoteIssueTargCommand"))
	{
		return;
	}
	for(%i = 1; %dest[%i] != ""; %i = %i + 1)
		if(!%dest[%i].muted[%commander])
			issueTargCommand(%commander, %dest[%i], %cmdIcon, %command, %targIdx);
}

function remoteCStatus(%clientId, %status, %message)
{
	if(String::containsCrash(%clientId, %message, "remoteCStatus"))
	{
		return;
	}
	// setCommandStatus returns false if no status was changed.
	// in this case these should just be team says.
	if(setCommandStatus(%clientId, %status, %message))
	{
	}
	else
		remoteSay(%clientId, true, %message);
}

function teamMessages(%mtype, %team1, %message1, %team2, %message2, %message3)
{
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numPlayers; %i = %i + 1)
	{
		%id = getClientByIndex(%i);
		if(Client::getTeam(%id) == %team1)
		{
			Client::sendMessage(%id, %mtype, %message1);
		}
		else if(%message2 != "" && Client::getTeam(%id) == %team2)
		{
			Client::sendMessage(%id, %mtype, %message2);
		}
		else if(%message3 != "")
		{
			Client::sendMessage(%id, %mtype, %message3);
		}
	}
}

function messageAll(%mtype, %message, %filter)
{
	if(%filter == "")
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			Client::sendMessage(%cl, %mtype, %message);
	else
	{
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.messageFilter & %filter)
				Client::sendMessage(%cl, %mtype, %message);
		}
	}
}

function messageAllExcept(%except, %mtype, %message)
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		if(%cl != %except)
			Client::sendMessage(%cl, %mtype, %message);
}

function messageAndAnimate(%animSeq, %wav)
{ 
	remoteEval(2048,playAnimWav,%animSeq,%wav);
}

function localMessage(%wav)
{
   remoteEval(2048, LMSG, %wav);
}

function localMessageSpamCheck( %cl ) {
	//return true;
	%allowMessage = true;
	%rawNow = getIntegerTime(true);
	%now = %rawNow >> 5;

	//if ( %cl.floodLocalMute ) {
	//	%delta = ( %cl.localMuteDoneTime - %now );
	//	if ( %delta <= 0 ) {
	//		%cl.floodLocalMute = "";
	//		%cl.localMuteDoneTime = "";
	//	} else {
	//		%allowMessage = false;
	//	}
	//}
	
	 //allow local messages every half a second
	if ( %rawNow - %cl.lastLocalMessage < 4 ) {
		%allowMessage = false;
	}
	
	if ( %allowMessage ) {
		%cl.lastLocalMessage = %rawNow;
		//%cl.floodLocalMessageCount++;

		//schedule(%cl @ ".floodLocalMessageCount--;", 5, %cl);
		//if ( %cl.floodLocalMessageCount > 4 ) {
		//	%cl.floodLocalMute = true;
		//	%cl.localMuteDoneTime = %now + 8;
		//}
	}
	
	//return ( %allowMessage );
}

function remotePlayAnimWav(%cl, %anim, %wav) {
	// play the anim anyway
	remotePlayAnim(%cl, %anim);
	
	if ( !localMessageSpamCheck( %cl ) )
		return;
	
	//if ( ( %wav != "wdeath" ) && ( !%cl.megaMute ) ) {
	if ( !%cl.megaMute ) {
		playVoice(%cl, %wav);
	}
}

function remoteLMSG(%cl, %wav) {
	if ( !localMessageSpamCheck( %cl ) )
		return;

	//if ( ( %wav != "wdeath" ) && ( !%cl.megaMute ) ) {
	if ( !%cl.megaMute ) {
		playVoice(%cl, %wav);
	}
}