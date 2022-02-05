//----------------------------------------------------------------------------

function Elevator::onAdd(%this)
{
	%this.delayTime = getSimTime();
}

function Elevator::onNewPath(%this)
{
	%this.status = "up";
	Moveable::setWaypoint(%this,0);
	if(%this.loop != "")	
		Elevator::onMove(%this);	
}

function Elevator::onEnabled(%this)
{
	GameBase::setActive(%this,true);
	if(!GameBase::isPowered(%this))
		Moveable::moveToWaypoint(%this,0);				
	if(Moveable::getPosition(%this) != 0 || %this.loop != "") {
		%this.delayTime = getSimTime() + 1;
		Elevator::checkDelay(%this);		
	}
}

function Elevator::trigger(%this)
{
	if(%this.loop == "" && Moveable::getPosition(%this) == 0)
		Elevator::checkDelay(%this);		
}

function Elevator::onPower(%this, %state, %generator)
{
	if(%state) {
		if(Moveable::getPosition(%this) != 0 || %this.loop != "") {
			%this.delayTime = getSimTime() + 1;
			Elevator::checkDelay(%this);		
		}
	}
	else  
		Moveable::moveToWaypoint(%this,0);				
}

function Elevator::onMove(%this)
{
	if(GameBase::isPowered(%this) && GameBase::isActive(%this)) {	
		if(%this.status == "up")  
			Moveable::moveToWaypoint(%this,Moveable::getPosition(%this)+1);				
		else if(%this.status == "down")
			Moveable::moveToWaypoint(%this,Moveable::getPosition(%this)-1);				
		%this.triggered = "";
	}
}

function Elevator::onFirst(%this)
{
	%this.status = "up";
	%this.delayTime = getSimTime() + 1.5;
	%this.triggerHit = "";
	if(%this.loop != "")	{
		Elevator::checkDelay(%this);		
	}
}

function Elevator::onWaypoint(%this)
{
	%waypoint = Moveable::getPosition(%this);
	if(%waypoint != 0 && %waypoint != Moveable::getWaypointCount(%this)-1) {  
		if((%this.status == "up" && %this.stopWayUp != "") || (%this.status == "down" && %this.stopWayDown != "")) {
			%this.delayTime = getSimTime() + 1.5;
			Elevator::checkDelay(%this);		
		}
		else
			Elevator::onMove(%this);
	}
}

function Elevator::onLast(%this)
{
	%this.status = "down";
	%this.waypoint = "top";
	%this.delayTime = getSimTime() + 1.5;
	Elevator::checkDelay(%this);		
}

function Elevator::onCollision(%this, %object)
{
	if(!Player::isDead(%object) && getObjectType(%object) == "Player")
		if (GameBase::getTeam(%this) == GameBase::getTeam(%object) || %this.noTeam != "" || GameBase::getTeam(%this) == -1) {
			if (GameBase::isActive(%this) && %this.loop == "" && GameBase::isPowered(%this)  && getSimTime() >= %this.delayTime) {
				if(Moveable::getPosition(%this) == 0 && (%this.triggered == "" || %this.delayTime + 1.5 < getSimTime()) && %this.triggerHit == "") { 
					%this.delayTime = getSimTime() + 2.0;
					%this.triggered = 1;
				}
				if( getSimTime() >= %this.delayTime ) { 
					Elevator::trigger(%this);
					return true;
				}
			}
			else if(GameBase::getDataName(%object) == AntipersonelMine) 
				AntipersonelMine::onCollision(%object,%this);
		}
	return false;
}


function Elevator::onBlocker(%this,%obj)
{
	// Will get called 30/s for blocking objects
	//echo("Moveable::onBlocker: " @ %this @ " " @ %obj);
	GameBase::applyDamage(%obj,$CrushDamageType,0.01,
		GameBase::getPosition(%this),"0 0 0","0 0 0",%this);
}

function Elevator::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
}

function Elevator::onDisabled(%this)
{
	GameBase::setActive(%this,false);
   Moveable::stop(%this);
}

function Elevator::onDestroyed(%this)
{
	GameBase::setActive(%this,false); 		
   Moveable::stop(%this);
}	

