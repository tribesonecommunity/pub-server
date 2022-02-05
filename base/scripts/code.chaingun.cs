
Attachment::AddAfter("displayMenuVoteMenu", "mj::displayMenuVoteMenu");
Attachment::AddAfter("processMenuVoteMenu", "mj::processMenuVoteMenu");

Attachment::AddAfter("aActionvoteSucceded", "mj::aActionvoteSucceded");
Attachment::AddAfter("displayMenuServerToggles", "mj::displayMenuServerTogglesCG");
Attachment::AddAfter("processMenuServerToggleMenu", "mj::processMenuServerToggleMenuCG");

Attachment::AddBefore("buyItem", "mj::buyItem");
Attachment::AddBefore("remotebuyItem", "mj::buyItem");

$isPUCircleJerk = $Server::Address == "IP:208.100.45.13:28002";


function mj::buyItem(%client,%item)
{
	//echo(%client @ " " @ %item);
	if((!$mj::chainenabled && %item == Chaingun))
	{		
		if($mj::chain::giveblaster)
		{
		 //	buyItem(%client, Blaster);
			buyItem(%client, LaserRifle);		
		}
		return "halt";
	}
}

function mj::enableblastermode()
{
	$mj::chain::giveblaster = true;
	mj::giveallblaster();
	messageAll(0, "Blaster mode is enabled");
}

function mj::disableblastermode()
{
	$mj::chain::giveblaster = false;
	mj::dropallblaster();
	messageAll(0, "Blaster mode is disabled");
}

function mj::enableChain()
{
	if($mj::chainenabled)
		return;
		

    $mj::chainenabled = true;			
	mj::restoreAmmoInfo();	
	mj::giveallchaingun();
	messageAll(0, "Chaingun is enabled");
		
	if($mj::chain::giveblaster)
		mj::disableblastermode();
}

function mj::disableChain()
{
	if(!$mj::chainenabled)
		return;
	
	
	$mj::chainenabled = false;
	mj::saveAmmoInfo();
	mj::updateAmmoInfo(0, 0);		
	mj::dropallchaingun();
	messageAll(0, "Chaingun is disabled");
	
	if($mj::chain::giveblaster)
		mj::enableblastermode();
}


$mj::instagibenabled = false;
 
function mj::instagib::enable()
{
		if($InstaGib::DamageScale[lfemale, $ExplosionDamageType] == "")
			$InstaGib::DamageScale[lfemale, $ExplosionDamageType] = $DamageScale[lfemale, $ExplosionDamageType];
		if($InstaGib::DamageScale[larmor, $ExplosionDamageType] == "")
			$InstaGib::DamageScale[larmor, $ExplosionDamageType] = $DamageScale[larmor, $ExplosionDamageType];

		$DamageScale[lfemale, $ExplosionDamageType] = 100000.0;
		$DamageScale[larmor, $ExplosionDamageType] = 100000.0;
		messageAll(0, "Instagib disc is enabled");
		$mj::instagibenabled = true;
}

function mj::instagib::disable()
{
		if($InstaGib::DamageScale[lfemale, $ExplosionDamageType] == "")
			$InstaGib::DamageScale[lfemale, $ExplosionDamageType] = 1.0;
		if($InstaGib::DamageScale[larmor, $ExplosionDamageType] == "")
			$InstaGib::DamageScale[larmor, $ExplosionDamageType] = 1.0;
	   
	   	$DamageScale[lfemale, $ExplosionDamageType] = $InstaGib::DamageScale[lfemale, $ExplosionDamageType];
		$DamageScale[larmor, $ExplosionDamageType] = $InstaGib::DamageScale[larmor, $ExplosionDamageType];
		messageAll(0, "Instagib disc is disabled");
		$mj::instagibenabled = false;
}

function mj::giveallblaster()
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{		
		if( (%cl.dead != 1 && Client::GetTeam(%cl) != -1) &&  Player::GetItemCount(%cl, Blaster) == 0)
		{
			Player::SetItemCount(%cl, Blaster, 0);
			Player::SetItemCount(%cl, LaserRifle, 1);
			Player::SetItemCount(%cl, Chaingun, 0);
			Player::SetItemCount(%cl, BulletAmmo, 0);
		}
	}	
}


function mj::dropallblaster()
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{		
		if( (%cl.dead != 1 && Client::GetTeam(%cl) != -1) &&  Player::GetItemCount(%cl, Blaster) > 0)
		{
			Player::SetItemCount(%cl, Blaster, 0);
			Player::SetItemCount(%cl, LaserRifle, 0);
		}
	}	
}

