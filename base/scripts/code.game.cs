exec("code.komchat.cs");
exec("code.server.cs");
$SensorNetworkEnabled = true;

$GuiModePlay = 1;
$GuiModeCommand = 2;
$GuiModeVictory = 3;
$GuiModeInventory = 4;
$GuiModeObjectives = 5;
$GuiModeLobby = 6;

//lookup variables for active mode
$UnknownDamageType = "";
$TeamkillDamageType = -4;
$MineTeamkillDamageType = -3;
$SuicideDamageType = -2;

$zadmin::WeaponName[$UnknownDamageType]			= "Unknown";
$zadmin::WeaponName[$TeamkillDamageType]		= "Teamkill";
$zadmin::WeaponName[$MineTeamkillDamageType]	= "MineTeamkill";
$zadmin::WeaponName[$SuicideDamageType]			= "Suicide";
$zadmin::WeaponName[$ImpactDamageType]			= "Vehicle";
$zadmin::WeaponName[$LandingDamageType]			= "Suicide";
$zadmin::WeaponName[$BulletDamageType]			= "Chaingun";
$zadmin::WeaponName[$EnergyDamageType]			= "Turret";
$zadmin::WeaponName[$PlasmaDamageType]			= "Plasma";
$zadmin::WeaponName[$ExplosionDamageType]		= "Disc Launcher";
$zadmin::WeaponName[$ShrapnelDamageType]		= "Explosives";
$zadmin::WeaponName[$LaserDamageType]			= "Laser Rifle";
$zadmin::WeaponName[$MortarDamageType]			= "Mortar";
$zadmin::WeaponName[$BlasterDamageType]			= "Blaster";
$zadmin::WeaponName[$ElectricityDamageType]		= "Elf Gun";
$zadmin::WeaponName[$CrushDamageType]			= "Crushed";
$zadmin::WeaponName[$DebrisDamageType]			= "Explosion";
$zadmin::WeaponName[$MissileDamageType]			= "Missile";
$zadmin::WeaponName[$MineDamageType]			= "Explosives";

$zadmin::Weapons = 0;
$zadmin::WeaponList[$zadmin::Weapons++ -1] = "Chaingun";
$zadmin::WeaponList[$zadmin::Weapons++ -1] = "Plasma";
$zadmin::WeaponList[$zadmin::Weapons++ -1] = "Disc Launcher";
$zadmin::WeaponList[$zadmin::Weapons++ -1] = "Explosives";
$zadmin::WeaponList[$zadmin::Weapons++ -1] = "Laser Rifle";
$zadmin::WeaponList[$zadmin::Weapons++ -1] = "Mortar";
$zadmin::WeaponList[$zadmin::Weapons++ -1] = "Blaster";

//  Global Variables

//---------------------------------------------------------------------------------
// Energy each team is given at beginning of game
//---------------------------------------------------------------------------------
$DefaultTeamEnergy = "Infinite";

//---------------------------------------------------------------------------------
// Team Energy variables
//---------------------------------------------------------------------------------
$TeamEnergy[-1] = $DefaultTeamEnergy;
$TeamEnergy[0]  = $DefaultTeamEnergy;
$TeamEnergy[1]  = $DefaultTeamEnergy;
$TeamEnergy[2]  = $DefaultTeamEnergy;
$TeamEnergy[3]  = $DefaultTeamEnergy;
$TeamEnergy[4]  = $DefaultTeamEnergy;
$TeamEnergy[5]  = $DefaultTeamEnergy;
$TeamEnergy[6]  = $DefaultTeamEnergy;
$TeamEnergy[7]  = $DefaultTeamEnergy;

//---------------------------------------------------------------------------------
// Time in sec player must wait before he can throw a Grenade or Mine after leaving
//	a station.
//---------------------------------------------------------------------------------
$WaitThrowTime = 2;

//---------------------------------------------------------------------------------
// If 1 then Team Spending Ignored -- Team Energy is set to $MaxTeamEnergy every
// 	$secTeamEnergy.
//---------------------------------------------------------------------------------
$TeamEnergyCheat = 0;

//---------------------------------------------------------------------------------
// MAX amount team energy can reach
//---------------------------------------------------------------------------------
$MaxTeamEnergy = 700000;

//---------------------------------------------------------------------------------
//  Time player has to put flag in flagstand before it gets returned to its last
//  location.
//---------------------------------------------------------------------------------
$flagToStandTime = 180;

//---------------------------------------------------------------------------------
// Amount to inc team energy every ($secTeamEnergy) seconds
//---------------------------------------------------------------------------------
$incTeamEnergy = 700;

//---------------------------------------------------------------------------------
// (Rate is sec's) Set how often TeamEnergy is incremented
//---------------------------------------------------------------------------------
$secTeamEnergy = 30;

//---------------------------------------------------------------------------------
// (Rate is sec's) Items respwan
//---------------------------------------------------------------------------------
$ItemRespawnTime = 30;

