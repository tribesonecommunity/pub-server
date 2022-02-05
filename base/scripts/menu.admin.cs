    function displayMenuAdminMenu(%cl) {

        %rec = %cl.selClient;
        %recName = Client::getName(%rec);
        %tModeWaiting = $Server::TourneyMode && (!$CountdownStarted && !$matchStarted);

        buildNewMenu("Options", "adminmenu", %cl);
            addLine("Change Teams/Observe", "changeteams", !$loadingMission, %cl);
            addLine("Change mission", "changeMission", %cl.canChangeMission, %cl);
            addLine("Disable team damage", "dtd", (%cl.canSwitchTeamDamage && $Server::TeamDamageScale == 1.0), %cl);
            addLine("Enable team damage", "etd", (%cl.canSwitchTeamDamage && !$Server::TeamDamageScale == 1.0), %cl);
            addLine("Start the match", "smatch", (%cl.canForceMatchStart && %tModeWaiting), %cl);
            addLine("Change to Tournament mode", "ctourney", (%cl.canChangeGameMode && !$Server::TourneyMode), %cl);
            addLine("Set Time Limit", "ctimelimit", %cl.canChangeTimeLimit, %cl);
            //addLine("Reset Server Defaults", "reset", %cl.canResetServer, %cl); //wtf uses this
            //addLine("Announce Server Takeover", "takeovermes", %cl.canAnnounceTakeover, %cl);
            addLine("Server toggles...", "serverToggles", true, %cl);
            addLine("Player options...", "playerOptions", true, %cl);
            addLine("Vote options...", "voteOptions", true, %cl);
    }

    function processMenuAdminMenu(%cl, %selection) {

        if(%selection == "changeteams") {
            // unique to menu.admin.cs
            displayMenuChangeTeamsMenu(%cl);
            return;
        }
        else if(%selection == "ctourney") {
            AActionsetModeTourney(%cl);
        }
        else if(%selection == "smatch") {
            AActionstartMatch(%cl);
        }
        else if(%selection == "changeMission") {
            //for admins initiating mission change votes.
            %cl.madeVote = "";

            displayMenuChangeMissionType(%cl, 0);
            return;
        }
        else if(%selection == "ctimelimit") {
            // unique to menu.admin.cs
            displayMenuChangeTimeLimit(%cl);
            return;
        }
        else if(%selection == "reset") {
            // unique to menu.admin.cs
            displayMenuResetServerDefaults(%cl);
            return;
        }
        else if(%selection == "takeovermes") {
            // unique to menu.admin.cs
            displayMenuAnnounceServerTakeover(%cl);
            return;
        }
        else if(%selection == "etd") {
            AActionsetTeamDamageEnable(%cl, true);
        }
        else if(%selection == "dtd") {
            AActionsetTeamDamageEnable(%cl, false);
        }
        else if(%selection == "voteOptions") {
            displayMenuVoteMenu(%cl);
            return;
        }
        else if(%selection == "playerOptions") {
            displayMenuPlayerMenu(%cl);
            return;
        }
        else if(%selection == "serverToggles") {
            displayMenuServerToggles(%cl);
            return;
        }

        Game::menuRequest(%cl);

    }

    function processMenuChangeTeamsMenu(%clientId, %team, %adminClient)
    {
    if ($loadMission)
    return;

    if ($freezedata::active)
    return;

    checkPlayerCash(%clientId);

    if ( %team != -1 && %team == Client::getTeam(%clientId) || %team >= getNumTeams( ) )
    return;

    %clientTeam = Client::getTeam(%clientId);

    if(%clientId.observerMode == "justJoined")
    {
    %clientId.observerMode = "";
    centerprint(%clientId, "");
    }

    if((!$matchStarted || !$Server::TourneyMode || %adminClient) && %team == -2)
    {
    if(Observer::enterObserverMode(%clientId))
    {
    %clientId.notready = "";

    if(%adminClient == "")
    messageAll(0, Client::getName(%clientId) @ " became an observer.");
    else
    messageAll(0, Client::getName(%clientId) @ " was forced into observer mode by " @ Client::getName(%adminClient) @ ".");

    Game::resetScores(%clientId);
    Game::refreshClientScore(%clientId);
    ObjectiveMission::refreshTeamScores();
    }
    return;
    }

    //automatic team
    if (%team == -1)
    {
    Game::assignClientTeam(%clientId);
    %team = Client::getTeam(%clientId);
    if (%team == %clientTeam)
    return;
    }

    %player = Client::getOwnedObject(%clientId);

    if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
    {
    playNextAnim(%clientId);
    Player::kill(%clientId);
    }
    %clientId.observerMode = "";

    if(%adminClient == "")
    messageAll(0, Client::getName(%clientId) @ " changed teams.");
    else
    messageAll(0, Client::getName(%clientId) @ " was teamchanged by " @ Client::getName(%adminClient) @ ".");

    //echo("setting team to team #" @ %team);
    GameBase::setTeam(%clientId, %team);
    %clientId.teamEnergy = 0;
    Client::clearItemShopping(%clientId);
    if(Client::getGuiMode(%clientId) != 1)
    Client::setGuiMode(%clientId,1);
    Client::setControlObject(%clientId, -1);

//
		if (%clientId.customSkinDisabled == "")
			%clientId.customSkinDisabled = true;        		
		
		if(!%clientId.customSkinDisabled)
			Client::setSkin(%clientId, $Client::info[%clientId, 0]);
		else
			Client::setSkin(%clientId, $Server::teamSkin[%team]);   
//

    Game::playerSpawn(%clientId, false);
    %team = Client::getTeam(%clientId);
    if($TeamEnergy[%team] != "Infinite")
    $TeamEnergy[%team] += $InitialPlayerEnergy;
    if($Server::TourneyMode && !$CountdownStarted)
    {
    Game::DisplayReadyMessage(%clientId);
    // bottomprint(%clientId, "<f1><jc>Press FIRE when ready.", 0);
    %clientId.notready = true;
    }

    ObjectiveMission::refreshTeamScores();
    }

   function displayMenuChangeTimeLimit(%cl)
       {
       buildNewMenu("Change Time Limit", "changeTimeLimit", %cl);

       //addLine("1 minute", 1, true, %cl);
       //addLine("7 minutes", 7, true, %cl);
       addLine("5 minutes", 5, true, %cl);
       addLine("12 minutes", 12, true, %cl);
       addLine("15 minutes", 15, true, %cl);
       addLine("20 minutes", 20, true, %cl);
       addLine("25 minutes", 25, true, %cl);
       addLine("30 minutes", 30, true, %cl);
       addLine("60 minutes", 60, true, %cl);
       addLine("No time limit", 0, true, %cl);
       }

       function processMenuChangeTimeLimit(%cl, %opt)
       {
       remoteSetTimeLimit(%cl, %opt);
   }

  function displayMenuResetServerDefaults(%cl)
      {
      buildNewMenu("Reset Server Defaults", "resetServerDefaults", %cl);

      addLine("Reset Server Defaults", "yes", true, %cl);
      addLine("Cancel", "cancel", true, %cl);
      }

      function processMenuResetServerDefaults(%cl, %opt)
      {
      if(%opt == "yes")
      {
      //if($logServerResets) logEntry(%cl, "reset server defaults", "");
      messageAll(0, Client::getName(%cl) @ " reset the server to default settings.");
      Server::refreshData();
      }

      Game::menuRequest(%cl);
      }

   function displayMenuAnnounceServerTakeover(%cl)
       {
       buildNewMenu("Announce Server Takeover", "announceServerTakeover", %cl);

       addLine("Friendly Message", "friendly", true, %cl);
       addLine("Firm Message", "firm", true, %cl);
       addLine("Cancel", "cancel", true, %cl);
       }

       function processMenuAnnounceServerTakeover(%clientId, %opt)
       {
       %mes = getWord(%opt, 0);
       if (%mes == "friendly")
       {
       CenterPrintAll("<jc>" @ Client::getName(%clientId) @ ": " @ $zadmin::pref::msg::friendlytakeover);
       if($zadmin::pref::log::Takeovers) logEntry(%clientId, "announced a friendly takeover message", "");
       }
       if (%mes == "firm")
       {
       CenterPrintAll("<jc>" @ Client::getName(%clientId) @ ": " @ $zadmin::pref::msg::firmtakeover);
       if($zadmin::pref::log::Takeovers) logEntry(%clientId, "announced a firm takeover message", "");
       }

       Game::menuRequest(%cl);
       }
    //echo("Executed Admin Menu Options");