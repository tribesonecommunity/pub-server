//2011/03/01
exec("map.stats.cs");

// Attachments
Attachment::AddBefore("Mission::Init", "MapStats::Mission::Init");
Attachment::AddBefore("ObjectiveMission::MissionComplete", "MapStats::ObjectiveMission::MissionComplete");
Attachment::AddBefore("Vote::changeMission", "MapStats::Mission::Vote");
Attachment::AddAfter("remoteScoresOn", "MapStats::remoteScoresOn");

function MapStats::remoteScoresOn(%clientId)
{
  MapStats::InfoPopulate(%clientId, "Mission Info:", "", 1);
  MapStats::InfoPopulate(%clientId, "Name: ", $MissionName, 2);
  MapStats::InfoPopulate(%clientId, "Popularity: ",
    "Tx" @ MapStats::GetStat("Played", "Total", $MissionName) @ ", " @
    "Dx" @ MapStats::GetStat("Played", "Today", $MissionName) @ ", " @
    "VIx" @ MapStats::GetStat("VotedIn", "Total", $MissionName) @ ", " @
    "VOx" @ MapStats::GetStat("VotedOut", "Total", $MissionName), 3);
  //MapStats::InfoPopulate(%clientId, "Next: ",
    //getWord(SmartRotation::GetNextMission($SmartRotation::idxMission), 0), 4);
  MapStats::InfoPopulate(%clientId, "", "", 4);
  MapStats::InfoPopulate(%clientId, "", "", 5);
  MapStats::InfoPopulate(%clientId, "", "", 6);
}

function MapStats::InfoPopulate(%clientId, %head, %text, %line)
{
  %delay = 0.1; // client executes score.gui.cs, so hacky, but worky?
  if(%clientId) {
    schedule("remoteEval(" @ %clientId @ ", setInfoLine, " @ %line @ ",\"" @%head@ %text@ "\");", %delay);
  }
}

function MapStats::Mission::Init()
{
  MapStats::Update();
  Attachment::RemoveBefore("Mission::Init", "MapStats::Mission::Init");
}

function MapStats::ObjectiveMission::MissionComplete()
{
  // only once
  if($MapStats::Token == $Stats::RecoveryToken)
    return;
  $MapStats::Token = $Stats::RecoveryToken;

  // make sure map was completed, not voted out
  %curTimeLeft = ($Server::timeLimit * 60) + floor($missionStartTime) - floor(getSimTime());
  for(%i = 0; %i < getNumTeams(); %i++)
  {
    if($teamScore[%i] >= $teamScoreLimit)
      %done = true;
  }

  if(%done || %curTimeLeft == 0)
  {
    %curLeader = 0;
		%tieGame = false;
		%tie = 0;
		%tieTeams[%tie] = %curLeader;

    // Check for tie
		for(%i = 1; %i < getNumTeams() ; %i++)
    {
		  if($teamScore[%i] == $teamScore[%curLeader])
		  {
        %tieGame = true;
        %tieTeams[%tie++] = %i;
			}
			else if($teamScore[%i] > $teamScore[%curLeader])
      {
        %curLeader = %i;
        %tieGame = false;
				%tie = 0;
				%tieTeams[%tie] = %curLeader;
      }
    }

    if(!%tie)
    {
      MapStats::Increment("Wins" @ "::" @ %curLeader, "Today", $missionName);
      MapStats::Increment("Wins" @ "::" @ %curLeader, "Total", $missionName);
      MapStats::Increment("Wins", "Today", $missionName);
      MapStats::Increment("Wins", "Total", $missionName);
    }
    echo("Mapstats::Increment");
    MapStats::Increment("Played", "Today", $missionName);
    MapStats::Increment("Played", "Total", $missionName);
  }

  MapStats::Export();
}

function MapStats::Mission::Vote()
{
  // make sure this was a vote, and not a map switch.
  if($curVoteOption != "")
  {
    MapStats::Increment("VotedOut", "Today", $missionName);
    MapStats::Increment("VotedOut", "Total", $missionName);
    MapStats::Increment("VotedIn", "Today", $curVoteOption);
    MapStats::Increment("VotedIn", "Total", $curVoteOption);
    MapStats::Export();
  }
}

function MapStats::Update()
{
  // check if daily stats need removed
  %date = MapStats::GetDate();
  if($MapStats::Date == "")
    $MapStats::Date = %date;

  if(%date != $MapStats::TodaysDate)
  {
    deleteVariables("$MapStats::*::Today::*");
    $MapStats::TodaysDate = %date;
  }

  MapStats::Export();
}

function MapStats::Reset()
{
  DeleteVariables("$MapStats*");

  $MapStats::Date = MapStats::GetDate();
  MapStats::Export();
}

function MapStats::GetStat(%type, %tag, %map)
{
  eval("%stat = $MapStats::" @ %type @"::" @ %tag @ "::"@ %map @";");
  if(%stat == "")
    %stat = 0;
  return %stat;
}

function MapStats::GetDate()
{
  return getWord(timeStamp(), 0);
}

function MapStats::Increment(%type, %tag, %map)
{
  eval("$MapStats::" @ %type @"::" @ %tag @ "::"@ %map @"++;");
}

function MapStats::Export()
{
  export("$MapStats*", "temp\\map.stats.cs", false);
}