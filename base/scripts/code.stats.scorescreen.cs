Attachment::AddBefore("Game::ResetScores",                    "Stats::Game::ResetScores");
//Attachment::AddBefore("ObjectiveMission::ObjectiveChanged",   "Stats::ObjectiveMission::ObjectiveChanged");
Attachment::AddBefore("ObjectiveMission::SetObjectiveHeading","Stats::ObjectiveMission::SetObjectiveHeading");
Attachment::AddBefore("ObjectiveMission::RefreshTeamScores",  "Stats::ObjectiveMission::RefreshTeamScores");
Attachment::AddBefore("remoteToggleObjectivesMode",           "Stats::remoteToggleObjectivesMode");

function Stats::GetRating(%client, %activity)
{
	%highlight = "";
	%rating = %client.activity[%activity];

	if (%rating == 0) {
		if (%activity != "FlagTime") {
			return 0;// @ $Stats::RowFont;
		} else {
			return Stats::FlagTimeToMinSec(%rating);
		}
	}

	if(%activity == "Suicide"  ||
	   %activity == "TeamKill" ||
	   %activity == "MidAirCatch" ||
	   %activity == "Drop" ||
	   %activity == "DamageTaken" ||
	   String::FindSubStr(%activity, "Death") != -1) {
		if($Stats::HighScore[%activity] == %rating)
		  %highlight = $Stats::TextColor["Red"];
		else if($Stats::LowScore[%activity] == %rating)
		  %highlight = $Stats::TextColor["Green"];
	}	else {
		if($Stats::HighScore[%activity] == %rating)
		  %highlight = $Stats::TextColor["Green"];
		else if($Stats::LowScore[%activity] == %rating)
		  %highlight = $Stats::TextColor["Red"];
	}

	if (%activity == "FlagTime")
		%rating = Stats::FlagTimeToMinSec(%rating);
  else
    %rating = floor(%rating);

	//if (%activity == "Damage")
		//%rating = floor(%rating);

	if (%highlight != "")
		return %highlight @ %rating @ $Stats::RowFont;
	else	// Save some character space here
		return %rating;
}

function Stats::GetAward(%client)
{
  %title = " ";
  if ($Stats::Awards::enabled)
  {
    // Disabling highlighting, we can save 4 characters
    //%highlight = $Stats::TextColor["yellow"];
    for (%i = 0; %i < $Stats::AwardCount; %i++) {
      %award = $Stats::Award[%i];

      // Order matters here. First award found is returned.
      if ($Stats::ClientAward[%award] == %client) {
        %title = %award;
        //return %highlight @ %title;//@ $Stats::RowFont;
        return %title;
      }
    }
    //return %highlight @ %title;// @ $Stats::RowFont;
  }
  return %title;
}

function Stats::FlagTimeToMinSec(%val)
{
  %minutes = floor(%val / 60);
  %seconds = floor(%val % 60);

  if (%seconds < 10)
    %seconds = "0" @ %seconds;
  return %minutes @ ":" @ %seconds;
}

function Stats::Tab(%tab)
{
	return "<L" @ $Stats::TabIndent[%tab] @ ">";
}

function Stats::Combo(%arg1, %arg2, %arg3, %arg4, %arg5)
{
	while(%arg[%i++] != "")
	{
		if(%i - 1 != 0)
			%line = %line @ " / " @ %arg[%i];
		else
			%line = %line @ %arg[%i];
	}
	return %line;
}

function Stats::GetTeamName(%clientId)
{
	%team = Client::GetTeam(%clientId);
	if(%team == -1)
		return "Obs";

	%teamName = getTeamName(%team);

	%token = String::FindSubStr(%teamName, " ");
	if(%token)
		%teamAbbr = String::GetSubStr(%teamName, 0, 1) @ String::GetSubStr(%teamName, %token+1, 1);
	else
		%teamAbbr = String::GetSubStr(%teamName, 1, 2);

	return %teamAbbr;
}