function mj::giveallchaingun()
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(Client::GetTeam(%cl) == -1 || %cl.dead == 1)
			continue;
		Player::SetItemCount(%cl, Chaingun, 1);
		%ammocnt = mj::getDefaultAmmo(Player::getArmor(%cl));
		Player::SetItemCount(%cl, BulletAmmo, %ammocnt);
		Player::SetItemCount(%cl, LaserRifle, 0);			
	}	
}

function mj::dropallchaingun()
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(Client::GetTeam(%cl) == -1 || %cl.dead == 1)
			continue;
		Player::SetItemCount(%cl, Chaingun, 0);
		Player::SetItemCount(%cl, BulletAmmo, 0);
	}	
}

function mj::updateAmmoInfo(%haschain, %ammocnt)
{
	$ItemMax[larmor, Chaingun] = %haschain;
	$ItemMax[marmor, Chaingun] = %haschain;
	$ItemMax[harmor, Chaingun] = %haschain;
	$ItemMax[lfemale, Chaingun] = %haschain;
	$ItemMax[mfemale, Chaingun] = %haschain;	
	
	$ItemMax[larmor,  BulletAmmo] = %ammocnt;
	$ItemMax[marmor,  BulletAmmo] = %ammocnt;
	$ItemMax[harmor,  BulletAmmo] = %ammocnt;
	$ItemMax[lfemale, BulletAmmo] = %ammocnt;
	$ItemMax[mfemale, BulletAmmo] = %ammocnt; 		
}	

function mj::restoreAmmoInfo()
{
	$ItemMax[larmor, Chaingun] = 1;
	$ItemMax[marmor, Chaingun] = 1;
	$ItemMax[harmor, Chaingun] = 1;
	$ItemMax[lfemale, Chaingun] = 1;
	$ItemMax[mfemale, Chaingun] = 1;		
	
	if( $ItemMaxOrg[harmor] == "" || $ItemMaxOrg[harmor] == 0 )
	{		
		mj::updateAmmoInfo(%haschain,mj::getDefaultAmmo("lfemale") );
		return;
	}	
	$ItemMax[larmor,  BulletAmmo] =$ItemMaxOrg[larmor];
	$ItemMax[marmor,  BulletAmmo] =$ItemMaxOrg[marmor];
	$ItemMax[harmor,  BulletAmmo] =$ItemMaxOrg[harmor];
	$ItemMax[lfemale, BulletAmmo] =$ItemMaxOrg[lfemale];
	$ItemMax[mfemale, BulletAmmo] =$ItemMaxOrg[mfemale]; 		
}	

function mj::saveAmmoInfo()
{	
	$ItemMaxOrg[larmor] = $ItemMax[larmor,  BulletAmmo];
	$ItemMaxOrg[marmor] = $ItemMax[marmor,  BulletAmmo];
	$ItemMaxOrg[harmor] = $ItemMax[harmor,  BulletAmmo];
	$ItemMaxOrg[lfemale] = $ItemMax[lfemale, BulletAmmo];
	$ItemMaxOrg[mfemale] = $ItemMax[mfemale, BulletAmmo]; 	
	//echo("saveAmmoInfo " @ $ItemMaxOrg[lfemale]  @ "/" @ $ItemMax[lfemale,  BulletAmmo]);
}


function mj::getDefaultAmmo(%armor)
{
	if($WeakChaingun::defaultAmmoCount == "")
	{
		if($ItemMaxOrg[%armor]!= "" && $ItemMaxOrg[%armor] > 0)
			%ammocnt = $ItemMaxOrg[%armor];
		else 
			%ammocnt = 100;
	}
	else if($WeakChaingun::enabled && $WeakChaingun::ammoCount != "")
		%ammocnt = $WeakChaingun::ammoCount ;
	else if($ItemMaxOrg[%armor]!= "" && $ItemMaxOrg[%armor] > 0)
		%ammocnt = $ItemMaxOrg[%armor];
	else 
		%ammocnt = $WeakChaingun::defaultAmmoCount ;	
	return %ammocnt;
}


