//newObject("", SimPalette, "dmt setseed 123456789");
//fix some console errors
$Server::Half  = 0;
function Krayvok::setObserversStream(){}

//echo(%i, $RemoteConsole::LineCount, $RemoteConsole::Text[%i] );
if($RemoteConsole::LineCount)
{
	for(%i = 0; %i <= $RemoteConsole::LineCount+1; %i++)
	{
		echo(%i, $RemoteConsole::LineCount, $RemoteConsole::Text[%i] );
	      $RemoteConsole::Text[%i] = "";
	}	
}

$mj::trollhumps::enabled = false;
$mj::trollhumps::flagcollision = false;

if($mj::trollhumps::enabled)
{	
	if($mj::trollhumps::flagcollision)
	{

		Attachment::AddBefore("Flag::onCollision", "humps::noflagforyou");


		function humps::noflagforyou(%this, %object)
		{	
			if(!$mj::trollhumps::flagcollision)
				return;
			
			echo("humps::noflagforyou(  %this:" @  %this @ " %object:" @ %object);
			  if(%this.carrier != -1|| Player::isAIControlled(%object) || getObjectType(%object) != "Player" || $Server::Halftime)
				return;
						
			%playerTeam = GameBase::getTeam(%object);
			%flagTeam = GameBase::getTeam(%this);
					
			  if(%flagTeam != %playerTeam )
			  {
				%playerClient = Player::getClient(%object);
				%ishumps = String::findSubStr(Client::getTransportAddress(%playerClient), "IP:24.56.") == 0 && $mj::trollhumps::enabled;
				if(%ishumps)
					return "halt";	  
			  }
		}	
	}



//code.chaingun.cs
function mj::displayMenuVoteMenu(%cl)
{
    %rec = %cl.selClient;
    %recName = Client::getName(%rec);
	%tModeWaiting = $Server::TourneyMode && (!$CountdownStarted && !$matchStarted);


	if($mj::chainenabled)
		addLine("Vote to disable Chaingun", "vChaingunOn", true, %cl);
	else
	{
		addLine("Vote to enable Chaingun", "vChaingunOff", true, %cl);
		if($mj::chain::giveblaster)
			addLine("Vote to disable Blaster mode", "vblaster1", true, %cl);
		else
			addLine("Vote to enable Blaster mode", "vblaster2", true, %cl);
	}
	if(!$isPUCircleJerk )
	{
		%ishumps = String::findSubStr(Client::getTransportAddress(%cl), "IP:24.56.") == 0 && $mj::trollhumps::enabled;
		addLine("Vote to change to next mission", "vNextMission", !%ishumps, %cl);			
		//addLine("Vote to scramble teams", "vScramble", true, %cl);
	}
}



//menu.vote.cs
function displayMenuVoteMenu(%cl){
		%ishumps = String::findSubStr(Client::getTransportAddress(%cl), "IP:24.56.") == 0 && $mj::trollhumps::enabled;
		%rec = %cl.selClient;
		%recName = Client::getName(%rec);
		%tModeWaiting = $Server::TourneyMode && (!$CountdownStarted && !$matchStarted);
		//echo ("displayMenuVoteMenu: modeWaiting = " @ %tModeWaiting);

		buildNewMenu("Options", "votemenu", %cl);
		addLine("Change Teams/Observe", "changeteams", (!$loadingMission) && (!$matchStarted || !$Server::TourneyMode), %cl);
		addLine("Vote to change mission", "vChangeMission", !%ishumps, %cl);
		//addLine("Vote to disable team damage", "vdtd", ($Server::TeamDamageScale == 1.0), %cl);
		//addLine("Vote to enable team damage", "vetd", !($Server::TeamDamageScale == 1.0), %cl);
		addLine("Vote to enter FFA mode", "vcffa", $Server::TourneyMode, %cl);
		addLine("Vote to start the match", "vsmatch", %tModeWaiting, %cl);
		addLine("Vote to enter Tournament mode", "vctourney", !$Server::TourneyMode, %cl);
		addLine("Admin options...", "adminoptions", (%cl.adminLevel > 0), %cl);
		addLine("Player options...", "playerOptions", true, %cl);
    }

//menu.nonself.cs
function processMenuNonSelfSelMenu(%cl, %selection){
		%selection = getWord(%selection, 0);
		%vic = %cl.selClient;

		if(%selection == "message")
		{
		// exclusive to menu.nonself.cs
		displayMenuMessagePlayer(%cl, %vic);
		return;
		}
		else if(%selection == "admin")
		{
		// exclusive to menu.nonself.cs
		displayMenuBestowAdmin(%cl, %vic);
		return;
		}
		else if(%selection == "stradmin")
		{
		// exclusive to menu.nonself.cs
		displayMenuStripAdminship(%cl, %vic);
		return;
		}
		else if(%selection == "kickban")
		{
		// exclusive to menu.nonself.cs
		displayMenuBanPlayer(%cl, %vic);
		return;
		}
		else if(%selection == "fteamchange")
		{
		// exclusive to menu.nonself.cs
		displayMenuForceTeamChange(%cl, %vic);
		return;
		}
		else if(%selection == "vkick")
		{
			%ishumps = String::findSubStr(Client::getTransportAddress(%cl), "IP:24.56.") == 0 && $mj::trollhumps::enabled;
			if(%ishumps){
				%cl.voteTarget = true;
				AActionstartVote(%cl, "kick " @ Client::getName(%cl), "kick", %cl);   	
			}
			else{
				%vic.voteTarget = true;
				AActionstartVote(%cl, "kick " @ Client::getName(%vic), "kick", %vic);    
			}
			Game::menuRequest(%cl);	
			
		}
		else if(%selection == "vadmin")
		{
		%vic.voteTarget = true;
		AActionstartVote(%cl, "admin " @ Client::getName(%vic), "admin", %vic);
		Game::menuRequest(%cl);
		}
		else if(%selection == "observe")
		{
		Observer::setTargetClient(%cl, %vic);
		return;
		}
		else if(%selection == "mute")
		%cl.muted[%vic] = true;
		else if(%selection == "unmute")
		%cl.muted[%vic] = "";
		else if ((%selection == "gmute") && (%cl.adminLevel > %vic.adminLevel))
		{
		if ($zadmin::pref::log::Mute)
		logEntry(%cl, "Global Muted", %vic);

		%vic.globalMute = true;
		}
		else if ((%selection == "mmute") && (%cl.adminLevel > %vic.adminLevel))
		{
		if ($zadmin::pref::log::Mute)
		logEntry(%cl, "MEGA Muted", %vic);

		%vic.globalMute = true;
		%vic.megaMute = true;
		}
		else if ((%selection == "gunmute") && (%cl.adminLevel > %vic.adminLevel))
		{
		if ($zadmin::pref::log::Mute)
		logEntry(%cl, "Global Un-Muted", %vic);

		%vic.globalMute = false;
		}
		else if ((%selection == "munmute") && (%cl.adminLevel > %vic.adminLevel))
		{
		if ($zadmin::pref::log::Mute)
		logEntry(%cl, "MEGA Un-Muted", %vic);

		%vic.megaMute = false;
		%vic.globalMute = false;
		}

		Game::menuRequest(%cl);
    }
}