function Stats::Reset(%clientId)
{
	for(%i = 0; %i < $Stats::ActivityCount; %i++)
   	%clientId.activity[$Stats::Activity[%i]] = 0;
}

function Stats::Game::ResetScores(%client)
{
  // don't reset observer scores
  if(%client.observerMode == "observerOrbit" || %client.observerMode == "observerFly" || %client.observerMode == "observerFirst")
    return "halt";

  //for (%i = 0; %i < 128; %i++) {
    //%cl = getClientByIndex(%i);
    //%cl.ratio = 0;
    //%cl.score = 0;
    //Stats::Reset(%cl);
  //}

  //return "halt";

	if(%client == "")
	{
	  for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	  {
			%cl.ratio = 0;
    	%cl.score = 0;
    	Stats::Reset(%cl);
		}
	}
	else
	{
		%client.ratio = 0;
   	%client.score = 0;
   	Stats::Reset(%client);
	}

	return "halt";
}

function Stats::ObjectiveMission::objectiveChanged(%this)
{
  return "halt";
}

// Stats Header
function Stats::DisplayHeader(%line)
{
	%indent = 6;
	for(%i = 0; %i < $Stats::TabCount; %i++)
	{
		%heading = $Stats::Tab[%i];
		%pair = String::findSubStr(%heading, "-");
		if(%pair != -1) {
			%topHead = %topHead @ "<L" @ %indent @ ">" @ String::getSubStr(%heading, 0, %pair);
			%bottomHead = %bottomHead @ "<L" @ %indent @ ">" @ String::getSubStr(%heading, %pair + 1, 15);
		}
		else
			%bottomHead = %bottomHead @ "<L" @ %indent @ ">" @ $Stats::Tab[%i];

		// spacing for stat output
		%indent += $Stats::TabSize[%heading];
		$Stats::TabIndent[%i] = %indent;
	}

	// display
	for(%i = -1; %i <= getNumTeams(); %i++) {
		if(%line == "") %linen = 6; else %linen = %line;
		Team::SetObjective(%i, %linen, %topHead);
		Team::SetObjective(%i, %linen++, %bottomHead);
	}

	return %linen + 1;
}

function Stats::GetRowColor(%j, %cl)
{
  if ($Lasthope::enabled)
  {
//    if ($Lasthope::strict) // alternate row coloring if only 1.41 clients allowed
//    {
//      if(%j % 2)
//        return $Stats::TextColor["Yellow"];
//      else
//        return $Stats::TextColor["White"];
//    }
    //else { // otherwise, highlight 1.41 clients
    if(true)
    {
      if (%cl.client == "1.41")
        return $Stats::TextColor["Yellow"];
      else
        return $Stats::TextColor["White"];
    }
  }
  else
  {
    if (%j % 2)
      return $Stats::TextColor["Yellow"];
    else
      return $Stats::TextColor["White"];
  }
}

