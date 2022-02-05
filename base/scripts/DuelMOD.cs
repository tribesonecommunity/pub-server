exec("code.game.cs");

$Server::TeamDamageScale = 1; //team damage must stay on

$curSpawn = 0;
if (!$Duel::Saved)
{
	$DuelMOD::AFKTimeLimit = $zadmin::AFKTimelimit;
	$zadmin::AFKTimelimit = 99999;
	$duel::saved=true;
}
$DuelMOD::origJoinMOTD = $Server::JoinMOTD;
//$Server::JoinMOTD = "<jc><f1>Welcome to DuelMOD™:\nClick on somebody's name in the Tab menu and click Duel";
$DuelMOD::origTeamName0 = $Server::teamName0;
//$Server::teamName0 = "Duel"; 
                  
$DuelSpotTaken[1] = false;
$DuelSpotTaken[2] = false;
$DuelSpotTaken[3] = false;
$DuelSpotTaken[4] = false;
$DuelSpotTaken[5] = false;
$DuelSpotTaken[6] = false;
$DuelSpotTaken[7] = false;
$DuelSpotTaken[8] = false;
$DuelSpotTaken[9] = false;
$DuelSpotTaken[10] = false;
$DuelSpotTaken[11] = false;
$DuelSpotTaken[12] = false;
                        
$DuelDelayTime = 4;
$DuelHurtDelay = 1.5;
                            
$DuelWeaponMax = 7;
$DuelPackMax = 4;

$DuelWeapon[1] = "Blaster";
$DuelWeapon[2] = "Chaingun";
$DuelWeapon[3] = "Disc Launcher";
$DuelWeapon[4] = "Grenade Launcher";
$DuelWeapon[5] = "Laser Rifle";
$DuelWeapon[6] = "Plasma Gun";
$DuelWeapon[7] = "ELF Gun";

$DuelPack[1] = "Energy Pack";
$DuelPack[2] = "Repair Pack";
$DuelPack[3] = "Shield Pack";
$DuelPack[4] = "Ammo Pack";

$DuelRealPack[1] = EnergyPack;
$DuelRealPack[2] = RepairPack;
$DuelRealPack[3] = ShieldPack;
$DuelRealPack[4] = AmmoPack;

$DuelRealWeapon[1] = Blaster;
$DuelRealWeapon[2] = Chaingun;
$DuelRealWeapon[3] = DiscLauncher;
$DuelRealWeapon[4] = GrenadeLauncher;
$DuelRealWeapon[5] = LaserRifle;
$DuelRealWeapon[6] = PlasmaGun;
$DuelRealWeapon[7] = EnergyRifle;

$DuelWeaponAmmo[2] = BulletAmmo;
$DuelWeaponAmmo[3] = DiscAmmo;
$DuelWeaponAmmo[4] = GrenadeAmmo;
$DuelWeaponAmmo[6] = PlasmaAmmo;
	                  
function Lightning::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId)
{                                                                                                              
   // This is to fix that problem with ELF gun.
   %damVal = %timeSlice * %damPerSec;
   %enVal  = %timeSlice * %enDrainPerSec;
                 
   %clientId = GameBase::GetControlClient(%target);
   if ($DuelStatus[%clientId] != "InDuel") {
	return;                               
   }          
   if (!$DuelHurt[%clientId])
	return;
   %foeId = %shooterId;
   if ($DuelStatus[%foeId] != "InDuel")
	return;
   if ($DuelLineup[%foeId] != %clientId && %clientId != %foeId)
   {
	Bottomprint(%foeId, "<jc><f2>Wrong Target!");
	return;                                         
   }
    
   GameBase::applyDamage(%target, $ElectricityDamageType, %damVal, %pos, %vec, %mom, %shooterId);

   %energy = GameBase::getEnergy(%target);
   %energy = %energy - %enVal;
   if (%energy < 0) {
      %energy = 0;
   }
   GameBase::setEnergy(%target, %energy);
}
                              
function DuelCountdown(%clientId, %foeId, %timeLeft, %clientPl, %foePl)
{                                               
	if ($DuelStatus[%clientId] != "InCountdown" || $DuelStatus[%foeId] != "InCountdown")
	{
		Client::SendMessage(%clientId,1,"Countdown Aborted");
		Client::SendMessage(%foeId,1,"Countdown Aborted");
		return;
	}
	if (%timeLeft == 0)
	{
		StartDuel(%clientId, %foeId, %clientPl, %foePl);
		return;
	}                                                                             
	if (%timeLeft == 1)
	{
		//Client::SendMessage(%clientId,1,"Duel starts in " @ %timeLeft @ " second.");
		//Client::SendMessage(%foeId,1,"Duel starts in " @ %timeLeft @ " second.");
		BottomPrint(%clientId,"<jc><f0>Duel starts in <f1>1<f0> second.",1);
		BottomPrint(%foeId,"<jc><f0>Duel starts in <f1>1<f0> second.",1);
		schedule("DuelCountdown(" @ %clientId @ ", " @ %foeId @ ", 0," @ %clientPl @ ", " @ %foePl @ ");",1);
	}
	else
	{         
		BottomPrint(%clientId,"<jc><f0>Duel starts in <f1>" @ %timeLeft @ "<f0> seconds.",1);
		BottomPrint(%foeId,"<jc><f0>Duel starts in <f1>" @ %timeLeft @ "<f0> seconds.",1);
		//Client::SendMessage(%clientId,0,"Duel starts in " @ %timeLeft @ " seconds.");
		//Client::SendMessage(%foeId,0,"Duel starts in " @ %timeLeft @ " seconds.");
		schedule("DuelCountdown(" @ %clientId @ ", " @ %foeId @ ", " @ %timeLeft - 1 @ ", " @ %clientPl @ ", " @ %foePl @ ");",1);
	}         
	
	//%counter = -1; 
	//for (%i = 5;%i > 0;%i--) {
	//	%counter++;
	//	if (%i == 1)
	//	{
	//		schedule("Client::SendMessage(" @ %clientId @ ",1,\"Duel starts in 1 second!\");", %counter);
	//		schedule("Client::SendMessage(" @ %foeId @ ",1,\"Duel starts in 1 second!\");", %counter);
	//	}
	//	else
	//	{
	//		schedule("Client::SendMessage(" @ %clientId @ ",0,\"Duel starts in " @ %i @ " seconds!\");", %counter);
	//		schedule("Client::SendMessage(" @ %foeId @ ",0,\"Duel starts in " @ %i @ " seconds!\");", %counter);
	//	}
	//} 
}                                     

function DuelResetClient(%clientId)
{
	$DuelStatus[%clientId] = "";
	$DuelLineup[%clientId] = "";
	$DuelWeaponSetup[%clientId] = "";  
	$DuelPack[%clientId] = "";
	$DuelLastEnemy[%clientId] = "";
}

function ClearMines(%clientId, %foeId)
{                         
	// clear client's mines                                      
	if ($DuelMine1[%clientId] != "")
	{                                                            
		if (getObjectType($DuelMine1[%clientId]) == "Mine")
			GameBase::setDamageLevel($DuelMine1[%clientId], 2);
		$DuelMine1[%clientId] = "";
	}
	if ($DuelMine2[%clientId] != "")
	{
		if (getObjectType($DuelMine2[%clientId]) == "Mine")
			GameBase::setDamageLevel($DuelMine2[%clientId], 2);
		$DuelMine2[%clientId] = "";
	}
	if ($DuelMine3[%clientId] != "")
	{
		if (getObjectType($DuelMine3[%clientId]) == "Mine")
			GameBase::setDamageLevel($DuelMine3[%clientId], 2);
		$DuelMine3[%clientId] = "";
	}                      
	
	// clear enemy's mines
	if ($DuelMine1[%foeId] != "")
	{                                   
		if (getObjectType($DuelMine1[%foeId]) == "Mine")
			GameBase::setDamageLevel($DuelMine1[%foeId], 2);
		$DuelMine1[%foeId] = "";
	}
	if ($DuelMine2[%foeId] != "")
	{                                                         
		if (getObjectType($DuelMine2[%foeId]) == "Mine")
			GameBase::setDamageLevel($DuelMine2[%foeId], 2);
		$DuelMine2[%foeId] = "";
	}
	if ($DuelMine3[%foeId] != "")
	{                                   
		if (getObjectType($DuelMine3[%foeId]) == "Mine")
			GameBase::setDamageLevel($DuelMine3[%foeId], 2);
		$DuelMine3[%foeId] = "";
	}                 
}
                        
function LockPlayers(%clientId, %foeId, %clientPl, %foePl)
{                          
	//Client::setOwnedObject(%clientId, -1);
	%clientId.observerMode = "pregame";
      	Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
      	Observer::setOrbitObject(%clientId, %clientPl, 5, 5, 5);                
      	GameBase::SetRotation(%clientId, GameBase::GetRotation(%clientPl));
      	//Client::setOwnedObject(%foeId, -1);
      	%foeId.observerMode = "pregame";
      	Client::setControlObject(%foeId, Client::getObserverCamera(%foeId));
      	Observer::setOrbitObject(%foeId, %foePl, 5, 5, 5);
      	GameBase::SetRotation(%foeId, GameBase::GetRotation(%foePl));
}                                                

function DuelStartHurt(%clientId, %foeId)
{
	$DuelHurt[%clientId] = true;
	$DuelHurt[%foeId] = true;
}

function StartDuel(%clientId, %foeId, %clientPl, %foePl)
{                                
	// This is just to prevent those first cheap shots.
	$DuelHurt[%clientId] = false;
	$DuelHurt[%foeId] = false;               
	%startTime = getSimTime();
	$DuelStartTime[%clientId] = %startTime;
	$DuelStartTime[%clientId] = %startTime;
	schedule("DuelStartHurt(" @ %clientId @ "," @ %foeId @ ");",$DuelHurtDelay);
	$DuelStatus[%clientId] = "InDuel";
	$DuelStatus[%foeId] = "InDuel";
	$DuelStarted[%clientId] = true;
	$DuelStarted[%foeId] = true;
	GameBase::SetDamageLevel(%clientPl,0);
	GameBase::SetDamageLevel(%clientPl,0);
	BottomPrint(%clientId,"<jc><f2>Fight!",1);
	BottomPrint(%foeId,"<jc><f2>Fight!",1);
	Client::setControlObject(%clientId, %clientPl);
	Client::setControlObject(%foeId, %foePl);	
}
           
