/////////////////////////////////////////////////////////////////////////////////////////////////////////
// Krayvok - 2019
// www.Krayvok.com/
// Observer Stream Cam Version 1.0
// Written for pickup game streaming.
/////////////////////////////////////////////////////////////////////////////////////////////////////////
// To initiate this place
// Krayvok::setObserversStream();
// into your base/scripts/objectives.cs file. (may be in scripts.vol)
/////////////////////////////////////////////////////////////////////////////////////////////////////////

    Attachment::AddAfter("displayMenuPlayerMenu", "Krayvok::displayMenuPlayerMenu");
    Attachment::AddAfter("processMenuPlayerOptionsMenu", "Krayvok::processMenuPlayerOptionsMenu");

    function Krayvok::displayMenuPlayerMenu(%cl) {
        addLine("Enable Streamer Cam", "enablesc", !%cl.streamCamDisabled, %cl);
        addLine("Disable Streamer Cam", "disablesc", %cl.streamCamDisabled, %cl);
    }

    function Krayvok::processMenuPlayerOptionsMenu(%cl,%sel) {
        if (%sel == "enablesc") {
            // Default to false; then it auto trues.
            %cl.streamCamDisabled = false;
            %cl.streamCamDisabled = true;
        }
        else if (%sel == "disablesc") {
            %cl.streamCamDisabled = false;
        }
    }

    function Krayvok::setObserversStream() {
        for (%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
            if (Krayvok::checkObserver(%cl)) {
                if (Krayvok::streamCamEnabled(%cl)) {
                        %highscore = Krayvok::getHighScorePlayer();
                        Krayvok::watchPlayer(%cl, %highscore);
                        Client::sendMessage(%cl, 1, "You are watching with Stream Cam! www.PlayT1.com/");
                }
                echo("Stream Cam Running");
            }
        }
        schedule("Krayvok::setObserversStream();", 15);
    }

    function Krayvok::getHighScorePlayer() {
        %topclient = -1;
        %topscore = 0;
        for (%cl=Client::getFirst(); %cl != -1; %cl=Client::getNext(%cl)) {
            if (!Krayvok::checkObserver(%cl)) {
                if (%cl.score > %topscore) {
                    %topclient = %cl;
                    %topscore = %cl.score;
                }
            }
        }
        return %topclient;
    }

    function Krayvok::watchPlayer(%clientId, %target) {
        %clientId.observerMode = "observerFirst";
        %clientId.observerTarget = %target;
        Observer::setOrbitObject(%clientId, %target, -1, -1, -1); // first
        bottomprint(%clientId, "<jc>First Person Observing " @ Client::getName(%target), 5);
        Client::sendMessage(%cl, 1, "You are auto watching with Stream Cam! www.PlayT1.com/~wmine_act.wav");
    }

    function Krayvok::streamCamEnabled(%observer) {
        if (%observer.streamCamDisabled) {
            return true;
        }
        else {
            return false;
        }
    }

    function Krayvok::checkObserver(%clientId) {
        if (%clientId.observerMode == "observerOrbit" || %clientId.observerMode == "observerFly" || %clientId.observerMode == "observerFirst") {
            return true;
        }
        else {
            return false;
        }
    }