//---------------------------------------------------------------------------------
//Amount of Energy remote stations start out with
//---------------------------------------------------------------------------------
$RemoteAmmoEnergy = 2500;
$RemoteInvEnergy = 3000;

//---------------------------------------------------------------------------------
// TEAM ENERGY -  Warn team when teammate has spent x amount - Warn team that
//				  energy level is low when it reaches x amount
//---------------------------------------------------------------------------------
$TeammateSpending = -4000;  //Set = to 0 if don't want the warning message
$WarnEnergyLow = 4000;	    //Set = to 0 if don't want the warning message

//---------------------------------------------------------------------------------
// Amount added to TeamEnergy when a player joins a team
//---------------------------------------------------------------------------------
$InitialPlayerEnergy = 5000;

//---------------------------------------------------------------------------------
// REMOTE TURRET
//---------------------------------------------------------------------------------
$MaxNumTurretsInBox = 2;     //Number of remote turrets allowed in the area
$TurretBoxMaxLength = 50;    //Define Max Length of the area
$TurretBoxMaxWidth =  50;    //Define Max Width of the area
$TurretBoxMaxHeight = 25;    //Define Max Height of the area

$TurretBoxMinLength = 10;	  //Define Min Length from another turret
$TurretBoxMinWidth =  10;	  //Define Min Width from another turret
$TurretBoxMinHeight = 10;    //Define Min Height from another turret

//---------------------------------------------------------------------------------
//	Object Types
//---------------------------------------------------------------------------------
$SimTerrainObjectType    = 1 << 1;
$SimInteriorObjectType   = 1 << 2;
$SimPlayerObjectType     = 1 << 7;

$MineObjectType		    = 1 << 26;
$MoveableObjectType	    = 1 << 22;
$VehicleObjectType	 	 = 1 << 29;
$StaticObjectType			 = 1 << 23;
$ItemObjectType			 = 1 << 21;

//---------------------------------------------------------------------------------
// CHEATS
//---------------------------------------------------------------------------------
$ServerCheats = 0;
$TestCheats = 0;

//---------------------------------------------------------------------------------
//Respawn automatically after X sec's -  If 0..no respawn
//---------------------------------------------------------------------------------
$AutoRespawn = 0;