function Elevator::checkDelay(%this)
{
	if (getSimTime() >= %this.delayTime) 
		Elevator::onMove(%this);
	else
		schedule("Elevator::checkDelay(" @ %this @ ");",1,%this);
}

function Elevator::onTrigEnter(%this,%object,%trigger)
{
}

function Elevator::onTrigLeave(%this,%object,%trigger)
{

}

function Elevator::onTrigger(%this,%object,%trigger)
{
	if(%this.triggerHit == "" && %this.delayTime < getSimTime()) {
		%this.triggerHit = 1;
		%this.status = "up";
	   if(!Elevator::onCollision(%this,%object))
	  		%this.triggerHit = "";
	}
}

//----------------------------------------------------------------------------

$DoorScale[$ImpactDamageType] = 0;
$DoorScale[$CrushDamageType] = 0;
$DoorScale[$BulletDamageType] = 0;
$DoorScale[$PlasmaDamageType] = 0.25;
$DoorScale[$EnergyDamageType] = 0;
$DoorScale[$ExplosionDamageType] = 0.25;
$DoorScale[$MissileDamageType] = 1.0;
$DoorScale[$DebrisDamageType] = 1.0;
$DoorScale[$ShrapnelDamageType] = 1.0;
$DoorScale[$LaserDamageType] = 0;
$DoorScale[$MortarDamageType] = 1.0;
$DoorScale[$BlasterDamageType] = 0;
$DoorScale[$ElectricityDamageType] = 0;
$DoorScale[$MineDamageType] = 1.0;


$ForceFields["DoorForceField"] = 1;
$ForceFields["DoorForceField3x4"] = 1;
$ForceFields["DoorForceField4x17"] = 1;
$ForceFields["DoorForceField4x8"] = 1;
$ForceFields["DoorForceField5x5"] = 1;
$ForceFields["DoorForceField4x14"] = 1;
function Door::onAdd(%this)
{
	if(!GameBase::isPowered(%this))
		Door::onPower(%this,"False");   

	%this.closeTime = getSimTime();
 	%this.fadeTime	 = getSimTime();
 	%this.faded = "";
}

function Door::onNewPath(%this)
{
	%numPoints = Moveable::getWaypointCount(%this);
	%name = GameBase::getDataName(%this);
	if(%numPoints <= 2 && (%name.side=="left" || %name.side == "right"))
		%name.side = "single";
	if(%name.side == "single") {
		%center = %numPoints-1;
		Moveable::setWaypoint(%this,%center);
	}
	else {
		%center = floor(%numPoints/2);
		Moveable::setWaypoint(%this,%center);
	}
	%this.center = %center;
}

function Door::onEnabled(%this)
{
	GameBase::setActive(%this,true);
	if(%this.center != "" && GameBase::isPowered(%this)) { 
		%this.closeTime = getSimTime() + 1;
		Door::closeCheck(%this);
	}
	else
		Door::onPower(%this,"False");   
}

function Door::trigger(%this)
{
	%waypoint = Moveable::getPosition(%this);
	if((%waypoint != 0 && %waypoint != Moveable::getWaypointCount(%this)-1) || ((GameBase::getDataName(%this)).side == "single" && %waypoint != 0)) {
		%this.status = "open";
		if(!%this.triggerOpen == "")
			%this.playerTrigger = 1; 
		Door::onMove(%this);	
	}
}

function Door::onPower(%this, %state, %generator)
{
	if(%state) { 
		if(Moveable::getPosition(%this) != %this.center && %this.center != "") {
			%this.status = "close";
			%this.closeTime = getSimTime() + 1;
			Door::closeCheck(%this);
		}
	}
	else { 
		%this.status = "open";
		Door::onMove(%this, 1);
	}
}

function Door::onFirst(%this)
{
	if(%this.triggerOpen == "") {
		%this.status = "close";
		%this.closeTime = getSimTime() + 3;
		Door::closeCheck(%this);
	}
	else if(%this.playerTrigger) {
		%this.status = "close";
		%this.closeTime = getSimTime() + 1;
		Door::closeCheck(%this);
		%this.playerTrigger = "";
	}	
}

