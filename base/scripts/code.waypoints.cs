$FlagstandWaypoint::Waypointtext[1 == 1] = "Waypoint set to Friendly flag-stand.";
$FlagstandWaypoint::Waypointtext[1 == 0] = "Waypoint set to Enemy flag-stand.";

function remoteFlagstandWaypoint(%cl, %team){
	if($teamflag[%team] == "") 
		return;

	%pos = $teamflag[%team].originalPosition;
	if(%pos=="") 
		return;

	%sameteam = %team ==  Client::GetTeam(%cl);
	%x = getWord(%pos,0);
	%y = getWord(%pos,1);
	issueCommand(%cl,%cl, 5,$FlagstandWaypoint::Waypointtext[%sameteam], %x, %y);
}

