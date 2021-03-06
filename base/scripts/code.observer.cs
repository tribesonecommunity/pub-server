 $GuiModeCommand    = 2;
$LastControlObject = 0;

function Observer::test()
{
   echo("VOL");
}

function Observer::triggerDown(%client)
{
}

function Observer::orbitObjectDeleted(%cl)
{
}

function Observer::leaveMissionArea(%cl)
{
}

function Observer::enterMissionArea(%cl)
{
}

function Observer::triggerUp(%client)
{
   if(%client.observerMode == "dead")
   {
      if(%client.dieTime + $Server::respawnTime < getSimTime())
      {
         if(Game::playerSpawn(%client, true))
         {
            %client.observerMode = "";
            Observer::checkObserved(%client);
         }
      }
   }
   else if ((%client.observerMode == "observerOrbit") || (%client.observerMode == "observerFirst"))
      Observer::nextObservable(%client);
   else if(%client.observerMode == "observerFly")
   {
      %camSpawn = Game::pickObserverSpawn(%client);
      Observer::setFlyMode(%client, GameBase::getPosition(%camSpawn),
	      GameBase::getRotation(%camSpawn), true, true);
   }
   else if(%client.observerMode == "justJoined")
   {
      %client.observerMode = "";
      Game::playerSpawn(%client, false);
   }
   else if(%client.observerMode == "pregame" && $Server::TourneyMode)
   {
      if($CountdownStarted)
         return;

      if(%client.notready)
      {
         %client.notready = "";
         MessageAll(0, Client::getName(%client) @ " is READY.");
         if(%client.notreadyCount < 3)
            bottomprint(%client, "<f1><jc>Waiting for match start (FIRE if not ready).", 0);
         else
            bottomprint(%client, "<f1><jc>Waiting for match start.", 0);
      }
      else
      {
         %client.notreadyCount++;
         if(%client.notreadyCount < 4)
         {
            %client.notready = true;
            MessageAll(0, Client::getName(%client) @ " is NOT READY.");
            // bottomprint(%client, "<f1><jc>Press FIRE when ready.", 0);
            Game::DisplayReadyMessage(%client);
         }
         return;
      }
      Game::CheckTourneyMatchStart();
   }
}

function Observer::jump(%client)
{
   //%client.observerTarget = "";

	if(%client.observerMode == "observerFly")
	{
		%client.observerMode = "observerOrbit";
		%client.observerTarget = %client;
		Observer::nextObservable(%client);
	}
	else if(%client.observerMode == "observerOrbit")
	{
		%client.observerMode = "observerFirst";
		//%client.observerTarget = %client;
		//Observer::nextObservable(%client);
		Observer::setTargetClient(%client, %client.observerTarget);
	}
	else if(%client.observerMode == "observerFirst")
	{

      //if (%client.observerTarget != "") {
         //bottomprint(%client.observerTarget, "<jc>Being observed by: <f2>" @ Client::getName(%client), 5);
         //zadmin::ActiveMessage::Single(%client, ObservedBy, %client.observerTarget, false);
      //}

		%client.observerTarget = "";
		%client.observerMode = "observerFly";

		%camSpawn = Game::pickObserverSpawn(%client);
		Observer::setFlyMode(%client, GameBase::getPosition(%camSpawn),
		GameBase::getRotation(%camSpawn), true, true);
	}
}

function Observer::isObserver(%clientId)
{
   return %clientId.observerMode == "observerOrbit" || %clientId.observerMode == "observerFly" || %clientId.observerMode == "observerFirst";
}

function Observer::enterObserverMode(%clientId)
{
   if(%clientId.observerMode == "observerOrbit" || %clientId.observerMode == "observerFly" || %clientId.observerMode == "observerFirst")
      return false;
   Client::clearItemShopping(%clientId);
   %player = Client::getOwnedObject(%clientId);
   if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)) {
		playNextAnim(%clientId);
	   Player::kill(%clientId);
	}
   Client::setOwnedObject(%clientId, -1);
   Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
   %clientId.observerMode = "observerFirst";
   GameBase::setTeam(%clientId, -1);
   Observer::jump(%clientId);
   remotePlayMode(%clientId);
   return true;
}

