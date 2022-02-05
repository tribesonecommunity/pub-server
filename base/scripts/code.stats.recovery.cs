Attachment::AddAfter("Mission::init", "Stats::Mission::Init");
Attachment::AddAfter("Game::ResetScores", "Stats::Recovery::ResetScores");
Attachment::AddAfter("Server::onClientConnect", "Stats::Server::onClientConnectDelay");
Attachment::AddAfter("Server::onClientDisconnect", "Stats::Server::onClientDisconnect");

function Stats::Server::OnClientConnectDelay(%clientId) {
  schedule("Stats::Server::onClientConnect("@%clientId@");", 5);
}

//function Stats::Mission::Init()
//{
	//echo("Deleting Stats::Recovery::Mission::Init");
	//deleteVariables("$Stats::Recovery*");
	//$Stats::RecoveryToken = floor(getRandom() * 100000);
//}

function Stats::Recovery::ResetScores()
{
	deleteVariables("$Stats::Recovery*");
	$Stats::RecoveryToken = floor(getRandom() * 100000);
}

function Stats::GetRecoveryIndex(%clientId)
{
	// ip
	%ip = Client::GetTransportAddress(%clientId);
	if(String::findSubStr(%ip, "IP:") == 0)
		%ip = String::getSubStr(%ip, 3, 16) @ ":";
	%ip = String::getSubStr(%ip, 0, String::findSubStr(%ip, ":"));

	// + name
	%idx = %ip @ escapeString(Client::GetName(%clientId));

	// + recovery token
	if($Stats::RecoveryToken == "")
		Stats::Recovery::ResetScores();
		//Stats::Mission::Init();
	%idx = %idx @ $Stats::RecoveryToken;

	echo("Client Recovery Index = " @ %idx);
  //export("$Stats::Recovery*", "temp\\StatsRecovery.cs", false);
	return %idx;
}

function Stats::Server::onClientDisconnect(%clientId)
{
	%idx = Stats::GetRecoveryIndex(%clientId);

	for(%i = 0; %i <= $Stats::ActivityCount; %i++)
  	$Stats::Recovery[%idx,%i] = %clientId.activity[$Stats::Activity[%i]];
}

function Stats::Server::onClientConnect(%clientId)
{
	%idx = Stats::GetRecoveryIndex(%clientId);

	if($Stats::Recovery[%idx,0] != "")
	{
		for(%i = 0; %i <= $Stats::ActivityCount; %i++)
   		%clientId.activity[$Stats::Activity[%i]] = $Stats::Recovery[%idx,%i];

		%clientId.score = %clientId.activity["Rating"];
   	Client::setScore(%clientId, "%n\t%t\t  " @ floor(%clientId.score)  @ "\t%p\t%l", %clientId.score + (9 - %team) * 10000);
	}
	else
		Game::resetScores(%clientId);
}