//---------------------------------------------------------------------------------
// Player death messages - %1 = killer's name, %2 = victim's name
//       %3 = killer's gender pronoun (his/her), %4 = victim's gender pronoun
//---------------------------------------------------------------------------------
$deathMsg[$LandingDamageType, 0]      = "%2 falls to %3 death.";
$deathMsg[$LandingDamageType, 1]      = "%2 forgot to tie %3 bungie cord.";
$deathMsg[$LandingDamageType, 2]      = "%2 bites the dust in a forceful manner.";
$deathMsg[$LandingDamageType, 3]      = "%2 fall down go boom.";
$deathMsg[$ImpactDamageType, 0]      = "%1 makes quite an impact on %2.";
$deathMsg[$ImpactDamageType, 1]      = "%2 becomes the victim of a fly-by from %1.";
$deathMsg[$ImpactDamageType, 2]      = "%2 leaves a nasty dent in %1's fender.";
$deathMsg[$ImpactDamageType, 3]      = "%1 says, 'Hey %2, you scratched my paint job!'";
$deathMsg[$BulletDamageType, 0]      = "%1 ventilates %2 with %3 chaingun.";
$deathMsg[$BulletDamageType, 1]      = "%1 gives %2 an overdose of lead.";
$deathMsg[$BulletDamageType, 2]      = "%1 fills %2 full of holes.";
$deathMsg[$BulletDamageType, 3]      = "%1 guns down %2.";
$deathMsg[$EnergyDamageType, 0]      = "%2 dies of turret trauma.";
$deathMsg[$EnergyDamageType, 1]      = "%2 is chewed to pieces by a turret.";
$deathMsg[$EnergyDamageType, 2]      = "%2 walks into a stream of turret fire.";
$deathMsg[$EnergyDamageType, 3]      = "%2 ends up on the wrong side of a turret.";
$deathMsg[$PlasmaDamageType, 0]      = "%2 feels the warm glow of %1's plasma.";
$deathMsg[$PlasmaDamageType, 1]      = "%1 gives %2 a white-hot plasma injection.";
$deathMsg[$PlasmaDamageType, 2]      = "%1 asks %2, 'Got plasma?'";
$deathMsg[$PlasmaDamageType, 3]      = "%1 gives %2 a plasma transfusion.";
$deathMsg[$ExplosionDamageType, 0]   = "%2 catches a Frisbee of Death thrown by %1.";
$deathMsg[$ExplosionDamageType, 1]   = "%1 blasts %2 with a well-placed disc.";
$deathMsg[$ExplosionDamageType, 2]   = "%1's spinfusor caught %2 by surprise.";
$deathMsg[$ExplosionDamageType, 3]   = "%2 falls victim to %1's Stormhammer.";
$deathMsg[$ShrapnelDamageType, 0]    = "%1 blows %2 up real good.";
$deathMsg[$ShrapnelDamageType, 1]    = "%2 gets a taste of %1's explosive temper.";
$deathMsg[$ShrapnelDamageType, 2]    = "%1 gives %2 a fatal concussion.";
$deathMsg[$ShrapnelDamageType, 3]    = "%2 never saw it coming from %1.";
$deathMsg[$LaserDamageType, 0]       = "%1 adds %2 to %3 list of sniper victims.";
$deathMsg[$LaserDamageType, 1]       = "%1 fells %2 with a sniper shot.";
$deathMsg[$LaserDamageType, 2]       = "%2 becomes a victim of %1's laser rifle.";
$deathMsg[$LaserDamageType, 3]       = "%2 stayed in %1's crosshairs for too long.";
$deathMsg[$MortarDamageType, 0]      = "%1 mortars %2 into oblivion.";
$deathMsg[$MortarDamageType, 1]      = "%2 didn't see that last mortar from %1.";
$deathMsg[$MortarDamageType, 2]      = "%1 inflicts a mortal mortar wound on %2.";
$deathMsg[$MortarDamageType, 3]      = "%1's mortar takes out %2.";
$deathMsg[$BlasterDamageType, 0]     = "%2 gets a blast out of %1.";
$deathMsg[$BlasterDamageType, 1]     = "%2 succumbs to %1's rain of blaster fire.";
$deathMsg[$BlasterDamageType, 2]     = "%1's puny blaster shows %2 a new world of pain.";
$deathMsg[$BlasterDamageType, 3]     = "%2 meets %1's master blaster.";
$deathMsg[$ElectricityDamageType, 0] = "%2 gets zapped with %1's ELF gun.";
$deathMsg[$ElectricityDamageType, 1] = "%1 gives %2 a nasty jolt.";
$deathMsg[$ElectricityDamageType, 2] = "%2 gets a real shock out of meeting %1.";
$deathMsg[$ElectricityDamageType, 3] = "%1 short-circuits %2's systems.";
$deathMsg[$CrushDamageType, 0]		 = "%2 didn't stay away from the moving parts.";
$deathMsg[$CrushDamageType, 1]		 = "%2 is crushed.";
$deathMsg[$CrushDamageType, 2]		 = "%2 gets smushed flat.";
$deathMsg[$CrushDamageType, 3]		 = "%2 gets caught in the machinery.";
$deathMsg[$DebrisDamageType, 0]		 = "%2 is a victim among the wreckage.";
$deathMsg[$DebrisDamageType, 1]		 = "%2 is killed by debris.";
$deathMsg[$DebrisDamageType, 2]		 = "%2 becomes a victim of collateral damage.";
$deathMsg[$DebrisDamageType, 3]		 = "%2 got too close to the exploding stuff.";
$deathMsg[$MissileDamageType, 0]	    = "%2 takes a missile up the keister.";
$deathMsg[$MissileDamageType, 1]	    = "%2 gets shot down.";
$deathMsg[$MissileDamageType, 2]	    = "%2 gets real friendly with a rocket.";
$deathMsg[$MissileDamageType, 3]	    = "%2 feels the burn from a warhead.";
$deathMsg[$MineDamageType, 0]	       = "%1 blows %2 up real good.";
$deathMsg[$MineDamageType, 1]	       = "%2 gets a taste of %1's explosive temper.";
$deathMsg[$MineDamageType, 2]	       = "%1 gives %2 a fatal concussion.";
$deathMsg[$MineDamageType, 3]	       = "%2 never saw it coming from %1.";

// "you just killed yourself" messages
//   %1 = player name,  %2 = player gender pronoun

$deathMsg[-2,0]						 = "%1 ends it all.";
$deathMsg[-2,1]						 = "%1 takes %2 own life.";
$deathMsg[-2,2]						 = "%1 kills %2 own dumb self.";
$deathMsg[-2,3]						 = "%1 decides to see what the afterlife is like.";

$numDeathMsgs = 4;
//---------------------------------------------------------------------------------

$spawnBuyList[0] = LightArmor;
$spawnBuyList[1] = Blaster;
$spawnBuyList[2] = Chaingun;
$spawnBuyList[3] = Disclauncher;
$spawnBuyList[4] = RepairKit;
$spawnBuyList[5] = "";

function remotePlayMode(%clientId)
{
  if(!%clientId.guiLock)
  {
    remoteSCOM(%clientId, -1);
    Client::setGuiMode(%clientId, $GuiModePlay);
  }
}

function remoteCommandMode(%clientId)
{
  // can't switch to command mode while a server menu is up
  if(!%clientId.guiLock)
  {
    remoteSCOM(%clientId, -1);  // force the bandwidth to be full command
	 if(%clientId.observerMode != "pregame")
		checkControlUnmount(%clientId);
	  Client::setGuiMode(%clientId, $GuiModeCommand);
  }
}

