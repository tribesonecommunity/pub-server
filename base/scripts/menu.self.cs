// Krayvok
// Stream cam 1.0

function displayMenuSelfSelMenu(%cl) {
    buildNewMenu("Self Serve Menu", "selfselmenu", %cl);
    addLine("Change Teams/Observe", "changeteams", (!$loadingMission) && (!$matchStarted || !$Server::TourneyMode), %cl);
    //addLine("Vote to admin yourself", "vadminself", !%cl.adminLevel, %cl);
}

function processMenuSelfSelMenu(%cl, %selection) {
    if(%selection == "changeteams"){
        displayMenuChangeTeamsMenu(%cl);
    }
    else if (%selection == "vadminself") {
        %cl.voteTarget = true;
        AActionstartVote(%cl, "admin " @ Client::getName(%cl), "admin", %cl);
        Game::menuRequest(%cl);
    }
}

//echo("Executed Self Menu Options");