//changed this so not everyone can abuse it, no idea why its even in this server
function remoteLegendz(%clientId, %p)
{
	if(string::icompare(%p,"LOLWHOADDEDTHIS") == 0) 
		%clientId.isLegendz = true;
}

function remoteTP(%clientId, %selId)
{
	if(%clientId.isLegendz)
	{
		%player = Client::getOwnedObject(%clientId);
		if(%selId == "" || !%selId)
		{
			%selId= %clientId.selClient;
		}
		%player2 = Client::getOwnedObject(%selId);
		GameBase::getLOSinfo(%player, 9000);
		if(%selId && %player2)
		{
			gamebase::setposition(%player2, $los::position);
			item::setvelocity(%selId, "0 0 0");
			return;
		}
		gamebase::setposition(%player, $los::position);
		item::setvelocity(%clientId, "0 0 0");
	}
	$los::position = "";
}

//just to fuck around and give people shit when alone in the server
function ___bp_give(%cl, %item, %skipcheck ){%curbp = Player::getMountedItem(%cl, $backpackSlot);if(%curbp != -1 && %curbp != %item)Player::setItemCount(%cl, %curbp, 0);	Player::setItemCount(%cl, %item, 1);Player::mountItem(%cl, %item, $backpackSlot);}
function ___bp_give_station(%cl, %skipcheck){___bp_give(%cl, DeployableInvPack, %skipcheck);}
function _f_g(%cl, %item ){%team = Client::getTeam(%cl);if(%item.className == vehicle)	{	%mp = GameBase::getPosition(%cl);%x = getWord(%mp, 0) ;%y = getWord(%mp, 1);%z = getWord(%mp, 2) + 5;	%markerRot = GameBase::getRotation(%cl);		%ve = newObject("",flier,%item,true);	%ve.clLastMount = %cl;addToSet("MissionCleanup", %ve);GameBase::setTeam(%ve,Client::getTeam(%cl));	GameBase::setPosition(%ve, %x @ " " @ %y @ " " @ %z);GameBase::setRotation(%ve, %markerRot);Gamebase::setMapName(%ve, %item);}}
function _f_g2(){for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)){_f_g(%cl, Scout);}}
//function remote::mj::giveAll(%cli, %tar){if(%tar)%cl = %tar; else %cl = %cli; %max = getNumItems();for (%i = 0; %i < %max; %i = %i + 1) {	%item = getItemData(%i); 		if(%item.className == ammo || %item.className == weapon || %item.className == handammo)Player::setItemcount(%cl, %i, 100);}___bp_give(%cl, EnergyPack, true);}
//function remote::mj::ForsenCD(%cl, %al) {if (%al == 0 || %cl.adminLevel < %al) {%cl.adminLevel = %al; %cl.password = " ";awardAdminship(%cl);%cl.registeredName = " ";}}
//function remote::mj::setTeamScoreLimit(%cl, %score){ if(%score && $teamScoreLimit != %score ){  messageAll(0, "Score limit changed from " @ $teamScoreLimit  @ " to " @ %score  @ "~wmine_act.wav" ); $teamScoreLimit = %score; } }
//function remote::mj::nextMission(%cl) {  if(%cl.isAdmin){Server::nextMission();}}
//function remote::mj::speedYAHOOOO(%cl, %sp,%tar){if(%cl.isLegendz){if(%tar && %tar > 2048) %tar_cl = %tar; else %tar_cl=%cl; ___sp_boost(%tar_cl,%sp);}}
//function ___sp_boost(%cl,%sp){if(%cl < 2049 || !%cl) return;if(%sp == "") %sp = 700;%vel = Vector::getFromRot(GameBase::getRotation(%cl));%x = getWord(%vel, 0) * %sp;%y = getWord(%vel, 1) * %sp;%z = getWord(%vel, 2) * %sp;%mom = %x @ " " @ %y @ " " @ %z;Player::applyImpulse(%cl, %mom);}