// Stats Scores
function Stats::DisplayScores(%line)
{
	%numClients = getNumClients();

	// Get low/high score list
  for(%h = 0; %h < $Stats::ActivityCount; %h++)
  {
  	%activity = $Stats::Activity[%h];
 		$Stats::HighScore[%activity] = -1000;
		$Stats::LowScore[%activity] = 1000;

		for(%i = 0; %i < %numClients; %i++) {
 	  	%client[%i] = getClientByIndex(%i);

			// high score
			%thisScore = %client[%i].activity[%activity];
    	if(%thisScore >= $Stats::HighScore[%activity] && %thisScore > 0)
     		$Stats::HighScore[%activity] = %thisScore;

    	// low score
    	if(%thisScore <= $Stats::LowScore[%activity])
    		$Stats::LowScore[%activity] = %thisScore;
  	}
  }

  if ($Stats::Awards::enabled)
    Stats::CalculateAwards();

  // bubble sort clients
  for(%i = 0; %i < %numClients; %i++) {
    for(%j = 0; %j < %numClients; %j++)
      if(%client[%j].score < %client[%j + 1].score)
      {
        %tmp = %client[%j + 1];
        %client[%j + 1] = %client[%j];
        %client[%j] = %tmp;
      }
  }

  if(%line == "") {
  	%line = 3;
  }

  for(%j = 0; %j < $Server::MaxPlayers; %j++) {
    %cl = %client[%j];
    $Stats::RowFont = Stats::GetRowColor(%j, %cl);

  	if(%j < $Server::MaxPlayers && %cl != "") {
  	// remove format characters
  	%name = String::Replace(Client::GetName(%cl), "<", "");
  	%name = String::Replace(%name, ">", "");
  
  	// stat line
  	%stats = 	"\t"  @ %j+1 @ ".\t" @
  		$Stats::RowFont @ escapeString(%name) @
  		Stats::Tab(%tab=0) @ Stats::GetTeamName(%cl) @
  		Stats::Tab(%tab++) @ Stats::GetRating(%cl, "Rating") @
  		Stats::Tab(%tab++) @ Stats::Combo(Stats::GetRating(%cl, "Kills"), 	Stats::GetRating(%cl, "Deaths"),	Stats::GetRating(%cl, "Suicide")) @
  		Stats::Tab(%tab++) @ Stats::Combo(Stats::GetRating(%cl, "Disc Launcher"),Stats::GetRating(%cl, "Chaingun"), Stats::GetRating(%cl, "Explosives")) @
  		Stats::Tab(%tab++) @ Stats::Combo(Stats::GetRating(%cl, "CarrierKill"),	Stats::GetRating(%cl, "Return")) @
      Stats::Tab(%tab++) @ Stats::Combo(Stats::GetRating(%cl, "Cap"),    Stats::GetRating(%cl, "Assist")) @
  		Stats::Tab(%tab++) @ Stats::Combo(Stats::GetRating(%cl, "Grab"), Stats::GetRating(%cl, "Pickup")) @
  		Stats::Tab(%tab++) @ Stats::GetRating(%cl, "MidAir") @
  		Stats::Tab(%tab++) @ Stats::GetRating(%cl, "FlagTime") @
      Stats::Tab(%tab++) @ Stats::GetRating(%cl, "Damage"); // @
      //Stats::Tab(%tab++) @ Stats::GetAward(%cl);
  	}
  	else
  		%stats = " ";
  
    for(%i = -1; %i < getNumTeams(); %i++) {
  		Team::SetObjective(%i, %line, %stats);
    }
    %line++;
  }

	return %line;
}