function FinalizeDuel(%clientId, %foeId, %damageType)
{              
	ClearMines(%clientId, %foeId); 
	if (%damageType == -2)
		%damageType = $SuicideDamageType;
	if (%damageType != -1)
	{	
		$DuelStatsLine0 = "Open";	
		$DuelStatsLine1 = $DuelEndTime[%clientId] - $DuelStartTime[%clientId];
        	$DuelStatsLine2 = Client::GetName(%foeId);
        	$DuelStatsLine3 = Client::GetName(%clientId);
        	
        	if (%damageType == $BulletDamageType)
			$DuelStatsLine4 = "Bullet";
		else if (%damageType == $PlasmaDamageType)
			$DuelStatsLine4 = "Plasma";
		else if (%damageType == $ExplosionDamageType)
			$DuelStatsLine4 = "Disc";
		else if (%damageType == $ShrapnelDamageType)
			$DuelStatsLine4 = "Grenade";
		else if (%damageType == $LaserDamageType)
			$DuelStatsLine4 = "Laser";
		else if (%damageType == $MortarDamageType)
			$DuelStatsLine4 = "Mortar";
		else if (%damageType == $BlasterDamageType)
			$DuelStatsLine4 = "Blaster";
		else if (%damageType == $ElectricityDamageType)
			$DuelStatsLine4 = "ELF";
		else if (%damageType == $MineDamageType)
			$DuelStatsLine4 = "Mine";
                else if (%damageType == $SuicideDamageType)
                	$DuelStatsLine4 = "Suicide";
                
                //if (isFile("temp\\DuelStats.txt"))
        	//	export("$DuelStatsLine*", "temp\\DuelStats.txt", true);
        	//else                                                             
        	//{
        	//	export("$Server::HostName", "temp\\DuelStats.txt", true);
        	//	export("$DuelStatsLine*", "temp\\DuelStats.txt", true);
        	//}
        }
                
	%clientId.guiLock = false;
	%foeId.guiLock = false;
	
	%foeNotHere = false;
	%clientNotHere = false;
	
	$DuelStatus[%clientId] = "Available";
	$DuelStatus[%foeId] = "Available";     
	
	if (Client::GetName(%foeId) == "")
		%foeNotHere = true;
	if (Client::GetName(%clientId) == "")
		%clientNotHere = true;
		  
	if (%clientNotHere)
		echo("Client not here");
	if (%foeNotHere)
		echo("Foe not here");
		
	$DuelStarted[%clientId] = false;
	$DuelStarted[%foeId] = false;
	if (!%foeNotHere)
	{
		//DuelEmptyPlayer(%foeId);
		//echo("Blowing up player " @ Client::GetOwnedObject(%foeId));
		%foePl = Client::GetOwnedObject(%foeId);
		GameBase::SetDamageLevel(%foePl,1);
		Player::blowUp(%foePl);
	}
	if (!%clientNotHere)
		%clientId.observerMode = "observerFly";
	if (!%foeNotHere)
		%foeId.observerMode = "observerFly";
	$DuelSpawnSpot[%clientId] = "";
	$DuelSpotTaken[$DuelSpot[%clientId]] = false;
	$DuelSpot[%clientId] = "";
	$DuelSpot[%foeId] = "";
	if (!%clientNotHere)
		GameBase::SetTeam(%clientId, -1);
	if (!%foeNotHere)
		GameBase::SetTeam(%foeId, -1);
	//if (!%clientNotHere)
	//	GameBase::SetTeam(Client::GetOwnedObject(%clientId), -1);
	//if (!%foeNotHere)
	//	GameBase::SetTeam(Client::GetOwnedObject(%foeId), -1);
	
	$DuelLineup[%clientId] = "";
	$DuelLineup[%foeId] = "";
	   
}                                                 

function EndDuel(%clientId, %damageType)
{                           
	if (%damageType == -2)
		%damageType = "";           
	if ($DuelStatus[%clientId] == "InDuel" && $DuelStarted[%clientId])
	{   
		%endTime = getSimTime();
		$DuelEndTime[%clientId] = %endTime;
		%foeId = $DuelLineup[%clientId];
		$DuelStreak[%clientId] = 0;
		$DuelStreak[%foeId]++;

		$DuelHurt[%foeId] = false;
		MessageAll(1,"" @ Client::GetName(%foeId) @ " has triumphed over " @ Client::GetName(%clientId) @ "!");
		centerprint(%clientId,"<jc><f1>You <f2>lost<f1>!",4);
		centerprint(%foeId,"<jc><f1>You <f2>won<f1>!  -- Streak Now Up To <f2>" @ $DuelStreak[%foeId],4);
		$DuelOver[%clientId] = true;
		$DuelOver[%foeId] = true;
		if (%damageType == "")
			schedule("FinalizeDuel(" @ %clientId @ "," @ %foeId @ ",-2);", $DuelDelayTime);
		else
			schedule("FinalizeDuel(" @ %clientId @ "," @ %foeId @ "," @ %damageType @ ");", $DuelDelayTime);
	}
	else if ($DuelStatus[%clientId] == "InCountdown" && !$DuelStarted[%clientId])
	{
		%foeId = $DuelLineup[%clientId];
		$DuelSpotTaken[$DuelSpot[%clientId]] = false;
		MessageAll(1,"" @ Client::GetName(%clientId) @ " has ended the duel prior to start."); 
		$DuelStatus[%clientId] = "Available";
		$DuelStatus[%foeId] = "Available";
		FinalizeDuel(%clientId,%foeId,-1);
	}
	
}        

function CheckDuelTime(%clientId,%foeId, %timeLeft)
{                       
	if ($DuelLineup[%foeId] == "" && $DuelLineup[%clientId] == %foeId && %timeLeft == 0)
	{
		$DuelLineup[%clientId] = "";
		Client::SendMessage(%clientId,0,"" @ Client::GetName(%foeId) @ " did not accept the duel in time.");
		Client::SendMessage(%foeId,0,"You can no longer accept the duel from " @ Client::GetName(%clientId) @ ".");
		return;
	}
	else if ($DuelLineup[%foeId] != "" && $DuelLineup[%foeId] != %clientId && $DuelStatus[%clientId] == "Available" && $DuelStatus[%foeId] == "Available")
	{
		$DuelLineup[%clientId] = "";
		Client::SendMessage(%clientId,0,"" @ Client::GetName(%foeId) @ " has just asked somebody else to duel. Try again soon.");
		return;
	}	                         
	else if ($DuelLineup[%foeId] != "" && $DuelLineup[%foeId] != %clientId && ($DuelStatus[%foeId] == "InDuel" || $DuelStatus[%foeId] == "InCountdown") && $DuelStatus[%clientId] == "Available")
	{
		$DuelLineup[%clientId] = "";
		Client::SendMessage(%clientId,0,"" @ Client::GetName(%foeId) @ " has accepted a duel with somebody else.");
		return;
	}
	else if (Client::GetName(%foeId) == "")
	{
		// enemy has left the game during this countdown, let's shut it off and notify client
		Client::SendMessage(%clientId,0,"The person you challenged has left the game.");
		$DuelLineup[%foeId] = "";
		$DuelStatus[%foeId] = "";
		$DuelLineup[%clientId] = "";      
		return;
	}
	else if (Client::GetName(%clientId) == "")
	{
		// challenger left the game, notify the person he/she challenged.
		Client::SendMessage(%foeId,0,"The person that challenged you has left the game, cancelling countdown.");
		$DuelLineup[%clientId] = "";
		$DuelStatus[%clientId] = ""; 
		return;
	}
	else if ($DuelLineup[%foeId] == %clientId && $DuelLineup[%clientId] == %foeId && $DuelStatus[%clientId] == "InCountdown" && $DuelStatus[%foeId] == "InCountdown")
	{              
		// Duel started, cancel this accept timer
		return;
	}
	schedule("CheckDuelTime(" @ %clientId @ ", " @ %foeId @ ", " @ %timeLeft-- @ ");",1);
}
                                                                              
function DuelCheckPlayers()
{                      
	// No need to continue calling if server switched to CTF mode                    
	if ($Game::missionType != "Duel") 
	{
		$DuelsStarted = "";
		return;             
	}         
	$DuelsStarted = 1;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
		// echo("" @ %cl @ ": " @ $DuelStatus[%cl]);                            
		//echo("" @ %cl.observerMode @ " " @ Client::GetTeam(%cl));
		if ($DuelStatus[%cl] == "")			
			$DuelStatus[%cl] = "Available";
		if ($DuelMode[%cl] == "")
			$DuelMode[%cl] = "On";
		//if (%cl.observerMode == "" && Client::GetTeam(%cl) == -1 && $DuelStatus[%cl] == "Available")
			//Centerprint(%cl,"" @ $Server::MOTD, 10);
		
		//if (($DuelStatus[%cl] == "Available" && $DuelLineup[%cl] == "" && Client::GetTeam(%cl) != -1) || ($DuelStatus[%cl] == "Available" && $DuelLineup[%cl] == "" && Client::GetOwnedObject(%cl) != -1))
		//if ($ClientInGame[%cl]  == "JoinedTeam" && $DuelStatus[%cl] == "Available")
		//{
		//	if (%cl.observerMode == "" || Client::GetOwnedObject(%cl) != -1)
		//		Player::Kill(Client::GetOwnedObject(%cl));
		//	GameBase::SetTeam(%cl, -1);
		//	%cl.observerMode = "observerFly";
		//	$ClientInGame[%cl] = "Done";
		//}                                                               
		//%foeId = $DuelLineup[%cl];
		//if (%foeId != "")
		//{
		//	if ($DuelStatus[%cl] == "InDuel" && $DuelStatus[%foeId] == "InDuel" && !Player::isDead(%cl) && !Player::isDead(%foeId))
		//	{
		//		%clientPl = Client::GetOwnedObject(%cl);
		//		%foePl = Client::GetOwnedObject(%foeId);
		//		if (%foePl != -1 && %clientPl != -1)
		//		{             
		//			if ($DuelRealWeapon[getWord($DuelWeaponSetup[%cl],0)] == LaserRifle || $DuelRealWeapon[getWord($DuelWeaponSetup[%cl],1)] == LaserRifle || $DuelRealWeapon[getWord($DuelWeaponSetup[%cl],2)] == LaserRifle || $DuelRealWeapon[getWord($DuelWeaponSetup[%foeId],0)] == LaserRifle || $DuelRealWeapon[getWord($DuelWeaponSetup[%foeId],1)] == LaserRifle || $DuelRealWeapon[getWord($DuelWeaponSetup[%foeId],2)] == LaserRifle)
		//			{
		//				// do nothing, don't check ammo since they have a laser rifle or elf gun
		//				%clientOutOfAmmo = false;
		//				%foeOutOfAmmo = false;
		//			}
		//			else
		//			{                                    
		//				
		//				if (Player::GetItemCount(%cl,$DuelWeaponAmmo[getWord($DuelWeaponSetup[%cl],0)]) == 0 && Player::GetItemCount(%cl,$DuelWeaponAmmo[getWord($DuelWeaponSetup[%cl],1)]) == 0 && Player::GetItemCount(%cl,$DuelWeaponAmmo[getWord($DuelWeaponSetup[%cl],2)]) == 0)
		//					%clientOutOfAmmo = true;
		//				else
		//					%clientOutOfAmmo = false;
		//				
		//				if (Player::GetItemCount(%foeId,$DuelWeaponAmmo[getWord($DuelWeaponSetup[%foeId],0)]) == 0 && Player::GetItemCount(%foeId,$DuelWeaponAmmo[getWord($DuelWeaponSetup[%foeId],1)]) == 0 && Player::GetItemCount(%foeId,$DuelWeaponAmmo[getWord($DuelWeaponSetup[%foeId],2)]) == 0)
		//					%foeOutOfAmmo = true;
		//				else
		//					%foeOutOfAmmo = false;
		//			}						
		//			if (%clientOutOfAmmo && %foeOutOfAmmo)
		//				schedule("PlayersOutOfAmmo(" @ %cl @ "," @ %foeId @ ");",5);
		//		}
		//	}                                                                                             
		//}
	}		
	schedule("DuelCheckPlayers();",1);
}                                                                             

