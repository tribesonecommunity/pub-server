Attachment::AddBefore("Player::onDamage", "Stats::Player::onDamage");

function Stats::Player::onDamage(%this, %type, %value, %pos, %vec, %mom, %vertPos, %quadrant, %object)
{
  //damage from impact etc.
  if(%type == 0)
    return;

  %shooterClient = %object;
  %damagedClient = Player::getClient(%this);
  %damagedClientTeam = Client::getTeam(%damagedClient);
  %shooterClientTeam = Client::getTeam(%shooterClient);

  // Team damage not on self
  if (%shooterClientTeam == %damagedClientTeam) {
    if (%shooterClient != %damagedClient) {
      %value = -1 * %value;
      //%shooterClient.activity["Damage"] -= %value;
    }
    else
      %value = 0;
  }
  //else { // Not team damage
    //%shooterClient.activity["Damage"] += %value;
  //}

  Stats::Client::adjustScoreMultiplyNoHalt(%shooterClient, "Damage", %value);
}
