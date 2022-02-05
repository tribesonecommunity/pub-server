function displayMenuNonSelfSelMenu(%cl)
    {
    %rec = %cl.selClient;
    %recName = Client::getName(%rec);
    if(%cl.canBan)
    %kickMsg = "Kick or Ban ";
    else
    %kickMsg = "Kick ";

    buildNewMenu("Options", "nonselfselmenu", %cl);

    //addLine("Vote to admin " @ %recName, "vadmin " @ %rec, !%cl.canMakeAdmin, %cl);
    //addLine("Vote to kick " @ %recName, "vkick " @ %rec, !%cl.canKick, %cl);

    addLine(%kickMsg @ %recName, "kickban " @ %rec, %cl.canKick, %cl);
    addLine("Message " @ %recName, "message " @ %rec, %cl.canSendWarning, %cl);
    addLine("Change " @ %recName @ "'s team", "fteamchange " @ %rec, %cl.canChangePlyrTeam, %cl);
    addLine("Admin " @ %recName, "admin " @ %rec, %cl.canMakeAdmin, %cl);
    addLine("Strip " @ %recName, "stradmin " @ %rec, (%cl.canStripAdmin && %rec.adminLevel > 0), %cl);

    addLine("Observe " @ %recName, "observe " @ %rec, (%cl.observerMode == "observerOrbit"), %cl);

    addLine("UnMute " @ %recName, "unmute " @ %rec, %cl.muted[%rec], %cl);
    addLine("Mute " @ %recName, "mute " @ %rec, !%cl.muted[%rec], %cl);

    addLine("Global UnMute " @ %recName, "gunmute " @ %rec, (%cl.adminLevel >= $minAccessRequired::Mute) && %rec.globalMute && !%rec.megaMute, %cl);
    addLine("Global Mute " @ %recName, "gmute " @ %rec, (%cl.adminLevel >= $minAccessRequired::Mute) && !%rec.globalMute && !%rec.megaMute, %cl);

    addLine("MEGA UnMute " @ %recName, "munmute " @ %rec, (%cl.adminLevel >= $minAccessRequired::Mute) && %rec.megaMute, %cl);
    addLine("MEGA Mute " @ %recName, "mmute " @ %rec, (%cl.adminLevel >= $minAccessRequired::Mute) && !%rec.megaMute, %cl);
    }

    function processMenuNonSelfSelMenu(%cl, %selection)
    {
    %selection = getWord(%selection, 0);
    %vic = %cl.selClient;

    if(%selection == "message")
    {
    // exclusive to menu.nonself.cs
    displayMenuMessagePlayer(%cl, %vic);
    return;
    }
    else if(%selection == "admin")
    {
    // exclusive to menu.nonself.cs
    displayMenuBestowAdmin(%cl, %vic);
    return;
    }
    else if(%selection == "stradmin")
    {
    // exclusive to menu.nonself.cs
    displayMenuStripAdminship(%cl, %vic);
    return;
    }
    else if(%selection == "kickban")
    {
    // exclusive to menu.nonself.cs
    displayMenuBanPlayer(%cl, %vic);
    return;
    }
    else if(%selection == "fteamchange")
    {
    // exclusive to menu.nonself.cs
    displayMenuForceTeamChange(%cl, %vic);
    return;
    }
    else if(%selection == "vkick")
    {
    %vic.voteTarget = true;
    AActionstartVote(%cl, "kick " @ Client::getName(%vic), "kick", %vic);
    Game::menuRequest(%cl);
    }
    else if(%selection == "vadmin")
    {
    %vic.voteTarget = true;
    AActionstartVote(%cl, "admin " @ Client::getName(%vic), "admin", %vic);
    Game::menuRequest(%cl);
    }
    else if(%selection == "observe")
    {
    Observer::setTargetClient(%cl, %vic);
    return;
    }
    else if(%selection == "mute")
    %cl.muted[%vic] = true;
    else if(%selection == "unmute")
    %cl.muted[%vic] = "";
    else if ((%selection == "gmute") && (%cl.adminLevel > %vic.adminLevel))
    {
    if ($zadmin::pref::log::Mute)
    logEntry(%cl, "Global Muted", %vic);

    %vic.globalMute = true;
    }
    else if ((%selection == "mmute") && (%cl.adminLevel > %vic.adminLevel))
    {
    if ($zadmin::pref::log::Mute)
    logEntry(%cl, "MEGA Muted", %vic);

    %vic.globalMute = true;
    %vic.megaMute = true;
    }
    else if ((%selection == "gunmute") && (%cl.adminLevel > %vic.adminLevel))
    {
    if ($zadmin::pref::log::Mute)
    logEntry(%cl, "Global Un-Muted", %vic);

    %vic.globalMute = false;
    }
    else if ((%selection == "munmute") && (%cl.adminLevel > %vic.adminLevel))
    {
    if ($zadmin::pref::log::Mute)
    logEntry(%cl, "MEGA Un-Muted", %vic);

    %vic.megaMute = false;
    %vic.globalMute = false;
    }

    Game::menuRequest(%cl);
    }

    function displayMenuStripAdminship(%cl, %vic)
    {
    buildNewMenu("Strip Adminship", "stripAdminship", %cl);

    addLine("Strip " @ Client::getName(%vic), "strip " @ %vic, true, %cl);
    addLine("Cancel", "no", true, %cl);
    }

    function processMenuStripAdminship(%clientId, %opt)
    {
    %action = getWord(%opt, 0);
    %cl = getWord(%opt, 1);

    if(%action == "strip")
    {
    if (%clientId.adminLevel > %cl.adminLevel)
    {
    //%cl.adminLevel = getAdminLevel("Player");
    %cl.adminLevel = 0;
    awardAdminship(%cl);
    if ($zadmin::pref::log::AdminStrip) logEntry(%clientId, "Stripped Admin from", %cl);

    %cl.registeredName = "Stripped by " @ %clientId.registeredName;
    }
    else
    {
    if($zadmin::pref::log::AdminStrip) logEntry(%clientId, "tried to strip Admin from", %cl);
    Client::sendMessage(%clientId, $MSGTypeSystem, "You do not have the power to strip " @ Client::getName(%cl) @ ".");
    Client::sendMessage(%cl, $MSGTypeGame, Client::getName(%clientId) @ " tried to strip your adminship.");
    }
    }
    Game::menuRequest(%clientId);
    }

    function a(){}

    function displayMenuBestowAdmin(%cl, %vic)
    {
    buildNewMenu("Bestow Admin", "bestowAdmin", %cl);

    for (%i = 1; (%i < $accessLevel::Count) && (%i < %cl.adminLevel); %i++)
    {
    addLine($accessLevel::[%i] @ " " @ Client::getName(%vic), "admin" @ %i @ " " @ %vic, true, %cl);
    }

    addLine("Cancel ", "cancel " @ %vic, true, %cl);
    }

    function processMenuBestowAdmin(%clientId, %opt)
    {
    %action = getWord(%opt, 0);
    %cl = getWord(%opt, 1);
    %recipientMessage = "You are now an admin, courtesy of " @ Client::getName(%clientId);
    %adminMessage = "Sent to " @ Client::getName(%cl) @ ": " @ %recipientMessage;

    if (String::FindSubStr(%action, "admin") == 0)
    {
    %adminLevel = String::GetSubStr(%action, 5, 1);

    if ((%clientId.adminLevel > %adminLevel) && (%cl.adminLevel < %adminLevel))
    {
    %cl.adminLevel = %adminLevel;
    %cl.password = "NOPASSWORD";
    awardAdminship(%cl);

    if($zadmin::pref::log::Adminships) logEntry(%clientId, "Adminned", %cl);
    if(%cl != %clientId)
    {
    %adminabbrev = String::getSubStr($accessLevel::[%adminLevel], 0, 1) @ "A";
    %cl.registeredName = %adminabbrev @ "->" @ %clientId.registeredName;

    BottomPrint(%cl, "<jc>" @ %recipientMessage);
    BottomPrint(%clientId, "<jc>" @ %adminMessage);
    Client::sendMessage(%cl, $MSGTypeSystem, %recipientMessage);
    }
    }
    }

    Game::menuRequest(%clientId);
    }

    function displayMenuMessagePlayer(%cl, %recipient)
    {
    buildNewMenu("Message Player", "messagePlayer", %cl);

    addLine($zadmin::pref::warnings::msg[1], 1 @ " " @ %recipient, $zadmin::pref::warnings::text[1] != "", %cl);
    addLine($zadmin::pref::warnings::msg[2], 2 @ " " @ %recipient, $zadmin::pref::warnings::text[2] != "", %cl);
    addLine($zadmin::pref::warnings::msg[3], 3 @ " " @ %recipient, $zadmin::pref::warnings::text[3] != "", %cl);
    addLine($zadmin::pref::warnings::msg[4], 4 @ " " @ %recipient, $zadmin::pref::warnings::text[4] != "", %cl);
    addLine($zadmin::pref::warnings::msg[5], 5 @ " " @ %recipient, $zadmin::pref::warnings::text[5] != "", %cl);
    addLine($zadmin::pref::warnings::msg[6], 6 @ " " @ %recipient, $zadmin::pref::warnings::text[6] != "", %cl);
    addLine("Cancel", "cancel " @ %recipient, true, %cl);
    }

    function processMenuMessagePlayer(%cl, %opt)
    {
    %choice = getWord(%opt, 0);
    %selId = getWord(%opt, 1);

    if(%choice == "cancel")
    return;
    else
    {
    CenterPrint(%selId, "<jc>" @ $zadmin::pref::warnings::text[%choice]);
    BottomPrint(%cl, "<jc>(Sent to " @ Client::getName(%selId) @ ") " @ $zadmin::pref::warnings::text[%choice]);
    if ($zadmin::pref::log::Warnings) logEntry(%cl, "issued a " @ $zadmin::pref::warnings::msg[%choice] @ " to", %selId);
    }

    Game::menuRequest(%cl);
    }

    function displayMenuBanPlayer(%clientId, %vic)
    {
    buildNewMenu("Boot " @ Client::getName(%vic), "banPlayer", %clientId);

    addLine("Kick " @ Client::getName(%vic), "kick " @ %vic, %clientId.canKick, %clientId);
    addLine("Ban " @ Client::getName(%vic), "ban " @ %vic, %clientId.canBan, %clientId);
    addLine("PermBan " @ parseIP(%vic, 4, 18, true), "fullIP " @ %vic, %clientId.canPermanentBan, %clientId);
    addLine("PermBan " @ parseIP(%vic, 3, 14, true), "threeOctet " @ %vic, %clientId.canPermanentBan, %clientId);
    addLine("PermBan " @ parseIP(%vic, 2, 10, true), "twoOctet " @ %vic, %clientId.canPermanentBan, %clientId);
    addLine("Cancel ", "cancel " @ %vic, true, %clientId);
    }

    function processMenuBanPlayer(%clientId, %opt)
    {
    %action = getWord(%opt, 0);
    %vic = getWord(%opt, 1);

    if (%action == "cancel")
    {
    Game::menuRequest(%clientId);
    return;
    }

    buildNewMenu("Boot " @ Client::getName(%vic) @ ", you sure?", "banAffirm", %clientId);

    addLine("Kick " @ Client::getName(%vic), %opt @ " yes", %action == "kick", %clientId);
    addLine("Ban " @ Client::getName(%vic), %opt @ " yes", %action == "ban", %clientId);
    addLine("PermBan " @ parseIP(%vic, 4, 18, true), %opt @ " yes", %action == "fullIP", %clientId);
    addLine("PermBan " @ parseIP(%vic, 3, 14, true), %opt @ " yes", %action == "threeOctet", %clientId);
    addLine("PermBan " @ parseIP(%vic, 2, 10, true), %opt @ " yes", %action == "twoOctet", %clientId);
    addLine("Cancel ", %opt @ " cancel", true, %clientId);
    }

    function processMenuBanAffirm(%clientId, %opt)
    {
    %action = getWord(%opt, 0);
    %recipient = getWord(%opt, 1);
    %affirm = getWord(%opt, 2);

    if (%affirm == "yes")
    {
    if (%action == "kick")
    AActionkick(%clientId, getWord(%opt, 1), false);
    else if(%action == "ban")
    AActionkick(%clientId, getWord(%opt, 1), true);
    else if (%action == "fullIP")
    permaBan(%clientId, %recipient, 4, 18, false);
    else if (%action == "threeOctet")
    permaBan(%clientId, %recipient, 3, 14, false);
    else if (%action == "twoOctet")
    permaBan(%clientId, %recipient, 2, 10, false);
    }

    Game::menuRequest(%clientId);
    }

    function displayMenuForceTeamChange(%cl, %vic) {
        %cl.ptc = %vic;
        buildNewMenu("Force Team Change", "forceTeamChange", %cl);

        addLine("Observer", -2, true, %cl);

        if ($Game::MissionType == "Rabbit") {
            addLine(getTeamName(0), 0, true, %cl);
        }
        else {
            addLine("Automatic", -1, true, %cl);

            for(%i = 0; %i < getNumTeams(); %i++) {
                addLine(getTeamName(%i), %i, true, %cl);
            }
        }
    }

    function processMenuForceTeamChange(%clientId, %team)
    {
    if(%clientId.canChangePlyrTeam && %clientId.adminlevel >= %clientId.ptc.adminLevel)
    {
    processMenuChangeTeamsMenu(%clientId.ptc, %team, %clientId);
    //if($logTeamChanges) logEntry(%clientId, "Team Changed", %clientId.ptc);
    }
    %clientId.ptc = "";
    }
//echo("Executed Non Self Menu Options");