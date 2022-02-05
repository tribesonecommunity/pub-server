function displayMenuVoteMenu(%cl)
    {
    %rec = %cl.selClient;
    %recName = Client::getName(%rec);
    %tModeWaiting = $Server::TourneyMode && (!$CountdownStarted && !$matchStarted);
    //echo ("displayMenuVoteMenu: modeWaiting = " @ %tModeWaiting);

    buildNewMenu("Options", "votemenu", %cl);
    addLine("Change Teams/Observe", "changeteams", (!$loadingMission) && (!$matchStarted || !$Server::TourneyMode), %cl);
    addLine("Vote to change mission", "vChangeMission", true, %cl);
    //addLine("Vote to disable team damage", "vdtd", ($Server::TeamDamageScale == 1.0), %cl);
    //addLine("Vote to enable team damage", "vetd", !($Server::TeamDamageScale == 1.0), %cl);
    addLine("Vote to enter FFA mode", "vcffa", $Server::TourneyMode, %cl);
    addLine("Vote to start the match", "vsmatch", %tModeWaiting, %cl);
    //addLine("Vote to enter Tournament mode", "vctourney", !$Server::TourneyMode, %cl);
    addLine("Admin options...", "adminoptions", (%cl.adminLevel > 0), %cl);
    addLine("Player options...", "playerOptions", true, %cl);
    }

    function processMenuVoteMenu(%cl, %selection)
    {
    if(%selection == "changeteams")
    {
    displayMenuChangeTeamsMenu(%cl);
    return;
    }
    else if(%selection == "vsmatch")
    AActionstartVote(%cl, "start the match", "smatch", 0);
    else if(%selection == "vetd")
    AActionstartVote(%cl, "enable team damage", "etd", 0);
    else if(%selection == "vdtd")
    AActionstartVote(%cl, "disable team damage", "dtd", 0);
    else if(%selection == "etd")
    AActionsetTeamDamageEnable(%cl, true);
    else if(%selection == "dtd")
    AActionsetTeamDamageEnable(%cl, false);
    else if(%selection == "vcffa")
    AActionstartVote(%cl, "change to Free For All mode", "ffa", 0);
    else if(%selection == "vctourney")
    AActionstartVote(%cl, "change to Tournament mode", "tourney", 0);
    else if(%selection == "vChangeMission")
    {
    %cl.madeVote = true;
    displayMenuChangeMissionType(%cl, 0);
    return;
    }
    else if(%selection == "playerOptions")
    {
    displayMenuPlayerMenu(%cl);
    return;
    }
    else if(%selection == "adminoptions")
    {
    //no need to add, falls through to Game::menu request anyway
    }
    Game::menuRequest(%cl);
}

//echo("Executed Vote Menu Options");