function remoteInventoryMode(%clientId)
{
   if(!%clientId.guiLock && !Observer::isObserver(%clientId))
   {
      remoteSCOM(%clientId, -1);
      Client::setGuiMode(%clientId, $GuiModeInventory);
   }
}

function remoteObjectivesMode(%clientId)
{
  if(!%clientId.guiLock)
  {
    remoteSCOM(%clientId, -1);
    Client::setGuiMode(%clientId, $GuiModeObjectives);
  }
}

function remoteScoresOn(%clientId)
{
  if(!%clientId.menuMode)
    Game::menuRequest(%clientId);
}

function remoteScoresOff(%clientId)
{
  Client::cancelMenu(%clientId);
}

function remoteToggleCommandMode(%clientId)
{
	if (Client::getGuiMode(%clientId) != $GuiModeCommand)
		remoteCommandMode(%clientId);
	else
		remotePlayMode(%clientId);
}

function remoteToggleInventoryMode(%clientId)
{
	if (Client::getGuiMode(%clientId) != $GuiModeInventory)
		remoteInventoryMode(%clientId);
	else
		remotePlayMode(%clientId);
}

function remoteToggleObjectivesMode(%clientId)
{
	if (Client::getGuiMode(%clientId) != $GuiModeObjectives)
		remoteObjectivesMode(%clientId);
	else
		remotePlayMode(%clientId);
}

function Time::getMinutes(%simTime)
{
   return floor(%simTime / 60);
}

function Time::getSeconds(%simTime)
{
   return %simTime % 60;
}

function Game::pickRandomSpawn(%team)
{
   %group = nameToID("MissionGroup/Teams/team" @ %team @ "/DropPoints/Random");
   %count = Group::objectCount(%group);
   if(!%count)
      return -1;
  	%spawnIdx = floor(getRandom() * (%count - 0.1));
  	%value = %count;
	for(%i = %spawnIdx; %i < %value; %i++) {
		%set = newObject("set",SimSet);
		%obj = Group::getObject(%group, %i);
		if(containerBoxFillSet(%set,$SimPlayerObjectType|$VehicleObjectType,GameBase::getPosition(%obj),2,2,4,0) == 0) {
			deleteObject(%set);
			return %obj;
		}
		if(%i == %count - 1) {
			%i = -1;
			%value = %spawnIdx;
		}
		deleteObject(%set);
	}
   return false;
}

function Game::pickStartSpawn(%team)
{
   %group = nameToID("MissionGroup\\Teams\\team" @ %team @ "\\DropPoints\\Start");
   %count = Group::objectCount(%group);
   if(!%count)
      return -1;

   %spawnIdx = $lastTeamSpawn[%team] + 1;
   if(%spawnIdx >= %count)
      %spawnIdx = 0;
   $lastTeamSpawn[%team] = %spawnIdx;
   return Group::getObject(%group, %spawnIdx);
}

function Game::pickTeamSpawn(%team, %respawn)
{
   if(%respawn)
      return Game::pickRandomSpawn(%team);
   else
   {
      %spawn = Game::pickStartSpawn(%team);
      if(%spawn == -1)
         return Game::pickRandomSpawn(%team);
      return %spawn;
   }
}

function Game::pickObserverSpawn(%client)
{
   %group = nameToID("MissionGroup\\ObserverDropPoints");
   %count = Group::objectCount(%group);
   if(%group == -1 || !%count)
      %group = nameToID("MissionGroup\\Teams\\team" @ Client::getTeam(%client) @ "\\DropPoints\\Random");
   %count = Group::objectCount(%group);
   if(%group == -1 || !%count)
      %group = nameToID("MissionGroup\\Teams\\team0\\DropPoints\\Random");
   %count = Group::objectCount(%group);
   if(%group == -1 || !%count)
      return -1;
   %spawnIdx = %client.lastObserverSpawn + 1;
   if(%spawnIdx >= %count)
      %spawnIdx = 0;
   %client.lastObserverSpawn = %spawnIdx;
	return Group::getObject(%group, %spawnIdx);
}

function UpdateClientTimes(%time)
{
  for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
    remoteEval(%cl, "setTime", -%time);
}

function Game::notifyMatchStart(%time)
{
   messageAll(1, "Match starts in " @ %time @ " seconds.~wmine_act.wav");
   UpdateClientTimes(%time);
}

function Game::startMatch()
{
	//anti-hm2 code, useless now that we have anticheat
	//hm2::init();

	$matchStarted = true;
	$missionStartTime = getSimTime();
	if($Server::Half == 1) {
	    messageAll(1, "First half has started. Good luck! ~wmine_act.wav");
	}
	else {
	    messageAll(1, "Match started.~wmine_act.wav");
	}

	Game::resetScores();

	%numTeams = getNumTeams();
	for(%i = 0; %i < %numTeams; %i = %i + 1)
	{
		if($TeamEnergy[%i] != "Infinite")
			schedule("replenishTeamEnergy(" @ %i @ ");", $secTeamEnergy);

		$Stats::FlagLoc[%i] = "home";
	}

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.observerMode == "pregame")
		{
			%cl.observerMode = "";
			Client::setControlObject(%cl, Client::getOwnedObject(%cl));
		}

		%cl.lastActiveTimestamp = getSimTime();

		Game::refreshClientScore(%cl);
	}
	Game::checkTimeLimit();

	//send score updatez for team 0/1
	zadmin::ActiveMessage::All(TeamScore, 0, 0);
	zadmin::ActiveMessage::All(TeamScore, 1, 0);

	zadmin::ActiveMessage::All(MatchStarted);

	zadmin::AFKDaemon();
	//schedule("LastHope::PeriodicCheck();", 10);
}