function PlayersOutOfAmmo(%clientId, %foeId)
{
	if ($DuelStatus[%clientId] == "InDuel" && $DuelStatus[%foeId] == "InDuel" && $DuelLineup[%clientId] == %foeId && $DuelLineup[%foeId] == %clientId && !$DuelOver[%clientId])
	{
		MessageAll(1,"" @ Client::GetName(%clientId) @ " and " @ Client::GetName(%foeId) @ " have both run out of ammo.  They have tied.");
		%clientPl = Client::GetownedObject(%clientId);
		Player::Kill(%clientPl);
		Player::Blowup(%clientPl);
		FinalizeDuel(%clientId, %foeId);
	}
}

function DuelInit(%clientId,%foeId)
{                                     
	
	if (%foeId != -1 && %foeId != 0)
	{                               
		if (Client::GetName(%foeId) == "")
		{
			Client::SendMessage(%clientId,0,"That person is not in the game.");
			return;
		}
		if (%foeId == %clientId)
		{
			client::sendmessage(%clientId,1,"You can't duel yourself!");
			return;
		}                       
		if ($DuelMode[%foeId] == "Off")
		{
			Client::SendMessage(%clientId,0,"" @ Client::GetName(%foeId) @ " has duels off.");
			return;
		}			
		if ($DuelLineup[%foeId] == "" && $DuelStatus[%foeId] == "Available" && $DuelStatus[%clientId] == "Available")
		{               
			$DuelLineup[%clientId] = %foeId;
			client::sendMessage(%clientId,0,"" @ Client::GetName(%foeId) @ " has 30 seconds to accept the duel.");
			//client::sendMessage(%foeId,0,"" @ Client::GetName(%clientId) @ " has requested a duel.");
			CenterPrint(%foeId,"<jc>" @ Client::GetName(%clientId) @ " has requested a duel.",5);
			client::sendMessage(%foeId,0,"You have 30 seconds to accept the duel.");
			CheckDuelTime(%clientId, %foeId, 30);
			return;
		}                                                 
		else if ($DuelLineup[%clientId] == "" && $DuelLineup[%foeId] != "" && $DuelStatus[%foeId] == "Available" && $DuelLineup[%foeId] != %clientId)
		{
			//$DuelLineup[%clientId] = %foeId;             
			//client::sendMessage(%clientId,0,"" @ Client::GetName(%foeId) @ " is requesting a duel from somebody else, but can still accept yours.");
			//client::sendMessage(%clientId,0,"" @ Client::GetName(%foeId) @ " has 30 seconds to accept the duel.");
			//client::sendMessage(%foeId,0,"" @ Client::GetName(%clientId) @ " has requested a duel.");
			//CenterPrint(%foeId,"<jc>" @ Client::GetName(%clientId) @ " has requested a duel.",5);
			//client::sendMessage(%foeId,0,"You have 30 seconds to accept the duel.");
			//CheckDuelTime(%clientId, %foeId, 30);
			client::sendMessage(%clientId,0,"" @ Client::GetName(%foeId) @ " is already requesting a duel from somebody else.  Try again later.");
			return;
		}			
		else if ($DuelLineup[%clientId] == "" && $DuelLineup[%foeId] != "" && ($DuelStatus[%foeId] == "InDuel" || $DuelStatus[%foeId] == "InCountdown"))
		{
			// This one fires if the challenged is in a duel
			client::sendmessage(%clientId,0,"" @ Client::GetName(%foeId) @ " is currently in a duel.  Try again later.");
			return;
		}        
		else if ($DuelStatus[%clientId] == "InDuel")
		{
			client::sendmessage(%clientId,0,"Hello!? You're already in a duel!");
			return;
		}        
		else if ($DuelLineup[%foeId] == %clientId && $DuelStatus[%clientId] == "Available" && $DuelStatus[%foeId] == "Available") // || ($DuelLineup[%clientId] != "" && $DuelStatus[%clientId] == "Available"))
		{
			// Prepare to start duel
			%spotTaken = true;
			%i = 0;   
			while (%spotTaken) {
				%i++;
				if (%i > 12)
				{
					Client::SendMessage(%clientId,0,"No spawn spots are empty. Try again when someone finishes.");
					Client::SendMessage(%foeId,0,"No spawn spots are empty. Try again when someone finishes.");
					$DuelLineup[%clientId] = "";
					$DuelLineup[%foeId] = "";
					$DuelStatus[%clientId] = "Available";
					$DuelStatus[%foeId] = "Available";
					return;
				}
				if (!$DuelSpotTaken[%i])
				{                                                  
					$DuelSpot[%clientId] = %i;
					$DuelSpot[%foeId] = %i;
					$DuelSpawnSpot[%clientId] = $DuelSpawn[%i];
					$DuelSpotTaken[%i] = true;
					%spotTaken = false;
				}
			}         
			// echo("Spot setup:" @ $DuelSpot[%clientId] @ " - " @ $DuelSpawnSpot[%clientId]);
			GameBase::SetTeam(%clientId,0);
			GameBase::SetTeam(%foeId, 0);
			%clientId.guiLock = true;
			%foeId.guiLock = true;
			$DuelLineup[%clientId] = %foeId;
			MessageAll(0,"" @ Client::GetName(%clientId) @ " and " @ Client::GetName(%foeId) @ " are about to duel.");
			%spawnMarker = $DuelSpawnSpot[%clientId];
   			%xPos = getWord(%spawnMarker, 0);
   			%yPos = getword(%spawnMarker, 1) + 25;
   			%zPos = getWord(%spawnMarker, 2) + 10;
   			$DuelSpawnSpot[%foeId] = "" @ %xPos @ " " @ %yPos @ " " @ %zPos @ "";
   			%clientId.observerMode = "";
   			%foeId.observerMode = "";
   			$DuelCanSpawn[%clientId] = true;
   			$DuelCanSpawn[%foeId] = true;
   			%clientPl = DuelSpawn(%clientId);
   			GameBase::SetTeam(%clientPl, 0);
			%foePl = DuelSpawn(%foeId);                                  
			GameBase::SetTeam(%foePl, 0);
   			GameBase::setRotation(%clientPl,"0 0 0");
   			GameBase::setRotation(%foePl,"0 0 3");
   			LockPlayers(%clientId, %foeId, %clientPl, %foePl);
   			%foeId.observerTarget = "";
   			%clientId.observerTarget = "";
   			$DuelStatus[%foeId] = "InCountdown";
			$DuelStatus[%clientId] = "InCountdown";
			$DuelOver[%clientId] = false;
			$DuelOver[%clientId] = false;
			$DuelLastEnemy[%clientId] = %foeId;
			$DuelLastEnemy[%foeId] = %clientId;
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) { 
				if (Client::GetTeam(%cl) == -1)
				{
					if (%cl.observerTarget == %clientId)
						Observer::setTargetClient(%cl, %clientId);
					else if (%cl.observerTarget == %foeId)                 
						Observer::setTargetClient(%cl, %foeId);
				}
			}		
			DuelCountdown(%clientId,%foeId,5,%clientPl, %foePl);
		}
	}
	else                                                                 
	{
		Client::SendMessage(%clientId,0,"Nobody here by that name.");
	}
}                
                                   
function DuelEmptyPlayer(%player)
{       
	return;        
	if (%player == -1)
		return;
	%max = getNumItems();
	for (%i = 0; %i < %max; %i++) { 
		%item = getItemData(%i); 
		
		%count = Player::getItemCount(%player,%item);
		if(%count > 0) {
			if(%item.className != Armor) 
				Player::setItemCount(%player, %item, 0);  
		}
	}                       
}

function DuelSpawn(%clientId)
{                                                                  
	%pl = spawnPlayer("larmor", $DuelSpawnSpot[%clientId], "0 0 0");
	if(%pl != -1)
	{
		Client::setOwnedObject(%clientId, %pl);
	}
	Item::setVelocity(%clientId,"0 0 0");
	GameBase::setEnergy(Client::GetOwnedObject(%clientId),100);
	if ($DuelSpawnSpot[%clientId] != "")
	{
		GameBase::SetPosition(%clientId,$DuelSpawnSpot[%clientId]);
	        if (Client::getGender(%clientId) == "Female")
	        {
	        	Player::SetArmor(%clientId,"lfemale"); 
	        	Player::SetItemCount(%clientId,LightArmor,1);
	        }
	        else
	        {
	        	Player::SetArmor(%clientId,"larmor");
	        	Player::SetItemCount(%clientId,LightArmor,1);
	        }
	        if ($DuelWeaponSetup[%clientId] == "" || (getWord($DuelWeaponSetup[%clientId],0) == -1 || getWord($DuelWeaponSetup[%clientId],1) == -1 || getWord($DuelWeaponSetup[%clientId],2) == -1))
	        	$DuelWeaponSetup[%clientId] = "3 2 4";
		%packNo = $DuelPackSetup[%clientId];
        	if (%packNo == "")
        		%packNo = 1;
        	%pack = $DuelRealPack[%packNo];
        	Player::SetItemCount(%clientId,%pack,1);
        	Player::UseItem(%clientId,%pack);
	        for (%i = 0;%i < 3;%i++) {
	        	%weapon = getWord($DuelWeaponSetup[%clientId],%i);
	        	%weaponAmmo = $DuelWeaponAmmo[%weapon];
	        	%realweapon = $DuelRealWeapon[%weapon];
	        	// echo("Giving " @ Client::GetName(%clientId) @ " a " @ %realweapon @ " - " @ %weapon);
	        	%armor = Player::GetArmor(%clientId);
	        	Player::SetItemCount(%clientId,%realweapon,1);
	        	if (%weaponAmmo != "")                        
	        	{
	        		if (%pack == AmmoPack && $AmmoPackMax[%weaponAmmo] != "")
	        			Player::SetItemCount(%clientId,%weaponAmmo, $ItemMax[%armor, %weaponAmmo] + $AmmoPackMax[%weaponAmmo]);
	        		else
	        			Player::SetItemCount(%clientId,%weaponAmmo, $ItemMax[%armor, %weaponAmmo]);
	        	}
	        }
	        Player::SetItemCount(%clientId,MineAmmo,3);
        	Player::SetItemCount(%clientId,Grenade,5);  
        	Player::SetItemCount(%clientId,RepairKit,1);
        	Player::UseItem(%clientId,$DuelRealWeapon[getWord($DuelWeaponSetup[%clientId],0)]);
	}
	return %pl;
}
                
