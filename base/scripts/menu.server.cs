    function displayMenuServerToggles(%cl) {
        %rec = %cl.selClient;
        %recName = Client::getName(%rec);

        buildNewMenu("Server Options", "serverToggleMenu", %cl);

        addLine("Pause game", "cpause", (%cl.canPause && !$freezedata::active), %cl);
        addLine("Resume game", "cresume", (%cl.canPause && $freezedata::active), %cl);

        addLine("Enable Anti-Rape", "ear", !$zadmin::pref::antirape::enabled && %cl.canAntiRape, %cl);
        addLine("Disable Anti-Rape", "dar", $zadmin::pref::antirape::enabled && %cl.canAntiRape, %cl);

        addLine("Enable No-Repair", "enr", !$zadmin::pref::antirape::norepair && %cl.canAntiRepair, %cl);
        addLine("Disable No-Repair", "dnr", $zadmin::pref::antirape::norepair && %cl.canAntiRepair, %cl);

        addLine("Enable Pickup Mode", "epm", !$zadmin::pref::pickup::enabled && %cl.canPickup, %cl);
        addLine("Disable Pickup Mode", "dpm", $zadmin::pref::pickup::enabled && %cl.canPickup, %cl);
    }

    function processMenuServerToggleMenu(%cl, %sel) {
        if (%sel == "ehs" && %cl.canDisableHitSounds)
            AActionSetHitSounds(%cl, true);
        else if (%sel == "dhs" && %cl.canDisableHitSounds)
            AActionSetHitSounds(%cl, false);
        else if(%sel == "cpause" && %cl.canPause) {
            AActionPause(%cl);
            return;
        }
        else if(%sel == "cresume" && %cl.canPause) {
            AActionResume(%cl);
            return;
        }
        else if ((%sel == "elh") && %cl.canEnableLasthope) {
            Lasthope::EnableLasthope(%cl);
            return;
        }
        else if ((%sel == "ear") && %cl.canAntiRape) {
            $zadmin::pref::antirape::enabled = true;
        }
        else if ((%sel == "dar") && %cl.canAntiRape) {
            $zadmin::pref::antirape::enabled = false;
        }
        else if ((%sel == "enr") && %cl.canAntiRepair) {
            $zadmin::pref::antirape::norepair = true;
        }
        else if ((%sel == "dnr") && %cl.canAntiRepair) {
            $zadmin::pref::antirape::norepair = false;
        }
        else if ((%sel == "epm") && %cl.canPickup) {
            $zadmin::pref::pickup::enabled = true;
            $Server::Password = "pickup";
            OverflowCycle(getNumClients());

            messageAll(1, Client::getName(%cl) @ " ENABLED Pickup Mode! Server Password='pickup'.~wmine_act.wav");

            if($zadmin::pref::log::Pickups) logEntry(%cl, "enabled PICKUP mode", "");
        }
        else if ((%sel == "dpm") && %cl.canPickup) {

            $zadmin::pref::pickup::enabled = false;
            OverflowCycle(getNumClients());

            messageAll(1, Client::getName(%cl) @ " DISABLED Pickup Mode.~wmine_act.wav");

            if($zadmin::pref::log::Pickups) {
                logEntry(%cl, "disabled PICKUP mode", "");
            }

        }

        displayMenuServerToggles(%cl);
    }

    function AActionSetHitSounds(%clientId, %mode) {
        $HitSounds::enabled = %mode;

        if (%mode)
            messageAll(1, "Hit sounds enabled by " @ Client::getName(%clientId) @ ".~wmine_act.wav");
        else
            messageAll(1, "Hit sounds disabled by " @ Client::getName(%clientId) @ ".~wmine_act.wav");

        return;
    }

    function AActionPause(%clientId) {
        freeze::start(%clientId);
    }

    function AActionResume(%clientId) {
        freeze::stop(%clientId);
    }

//echo("Executed Server Menu Options");