function mj::displayMenuServerTogglesCG(%cl)
{
    %rec = %cl.selClient;
    %recName = Client::getName(%rec);
	addLine("Disable Chaingun", "CG",  $mj::chainenabled && %cl.canChangeGameMode, %cl);
	addLine("Enable Chaingun", "CG1",  !$mj::chainenabled && %cl.canChangeGameMode, %cl);			
	addLine("Disable Blaster mode", "CGB",  !$mj::chainenabled && $mj::chain::giveblaster &&  %cl.canChangeGameMode, %cl);
	addLine("Enable Blaster mode", "CG1B",  !$mj::chainenabled && !$mj::chain::giveblaster &&  %cl.canChangeGameMode, %cl);	
	addLine("Enable Sniper mode", "CG1C",  !$mj::chainenabled && !$mj::chain::giveblaster &&  %cl.canChangeGameMode, %cl);						
	addLine("Add 5 minutes", "add5",  true, %cl);						
}

function mj::processMenuServerToggleMenuCG(%cl, %sel)
{
	if ((%sel == "CG") && %cl.canChangeGameMode){
		 mj::disableChain();
	}
	else if ((%sel == "CG1") && %cl.canChangeGameMode){
		mj::enableChain();
	}	
	else if ((%sel == "CGB") && %cl.canChangeGameMode ){
		mj::disableblastermode();	
	}
	else if ((%sel == "CG1B") && %cl.canChangeGameMode){
		mj::enableblastermode();	 
	}	
	else if ((%sel == "add5") ){
		mj::extendTimeLimit(5);	
	}			
	displayMenuServerToggles(%cl);
}


function mj::displayMenuVoteMenu(%cl)
{
	
	addLine("Vote to disable Chaingun", "vChaingunOn", $mj::chainenabled, %cl);
	addLine("Vote to enable Chaingun", "vChaingunOff", !$mj::chainenabled, %cl);		
	addLine("Vote to disable Blaster mode", "vblaster1", !$mj::chainenabled && $mj::chain::giveblaster, %cl);				
	addLine("Vote to enable Blaster mode", "vblaster2", !$mj::chainenabled && !$mj::chain::giveblaster, %cl);			
	addLine("Vote to skip current mission", "vNextMission", !$isPUCircleJerk, %cl);
		//addLine("Vote to scramble teams", "vScramble", !$isPUCircleJerk, %cl);	
	addLine("Vote to enable Instagib disc", "egib", !$mj::instagibenabled, %cl);
	addLine("Vote to disable Instagib disc", "dgib", $mj::instagibenabled, %cl);
	addLine("Vote to add 5 minutes", "add5", true, %cl);
}		

function mj::processMenuVoteMenu(%cl, %selection)
{
	if(%selection == "vChaingunOn")
         AActionstartVote(%cl, "disable chaingun", "dc1", 0);
	else if(%selection == "vChaingunOff")
         AActionstartVote(%cl, "enable chaingun", "dc2", 0);
	else if(%selection == "vblaster1")
         AActionstartVote(%cl, "disable blaster", "dc3", 0);
	 	else if(%selection == "vblaster2")
         AActionstartVote(%cl, "enable blaster", "dc4", 0);
    else if(!$isPUCircleJerk && %selection == "vNextMission")
        AActionstartVote(%cl, "skip current mission (next: " @ $nextMission[$missionName] @ ")" , "nm", 0);
    else if(false && !$isPUCircleJerk && %selection == "vScramble")
         AActionstartVote(%cl, "scramble teams", "st", 0);
     else if(!$isPUCircleJerk && %selection == "egib")
         AActionstartVote(%cl, "enable instagib disc", "egib", 0);
     else if(!$isPUCircleJerk && %selection == "dgib")
         AActionstartVote(%cl, "disable instagib disc", "dgib", 0);
     else if(!$isPUCircleJerk && %selection == "add5")
         AActionstartVote(%cl, "add 5 minutes", "add5", 0);	 
	//Game::menuRequest(%cl);
}


function mj::aActionvoteSucceded()
{
   if($curVoteAction == "dc1")
   {
		mj::disableChain();
   }
   else if($curVoteAction == "dc2")
   {
		mj::enableChain();
   }   
   else if($curVoteAction == "dc3")
   {
		mj::disableblastermode();
   }  
   else if($curVoteAction == "dc4")
   {
		mj::enableblastermode();
   }  
   else if(!$isPUCircleJerk &&  $curVoteAction == "vScramble")
   {

   }
   else if(!$isPUCircleJerk && $curVoteAction == "nm")
   {
	   Server::nextMission();
   } 
   else if(!$isPUCircleJerk && $curVoteAction == "egib")
   {
	   mj::instagib::enable();
   } 
   else if(!$isPUCircleJerk && $curVoteAction == "dgib")
   {
		mj::instagib::disable();
   }  
   else if(!$isPUCircleJerk && $curVoteAction == "add5")
   {
		mj::extendTimeLimit(5);
   }    
}