///////////////////////////////////////////////////////////////////////////////////////////////
//
// shitty duel functions
//
///////////////////////////////////////////////////////////////////////////////////////////////

function DuelToggleMode(%clientId)
{
	if ($DuelMode[%clientId] == "On")
	{
		client::sendmessage(%clientId,0,"You have turned duels off.");
		$DuelMode[%clientId] = "Off";
	}
	else
	{
		client::sendmessage(%clientId,0,"You have turned duels on.");
		$DuelMode[%clientId] = "On";
	}
}

function StartWeaponSetup(%clientId)
{
	Client::buildMenu(%clientId, "Weapon Setup (Choose 3):", "weaponselect1", true);
	for (%i = 1;%i <= $DuelWeaponMax;%i++) {
		Client::addMenuItem(%clientId, "" @ %i @ $DuelWeapon[%i], %i);
	}                   
}
                                    
function StartPackSetup(%clientId)
{
	Client::buildMenu(%clientId, "Backpack Setup (Choose 1):", "packselect", true);
	for (%i = 1;%i <= $DuelPackMax;%i++) {
		Client::addMenuItem(%clientId, "" @ %i @ $DuelPack[%i], %i);
	}                   
}

function processMenuPackSelect(%clientId, %option)
{
	$DuelPackSetup[%clientId] = %option;
	if ($DuelWeaponSetup[%clientId] == "" || (getWord($DuelWeaponSetup[%clientId],0) == -1 || getWord($DuelWeaponSetup[%clientId],1) == -1 || getWord($DuelWeaponSetup[%clientId],2) == -1))
	        $DuelWeaponSetup[%clientId] = "3 2 4";
	bottomprint(%clientId,"Your weapon setup is: " @ $DuelWeapon[getWord($DuelWeaponSetup[%clientId],0)] @ ", " @ $DuelWeapon[getWord($DuelWeaponSetup[%clientId],1)] @ ", and " @ $DuelWeapon[getWord($DuelWeaponSetup[%clientId],2)] @ "\nYour backpack setup is: " @ $DuelPack[$DuelPackSetup[%clientId]]);
	return;
}

function processMenuWeaponSelect1(%clientId, %option)
{               
	Client::buildMenu(%clientId, "Weapon Setup (Choose 2 more):", "weaponselect2", true);
	%tmpCounter = 0;
	for (%i = 1;%i <= $DuelWeaponMax;%i++) {
		%tmpCounter++;
		if (%i != %option)
		{
			Client::addMenuItem(%clientId, "" @ %tmpCounter @ $DuelWeapon[%i], %i);
		}
		else
		{
			$DuelWeaponSetup[%clientId] = %i;
		}
	}
}

function processMenuWeaponSelect2(%clientId, %option)
{               
	Client::buildMenu(%clientId, "Weapon Setup (Choose 1 more):", "weaponselect3", true);
	%tmpCounter = 0;
	for (%i = 1;%i <= $DuelWeaponMax;%i++) {
		%tmpCounter++;
		if (%i != %option && %i != getWord($DuelWeaponSetup[%clientId],0))
		{
			Client::addMenuItem(%clientId, "" @ %tmpCounter @ $DuelWeapon[%i], %i);
		}
		else if (%i != getWord($DuelWeaponSetup[%clientId],0))
		{
			$DuelWeaponSetup[%clientId] = $DuelWeaponSetup[%clientId] @ " " @ %i;
		}
	}
}

function processMenuWeaponSelect3(%clientId, %option)
{               
	$DuelWeaponSetup[%clientId] = $DuelWeaponSetup[%clientId] @ " " @ %option;                                                                                                                                                                                        
	if ($DuelPackSetup[%clientId] == "")
		$DuelPackSetup[%clientId] = 1;
	bottomprint(%clientId,"Your weapon setup is: " @ $DuelWeapon[getWord($DuelWeaponSetup[%clientId],0)] @ ", " @ $DuelWeapon[getWord($DuelWeaponSetup[%clientId],1)] @ ", and " @ $DuelWeapon[getWord($DuelWeaponSetup[%clientId],2)] @ "\nYour backpack setup is: " @ $DuelPack[$DuelPackSetup[%clientId]]);
	
}


function a(){}

function buildNewMenu(%displayName, %menuHandle, %cl)
{
   Client::buildMenu(%cl, %displayName, %menuHandle, true);
   %cl.menuLine = 0;
}

function addLine(%item, %itemResult, %condition, %cl)
{
    if(%condition)
	   Client::addMenuItem(%cl, %cl.menuLine++ @ %item, %itemResult);
}

function Game::menuRequest(%cl)
{     
   if(%cl.selClient && %cl.selClient != %cl)
      displayMenuNonSelfSelMenu(%cl);

   else if(%cl.selClient == %cl)
      displayMenuSelfSelMenu(%cl);
      
   else if($curVoteTopic != "" && (%cl.vote == "" || %cl.canCancelVote))
      displayMenuVotePendingMenu(%cl);   
         
   else if(%cl.adminLevel)
   	  displayMenuAdminMenu(%cl);

   else 
      displayMenuVoteMenu(%cl);            
}

function a(){}

function displayMenuAdminMenu(%cl)
{
	%rec = %cl.selClient;
    %recName = Client::getName(%rec);
	%tModeWaiting = $Server::TourneyMode && (!$CountdownStarted && !$matchStarted);

    buildNewMenu("Options", "adminmenu", %cl);

	addLine("Re-request Latest Duel", "rerequest " @ $DuelLastEnemy[%cl], $DuelLastEnemy[%cl] != "" && $DuelMode[$DuelLastEnemy[%cl]] == "On" && $DuelMode[%cl] == "On", %cl);
	addLine("Weapons Setup", "weaponsetup", true, %cl);
	addLine("Backpack Setup", "packsetup", true, %cl);
	addLine("Toggle Duel Mode", "toggleduelmode", true, %cl);

	if ($DuelStatus[%cl] == "Available")
		for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
		{ 
			if ($DuelStatus[%clientId] == "Available" && $DuelLineup[%clientId] == %cl)
				addLine("Accept duel from " @ Client::GetName(%clientId), "acceptduel " @ %clientId, true, %cl);
		}

	addLine(" ", "adminOptions", true, %cl);
    addLine("Change mission", "changeMission", %cl.canChangeMission, %cl);
	addLine("Set Time Limit", "ctimelimit", %cl.canChangeTimeLimit, %cl);
	addLine("Announce Server Takeover", "takeovermes", %cl.canAnnounceTakeover, %cl);
	addLine("Vote options...", "voteOptions", true, %cl);	
}

function processMenuAdminMenu(%cl, %selection)
{
	%selcl = getword(%selection, 1);
	%selection = getword(%selection, 0);

	if(%selection == "changeMission")
    {
         %cl.madeVote = ""; //for admins initiating mission change votes.
         displayMenuChangeMissionType(%cl, 0);
         return;         
    }
	else if (%selection == "rerequest")
	{
		DuelInit(%cl, %selcl);
		return;
	}
    else if (%selection == "weaponsetup")
    {
		StartWeaponSetup(%cl);
		return;
    }
	else if (%selection == "packsetup")
	{
		StartPackSetup(%cl);
		return;
	}
	else if (%selection == "toggleduelmode")
		DuelToggleMode(%cl);
    else if(%selection == "ctimelimit")
    {    
		 displayMenuChangeTimeLimit(%cl);
         return;		 
    }
    else if(%selection == "reset")
	{
    	 displayMenuResetServerDefaults(%cl);         
    	 return;
    }
    else if(%selection == "takeovermes")
	{
         displayMenuAnnounceServerTakeover(%cl);    
    	 return;
	}
    else if(%selection == "voteOptions")
    { 
	     displayMenuVoteMenu(%cl);
		 return;
 	}
	else if (%selection == "acceptduel")
	{
		if ($DuelStatus[%cl] == "Available" && $DuelLineup[%selcl] == %cl)
		{
			DuelInit(%cl, %selcl);
			return;
		}
		else
		{
			Client::SendMessage(%cl,0,"You can no longer accept the duel from " @ Client::GetName(%selcl));
			return;
		}
	}

    Game::menuRequest(%cl);
}

function a(){}

function displayMenuVoteMenu(%cl)
{
    %rec = %cl.selClient;
    %recName = Client::getName(%rec);
	%tModeWaiting = $Server::TourneyMode && (!$CountdownStarted && !$matchStarted);
	
	buildNewMenu("Options", "votemenu", %cl);
	addLine("Re-request Latest Duel", "rerequest " @ $DuelLastEnemy[%cl], $DuelLastEnemy[%cl] != "" && $DuelMode[$DuelLastEnemy[%cl]] == "On" && $DuelMode[%cl] == "On", %cl);
	addLine("Weapons Setup", "weaponsetup", true, %cl);
	addLine("Backpack Setup", "packsetup", true, %cl);
	addLine("Toggle Duel Mode", "toggleduelmode", true, %cl);

	if ($DuelStatus[%cl] == "Available")
		for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
		{ 
			if ($DuelStatus[%clientId] == "Available" && $DuelLineup[%clientId] == %cl)
				addLine("Accept duel from " @ Client::GetName(%clientId), "acceptduel " @ %clientId, true, %cl);
		}

	addLine("Vote to change mission", "vChangeMission", true, %cl);
	addLine("Admin Options...", "adminoptions", (%cl.adminLevel > 0), %cl);
}

