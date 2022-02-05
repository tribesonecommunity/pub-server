function displayMenuVotePendingMenu(%cl) {
    buildNewMenu("Vote in progress", "votePendingMenu", %cl);

    addLine("Vote YES to " @ $curVoteTopic, "voteYes " @ $curVoteCount, %cl.vote == "", %cl);
    addLine("Vote No to " @ $curVoteTopic, "voteNo " @ $curVoteCount, %cl.vote == "", %cl);
    addLine("VETO Vote to " @ $curVoteTopic, "veto", %cl.canCancelVote, %cl);
    addLine("Admin Options...", "adminoptions", (%cl.adminLevel > 0), %cl);
    }

    function processMenuVotePendingMenu(%cl, %sel)
    {
    %selection = getWord(%sel, 0);
    if(%selection == "voteYes") // && %cl == $curVoteCount)  ************************
    {
    %cl.vote = "yes";
    centerprint(%cl, "", 0);
    }
    else if(%selection == "voteNo") // && %cl == $curVoteCount)  *************************
    {
    %cl.vote = "no";
    centerprint(%cl, "", 0);
    }
    else if(%selection == "veto")
    {
    messageAll(0, "Vote to " @ $curVoteTopic @ " was VETO'd by an Admin.");
    bottomPrintAll("",0);
    $curVoteTopic = "";
    aActionvoteFailed();
    }
    else if(%selection == "adminoptions")
    {
    displayMenuAdminMenu(%cl);
    return;
    }
    Game::menuRequest(%cl);
}

//echo("Executed Vote In Progress Menu Options");