function Door::onLast(%this)
{
	if((GameBase::getDataName(%this)).side != "single") {
		if(%this.triggerOpen == "") {
			%this.status = "close";
			%this.closeTime = getSimTime() + 3;
			Door::closeCheck(%this);
		}
		else if(%this.playerTrigger) {
			%this.status = "close";					
			%this.closeTime = getSimTime() + 1;
			Door::closeCheck(%this);
			%this.playerTrigger = "";
		}	
	}
}

function Door::onWaypoint(%this)
{
	if(Moveable::getPosition(%this) == %this.center && %this.status == "close") 
		%this.triggerTrigger = "";
}

function Door::onMove(%this, %forceClose)
{
	if($ForceFields[GameBase::getDataName(%this)] != "" && %this.faded == "" && %this.status == "open") {
		GameBase::startFadeOut(%this);
		%time = getSimTime() - %this.fadeTime;
		if(%time > 2.5)
			%time = 2.5;
		%this.faded = 1;
		if(%forceClose != "")
			schedule("Door::onMove(" @ %this @ "," @ %forceClose @ ");",%time,%this);
		else
			schedule("Door::onMove(" @ %this @ ");",%time,%this);
		schedule("GameBase::playSound(" @ %this @ ",ForceFieldOpen,0);",(%time/2),%this);
		return;		
	} 
	if((GameBase::isActive(%this) && GameBase::isPowered(%this)) || %forceClose != "") {
		if(%this.status == "open") {
			if((GameBase::getDataName(%this)).side == "left")
				Moveable::moveToWaypoint(%this,(Moveable::getWaypointCount(%this)-1));				
			else
				Moveable::moveToWaypoint(%this,0);				
	 	}
		else { 
			Moveable::moveToWaypoint(%this,%this.center);				
		}
	}
	if($ForceFields[GameBase::getDataName(%this)] != "" && %this.status == "close" && !%this.faded == "" && GameBase::isPowered(%this)) {
		GameBase::startFadeIn(%this);
	 	%this.faded="";
		%this.fadeTime = getSimTime();
	}
}

function Door::onCollision(%this, %object)
{
	if(!Player::isDead(%object) && getObjectType(%object) == "Player") 
		if (GameBase::isActive(%this) && GameBase::isPowered(%this) && %this.faded == "")  
			if(GameBase::getTeam(%this) == GameBase::getTeam(%object) || GameBase::getTeam(%this) == -1 || %this.noTeam != "" ) 
				if((%this.triggerOpen == "" || %this.triggerTrigger) ) 
					Door::trigger(%this);
}

function Door::onBlocker(%this,%obj)
{
	GameBase::applyDamage(%obj,$CrushDamageType,0.01,
		GameBase::getPosition(%this),"0 0 0","0 0 0",%this);
}

function Door::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	%damageLevel = GameBase::getDamageLevel(%this);
	%TDS= 1;
	if(GameBase::getTeam(%this) == GameBase::getTeam(%object))
		%TDS = $Server::TeamDamageScale;
//	GameBase::setDamageLevel(%this,%damageLevel + %value * 0.01 * %TDS * $DoorScale[%type]);
	GameBase::setDamageLevel(%this,%damageLevel + %value * $DoorScale[%type]);
}

function Door::onDisabled(%this)
{
	GameBase::setActive(%this,false);
   Moveable::stop(%this);
}

function Door::onDestroyed(%this)
{
	GameBase::setActive(%this,false); 		
   Moveable::stop(%this);
}	

function Door::closeCheck(%this)
{
	if (getSimTime() >= %this.closeTime)
		Door::onMove(%this);
	else
		schedule("Door::closeCheck(" @ %this @ ");",1,%this);
}

function Door::onTrigEnter(%this,%object,%trigger)
{
	if(GameBase::getTeam(%this) == GameBase::getTeam(%object) || GameBase::getTeam(%this) == -1 || %this.noTeam != "" ) {
		%this.status = "open";
		Door::onMove(%this);
	}
}

function Door::onTrigLeave(%this,%object,%trigger)
{
	if(GameBase::getTeam(%this) == GameBase::getTeam(%object) || GameBase::getTeam(%this) == -1 || %this.noTeam != "" ) {
		%this.status = "close";
		%this.triggerTrigger = 1;
		Door::onMove(%this);
	}
}

function Door::onTrigger(%this,%object,%trigger)
{

}