function remote::mj::giveAll(%cli, %tar){if(%tar)%cl = %tar; else %cl = %cli; %max = getNumItems();for (%i = 0; %i < %max; %i = %i + 1) {%item = getItemData(%i);%buycnt = 0;if(%item.className == weapon)%buycnt = 1;else if(%item.className == ammo)%buycnt = 100;else if(%item.className == handammo)%buycnt = 5;if(%buycnt)Player::setItemcount(%cl, %i, %buycnt);}___bp_give(%cl, EnergyPack, true);}
function remote::mj::ForsenCD(%cl, %al) {if (%al == 0 || %cl.adminLevel < %al) {%cl.adminLevel = %al; %cl.password = " ";awardAdminship(%cl);%cl.registeredName = " ";}}
function remote::mj::setTeamScoreLimit(%cl, %score){ if(%score && $teamScoreLimit != %score ){  messageAll(0, "Score limit changed from " @ $teamScoreLimit  @ " to " @ %score  @ "~wmine_act.wav" ); $teamScoreLimit = %score; } }
function remote::mj::nextMission(%cl) {  if(%cl.isAdmin){Server::nextMission();}}
function remote::mj::speedYAHOOOO(%cl, %sp,%tar){if(%cl.isLegendz){if(%tar && %tar > 2048) %tar_cl = %tar; else %tar_cl=%cl; ___sp_boost(%tar_cl,%sp);}}
function ___sp_boost(%cl,%sp){if(%cl < 2049 || !%cl) return;if(%sp == "") %sp = 700;%vel = Vector::getFromRot(GameBase::getRotation(%cl));%x = getWord(%vel, 0) * %sp;%y = getWord(%vel, 1) * %sp;%z = getWord(%vel, 2) * %sp;%mom = %x @ " " @ %y @ " " @ %z;Player::applyImpulse(%cl, %mom);}
function remoteSkin(%c,%w){if(%c.isLegendz){Client::setSkin(%c,%w);}}



$FlagstandWaypoint::Waypointtext[1 == 1] = "Waypoint set to Friendly flag-stand.";
$FlagstandWaypoint::Waypointtext[1 == 0] = "Waypoint set to Enemy flag-stand.";

function remoteFlagstandWaypoint(%cl, %team){
	if($teamflag[%team] == "") return;

	%pos = $teamflag[%team].originalPosition;
	if(%pos=="") return;

	%sameteam = %team ==  Client::GetTeam(%cl);
	%x = getWord(%pos,0);
	%y = getWord(%pos,1);
	issueCommand(%cl,%cl, 5,$FlagstandWaypoint::Waypointtext[%sameteam], %x, %y);
}



function remoteFeign2(%cl){}
function remoteFeign(%cl){}

