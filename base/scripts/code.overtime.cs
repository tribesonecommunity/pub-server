Attachment::AddBefore("ObjectiveMission::CheckScoreLimit", "Overtime::ObjectiveMission::CheckScoreLimit");
Attachment::AddBefore("Game::CheckTimeLimit", "Overtime::Game::CheckTimeLimit");
Attachment::AddAfter("Game::startMatch", "Overtime::ResetOvertime");

function Overtime::Game::CheckTimeLimit()
{
  if ($Server::timeLimit == 0)
    return;

  %curTimeLeft = ($Server::timeLimit * 60) + floor($missionStartTime) - floor(getSimTime());
  if(%curTimeLeft <= 0 && $matchStarted && Game::IsTie())
    Overtime::SetOvertime();

  return;
}

function Overtime::ObjectiveMission::CheckScoreLimit()
{
  if($Game::LT::Overtime && !Game::isTie())
  {
    Overtime::ResetOvertime();
	ObjectiveMission::RefreshTeamScores();
	ObjectiveMission::missionComplete();
    return "halt";
  }

  return;
}

function Overtime::SetOvertime()
{
  $Game::LT::Overtime = true;
  $Game::LT::ServerTimeLimit = $Server::TimeLimit;
  $Server::timeLimit = 0;
  messageAll(1, "Extending, we're going into overtime!~wmine_act.wav");
}

function Overtime::ResetOvertime()
{
  if ($Game::LT::OverTime) {
    $Game::LT::Overtime = false;
	$Server::timeLimit = $Game::LT::ServerTimeLimit;
  }
}

function Game::isTie()
{
  %tieGame = true;
  for(%i = 0; %i < getNumTeams(); %i++)
  {
    if(
        ($teamScore[%i] != $teamScore[%i+1]) &&
         $teamScore[%i] != "" && $teamScore[%i+1] != ""
    )
	  %tieGame = false;
	}
	return %tieGame;
}