//ProjectileData::setDamageValue and ProjectileData:getDamageValue are defined in Serverlib.dll
//ProjectileData::setDamageValue("BlasterBolt",$WeakBlaster::weakDamage); 
//ProjectileData::setDamageValue("ChaingunBullet", $WeakChaingun::weakDamage);


//------------------------- CHAINGUN ---------------------------------

function WeakChaingun::Set(%mode, %clientId)
{
	if(%clientId > 2048)
		%name_issuedby = Client::getName(%clientId);
	else
		%name_issuedby = "The Server";
	$WeakChaingun::enabled = %mode;
	%curdmg = 0.0; //ProjectileData::getDamageValue("ChaingunBullet");
	if (%mode)
	{		
		$ItemMax["larmor", "BulletAmmo"] = $WeakChaingun::ammoCount;
		$ItemMax["lfemale", "BulletAmmo"] = $WeakChaingun::ammoCount;
		for (%i = 0; %i < getNumClients(); %i++)
		{
			%cl = getClientByIndex(%i);
			%ammo = Player::getItemCount(%cl,"BulletAmmo");
			if (%ammo > $WeakChaingun::ammoCount)
				Player::setItemCount(%cl, "BulletAmmo", $WeakChaingun::ammoCount);
		}
		if( $WeakChaingun::weakDamage != %curdmg)
		{
			ProjectileData::setDamageValue("ChaingunBullet", $WeakChaingun::weakDamage);
			messageAll(0, "Weakened Chaingun enabled by " @ %name_issuedby @ " ( Damage set to " @ floor($WeakChaingun::weakDamage * 100.0 /$WeakChaingun::defaultDamage) @ "% ).~wmine_act.wav");
			echo(floor( ($WeakChaingun::weakDamage * 100.0) / $WeakChaingun::defaultDamage) , "%" ) ;

		}
	}
	else
	{				
		$ItemMax["larmor", "BulletAmmo"] = $WeakChaingun::defaultAmmoCount;
		$ItemMax["lfemale", "BulletAmmo"] = $WeakChaingun::defaultAmmoCount;
		for (%i = 0; %i < getNumClients(); %i++)
		{
			%cl = getClientByIndex(%i);
			Player::setItemCount(%cl, "BulletAmmo", $WeakChaingun::defaultAmmoCount);
		}
		if( $WeakChaingun::defaultDamage != %curdmg)
		{
			ProjectileData::setDamageValue("ChaingunBullet", $WeakChaingun::defaultDamage);
			messageAll(0, "Chaingun damage restored to default by " @ %name_issuedby @ " (" @ floor($WeakChaingun::defaultDamage) @ ").~wmine_act.wav");
		}
	}
	echo("WeakChaingun::Set: "@ %mode @ " " @ %clientId @ " " @ $WeakChaingun::weakDamage @ " " @ $WeakChaingun::defaultDamage @ " ( " @ floor( ($WeakChaingun::weakDamage * 100.0) / $WeakChaingun::defaultDamage) , "% )" );
}
schedule("WeakChaingun::Set($WeakChaingun::enabled, 0);", 5);


//------------------------- BLASTER ---------------------------------

function WeakBlaster::Set(%mode, %clientId)
{
	if(%clientId > 2048)
		%name_issuedby = Client::getName(%clientId);
	else
		%name_issuedby = "The Server";
	$WeakBlaster::enabled = %mode;
	%curdmg = 0.0; //ProjectileData::getDamageValue("BlasterBolt");
	if (%mode)
	{
		if( $weakBlaster::weakDamage != %curdmg)
		{
			ProjectileData::setDamageValue("BlasterBolt",$WeakBlaster::weakDamage); 
			messageAll(0, "Weakened Blaster enabled by " @ %name_issuedby @ " ( Damage set to " @ floor($WeakBlaster::weakDamage * 100.0 /$WeakBlaster::defaultDamage) @ "% ).~wmine_act.wav");
		}
	}
	else
	{
		if( $WeakBlaster::defaultDamage != %curdmg)
		{
		
			ProjectileData::setDamageValue("BlasterBolt",$WeakBlaster::defaultDamage); 
			messageAll(0, "Blaster damage restored to default by " @ %name_issuedby @ " (" @ floor($WeakBlaster::defaultDamage) @ ").~wmine_act.wav");
		}
	}
	echo("WeakBlaster::Set: "@ %mode @ " " @ %clientId @ " " @ $WeakBlaster::weakDamage @ " " @ $WeakBlaster::defaultDamage @ " ( " @ floor( ($WeakChaingun::weakDamage * 100.0) / $WeakBlaster::defaultDamage) , "% )" );
}


schedule("WeakBlaster::Set($WeakBlaster::enabled, 0);", 5);
