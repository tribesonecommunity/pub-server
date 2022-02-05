
//Attachment::AddAfter("Client::onFlagPickup",  "Announcer::onFlagPickup");
//Attachment::AddAfter("Client::onFlagDrop",    "Announcer::onFlagDrop");
//Attachment::AddAfter("Client::onFlagReturn",  "Announcer::onFlagReturn");
//Attachment::AddAfter("Game::startMatch",      "Announcer::onMatchStart");
//Attachment::AddAfter("Server::Countdown",     "Announcer::onCountdown");
//Attachment::AddAfter("Client::onFlagCap",     "Announcer::onFlagCap");
//Attachment::AddBefore("Client::onKilled",     "Announcer::onKilled");

// Pass detection
//Attachment::AddAfter("Client::onFlagDrop",          "Announcer::GetPasser");
//Attachment::AddAfter("Client::onFlagGrab",          "Announcer::ResetPass");
//Attachment::AddAfter("Client::onFlagReturn",        "Announcer::ResetPass");

function Announcer::onCountdown(%time) 
{
  Announcer::announce("~wprepare3.wav");
}

function Announcer::MidairFlagReturn(%returnerName, %flagTeamName)
{
  Announcer::announce("~wknife3.wav");
}

function Announcer::MidairPass(%passerName, %catcherName, %flagTeamName)
{
  Announcer::announce("~wimpressive.wav");
}

function Announcer::onFlagCap(%team, %cl)
{
  if (!$Announcer::FirstBloodCap) {
    $Announcer::FirstBloodCap = true;
    Announcer::announce("~wfirstblood.wav");
    //zadmin::ActiveMessage::All( FirstBloodCap, Client::GetName(%cl), getTeamName(%team) );
  }

  %clTeam = Client::getTeam(%cl);
  if ($Announcer::teamLastCap == %clTeam) {
    %clTeam.capStreak += 1;
    if (%clTeam.capStreak >= 3) {
      schedule('Announcer::announce("~wownage.wav");', 3);
      //schedule('remoteBP(2048, "<JC><f2>' @ %teamName @ ' <f1>are <f2>OWNING!", 3);', 3);
    }
  }
    //zadmin::ActiveMessage::All( CapStreak, getTeamName(%clTeam), %clTeam.capStreak );

  else {
    for (%i = 0; %i < getNumTeams(); %i++)
      Announcer::ResetCapStreak(%i);
    %clTeam.capStreak += 1;
    $Announcer::teamLastCap = %clTeam;
  }

  %score = %cl.activity["Cap"];
  if (%score == 3) {
    Announcer::announce("~wdominating.wav");
    //remoteBP(2048, "<JC><f2>" @ %playerName @ " <f1>is <f2>dominating!", 3);
  }
  else if (%score == 4) {
    Announcer::announce("~wunstoppable.wav");
    //remoteBP(2048, "<JC><f2>" @ %playerName @ " <f1>is <f2>unstoppable!", 3);
  }
  else if (%score >= 5) {
    Announcer::announce("~wrampage.wav");
    //remoteBP(2048, "<JC><f2>" @ %playerName @ " <f1>is on a <f2>RAMPAGE!", 3);
  }
  //zadmin::ActiveMessage::All( ScoreCap, Client::GetName(%cl), %score );
}

function Announcer::onMatchStart()
{
  $Announcer::FirstBloodCap = false;
  $Announcer::FirstBloodKill = false;
  $Announcer::teamLastCap = -1;
  for (%i = 0; %i < getNumClients(); %i++) {
    Announcer::ResetKillStreak(%cl);
  }
  for (%i = 0; %i < getNumTeams(); %i++)
    Announcer::ResetCapStreak(%i);

  Announcer::announce("~wprepare4.wav");
}

function Announcer::announce(%sound) //, %msg)
{
  if ($QuakeAnnounce::enabled)
  {
    for (%i = 0; %i < getNumClients(); %i++) {
      %cl = getClientByIndex(%i);
      if (!%cl.quakeAnnouncerDisabled)
      {
        Client::SendMessage(%cl, 0, %sound);
        //if (%msg)
          //BottomPrint(%cl, "<jc>" @ %msg);
      }
    }
  }
}