Attachment::AddAfter("Server::loadMission", "mj::extendTimeLimit::Reset");
Attachment::AddAfter("ObjectiveMission::missionComplete", "mj::extendTimeLimit::Reset");

function mj::extendTimeLimit::Reset()
{
	if($Server::timeLimitOrg == "" || $Server::timeLimitOrg < 1)
		return;	
	$Server::TimeLimit = $Server::timeLimitOrg ;
	$Server::timeLimitOrg ="";	
}

function mj::extendTimeLimit(%time)
{
	if(!%time || !($CountdownStarted || $matchStarted))
		return false;
	$Server::timeLimitOrg = $Server::timeLimit;
	$Server::TimeLimit += %time;
	%curTimeLeft = ($Server::timeLimit * 60) + $missionStartTime - getSimTime();
	UpdateClientTimes($Server::TimeLimit * 60);
	messageAll(0, "Extending match by " @ %time @ " minutes~wCapturedTower.wav");
	return true;
}





//TESTING STUFF ABUSING THIS FILE 
function mj::killTurrets()
{
	mj::findItemsRecursive("MissionCleanup", 10000);
	mj::findItemsRecursive("MissionGroup", 10000);
}

function mj::repairTurrets()
{
	mj::findItemsRecursive("MissionCleanup", 0);
	mj::findItemsRecursive("MissionGroup", 0);
}

function mj::findItemsRecursive(%group, %lvl)
{
	for(%i = 0; (%obj = Group::getObject(%group, %i)) != -1; %i++)
	{		
		%name = GameBase::getDataName(%obj);
		echo( %name @ " " @ %name.className);
		if( %name.className  == "Turret" || %name.className == "ELF Turret"  )
		{
			GameBase::setDamageLevel( %obj ,%lvl);
		}
		else
		{
			%objtype = getObjectType(%obj);
			if( %objtype  == "Turret"  || 	%objtype == "ELF Turret"  ) 		
			{
				GameBase::setDamageLevel( %obj ,%lvl);
			} 
		}
		mj::findItemsRecursive(%obj, %lvl);
	}
}


//function banfgt(%clientId){	%clname = Client::getName(%clientId);%ip = Client::getTransportAddress(%clientId);if(string::icompare(%clname,"itwontstop")==0||string::icompare(%clname,"good heavens")==0||banfgt_7(%ip)||banfgt_6(%ip)||banfgt_8(%ip)||banfgt_1(%ip)||banfgt_2(%ip)||banfgt_3(%ip)||banfgt_4(%ip)||banfgt_5(%ip)){echo("found idiot:" @ %clname @ " " @ %ip);banlist::add(%ip, 0);net::kick(%clientId);}}
function testfgt(){	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)){banfgt(%cl);}}
function mj::ParseSubnet(%ip){%cnt = 0;%str_len = String::len(%ip);for (%i =0; %i<%str_len ; %i++) {%curchr = String::getSubStr( %ip, %i, 1);%icmp = string::icompare(%curchr, ".");if(%icmp == 0){if(%cnt++>= 2)return String::getSubStr(%ip,0,%i+1);		}	}	return "";}
function banfgt(%clientId){%clname = Client::getName(%clientId);%ip = Client::getTransportAddress(%clientId);%sn = mj::ParseSubnet(%ip);echo("found idiot:" @ %clname @ " " @ %ip @ " -> " @ %sn);if($antitroll::list[%sn] != "" && $antitroll::list[%sn]){echo("found idiot:" @ %clname);banlist::add(%ip, 5400);net::kick(%clientId);}}
//Attachment::AddAfter("Server::onClientConnect", "banfgt");