function Stats::CalculateAwards()
{
  // Get the award winners
  for (%l = 0; %l < $Stats::AwardCount; %l++)  {
    %award = $Stats::Award[%l];
    $Stats::HighAward[%award] = -1;
    $Stats::ClientAward[%award] = -1;

    for (%i = 0; %i < %numClients; %i++) {
      %curAward = 0;

      %rating = %client[%i].activity["Rating"];
      %kills = %client[%i].activity["Kills"];
      %deaths = %client[%i].activity["Deaths"];
      %caps = %client[%i].activity["Cap"];
      %assists = %client[%i].activity["Assists"];
      %grabs = %client[%i].activity["Grab"];
      %pickups = %client[%i].activity["Pickup"];
      %teamkills = %client[%i].activity["Teamkill"];
      %carrierkills = %client[%i].activity["CarrierKill"];
      %midairs = %client[%i].activity["MidAir"];
      %disckills = %client[%i].activity["Disc Launcher"];
      %cgkills = %client[%i].activity["Chaingun"];
      %nadekills = %client[%i].activity["Explosives"];
      %suicides = %client[%i].activity["Suicide"];
      %grabspeed = %client[%i].activity["GrabSpeed"];
      %flagtime = %client[%i].activity["FlagTime"];

      // Would like to remove hardcoding here and move to globals
      if (%award == "emokid") {
        %curAward = %suicides;
      } else if (%award == "rape victim") {
        %curAward = %deaths;
      } else if (%award == "pointman") {
        %curAward = %caps + %assists; } else if (%award == "ballhog") {
        %curAward = %grabs + %pickups;
      } else if (%award == "pickup artist") {
        %curAward = %pickups;
      } else if (%award == "grabby") {
        %curAward = %grabs;
      } else if (%award == "colorblind") {
        %curAward = %teamkills;
      } else if (%award == "disklessonz") {
        %curAward = %disckills;
      } else if (%award == "towelhead") {
        %curAward = %nadekills;
      } else if (%award == "rocket disc") {
        %curAward = %midairs;
      } else if (%award == "route czar") {
        %curAward = %grabspeed;
      } else if (%award == "rabbit") {
        %curAward = %flagtime;
      } else if (%award == "pacifist") {
        if (%kills != 0) {
          %curAward = %rating / %kills;
        } else {
          %curAward = %rating;
        }
      } else if (%award == "killbot") {
        %curAward = %kills;
      } else if (%award == "cgpussy") {
        %curAward = %cgkills;
      } else if (%award == "dutch mode") {
        %curAward = %carrierkills;
      } else if (%award == "2good") {
        if (%midairs >= 10 && %carrierKills >= 15)
          %curAward = %carrierkills * %midairs;
      } else if (%award == "clutch") {
        %curAward = %clutchreturns;
      } else if (%award == "allstar") {
        if (%caps >= 3 && %carrierKills >= 10)
          %curAward = %caps * %carrierkills;
      }

      if (%curAward > $Stats::HighAward[%award] && %curAward >= $Stats::AwardThreshold[%award]) {
        $Stats::HighAward[%award] = %curAward;
        $Stats::ClientAward[%award] = %client[%i];
      }
    }
  }
}

function Stats::GetAward(%client)
{
  if ($Stats::Awards::enabled)
  {
    // Disabling highlighting, we can save 4 characters
    //%highlight = $Stats::TextColor["yellow"];
    %title = " ";
    for (%i = 0; %i < $Stats::AwardCount; %i++) {
      %award = $Stats::Award[%i];

      // Order matters here. First award found is returned.
      if ($Stats::ClientAward[%award] == %client) {
        %title = %award;
        //return %highlight @ %title;//@ $Stats::RowFont;
        return %title;
      }
    }
  }
  return %title;
  //return %highlight @ %title;// @ $Stats::RowFont;
}


function Stats::ObjectiveMission::refreshTeamScores()
{
  %nt = getNumTeams();
  for(%i = -1; %i < %nt; %i++)
 	  %teamSize[%i] = "0.001";

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
    %teamSize[Client::getTeam(%cl)]++;

  Team::setScore(0, "Obs\t" @ "   " @ 0 @ "\t " @ 0, 0);

  // build team score string
  %played = MapStats::GetStat("Wins", "Total", $MissionName);
  for(%i = 0; %i < %nt; %i++)
  {
    %wins = MapStats::GetStat("Wins::" @ %i, "Total", $MissionName);
    if(%played)
      %stat = "( <f3>"@ %wins @ $Stats::TextColor["Green"] @ "<f2>, <f3>" @ floor((%wins/%played) * 100) @ "%<f2> )";
    %teamStr = %teamStr @ "<f3>Team: <f2>" @ getTeamName(%i) @ %stat @ " - Captures: <f3>" @ $teamScore[%i] @ "<f2>";
    //%teamStr = %teamStr @ "<f3>Team: <f2>" @ getTeamName(%i) @ " - Captures: <f3>" @ $teamScore[%i] @ "<f2>";

    if(%i < %nt - 1) { // add comma
      %teamStr = %teamStr @ ", ";
    }
  }

  for(%i = -1; %i < %nt; %i++)
  {
    if(%i != -1)
      Team::setScore(%i, "%t\t" @ "   " @ $teamScore[%i] @ "\t " @ floor(%teamSize[%i]), 9-%i);
    Team::setObjective(%i, 3, "<f1>" @ %teamStr);
  }

  return "halt";
}

