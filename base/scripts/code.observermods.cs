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
			if($mj::enableObsNotification)
				bottomprint(%target, "<f1><jc>" @ Client::GetName(%client) @ " <f2> is observing you", 3);
			zadmin::ActiveMessage::Single(%target, ObservedBy, %client, true);
	}
	//endmj

	%client.observerTarget = %target;
	return true;
}


function Observer::jump(%client)
{
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
		//mj
		if( %client.observerTarget != "")
		{
			if($mj::enableObsNotification)
				bottomprint( %client.observerTarget, "<f1><jc>" @ Client::GetName(%client) @ " <f2>is no longer observing you", 5);						
			zadmin::ActiveMessage::Single(%client.observerTarget, ObservedBy, %client, false);
			//zadmin::ActiveMessage::Single(%client.observerTarget, ObservedBy,%client, false);
		}
		//endmj
		
		%client.observerTarget = "";
		%client.observerMode = "observerFly";

		%camSpawn = Game::pickObserverSpawn(%client);
		Observer::setFlyMode(
			%client, 
			GameBase::getPosition(%camSpawn), 
			GameBase::getRotation(%camSpawn), 
			true, 
			true
		);
	}
}
