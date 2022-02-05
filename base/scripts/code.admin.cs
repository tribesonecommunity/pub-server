    exec("code.prefs.cs");
    zAdmin::InitPrefs();

    exec("zadmin.missionlist.cs");
    exec("code.log.cs");
    exec("code.iplog.cs");
    exec("code.menu.cs");
    exec("code.config.cs");

    function zAdminInit() {

        // Max Menu Size 0-8
        $maxMenuSize = 7;

        %serverName = "";

        //remove spaces and illegal characters
        for (%i = 0; %i < getLength($Server::HostName); %i++) {
            %char = String::getSubStr($Server::hostName, %i, 1);
            %result = String::iCompare(%char, "z");

            if((%result >= -42 && %result <= -33) || (%result >= -25 && %result <= 0)) {
                %serverName = %serverName @ %char;
            }
        }

        %suffix = zadmin::getFileTimeStamp();

        $zAdminLogFile = "zadmin.admin.log";
        $zAdminBanLogFile = "zadmin.banlog.cs";
        $zAdminBanExclusionsFile = "zadmin.banexclusions.cs";
        $zadminCheatLogFile = "zadmin.cheat.log";
        $curVoteTopic = "";
        $curVoteAction = "";
        $curVoteOption = "";
        $curVoteCount = 0;

    }

    function awardAdminship(%client) {
        %client.canKick                 = (%client.adminLevel >= $minAccessRequired::kick);
        %client.canBan                  = (%client.adminLevel >= $minAccessRequired::ban);
        %client.canChangeMission        = (%client.adminLevel >= $minAccessRequired::changeMission);
        %client.canSetPassword          = (%client.adminLevel >= $minAccessRequired::setPassword);
        %client.canChangeTimeLimit      = (%client.adminLevel >= $minAccessRequired::changeTimeLimit);
        %client.cansetTeamInfo          = (%client.adminLevel >= $minAccessRequired::setTeamInfo);
        %client.canChangeGameMode       = (%client.adminLevel >= $minAccessRequired::changeGameMode);
        %client.canChangePlyrTeam       = (%client.adminLevel >= $minAccessRequired::changePlyrTeam);
        %client.canForceMatchStart      = (%client.adminLevel >= $minAccessRequired::forceMatchStart);
        %client.canSwitchTeamDamage     = (%client.adminLevel >= $minAccessRequired::switchTeamDamage);
        %client.canMakeAdmin            = (%client.adminLevel >= $minAccessRequired::makeAdmin);
        %client.canMakeGadmin           = (%client.adminLevel >= $minAccessRequired::makeGadmin);
        %client.canMakeSadmin           = (%client.adminLevel >= $minAccessRequired::makeSadmin);
        %client.canResetServer          = (%client.adminLevel >= $minAccessRequired::resetServer);
        %client.canSeePlayerSpecs       = (%client.adminLevel >= $minAccessRequired::seePlayerSpecs);
        %client.canSendWarning          = (%client.adminLevel >= $minAccessRequired::sendWarning);
        %client.canAnnounceTakeover     = (%client.adminLevel >= $minAccessRequired::announceTakeover);
        %client.canStripAdmin           = (%client.adminLevel >= $minAccessRequired::stripAdmin);
        %client.canReceiveAlerts        = (%client.adminLevel >= $minAccessRequired::receiveAlerts);
        %client.canPermanentBan         = (%client.adminLevel >= $minAccessRequired::permanentBan);
        %client.canCancelVote           = (%client.adminLevel >= $minAccessRequired::cancelVote);
        %client.canSendPrivateMsgs      = (%client.adminLevel >= $minAccessRequired::sendPrivateMsgs);
        %client.canSeePlayerlist        = (%client.adminLevel >= $minAccessRequired::seePlayerList);
        %client.canAntiRape             = (%client.adminLevel >= $minAccessRequired::antiRape);
        %client.canAntiRepair           = (%client.adminLevel >= $minAccessRequired::antiRepair);
        %client.canPickup               = (%client.adminLevel >= $minAccessRequired::pickupMode);
        %client.canPause                = (%client.adminLevel >= $minAccessRequired::pause);
        %client.canFlipCoin             = (%client.adminLevel >= $minAccessRequired::flipCoin);
        %client.canFreeze               = (%client.adminLevel >= $minAccessRequired::freeze);
        %client.canDisableHitSounds     = (%client.adminLevel >= $minAccessRequired::hitSounds);
        %client.canWeakChaingun         = (%client.adminLevel >= $minAccessRequired::weakChaingun);
        %client.canEnableLasthope       = (%client.adminLevel >= $minAccessRequired::enableLasthope);
        %client.isAdmin                 = %client.adminLevel > 0;
    }

    function remoteSetPassword(%client, %password) {
        if(%client.canSetPassword) {

            $Server::Password = %password;

            if($zadmin::pref::log::PasswordChanges) {
                logEntry(%client, "changed the password to" @ %password, "");
            }

        }
    }

    function remoteSetTimeLimit(%client, %time) {
        %time = floor(%time);

        if(%time == $Server::timeLimit || (%time != 0 && %time < 1))
            return;

        if(%client.canChangeTimeLimit) {
            if($zadmin::pref::log::TimeChanges) logEntry(%client, "changed time limit to " @ %time, "");
                $Server::timeLimit = %time;
            if(%time)
                messageAll(1, Client::getName(%client) @ " changed the time limit to " @ %time @ " minute(s). ~wmine_act.wav");
            else
                messageAll(1, Client::getName(%client) @ " disabled the time limit.~wmine_act.wav");
        }
    }

    function remoteSetTeamInfo(%client, %team, %teamName, %skinBase) {
        if(%team >= 0 && %team < 8 && %client.canSetTeamInfo) {
            //if($logNameSkinChanges) logEntry(%client, "set team " @ %team @ " name to " @ %teamName @ " and skin to " @ %skinBase, "");
            $Server::teamName[%team] = %teamName;
            $Server::teamSkin[%team] = %skinBase;
            messageAll(0, "Team " @ %team @ " is now \"" @ %teamName @ "\" with skin: " @ %skinBase @ " courtesy of " @ Client::getName(%client) @ ".  Changes will take effect next mission.");
        }
    }

    function remoteSelectClient(%clientId, %selId) {

        %clientId.tries++;

        if(%clientId.tries > 10) {

            if(%clientId.gone) {
                return;
            }

            %name = client::getName(%clientId);
            Log::Exploit(%clientId, "Server-Crash", "remoteSelectClient");
            banlist::add(client::getTransportAddress(%clientId), 999);
            kick(%clientId, "You Were Kicked For Spamming remoteSelectClient");
            %clientId.gone = true;
            return;
        }

        schedule(%clientId@".tries = 0;", 0.5);

        if(%clientId.selClient != %selId) {

            %clientId.selClient = %selId;
            Game::menuRequest(%clientId);

            if (%selId.registeredName == "")
                %selId.registeredName = "Unknown";
            if(!%selId.adminLevel)
                %selId.adminLevel = 0;

            if(%clientId.canSeePlayerSpecs) {
                if(%clientId.canSendPrivateMsgs)
                    remoteEval(%clientId, "setInfoLine", 1, "**PVT MESSAGING ACTIVE**");
                else
                    remoteEval(%clientId, "setInfoLine", 1, "Player Info for " @ Client::getName(%selId) @ ":");
                    remoteEval(%clientId, "setInfoLine", 2, "Admin Status: " @ $accessLevel::[%selId.adminLevel]);
                    remoteEval(%clientId, "setInfoLine", 3, "Name: " @ %selId.registeredName);
                    remoteEval(%clientId, "setInfoLine", 4, "IP: " @ Client::getTransportAddress(%selId));
                    remoteEval(%clientId, "setInfoLine", 5, "Version: " @ %selId.client @ ", " @ %selId.LHinit);

                if(%clientId.canSendPrivateMsgs  && %clientId == %selId) {
                    //remoteEval(%clientId, "setInfoLine", 5, "");
                    remoteEval(%clientId, "setInfoLine", 6, "CHAT now Broadcasts message.");
                }
                else if(%clientId.canSendPrivateMsgs  && %clientId != %selId) {
                    //remoteEval(%clientId, "setInfoLine", 5, "");
                    remoteEval(%clientId, "setInfoLine", 6, "CHAT now /pm's " @ Client::getName(%selId));
                }
            }
            else {
                remoteEval(%clientId, "setInfoLine", 1, "Player Info for " @ Client::getName(%selId) @ ":");
                remoteEval(%clientId, "setInfoLine", 2, "Real Name: " @ $Client::info[%selId, 1]);
                remoteEval(%clientId, "setInfoLine", 3, "Email Addr: " @ $Client::info[%selId, 2]);
                remoteEval(%clientId, "setInfoLine", 4, "Tribe: " @ $Client::info[%selId, 3]);
                remoteEval(%clientId, "setInfoLine", 5, "URL: " @ $Client::info[%selId, 4]);
                remoteEval(%clientId, "setInfoLine", 5, "Version: " @ %selId.client);
                //remoteEval(%clientId, "setInfoLine", 6, "Other: " @ $Client::info[%selId, 5]);
            }

        }

    }


    function remoteVoteYes(%clientId) {
        %clientId.vote = "yes";
        centerprint(%clientId, "", 0);
    }

    function remoteVoteNo(%clientId) {
        %clientId.vote = "no";
        centerprint(%clientId, "", 0);
    }

    function a(){}

    function buildNewMenu(%displayName, %menuHandle, %cl) {
        Client::buildMenu(%cl, %displayName, %menuHandle, true);
        %cl.menuLine = 0;
    }

    function addLine(%item, %itemResult, %condition, %cl) {
        if(%condition) {
            Client::addMenuItem(%cl, %cl.menuLine++ @ %item, %itemResult);
        }
    }

    function Game::menuRequest(%cl) {
        %cl.tries++;
        if(%cl.tries > 10) {
            if(%cl.gone) {
                return;
            }

            %name = client::getName(%cl);
            Log::Exploit(%cl, "Server-Crash", "remoteScoresOn");
            banlist::add(client::getTransportAddress(%cl), 999);
            kick(%cl, "You Were Kicked For Spamming remoteScoresOn");
            %cl.gone = true;

            return;
        }
        schedule(%cl@".tries = 0;", 0.5);


        if(%cl.selClient && %cl.selClient != %cl) {
            displayMenuNonSelfSelMenu(%cl);
        }
        else if(%cl.selClient == %cl) {
            displayMenuSelfSelMenu(%cl);
        }
        else if($curVoteTopic != "" && (%cl.vote == "" || %cl.canCancelVote)) {
            displayMenuVotePendingMenu(%cl);
        }
        else if(%cl.adminLevel) {
            displayMenuAdminMenu(%cl);
        }
        else {
            displayMenuVoteMenu(%cl);
        }
    }

    exec("menu.admin.cs");
    exec("menu.vote.cs");
    exec("menu.player.cs");
    exec("menu.server.cs");
    exec("menu.votepending.cs");
    exec("menu.self.cs");
    exec("menu.nonself.cs");
    exec("menu.pickup.cs");