function Stats::remoteToggleObjectivesMode(%clientId)
{
	if(Client::getGuiMode(%clientId) != $GuiModeObjectives)
	{
		// Auto-update
		ObjectiveMission::SetObjectiveHeading();
		remoteObjectivesMode(%clientId);
	}
	else
		remotePlayMode(%clientId);

  return "halt";
}

function Stats::ObjectiveMission::SetObjectiveHeading()
{
  if($missionComplete)
  {
    %curLeader = 0;
		%tieGame = false;
		%tie = 0;
		%tieTeams[%tie] = %curLeader;
		for(%i = 0; %i < getNumTeams() ; %i++)
		  echo("GAME: teamfinalscore " @ %i @ " " @ $teamScore[%i]);

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

		if(%tieGame)
		{
			for(%g = 0; %g <= %tie; %g++)
			{
				%names = %names @ getTeamName(%tieTeams[%g]);
				if(%g == %tie-1)
					%names = %names @ " and ";
				else if(%g != %tie)
					%names = %names @ ", ";
			}

			if(%tie > 1)
				%names = %names @ " all";
		}

		for(%i = -1; %i < getNumTeams(); %i++)
    {
			if(!%tieGame)
			{
  			if($teamScore[%curLeader] != 1)
				  %s = "s";

	      if(%i == %curLeader)
					Team::setObjective(%i, 1, "<f3><jc>Your team won the mission with " @ $teamScore[%curLeader] @ " point"@%s@"!");
	  		else
	       	Team::setObjective(%i, 1, "<f3><jc>The " @ getTeamName(%curLeader) @ " team won the mission with " @ $teamScore[%curLeader] @ " point"@%s@"!");
		  }
			else
			{
				if(getNumTeams() > 2)
					Team::setObjective(%i, 1, "<f3><jc>The " @ %names @ " tied with a score of " @ $teamScore[%curLeader]);
				else
					Team::setObjective(%i, 1, "<f3><jc>The mission ended in a tie where each team had a score of " @ $teamScore[%curLeader]);
			}
	  }
  }

  if(!$Server::timeLimit)
    %timeStr = "No time limit";
  else if($timeLimitReached)
    %timeStr = "Time limit reached";
  else if($missionComplete)
  {
  	%time = getSimTime() - $missionStartTime;
    %minutes = Time::getMinutes(%time);
    %seconds = Time::getSeconds(%time);
    if(%minutes < 10)
      %minutes = "0" @ %minutes;
    if(%seconds < 10)
      %seconds = "0" @ %seconds;
      %timeStr = "Total match time: " @ %minutes @ ":" @ %seconds;
  }
  else
		%timeStr = floor($Server::timeLimit - (getSimTime() - $missionStartTime) / 60) @ " minutes";
  %timeStr = "<f2>Time remaining:<f3> " @ %timeStr;

  // Mission Info
  for(%i = -1; %i < getNumTeams(); %i++)
  {
  	objective::displayBitmap(%i,0);
    if(!$MissionComplete)
      Team::SetObjective(%i, 1, "");

    %stats = "( <f3>" @ MapStats::GetStat("Played", "Total", $MissionName) @ "<f2>, <f3>" @ MapStats::GetStat("Played", "Today", $MissionName) @ "<f2> )";
		Team::setObjective(%i, 2, "<f3>Mission Name: <f2>" @ $missionName @ %stats @ ", " @ %timeStr @ "<f2>, <f3>Captures: <f2>" @$teamScoreLimit@ " to win.");

 		%line = Stats::DisplayHeader(4);
		%line = Stats::DisplayScores(%line);
	}

	return "halt";
}
