// Attachment Plugin Required
////////////////////////////////////////////////////////////////////////////////

Attachment::AddAfter("Game::CheckTimeLimit", "Notifications::CountDown");
//Attachment::AddAfter("aActionStartVote", "Notifications::aActionStartVote");
//Attachment::AddBefore("remoteVoteYes", "Notifications::remoteVoteYes");
//Attachment::AddBefore("remoteVoteNo", "Notifications::remoteVoteNo");

function Notifications::halftime() {
$Server::Halftime = true;
Game::DisplayHalfScoreboard();
Krayvok::resetAssets();
}

function Notifications::remoteVoteYes(%clientId) {
%ret = Vote::GetCount($curVoteCount);
Notifications::DisplayVotes(getWord(%ret, 0),getWord(%ret, 1),
getWord(%ret, 2),getWord(%ret, 3));
}

function Notifications::remoteVoteNo(%clientId) {
%ret = Vote::GetCount($curVoteCount);
Notifications::DisplayVotes(getWord(%ret, 0),getWord(%ret, 1),
getWord(%ret, 2),getWord(%ret, 3));
}

function Vote::GetCount(%curVote) {
if(%curVote != $curVoteCount)
return;

if($curVoteTopic == "")
return;

%votesFor = 0;
%votesAgainst = 0;
%votesAbstain = 0;
%totalClients = 0;
%totalVotes = 0;
for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
%totalClients++;
if(%cl.vote == "yes") {
%votesFor++;
%totalVotes++;
}
else if(%cl.vote == "no") {
%votesAgainst++;
%totalVotes++;
}
else
%votesAbstain++;
}
%minVotes = floor($Server::MinVotesPct * %totalClients);
if(%minVotes < $Server::MinVotes)
%minVotes = $Server::MinVotes;

if(%totalVotes < %minVotes) {
%votesAgainst += %minVotes - %totalVotes;
%totalVotes = %minVotes;
}
%margin = $Server::VoteWinMargin;
if($curVoteAction == "admin") {
%margin = $Server::VoteAdminWinMargin;
%totalVotes = %votesFor + %votesAgainst + %votesAbstain;
if(%totalVotes < %minVotes)
%totalVotes = %minVotes;
}
if(%votesFor / %totalVotes >= %margin) {
%votePassed = true;
}
else {
 // special team kick option:
if($curVoteAction == "kick") {
// check if the team did a majority number on him:
%votesFor = 0;
%totalVotes = 0;
for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
if(GameBase::getTeam(%cl) == $curVoteOption.kickTeam) {
%totalVotes++;
if(%cl.vote == "yes")
%votesFor++;
}
}
if(%totalVotes >= $Server::MinVotes && %votesFor / %totalVotes >= $Server::VoteWinMargin) {
%votePassed = true;
}
}
%votePassed = false;
}

return %votesFor @ " " @ %votesAgainst @ " " @ %totalVotes @ " " @ %votePassed;
}

function Notifications::aActionStartVote(%clientId, %topic, %action, %option) {
%numClients = getNumClients();
%center = $Server::MaxPlayers + 1;
%size = ($Server::MaxPlayers * 2) + 1;
%unitSize = %size / %numClients / 2 - 1;
for(%i = 0; %i <= %size; %i++) {
if(%i == %center)
%blob = %blob @ "<f0>*<f2>";
else
%blob = %blob @ " ";
}
%blob = "<f2>-"@%blob@"+";
bottomprintall("<jc>" @ %blob, 50);
}

function Notifications::DisplayVotes(%votesFor, %votesAgainst, %totalVotes, %votePassed) {
%numClients = getNumClients();
%center = $Server::MaxPlayers + 1;
%size = ($Server::MaxPlayers * 2) + 1;
%unitSize = floor((%size / %numClients) - 1);
echo(%unitSize);
for(%i = 0; %i <= %size + 1; %i++) {
if(%i == (%center + %unitSize)) {
%blob = %blob @ "<f0>*<f2>";
}
else
%blob = %blob @ " ";
}
%blob = "<f2>-"@%blob@"+";
bottomprintall("<jc>" @ %blob, 50);
}

    function Notifications::CountDown() {

        %curTimeLeft = ($Server::timeLimit * 60) + floor($missionStartTime) - floor(getSimTime());
        if(Game::IsTie() && $Game::LT::OvertimeEnabled)
            %ae = " [ Overtime Enabled ]";
        // if(Game::IsTie()) %ae = " [ Overtime Enabled ]";

        if ($Server::Half == 1) {
            %type = "First half";
            %ae = "";
        }
        else if ($Server::Half == 2) {
            %type = "Second half";
            %ae = "";
         }
         else {
            %type = "Match";
         }

        // notification 2 minutes or less
        if(%curTimeLeft % 60 == 0) {
            // minutes
            %min = %curTimeLeft / 60;
            if(%min > 1) {
                %m = "s";
            }

            // 2 minutes or less
            if(%min <= 2 && %min > 0) {
                MessageAll(1, %type @ " ends in " @ %min @ " minute" @ %m @ %t @ %ae @ ".~wmine_act.wav");
            }
        }
        else if(%curTimeLeft == 20) {
            //schedule("Game::CheckTimeLimit();", 5);
            schedule("Notifications::CountDown();", 5);
        }
        else if(%curTimeLeft <= 15 && %curTimeLeft > 0) {
            if(%curTimeLeft % 5 == 0)
                MessageAll(1, %type @ " ends in " @ %curTimeLeft @ " seconds" @ %ae @ ".~wmine_act.wav");

            if(%curTimeLeft % 2 == 0)
                MessageAll(0, "~wbutton5.wav");
            else // tock
                MessageAll(0, "~wbutton4.wav");

            schedule("Notifications::CountDown();", 1);
        }

        return;

    }