function processMenuVoteMenu(%cl, %selection)
{
	%selcl = getword(%selection, 1);
	%selection = getword(%selection, 0);
	
	if(%selection == "vChangeMission")
    {
         %cl.madeVote = true;
         displayMenuChangeMissionType(%cl, 0);
         return;
    }
	else if (%selection == "rerequest")
	{
		DuelInit(%cl, %selcl);
		return;
	}
    else if (%selection == "weaponsetup")
    {
		StartWeaponSetup(%cl);
		return;
    }
	else if (%selection == "packsetup")
	{
		StartPackSetup(%cl);
		return;
	}
	else if (%selection == "toggleduelmode")
		DuelToggleMode(%cl);
	else if (%selection == "acceptduel")
	{
		if ($DuelStatus[%cl] == "Available" && $DuelLineup[%selcl] == %cl)
		{
			DuelInit(%cl, %selcl);
			return;
		}
		else
		{
			Client::SendMessage(%cl,0,"You can no longer accept the duel from " @ Client::GetName(%selcl));
			return;
		}
	}
	else if(%selection == "adminoptions")
	{
	   //no need to add, falls through to Game::menu request anyway
    }

	Game::menuRequest(%cl);
}

function a(){}

function displayMenuVotePendingMenu(%cl)
{	    
    buildNewMenu("Vote in progress", "votePendingMenu", %cl);

	addLine("Vote YES to " @ $curVoteTopic, "voteYes " @ $curVoteCount, %cl.vote == "", %cl);
	addLine("Vote No to " @ $curVoteTopic, "voteNo " @ $curVoteCount, %cl.vote == "", %cl);
	addLine("VETO Vote to " @ $curVoteTopic, "veto", %cl.canCancelVote, %cl);
	addLine("Admin Options...", "adminoptions", (%cl.adminLevel > 0), %cl);
}

function processMenuVotePendingMenu(%cl, %sel)
{
	%selection = getWord(%sel, 0);
	if(%selection == "voteYes") // && %cl == $curVoteCount)	************************
    {
         %cl.vote = "yes";
         centerprint(%cl, "", 0);		 
    }
    else if(%selection == "voteNo") // && %cl == $curVoteCount)	*************************
    {
         %cl.vote = "no";
         centerprint(%cl, "", 0);
    }
	else if(%selection == "veto")
	{
	    messageAll(0, "Vote to " @ $curVoteTopic @ " was VETO'd by an Admin.");
		bottomPrintAll("",0);
		$curVoteTopic = "";
      	aActionvoteFailed();	      
    }
	else if(%selection == "adminoptions")
	{
	   displayMenuAdminMenu(%cl);
	   return;
	}
	Game::menuRequest(%cl);
}

function a(){}

function displayMenuSelfSelMenu(%cl)
{		
	buildNewMenu("Options", "selfselmenu", %cl);
	addLine("Vote to admin yourself", "vadminself", !%cl.adminLevel, %cl);   
}

function processMenuSelfSelMenu(%cl, %selection)
{
    if (%selection == "vadminself")	
    {
         %cl.voteTarget = true;
         AActionstartVote(%cl, "admin " @ Client::getName(%cl), "admin", %cl);
		 Game::menuRequest(%cl);
    }	   
}

function a(){}

function displayMenuNonSelfSelMenu(%cl)
{		
	%rec = %cl.selClient;
    %recName = Client::getName(%rec);
	if(%cl.canBan)
	   %kickMsg = "Kick or Ban ";
	else
	   %kickMsg = "Kick ";

	buildNewMenu("Options", "nonselfselmenu", %cl);

	addLine("Duel " @ %recName, "duel " @ %rec, ($DuelMode[%rec] == "On" && $DuelMode[%cl] == "On" && $DuelLineup[%cl] != %rec), %cl);

	addLine("Vote to admin " @ %recName, "vadmin " @ %rec, !%cl.canMakeAdmin, %cl);
	addLine("Vote to kick " @ %recName, "vkick " @ %rec, !%cl.canKick, %cl);

	addLine(%kickMsg @ %recName, "kickban " @ %rec, %cl.canKick, %cl);
	addLine("Message " @ %recName, "message " @ %rec, %cl.canSendWarning, %cl);
	addLine("Admin " @ %recName, "admin " @ %rec, %cl.canMakeAdmin, %cl);
	addLine("Strip " @ %recName, "stradmin " @ %rec, (%cl.canStripAdmin && %rec.adminLevel > 0), %cl);

    addLine("Observe " @ %recName, "observe " @ %rec, (%cl.observerMode == "observerOrbit"), %cl);

	addLine("UnMute " @ %recName, "unmute " @ %rec, %cl.muted[%rec], %cl);
	addLine("Mute " @ %recName, "mute " @ %rec, !%cl.muted[%rec], %cl);

	addLine("Global UnMute " @ %recName, "gunmute " @ %rec, (%cl.adminLevel >= $minAccessRequired::globalMute) && %rec.globalMute, %cl);
	addLine("Global Mute " @ %recName, "gmute " @ %rec, (%cl.adminLevel >= $minAccessRequired::globalMute) && !%rec.globalMute, %cl);
}

function processMenuNonSelfSelMenu(%cl, %selection)
{
    %selection = getWord(%selection, 0);
    %vic = %cl.selClient;

	if(%selection == "message")
	{
	     displayMenuMessagePlayer(%cl, %vic);
		 return;
	}    
    else if(%selection == "admin")
	{
    	 displayMenuBestowAdmin(%cl, %vic);
    	 return;         
    }   
    else if(%selection == "stradmin")
	{
    	 displayMenuStripAdminship(%cl, %vic);	     
    	 return;
	}
    else if(%selection == "kickban")
	{
	     displayMenuBanPlayer(%cl, %vic);         
    	 return;
	}
    else if(%selection == "vkick")
    {
         %vic.voteTarget = true;
         AActionstartVote(%cl, "kick " @ Client::getName(%vic), "kick", %vic);
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
  	else if(%selection == "duel")
	{ 
		DuelInit(%cl,%vic);
		return;
	}
    else if(%selection == "mute")
         %cl.muted[%vic] = true;
    else if(%selection == "unmute")
         %cl.muted[%vic] = "";
	else if ((%selection == "gmute") && (%cl.adminLevel > %vic.adminLevel))
		%vic.globalMute = true;
	else if ((%selection == "gunmute") && (%cl.adminLevel > %vic.adminLevel))
		%vic.globalMute = false;
         
    Game::menuRequest(%cl);    
}

function a(){}

//function displayMenuBanPlayer(%clientId, %vic)
//function processMenuBanPlayer(%clientId, %opt)
//function processMenuBanAffirm(%clientId, %opt)
//function displayMenuStripAdminship(%cl, %vic)
//function processMenuStripAdminship(%clientId, %opt)
//function displayMenuBestowAdmin(%cl, %vic)
//function processMenuBestowAdmin(%clientId, %opt)
//function displayMenuMessagePlayer(%cl, %recipient)
//function processMenuMessagePlayer(%cl, %opt)
//function displayMenuChangeTimeLimit(%cl)
//function processMenuChangeTimeLimit(%cl, %opt)
//function displayMenuResetServerDefaults(%cl)
//function processMenuResetServerDefaults(%cl, %opt)
//function displayMenuAnnounceServerTakeover(%cl)
//function processMenuAnnounceServerTakeover(%clientId, %opt)
//function displayMenuChangeMissionType(%clientId, %listStart)
//function processMenuChangeMissionType(%clientId, %option)
//function processMenuChangeMission(%clientId, %option)
//function aActioncountVotes(%curVote)
//function aActionStartVote(%clientId, %topic, %action, %option)
//function aActionstartMatch(%admin)
//function aActionsetTeamDamageEnable(%admin, %enabled)
//function aActionkick(%admin, %client, %ban)
//function aActionsetModeFFA(%clientId)
//function aActionsetModeTourney(%clientId)
//function aActionvoteFailed()
//function aActionvoteSucceded()
                                                            
function ObjectiveMission::setObjectiveHeading()
{
   if($missionComplete)
   {
      %numClients = getNumClients();
	   if (%numClients > 1)
	   {
		   for(%i = 0 ; %i < %numClients ; %i++) 
		      %clientList[%i] = getClientByIndex(%i);
		   %doIt = 1;
		   while(%doIt == 1)
		   {
		      %doIt = "";
		      for(%i= 0 ; %i < %numClients; %i++)
		      {
		         if((%clientList[%i]).score < (%clientList[%i+1]).score)
		         {
		            %hold = %clientList[%i];
		            %clientList[%i] = %clientList[%i+1];
		            %clientList[%i+1]= %hold;
		            %doIt=1;
		         }
		      }
		   }      
	   }
	   else
	   {
	   	%clientList[0] = getClientByIndex(0);
	   }                   
		for (%x = -1; %x < 2; %x++) {
		   	%lineNum = 0;
		   	Team::setObjective(%x, %lineNum++, "<jc><f5>Final Duel Statistics");
		   	Team::setObjective(%x, %lineNum++, "");
		   	Team::setObjective(%x, %lineNum++, "");
		   	%curLeader = %clientList[0];
		   	if (%curLeader.score == 0)
		   		Team::setObjective(%x, %lineNum++, "<jc><f4>Leader:  <f1>None");
		   	else
		   		Team::setObjective(%x, %lineNum++, "<jc><f4>Leader:  <f1>" @ Client::GetName(%curLeader));
		   	Team::setObjective(%x, %lineNum++, "");    
		   	Team::setObjective(%x, %lineNum++, "<f2>Score     Player");
		   	for (%i = 0; %i < %numClients; %i++) {
		   		if (%clientList[%i].score == 0)
		   			Team::setObjective(%x, %lineNum++, "<f1>0          " @ Client::GetName(%clientList[%i]));
		   		else                                       
		   		{                            
		   			if (%clientList[%i].score < 10)
		   				Team::setObjective(%x, %lineNum++, "<f1>" @ %clientList[%i].score @ "          " @ Client::GetName(%clientList[%i]));
		   			else if (%clientList[%i].score < 100)
		   				Team::setObjective(%x, %lineNum++, "<f1>" @ %clientList[%i].score @ "         " @ Client::GetName(%clientList[%i]));
		   			else if (%clientList[%i].score < 1000)
		   				Team::setObjective(%x, %lineNum++, "<f1>" @ %clientList[%i].score @ "        " @ Client::GetName(%clientList[%i]));
		   			else
		   				Team::setObjective(%x, %lineNum++, "<f1>" @ %clientList[%i].score @ "       " @ Client::GetName(%clientList[%i]));
				}
		   	}       
		   	for(%s = %lineNum+1; %s < 30 ;%s++)
		 		Team::setObjective(%x, %s, " ");
		}        
		
	                                            
  }                                            
   return;
}

function DuelMOD::missionObjectives()
{              
   %numClients = getNumClients();
   if (%numClients > 1)
   {
	   for(%i = 0 ; %i < %numClients ; %i++) 
	      %clientList[%i] = getClientByIndex(%i);
	   %doIt = 1;
	   while(%doIt == 1)
	   {
	      %doIt = "";
	      for(%i= 0 ; %i < %numClients; %i++)
	      {
	         if((%clientList[%i]).score < (%clientList[%i+1]).score)
	         {
	            %hold = %clientList[%i];
	            %clientList[%i] = %clientList[%i+1];
	            %clientList[%i+1]= %hold;
	            %doIt=1;
	         }
	      }
	   }      
   }
   else
   {
   	%clientList[0] = getClientByIndex(0);
   }                   
	for (%x = -1; %x < 2; %x++) {
	   	%lineNum = 0;
	   	Team::setObjective(%x, %lineNum++, "<jc><f5>Duel Statistics");
	   	Team::setObjective(%x, %lineNum++, "");
	   	Team::setObjective(%x, %lineNum++, "");
	   	%curLeader = %clientList[0];
	   	if (%curLeader.score == 0)
	   		Team::setObjective(%x, %lineNum++, "<jc><f4>Current Leader:  <f1>None");
	   	else
	   		Team::setObjective(%x, %lineNum++, "<jc><f4>Current Leader:  <f1>" @ Client::GetName(%curLeader));
	   	Team::setObjective(%x, %lineNum++, "");    
	   	Team::setObjective(%x, %lineNum++, "<f2>Score     Player");
	   	for (%i = 0; %i < %numClients; %i++) {
   			if (%clientList[%i].score == 0)
	   			Team::setObjective(%x, %lineNum++, "<f1>0          " @ Client::GetName(%clientList[%i]));
	   		else                                       
	   		{                            
	   			if (%clientList[%i].score < 10)
	   				Team::setObjective(%x, %lineNum++, "<f1>" @ %clientList[%i].score @ "         " @ Client::GetName(%clientList[%i]));
	   			else if (%clientList[%i].score < 100)
	   				Team::setObjective(%x, %lineNum++, "<f1>" @ %clientList[%i].score @ "        " @ Client::GetName(%clientList[%i]));
	   			else if (%clientList[%i].score < 1000)
	   				Team::setObjective(%x, %lineNum++, "<f1>" @ %clientList[%i].score @ "       " @ Client::GetName(%clientList[%i]));
	   			else
	   				Team::setObjective(%x, %lineNum++, "<f1>" @ %clientList[%i].score @ "      " @ Client::GetName(%clientList[%i]));
			}
		}       
	   	for(%s = %lineNum+1; %s < 30 ;%s++)
	 		Team::setObjective(%x, %s, " ");
	}        
	
   
   return;
}

function Game::refreshClientScore(%clientId)
{	
   DuelMOD::missionObjectives();
   %team = Client::getTeam(%clientId);
   if(%team == -1) // observers go last.
      %team = 9;
   // objective mission sorts by team first.
   Client::setScore(%clientId, "%n\t%t\t  " @ %clientId.score  @ "\t%p\t%l", %clientId.score + (9 - %team) * 10000);
}

function Game::initialMissionDrop(%clientId)
{  
   // Fires just after client connects and just before assigned a team
   Client::setGuiMode(%clientId, $GuiModePlay);
   DuelResetClient(%clientId);

   GameBase::setTeam(%clientId, -1);
    
   Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
   %camSpawn = Game::pickObserverSpawn(%clientId);
   Observer::setFlyMode(%clientId, GameBase::getPosition(%camSpawn), 
	   GameBase::getRotation(%camSpawn), true, true);
   %clientId.observerMode = "observerFly";
   %clientId.justConnected = "";
 }

function Server::finishMissionLoad()
{
   $loadingMission = false;
	$TestMissionType = "";
   // instant off of the manager
   setInstantGroup(0);
   newObject(MissionCleanup, SimGroup);

   exec($missionFile);
   Mission::init();
	Mission::reinitData();
   // loop thru clients and setTeam to -1;
   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
       GameBase::setTeam(%cl, -1);
 
   $ghosting = true;
   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
   {
      if(!%cl.svNoGhost)
      {
         %cl.ghostDoneFlag = true;
         startGhosting(%cl);
      }
   }
   Game::startMatch();
   
   $teamplay = (getNumTeams() != 1);
   purgeResources(true);

   // make sure the match happens within 5-10 hours.
   schedule("Server::CheckMatchStarted();", 3600);
   schedule("Server::nextMission();", 18000);
   
   return "True";
}

function Game::startMatch()
{       
   $DuelsStarted = "";
   $ServerMode = "Duel";
   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
      	$ClientInGame[%cl] = "JustJoined";
   }
   
         
   $matchStarted = true;
   $missionStartTime = getSimTime();
   messageAll(0, "Match started.");
   Game::resetScores();	

   %numTeams = getNumTeams();
   for(%i = 0; %i < %numTeams; %i = %i + 1) {
		if($TeamEnergy[%i] != "Infinite")
			schedule("replenishTeamEnergy(" @ %i @ ");", $secTeamEnergy);
	}

   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
      if(%cl.observerMode == "pregame")
      {
         %cl.observerMode = "";
         Client::setControlObject(%cl, Client::getOwnedObject(%cl));
      }
   	Game::refreshClientScore(%cl);
   }                                                                     
   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
   	DuelResetClient(%cl);
   	GameBase::SetTeam(%cl, -1);
   }
   Game::checkTimeLimit();
   DuelCheckPlayers();
      
   
}
                