function Announcer::ResetCapStreak(%team)
{
  %team.capStreak = 0;
}

function Announcer::ResetKillStreak(%cl)
{
  %cl.killStreak = 0;
  %cl.lastKillTime = getSimTime();
  %cl.multiKillCount = 0;
}

function Announcer::onKilled(%playerId, %killerId, %damageType)
{
  Announcer::ResetKillStreak(%playerId);
  if (%playerId != %killerId && %killerId)
  {
    if (Client::getTeam(%playerId) != Client::GetTeam(%killerId)) {
      %killerId.killStreak += 1;

      if (!$Announcer::FirstBloodKill) {
        $Announcer::FirstBloodKill = true;
        Announcer::announce("~wfirstblood2.wav");
      }

      Announcer::UpdateMultiKill(%killerId);
      Announcer::CheckKillStreak(%killerId);

      %time = getIntegerTime(true) >> 5;
      %oppositeTeam = Client::GetTeam(%playerId) ^ 1;

      if ( (%time == $Stats::FlagDropped[%oppositeTeam]) && ($Stats::PlayerDropped[%oppositeTeam] == %playerId) ) {
        if ((%killerId.lastMidairTarget == %playerId) && (%killerId.lastMidairTime == getSimTime())) {
          Announcer::announce("~wheadshot3.wav");
          //zadmin::ActiveMessage::All( MidairCarrierKill, Client::getName(%killerId), Client::getName(%playerId) );
        }
        Announcer::checkCarrierKills(%killerId);
      }
    }
  }
}

function Announcer::checkCarrierKills(%killerId)
{
  %score = %killerId.activity["CarrierKill"];
  // We attach before, so target score - 1
  if (%score == 14) {
    schedule('Announcer::announce("~wwickedsick.wav");', 3);
    //schedule('localSound(wickedsick);', 3);
    //schedule('remoteBP(2048, "<JC><f2>' @ %playerName @ ' <f1>is <f2>wicked sick!", 3);', 3);
  }
  else if (%score == 19) {
    schedule('Announcer::announce("~wgodlike.wav");', 3);
    //schedule('localSound(godlike);', 3);
    //schedule('remoteBP(2048, "<JC><f2>' @ %playerName @ ' <f1>is <f2>godlike!", 3);', 3);
  }
  else if (%score == 24) {
    schedule('Announcer::announce("~wholyshit.wav");', 3);
    //schedule('localSound(godlike);', 3);
    //schedule('remoteBP(2048, "<JC><f2>' @ %playerName @ ' <f1>is <f2>godlike!", 3);', 3);
  }
}

function Announcer::UpdateMultiKill(%cl)
{
  %multiKillThreshold = 10;

  if ((getSimTime() - %cl.lastKillTime) < %multiKillThreshold) {
    %cl.multiKillCount += 1;
    %count = %cl.multiKillCount;
    if (%count == 2) {
      Announcer::announce("~wdoublekill.wav");
    }
    else if (%count == 3) {
      Announcer::announce("~wtriplekill.wav");
    }
    else if (%count == 4) {
      Announcer::announce("~wultrakill.wav");
    }
    //zadmin::ActiveMessage::All ( MultiKill, Client::GetName(%cl), %cl.multiKillCount );
  }
  else {
    %cl.multiKillCount = 1;
  }

  %cl.lastKillTime = getSimTime();
}

function Announcer::CheckKillStreak(%cl)
{
  %streak = %cl.killStreak;
  if (%streak == 4) {
    Announcer::announce("~wkillingspree.wav");
  }
  else if (%streak == 5) {
    Announcer::announce("~wmegakill.wav");
  }
  else if (%streak >= 6) {
    Announcer::announce("~wmonsterkill.wav");
  }
  //if (%cl.killStreak >= 2)
    //zadmin::ActiveMessage::All ( KillStreak, Client::GetName(%cl), %cl.killStreak );
}

function Announcer::GetPasser(%team, %name)
{
  %clTeam = Client::GetTeam(%name);
  $Announcer::Passer[%clTeam] = %name;
}

function Announcer::ResetPass(%team, %name)
{
  $Announcer::Passer[%team] = -1;
}