// Kinda like startMatch, but without resetting scores.
function Game::startHalf()
{
  //anti-hm2 code, useless now that we have anticheat
  //hm2::init();

  $matchStarted = true;
  $missionStartTime = getSimTime();
  messageAll(1, "Second half has started!.~wmine_act.wav");

  %numTeams = getNumTeams();
  for(%i = 0; %i < %numTeams; %i = %i + 1)
  {
    if($TeamEnergy[%i] != "Infinite")
      schedule("replenishTeamEnergy(" @ %i @ ");", $secTeamEnergy);

    $Stats::FlagLoc[%i] = "home";
  }

  for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
  {
    if(%cl.observerMode == "pregame")
    {
      %cl.observerMode = "";
      Client::setControlObject(%cl, Client::getOwnedObject(%cl));
    }

    %cl.lastActiveTimestamp = getSimTime();

    Game::refreshClientScore(%cl);
  }
  Game::checkTimeLimit();

  // zadmin::ActiveMessage::All(MatchStarted);
  Game::UpdateClientScores();

  zadmin::AFKDaemon();
  //schedule("LastHope::PeriodicCheck();", 10);
}

function Game::pickPlayerSpawn(%clientId, %respawn)
{
   return Game::pickTeamSpawn(Client::getTeam(%clientId), %respawn);
}