function Observer::checkObserved(%client)
{
   // this function loops through all the clients and checks
   // if anyone was observing %client... if so, it updates that
   // observation to reflect the new %client owned object.

   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
   {
      if(%cl.observerTarget == %client)
      {
         if(%cl.observerMode == "observerOrbit")
      	   Observer::setOrbitObject(%cl, %client, 5, 5, 5);
         else if(%cl.observerMode == "observerFirst")
      	   Observer::setOrbitObject(%cl, %client, -1, -1, -1);
         else if(%cl.observerMode == "commander")
   		   Observer::setOrbitObject(%cl, %client, -3, -3, -3);
      }
   }
}

function Observer::setTargetClient(%client, %target)
{
	//echo ("Entered setTarget...");
	if ((%client.observerMode != "observerOrbit") && (%client.observerMode != "observerFirst"))
		return false;

	%owned = Client::getOwnedObject(%target);
	if(%owned == -1)
		return false;

	if(%client.observerMode == "observerOrbit")
	{
		Observer::setOrbitObject(%client, %target, 5, 5, 5);
		bottomprint(%client, "<jc>Third Person Observing " @ Client::getName(%target), 5);
	}
	else if(%client.observerMode == "observerFirst")
	{
		Observer::setOrbitObject(%client, %target, -1, -1, -1);
		bottomprint(%client, "<jc>First Person Observing " @ Client::getName(%target), 5);
	}

	//mj
	if(%client.observerTarget != %target)
	{
		//echo ("sending message to client");
		//zadmin::ActiveMessage::Single(%target, ObservedBy, Client::getName(%client));
      //bottomprint(%target, "<jc>Being observed by <f2>" @ Client::getName(%client), 5);
	}
	//endmj

	%client.observerTarget = %target;
	return true;
}

function Observer::nextObservable(%client)
{
   %lastObserved = %client.observerTarget;
   %nextObserved = Client::getNext(%lastObserved);
   %ct = 128;  // just in case
   while(%ct--)
   {
      if(%nextObserved == -1)
      {
         %nextObserved = Client::getFirst();
         continue;
      }
      %owned = Client::getOwnedObject(%nextObserved);
      if(%nextObserved == %lastObserved && %owned == -1)
      {
         Observer::jump(%client);
         return;
      }
      if(%owned == -1)
      {
         %nextObserved = Client::getNext(%nextObserved);
         continue;
      }
      Observer::setTargetClient(%client, %nextObserved);
      return;
   }
   Observer::jump(%client);
}

function Observer::prevObservable(%client)
{
}

function remoteSCOM(%clientId, %observeId)
{
   if (%observeId != -1)
   {
      if (Client::getTeam(%clientId) == Client::getTeam(%observeId) &&
         (%clientId.observerMode == "" || %clientId.observerMode == "commander") && Client::getGuiMode(%clientId) == $GuiModeCommand)
      {
         Client::limitCommandBandwidth(%clientId, true);
         if(%clientId.observerMode != "commander")
         {
            %clientId.observerMode = "commander";
	         %clientId.lastControlObject = Client::getControlObject(%clientId);
         }
	      Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
		   Observer::setOrbitObject(%clientId, %observeId, -3, -3, -3);
         %clientId.observerTarget = %observeId;
         Observer::setDamageObject(%clientId, %clientId);
      }
   }
   else
   {
      Client::limitCommandBandwidth(%clientId, false);
      if(%clientId.observerMode == "commander")
      {
         Client::setControlObject(%clientId, %clientId.lastControlObject);
		   %clientId.lastControlObject = "";
         %clientId.observerMode = "";
         %clientId.observerTarget = "";
	   }
   }
}