//    exec("stream.cam.cs");

    function displayMenuChangeTeamsMenu(%cl, %opt) {

        buildNewMenu("Change Teams", "changeTeamsMenu", %cl);
            addLine("Observer", -2, true, %cl);
            addLine("Automatic", -1, true, %cl);

        for(%i = 0; %i < getNumTeams(); %i++) {
            addLine(getTeamName(%i), %i, true, %cl);
        }
    }

    function displayMenuChangeMissionType(%clientId, %listStart) {

        buildNewMenu("Pick Mission Type", "changeMissionType", %clientId);

        for (%mTypeIndex = %listStart; %mTypeIndex < $MLIST::TypeCount; %mTypeIndex++) {
            if (%lineNum++ > $maxMenuSize) {
                addLine("More mission types...", "moreTypes " @ %mTypeIndex, true, %clientId);
                break;
            }
            else if ($MLIST::Type[%mTypeIndex] != "Training") {
                addLine($MLIST::Type[%mTypeIndex], %mTypeIndex @ " 0", true, %clientId);
            }
        }
    }

    function processMenuChangeMissionType(%clientId, %option) {
        %type = getWord(%option, 0);
        %index = getWord(%option, 1);

        if (%type == "moreTypes") {
            displayMenuChangeMissionType(%clientId, %index);
        }
        else {

            buildNewMenu("Change Mission", "changeMission", %clientId);
            for(%i = 0; (%misIndex = getWord($MLIST::MissionList[%type], %index + %i)) != -1; %i++) {
                if ((%i + 1) > $maxMenuSize) {
                    addLine("More missions...", "more " @ %index + %i @ " " @ %type, true, %clientId);
                    break;
                }
                addLine($MLIST::EName[%misIndex], %misIndex @ " " @ %type, true, %clientId);
            }
        }
    }

    function processMenuChangeMission(%clientId, %option) {

        if(getWord(%option, 0) == "more") {
            %first = getWord(%option, 1);
            %type = getWord(%option, 2);
            processMenuChangeMissionType(%clientId, %type @ " " @ %first);
            return;
        }

        %mi = getWord(%option, 0);
        %mt = getWord(%option, 1);

        %misName = $MLIST::EName[%mi];
        %misType = $MLIST::Type[%mt];

        // verify that this is a valid mission:
        if(%misType == "" || %misType == "Training") {
            return;
        }

        for(%i = 0; true; %i++) {
            %misIndex = getWord($MLIST::MissionList[%mt], %i);
            if(%misIndex == %mi)
                break;
                if(%misIndex == -1)
                return;
        }

        if(%clientId.canChangeMission && !%clientId.madeVote) {
            if($zadmin::pref::log::MissionChanges) {
                logEntry(%clientId, "changed mission to " @ %misName, "");
            }

            messageAll(0, Client::getName(%clientId) @ " changed the mission to " @ %misName @ " (" @ %misType @ ")");
            Vote::changeMission();
            Server::loadMission(%misName);
        }
        else {
            %clientId.madeVote = "";
            aActionStartVote(%clientId, "change the mission to " @ %misName @ " (" @ %misType @ ")", "changeMission", %misName);
            Game::menuRequest(%clientId);
        }

    }

    function a(){}

    function aActioncountVotes(%curVote) {

        if(%curVote != $curVoteCount) {
            return;
        }
        if($curVoteTopic == "") {
            return;
        }

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
            else {
                %votesAbstain++;
            }
        }

        %minVotes = floor($Server::MinVotesPct * %totalClients);

        if(%minVotes < $Server::MinVotes) {
            %minVotes = $Server::MinVotes;
        }

        if(%totalVotes < %minVotes) {
            %votesAgainst += %minVotes - %totalVotes;
            %totalVotes = %minVotes;
        }

        %margin = $Server::VoteWinMargin;

        if($curVoteAction == "admin") {
            %margin = $Server::VoteAdminWinMargin;
            %totalVotes = %votesFor + %votesAgainst + %votesAbstain;
            if(%totalVotes < %minVotes) {
                %totalVotes = %minVotes;
            }
        }

        if(%votesFor / %totalVotes >= %margin) {
            messageAll(0, "Vote to " @ $curVoteTopic @ " passed: " @ %votesFor @ " to " @ %votesAgainst @ " with " @ %totalClients - (%votesFor + %votesAgainst) @ " abstentions.");
            aActionvoteSucceded();
        }
        else {
            // special team kick option:
            if($curVoteAction == "kick") {
                // check if the team did a majority number on him:
                %votesFor = 0;
                %totalVotes = 0;
                for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)){
                    if(GameBase::getTeam(%cl) == $curVoteOption.kickTeam){
                        %totalVotes++;
                        if(%cl.vote == "yes") {
                            %votesFor++;
                        }
                    }
                }

                if(%totalVotes >= $Server::MinVotes && %votesFor / %totalVotes >= $Server::VoteWinMargin){
                    messageAll(0, "Vote to " @ $curVoteTopic @ " passed: " @ %votesFor @ " to " @ %totalVotes - %votesFor @ ".");
                    aActionvoteSucceded();
                    $curVoteTopic = "";
                    return;
                }
            }

            messageAll(0, "Vote to " @ $curVoteTopic @ " did not pass: " @ %votesFor @ " to " @ %votesAgainst @ " with " @ %totalClients - (%votesFor + %votesAgainst) @ " abstentions.");
            aActionvoteFailed();

        }

        $curVoteTopic = "";
    }

    function aActionStartVote(%clientId, %topic, %action, %option)
    {
    if(%clientId.lastVoteTime == "")
    %clientId.lastVoteTime = -$Server::MinVoteTime;

    // we want an absolute time here.
    %time = getIntegerTime(true) >> 5;
    %diff = %clientId.lastVoteTime + $Server::MinVoteTime - %time;

    if(%diff > 0)
    {
    Client::sendMessage(%clientId, 0, "You can't start another vote for " @ floor(%diff) @ " seconds.");
    return;
    }
    if($curVoteTopic == "")
    {

    if ($dedicated)
    echo("VOTE INITIATED: " @ Client::getName(%clientId) @ " initiated a vote to " @ %topic);

    if(%clientId.numFailedVotes)
    %time += %clientId.numFailedVotes * $Server::VoteFailTime;

    %clientId.lastVoteTime = %time;
    $curVoteInitiator = %clientId;
    $curVoteTopic = %topic;
    $curVoteAction = %action;
    $curVoteOption = %option;
    if(%action == "kick")
    $curVoteOption.kickTeam = GameBase::getTeam($curVoteOption);
    $curVoteCount++;
    bottomprintall("<jc><f1>" @ Client::getName(%clientId) @ " <f0>initiated a vote to <f1>" @ $curVoteTopic, 10);
    for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
    %cl.vote = "";
    %clientId.vote = "yes";
    for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
    if(%cl.menuMode == "options")
    Game::menuRequest(%clientId);
    schedule("aActioncountVotes(" @ $curVoteCount @ ", true);", $Server::VotingTime, 35);
    }
    else
    {
    Client::sendMessage(%clientId, 0, "Voting already in progress.");
    }
    }

    function aActionstartMatch(%admin)
    {
    if(%admin == -1 || %admin.canForceMatchStart)
    {
    if(!$CountdownStarted && !$matchStarted)
    {
    if(%admin == -1)
    messageAll(0, "Match start countdown forced by vote.");
    else
    messageAll(0, "Match start countdown forced by " @ Client::getName(%admin));
    Game::ForceTourneyMatchStart();
    }
    }
    }

    function aActionsetTeamDamageEnable(%admin, %enabled)
    {
    if(%admin == -1 || %admin.canSwitchTeamDamage)
    {
    if(%enabled)
    {
    $Server::TeamDamageScale = 1;
    if(%admin == -1)
    messageAll(0, "Team damage set to ENABLED by consensus.");
    else
    {
    messageAll(0, Client::getName(%admin) @ " ENABLED team damage.");
    if($zadmin::pref::log::TeamDamage) logEntry(%admin, "enabled Team Damage", "");
    }
    }
    else
    {
    $Server::TeamDamageScale = 0;
    if(%admin == -1)
    messageAll(0, "Team damage set to DISABLED by consensus.");
    else
    {
    messageAll(0, Client::getName(%admin) @ " DISABLED team damage.");
    if($zadmin::pref::log::TeamDamage) logEntry(%admin, "disabled Team Damage", "");
    }
    }
    }
    }

    function aActionkick(%admin, %client, %ban)
    {
    if(%admin != %client && (%admin == -1 || %admin.adminLevel))
    {
    if(%ban && !%admin.canBan)
    return;

    if(%ban)
    {
    %word = "banned";
    %cmd = "BAN: ";
    %desc = " ban ";
    }
    else
    {
    %word = "kicked";
    %cmd = "KICK: ";
    %desc = " kick ";
    }

    if(%client.adminLevel > 0)
    {
    if(%admin == -1 && %client.adminLevel > getAdminLevel("Public Admin")) //only voted admins can be kicked by vote
    {
    messageAll(0, Client::getName(%client) @ "is an admin and can't be " @ %word @ " by vote.");
    return;
    }
    else if (%admin.adminLevel <= %client.adminLevel) //you must be higher level than the other admin to kick/ban him
    {
    Client::sendMessage(%admin, $MSGTypeSystem, "You do not have the power to" @ %desc @ Client::getName(%client)@".");
    Client::sendMessage(%client, $MSGTypeGame, Client::getName(%admin) @ " just tried to" @ %desc @ "you.");
    if($zadmin::pref::log::KickBan) logEntry(%admin, "attempted to" @ %desc, %client);
    return;
    }
    }

    %ip = Client::getTransportAddress(%client);

    //echo(%cmd @ %admin @ " " @ %client @ " " @ %ip);

    if(%ip == "")
    return;
    if(%ban)
    BanList::add(%ip, $zadmin::pref::time::ban);
    else
    BanList::add(%ip, $zadmin::pref::time::Kick);

    %name = Client::getName(%client);

    if ($zadmin::pref::log::KickBan && %word == "kicked") logEntry(%admin, %word, %client);
    if ($zadmin::pref::log::KickBan && %word == "banned") logEntry(%admin, %word, %client, "@");

    if(%admin == -1)
    {
    MessageAll(0, %name @ " was " @ %word @ " from vote.");
    Net::kick(%client, "You were " @ %word @ " by  consensus.");
    }
    else
    {
    MessageAll(0, %name @ " was " @ %word @ " by " @ Client::getName(%admin) @ ".");
    Net::kick(%client, "You were " @ %word @ " by " @ Client::getName(%admin));
    }
    }
    }

    // Krayvoks New fix to change time limit back to 25 if not in tounry mode (aka going into FFA)
    function aActionsetModeFFA(%clientId) {
        if($Server::TourneyMode && (%clientId == -1 || %clientId.canChangeGameMode)) {
            //$Server::TeamDamageScale = 0;
            if(%clientId == -1) {
                messageAll(0, "Server switched to Free-For-All Mode.");
                $Server::timeLimit = 25;
            }
            else {
                messageAll(1, "Server switched to Free-For-All Mode by " @ Client::getName(%clientId) @ ".~wmine_act.wav");
                $Server::timeLimit = 25;
                if($zadmin::pref::log::GameModeChanges) {
                    logEntry(%clientId, "switched to FFA Mode.", "");
                }
            }
            $Server::TourneyMode = false;
            $Server::BalancedMode = false;
            $Server::Half = -1;
            centerprintall(); // clear the messages
            if(!$matchStarted && !$countdownStarted) {
                if($Server::warmupTime) {
                    Server::Countdown($Server::warmupTime);
                }
                else {
                    Game::startMatch();
                }
            }
        }
    }
    
    // Orignal method prior to my update.
    //function aActionsetModeFFA(%clientId)
    //{
    //if($Server::TourneyMode && (%clientId == -1 || %clientId.canChangeGameMode))
    //{
    ////$Server::TeamDamageScale = 0;
    //if(%clientId == -1)
    //messageAll(0, "Server switched to Free-For-All Mode.");
    //else
    //{
    //messageAll(1, "Server switched to Free-For-All Mode by " @ Client::getName(%clientId) @ ".~wmine_act.wav");
    //if($zadmin::pref::log::GameModeChanges) logEntry(%clientId, "switched to FFA Mode.", "");
    //}
    //$Server::TourneyMode = false;
    //$Server::BalancedMode = false;
    //$Server::Half = -1;
    //centerprintall(); // clear the messages
    //if(!$matchStarted && !$countdownStarted)
    //{
    //if($Server::warmupTime)
    //Server::Countdown($Server::warmupTime);
    //else
    //Game::startMatch();
    //}
    //}
    //}

    function aActionsetModeTourney(%clientId)
    {
    if(!$Server::TourneyMode && (%clientId == -1 || %clientId.canChangeGameMode))
    {
    $Server::TeamDamageScale = 1;
    %mode = "";

    if(%clientId == -1)
    messageAll(0, "Server switched to Tournament Mode.");
    else
    {
    messageAll(1, "Server switched to Tournament Mode by " @ Client::getName(%clientId) @ ".~wmine_act.wav");
    if($zadmin::pref::log::GameModeChanges)
    logEntry(%clientId, "switched to Tournament Mode.", "");
    }

    $Server::TourneyMode = true;
    Server::nextMission();
    }
    }
    function aActionvoteFailed()
    {
    $curVoteInitiator.numVotesFailed++;

    if($curVoteAction == "kick" || $curVoteAction == "admin")
    $curVoteOption.voteTarget = "";
    }

    function aActionvoteSucceded()
    {
    $curVoteInitiator.numVotesFailed = "";
    if($curVoteAction == "kick")
    {
    if($curVoteOption.voteTarget)
    aActionkick(-1, $curVoteOption);
    }
    else if($curVoteAction == "admin")
    {
    if($curVoteOption.voteTarget)
    {
    if($zadmin::pref::log::Adminships) logEntry(-1, "adminned", $curVoteOption);
    $curVoteOption.adminLevel = getAdminLevel("Public Admin");
    $curVoteOption.registeredName = "Admin by vote";
    awardAdminship($curVoteOption);

    messageAll(0, Client::getName($curVoteOption) @ " has become an administrator.");
    if($curVoteOption.menuMode == "options")
    Game::menuRequest($curVoteOption);
    }
    $curVoteOption.voteTarget = false;
    }
    else if($curVoteAction == "changeMission")
    {
    messageAll(0, "Changing to mission " @ $curVoteOption @ ".");
    Vote::changeMission();
    Server::loadMission($curVoteOption);
    }
    else if($curVoteAction == "tourney")
    aActionsetModeTourney(-1,false);
    else if($curVoteAction == "ffa")
    aActionsetModeFFA(-1);
    else if($curVoteAction == "etd")
    aActionsetTeamDamageEnable(-1, true);
    else if($curVoteAction == "dtd")
    aActionsetTeamDamageEnable(-1, false);
    else if($curVoteOption == "smatch")
    aActionstartMatch(-1);
    }

    function remoteAdminPassword(%client, %password) {
        %client.tries++;

        if(%client.tries > 5) {

            if(%client.gone) {
                return;
            }

            %name = client::getName(%client);
            Admin::Exploit(%client, "SAD() Password Spam");
            banlist::add(client::getTransportAddress(%client), 300);
            messageall(0, %name@" Was Kicked For Spamming Admin Passwords");
            kick(%client, "You Were Kicked For Spamming Admin Passwords");
            %client.gone = true;
            return;
        }

        schedule(%client@".tries = 0;", 0.5);

        %oldLevel = %client.adminLevel;

        if ($zadmin::admins[%password] != "") {
            %client.adminLevel = $zadmin::admins[%password, level];
            %client.registeredName = $zadmin::admins[%password, name];
            Client::sendMessage(%client, $MSGTypeCommand, "Success! You have successfully logged in as an admin.");
        }
        else {
            %client.registeredName = "";
            %client.adminLevel = 0;
            awardAdminship(%client);
            Client::sendMessage(%client, $MSGTypeCommand, "You have failed to login as an admin. Keep it up and you'll be kicked!");
            return;
        }

    %client.password =  %password;
    schedule("testAdminDuplication(" @ %client @ ");", 5);  //wait 5 seconds so we don't override the "has logged in" message sent to Uadmins.
    awardAdminShip(%client);

    if (%client.canSeePlayerlist)
    LP(%client); //spam client's console with player info

    if (%oldLevel != %client.adminLevel) //allow admin to relogin to see LP list without broadcasting alert or logging
    {
    inGameAlert(Client::getName(%client) @ " has logged in as " @ $accessLevel::[%client.adminLevel] @ " using " @ %client.registeredName @ "\'s password.");
    if ($zadmin::pref::log::AdminLogins)
    logEntry(%client, "activated his/her " @ $accessLevel::[%client.adminLevel] @ " account.", "", "+");
    }
    }

    function testAdminDuplication(%cl)
    {
    %numClients = getNumClients();
    %violatorIndex = 0;
    %violatorList[%violatorIndex] = %cl;
    for (%clientIndex = 0; %clientIndex < %numClients; %clientIndex++)
    {
    %otherClient = getClientByIndex(%clientIndex);
    if (%cl != %otherClient && %cl.password == %otherClient.password)
    {
    %duplicate = true;
    %violatorList[%violatorIndex++] = %otherClient;
    }
    }
    if(%duplicate)
    {
    $Alert = %cl.registeredName @ "\'s password is in use by : " @ Client::getName(%cl);
    for (%vio = 1; %vio <= %violatorIndex; %vio++)
    $Alert = $Alert @ " & " @ Client::getName(%violatorList[%vio]);
    export("Alert", "config\\" @ $zAdminLogFile, true);
    inGameAlert($alert);
    }
    }

    function inGameAlert(%message)
    {
    %numClients = getNumClients();
    for(%adminIndex = 0; %adminIndex < %numClients; %adminIndex++)
    {
    %admin = getClientByIndex(%adminIndex);
    if(%admin.canReceiveAlerts)
    BottomPrint(%admin, "<jc>" @ %message);
    }
    }

    function permaBan(%admin, %bannedClient, %numWords, %stringSize)
    {
    if(!%admin.canPermanentBan)
    return;
    if (%admin.adminLevel <= %bannedClient.adminLevel) //you must be higher level than the other admin to kick/ban him
    {
    Client::sendMessage(%admin, $MSGTypeSystem, "You do not have the power to ban " @ Client::getName(%bannedClient) @ ".");
    Client::sendMessage(%bannedClient, $MSGTypeGame, Client::getName(%admin) @ " just tried to ban you.");
    if($zadmin::pref::log::KickBan) logEntry(%admin, "attempted to ban ", %bannedClient);
    return;
    }

    %word = 0;
    %charIndex = 0;

    %ip = Client::getTransportAddress(%bannedClient);

    if (String::findSubStr(%ip, "IPX") != -1 || String::findSubStr(%ip, "LOOPBACK") != -1)
    return; //don't deal with IPX or Loopbacks right now.

    %truncatedIP = parseIP(%bannedClient, %numWords, %stringSize, false);

    $IPBan[$IPBanCount++] = format(%truncatedIP, 20) @ format (%ip, 26) @ Client::getName(%bannedClient) @ " permanently banned by " @ %admin.registeredName @ ".";

    logEntry(%admin, "permanently banned", %bannedClient, "@");
    export("IPBan" @ $IPBanCount, "config\\" @ $zAdminBanLogFile, true);
    MessageAll(1, Client::getName(%bannedClient) @ " was banned by " @ Client::getName(%admin));

    Net::kick(%bannedClient, $permaBanMessage);
    BanList::addAbsolute();
    BanList::add(%ip, $zadmin::pref::time::ban);
    }

    function parseIP(%clientId, %numWords, %stringSize, %fillEmptySlots)
    {
    %ip = Client::getTransportAddress(%clientId);

    %word = 0;
    %charIndex = 0;

    if (String::findSubStr(%ip, "IPX") != -1 || String::findSubStr(%ip, "LOOPBACK") != -1)
    return; //don't deal with IPX or Loopbacks

    %formattedIP ="";

    while (%word <= %numWords && %charIndex <= %stringSize)
    {
    %char = String::getSubStr(%ip, %charIndex, 1);

    if(String::iCompare(%char, ".") == 0 || String::iCompare(%char, ":") == 0)
    %word++;
    %charIndex++;
    %formattedIP = %formattedIP @ %char;
    }

    if (%fillEmptySlots)
    for (%append = 0; %append <= 4 - %word; %append++)
    {
    %formattedIP = %formattedIP @ "xxx";
    if (%append < (4 - %word))
    %formattedIP = %formattedIP @ ".";
    }
    return %formattedIP;
    }

    function onPermaBanList(%clientId)
    {
    %match = false;

    %ip = Client::getTransportAddress(%clientId);

    for (%index = 1; %index <= $IPBanCount; %index++)
    {
    %loggedIP = getWord($IPBan[%index], 0);
    if (%loggedIP != "" && (!String::nCompare(%ip, %loggedIP, getLength(%loggedIP))))
    {
    echo("$IPBan" @ %index @ " causes this player to be banned.");
    %match = true;
    }
    }
    return %match;
    }




    function kickBanned(%cl)
    {
    logEntry(-2, "automatically re-banned", %cl, "!");
    echo("AUTOBOOT: " @ Client::getName(%cl) @ " has been previously permabanned and is being dropped.");
    %ip = Client::getTransportAddress(%cl);
    BanList::add(%ip, $zadmin::pref::time::ban);
    Net::Kick(%cl, $zadmin::pref::msg::permanentban);
    }

    function resetNumBanEntries()
    {
    deleteVariables("$IPBan*");
    //   for (%i = 0; %i < 1000; %i++)
    //      $IPBan[%i] = "";

    exec($zAdminBanLogFile);

    for (%i = 0; %i < 1000; %i++)
    {
    if ($IPBan[%i] != "")
    $IPBanCount = %i;
    }

    //   %entryNum = 0;
    //   while ($IPBan[%entryNum++] != "")
    //      $IPBanCount = %entryNum;
    }

    function BANEXCLUSIONS()
    {
    //dummy for text editor.
    }

    function BanExclusions::refresh()
    {
    %i = 0;
    while($exclusionList[%i, 0])
    {
    $exclusionList[%i, 0] = "";
    $exclusionList[%i, 1] = "";
    $exclusionList[%i, 2] = "";
    $exclusionList[%i, 3] = "";
    $exclusionList[%i, 4] = "";
    $exclusionList[%i, 5] = "";
    %i++;
    }

    $Exclusions = 0;
    exec($zAdminBanExclusionsFile);
    }

    function BanExclusions::add(%ip, %smurf1, %smurf2, %smurf3, %smurf4, %smurf5)
    {
    $Exclusions++;
    $exclusionList[$Exclusions, 0] = %ip;
    $exclusionList[$Exclusions, 1] = %smurf1;
    $exclusionList[$Exclusions, 2] = %smurf2;
    $exclusionList[$Exclusions, 3] = %smurf3;
    $exclusionList[$Exclusions, 4] = %smurf4;
    $exclusionList[$Exclusions, 5] = %smurf5;
    }

    function BanExclusions::isMember(%cl)
    {
    %loginIP = Client::getTransportAddress(%cl);

    for (%i = 1; %i <= $Exclusions; %i++)
    {
    %excludedIP   = $ExclusionList[%i, 0];
    echo("...comparing " @ %excludedIP @ " with " @ %loginIP);
    if (%excludedIP != "" && (!String::nCompare(%loginIP, %excludedIP, getLength(%excludedIP))))
    {
    %smurfIndex = 0;
    %loginName = Client::getName(%cl);
    while($ExclusionList[%i, %smurfIndex++] != "")
    {
    %smurf = $ExclusionList[%i, %smurfIndex];
    echo("...comparing " @ %smurf @ " with " @ %loginName);
    if(!String::nCompare(%loginName, %smurf, getLength(%loginName)))
    {
    echo("matches an exclusion list entry - he's in!");
    return true;
    }
    }
    }
    }
    echo("compared against all exclusion entries - no match.  Bye bye!");
    return false;
    }

    function LP(%requester)
    {
    if(%requester)
    Client::sendMessage(%requester, $MSGTypeCommand, "________________________________________________________________________");
    else
    echo("________________________________________________________");


    for (%i = 0; %i < getNumClients(); %i++)
    {
    %cl = getClientByIndex(%i);
    if (%cl.adminLevel < 1)
    {
    %admin = "##";
    %smurf = "";
    }
    else
    {
    %admin = String::getSubStr($accessLevel::[%cl.adminLevel], 0, 1) @ "A";
    %smurf = "/" @ %cl.registeredName;
    }

    %clId = format(%cl, 6);
    %admin = format(%admin, 4);
    %score = format("Score: " @ %cl.score, 12);
    %tks = format("TKs: " @ %cl.TKs, 9);
    %ip = format(parseIP(%cl, 4, 18, false), 19);
    %name = Client::getName(%cl) @ %smurf;

    if( %requester)
    {
    %clInfo = %clId @ %admin @ %tks @ %score @ %ip @ %name;
    Client::sendMessage(%requester, $MSGTypeCommand, %clInfo);
    }
    else
    {
    %clInfo = %admin @ %tks @ %score @ %ip @ %name;
    echo(%clInfo);
    }
    }
    if(%requester)
    Client::sendMessage(%requester, $MSGTypeCommand, "________________________________________________________________________");
    else
    echo("________________________________________________________");
    }

    zAdminInit();
    resetNumBanEntries();
    banExclusions::refresh();