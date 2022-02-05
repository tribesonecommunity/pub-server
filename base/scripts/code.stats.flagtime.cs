// Flag time
Attachment::AddAfter("Client::onFlagGrab",					"Stats::FlagTime::StartTimer");
Attachment::AddAfter("Client::onFlagPickup",				"Stats::FlagTime::StartTimer");
Attachment::AddAfter("Client::onFlagCap",						"Stats::FlagTime::StopTimer");
Attachment::AddAfter("Client::onFlagDrop",					"Stats::FlagTime::StopTimer");
Attachment::AddBefore("Stats::DisplayScores",       "Stats::FlagTime::ExtraTime");
//Attachment::AddBefore("ObjectiveMission::missionComplete",  "Stats::FlagTime::ExtraTime");

// Pass detection
Attachment::AddAfter("Client::onFlagDrop",					"Stats::FlagTime::GetPasser");
Attachment::AddAfter("Client::onFlagGrab",					"Stats::FlagTime::ResetPass");
Attachment::AddAfter("Client::onFlagReturn",				"Stats::FlagTime::ResetPass");

// Calculate flag time at map end, this gets called frequently...
function Stats::FlagTime::ExtraTime()
{
  if ($missionComplete) {
    for(%i = 0; %i < getNumTeams(); %i++) {
      %loc = $Stats::FlagLoc[%i];
      if (%loc != "home" && %loc != "field")
        $Stats::FlagLoc[%i] = "field";
        Stats::FlagTime::StopTimer(%i, %loc);
    }
  }
}

function Stats::FlagTime::StartTimer(%team, %name)
{
	$Stats::FlagTime::StartTime[%name] = getSimTime();
}

function Stats::FlagTime::StopTimer(%team, %name)
{
  %time = getSimTime();
	%duration = %time - $Stats::FlagTime::StartTime[%name];
  Stats::Client::adjustScoreMultiply(%name, "FlagTime", %duration);
}

function Stats::FlagTime::GetPasser(%team, %name)
{
	%clTeam = Client::GetTeam(%name);
	$Stats::FlagTime::Passer[%clTeam] = %name;
}

function Stats::FlagTime::ResetPass(%team, %name)
{
	$Stats::FlagTime::Passer[%team] = -1;
}
