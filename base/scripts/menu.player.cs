function displayMenuPlayerMenu(%cl) {
    %rec = %cl.selClient;
    %recName = Client::getName(%rec);
	if(%cl.customSkinDisabled == "")
		%cl.customSkinDisabled = true;

	buildNewMenu("Player Options", "playerOptionsMenu", %cl);

	// Using negatives, enabled by default
	addLine("Enable hit sounds (uses hit.ogg)", "ehs", %cl.hitSoundsDisabled, %cl);
	addLine("Disable hit sounds (uses hit.ogg)", "dhs", !%cl.hitSoundsDisabled, %cl);

	addLine("Enable Quake announcer", "eqa", %cl.quakeAnnouncerDisabled, %cl);
	addLine("Disable Quake announcer", "dqa", !%cl.quakeAnnouncerDisabled, %cl);
	addLine("Enable custom skin", "ecskin", %cl.customSkinDisabled, %cl);
	addLine("Disable custom skin", "dcskin", !%cl.customSkinDisabled, %cl);

}

function processMenuPlayerOptionsMenu(%cl, %sel) {

    if (%sel == "ehs") {
        %cl.hitSoundsDisabled = false;
    }
    else if (%sel == "dhs") {
        %cl.hitSoundsDisabled = true;
    }
    else if (%sel == "eqa") {
        %cl.quakeAnnouncerDisabled = false;
    }
    else if (%sel == "dqa") {
        %cl.quakeAnnouncerDisabled = true;
    }

    else if (%sel == "ecskin") {
        %cl.customSkinDisabled = false;
	Client::setSkin(%cl, $Client::info[%cl, 0]);
    }
    else if (%sel == "dcskin") {
        %cl.customSkinDisabled = true;
	Client::setSkin(%cl, $Server::teamSkin[Client::getTeam(%cl)]);
    }
    displayMenuPlayerMenu(%cl);

}


//echo("Executed Player Menu Options");