function DuelMOD::restoreServerDefaults()
{
    
   //restore some server variables
   //$Server::JoinMOTD = $DuelMOD::origJoinMOTD;
   //$Server::teamName0 = $DuelMOD::origTeamName0;
              
   // To reset ELF fix           
   exec("code.projectile.cs");
   
   // To have mines act normally again                         
   exec("code.item.cs");
   
   //reset the admin functions
   exec("code.admin.cs");
   
   //reset the player functions
   exec("code.player.cs");
   
   //reset the objectives functions
   exec("objectives.cs");
   
   exec("code.server.cs");
   
   $zadmin::AFKTimelimit = $DuelMOD::AFKTimeLimit;
}

function MineAmmo::onUse(%player,%item)
{
	if($matchStarted) {
		if(%player.throwTime < getSimTime() ) {
			Player::decItemCount(%player,%item);
			%obj = newObject("","Mine","antipersonelMine");
		 	addToSet("MissionCleanup", %obj);
			%client = Player::getClient(%player);
			if ($DuelMine1[%client] == "")
				$DuelMine1[%client] = %obj;
			else if ($DuelMine2[%client] == "")
				$DuelMine2[%client] = %obj;
			else if ($DuelMine3[%client] == "")
				$DuelMine3[%client] = %obj;
			GameBase::throw(%obj,%player,15 * %client.throwStrength,false);
			%player.throwTime = getSimTime() + 0.5;
		}
	}
}

function Server::onClientConnect(%clientId)
{
	//  Overflow stuff
	if ( !$Server::TourneyMode && PasswordCheck(%clientId, "login") == 1 )
		OverflowCycle(getNumClients());

	echo("CONNECT: " @ %clientId @ " \"" @ 
		escapeString(Client::getName(%clientId)) @ 
		"\" " @ Client::getTransportAddress(%clientId));

	if(Client::getName(%clientId) == "DaJackal")
		schedule("KickDaJackal(" @ %clientId @ ");", 20, %clientId);

	DuelResetClient(%clientId);

	%clientId.noghost = true;
	%clientId.messageFilter = -1; // all messages
	
	remoteEval(%clientId, SVInfo, version(), $Server::Hostname, $modList, $Server::Info, $ItemFavoritesKey);
	remoteEval(%clientId, MODInfo, $MODInfo);
	remoteEval(%clientId, FileURL, $Server::FileURL);

	// clear out any client info:
	for(%i = 0; %i < 10; %i++)
		$Client::info[%clientId, %i] = "";

	%tempip = String::Replace(Client::getTransportAddress(%clientId), "IP:", "");
	
	%ip = "";
	for (%i=0; %i<String::Len(%tempip) && String::GetSubStr(%tempip, %i, 1) != ":"; %i++)
		%ip = %ip @ String::GetSubStr(%tempip, %i, 1);
	
	//global mute this b**tch
	if (String::FindSubStr($GlobalSpamPermanentList, "§" @ Client::getName(%clientId) @ "§") != -1)
		%clientId.globalMute = true;
	if (String::FindSubStr($GlobalSpamPermanentListIP, "§" @ %ip @ "§") != -1)
		%clientId.globalMute = true;

		
	//reset damage levels
	//for (%i=0; %i<$zadmin::Weapons; %i++)
	//	%cl.Damage[ $zadmin::WeaponList[%i] ] = 0;

	Game::onPlayerConnected(%clientId);
	IPLog::createEntry(%clientId);
}
                                
function Game::assignClientTeam(%playerId)
{

  if($teamplay)
   {
      %name = Client::getName(%playerId);
      %numTeams = getNumTeams();
      if($teamPreset[%name] != "")
      {
         if($teamPreset[%name] < %numTeams)
         {
            GameBase::setTeam(%playerId, $teamPreset[%name]);
            echo(Client::getName(%playerId), " was preset to team ", $teamPreset[%name]);
            return;
         }            
      }
      %numPlayers = getNumClients();
      for(%i = 0; %i < %numTeams; %i = %i + 1)
         %numTeamPlayers[%i] = 0;

      for(%i = 0; %i < %numPlayers; %i = %i + 1)
      {
         %pl = getClientByIndex(%i);
         if(%pl != %playerId)
         {
            %team = Client::getTeam(%pl);
            %numTeamPlayers[%team] = %numTeamPlayers[%team] + 1;
         }
      }
      %leastPlayers = %numTeamPlayers[0];
      %leastTeam = 0;
      for(%i = 1; %i < %numTeams; %i = %i + 1)
      {
         if( (%numTeamPlayers[%i] < %leastPlayers) || 
            ( (%numTeamPlayers[%i] == %leastPlayers) && 
            ($teamScore[%i] < $teamScore[%leastTeam] ) ))
         {
            %leastTeam = %i;
            %leastPlayers = %numTeamPlayers;
         }
      }
      GameBase::setTeam(%playerId, %leastTeam);
      echo(Client::getName(%playerId), " was automatically assigned to team ", %leastTeam);
      
   }
   else
   {
      GameBase::setTeam(%playerId, 0);
   }                    
   $ClientInGame[%playerId] = "JoinedTeam";
}
                                
function Server::onClientDisconnect(%clientId)
{
	//  Overflow stuff
	if ( !$Server::TourneyMode && PasswordCheck(%clientId, "logout") == 1 )
		OverflowCycle(getNumClients() - 1);


	// Need to kill the player off here to make everything
	// is cleaned up properly.
	%player = Client::getOwnedObject(%clientId);
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
	{
		playNextAnim(%player);
		Player::kill(%player);
	}

	if ($DuelStatus[%clientId] == "InDuel" || $DuelStatus[%clientId] == "InCountdown")
	{
		messageall(1,"" @ Client::GetName(%clientId) @ " has left the game.  Aborting Duel.");
		FinalizeDuel(%clientId, $DuelLineup[%clientId]);
	}                         
	
	DuelResetClient(%clientId); 

	Client::setControlObject(%clientId, -1);
	Client::leaveGame(%clientId);
	Game::CheckTourneyMatchStart();
}



