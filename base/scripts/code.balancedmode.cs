Attachment::AddAfter("displayMenuServerToggles", "BalancedMode::displayMenuServerToggles");
Attachment::AddAfter("processMenuServerToggleMenu", "BalancedMode::processMenuServerToggleMenu");

Attachment::AddBefore("Player::onDamage", "BalancedMode::onDamage");
Attachment::AddAfter("Client::onFlagCap", "BalancedMode::CapoutWarning");

Attachment::AddBefore("Overtime::SetOvertime", "BalancedMode::SetOvertime");
Attachment::AddBefore("ObjectiveMission::CheckScoreLimit", "BalancedMode::ObjectiveMission::CheckScoreLimit");

// Default balance mode to...
$Server::BalancedMode = false;

function BalancedMode::SetOvertime() {
  if ($Server::BalancedMode)
    return "halt";
}

function BalancedMode::ObjectiveMission::CheckScoreLimit() {
    if ($Server::BalancedMode) {

        for (%i = 0; %i < getNumTeams(); %i++) {
        if ($teamScore[%i] >= $teamScoreLimit)
            %done = true;
            %totalScore += $teamScore[%i];
        }

        // If capout is 8, then first half ends at 7 and second half ends at 14
        // $Server::Half goes from 1 to 2 (change this from 0 to 1?)
        %scoreLimit = $teamScoreLimit * $Server::Half - $Server::Half;
        if (%totalScore >= %scoreLimit) {

            %done = true;
        }

        if(%done) {
            ObjectiveMission::missionComplete();
            return "halt";
        }
    }
}

function BalancedMode::displayMenuServerToggles(%clientId)
{
  addLine("Enable Balanced Mode", "ebtm", (%clientId.canChangeGameMode && $Server::TourneyMode && !$Server::BalancedMode), %clientId);
  addLine("Disable Balanced Mode", "dbtm", (%clientId.canChangeGameMode && $Server::TourneyMode && $Server::BalancedMode), %clientId);
}

function BalancedMode::processMenuServerToggleMenu(%clientId, %sel)
{
  if (%sel == "ebtm" && %clientId.canChangeGameMode)
    AActionsetModeBalance(%clientId, true);
  else if (%sel == "dbtm" && %clientId.canChangeGameMode)
    AActionsetModeBalance(%clientId, false);
  displayMenuServerToggles(%clientId);
}

function AActionsetModeBalance(%clientId, %mode)
{
    $Server::BalancedMode = %mode;
    $Server::Half = 1;
    if (%mode)
        messageAll(1, "Balanced Tournament Mode enabled by " @ Client::getName(%clientId) @ ".~wmine_act.wav");
    else
        messageAll(1, "Balanced Tournament Mode disabled by " @ Client::getName(%clientId) @ ".~wmine_act.wav");
    return;
}

function BalancedMode::onDamage(%this, %type, %value, %pos, %vec, %mom, %vertPos, %quadrant, %object)
{
    // Make players invulnerable during halftime for fun
    if ($Server::Halftime) {
        return "halt";
    }
    // If the match is not started do not allow damage to take place.
    if (!$matchStarted && !$countdownStarted) {
        return "halt";
    }
}

function BalancedMode::CapoutWarning(%team, %cl)
{
  if (!$Server::BalancedMode)
    return;

  %combinedScores = 0;
  for (%i = 0; %i < getNumTeams(); %i++)
  {
    %combinedScores += $teamScore[%i];
    if ($teamScore[%i] == $teamScoreLimit)
      return;
  }

  %warningThreshold = 3;

  // If individual team capout is at 8 then each half caps out at 7 and 14.
  %scoreLimit = $teamScoreLimit * $Server::Half - $Server::Half;
  %difference = %scoreLimit - %combinedScores;

  if (%difference <= %warningThreshold && %difference > 0)
  {
    %caps = " caps";
    if (%difference == 1)
      %caps = " cap";
    if ($Server::Half == 1)
    {
      MessageAll(1, "Half ends in " @ %difference @ %caps @ ".~wmine_act.wav");
    }
    else if ($Server::Half == 2)
    {
      MessageAll(1, "Match ends in " @ %difference @ %caps @ ".~wmine_act.wav");
    }
  }
}
function Krayvok::resetAssets() {

    for (%objectItem = 8000; %objectItem < 9000; %objectItem++) {

        %objectType = getObjectType(%objectItem);

        if (%objectType != "False") {
            %name = GameBase::getDataName(%objectItem);

            if (%objectType == "StaticShape" || %objectType == "Turret" || %objectType == "Sensor" || %objectType == "Mine") {

                // Restore Assets
                if (%name == "IndoorTurret" || %name == "PlasmaTurret" || %name == "ELFTurret" || %name == "InventoryStation" || %name == "PortGenerator" || %name == "Generator" || %name == "AmmoStation" || %name == "MediumPulseSensor") {
                    if (GameBase::getDamageLevel(%objectItem) == 1) {
                        GameBase::setDamageLevel(%objectItem, 0);
                    }
                    else {
                        // Destroy it anwyways.
                        GameBase::setDamageLevel(%objectItem, 1);

                        // Restore it again.
                        GameBase::setDamageLevel(%objectItem, 0);
                    }
                }

                // Destroy Depoyables.
                if (%name == "DeployableTurret" || %name == "CameraTurret" || %name == "DeployableAmmoStation" || %name == "DeployableInvStation" || %name == "DefaultBeacon" || %name == "DeployablePulseSensor" || %name == "DeployableSensorJammer" || %name == "DeployableMotionSensor" || %name == "AntipersonelMine") {
                    GameBase::setDamageLevel(%objectItem, 1);
                }

            }
        }
    }
}