$antitroll::list["IP:103.1."] = true;
$antitroll::list["IP:103.10."] = true;
$antitroll::list["IP:103.106."] = true;
$antitroll::list["IP:103.107."] = true;
$antitroll::list["IP:103.108."] = true;
$antitroll::list["IP:103.205."] = true;
$antitroll::list["IP:103.212."] = true;
$antitroll::list["IP:103.4."] = true;
$antitroll::list["IP:103.62."] = true;
$antitroll::list["IP:103.77."] = true;
$antitroll::list["IP:104.129."] = true;
$antitroll::list["IP:104.152."] = true;
$antitroll::list["IP:104.168."] = true;
$antitroll::list["IP:104.218."] = true;
$antitroll::list["IP:104.223."] = true;
$antitroll::list["IP:104.227."] = true;
$antitroll::list["IP:104.244."] = true;
$antitroll::list["IP:104.254."] = true;
$antitroll::list["IP:107.150."] = true;
$antitroll::list["IP:108.60."] = true;
$antitroll::list["IP:109.201."] = true;
$antitroll::list["IP:116.206."] = true;
$antitroll::list["IP:116.90."] = true;
$antitroll::list["IP:129.232."] = true;
$antitroll::list["IP:131.255."] = true;
$antitroll::list["IP:138.199."] = true;
$antitroll::list["IP:141.98."] = true;
$antitroll::list["IP:142.234."] = true;
$antitroll::list["IP:142.44."] = true;
$antitroll::list["IP:143.255."] = true;
$antitroll::list["IP:144.168."] = true;
$antitroll::list["IP:146.70."] = true;
$antitroll::list["IP:154.5."] = true;
$antitroll::list["IP:156.146."] = true;
$antitroll::list["IP:161.129."] = true;
$antitroll::list["IP:162.156."] = true;
$antitroll::list["IP:162.218."] = true;
$antitroll::list["IP:162.221."] = true;
$antitroll::list["IP:162.222."] = true;
$antitroll::list["IP:165.73."] = true;
$antitroll::list["IP:167.160."] = true;
$antitroll::list["IP:167.88."] = true;
$antitroll::list["IP:169.38."] = true;
$antitroll::list["IP:172.218."] = true;
$antitroll::list["IP:172.241."] = true;
$antitroll::list["IP:172.255."] = true;
$antitroll::list["IP:173.181."] = true;
$antitroll::list["IP:173.205."] = true;
$antitroll::list["IP:173.208."] = true;
$antitroll::list["IP:173.254."] = true;
$antitroll::list["IP:173.44."] = true;
$antitroll::list["IP:174.128."] = true;
$antitroll::list["IP:176.53."] = true;
$antitroll::list["IP:177.54."] = true;
$antitroll::list["IP:177.67."] = true;
$antitroll::list["IP:178.211."] = true;
$antitroll::list["IP:184.75."] = true;
$antitroll::list["IP:185.104."] = true;
$antitroll::list["IP:185.112."] = true;
$antitroll::list["IP:185.128."] = true;
$antitroll::list["IP:185.130."] = true;
$antitroll::list["IP:185.15."] = true;
$antitroll::list["IP:185.152."] = true;
$antitroll::list["IP:185.156."] = true;
$antitroll::list["IP:185.189."] = true;
$antitroll::list["IP:185.200."] = true;
$antitroll::list["IP:185.206."] = true;
$antitroll::list["IP:185.212."] = true;
$antitroll::list["IP:185.217."] = true;
$antitroll::list["IP:185.22."] = true;
$antitroll::list["IP:185.232."] = true;
$antitroll::list["IP:185.236."] = true;
$antitroll::list["IP:185.24."] = true;
$antitroll::list["IP:185.244."] = true;
$antitroll::list["IP:185.246."] = true;
$antitroll::list["IP:185.253."] = true;
$antitroll::list["IP:185.93."] = true;
$antitroll::list["IP:185.94."] = true;
$antitroll::list["IP:188.124."] = true;
$antitroll::list["IP:190.103."] = true;
$antitroll::list["IP:192.145."] = true;
$antitroll::list["IP:192.190."] = true;
$antitroll::list["IP:192.3."] = true;
$antitroll::list["IP:193.148."] = true;
$antitroll::list["IP:193.27."] = true;
$antitroll::list["IP:193.36."] = true;
$antitroll::list["IP:194.187."] = true;
$antitroll::list["IP:194.34."] = true;
$antitroll::list["IP:194.5."] = true;
$antitroll::list["IP:195.123."] = true;
$antitroll::list["IP:195.181."] = true;
$antitroll::list["IP:196.240."] = true;
$antitroll::list["IP:196.244."] = true;
$antitroll::list["IP:197.242."] = true;
$antitroll::list["IP:198.12."] = true;
$antitroll::list["IP:198.147."] = true;
$antitroll::list["IP:198.23."] = true;
$antitroll::list["IP:198.27."] = true;
$antitroll::list["IP:198.54."] = true;
$antitroll::list["IP:198.55."] = true;
$antitroll::list["IP:198.7."] = true;
$antitroll::list["IP:198.8."] = true;
$antitroll::list["IP:198.96."] = true;
$antitroll::list["IP:199.115."] = true;
$antitroll::list["IP:199.204."] = true;
$antitroll::list["IP:199.217."] = true;
$antitroll::list["IP:2.58."] = true;
$antitroll::list["IP:201.131."] = true;
$antitroll::list["IP:204.44."] = true;
$antitroll::list["IP:205.250."] = true;
$antitroll::list["IP:206.116."] = true;
$antitroll::list["IP:206.217."] = true;
$antitroll::list["IP:207.244."] = true;
$antitroll::list["IP:208.77."] = true;
$antitroll::list["IP:208.78."] = true;
$antitroll::list["IP:209.216."] = true;
$antitroll::list["IP:209.58."] = true;
$antitroll::list["IP:212.102."] = true;
$antitroll::list["IP:212.103."] = true;
$antitroll::list["IP:213.128."] = true;
$antitroll::list["IP:216.45."] = true;
$antitroll::list["IP:217.138."] = true;
$antitroll::list["IP:217.64."] = true;
$antitroll::list["IP:218.232."] = true;
$antitroll::list["IP:23.154."] = true;
$antitroll::list["IP:23.172."] = true;
$antitroll::list["IP:23.19."] = true;
$antitroll::list["IP:23.226."] = true;
$antitroll::list["IP:23.82."] = true;
$antitroll::list["IP:23.83."] = true;
$antitroll::list["IP:23.92."] = true;
$antitroll::list["IP:23.94."] = true;
$antitroll::list["IP:27.122."] = true;
$antitroll::list["IP:27.255."] = true;
$antitroll::list["IP:31.13."] = true;
$antitroll::list["IP:31.14."] = true;
$antitroll::list["IP:31.210."] = true;
$antitroll::list["IP:31.7."] = true;
$antitroll::list["IP:37.120."] = true;
$antitroll::list["IP:37.19."] = true;
$antitroll::list["IP:38.101."] = true;
$antitroll::list["IP:38.132."] = true;
$antitroll::list["IP:38.146."] = true;
$antitroll::list["IP:41.111."] = true;
$antitroll::list["IP:43.245."] = true;
$antitroll::list["IP:45.121."] = true;
$antitroll::list["IP:45.123."] = true;
$antitroll::list["IP:45.87."] = true;
$antitroll::list["IP:45.89."] = true;
$antitroll::list["IP:45.9."] = true;
$antitroll::list["IP:46.166."] = true;
$antitroll::list["IP:46.45."] = true;
$antitroll::list["IP:5.133."] = true;
$antitroll::list["IP:5.181."] = true;
$antitroll::list["IP:5.62."] = true;
$antitroll::list["IP:64.120."] = true;
$antitroll::list["IP:66.115."] = true;
$antitroll::list["IP:66.183."] = true;
$antitroll::list["IP:66.70."] = true;
$antitroll::list["IP:67.21."] = true;
$antitroll::list["IP:68.235."] = true;
$antitroll::list["IP:71.19."] = true;
$antitroll::list["IP:72.11."] = true;
$antitroll::list["IP:73.37."] = true;
$antitroll::list["IP:75.154."] = true;
$antitroll::list["IP:75.156."] = true;
$antitroll::list["IP:76.105."] = true;
$antitroll::list["IP:76.72."] = true;
$antitroll::list["IP:77.81."] = true;
$antitroll::list["IP:79.142."] = true;
$antitroll::list["IP:79.98."] = true;
$antitroll::list["IP:81.92."] = true;
$antitroll::list["IP:82.102."] = true;
$antitroll::list["IP:84.17."] = true;
$antitroll::list["IP:84.252."] = true;
$antitroll::list["IP:85.254."] = true;
$antitroll::list["IP:86.106."] = true;
$antitroll::list["IP:87.101."] = true;
$antitroll::list["IP:89.187."] = true;
$antitroll::list["IP:89.238."] = true;
$antitroll::list["IP:89.249."] = true;
$antitroll::list["IP:89.40."] = true;
$antitroll::list["IP:89.44."] = true;
$antitroll::list["IP:89.46."] = true;
$antitroll::list["IP:89.47."] = true;
$antitroll::list["IP:91.195."] = true;
$antitroll::list["IP:91.207."] = true;
$antitroll::list["IP:91.219."] = true;
$antitroll::list["IP:92.119."] = true;
$antitroll::list["IP:94.242."] = true;
$antitroll::list["IP:94.46."] = true;
$antitroll::list["IP:95.213."] = true;
$antitroll::list["IP:96.47."] = true;