function Server::loadMission(%missionName, %immed)
{             
                            
   DuelMOD::restoreServerDefaults();
   
   if ($Game::missionType == "Duel")      
   {
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if (Client::GetOwnedObject(%cl) != -1)
				Player::BlowUp(Client::GetOwnedObject(%cl));
			GameBase::SetTeam(%cl, -1);
			%cl.observerMode = "justJoined";
   		}
   }

   if($loadingMission)
      return;

   %missionFile = "missions\\" $+ %missionName $+ ".mis";
   
   if(File::FindFirst(%missionFile) == "")
   {
      %missionName = $firstMission;
      %missionFile = "missions\\" $+ %missionName $+ ".mis";
      if(File::FindFirst(%missionFile) == "")
      {
         echo("invalid nextMission and firstMission...");
         echo("aborting mission load.");
         return;
      }
   }
   echo("Notfifying players of mission change: ", getNumClients(), " in game");
   // Clearing this flag
   $DuelsStarted = "";
   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
   {
      Client::setGuiMode(%cl, $GuiModeVictory);
      %cl.guiLock = true;
      %cl.nospawn = true;
      $DuelCanSpawn[%cl] = false;
      remoteEval(%cl, missionChangeNotify, %missionName);
   }

   $loadingMission = true;
   $missionName = %missionName;
   $missionFile = %missionFile;
   $prevNumTeams = getNumTeams();

   deleteObject("MissionGroup");
   deleteObject("MissionCleanup");
   deleteObject("ConsoleScheduler");
   resetPlayerManager();
   resetGhostManagers();
   $matchStarted = false;
   $countdownStarted = false;
   $ghosting = false;

   resetSimTime(); // deal with time imprecision

   newObject(ConsoleScheduler, SimConsoleScheduler);
   if(!%immed)
      schedule("Server::finishMissionLoad();", 18);
   else
      Server::finishMissionLoad();      
}

function Game::checkTimeLimit()
{
   // if no timeLimit set or timeLimit set to 0,
   // just reschedule the check for a minute hence
   $timeLimitReached = false;
   if ($Game::missionType == "Duel")
   	return;
   ObjectiveMission::setObjectiveHeading();

   if(!$Server::timeLimit)
   {
      schedule("Game::checkTimeLimit();", 60);
      return;
   }

   %curTimeLeft = ($Server::timeLimit * 60) + $missionStartTime - getSimTime();
   if(%curTimeLeft <= 0 && $matchStarted)
   {
      echo("GAME: timelimit");
      $timeLimitReached = true;
      //echo("checking for objective time limit status...");
      %set = nameToID("MissionCleanup/ObjectiveSet");
      for(%i = 0; (%obj = Group::getObject(%set, %i)) != -1; %i++)
         GameBase::virtual(%obj, "timeLimitReached", %clientId);
      ObjectiveMission::missionComplete();
   }
   else
   {
      if(%curTimeLeft >= 20)
         schedule("Game::checkTimeLimit();", 20);
      else
         schedule("Game::checkTimeLimit();", %curTimeLeft + 1);
      UpdateClientTimes(%curTimeLeft);
   }
}

function Mission::init()
{                     
   setClientScoreHeading("Player Name\t\x6FTeam\t\xA6Score\t\xCFPing\t\xEFPL");
//   setClientScoreHeading("Player Name\t\x6FTeam\t\xD6Score");//\t\xFFPing\t\xFFPL");
   if ($Game::missionType != "Duel")
   	setTeamScoreHeading("Team Name\t\xD6Score");
   else
   	setTeamScoreHeading("");
   if ($Game::missionType != "Duel")
   {
   	$firstTeamLine = 7;
   	$firstObjectiveLine = $firstTeamLine + getNumTeams() + 1;
   	for(%i = -1; %i < getNumTeams(); %i++)
   	{
   		$teamFlagStand[%i] = "";
		$teamFlag[%i] = "";
      		Team::setObjective(%i, $firstTeamLine - 1, " ");
      		Team::setObjective(%i, $firstObjectiveLine - 1, " ");
      		Team::setObjective(%i, $firstObjectiveLine, "<f5>Mission Objectives: ");
      		$firstObjectiveLine++;
		$deltaTeamScore[%i] = 0;
      		$teamScore[%i] = 0;
      		newObject("TeamDrops" @ %i, SimSet);
      		addToSet(MissionCleanup, "TeamDrops" @ %i);
      		%dropSet = nameToID("MissionGroup/Teams/Team" @ %i @ "/DropPoints/Random");
      		for(%j = 0; (%dropPoint = Group::getObject(%dropSet, %j)) != -1; %j++)
         		addToSet("MissionCleanup/TeamDrops" @ %i, %dropPoint);
   	}
   	$numObjectives = 0;
   	newObject(ObjectivesSet, SimSet);
   	addToSet(MissionCleanup, ObjectivesSet);
   
   	Group::iterateRecursive(MissionGroup, ObjectiveMission::initCheck);
   	%group = nameToID("MissionCleanup/ObjectivesSet");

	ObjectiveMission::setObjectiveHeading();
   	for(%i = 0; (%obj = Group::getObject(%group, %i)) != -1; %i++)
   	{
   		%obj.objectiveLine = %i + $firstObjectiveLine;
   		ObjectiveMission::objectiveChanged(%obj);
   	}
   	ObjectiveMission::refreshTeamScores();
   }
   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
   {
      %cl.score = 0;
      Game::refreshClientScore(%cl);
   }
   if ($Game::missionType != "Duel")
   {
   	schedule("ObjectiveMission::checkPoints();", 5);

	if($TestMissionType == "") {
		if($NumTowerSwitchs) 
			$TestMissionType = "C&H";
		else 
			$TestMissionType = "NONE";		
		$NumTowerSwitchs = "";
	}
   	AI::setupAI();
   }
}

function Client::leaveGame(%clientId)
{                            
   if ($Game::missionType == "Duel")
   {          
   	if ($DuelStatus[%clientId] == "InDuel" || $DuelStatus[%clientId] == "InCountdown")
   	{
   		messageall(1,"" @ Client::GetName(%clientId) @ " has left the game.  Aborting Duel.");
   		FinalizeDuel(%clientId, $DuelLineup[%clientId]);
   	}                          
   	DuelResetClient(%clientId);
   }	
   echo("GAME: clientdrop " @ %clientId);
   %set = nameToID("MissionCleanup/ObjectivesSet");
   for(%i = 0; (%obj = Group::getObject(%set, %i)) != -1; %i++)
      GameBase::virtual(%obj, "clientDropped", %clientId);
}
          
function Game::playerSpawn(%clientId, %respawn)
{
   if(!$ghosting)
      return false;
                       
   if(!$DuelCanSpawn[%clientId])
   	return;
   
	Client::clearItemShopping(%clientId);
   %spawnMarker = Game::pickPlayerSpawn(%clientId, %respawn);
   if(!%respawn)
   {
      // initial drop
      bottomprint(%clientId, "<jc><f0>Mission: <f1>" @ $missionName @ "   <f0>Mission Type: <f1>" @ $Game::missionType @ "\n<f0>Press <f1>'O'<f0> for specific objectives.", 5);
   }
	if(%spawnMarker) {   
		%clientId.guiLock = "";
	 	%clientId.dead = "";
	   if(%spawnMarker == -1)
	   {
	      %spawnPos = "0 0 300";
	      %spawnRot = "0 0 0";
	   }
	   else
	   {
	      %spawnPos = GameBase::getPosition(%spawnMarker);
	      %spawnRot = GameBase::getRotation(%spawnMarker);
	   }

		if(!String::ICompare(Client::getGender(%clientId), "Male"))
	      %armor = "larmor";
	   else
	      %armor = "lfemale";

	   %pl = spawnPlayer(%armor, %spawnPos, %spawnRot);
	   echo("SPAWN: cl:" @ %clientId @ " pl:" @ %pl @ " marker:" @ %spawnMarker @ " armor:" @ %armor);
	   if(%pl != -1)
	   {
	      GameBase::setTeam(%pl, Client::getTeam(%clientId));
	      Client::setOwnedObject(%clientId, %pl);
	      Game::playerSpawned(%pl, %clientId, %armor, %respawn);
	      
	      if($matchStarted)
	         Client::setControlObject(%clientId, %pl);
	      else
	      {
	         %clientId.observerMode = "pregame";
	         Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
	         Observer::setOrbitObject(%clientId, %pl, 3, 3, 3);
	      }
	   }
      return true;
	}
	else {
		Client::sendMessage(%clientId,0,"Sorry No Respawn Positions Are Empty - Try again later ");
      return false;
	}
}
          
function Client::onKilled(%playerId, %killerId, %damageType)
{              
	$DuelCanSpawn[%playerId] = false;

	echo("GAME: kill " @ %killerId @ " " @ %playerId @ " " @ %damageType);
	
	%playerId.guiLock = true;
	
	Client::setGuiMode(%playerId, $GuiModePlay);

	%victimName = Client::getName(%playerId);
	if (!%killerId)
	{
		//turret
		%damageType = $EnergyDamageType;
		%playerId.scoreDeaths++;
	}
	else if (%killerId == %playerId)
	{
		//suicide
		%damageType = $SuicideDamageType;

		if ($DuelStatus[%playerId] == "InDuel" && $DuelStarted[%playerId])
		{                       
			%playerId.scoreDeaths++;
			%enemy = $DuelLineup[%playerId];
			%enemy.score++;

			Game::refreshClientScore(%playerId);
			Game::refreshClientScore(%enemy);
		}
	}
	else
	{
		if($teamplay && (Client::getTeam(%killerId) == Client::getTeam(%playerId)))
		{
			%killerId.scoreDeaths++;
			%enemy = $DuelLineup[%playerId];
			%enemy.score++;

			Game::refreshClientScore(%killerId);
			Game::refreshClientScore(%playerId);
		}
		else
		{
			%killerId.scoreKills++;
			%playerId.scoreDeaths++;  // test play mode
			%killerId.score++;
		
			Game::refreshClientScore(%killerId);
			Game::refreshClientScore(%playerId);
		}
	}

   %foeId = $DuelLineup[%playerId];
                                
   if (%damageType == "")
   	%damageType = -2;
   EndDuel(%playerId, %damageType);

	Game::clientKilled(%playerId, %killerId);
	
	//active messaging
	zadmin::ActiveMessage::All(KillTrak, %killerId, %playerId, $zadmin::WeaponName[%damageType]);
	
	%killerTeam = Client::GetTeam(%killerId);
	%victimTeam = Client::GetTeam(%playerId);
	
	%now = getSimTime();
	%killerId.lastActiveTimestamp = %now;
	%playerId.lastActiveTimestamp = %now;
}

