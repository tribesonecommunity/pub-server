// This file will respresent all Krayvoks stand alone scripts, or script alterations.
// Each section will be broken up by a line of //s stretching from left to right.
// All scripts are written with apprecaite for the community that has been around 2/3rds of my life.

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Pickup Options Menu
// Various scripts to help pickups.

    Attachment::AddAfter("displayMenuAdminMenu", "Krayvok::displayMenuAdminMenu");
    Attachment::AddAfter("processMenuAdminMenu", "Krayvok::processMenuAdminMenu");

    function Krayvok::displayMenuAdminMenu(%cl) {

        addLine("Pickup options...", "pickupOptions", $Server::TourneyMode, %cl);

    }

    function displayMenuPickupMenu(%cl) {

        buildNewMenu("Pickup Options", "pickupOptionsMenu", %cl);
            addLine("Change Team Scores", "cts", true, %cl);
            addLine("Flip a coin", "cflipcoin", (%cl.canFlipCoin && $Server::TourneyMode), %cl);
            addLine("Change to FFA mode", "cffa", (%cl.canChangeGameMode && $Server::TourneyMode), %cl);
    }

    function displayChangeTeamScoreMenu(%cl) {

        buildNewMenu("Change Team Score", "changeTeamScoreMenu", %cl);
            addLine("Blood Eagle", "ctsbe", true, %cl);
            addLine("Diamond Sword", "ctsds", true, %cl);

    }

    function displayChangeBETeamScoreMenu(%cl) {

        buildNewMenu("Change BE Score", "changeBETeamScoreMenu", %cl);
            addLine("Set BE Score to 0", "cbe0", true, %cl);
            addLine("Set BE Score to 1", "cbe1", true, %cl);
            addLine("Set BE Score to 2", "cbe2", true, %cl);
            addLine("Set BE Score to 3", "cbe3", true, %cl);
            addLine("Set BE Score to 4", "cbe4", true, %cl);
            addLine("Set BE Score to 5", "cbe5", true, %cl);
            addLine("Set BE Score to 6", "cbe6", true, %cl);
            addLine("Set BE Score to 7", "cbe7", true, %cl);

    }

    function displayChangeDSTeamScoreMenu(%cl) {

        buildNewMenu("Change DS Score", "changeDSTeamScoreMenu", %cl);
            addLine("Set DS Score to 0", "cds0", true, %cl);
            addLine("Set DS Score to 1", "cds1", true, %cl);
            addLine("Set DS Score to 2", "cds2", true, %cl);
            addLine("Set DS Score to 3", "cds3", true, %cl);
            addLine("Set DS Score to 4", "cds4", true, %cl);
            addLine("Set DS Score to 5", "cds5", true, %cl);
            addLine("Set DS Score to 6", "cds6", true, %cl);
            addLine("Set DS Score to 7", "cds7", true, %cl);

    }

    function Krayvok::processMenuAdminMenu(%cl,%sel) {

        if(%sel == "pickupOptions") {
            displayMenuPickupMenu(%cl);
            return;
        }

    }

    function processMenuPickupOptionsMenu(%cl, %sel) {

        if (%sel == "cts") {
            displayChangeTeamScoreMenu(%cl);
            return;
        }
        else if(%sel == "cflipcoin") {
            Krayvok::flipCoin(%cl);
        }
        else if(%sel == "cffa") {
            AActionsetModeFFA(%cl);
        }

        displayMenuAdminMenu(%cl);

    }

    function processMenuChangeTeamScoreMenu(%cl, %sel) {

        if (%sel == "ctsbe") {
            displayChangeBETeamScoreMenu(%cl);
            return;
        }
        else if (%sel == "ctsds") {
            displayChangeDSTeamScoreMenu(%cl);
            return;
        }

        displayMenuAdminMenu(%cl);
    }

    function processMenuChangeBETeamScoreMenu(%cl, %sel) {

        if (%sel == "cbe0") {
            Krayvok::changeScore(0,0);
        }
        else if (%sel == "cbe1") {
            Krayvok::changeScore(0,1);
        }
        else if (%sel == "cbe2") {
            Krayvok::changeScore(0,2);
        }
        else if (%sel == "cbe3") {
            Krayvok::changeScore(0,3);
        }
        else if (%sel == "cbe4") {
            Krayvok::changeScore(0,4);
        }
        else if (%sel == "cbe5") {
            Krayvok::changeScore(0,5);
        }
        else if (%sel == "cbe6") {
            Krayvok::changeScore(0,6);
        }
        else if (%sel == "cbe7") {
            Krayvok::changeScore(0,7);
        }

        return

        displayChangeTeamScoreMenu(%cl);

    }

    function processMenuChangeDSTeamScoreMenu(%cl, %sel) {

        if (%sel == "cds0") {
            Krayvok::changeScore(1,0);
        }
        else if (%sel == "cds1") {
            Krayvok::changeScore(1,1);
        }
        else if (%sel == "cds2") {
            Krayvok::changeScore(1,2);
        }
        else if (%sel == "cds3") {
            Krayvok::changeScore(1,3);
        }
        else if (%sel == "cds4") {
            Krayvok::changeScore(1,4);
        }
        else if (%sel == "cds5") {
            Krayvok::changeScore(1,5);
        }
        else if (%sel == "cds6") {
            Krayvok::changeScore(1,6);
        }
        else if (%sel == "cds7") {
            Krayvok::changeScore(1,7);
        }

        return

        displayChangeTeamScoreMenu(%cl);

    }

    function Krayvok::changeScore(%team,%score) {

        if (%team == 1) {
            %teamName = "Diamond Sword";
        }
        else if (%team == 0) {
            %teamName = "Blood Eagle";
        }

        %playerName = escapeString(Client::getName(%clientId));

        MessageAll(1, %teamName@" score was set to "@%score@" ~waccess_denied.wav");

        ObjectiveMission::refreshTeamScores();

        $teamScore[%team] = %score;

        Game::UpdateClientScores();
        Game::refreshClientScore(%cl);

    }

    function Krayvok::flipCoin(%cl) {
        %result = "TAILS";
        if (floor(getRandom() * 2) >= 1)
        %result = "HEADS";

        messageAll(0, Client::getName(%cl) @ " flipped a coin and it came up " @ %result @ ".");

        return;
    }