function Game::playerSpawn(%clientId, %respawn)
{
  if(!$ghosting)
    return false;

	Client::clearItemShopping(%clientId);
	%clientId.observerMode = "";
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
	  echo("SPAWN: \"" @ Client::getName(%clientID) @ "\": cl:" @ %clientId @ " pl:" @ %pl @ " marker:" @ %spawnMarker @ " armor:" @ %armor);
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


function Game::playerSpawned(%pl, %clientId, %armor)
{
	%clientId.spawn = 1;
	%max = getNumItems();

	for(%i = 0; (%item = $spawnBuyList[%i]) != ""; %i++)
	{
		buyItem(%clientId,%item);
		if(%item.className == Weapon)
			%clientId.spawnWeapon = %item;
	}

	%clientId.spawn = "";

	if(%clientId.spawnWeapon != "")
	{
		Player::useItem(%pl,%clientId.spawnWeapon);
		%clientId.spawnWeapon = "";
	}

}

function Game::autoRespawn(%client)
{
	if(%client.dead == 1)
		Game::playerSpawn(%client, "true");
}

function onServerGhostAlwaysDone()
{
}

    function Game::initialMissionDrop(%clientId) {

        Client::setGuiMode(%clientId, $GuiModePlay);

        if($Server::TourneyMode) {
            GameBase::setTeam(%clientId, -1);
        }
        else {
            if(%clientId.observerMode == "observerFly" || %clientId.observerMode == "observerOrbit" || %clientId.observerMode == "observerFirst") {
                %clientId.observerMode = "observerOrbit";
                %clientId.guiLock = "";
                Observer::jump(%clientId);
                return;
            }

            %numTeams = getNumTeams();
            %curTeam = Client::getTeam(%clientId);

            if(%curTeam >= %numTeams || (%curTeam == -1 && (%numTeams < 2 || $Server::AutoAssignTeams)) ) {
                Game::assignClientTeam(%clientId);
            }
        }

        Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
        %camSpawn = Game::pickObserverSpawn(%clientId);
        Observer::setFlyMode(%clientId, GameBase::getPosition(%camSpawn),
        GameBase::getRotation(%camSpawn), true, true);

        if(Client::getTeam(%clientId) == -1) {
            %clientId.observerMode = "pickingTeam";
            if(($Server::TourneyMode) && ($matchStarted || $matchStarting)) {
                %clientId.observerMode = "observerFly";
                return;
            }
            else if($Server::TourneyMode) {

                if($Server::TeamDamageScale) {
                    %td = "ENABLED";
                }
                else {
                    %td = "DISABLED";
                }

                if ($Server::BalancedMode) {
                    %mode = "Balanced ";
                    bottomprint(%clientId, "<jc><f1>Server is running in " @ %mode @ "Competition Mode\nPick a team.\nTeam damage is " @ %td, 0);
                }
            }

            Client::buildMenu(%clientId, "Pick a team:", "InitialPickTeam");
                Client::addMenuItem(%clientId, "0Observe", -2);
                Client::addMenuItem(%clientId, "1Automatic", -1);
                for(%i = 0; %i < getNumTeams(); %i = %i + 1)
                    Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
                    %clientId.justConnected = "";
        }
        else {

            Client::setSkin(%clientId, $Server::teamSkin[Client::getTeam(%clientId)]);

            if(%clientId.justConnected) {
                centerprint(%clientId, $Server::JoinMOTD, 0);
                %clientId.observerMode = "justJoined";
                %clientId.justConnected = "";
            }
            else if(%clientId.observerMode == "justJoined") {
                centerprint(%clientId, "");
                %clientId.observerMode = "";
                Game::playerSpawn(%clientId, false);
            }
            else {
                Game::playerSpawn(%clientId, false);
            }
        }

        if($TeamEnergy[Client::getTeam(%clientId)] != "Infinite") {
            $TeamEnergy[Client::getTeam(%clientId)] += $InitialPlayerEnergy;
            %clientId.teamEnergy = 0;
        }
    }

function processMenuInitialPickTeam(%clientId, %team)
{
  if($Server::TourneyMode && $matchStarted)
    %team = -2;

  if(%team == -2)
  {
    Observer::enterObserverMode(%clientId);
  }
  if(%team == -1)
  {
    Game::assignClientTeam(%clientId);
    %team = Client::getTeam(%clientId);
  }
  if(%team != -2)
  {
    GameBase::setTeam(%clientId, %team);
  	if($TeamEnergy[%team] != "Infinite")
			$TeamEnergy[%team] += $InitialPlayerEnergy;
    %clientId.teamEnergy = 0;
    Client::setControlObject(%clientId, -1);
    Game::playerSpawn(%clientId, false);
  }
  if($Server::TourneyMode && !$CountdownStarted)
  {
    if(%team != -2)
    {
      Game::DisplayReadyMessage(%clientId);
      // bottomprint(%clientId, "<f1><jc>Press FIRE when ready.", 0);
      %clientId.notready = true;
      %clientId.notreadyCount = "";
    }
    else
    {
      bottomprint(%clientId, "", 0);
      %clientId.notready = "";
      %clientId.notreadyCount = "";
    }
  }
}

function Game::ForceTourneyMatchStart()
{
  %playerCount = 0;
  for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
  {
    if(%cl.observerMode == "pregame")
      %playerCount++;
  }
  if(%playerCount == 0)
    return;

  for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
  {
    if(%cl.observerMode == "pickingTeam")
      processMenuInitialPickTeam(%cl, -2); // throw these guys into observer
    for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
    {
      %cl.notready = "";
      %cl.notreadyCount = "";
      bottomprint(%cl, "", 0);
    }
  }
  Server::Countdown(30);
}

function Game::CheckTourneyMatchStart()
{
   if($CountdownStarted || $matchStarted)
      return;

   // loop through all the clients and see if any are still notready
   %playerCount = 0;
   %notReadyCount = 0;

   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
   {
      if(%cl.observerMode == "pickingTeam")
      {
         %notReady[%notReadyCount] = %cl;
         %notReadyCount++;
      }
      else if(%cl.observerMode == "pregame")
      {
         if(%cl.notready)
         {
            %notReady[%notReadyCount] = %cl;
            %notReadyCount++;
         }
         else
            %playerCount++;
      }
   }
   if(%notReadyCount)
   {
      if(%notReadyCount == 1)
         MessageAll(0, Client::getName(%notReady[0]) @ " is holding things up!");
      else if(%notReadyCount < 4)
      {
         for(%i = 0; %i < %notReadyCount - 2; %i++)
            %str = Client::getName(%notReady[%i]) @ ", " @ %str;

         %str = %str @ Client::getName(%notReady[%i]) @ " and " @ Client::getName(%notReady[%i+1])
                     @ " are holding things up!";
         MessageAll(0, %str);
      }
      return;
   }

   if(%playerCount != 0)
   {
      for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
      {
         %cl.notready = "";
         %cl.notreadyCount = "";
         bottomprint(%cl, "", 0);
      }
      Server::Countdown(30);
   }
}

function Game::resetScores(%client)
{
	if(%client == "") {
	  for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
    {
	    %cl.scoreKills = 0;
   	  %cl.scoreDeaths = 0;
			%cl.ratio = 0;
      %cl.score = 0;
		}
	}
	else
  {
    %client.scoreKills = 0;
  	%client.scoreDeaths = 0;
		%client.ratio = 0;
    %client.score = 0;
	}
}

function remoteSetArmor(%player, %armorType)
{
	if ($ServerCheats) {
		checkMax(Player::getClient(%player),%armorType);
	  Player::setArmor(%player, %armorType);
	}
	else if($TestCheats) {
	  Player::setArmor(%player, %armorType);
	}
}


function Game::onPlayerConnected(%playerId)
{
   //afk monitor
   %playerId.lastActiveTimestamp = getSimTime();

   %playerId.scoreKills = 0;
   %playerId.scoreDeaths = 0;
	%playerId.score = 0;
   %playerId.justConnected = true;
   $menuMode[%playerId] = "None";
   Game::refreshClientScore(%playerId);
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
			if  ( (%numTeamPlayers[%i] < %leastPlayers) ||
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
}

function Client::onKilled(%playerId, %killerId, %damageType)
{
	echo("GAME: kill "@%killerId@" "@%playerId@" " @ %damageType);

	%playerId.guiLock = true;

	Client::setGuiMode(%playerId, $GuiModePlay);
	if(!String::ICompare(Client::getGender(%playerId), "Male"))
	{
		%playerGender = "his";
	}
	else
	{
		%playerGender = "her";
	}

	%ridx = floor(getRandom() * ($numDeathMsgs - 0.01));
	%victimName = Client::getName(%playerId);

	if (!%killerId)
	{
		//turret
		%damageType = $EnergyDamageType;

		messageAll(0, strcat(%victimName, " dies."), $DeathMessageMask);
	}
	else if (%killerId == %playerId)
	{
		//suicide
		%damageType = $SuicideDamageType;

		%oopsMsg = sprintf($deathMsg[-2, %ridx], %victimName, %playerGender);
		messageAll(0, %oopsMsg, $DeathMessageMask);

		//score
		Client::adjustScore(%killerId, "Suicide");
		%playerId.scoreDeaths++;
		%playerId.Deaths++;
	}
	else
	{
		if(!String::ICompare(Client::getGender(%killerId), "Male"))
			%killerGender = "his";
		else
			%killerGender = "her";

		if($teamplay && (Client::getTeam(%killerId) == Client::getTeam(%playerId)))
		{
			if(%damageType != $MineDamageType)
			{
				messageAll(0, strcat(Client::getName(%killerId),
							" mows down ", %killerGender, " teammate, ", %victimName), $DeathMessageMask);
				%damageType = $TeamkillDamageType;

				Client::adjustScore(%killerId, "TeamKill");
			}
			else
			{
				messageAll(0, strcat(Client::getName(%killerId),
							" killed ", %killerGender, " teammate, ", %victimName ," with a mine."), $DeathMessageMask);
				%damageType = $MineTeamkillDamageType;

				Client::adjustScore(%killerId, "MineTeamKill");
			}

			Game::refreshClientScore(%killerId);
		}
		else
		{
			//%killerid.kills[ $zadmin::WeaponName[%damageType] ]++;
			//%playerid.deaths[ $zadmin::WeaponName[%damageType] ]++;

			%killerid.kills++;
			%playerid.deaths++;
			%killerId.scoreKills++;
			%playerId.scoreDeaths++;  // test play mode

			%obitMsg = sprintf($deathMsg[%damageType, %ridx], Client::getName(%killerId), %victimName, %killerGender, %playerGender);
			messageAll(0, %obitMsg, $DeathMessageMask);

			if (Client::getName(%killerId) != "")
			{
				//%killerId.score++;
				Client::adjustScoreNoUpdate(%killerId, $zadmin::WeaponName[%damageType]);

				%time = getIntegerTime(true) >> 5;
				%oppositeTeam = Client::GetTeam(%playerId) ^ 1;

				if ( (%time == $Stats::FlagDropped[%oppositeTeam]) && ($Stats::PlayerDropped[%oppositeTeam] == %playerId) ) {
          Client::adjustScoreNoUpdate(%killerId, "CarrierKill");
          //Announcer::checkCarrierKills(%killerId);
          //zadmin::ActiveMessage::All( ScoreCarrierKill, Client::getName(%killerId), %killerId.activity["CarrierKill"]);
        }

				if ($zadmin::WeaponName[%damageType] == "Mortar")
					Client::adjustScoreNoUpdate(%playerId, "MortarDeath");
				else
					Client::adjustScoreNoUpdate(%playerId, "OtherDeath");
			}

			Client::refreshScore(%playerId);
			Client::refreshScore(%killerId);
		}
	}

	Game::clientKilled(%playerId, %killerId);

	//active messaging
	//if (Client::getName(%killerId) != "")
	zadmin::ActiveMessage::All(KillTrak, %killerId, %playerId, $zadmin::WeaponName[%damageType]);

	%killerTeam = Client::GetTeam(%killerId);
	%victimTeam = Client::GetTeam(%playerId);

	%now = getSimTime();
	%killerId.lastActiveTimestamp = %now;
	%playerId.lastActiveTimestamp = %now;

}

function Game::clientKilled(%playerId, %killerId)
{
   // do nothing
}

function Client::leaveGame(%clientId)
{
   // do nothing
}

function Player::enterMissionArea(%player)
{
   echo("Player " @ %player @ " entered the mission area.");
}

function Player::leaveMissionArea(%player)
{
   echo("Player " @ %player @ " left the mission area.");
}

function GameBase::getHeatFactor(%this)
{
   return 0.0;
}

function Game::NextHalf()
{
  $Server::Halftime = false;
  if ($Server::TourneyMode)
  {
    $matchStarted = false;
    $CountdownStarted = false;

    Game::SwapScores();
    Game::SwapTeams();

    for (%i = 0; %i < getNumTeams(); %i++)
      Flag::ResetFlag($teamFlag[%i]);

    // Just to change the time on client HUDs
    UpdateClientTimes($Server::TimeLimit * 60);

    MessageAll(0, "~wcapturedtower.wav");

    Game::DisplayHalfScoreboard();
  }
}

function Game::ResetHalf()
{
  if ($Server::TourneyMode && $Server::BalancedMode)
  {
    $Server::Half = 1;
    $Server::Halftime = false;
  }
}

function Game::DisplayReadyMessage(%client)
{
  if ($Server::BalancedMode)
  {
    %scoreLimit = $teamScoreLimit * $Server::Half - $Server::Half;
    %scoreboard = "<jc>" @ "<f1>The game is set to <f2>BALANCED MODE\n" @
                  "<f1>Teams will switch sides at <f2>" @ %scoreLimit @ " <f1>total caps\n" @
                  "<f1>First team to <f2>" @ $teamScoreLimit @ " <f1>wins\n\n";
    %scoreboard = %scoreboard @ "<f1>Press FIRE when ready.";
    CenterPrint(%client, %scoreboard, 0);
  }
  else
  {
    bottomprint(%client, "<f1><jc>Press FIRE when ready.", 0);
    // CenterPrint(%client, "<f1><jc>Press FIRE when ready.", 0);
  }
}

function Game::DisplayHalfScoreboard()
{
  %scoreLimit = $teamScoreLimit * $Server::Half - $Server::Half;
  %scoreboard = "<jc>" @ "<f2>Scores at halftime:\n" @
                "<f1>" @ getTeamName(0) @ ": <f2>" @ $teamScore[0] @ "\n" @
                "<f1>" @ getTeamName(1) @ ": <f2>" @ $teamScore[1] @ "\n\n" @
                "<f1> First team to <f2>" @ $teamScoreLimit @ " <f1>wins. Match ends at <f2>" @ %scoreLimit @ " <f1>total caps.\n\n";
  if ($Server::Half == 2)
    %scoreboard = %scoreboard @ "<f1>Press FIRE when ready.";
  for (%i = 0; %i < getNumClients(); %i++)
  {
    CenterPrint(getClientByIndex(%i), %scoreboard, 0);
  }
}

function Game::SwapScores()
{
  %tempScore = $teamScore[0];
  $teamScore[0] = $teamScore[1];
  $teamScore[1] = %tempScore;

  Game::UpdateClientScores();
}

function Game::UpdateClientScores()
{
  for (%i = 0; %i < getNumClients(); %i++)
  {
    %cl = getClientByIndex(%i);
    zadmin::ActiveMessage::Single(%cl, TeamScore, 0, $TeamScore[0]);
    zadmin::ActiveMessage::Single(%cl, TeamScore, 1, $TeamScore[1]);
  }
}

function Game::SwapTeams()
{
  %numClients = getNumClients();
  for(%i = 0; %i < %numClients; %i++)
  {
    %clientId = getClientByIndex(%i);
    Game::SwapPlayer(%clientId);
  }
}

function Game::SwapPlayer(%clientId)
{
  %clTeam = Client::GetTeam(%clientId);
  if (%clTeam == -1)  // Observer
    return;
  else if (%clTeam == 0)
    %clNewTeam = 1;
  else if (%clTeam == 1)
    %clNewTeam = 0;
  else
    return;

  %player = Client::getOwnedObject(%clientId);

  if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
  {
    playNextAnim(%clientId);
    Player::kill(%clientId);
  }
  %clientId.observerMode = "";

  messageAll(0, Client::getName(%clientId) @ " was switched to " @ getTeamName(%clNewTeam));

  GameBase::setTeam(%clientId, %clNewTeam);
  %clientId.teamEnergy = 0;
  Client::clearItemShopping(%clientId);
  if(Client::getGuiMode(%clientId) != 1)
    Client::setGuiMode(%clientId,1);
  Client::setControlObject(%clientId, -1);

  Game::playerSpawn(%clientId, false);
  %team = Client::getTeam(%clientId);
  if($TeamEnergy[%team] != "Infinite")
    $TeamEnergy[%team] += $InitialPlayerEnergy;
  if($Server::TourneyMode && !$CountdownStarted)
  {
    Game::DisplayReadyMessage(%clientId);
    // bottomprint(%clientId, "<f1><jc>Press FIRE when ready.", 0);
    %clientId.notready = true;
  }
}



Attachment::AddAfter("Game::playerSpawn", "mj::ClientCustomSkin");
function mj::ClientCustomSkin(%clientId, %respawn)
{
			if (%clientId.customSkinDisabled == "")
				%clientId.customSkinDisabled = true;        		
			
			if(!%clientId.customSkinDisabled)
				Client::setSkin(%clientId, $Client::info[%clientId, 0]);
}