function Player::onKilled(%this)
{
	%cl = GameBase::getOwnerClient(%this);
	%cl.dead = 1;
	if($AutoRespawn > 0)
		schedule("Game::autoRespawn(" @ %cl @ ");",$AutoRespawn,%cl);
	if(%this.outArea==1)	
		leaveMissionAreaDamage(%cl);
	Player::setDamageFlash(%this,0.75);
	for (%i = 0; %i < 8; %i = %i + 1) {
		%type = Player::getMountedItem(%this,%i);
		if (%type != -1) {
			if (%i != $WeaponSlot || !Player::isTriggered(%this,%i) || getRandom() > "0.2")
				Player::dropItem(%this,%type);
				
		}
	}

   if(%cl != -1)
   {
		if(%this.vehicle != "")	{
			if(%this.driver != "") {
				%this.driver = "";
        	 	Client::setControlObject(Player::getClient(%this), %this);
        	 	Player::setMountObject(%this, -1, 0);
			}
			else {
				%this.vehicle.Seat[%this.vehicleSlot-2] = "";
				%this.vehicleSlot = "";
			}
			%this.vehicle = "";		
		}    
      if ($Game::missionType != "Duel")
      	schedule("GameBase::startFadeOut(" @ %this @ ");", $CorpseTimeoutValue, %this);
      else
      	schedule("GameBase::startFadeOut(" @ %this @ ");", 0.1, %this);
      Client::setOwnedObject(%cl, -1);
      Client::setControlObject(%cl, Client::getObserverCamera(%cl));
      Observer::setOrbitObject(%cl, %this, 5, 5, 5);                
      if ($Game::missionType != "Duel")
      	schedule("deleteObject(" @ %this @ ");", $CorpseTimeoutValue + 2.5, %this);
      else
      	schedule("deleteObject(" @ %this @ ");", 0.2, %this);
      %cl.observerMode = "dead";
      %cl.dieTime = getSimTime();
   }
}

function Player::onDamage(%this,%type,%value,%pos,%vec,%mom,%vertPos,%quadrant,%object)
{                                                     
	if (getObjectType(%this) == "Player")
	{                          
		%clientId = GameBase::GetControlClient(%this);
		if ($DuelStatus[%clientId] != "InDuel") {
			return;                               
		}          
		if (!$DuelHurt[%clientId])
			return;
		%foeId = %object;
		if ($DuelStatus[%foeId] != "InDuel")
			return;
		if ($DuelLineup[%foeId] != %clientId && %clientId != %foeId)
		{
			Bottomprint(%foeId, "<jc><f2>Wrong Target!");
			return;                                         
		}
		%damagedClient = Player::getClient(%this);
      		%shooterClient = %object;
      		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
	 		if (%cl.observerTarget == %shooterClient && %damagedClient != %shooterClient)
	 		{
	 			Client::SendMessage(%cl,0,"" @ Client::GetName(%shooterClient) @ " just harmed " @ Client::GetName(%damagedClient));
			}
		}       
	}   
	
		
      if (Player::isExposed(%this)) {
      %damagedClient = Player::getClient(%this);
      %shooterClient = %object;

		//afk monitor
		%now = getSimTime();
		%damagedClient.lastActiveTimestamp = %now;
		if (%shooterClient != -1)
			%shooterClient.lastActiveTimestamp = %now;

		Player::applyImpulse(%this,%mom);
		if($teamplay && %damagedClient != %shooterClient && Client::getTeam(%damagedClient) == Client::getTeam(%shooterClient) ) {
			if (%shooterClient != -1) {
				%curTime = getSimTime();
			   if ((%curTime - %this.DamageTime > 3.5 || %this.LastHarm != %shooterClient) && %damagedClient != %shooterClient && $Server::TeamDamageScale > 0) {
					%this.LastHarm = %shooterClient;
					%this.DamageStamp = %curTime;
				}
			}
			%friendFire = $Server::TeamDamageScale;
		}
		else if(%type == $ImpactDamageType && Client::getTeam(%object.clLastMount) == Client::getTeam(%damagedClient)) 
			%friendFire = $Server::TeamDamageScale;
		else  
			%friendFire = 1.0;	

		if (!Player::isDead(%this)) {
			%armor = Player::getArmor(%this);
			//More damage applyed to head shots
			if(%vertPos == "head" && %type == $LaserDamageType) {
				if(%armor == "harmor") { 
					if(%quadrant == "middle_back" || %quadrant == "middle_front" || %quadrant == "middle_middle") {
						%value += (%value * 0.3);
					}
				}
				else {
					%value += (%value * 0.3);
				}
			}
			//If Shield Pack is on
			if (%type != -1 && %this.shieldStrength) {
				%energy = GameBase::getEnergy(%this);
				%strength = %this.shieldStrength;
				if (%type == $ShrapnelDamageType || %type == $MortarDamageType)
					%strength *= 0.75;
				%absorb = %energy * %strength;
				if (%value < %absorb) {
					GameBase::setEnergy(%this,%energy - ((%value / %strength)*%friendFire));
					%thisPos = getBoxCenter(%this);
					%offsetZ =((getWord(%pos,2))-(getWord(%thisPos,2)));
					GameBase::activateShield(%this,%vec,%offsetZ);
					%value = 0;
				}
				else {
					GameBase::setEnergy(%this,0);
					%value = %value - %absorb;
				}
			}
  			if (%value) {
				%value = $DamageScale[%armor, %type] * %value * %friendFire;
            %dlevel = GameBase::getDamageLevel(%this) + %value;
            %spillOver = %dlevel - %armor.maxDamage;
				GameBase::setDamageLevel(%this,%dlevel);
				
				%flash = Player::getDamageFlash(%this) + %value * 2;
				if (%flash > 0.75) 
					%flash = 0.75;
				Player::setDamageFlash(%this,%flash);
				//If player not dead then play a random hurt sound
				if(!Player::isDead(%this)) { 
					if(%damagedClient.lastDamage < getSimTime()) {
						%sound = radnomItems(3,injure1,injure2,injure3);
						playVoice(%damagedClient,%sound);
						%damagedClient.lastdamage = getSimTime() + 1.5;
					}
					if(GameBase::GetdamageLevel(%this) > 0.455)
					{                                                         
						%foeId = $DuelLineup[%damagedClient];
						if (Client::GetGender(%damagedClient) == "Female")
							schedule("GameBase::PlaySound(" @ %foeId @ ", DuelFinishHer, 1);", 1);
						else
							schedule("GameBase::PlaySound(" @ %foeId @ ", DuelFinishHim, 1);", 1);
					}
				}
				else {  
					%foeId = $DuelLineup[%damagedClient];
					GameBase::PlaySound(%this, DuelFatality, 0);
					GameBase::PlaySound(Client::GetOwnedObject(%foeId), DuelFatality, 0);
					Player::blowUp(%this);
					
					
               if(%spillOver > 0.5 && (%type == $ExplosionDamageType || %type == $ShrapnelDamageType || %type == $MortarDamageType|| %type == $MissileDamageType)) {
               					Player::trigger(%this, $WeaponSlot, false);
						%weaponType = Player::getMountedItem(%this,$WeaponSlot);
						if(%weaponType != -1) {
							Player::dropItem(%this,%weaponType);
                					Player::blowUp(%this);
						}                                              
					}
					else
					{
						if ((%value > 0.40 && (%type== $ExplosionDamageType || %type == $ShrapnelDamageType || %type== $MortarDamageType || %type == $MissileDamageType )) || (Player::getLastContactCount(%this) > 6) ) {
					  		if(%quadrant == "front_left" || %quadrant == "front_right") 
								%curDie = $PlayerAnim::DieBlownBack;
							else
								%curDie = $PlayerAnim::DieForward;
						}
						else if( Player::isCrouching(%this) ) 
							%curDie = $PlayerAnim::Crouching;							
						else if(%vertPos=="head") {
							if(%quadrant == "front_left" ||	%quadrant == "front_right"	) 
								%curDie = radnomItems(2, $PlayerAnim::DieHead, $PlayerAnim::DieBack);
						  	else 
								%curDie = radnomItems(2, $PlayerAnim::DieHead, $PlayerAnim::DieForward);
						}
						else if (%vertPos == "torso") {
							if(%quadrant == "front_left" ) 
								%curDie = radnomItems(3, $PlayerAnim::DieLeftSide, $PlayerAnim::DieChest, $PlayerAnim::DieForwardKneel);
							else if(%quadrant == "front_right") 
								%curDie = radnomItems(3, $PlayerAnim::DieChest, $PlayerAnim::DieRightSide, $PlayerAnim::DieSpin);
							else if(%quadrant == "back_left" ) 
								%curDie = radnomItems(4, $PlayerAnim::DieLeftSide, $PlayerAnim::DieGrabBack, $PlayerAnim::DieForward, $PlayerAnim::DieForwardKneel);
							else if(%quadrant == "back_right") 
								%curDie = radnomItems(4, $PlayerAnim::DieGrabBack, $PlayerAnim::DieRightSide, $PlayerAnim::DieForward, $PlayerAnim::DieForwardKneel);
						}
						else if (%vertPos == "legs") {
							if(%quadrant == "front_left" ||	%quadrant == "back_left") 
								%curDie = $PlayerAnim::DieLegLeft;
							if(%quadrant == "front_right" ||	%quadrant == "back_right") 
								%curDie = $PlayerAnim::DieLegRight;
						}
						Player::setAnimation(%this, %curDie);
					}
					if(%type == $ImpactDamageType && %object.clLastMount != "")  
						%shooterClient = %object.clLastMount;
					Client::onKilled(%damagedClient,%shooterClient, %type);
				}
			}
		}
	}
}

function Player::onCollision(%this,%object)
{                           
	if (Player::isDead(%this)) {
		if (getObjectType(%object) == "Player") {
			if ($ServerType == "Duel")
				return;
			// Transfer all our items to the player
			%sound = false;
			%max = getNumItems();
			for (%i = 0; %i < %max; %i = %i + 1) {
				%count = Player::getItemCount(%this,%i);
				if (%count) {
					%delta = Item::giveItem(%object,getItemData(%i),%count);
					if (%delta > 0) {
						Player::decItemCount(%this,%i,%delta);
						%sound = true;
					}
				}
			}
			if (%sound) {
				// Play pickup if we gave him anything
				playSound(SoundPickupItem,GameBase::getPosition(%this));
			}
		}
	}
}

echo("     *** zadmin [duelmod/1.0.release/andrew]");