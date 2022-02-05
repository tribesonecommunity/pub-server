// see the following files:
// code.scoring.cs in scripts.vol
// code.game.cs - client::onKilled

Attachment::AddBefore("Client::AdjustScore",                  "Stats::Client::AdjustScore");
Attachment::AddBefore("Client::AdjustScoreNoUpdate",          "Stats::Client::AdjustScoreNoUpdate");
Attachment::AddBefore("Client::OnKilled",                     "Stats::Client::OnKilled");
Attachment::AddBefore("Client::adjustScoreMultiply",					"Stats::Client::adjustScoreMultiply");

function Stats::Client::adjustScore(%cl, %activity, %val)
{
	%score = $Stats::Rating[%activity];

	// increment 'this' activity
	if(%val)
		%cl.activity[%activity]+=%val;
	%cl.activity[%activity]++;
	%cl.score += %score;
	%cl.activity["Rating"] = %cl.score;

	%team = Client::getTeam(%cl);
	if(%team == -1) // observers go last.
		%team = 9;

	// objective mission sorts by team first.
	Client::setScore(%cl, "%n\t%t\t  " @ floor(%cl.score)  @ "\t%p\t%l", %cl.score + (9 - %team) * 10000);

	return "halt";
}

function Stats::Client::adjustScoreMultiply(%cl, %activity, %val)
{
	%score = $Stats::Rating[%activity];

	// increment 'this' activity
	if(%val)
		%cl.activity[%activity] += %val * %score;

	%cl.score += %val * %score;
	%cl.activity["Rating"] = %cl.score;

	%team = Client::getTeam(%cl);
	if(%team == -1) // observers go last.
		%team = 9;

	// objective mission sorts by team first.
	Client::setScore(%cl, "%n\t%t\t  " @ floor(%cl.score)  @ "\t%p\t%l", %cl.score + (9 - %team) * 10000);

	return "halt";
}

function Stats::Client::adjustScoreMultiplyNoHalt(%cl, %activity, %val)
{
	%score = $Stats::Rating[%activity];

	// increment 'this' activity
	if(%val)
		%cl.activity[%activity] += %val;

	%cl.score += %val * %score;
	%cl.activity["Rating"] = %cl.score;

	%team = Client::getTeam(%cl);
	if(%team == -1) // observers go last.
		%team = 9;

	// objective mission sorts by team first.
	Client::setScore(%cl, "%n\t%t\t  " @ floor(%cl.score)  @ "\t%p\t%l", %cl.score + (9 - %team) * 10000);
}

function Stats::Client::adjustScoreNoUpdate(%cl, %activity, %val)
{
	%score = $Stats::Rating[%activity];

	if(%val)
		%cl.activity[%activity]+=%val;

	%cl.activity[%activity]++;
	%cl.score += %score;
	%cl.activity["Rating"] = %cl.score;

	return "halt";
}

function Stats::Client::replaceScoreNoUpdate(%cl, %activity, %val)
{
	%score = $Stats::Rating[%activity];

	// Replaces the current score rather than incrementing
	if (%val)
		%cl.activity[%activity] = %val;

	return "halt";
}

function Stats::Client::onKilled(%playerId, %killerId, %damageType)
{
	if(%killerId != %playerId && %killerId)
	{
		if(Client::getTeam(%killerId) != Client::getTeam(%playerId)) {
			Client::adjustScoreNoUpdate(%playerId, "Deaths");
			Client::adjustScoreNoUpdate(%killerId, "Kills");

			if(Client::getName(%killerId) != "")
			{
				if($zadmin::WeaponName[%damageType] != "Mortar")
					Client::adjustScoreNoUpdate(%playerId, $zadmin::WeaponName[%damageType] @ "Death");
			}
		}
	}

	return;
}
